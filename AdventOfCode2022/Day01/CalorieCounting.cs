using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day01;
static class CalorieCounting
{
    public static int MaxCalorie(string input) => input
        .Split(Environment.NewLine)
        .Select(x => int.TryParse(x, out int v) ? v : 0)
        .Aggregate(new List<(int Index, int Value)> { (0, 0) },
        (acc, item) =>
        {
            var (index, value) = acc[^1];

            if (item == 0) acc.Add((++index, item));
            else acc.Add((index, item));

            return acc;
        })
        .GroupBy(x => x.Index, v => v.Value)
        .Select(x => x.Sum())
        .Max();

    public static int MaxCalorieTop3(string input) => input
        .Split(Environment.NewLine)
        .Select(x => int.TryParse(x, out int v) ? v : 0)
        .Aggregate(new List<(int Index, int Value)> { (0, 0) },
        (acc, item) =>
        {
            var (index, value) = acc[^1];

            if (item == 0) acc.Add((++index, item));
            else acc.Add((index, item));

            return acc;
        })
        .GroupBy(x => x.Index, v => v.Value)
        .Select(x => x.Sum())
        .OrderDescending()
        .Take(3)
        .Sum();
}
