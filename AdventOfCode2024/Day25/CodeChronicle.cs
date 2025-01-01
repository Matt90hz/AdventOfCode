
namespace AdventOfCode2024.Day25;
public static class CodeChronicle
{
    public static int CountFittingKeys(string input)
    {
        var (locks, keys) = ParseLocksAndKeys(input);
        var combinations = locks.SelectMany(l => keys.Select(k => (l, k)));
        var fitting = combinations.Where(x =>
        {
            var (l, k) = x;
            var len = k.Length;
            var isFit = l
                .Zip(k)
                .Select(x => x.First + x.Second)
                .All(x => x <= 5);
            return isFit;
        });
        return fitting.Count();
    }

    private static (int[][] locks, int[][] keys) ParseLocksAndKeys(string input)
    {
        var locksAndKeys = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Chunk(7);
        var locks = locksAndKeys
            .Where(x => x.First().All(x => x == '#'))
            .Select(x => x
                .ToMultidimensionalArray()
                .GetColumns()
                .Select(x => x.Count(x => x == '#') - 1)
                .ToArray())           
            .ToArray();
        var keys = locksAndKeys
            .Where(x => x.Last().All(x => x == '#'))
            .Select(x => x
                .ToMultidimensionalArray()
                .GetColumns()
                .Select(x => x.Count(x => x == '#') - 1)
                .ToArray())
            .ToArray();
        return (locks, keys);
    }
}