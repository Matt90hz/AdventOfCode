using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz12;

abstract record Spring(int Count);
record Operational(int Count) : Spring(Count);
record Dameged(int Count) : Spring(Count);
record Unknown(int Count) : Spring(Count);

internal static class HotSprings
{
    const char OPERATIONAL = '.';
    const char DAMEGED = '#';
    const char UNKNOWN = '?';

    private static readonly IDictionary<string, long> _cache =  new Dictionary<string, long>();

    public static long ConditionRecordsCombinationsUnfolded(string[] records)
    {
        var conditionsCombinations = records
            .Select(GetConditionRecordExpanded)
            .Select(x => ConditionRecordCombinationsUnfolded(x.Springs, x.DamegedMap))
            .ToArray();

        var combinations = conditionsCombinations.Sum();

        return combinations;
    }

    public static int ConditionRecordsCombinations(string[] records)
    {
        var conditionsCombinations = records
            .Select(GetConditionRecord)
            .Select(x => ConditionRecordCombinations(x.Springs, x.DamegedMap))
            .ToArray();

        var combinations = conditionsCombinations.Sum();

        return combinations;
    }

    public static long ConditionRecordCombinationsUnfolded(IEnumerable<Spring> springs, IEnumerable<int> damegedMap)
    {
        if (springs.Any() is false)
        {
            if (damegedMap.Any() is false) return 1;

            return 0;
        }

        if (damegedMap.Any() is false)
        {
            if (springs.Any(x => x is Dameged)) return 0;

            return 1;
        }

        var key = springs.ToKey() + damegedMap.ToKey();

        if(_cache.ContainsKey(key)) return _cache[key];

        if (springs.First() is Operational)
            return ConditionRecordCombinationsUnfolded(springs.Skip(1), damegedMap);

        long combinations = 0;

        if (springs.First() is Operational or Unknown)
            combinations += ConditionRecordCombinationsUnfolded(springs.Skip(1), damegedMap);

        if (springs.First() is Dameged or Unknown)
        {
            var thereAreEnoughSprings = damegedMap.First() <= springs.Count();
            var allSpringsAreNotOperational = springs.Take(damegedMap.First()).All(x => x is not Operational);

            if (thereAreEnoughSprings && allSpringsAreNotOperational
                && (damegedMap.First() == springs.Count() || springs.ElementAt(damegedMap.First()) is not Dameged))
            {
                combinations += ConditionRecordCombinationsUnfolded(springs.Skip(damegedMap.First() + 1), damegedMap.Skip(1));
            }
        }

        _cache.Add(key, combinations);

        return combinations;
    }

    public static int ConditionRecordCombinations(IEnumerable<Spring> springs, IEnumerable<int> damegedMap)
    {
        if (springs.Any() is false)
        {
            if (damegedMap.Any() is false) return 1;

            return 0;
        }

        if (damegedMap.Any() is false)
        {
            if (springs.Any(x => x is Dameged)) return 0;

            return 1;
        }

        if (springs.First() is Operational)
            return ConditionRecordCombinations(springs.Skip(1), damegedMap);

        int combinations = 0;

        if (springs.First() is Operational or Unknown)
            combinations += ConditionRecordCombinations(springs.Skip(1), damegedMap);

        if (springs.First() is Dameged or Unknown)
        {
            var thereAreEnoughSprings = damegedMap.First() <= springs.Count();
            var allSpringsAreNotOperational = springs.Take(damegedMap.First()).All(x => x is not Operational);

            if (thereAreEnoughSprings && allSpringsAreNotOperational
                && (damegedMap.First() == springs.Count() || springs.ElementAt(damegedMap.First()) is not Dameged))
            {
                combinations += ConditionRecordCombinations(springs.Skip(damegedMap.First() + 1), damegedMap.Skip(1));
            }
        }

        return combinations;
    }

    public static (Spring[] Springs, int[] DamegedMap) GetConditionRecordExpanded(string record)
    {
        var mapAndBroken = record.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var map = mapAndBroken[0]
            .Select(GetSpring)
            .Append(new Unknown(1))
            .ToArray();

        var mapTime5 = Enumerable.Range(1, 5)
            .SelectMany(_ => map)
            .SkipLast(1)
            .ToArray();

        var broken = mapAndBroken[1]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        var brokenTime5 = Enumerable.Range(1, 5)
            .SelectMany(_ => broken)
            .ToArray();

        return (mapTime5, brokenTime5);
    }

    public static (Spring[] Springs, int[] DamegedMap) GetConditionRecord(string record)
    {
        var mapAndBroken = record.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var map = mapAndBroken[0]
            .Select(GetSpring)
            .ToArray();

        var broken = mapAndBroken[1]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        return (map, broken);
    }

    static Spring GetSpring(char x) => x switch
    {
        OPERATIONAL => new Operational(1),
        DAMEGED => new Dameged(1),
        _ => new Unknown(1)
    };

    static string ToKey(this IEnumerable<int> damegeMap) => new string(damegeMap.SelectMany(x => x.ToString() + ",").ToArray());

    static string ToKey(this IEnumerable<Spring> springs) => new string(springs.Select(ToKey).ToArray());

    static char ToKey(this Spring spring) => spring switch
    {
        Operational => OPERATIONAL,
        Dameged => DAMEGED,
        _ => UNKNOWN
    };
}