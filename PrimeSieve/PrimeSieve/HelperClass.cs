using System;
using System.Collections.Generic;
using System.Numerics;

namespace PrimeGeometry
{
    /// <summary>
    /// Hilfsklasse zur Bestimmung aller Darstellungen
    ///   z = a² + b²
    /// mit a,b ≥ 0.
    /// </summary>
    public static class SumOfSquaresSolver
    {
        /// <summary>
        /// Liefert alle Paare (a,b) mit a² + b² = n, a,b ≥ 0.
        /// Für a ≠ b wird sowohl (a,b) als auch (b,a) zurückgegeben.
        /// </summary>
        public static IReadOnlyList<(long a, long b)> FindSumOfSquaresRepresentations(long n)
        {
            var result = new List<(long a, long b)>();

            if (n < 0)
                return result;

            if (n == 0)
            {
                result.Add((0L, 0L));
                return result;
            }

            long maxA = (long)Math.Sqrt(n); // floor(sqrt(n))

            for (long a = 0; a <= maxA; a++)
            {
                long a2 = a * a;
                long b2 = n - a2;
                if (b2 < 0)
                    break;

                long b = (long)Math.Sqrt(b2);
                if (b * b != b2)
                    continue;

                // a,b ≥ 0
                result.Add((a, b));
                if (a != b)
                    result.Add((b, a));
            }

            return result;
        }

        // ----------------- interne Hilfsfunktionen -----------------

        private static BigInteger IntegerSqrt(BigInteger n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException(nameof(n), "n must be non-negative.");

            if (n == 0 || n == 1)
                return n;

            BigInteger low = 0;
            BigInteger high = n;
            while (high - low > 1)
            {
                BigInteger mid = (low + high) >> 1;
                BigInteger midSq = mid * mid;
                if (midSq == n)
                    return mid;
                if (midSq < n)
                    low = mid;
                else
                    high = mid;
            }

            return low;
        }

        private static bool IsPerfectSquare(BigInteger n, out BigInteger sqrt)
        {
            if (n < 0)
            {
                sqrt = 0;
                return false;
            }

            sqrt = IntegerSqrt(n);
            return sqrt * sqrt == n;
        }
    }
}
