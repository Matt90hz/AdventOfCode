using AdventOfCode2024.Day12;
using Xunit.Abstractions;

namespace AdventOfCode2024.Day12;
public static class GardenGroups
{
    public static int FencingCost(string input)
    {
        var plot = ParsePlot(input);
        var regions = plot.GetRegions();
        var cost = regions.Sum(x => x.GetArea() * x.GetPerimeter());
        return cost;
    }

    public static int FencingDiscountCost(string input)
    {
        var plot = ParsePlot(input);
        var regions = plot.GetRegions();
        var cost = regions.Sum(x => x.GetArea() * x.GetSides());
        return cost;
    }

    private static int GetSides(this IEnumerable<Position<char>> region)
    {
        var border = region.Where(x => x.GetAdjacent().Any(y => x.Value))
    }

    private static int GetPerimeter(this IEnumerable<Position<char>> region)
    {
        var sides = region.Select(x =>
        {
            var adjacent = x
                .GetAdjacent()
                .Where(y => x.Value == y.Value);

            var sides = 4 - adjacent.Count();

            return sides;
        });

        var perimeter = sides.Sum();

        return perimeter;
    }

    private static int GetArea(this IEnumerable<Position<char>> region)
    {
        var area = region.Count();

        return area;
    }

    private static IEnumerable<IEnumerable<Position<char>>> GetRegions(this char[,] plot)
    {
        var done = plot.Select(_ => false);

        var positions = plot.GetPositions();

        foreach (var position in positions) 
        {
            var (row, col) = position;

            if (done[row, col]) continue;
            
            var region = GetRegion(position, done);

            yield return region;
        }

        static IEnumerable<Position<char>> GetRegion(Position<char> x, bool[,] done)
        {
            var (r, c) = x;

            done[r, c] = true;

            var adjacent = x
                .GetAdjacent()
                .Where(a => a.Value == x.Value && !done[a.Row, a.Column]);

            var region = new List<Position<char>>() { x };

            foreach(var a in adjacent)
            {
                region.AddRange(GetRegion(a, done));
            }

            return region;
        }
    }

    private static char[,] ParsePlot(string input)
    {
        var plot = input
            .Split(Environment.NewLine)
            .Select(x => x.ToCharArray())
            .ToMultidimensionalArray();

        return plot;
    }
}