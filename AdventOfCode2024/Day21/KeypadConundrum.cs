using AdventOfCode2024.Day03;

namespace AdventOfCode2024.Day21;
public static class KeypadConundrum
{
    public static long AirlockPassword(string input, int robots = 2)
    {
        var codes = input.Split(Environment.NewLine);
        var moves = codes.Select(code =>
        {
            var airlock = NumericKeypadMoves(code);
            var count = StackedDirectionalKeypadStrokesCount(airlock, robots, []);
            return int.Parse(code[..3]) * count;
        });
        return moves.Sum();
    }

    private static long StackedDirectionalKeypadStrokesCount(IEnumerable<char> moves, int robot, Dictionary<(string, int), long> cache)
    {
        if (robot == 0) return moves.LongCount();

        var key = (new string(moves.ToArray()), robot);

        if (cache.TryGetValue(key, out var cached)) return cached;

        var start = 'A';
        long count = 0;

        foreach (var move in moves)
        {
            var x = DirectionalKeypadMoves(start, move);
            count += StackedDirectionalKeypadStrokesCount(x, robot - 1, cache);
            start = move;
        }

        cache.TryAdd(key, count);

        return count;
    }

    private static IEnumerable<char> DirectionalKeypadMoves(char start, char end)
    {
        IEnumerable<char> moves = (start, end) switch
        {
            (var s, var e) when s == e => ['A'],
            ('A', '>') or ('^', 'v') => ['v', 'A'],
            ('A', '^') or ('v', '<') or ('>', 'v') => ['<', 'A'],
            ('A', 'v') => ['<', 'v', 'A'],
            ('A', '<') => ['v', '<', '<', 'A'],
            ('^', 'A') or ('v', '>') or ('<', 'v') => ['>', 'A'],
            ('^', '>') => ['v', '>', 'A'],
            ('^', '<') => ['v', '<', 'A'],
            ('>', 'A') or ('v', '^') => ['^', 'A'],
            ('>', '^') => ['<', '^', 'A'],
            ('>', '<') => ['<', '<', 'A'],
            ('v', 'A') => ['^', '>', 'A'],
            ('<', '^') => ['>', '^', 'A'],
            ('<', 'A') => ['>', '>', '^', 'A'],
            (_, _) => throw new Exception($"Keypad combo not found {start} {end}")
        };

        return moves;
    }

    private static IEnumerable<char> NumericKeypadMoves(IEnumerable<char> code, char start = 'A')
    {
        var pairs = code.Prepend('A').Zip(code);
        var distinctMoves = pairs.Select(x => NumericKeypadMoves(x.First, x.Second));
        var moves = distinctMoves.Skip(1).Aggregate(distinctMoves.First(), (a, x) => a.Concat(x));
        return moves;
    }

    private static IEnumerable<char> NumericKeypadMoves(char start, char end)
    {
        IEnumerable<char> moves = (start, end) switch
        {
            (var s, var e) when s == e => ['A'],
            ('A', '0') or ('3', '2') or ('2', '1') or ('6', '5') or ('5', '4') or ('9', '8') or ('8', '7') => ['<', 'A'],
            ('A', '1') => ['^', '<', '<', 'A'],
            ('A', '2') or ('3', '5') or ('6', '8') or ('2', '4') or ('5', '7') => ['<', '^', 'A'],
            ('A', '3') or ('0', '2') or ('1', '4') or ('2', '5') or ('3', '6') or ('4', '7') or ('5', '8') or ('6', '9') => ['^', 'A'],
            ('A', '4') => ['^', '^', '<', '<', 'A'],
            ('A', '5') or ('2', '7') or ('3', '8') => ['<', '^', '^', 'A'],
            ('A', '6') or ('0', '5') or ('1', '7') or ('2', '8') or ('3', '9') => ['^', '^', 'A'],
            ('A', '7') => ['^', '^', '^', '<', '<', 'A'],
            ('A', '8') => ['<', '^', '^', '^', 'A'],
            ('A', '9') or ('0', '8') => ['^', '^', '^', 'A'],
            ('0', 'A') or ('2', '3') or ('5', '6') or ('8', '9') or ('1', '2') or ('4', '5') or ('7', '8') => ['>', 'A'],
            ('0', '1') => ['^', '<', 'A'],
            ('0', '3') or ('2', '6') or ('5', '9') or ('1', '5') or ('4', '8') => ['^', '>', 'A'],
            ('0', '4') => ['^', '^', '<', 'A'],
            ('0', '6') or ('2', '9') or ('1', '8') => ['^', '^', '>', 'A'],
            ('0', '7') => ['^', '^', '^', '<', 'A'],
            ('0', '9') => ['^', '^', '^', '>', 'A'],
            ('1', '0') => ['>', 'v', 'A'],
            ('1', '3') or ('4', '6') or ('7', '9') => ['>', '>', 'A'],
            ('1', '6') or ('4', '9') => ['^', '>', '>', 'A'],
            ('1', '9') => ['^', '^', '>', '>', 'A'],
            ('2', 'A') or ('5', '3') or ('8', '6') or ('7', '5') or ('4', '2') => ['v', '>', 'A'],
            ('2', '0') or ('3', 'A') or ('4', '1') or ('5', '2') or ('6', '3') or ('7', '4') or ('8', '5') or ('9', '6') => ['v', 'A'],
            ('3', '1') or ('6', '4') or ('9', '7') => ['<', '<', 'A'],
            ('3', '7') => ['<', '<', '^', '^', 'A'],
            ('5', 'A') or ('7', '2') or ('8', '3') => ['v', 'v', '>', 'A'],
            ('5', '0') or ('7', '1') or ('8', '2') or ('9', '3') or ('6', 'A') => ['v', 'v', 'A'],
            ('6', '0') or ('9', '2') or ('8', '1') => ['<', 'v', 'v', 'A'],
            ('7', 'A') => ['>', '>', 'v', 'v', 'v', 'A'],
            ('7', '0') => ['>', 'v', 'v', 'v', 'A'],
            ('7', '3') => ['v', 'v', '>', '>', 'A'],
            ('8', 'A') => ['v', 'v', 'v', '>', 'A'],
            ('8', '0') or ('9', 'A') => ['v', 'v', 'v', 'A'],
            ('9', '0') => ['<', 'v', 'v', 'v', 'A'],
            ('9', '1') => ['<', '<', 'v', 'v', 'A'],
            (_, _) => throw new Exception($"Keypad combo not found {start} {end}")
        };

        return moves;
    }
}