using System;

namespace PrimeGeometry
{
    /// <summary>
    /// Ein einzelner "Sieb-Kandidat":
    ///   p1, p2, n, z,
    ///   Descartes-Tripel (k1,k2,k3)
    ///   und Quadruple (a,b,d,e).
    /// </summary>
    public sealed class SieveCandidate
    {
        public long P1 { get; }
        public long P2 { get; }
        public long N { get; }
        public long Z { get; }

        /// <summary>
        /// Descartes-Tripel k1 + k2 + k3 = p1.
        /// </summary>
        public long K1 { get; }
        public long K2 { get; }
        public long K3 { get; }

        public Quadruple Quad { get; }

        public SieveCandidate(long p1, long p2, long n, long z,
                              long k1, long k2, long k3,
                              Quadruple quad)
        {
            P1 = p1;
            P2 = p2;
            N = n;
            Z = z;

            K1 = k1;
            K2 = k2;
            K3 = k3;

            Quad = quad ?? throw new ArgumentNullException(nameof(quad));
        }
    }

    /// <summary>
    /// Endergebnis des Siebs für einen Kandidaten:
    /// enthält p1, p2, z, n, die Koordinaten (a,b,d,e)
    /// und die Informationen, ob p1, p2 und z im R-Ganzzahlraum prim sind.
    /// </summary>
    public sealed class SieveResult
    {
        public long P1 { get; }
        public long P2 { get; }
        public long Z { get; }
        public long N { get; }

        public long K1 { get; }
        public long K2 { get; }
        public long K3 { get; }

        public long A { get; }
        public long B { get; }
        public long D { get; }
        public long E { get; }

        public bool? IsPrimeP1 { get; }
        public bool? IsPrimeP2 { get; }
        public bool? IsPrimeZ { get; }

        public SieveResult(
            long p1,
            long p2,
            long z,
            long n,
            long k1,
            long k2,
            long k3,
            long a,
            long b,
            long d,
            long e,
            bool? isPrimeP1,
            bool? isPrimeP2,
            bool? isPrimeZ)
        {
            P1 = p1;
            P2 = p2;
            Z = z;
            N = n;

            K1 = k1;
            K2 = k2;
            K3 = k3;

            A = a;
            B = b;
            D = d;
            E = e;

            IsPrimeP1 = isPrimeP1;
            IsPrimeP2 = isPrimeP2;
            IsPrimeZ = isPrimeZ;
        }
    }
}
