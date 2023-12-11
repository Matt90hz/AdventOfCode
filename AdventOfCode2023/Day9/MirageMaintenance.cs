using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day9;
internal static class MirageMaintenance
{
    public static int OasisBackwardAnalisys(string[] lines)
    {
        var predictions = lines
            .Select(ParseValues)
            .Select(PredictPastAnalisys);

        var analisys = predictions.Sum();

        return analisys;
    }

    public static int OasisAnalisys(string[] lines)
    {
        var predictions = lines
            .Select(ParseValues)
            .Select(PredictAnalisys);

        var analisys = predictions.Sum();

        return analisys;
    }

    public static int[] ParseValues(string line)
    {
        return line
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();
    }

    public static int PredictAnalisys(int[] values)
    {
        var pairSubtraction = values
            .Skip(1)
            .Select((x, i) => x - values[i])
            .ToArray();

        if (pairSubtraction.All(x => x == 0)) return values.Last();

        return values.Last() + PredictAnalisys(pairSubtraction);
    }

    public static int PredictPastAnalisys(int[] values)
    {
        var pairSubtraction = values
            .Skip(1)
            .Select((x, i) => x - values[i])
            .ToArray();

        if (pairSubtraction.All(x => x == 0)) return values.First();

        return values.First() - PredictPastAnalisys(pairSubtraction);
    }
}
