
namespace AdventOfCode2024.Day15;
public static class WarehouseWoes
{
    public static int SumGPSCoordinates(string input)
    {
        var (map, moves) = ParseInput(input);
        var simulation = moves.Select(x => Move(map, x)).ToArray();
        var sum = simulation.Last().GetPositions().Sum(GPSCoordinate);

        return sum;
    }

    private static int GPSCoordinate(Position<char> p)
    {
        if(p.Value != 'O') return 0;

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
