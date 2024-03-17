using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day04;
static class CampCleanup
{
    public static int CountRedundants(string input) => input
        .Split(Environment.NewLine)
        .Select(line => line
            .Split(',', '-')
            .Select(int.Parse)
            .ToArray())
        .Where(pair =>
        {
            var (a, b, x, y) = (pair[0], pair[1], pair[2], pair[3]);

            return (a <= x && b >= y) || (x <= a && y >= b);
        })
        .Count();

    public static int CountPartiallyRedundants(string input) => input
        .Split(Environment.NewLine)
        .Select(line => line
            .Split(',', '-')
            .Select(int.Parse)
            .ToArray())
        .Where(pair =>
        {
            var (a, b, x, y) = (pair[0], pair[1], pair[2], pair[3]);

            return (a <= x && b >= y) || (x <= a && y >= b)
                || (a <= x && b >= x) || (b >= y && a <= y);
        })
        .Count();
}
