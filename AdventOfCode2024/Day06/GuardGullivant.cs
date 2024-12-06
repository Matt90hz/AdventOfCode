namespace AdventOfCode2024.Day06;
public static class GuardGallivant
{
    public static int CountPositions(string input)
    {
        var map = ParseMap(input);
        var drawnMap = map.DrawGuardPath();
        var count = drawnMap.Count(x => x == 'X');

        return count;
    }

    public static int CountInfiniteLoops(string input)
    {
        var map = ParseMap(input);

        var rowIndexes = Enumerable.Range(0, map.GetLength(0));
        var colIndexes = Enumerable.Range(0, map.GetLength(1));
        var drawn = map.DrawGuardPath();

        var possibleMaps = rowIndexes
            .SelectMany(row => colIndexes.Select(col => (row, col)))
            .Where(pos => map[pos.row, pos.col] == '.' && drawn[pos.row, pos.col] == 'X')
            .Select(pos => map.Select((x, _pos) => _pos == pos ? 'O' : x));

        int count = 0;
        var (row, col) = map.FindPosition('^') ?? default;

        Parallel.ForEach(possibleMaps, x =>
        {
            if (IsInfiniteLoop(x, row, col)) count++;         
        });

        return count;
    }

    public static bool IsInfiniteLoop(char[,] map, long row, long col)
    {
        var guard = map.GetPosition(row, col);

        guard.Value = '.';

        var direction = Direction.Up;

        while (guard.TryMove(direction, out var next))
        {
            switch ((next.Value, direction))
            {
                case ('.', _):
                    guard = next;
                    break;
                case ('^', Direction.Up)
                    or ('v', Direction.Down)
                    or ('>', Direction.Right)
                    or ('<', Direction.Left):
                    return true;
                default:
                    (next.Value, direction) = direction switch
                    {
                        Direction.Up => ('^', Direction.Right),
                        Direction.Down => ('v', Direction.Left),
                        Direction.Right => ('>', Direction.Down),
                        Direction.Left => ('<', Direction.Up),
                        _ => ('#', direction)
                    };
                    break;
            }
        }

        return false;
    }

    public static char[,] DrawGuardPath(this char[,] map)
    {
        var drawn = map.Select(x => x);

        var guard = drawn.FindPosition('^') ?? throw new Exception("Guard not found");
        var direction = Direction.Up;

        guard.Value = 'X';

        while (guard.TryMove(direction, out var next))
        {
            var x = next.Value;

            switch (x)
            {
                case '.' or 'X':
                    next.Value = 'X';
                    guard = next;
                    break;
                case '#':
                    direction = direction switch
                    {
                        Direction.Up => Direction.Right,
                        Direction.Down => Direction.Left,
                        Direction.Right => Direction.Down,
                        Direction.Left => Direction.Up,
                        _ => direction
                    };
                    break;
            }
        }

        return drawn;
    }

    public static char[,] ParseMap(string input)
    {
        var map = input
            .Split(Environment.NewLine)
            .Select(line => line.ToCharArray())
            .ToMultidimensionalArray();

        return map;
    }
}
