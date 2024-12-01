namespace AdventOfCode2024.Day01;
public static class HistorianHysteria
{
    public static int TotalDistanceBetweenLists(string input)
    {
        var (list1, list2) = ParseLists(input);

        list1.Sort();
        list2.Sort();

        var distances = list1
            .Zip(list2)
            .Select(x => Math.Abs(x.First - x.Second));

        var totalDistance = distances.Sum();

        return totalDistance;
    }

    public static int SimilarityScore(string input)
    {
        var (list1, list2) = ParseLists(input);

        var scores = list1.Select(x => list2.Count(y => x == y) * x);

        var totalScore = scores.Sum();

        return totalScore;
    }

    private static (List<int> List1, List<int> List2) ParseLists(string input)
    {
        var valuePairs = input
            .Split(Environment.NewLine)
            .Select(ParseValuePairs);

        List<int> list1 = [];
        List<int> list2 = [];

        foreach (var (value1, value2) in valuePairs)
        {
            list1.Add(value1);
            list2.Add(value2);
        }

        return (list1, list2);
    }

    private static (int X, int Y) ParseValuePairs(string line)
    {
        var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var x = int.Parse(split[0]);
        var y = int.Parse(split[1]);

        return (x, y);
    }
}
