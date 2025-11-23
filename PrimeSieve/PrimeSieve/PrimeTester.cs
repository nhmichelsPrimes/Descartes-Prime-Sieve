using System;

namespace PrimeGeometry
{
    /// <summary>
    /// Einfache Primzahlprüfung im ℤ-Bereich für long.
    /// 
    /// Wird im Sieb verwendet, um p1, p2 und z im normalen Zahlenraum
    /// auf Primheit zu testen. Für die hier üblichen Größenordnungen
    /// (p1 bis ~10^5, z bis ca. 10^10) reicht eine 6k±1-Teilbarkeitstestschleife.
    /// </summary>
    public static class PrimeTester
    {
        /// <summary>
        /// Prüft, ob n im ℤ-Bereich eine Primzahl ist.
        /// </summary>
        public static bool IsPrime(long n)
        {
            if (n <= 1)
                return false;
            if (n <= 3)
                return true;    // 2,3
            if ((n & 1) == 0)
                return false;   // gerade
            if (n % 3 == 0)
                return n == 3;

            // 6k±1-Check bis sqrt(n)
            long limit = (long)Math.Sqrt(n);
            for (long i = 5; i <= limit; i += 6)
            {
                if (n % i == 0)
                    return false;
                if (n % (i + 2) == 0)
                    return false;
            }

            return true;
        }
    }
}
