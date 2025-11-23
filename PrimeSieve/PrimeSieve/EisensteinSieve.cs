using static System.Math;

namespace PrimeGeometry
{
    public static class EisensteinSieve
    {
        /// <summary>
        /// Grobes Sieb in Z[w]:
        /// Gibt true zurück, wenn z = d + e*w überhaupt noch Kandidat
        /// für eine Eisenstein-Primzahl mit Norm p ≡ 1 (mod 12) sein kann.
        /// Berechnet keine Norm und macht keinen Primtest.
        /// </summary>
        public static bool MayBeZPrime(long d, long e)
        {
            // 0. Trivialer Ausschluss
            if (d == 0 && e == 0)
                return false;

            long ad = Abs(d);
            long ae = Abs(e);

            // 1. gcd-Filter: wenn gcd(d, e) > 1, ist N(d,e) = g² * N(d',e') nie prim
            long g = Gcd(ad, ae);
            if (g > 1)
                return false;

            // Wenn der Kandidat bis hier überlebt, ist er wahrscheinlich prim.
            return true;
        }

        //Simpler Gcd-Test
        private static long Gcd(long a, long b)
        {
            while (b != 0)
            {
                long t = a % b;
                a = b;
                b = t;
            }
            return a < 0 ? -a : a;
        }

        private static int Mod(long x, int m)
        {
            int r = (int)(x % m);
            return r < 0 ? r + m : r;
        }
    }
}
