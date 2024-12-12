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
        var plot = ParsePlot(input).SurroundWith('#');
        var regions = plot.GetRegions().Skip(1);
        var cost = regions.Sum(x => x.GetArea() * x.GetSides());
        return cost;
    }

    private static int GetSides(this IEnumerable<Position<char>> region)
    {
        var sides = region.Sum(x => 
        { 
            var corners = x.GetNeighbors().Where(n => IsCorner(n, x));
            
            return corners.Count();
        });

        return sides;

        static bool IsCorner(Position<char> n, Position<char> p)
        {
            var commons = n.GetAdjacent().Intersect(p.GetAdjacent());

            var isCorner = commons.Count() == 2;
            var isExternal = commons.All(n => n.Value != p.Value);
            var isInternal = n.Value != p.Value && commons.All(n => n.Value == p.Value);

            return isCorner && (isExternal || isInternal);
        }
    }

    private static int GetPerimeter(this IEnumerable<Position<char>> region)
    {
        var perimeter = region.Sum(x =>
        {
            var adjacent = x.GetAdjacent().Where(y => x.Value == y.Value);
            var sides = 4 - adjacent.Count();

            return sides;
        });

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

            var adjacent = x.GetAdjacent().Where(a => a.Value == x.Value && !done[a.Row, a.Column]);

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