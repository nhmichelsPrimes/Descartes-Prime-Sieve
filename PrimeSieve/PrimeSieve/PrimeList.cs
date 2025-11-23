using System;
using System.Collections.Generic;

namespace PrimeGeometry
{
    /// <summary>
    /// Erzeugt Primzahlen bis zu einer Obergrenze mittels Siebverfahren
    /// mit 6k±1-Optimierung.
    /// </summary>
    public static class PrimeList
    {
        public static IEnumerable<long> PrimesUpTo(long n)
        {
            if (n >= 2)
                yield return 2;
            if (n >= 3)
                yield return 3;

            var sieve = new BitSet((int)(n + 1));
            int limit = (int)Math.Sqrt(n);

            // Sieb über Kandidaten 6k ± 1
            for (int p = 5, step = 2; p <= limit; p += step, step = 6 - step)
            {
                if (!sieve[p])
                {
                    for (long q = (long)p * p; q <= n; q += 2L * p)
                        sieve[(int)q] = true;
                }
            }

            for (int p = 5, step = 2; p <= n; p += step, step = 6 - step)
            {
                if (!sieve[p])
                    yield return p;
            }
        }

        public static bool IsPrime(int n)
        {
            if (n < 2) return false;
            if (n % 2 == 0) return n == 2;
            if (n % 3 == 0) return n == 3;

            int limit = (int)Math.Sqrt(n);
            for (int i = 5; i <= limit; i += 6)
            {
                if (n % i == 0 || n % (i + 2) == 0) return false;
            }
            return true;
        }

        public static bool IsPrime(long n)
        {
            if (n < 2) return false;
            if (n % 2 == 0) return n == 2;
            if (n % 3 == 0) return n == 3;

            long limit = (long)Math.Sqrt(n);
            for (long i = 5; i <= limit; i += 6)
            {
                if (n % i == 0 || n % (i + 2) == 0) return false;
            }
            return true;
        }

        /// <summary>
        /// Einfache Bitset-Implementierung für das Sieb.
        /// </summary>
        private sealed class BitSet
        {
            private readonly uint[] data;

            public BitSet(int n)
            {
                data = new uint[(n + 31) >> 5];
            }

            public bool this[int i]
            {
                get => (data[i >> 5] & (1u << (i & 31))) != 0;
                set
                {
                    if (value)
                        data[i >> 5] |= 1u << (i & 31);
                    else
                        data[i >> 5] &= ~(1u << (i & 31));
                }
            }
        }
    }
}
