using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

    public static int SummarizeSmudge(string input)
    {
        var patters = GetPatterns(input);

        var reflections = patters.Select(GetReflectionsSmudge);

        return reflections.Sum();
    }

    static int GetReflectionsSmudge(char[,] pattern, int ind)
    {
        var index = pattern.PerfectReflectionIndex();

        for (int i = 0; i < pattern.GetLength(0); i++)
        {
            for (int j = 0; j < pattern.GetLength(1); j++)
            {
                pattern[i, j] = Flip(pattern[i, j]);

                var newIndex = pattern
                    .PerfectReflectionIndexes()
                    .FirstOrDefault(x => x != index, (-1, -1));

                var reflections = newIndex.CalculateReflections();

                if (reflections > 0) return reflections;

                pattern[i, j] = Flip(pattern[i, j]);
            }
        }

        return 0;
    }

    static int GetReflections(this char[,] pattern) => 
        pattern.PerfectReflectionIndex().CalculateReflections();

    static (int Row, int Column) PerfectReflectionIndex(this char[,] pattern) =>
        pattern.PerfectReflectionIndexes().FirstOrDefault((-1, -1));

    static (int Row, int Column)[] PerfectReflectionIndexes(this char[,] pattern)
    {
        var indexes = Enumerable.Empty<(int, int)>();

        var rows = pattern.GetRows();

        var refelctionIndexesR = rows.RefectionIndexes();

        var perfectReflectionR = refelctionIndexesR.Select(x => (IsPerfect: rows.IsPerfectReflection(x), Index: x));

        if (perfectReflectionR.Any(x => x.IsPerfect))
        {
            var index = perfectReflectionR
                .Where(x => x.IsPerfect)
                .Select(x => (x.Index, -1));

            indexes = indexes.Concat(index);
        }

        var cols = pattern.GetColumns();

        var reflectionIndexesC = cols.RefectionIndexes();

        var perfectReflectionC = reflectionIndexesC.Select(x => (IsPerfect: cols.IsPerfectReflection(x), Index: x));

        if (perfectReflectionC.Any(x => x.IsPerfect))
        {
            var index = perfectReflectionC
                .Where(x => x.IsPerfect)
                .Select(x => (-1, x.Index))
            .ToArray();

            indexes = indexes.Concat(index);
        }

        return indexes.ToArray();
    }

    static bool IsPerfectReflection<T>(this IEnumerable<IEnumerable<T>> source, int index)
    {
        for (int i = index, j = index + 1; i >= 0 && j < source.Count(); i--, j++)
        {
            if (source.ElementAt(i).SequenceEqual(source.ElementAt(j)) is false) return false;
        }

        return true;
    }

    static IEnumerable<int> RefectionIndexes<T>(this IEnumerable<IEnumerable<T>> seq)
    {
        var next = seq.Skip(1);

        var matchingIndex = next
            .Select((item, i) => item.SequenceEqual(seq.ElementAt(i)) ? i : -1)
            .Where(x => x != -1);

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

    static char Flip(char x) => x switch
    {
        '.' => '#',
        _ => '.'
    };

    static int CalculateReflections(this (int, int) index) => index switch
    {
        (var row, _) when row >= 0 => (row + 1) * 100,
        (_, var col) when col >= 0 => col + 1,
        _ => 0
    };
}
