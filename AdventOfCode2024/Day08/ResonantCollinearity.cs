
using AdventOfCode2024.Day07;

namespace AdventOfCode2024.Day08;
public static class ResonantCollinearity
{
    public static int CountAntinodes(string input)
    {
        var map = ParseMap(input);
        var antinodes = FindAntinodes(map);
        var count = antinodes.Count();
        return count;
    }

    private static IEnumerable<Position<char>> FindAntinodes(char[,] map)
    {
        var antennas = map
            .GetPositions()
            .Where(x => x.Value != '.')
            .ToArray();

        var antennasPairs = antennas
            .SelectMany((x, i) => antennas.Skip(i + 1).Select(y => (x, y)));

        var antinodes = antennasPairs
            .SelectMany(pair => CalculateAntinodes(pair.x, pair.y))
            .Distinct();

        return antinodes;
    }

    private static IEnumerable<Position<char>> CalculateAntinodes(Position<char> x, Position<char> y)
    {
        if (x.Value != y.Value) yield break;

        var (map, xr, xc) = x;
        var (yr, yc) = y;
        var dr = xr - yr;
        var dc = xc - yc;

        var a1 = map.GetPosition(xr + dr, xc + dc);
        var a2 = map.GetPosition(yr - dr, yc - dc);

        if(!a1.IsOutOfBound()) yield return a1;
        if(!a2.IsOutOfBound()) yield return a2;
    }

    private static char[,] ParseMap(string input)
    {
        var map = input
            .Split(Environment.NewLine)
            .Select(x => x.ToCharArray())
            .ToMultidimensionalArray();

        return map;
    }
}