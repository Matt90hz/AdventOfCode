namespace AdventOfCode2024.Day05;
public static class PrintQueue
{
    public static int SumMiddlePageOfCorrectUpdates(string input)
    {
        var (rules, updates) = ParseRulesAndUpdates(input);

        var correctUpdates = updates.Where(x => IsCorrect(x, rules));

        var sum = correctUpdates.Sum(x => x[x.Length / 2]);

        return sum;
    }

    public static int SumMiddlePageOfCorrectedUpdates(string input)
    {
        var (rules, updates) = ParseRulesAndUpdates(input);

        var correctedUpdates = updates
            .Where(x => IsCorrect(x, rules) is false)
            .Select(x => Correct(x, rules));

        var sum = correctedUpdates.Sum(x => x[x.Length / 2]);

        return sum;
    }

    private static int[] Correct(int[] update, (int Before, int After)[] rules)
    {
        var comparer = Comparer<int>.Create((x, y) =>
        {
            if(x == y) return 0;

            var (before, _) = rules.First(rule =>
            {
                var (b, a) = rule;

                return (b == x && a == y) || (b == y && a == x);
            });

            var compare = before == x ? -1 : 1;

            return compare;
        });

        Array.Sort(update, comparer);

        return update;
    }

    private static bool IsCorrect(int[] update, (int Before, int After)[] rules)
    {
        var isCorrect = rules.All(rule =>
        {
            var (before, after) = rule;

            var i = Array.IndexOf(update, before);
            var j = Array.IndexOf(update, after);

            return i < 0 || j < 0 || i < j;
        });

        return isCorrect;
    }

    private static ((int Before, int After)[] Rules, int[][] Updates) ParseRulesAndUpdates(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var rules = lines
            .TakeWhile(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Split('|'))
            .Select(x => (int.Parse(x[0]), int.Parse(x[1])))
            .ToArray();

        var updates = lines
            .SkipWhile(x => !string.IsNullOrWhiteSpace(x))
            .Skip(1)
            .Select(x => x.Split(',').Select(int.Parse).ToArray())
            .ToArray();

        return (rules, updates);
    }
}

