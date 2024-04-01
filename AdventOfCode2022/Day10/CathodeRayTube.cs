using IncaTechnologies.Collection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day10;

static class CathodeRayTube
{
    public static int SumInterestingSignalStranght(string input) => input
        .Split(Environment.NewLine)
        .SelectMany<string, int>(line => line == "noop" ? [0] : [0, int.Parse(line[5..])])
        .Aggregate(new List<int> { 1 },
        (listOfX, x) =>
        {
            listOfX.Add(x + listOfX[^1]);

            return listOfX;
        })
        .Select((x, i) => x * (i + 1))
        .Where((_, i) => i + 1 is 20 or 60 or 100 or 140 or 180 or 220)
        .Sum();

    public static string GetScreenImage(string input) => input
        .Split(Environment.NewLine)
        .SelectMany<string, int>(line => line == "noop" ? [0] : [0, int.Parse(line[5..])])
        .Aggregate(new List<int> { 1 },
        (listOfX, x) =>
        {
            listOfX.Add(x + listOfX[^1]);

            return listOfX;
        })
        .Take(240)
        .Chunk(40)
        .Select(cycle => cycle.Select((sprite, pixel) => pixel >= sprite - 1 && pixel <= sprite + 1 ? '#' : '.'))
        .ToMultidimensionalArray()
        .ToFriendlyString();

}
