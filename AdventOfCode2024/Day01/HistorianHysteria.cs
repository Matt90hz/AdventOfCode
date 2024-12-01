using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Day01;
public static class HistorianHysteria
{
    public static int TotalDistanceBetweenLists(string input)
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

        list1.Sort();
        list2.Sort();

        var distances = list1
            .Zip(list2)
            .Select(x => Math.Abs(x.First - x.Second));

        var totalDistance = distances.Sum();

        return totalDistance;
    }

    private static (int X, int Y) ParseValuePairs(string line)
    {
        var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var x = int.Parse(split[0]);
        var y = int.Parse(split[1]);

        return (x, y);
    }
}
