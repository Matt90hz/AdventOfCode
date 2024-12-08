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

    public static int CountResonantAntinodes(string input)
    {
        var map = ParseMap(input);
        var antinodes = FindResonantAntinodes(map);
        var count = antinodes.Count();
        return count;
    }

    private static IEnumerable<Position<char>> FindResonantAntinodes(char[,] map)
    {
        var antennas = map
            .GetPositions()
            .Where(x => x.Value != '.')
            .ToArray();

        var antennasPairs = antennas
            .SelectMany((x, i) => antennas.Skip(i + 1).Select(y => (x, y)))
            .Where(pair => pair.x.Value == pair.y.Value);

        var antinodes = antennasPairs
            .SelectMany(pair => CalculateResonantAntinodes(pair.x, pair.y))
            .Distinct();

        return antinodes;
    }

    private static IEnumerable<Position<char>> CalculateResonantAntinodes(
        Position<char> x, 
        Position<char> y)
    {
        var (map, xr, xc) = x;
        var (yr, yc) = y;
        var dr = xr - yr;
        var dc = xc - yc;
        int i = 1;

        while (true)
        {
            var a1 = map.GetPosition(xr + dr * i, xc + dc * i);
            var a2 = map.GetPosition(yr - dr * i, yc - dc * i);

            bool a1Out = a1.IsOutOfBound();
            bool a2Out = a2.IsOutOfBound();

            if (a1Out && a2Out)
            {               
                yield return x;
                yield return y;
                yield break; 
            }

            if (!a1Out) yield return a1;
            if (!a2Out) yield return a2;

            i++;
        }
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