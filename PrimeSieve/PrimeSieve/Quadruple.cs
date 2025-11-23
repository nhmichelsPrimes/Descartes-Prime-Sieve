namespace PrimeGeometry
{
    /// <summary>
    /// Bündelt die beiden Darstellungen der Norm z:
    /// 
    ///   z = a² + b²       (Gauss-Ebene)
    ///   z = d² - d e + e² (Eisenstein-Ebene)
    /// 
    /// Wird in SieveCandidate und SieveResult verwendet.
    /// </summary>
    public sealed class Quadruple
    {
        /// <summary>
        /// Gauss-Koordinaten: z = A² + B².
        /// </summary>
        public long A { get; }

        public long B { get; }

        /// <summary>
        /// Eisenstein-Koordinaten: z = D² - D E + E².
        /// </summary>
        public long D { get; }

        public long E { get; }

        public Quadruple(long a, long b, long d, long e)
        {
            A = a;
            B = b;
            D = d;
            E = e;
        }

        public override string ToString()
        {
            return $"(a={A}, b={B}, d={D}, e={E})";
        }
    }
}
