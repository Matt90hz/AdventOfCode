using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day6;

internal static class SailCompetition
{
    //Time:        56     97     78     75
    //Distance:   546   1927   1131   1139
    public static (int Time, int Distance)[] Inputs { get; } = new[] 
    { 
        (56, 546), 
        (97, 1927), 
        (78, 1131), 
        (75, 1139) 
    };

    //Time:      7  15   30
    //Distance:  9  40  200
    public static (int Time, int Distance)[] InputsTest { get; } = new[]
    {
        (7, 9),
        (15, 40),
        (30, 200)
    };

    //Time:       56977875
    //Distance:   546192711311139
    public static (long Time, long Distance) Inputs2 { get; } = new(56977875, 546192711311139);

    //Time:       71530
    //Distance:   940200
    public static (long Time, long Distance) Inputs2Test { get; } = new(71530, 940200);

    /*
     * for time 7 distance 9
     * 0 press -> 0 distance
     * 1 press -> 6 distance
     * 2 press -> 10 distance
     * 3 press -> 12 distance
     * 4 press -> 12 distance
     * 5 press -> 10 distance
     * 6 press -> 6 distance
     * 7 press -> 0 distance
     */

    public static long WaysToWinSeriusSeries(long time, long distance)
    {
        long count = 0;

        for (long i = 1; i < time; i++)
        {
            count += i * (time - i) > distance ? 1 : 0;
        }

        return count;
    }

    public static int WaysToWin((int Time, int Distance)[] inputs)
    {
        var recordDistances = inputs.Select(x => RecordDistances(x.Time, x.Distance));
        var aggregate = recordDistances.Aggregate((acc, x) => acc * x);

        return aggregate;
    }

    static int RecordDistances(int time, int distance)
    {
        var distances = GetDistances(time);
        var recordDistances = distances.Where(x => x > distance);
        var recordConut = recordDistances.Count();
        
        return recordConut;
    }

    static IEnumerable<int> GetDistances(int time)
    {
        for (var i = 1; i < time; i++)
        {
            yield return i * (time - i);
        }
    }
}
