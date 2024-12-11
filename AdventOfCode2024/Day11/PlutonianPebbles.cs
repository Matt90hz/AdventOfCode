using AdventOfCode2024.Day10;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AdventOfCode2024.Day11;
public static class PlutonianPebbles
{
    public static long StonesAfterBlinks(string input, int blinks)
    {
        var stones = input
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse);

        var count = stones.Select(x => StonesAfterBlinks(x, blinks, [])).Sum();

        return count;
    }

    public static long StonesAfterBlinks(long stone, int blinks, Dictionary<(long, long), long> cache)
    {
        var key = (stone, blinks); 

        if (cache.ContainsKey(key)) return cache[key];

        if (blinks == 0) return 1;

        if (stone == 0) return StonesAfterBlinks(1, blinks - 1, cache);

        int nDigits = (int)Math.Log10(stone) + 1;

        long stones;

        if (nDigits % 2 == 0) 
        { 
            int power = (int)Math.Pow(10, nDigits / 2);
            var a = StonesAfterBlinks(stone / power, blinks - 1, cache);
            var b = StonesAfterBlinks(stone % power, blinks - 1, cache);
            stones = a + b;
            cache[key] = stones; 
            return stones; 
        }

        stones = StonesAfterBlinks(stone * 2024, blinks - 1, cache); 
        cache[key] = stones; 
        return stones;
    }
}