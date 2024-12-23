namespace AdventOfCode2024.Day21;
public static class KeypadConundrum
{
    public static int AirlockPassword(string input)
    {
        var codes = input.Split(Environment.NewLine);
        var moves = codes.Select(code =>
        {
            var airlock = NumericKeypadMoves(code);

            var firstRobot = airlock
                .SelectMany(x => DirectionalKeypadMoves(x))
                .GroupBy(x => x.Count())
                .MinBy(x => x.Key);

            var secondRobot = firstRobot!
                .SelectMany(x => DirectionalKeypadMoves(x))
                .GroupBy(x => x.Count())
                .MinBy(x => x.Key);

            return int.Parse(code[..3]) * secondRobot!.First().Count();
        });
        return moves.Sum();
    }

    private static IEnumerable<IEnumerable<char>> DirectionalKeypadMoves(IEnumerable<char> code, char start = 'A')
    {
        var moveOptions = code.Select(x =>
        {
            var directions = DirectionalKeypadMoves(start, x);
            start = x;
            return directions;
        });

        var moves = moveOptions.Skip(1).Aggregate(moveOptions.First(), (a, x) => a.SelectMany(y => x.Select(z => y.Concat(z))));

        return moves;
    }

    private static IEnumerable<IEnumerable<char>> DirectionalKeypadMoves(char start, char end)
    {
        IEnumerable<IEnumerable<char>> moves = (start, end) switch
        {
            (var s, var e) when s == e => [['A']],
            ('A', '>') or ('^', 'v') => [['v', 'A']],
            ('A', '^') or ('v', '<') or ('>', 'v') => [['<', 'A']],
            ('A', 'v') => [['v', '<', 'A'], ['<', 'v', 'A']],
            ('A', '<') => [['v', '<', '<', 'A'], ['<', 'v', '<', 'A']],
            ('^', 'A') or ('v', '>') or ('<', 'v') => [['>', 'A']],
            ('^', '>') => [['v', '>', 'A'], ['>', 'v', 'A']],
            ('^', '<') => [['v', '<', 'A']],
            ('>', 'A') or ('v', '^') => [['^', 'A']],
            ('>', '^') => [['<', '^', 'A'], ['^', '<', 'A']],
            ('>', '<') => [['<', '<', 'A']],
            ('v', 'A') => [['>', '^', 'A'], ['^', '>', 'A']],
            ('<', '^') => [['>', '^', 'A']],
            ('<', 'A') => [['>', '>','^', 'A'], ['>', '^', '>', 'A']],
            (_, _) => throw new Exception($"Keypad combo not found {start} {end}")
        };

        return moves;
    }

    private static IEnumerable<IEnumerable<char>> NumericKeypadMoves(IEnumerable<char> code, char start = 'A')
    {
        var moveOptions = code.Select(x =>
        {
            var directions = NumericKeypadMoves(start, x);
            start = x;
            return directions;
        });

        var moves = moveOptions.Skip(1).Aggregate(moveOptions.First(), (a, x) => a.SelectMany(y => x.Select(z => y.Concat(z))));

        return moves;
    }

