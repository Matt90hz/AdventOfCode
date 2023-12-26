namespace AdventOfCode2023.Dayz18;

internal static class DigPlanParser
{
    public static (char Direction, int Steps)[] CorrectParse(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var digPlan = lines.Select(CorrectParseLine);

        return digPlan.ToArray();
    }

    static (char Direction, int Steps) CorrectParseLine(string line)
    {
        var hex = line[line.IndexOf('(')..line.IndexOf(')')];

        var dir = hex[^1] switch
        {
            '0' => 'R',
            '1' => 'D',
            '2' => 'L',
            _ => 'U',
        };

        var steps = Convert.ToInt32(hex[2..7], 16);

        return (dir, steps);
    }

    public static (char Direction, int Steps)[] Parse(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var digPlan = lines.Select(ParseLine);

        return digPlan.ToArray();
    }

    static (char direction, int steps) ParseLine(string line)
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var direction = line[0];
        var steps = int.Parse(parts[1]);

        return (direction, steps);
    }
}
