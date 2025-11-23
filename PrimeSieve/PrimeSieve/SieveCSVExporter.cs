using PrimeGeometry;

public static class SieveCsvExporter
{
    public static void WriteResultsToCsv(
        string filePath,
        IReadOnlyList<SieveResult> results)
    {
        using var writer = new StreamWriter(filePath);

        // Kopfzeile inkl. k1,k2,k3
        writer.WriteLine(
            "P1;P2;Z;N;" +
            "K1;K2;K3;" +
            "A;B;D;E;" +
            "IsPrimeP1;IsPrimeP2;IsPrimeZ");

        foreach (var r in results)
        {
            string line = string.Join(";", new[]
            {
                r.P1.ToString(),
                r.P2.ToString(),
                r.Z.ToString(),
                r.N.ToString(),

                r.K1.ToString(),
                r.K2.ToString(),
                r.K3.ToString(),

                r.A.ToString(),
                r.B.ToString(),
                r.D.ToString(),
                r.E.ToString(),

                r.IsPrimeP1?? true ? "1" : "0",
                r.IsPrimeP2?? true ? "1" : "0",
                r.IsPrimeZ?? true  ? "1" : "0"
            });

            writer.WriteLine(line);
        }
    }
}
