using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeSieve
{
    internal class PerfectSquareSieve
    {

        /// <summary>
        /// Prüft, ob n = a^k mit a >= 13 und k >= 2 ist.
        /// Nur als Filter gedacht, Basis muss nicht prim sein.
        /// </summary>
        public static bool IsPerfectPowerWithBaseAtLeast13(ulong n)
        {
            // Unterhalb 13^2 kann nichts der Form a^k mit a>=13,k>=2 sein
            if (n < 169UL)
                return false;

            // Hilfsfunktion: schnelle Potenz mit Abbruch
            static bool EqualsPow(ulong n, ulong a, int k)
            {
                ulong p = 1;
                for (int i = 0; i < k; i++)
                {
                    p *= a;
                    if (p > n) return false;
                }
                return p == n;
            }

            // 1. Quadrate testen (k = 2) – das reicht für alle geraden Exponenten
            {
                ulong r = (ulong)Math.Sqrt(n);

                // Korrigiere eventuelle Rundungsfehler
                while ((r + 1) * (r + 1) <= n) r++;
                while (r * r > n) r--;

                if (r >= 13UL && r * r == n)
                    return true;
            }

            // 2. Ungerade Exponenten k = 3,5,7,... bis zur durch 13 begrenzten Obergrenze
            float maxK = (float)(Math.Log(n) / Math.Log(13.0));  // Basis mindestens 13
            
            if(n == 3 || n == 5 || n == 7)
            {
                return true;
            }

            for (int k = 3; k <= maxK; k += 2)
            {
                // Doppelnäherung für die k-te Wurzel
                double rootD = Math.Pow(n, 1.0 / k);
                if (rootD < 12.95)
                    break; // für größere k wird die Basis nur kleiner;12,95 statt 13 - da Math.Pow nicht exakt arbeitet => sonst false-pos wegen mgl. Rundungsfehler

                ulong r = (ulong)Math.Round(rootD);
                if (r < 13UL)
                    continue;

                // Korrigiert Rundungsfehler durch exakten Vergleich
                if (EqualsPow(n, r, k)) return true;
                if (r > 13UL && EqualsPow(n, r - 1, k)) return true;
                if (EqualsPow(n, r + 1, k)) return true;
            }

            return false;
        }
    }

}

