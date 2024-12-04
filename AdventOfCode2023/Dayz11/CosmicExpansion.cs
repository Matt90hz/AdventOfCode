using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncaTechnologies.Collection.Extensions;

namespace AdventOfCode2023.Dayz11;
internal static class CosmicExpansion
{
    const char VOID = '.';
    const char SPACE = 'S';
    const char GALAXY = '#';

    public static long MegaCosmicDistances(string[] input, long expansion)
    {
        var map = input.Select(x => x.Select(x => x)).ToMultidimensionalArray();

        var rowExpansion = map
            .GetRows()
            .Select((x, i) => x.Contains('#') ? 1L : expansion)
            .ToArray();

        var colExpansion = map
            .GetColumns()
            .Select((x, i) => x.Contains('#') ? 1L : expansion)
            .ToArray();

        var galaxyPositions = map
            .GetPositions()
            .Where(x => x.Value == GALAXY);

        var distances = galaxyPositions.SelectMany((x, i) => galaxyPositions.Skip(i + 1).Select(y =>
        {
            var xr = (int)x.Row;
            var yr = (int)y.Row;

            var rowDistance = Enumerable
                .Range(xr < yr ? xr : yr, int.Abs(xr - yr))
                .Sum(x => rowExpansion[x]);

            var xc = (int)x.Column;
            var yc = (int)y.Column;

            var colDistance = Enumerable
                .Range(xc < yc ? xc : yc, int.Abs(xc - yc))
                .Sum(x => colExpansion[x]);

            return rowDistance + colDistance;
        }));

        var distance = distances.Sum();

        return distance;
    }

    public static long CosmicDistances(string[] input)
    {
        var map = input.Select(x => x.Select(x => x)).ToMultidimensionalArray();

        var expandedMap = map.Expand();

        var galaxyPositions = expandedMap
            .GetPositions()
            .Where(x => x.Value == GALAXY)
            .ToArray();

        var distances = galaxyPositions
            .SelectMany((x, i) => galaxyPositions
                .Skip(i + 1)
                .Select(y => long.Abs(x.Row - y.Row) + long.Abs(x.Column - y.Column)));

        long distance = distances.Sum();

        return distance;
    }

    public static char[,] Expand(this char[,] map)
    {
        var rowsToExpand = map
            .GetRows()
            .Select((x, i) => x.Contains('#') ? -1 : i)
            .Where(x => x >= 0)
            .Reverse();

        var colsToExpand = map
            .GetColumns()
            .Select((x, i) => x.Contains('#') ? -1 : i)
            .Where(x => x >= 0)
            .Reverse();

        var rowToAdd = Enumerable
            .Range(0, map.GetLength(1))
            .Select(_ => '.')
            .ToArray();

        var expandRows = rowsToExpand.Aggregate(map, (a, x) => a.AddRow(rowToAdd, x));

        var colToAdd = Enumerable
            .Range(0, expandRows.GetLength(0))
            .Select(_ => '.')
            .ToArray();

        var expandCols = colsToExpand.Aggregate(expandRows, (a, x) => a.AddColumn(colToAdd, x));

        return expandCols;
    }
}
