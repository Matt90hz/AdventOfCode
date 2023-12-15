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

        var load = tilted.GetRows()
            .Select((row, i) => row.Count(IsRoundRock) * (platform.GetLength(0) - i))
            .Sum();

        return load;
    }

    public static int LoadOnNorthSupportBeamsAfterAnInsaneAmountOfCycles(string input)
    {
        long cycles = 1_000_000_000;

        char[,] platform = GetPlatform(input);

        var cache = new Dictionary<string, char[,]>();

        while (cycles > 0)
        {
            if (cache.TryGetValue(platform.GetKey(), out var cached))
            {
                var loop = cache.SkipWhile(x => x.Key != platform.GetKey());

                var outsiders = cache.Count - loop.Count() + 1;

                var resultIndex = (1_000_000_000 - outsiders) % (loop.Count());

                var result = loop.ElementAt((int)resultIndex).Value.Load();

                return result;
                
            }
            else
            {
                var key = platform.GetKey();
                var spinned = Spin(platform);
                cache.Add(key, spinned);
                platform = spinned;
            }

            cycles--;
        }

        var load = platform.Load();

        return load;
    }

    static char[,] Spin(char[,] platform)
    {
        var tiltedNorth = platform.TiltNorth();
            
         var tiltedWest = tiltedNorth
            .RotateClockwise()
            .TiltNorth();

        var tiltedSouth = tiltedWest
            .RotateClockwise()
            .TiltNorth();

        var tiltedEast = tiltedSouth
            .RotateClockwise()
            .TiltNorth();

        var spinned = tiltedEast.RotateClockwise();

        return spinned;
    }

    static char[,] TiltNorth(this char[,] platform)
    {
        var columns = platform
            .GetColumns()
            .Select(MoveRocksNorth);

        var tilted = columns
            .ToMultidimensionalArray()
            .GetColumns()
            .ToMultidimensionalArray();

        return tilted;
    }

    static IEnumerable<char> MoveRocksNorth(IEnumerable<char> column)
    {
        if (column.Any() is false || column.Any(IsRoundRock) is false || column.All(IsRoundRock)) return column;

        if(IsSquareRock(column.First()))
        {
            var squareRocksChunk = column.TakeWhile(IsSquareRock);
            var next = column.SkipWhile(IsSquareRock);

            return squareRocksChunk.Concat(MoveRocksNorth(next));
        }

        var firstChunk = column.TakeWhile(IsNotSquareRock);

        var roundRocks = firstChunk.Count(IsRoundRock);
        var empties = firstChunk.Count(IsEmpty);

        var tilted = Enumerable.Repeat('O', roundRocks)
            .Concat(Enumerable.Repeat('.', empties));

        var rest = column.SkipWhile(IsNotSquareRock);

        return tilted.Concat(MoveRocksNorth(rest));
    }

    static int Load(this char[,] platform) => platform
        .GetRows()
        .Select((row, i) => row.Count(IsRoundRock) * (platform.GetLength(0) - i))
        .Sum();

    static bool IsRoundRock(char x) => x == 'O';

    static bool IsSquareRock(char x) => x == '#';

    static bool IsNotSquareRock(char x) => x != '#';

    static bool IsEmpty(char x) => x == '.';

    static string GetKey(this char[,] platform) 
    {
        var key = new char[platform.Length]; 

        long i = 0;

        foreach(char x in platform)
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
