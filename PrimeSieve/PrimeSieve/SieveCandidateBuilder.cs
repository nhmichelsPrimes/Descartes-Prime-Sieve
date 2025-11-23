using PrimeSieve;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace PrimeGeometry
{
    /// <summary>
    /// Erzeugt für ein gegebenes p1 alle "Sieb-Kandidaten":
    /// 
    /// Zahlenpaare (p1, p2) mit
    ///   p2 = p1 + 4n,
    ///   z  = p1² - 12n² ≥ 0,
    ///   z = d² - d e + e² (Eisenstein-Darstellung),
    ///   z = a² + b²       (Gauss-Darstellung),
    ///   a ≠ 0, b ≠ 0,
    ///   EisensteinSieve.MayBeZPrime(d,e) = true.
    /// 
    /// Diese Kandidaten werden anschließend im normalen Zahlenraum auf Primheit getestet.
    /// </summary>
    public static class SieveCandidateBuilder
    {
        /// <summary>
        /// Sucht alle Kandidaten (p1,p2,z,a,b,d,e) für ein gegebenes p1.
        /// </summary>
        public static IReadOnlyList<SieveCandidate> FindCandidatesForP1(long p1)
        {
            var candidates = new List<SieveCandidate>();

            // Obergrenze für n
            // H = (2 * p1) / 3°0,5, nMax = H / 2  =>  nMax = p1 / 3^0,5 (ganzzahlig)
            long nMax = (long)(p1 / Math.Sqrt(3));
            if (nMax <= 0)
                return candidates;

            // Restklasse von p1 modulo 4 (p1 & 3 ist äquivalent zu p1 (mod 4))
            int residueP1Mod4 = (int)(p1 & 3);

            for (long n = 1; n <= nMax; n++)
            {
                long p2 = p1 + 4 * n;

                // z = p1² - 12 n², mit Overflow-Schutz
                long z;
                try
                {
                    checked
                    {
                        z = p1 * p1 - 12L * n * n;
                    }
                }
                catch (OverflowException)
                {
                    // Wenn hier ein Overflow auftritt, werden größere n nur noch schlimmer.
                    break;
                }

                if (z < 0)
                    break;

                // Untere und obere Grenze für k1 /ergibt sich aus der Elipsengleichung 
                double root = Math.Sqrt(z);
                long k1Lo = (long)Math.Ceiling((p1 - 2.0 * root) / 3.0);
                //k1 = p1 produziert nur degenerierte Fälle
                long k1Hi = p1 - 1;
                if (k1Lo > k1Hi)
                    continue;

                // Set, um identische Descartes-Tripel (bis auf Permutation) auszusortieren
                var seenTriples = new HashSet<(long, long, long)>();

                // Drei Varianten: jede Koordinate bekommt einmal die Restklasse von p1, die anderen 0.
                SearchWithResidues(
                    p1, n, p2, z,
                    k1Lo, k1Hi,
                    residueP1Mod4, 0, 0,
                    seenTriples, candidates);

               /*/ SearchWithResidues(
                    p1, n, p2, z,
                    k1Lo, k1Hi,
                    0, residueP1Mod4, 0,
                    seenTriples, candidates);

                SearchWithResidues(
                    p1, n, p2, z,
                    k1Lo, k1Hi,
                    0, 0, residueP1Mod4,
                    seenTriples, candidates);*/
            }

            return candidates;
        }

        /// <summary>
        /// Sucht über den Bereich [k1Lo..k1Hi] nach ganzzahligen (k1,k2,k3),
        /// passend zu den vorgegebenen Restklassen (r1,r2,r3),
        /// und erzeugt daraus Sieve-Kandidaten.
        /// 
        /// Heuristik:
        ///   - Pro Residuenkonfiguration (r1,r2,r3) wird nur dann übernommen,
        ///     wenn genau ein Kandidat gefunden wurde.
        /// </summary>
        private static void SearchWithResidues(
            long p1,
            long n,
            long p2,
            long z,
            long k1Lo,
            long k1Hi,
            int r1,
            int r2,
            int r3,
            HashSet<(long, long, long)> seenTriples,
            List<SieveCandidate> globalCandidates)
        {
            // Startwert für k1 so wählen, dass k1 ≡ r1 (mod 4)
            long k1Start = AlignUpToResidue(k1Lo, 4, r1);
            if (k1Start > k1Hi)
                return;

            var localCandidates = new List<SieveCandidate>();

            for (long k1 = k1Start; k1 <= k1Hi; k1 += 4)
            {
                // quadratische Gleichung in k2:
                //
                // delta = B² - 4C, mit
                //   B = k1 - p1,
                //   C = k1² - p1*k1 + 4n²
                long B = k1 - p1;
                long C = k1 * k1 - p1 * k1 + 4 * n * n;
                long delta = B * B - 4 * C;
                if (delta < 0)
                    continue;

                ulong d = (ulong)delta;
                ulong sqrtDelta = IntegerSqrt64(d);
                if (sqrtDelta * sqrtDelta != d)
                    continue;

                // Beide Lösungen der quadratischen Gleichung testen
                TryCreateCandidate(
                    p1, n, p2, z,
                    k1,
                    -B + (long)sqrtDelta,
                    r2, r3,
                    seenTriples,
                    localCandidates);

                if (sqrtDelta != 0)
                {
                    TryCreateCandidate(
                        p1, n, p2, z,
                        k1,
                        -B - (long)sqrtDelta,
                        r2, r3,
                        seenTriples,
                        localCandidates);
                }
            }

            // Filterkriterium: Mehr als ein Tripel
            // Nur wenn genau eine Lösung für diese Restklassenkonfiguration existiert,
            // übernehmen wir sie in die globale Kandidatenliste. 
            if (localCandidates.Count == 1)
            {
                globalCandidates.AddRange(localCandidates);
            }
        }

        /// <summary>
        /// Verarbeitet eine konkrete Lösung der quadratischen Gleichung:
        ///   num = -B ± sqrt(Δ),
        /// daraus k2, k3, und dann (d,e) und (a,b).
        /// 
        /// Bedingungen:
        ///   - k2 ganzzahlig,
        ///   - (k1,k2,k3) noch nicht als Tripel (bis auf Permutation) gesehen,
        ///   - z = d² - d e + e²,
        ///   - z > 0,
        ///   - EisensteinSieve.MayBeZPrime(d,e) = true,
        ///   - z = a² + b² mit a,b ≠ 0.
        /// </summary>
        private static void TryCreateCandidate(
    long p1,
    long n,
    long p2,
    long z,
    long k1,
    long num,
    int r2,
    int r3,
    HashSet<(long, long, long)> seenTriples,
    List<SieveCandidate> localCandidates)
        {
            if ((num & 1) != 0)
                return;

            long k2 = num / 2;
            long k3 = p1 - k1 - k2;

            var sortedTriple = Sort3(k1, k2, k3);
            if (!seenTriples.Add(sortedTriple))
                return;

            long d, e, zEis;
            try
            {
                checked
                {
                    d = k1 - k2;
                    e = k1 - k3;
                    zEis = d * d - d * e + e * e;
                }
            }
            catch (OverflowException)
            {
                //to big integer
                return;
            }


            //GCD-Filter: killt 99% der nicht primes
            if (!EisensteinSieve.MayBeZPrime(d, e))
                return;


            // Wurzelfilter: Der GCD erwischt eigentlich die meisten p^k
            // Weniger als 1 der Zahlen, die hier ankommen, sind tatsächlich
            // nicht prime. Aus Performance-Gründen kann man das auch ausschalten.
            // Das ist (neben der Validierung) der rechenintensivste Schritt.
            if (PerfectSquareSieve.IsPerfectPowerWithBaseAtLeast13((ulong)z)) { return; }


            //Nur zu Testzwecken
            //Alle Zahlen die hier ankommen sind automatisch auch als a^2 + b^2 abbildbar.
            //Man kann darüber auch einen Filter definieren. 
            //var sos = SumOfSquaresSolver.FindSumOfSquaresRepresentations(z);

            /* Alternativer Check auf Quadrate
            foreach (var (aTest, bTest) in sos)
            {
                if (aTest == 0 || bTest == 0)
                    return;
            }
            
            */
            // (aBI, bBI) = sos[0];
            /*
            //#NAN für große Integer
            if (aBI > long.MaxValue || aBI < long.MinValue) 
                return;
           //#NAN for to big interegers
            if (bBI > long.MaxValue || bBI < long.MinValue) 
                return;
            */
            //long a = (long)aBI;
            //long b = (long)bBI;

            //var quad = new Quadruple(a, b, d, e);

            //a und b sind hier 0 gesetzt. Sie sind natürlich nicht 0. 
            var quad = new Quadruple(0, 0, d, e);

            var candidate = new SieveCandidate(p1, p2, n, z, k1, k2, k3, quad);
            localCandidates.Add(candidate);
        }


        // ----------------- Hilfsfunktionen -----------------

        /// <summary>
        /// Sortiert drei Werte aufsteigend und gibt sie als Tuple zurück.
        /// Dient dazu, (k1,k2,k3)-Tripel unabhängig von der Reihenfolge zu vergleichen.
        /// </summary>
        private static (long, long, long) Sort3(long a, long b, long c)
        {
            if (a > b) (a, b) = (b, a);
            if (b > c) (b, c) = (c, b);
            if (a > b) (a, b) = (b, a);
            return (a, b, c);
        }

        /// <summary>
        /// Liefert das kleinste y ≥ x mit y ≡ residue (mod mod).
        /// </summary>
        private static long AlignUpToResidue(long x, int mod, int residue)
        {
            residue = ((residue % mod) + mod) % mod;
            long r = ((x % mod) + mod) % mod;
            long diff = (residue - r + mod) % mod;
            return x + diff;
        }

        /// <summary>
        /// Ganzzahlige Quadratwurzel von x (floor(sqrt(x))) für ulong.
        /// </summary>
        private static ulong IntegerSqrt64(ulong x)
        {
            if (x == 0)
                return 0;

            ulong r = (ulong)Math.Sqrt(x);

            while (r * r > x)
                r--;

            while ((r + 1) * (r + 1) <= x)
                r++;

            return r;
        }
    }
}
