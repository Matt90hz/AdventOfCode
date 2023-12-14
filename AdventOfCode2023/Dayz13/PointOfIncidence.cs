using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncaTechnologies.Collection.Extensions;

namespace AdventOfCode2023.Dayz13;

internal static class PointOfIncidence
{
    public static int Summarize(string input)
    {
        var patters = GetPatterns(input);

        var reflections = patters.Select(GetReflections);

        return reflections.Sum();
    }

    static int GetReflections(char[,] pattern)
    {
        var rows = pattern.GetRows();

        var refelctionIndexesR = rows.RefectionIndexes();

        var perfectReflectionR = refelctionIndexesR.Select(x => (IsPerfect: rows.IsPerfectReflection(x), Index: x));

        if(perfectReflectionR.Any(x => x.IsPerfect))
        {
            return (perfectReflectionR.First(x => x.IsPerfect).Index + 1) * 100;
        }

        var cols = pattern.GetColumns();

        var reflectionIndexesC = cols.RefectionIndexes();

        var perfectReflectionC = reflectionIndexesC.Select(x => (IsPerfect: cols.IsPerfectReflection(x), Index: x));

        if (perfectReflectionC.Any(x => x.IsPerfect))
        {
            return perfectReflectionC.First(x => x.IsPerfect).Index + 1;
        }

        return 0;
    }

    static bool IsPerfectReflection<T>(this IEnumerable<IEnumerable<T>> source, int index)
    {
        for (int i = index, j = index + 1; i >= 0 && j < source.Count(); i--, j++)
        {
            if(source.ElementAt(i).SequenceEqual(source.ElementAt(j)) is false) return false;
        }

        return true;
    } 

    static IEnumerable<int> RefectionIndexes<T>(this IEnumerable<IEnumerable<T>> seq)
    {
        var next = seq.Skip(1);

        var matchingIndex = next
            .Select((item, i) => item.SequenceEqual(seq.ElementAt(i)) ? i : -1)
            .Where(x => x != - 1);

        return matchingIndex;
    }

    static IEnumerable<char[,]> GetPatterns(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var patters = lines
            .Select((line, i) => line == string.Empty ? ">" : line)
            .SplitOn(x => x == ">")
            .Select(x => x.ToMultidimensionalArray());

        return patters;
    }
}