    private static IEnumerable<IEnumerable<char>> NumericKeypadMoves(char start, char end)
    {
        IEnumerable<IEnumerable<char>> moves = (start, end) switch
        {
            (var s, var e) when s == e => [['A']],
            ('A', '0') or ('3', '2') or ('2', '1') or ('6', '5') or ('5', '4') or ('9', '8') or ('8', '7') => [['<', 'A']],
            ('A', '1') => [['^', '<', '<', 'A'], ['<', '^', '<', 'A']],
            ('A', '2') or ('3', '5') or ('6', '8') or ('2', '4') or ('5', '7') => [['^', '<', 'A'], ['<', '^', 'A']],
            ('A', '3') or ('0', '2') or ('1', '4') or ('2', '5') or ('3', '6') or ('4', '7') or ('5', '8') or ('6', '9') => [['^', 'A']],
            ('A', '4') => [['^', '^', '<', '<', 'A'], ['^', '<', '^', '<', 'A'], ['^', '<', '<', '^', 'A'], ['<', '^', '<', '^', 'A']],
            ('A', '5') or ('2', '7') or ('3', '8') => [['^', '^', '<', 'A'], ['^', '<', '^', 'A'], ['<', '^', '^', 'A']],
            ('A', '6') or ('0', '5') or ('1', '7') or ('2', '8') or ('3', '9') => [['^', '^', 'A']],
            ('A', '7') => [
                ['^', '^', '^', '<', '<', 'A'],
                ['^', '^', '<', '^', '<', 'A'],
                ['^', '^', '<', '<', '^', 'A'],
                ['^', '<', '^', '^', '<', 'A'],
                ['^', '<', '<', '^', '^', 'A'],
                ['<', '^', '^', '^', '<', 'A'],
                ['<', '^', '^', '<', '^', 'A'],
                ['<', '^', '<', '^', '^', 'A'],],
            ('A', '8') => [
                ['^', '^', '^', '<', 'A'],
                ['^', '^', '<', '^', 'A'],
                ['^', '<', '^', '^', 'A'],
                ['<', '^', '^', '^', 'A']],
            ('A', '9') or ('0', '8') => [['^', '^', '^', 'A']],
            ('0', 'A') or ('2', '3') or ('5', '6') or ('8', '9') or ('1', '2') or ('4', '5') or ('7', '8') => [['>', 'A']],
            ('0', '1') => [['^', '<', 'A']],
            ('0', '3') or ('2', '6') or ('5', '9') or ('1', '5') or ('4', '8') => [['>', '^', 'A'], ['^', '>', 'A']],
            ('0', '4') => [['^', '^', '<', 'A'], ['^', '<', '^', 'A']],
            ('0', '6') or ('2', '9') or ('1', '8') => [['^', '^', '>', 'A'], ['^', '>', '^', 'A'], ['>', '^', '^', 'A']],
            ('0', '7') => [['^', '^', '^', '<', 'A'], ['^', '^', '<', '^', 'A'], ['^', '<', '^', '^', 'A']],
            ('0', '9') => [
                ['^','^','^','>','A'],
                ['^','^','>','^','A'],
                ['^','>','^','^','A'],
                ['>','^','^','^','A']],
            ('1', '0') => [['>', 'v', 'A']],
            ('1', '3') or ('4', '6') or ('7', '9') => [['>', '>', 'A']],
            ('1', '6') or ('4', '9') => [
                ['^','>','>','A'],
                ['>','^','>','A'],
                ['>','>','^','A']],
            ('1', '9') => [
                ['^','^','>','>','A'],
                ['^','>','^','>','A'],
                ['^','>','>','^','A'],
                ['>','^','>','^','A'],
                ['>','>','^','^','A']],
            ('2', 'A') or ('5', '3') or ('8', '6') or ('7', '5') or ('4', '2') => [['v', '>', 'A'], ['>', 'v', 'A']],
            ('2', '0') or ('3', 'A') or ('4', '1') or ('5', '2') or ('6', '3') or ('7', '4') or ('8', '5') or ('9', '6') => [['v', 'A']],
            ('3', '1') or ('6', '4') or ('9', '7') => [['<', '<', 'A']],
            ('3', '7') => [
                ['^','^','<','<','A'],
                ['^','<','^','<','A'],
                ['^','<','<','^','A'],
                ['<','^','<','^','A'],
                ['<','<','^','^','A']],
            ('5', 'A') or ('7', '2') or ('8', '3') => [['v', 'v', '>', 'A'], ['v', '>', 'v', 'A'], ['>', 'v', 'v', 'A']],
            ('5', '0') or ('7', '1') or ('8', '2') or ('9', '3') or ('6', 'A') => [['v', 'v', 'A']],
            ('6', '0') or ('9', '2') or ('8', '1') => [['v', 'v', '<', 'A'], ['v', '<', 'v', 'A'], ['<', 'v', 'v', 'A']],
            ('7', 'A') => [
                ['>', '>','v','v','v','A'],
                ['>', 'v','>','v','v','A'],
                ['>', 'v','v','>','v','A'],
                ['>', 'v','v','v','>','A'],
                ['v', '>','>','v','v','A'],
                ['v', '>','v','>','v','A'],
                ['v', '>','v','v','>','A'],
                ['v', 'v','>','>','v','A'],
                ['v', 'v','>','v','>','A']],
            ('7', '0') => [
                ['>','v','v','v','A'],
                ['v','>','v','v','A'],
                ['v','v','>','v','A']],
            ('7', '3') => [
                ['>', '>','v','v','A'],
                ['>', 'v','>','v','A'],
                ['>', 'v','v','>','A'],
                ['v', '>','>','v','A'],
                ['v', '>','v','>','A'],
                ['v', 'v','>','>','A']],
            ('8', 'A') => [
                ['>','v','v','v','A'],
                ['v','>','v','v','A'],
                ['v','v','>','v','A'],
                ['v','v','v','>','A']],
            ('8', '0') or ('9', 'A') => [['v', 'v', 'v', 'A']],
            ('9', '0') => [
                ['<','v','v','v','A'],
                ['v','<','v','v','A'],
                ['v','v','<','v','A'],
                ['v','v','v','<','A']],
            ('9', '1') => [
                ['<', '<','v','v','A'],
                ['<', 'v','<','v','A'],
                ['<', 'v','v','<','A'],
                ['v', '<','<','v','A'],
                ['v', '<','v','<','A'],
                ['v', 'v','<','<','A']],
            (_, _) => throw new Exception($"Keypad combo not found {start} {end}")
        };

        return moves;
    }
}