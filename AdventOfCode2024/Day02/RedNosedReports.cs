namespace AdventOfCode2024.Day02;
public static class RedNosedReports
{
    public static int HowManyReportsAreSafe(string input)
    {
        var reports = ParseReports(input);
        var safe = reports.Where(IsSafe);

        return safe.Count();
    }

    public static int HowManyReportsAreSafeWithProblemDampener(string input)
    {
        var reports = ParseReports(input).Select(x => x.ToArray());
        var safe = reports.Where(IsSafeWithDampener);

        return safe.Count();
    }

    private static bool IsSafeWithDampener(int[] report)
    {
        var dampenerReports = report
                .Select((x, i) => report.Where((_, j) => j != i))
                .Prepend(report);

        var isSafe = dampenerReports.Any(IsSafe);

        return isSafe;
    }

    private static bool IsSafe(IEnumerable<int> report)
    {
        var pairs = report.Zip(report.Skip(1));
        var differences = pairs.Select(pair => pair.First - pair.Second);
        int sign = int.Sign(differences.First());

        if(sign == 0) return false;

        var isSafe = differences.All(x => sign == int.Sign(x) && int.Abs(x) <= 3);

        return isSafe;
    }

    private static IEnumerable<IEnumerable<int>> ParseReports(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var reports = lines.Select(line => line.Split(' ').Select(int.Parse));

        return reports;
    }
}
