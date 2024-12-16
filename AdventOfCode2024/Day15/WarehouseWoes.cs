
namespace AdventOfCode2024.Day15;
public static class WarehouseWoes
{
    public static int SumGPSCoordinates(string input)
    {
        var (map, moves) = ParseInput(input);

        foreach (var move in moves) Move(map, move);

        var sum = map.GetPositions().Sum(GPSCoordinate);

        return sum;
    }

    public static int SumGPSCoordinatesBigWarehouse(string input)
    {
        var (map, moves) = ParseInput(input);

        var border = new[] { '#', '#' };
        var empty = new[] { '.', '.' };
        var obstacle = new[] { '[', ']' };
        var robot = new[] { '@', '.' };

        var mapConversion = map.GetRows().Select(x => x.SelectMany(x => x switch
        {
            '#' => border,
            '.' => empty,
            'O' => obstacle,
            _ => robot
        }));

        var bigMap = mapConversion.ToMultidimensionalArray();

        foreach (var move in moves) MoveBig(bigMap, move);

        var sum = bigMap.GetPositions().Sum(GPSCoordinateBig);

        var debug = bigMap.ToFriendlyString();

        return sum;
    }

    private static char[,] MoveBig(char[,] map, char move)
    {
        var robot = map.FindPosition('@') ?? throw new ArgumentNullException(nameof(map));

        var (next, direction) = move switch
        {
            '>' => (robot.MoveRight(), Direction.Right),
            '<' => (robot.MoveLeft(), Direction.Left),
            'v' => (robot.MoveDown(), Direction.Down),
            _ => (robot.MoveUp(), Direction.Up),
        };

        switch (next.Value)
        {
            case '.':
                next.Value = '@';
                robot.Value = '.';
                break;
            case '[' or ']':
                var connected = GetConnected(next, direction).ToArray();
                var moved = connected.Select(x => x.Move(direction)).ToArray();
                if (moved.Any(x => x is { Value: '#' })) break;
                var conValues = connected.Select(x => x.Value).ToArray();
                foreach (var c in connected) c.Value = '.';
                foreach (var (i, m) in moved.Index()) m.Value = conValues[i];
                next.Value = '@';
                robot.Value = '.';
                break;
        }

        return map;
    }

    private static IEnumerable<Position<char>> GetConnected(Position<char> next, Direction direction)
    {
        if (next.Value is not '[' and not ']') return [];

        if (direction is Direction.Left)
        {
            return next
                .GetWest()
                .TakeWhile(x => x.Value is '[' or ']')
                .Append(next);
        }

        if (direction is Direction.Right)
        {
            return next
                .GetEast()
                .TakeWhile(x => x.Value is '[' or ']')
                .Append(next);
        }

        var adjacent = next.Value switch
        {
            '[' => next.MoveRight(),
            _ => next.MoveLeft(),
        };

        var top = next.Move(direction);
        var topAdjacent = adjacent.Move(direction);

        var connected = (next.Value, top.Value) switch
        {
            (']', ']') or ('[', '[') => GetConnected(top, direction),
            _ => GetConnected(top, direction).Concat(GetConnected(topAdjacent, direction)),
        };

        return connected.Append(next).Append(adjacent);
    }

    private static int GPSCoordinateBig(Position<char> p)
    {
        if (p.Value != '[') return 0;

        var (x, y) = p;
        var gps = (x * 100) + y;

        return (int)gps;
    }

    private static int GPSCoordinate(Position<char> p)
    {
        if (p.Value != 'O') return 0;

        var (x, y) = p;
        var gps = (x * 100) + y;

        return (int)gps;
    }

    private static char[,] Move(char[,] map, char move)
    {
        var robot = map.FindPosition('@') ?? throw new ArgumentNullException();

        var path = move switch
        {
            '>' => robot.GetEast(),
            '<' => robot.GetWest(),
            'v' => robot.GetSouth(),
            _ => robot.GetNorth(),
        };

        var next = path.First();

        switch (next.Value)
        {
            case '.':
                next.Value = '@';
                robot.Value = '.';
                break;
            case 'O':
                var spaces = path.SkipWhile(x => x.Value == 'O');
                var last = spaces.First();
                if (last.Value != '.') break;
                next.Value = '@';
                robot.Value = '.';
                last.Value = 'O';
                break;
        }

        return map;
    }

    private static (char[,] map, char[] moves) ParseInput(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var map = lines
            .TakeWhile(x => x.Length > 0)
            .ToMultidimensionalArray();

        var moves = lines
            .SkipWhile(x => x.Length > 0)
            .SelectMany(x => x)
            .ToArray();

        return (map, moves);
    }
}
