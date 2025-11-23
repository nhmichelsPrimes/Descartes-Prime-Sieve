using System;
using System.Collections.Generic;

namespace PrimeGeometry
{
    /// <summary>
    /// Einstiegspunkt des Primzahl-Siebs.
    /// 
    /// Pipeline:
    ///   1. Für jede Primzahl p1 ≤ MaxPrime werden Kandidaten (p1,p2,z,a,b,d,e) gesucht.
    ///   2. Für jeden Kandidaten werden p1, p2 und z auf Primheit getestet.
    ///   3. Alle Ergebnisse werden als CSV geschrieben.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Obergrenze für p1 (inklusive).
        /// </summary>
        private const long MaxPrime = 100_000;

        private static void Main(string[] args)
        {
            try
            {
                var allResults = new List<SieveResult>();

                foreach (long p1 in PrimeList.PrimesUpTo(MaxPrime))
                {
                    Console.WriteLine($"p1 = {p1}");

                    // 1. Finde alle Sieve-Kandidaten für dieses p1
                    var candidates = SieveCandidateBuilder.FindCandidatesForP1(p1);
                    Console.WriteLine($"  Kandidaten: {candidates.Count}");

                    // 2. Primtests im normalen Zahlenraum für p1, p2, z
                    // Die Prime-Tests hier dienen nur der Validierung; danach wird nicht gefiltert; 
                    // Man kann das durch noTesting = true ausstellen, da es auch rechentintensiv ist

                    bool noTesting = false;
                    foreach (var candidate in candidates)
                    {
                        bool? isPrimeP1 = candidate.P1 > 0 && noTesting ? null :  PrimeTester.IsPrime(candidate.P1);
                        bool? isPrimeP2 = candidate.P2 > 0 && noTesting ? null : PrimeTester.IsPrime(candidate.P2);
                        bool? isPrimeZ = candidate.Z > 0 &&   noTesting ? null : PrimeTester.IsPrime(candidate.Z);

                        var q = candidate.Quad;

                        var result = new SieveResult(
                            p1: candidate.P1,
                            p2: candidate.P2,
                            z: candidate.Z,
                            n: candidate.N,
                            k1: candidate.K1,
                            k2: candidate.K2,
                            k3: candidate.K3,
                            a: q.A,
                            b: q.B,
                            d: q.D,
                            e: q.E,
                            isPrimeP1: isPrimeP1,
                            isPrimeP2: isPrimeP2,
                            isPrimeZ: isPrimeZ
                        );

                        allResults.Add(result);
                    }
                }

                string outputPath = $"prime_sieve_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                Console.WriteLine($"Schreibe CSV nach '{outputPath}' ...");

                SieveCsvExporter.WriteResultsToCsv(outputPath, allResults);

                Console.WriteLine("Fertig.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler in Main:");
                Console.WriteLine(ex);
            }
        }
    }
}
