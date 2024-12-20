using AdventOfCode2024.Day18;

namespace AdventOfCode2024.Day19;
public static class LinenLayout
{
    public static int CountPossibleLayouts(string input)
    {
        var (availableLayouts, desiredLayouts) = ParseInput(input);
        var count = desiredLayouts.Count(x => CountPossibleLayouts(x, availableLayouts, []) > 0);
        return count;
    }

    public static long CountAllPossibleLayouts(string input)
    {
        var (availableLayouts, desiredLayouts) = ParseInput(input);
        var count = desiredLayouts.Sum(x => CountPossibleLayouts(x, availableLayouts, []));
        return count;
    }

    private static long CountPossibleLayouts(string desiredLayout, string[] availableLayouts, Dictionary<string, long> cache)
    {
        if (cache.TryGetValue(desiredLayout, out var cached)) return cached;

        long possible = 0;

        foreach (var availableLayout in availableLayouts)
        {
            if (desiredLayout == availableLayout)
            {
                possible++;
                continue;
            }

            if (availableLayout.Length > desiredLayout.Length) continue;

            var bitDesired = desiredLayout[..(availableLayout.Length)];

            if (bitDesired != availableLayout) continue;

            possible += CountPossibleLayouts(desiredLayout[(bitDesired.Length)..], availableLayouts, cache);
        }

        cache.TryAdd(desiredLayout, possible);

        return possible;
    }

    private static (string[] AvailableLayouts, string[] DesiredLayouts) ParseInput(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var availableLayouts = lines[0].Split(',', StringSplitOptions.TrimEntries);
        var desiredLayouts = lines[2..];

        return (availableLayouts, desiredLayouts);
    }
}
