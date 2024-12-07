using System.Diagnostics;

namespace AdventOfCode2024.Day07;
public static class BridgeRepair
{
    public static long TotalCalibrationResult(string input)
    {
        var equations = ParseEquations(input);
        var trueEquations = equations.Where(IsTrue);
        var total = trueEquations.Sum(x => x.Result);
        return total;
    }

    public static long TotalCalibrationResultFix(string input)
    {
        var equations = ParseEquations(input);
        var trueEquations = equations.Where(IsTrueFix);
        var total = trueEquations.Sum(x => x.Result);
        return total;
    }

    private static bool IsTrueFix((long Result, long[] Values) equation)
    {
        var (total, values) = equation;

        var results = values.Take(1).ToList();

        foreach (var value in values.Skip(1))
        {
            var newResults = new List<long>(3 * results.Count);

            foreach (var result in results)
            {
                newResults.Add(result + value);
                newResults.Add(result * value);
                newResults.Add(Concat(result, value));
            }

            results = newResults;
        }

        return results.Contains(total);
    }

    public static long Concat(long a, long b)
    {
        long multiplier = 1;
        long temp = b;
        while (temp > 0)
        {
            temp /= 10;
            multiplier *= 10;
        }

        return a * multiplier + b;
    }

    private static bool IsTrue((long Result, long[] Values) equation)
    {
        var (total, v) = equation;
        var results = v.Take(1).ToList();

        foreach (var value in v.Skip(1))
        {
            var newResults = new List<long>(2 * results.Count);

            foreach (var result in results)
            {
                newResults.Add(result + value);
                newResults.Add(result * value);
            }

            results = newResults;
        }

        return results.Contains(total);
    }

    private static (long Result, long[] Values)[] ParseEquations(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var equations = lines.Select(line =>
        {
            var split = line.Split(':');
            var result = long.Parse(split[0]);
            var values = split[1]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray();

            return (result, values);
        });

        return equations.ToArray();
    }
}