using IncaTechnologies.Collection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz14;
internal static class ParabolicReflectorDish
{
    public static int LoadOnNorthSupportBeams(string input)
    {
        char[,] platform = GetPlatform(input);

        var tilted = platform.TiltNorth();

        var load = tilted.CalculateLoad();

        return load;
    }

    public static int LoadOnNorthSupportBeamsAfterAnInsaneAmountOfCycles(string input)
    {
        long cycles = 1_000_000_000;

        char[,] platform = GetPlatform(input);

        var cache = new List<string>();
        var map = new Dictionary<string, char[,]>();

        while (cycles > 0)
        {
            if (cache.Contains(platform.GetKey())) break;

            var key = platform.GetKey();

            cache.Add(key);
            map.Add(key, platform);

            platform = Spin(platform);

            cycles--;
        }

        // the cache as found a loop that will repeat infinitely
        // so you have only to find where it will end the repeated loop
        var spare = cache.IndexOf(platform.GetKey());
        var loop = cache.Count - spare;
        var indexInLoop = cycles % loop;
        var indexInCache = spare + indexInLoop;

        var load = map[cache[(int)indexInCache]].CalculateLoad();

        return load;
    }

    static char[,] Spin(char[,] platform)
    {
        var tiltedNorth = platform
            .Select(x => x) //clone the platform
            .TiltNorth();

        var tiltedWest = tiltedNorth
           .RotateClockwise()
           .TiltNorth();

        var tiltedSouth = tiltedWest
            .RotateClockwise()
            .TiltNorth();

        var tiltedEast = tiltedSouth
            .RotateClockwise()
            .TiltNorth();

        var original = tiltedEast.RotateClockwise();

        return original;
    }

    static char[,] TiltNorth(this char[,] platform)
    {
        int rows = platform.GetLength(0);
        int cols = platform.GetLength(1);

        for (int c = 0; c < cols; c++)
        {
            int empty = 0;

            for (int r = 0; r < rows; r++)
            {
                var x = platform[r, c];

                switch (x)
                {
                    case '#':
                        empty = r + 1;
                        break;
                    case 'O':
                        platform[r, c] = '.';
                        platform[empty, c] = x;
                        empty++;
                        break;
                }
            }
        }

        return platform;
    }

    static int CalculateLoad(this char[,] platform) => platform
        .GetRows()
        .Select((row, i) => row.Count(x => x == 'O') * (platform.GetLength(0) - i))
        .Sum();

    static string GetKey(this char[,] platform)
    {
        var key = new char[platform.Length];

        long i = 0;

        foreach (char x in platform)
        {
            key[i++] = x;
        }

        return new string(key);
    }

    static char[,] GetPlatform(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var platform = lines
            .Select(x => x.ToArray())
            .ToArray()
            .ToMultidimensionalArray();

        return platform;
    }
}
