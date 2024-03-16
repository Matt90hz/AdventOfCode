using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day1;
static class CalorieCounting
{
    public static int MaxCalorie(string input) => input
        .Split(Environment.NewLine)
        .Select(x => int.TryParse(x, out int v) ? v : 0)
        .Aggregate(Enumerable.Empty<(int Index, int Value)>(),
        (acc, item) =>
        {
            int index = acc.Any()
                ? acc.Last().Index
                : 1;

            return item == 0
                ? acc.Append((++index, item))
                : acc.Append((index, item));
        })
        .GroupBy(x => x.Index, v => v.Value)
        .Select(x => x.Sum())
        .Max();

    public static int MaxCalorieTop3(string input) => input
        .Split(Environment.NewLine)
        .Select(x => int.TryParse(x, out int v) ? v : 0)
        .Aggregate(Enumerable.Empty<(int Index, int Value)>(),
        (acc, item) =>
        {
            int index = acc.Any()
                ? acc.Last().Index
                : 1;

            return item == 0
                ? acc.Append((++index, item))
                : acc.Append((index, item));
        })
        .GroupBy(x => x.Index, v => v.Value)
        .Select(x => x.Sum())
        .OrderDescending()
        .Take(3)
        .Sum();
}
