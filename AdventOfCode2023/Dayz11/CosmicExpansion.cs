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

        var galaxyPositions = map
            .GetPositions()
            .Where(x => x.Value == GALAXY);

        var distances = galaxyPositions
            .SelectMany((x, i) =>
                x.DistancesFrom(galaxyPositions.Skip(i + 1), expansion));

        var distance = distances.Sum();

        return distance;
    }

    public static long CosmicDistances(string[] input)
    {
        var map = input.Select(x => x.Select(x => x)).ToMultidimensionalArray();

        var galaxyPositions = map
            .GetPositions()
            .Where(x => x.Value == GALAXY);

        var distances = galaxyPositions
            .SelectMany((x, i) =>
                x.DistancesFrom(galaxyPositions.Skip(i + 1), 2));

        var distance = distances.Sum();

        return distance;
    }

    static IEnumerable<long> DistancesFrom(this Position<char> @this, IEnumerable<Position<char>> otherGalaxies, long expansion) =>
        otherGalaxies.Select(x => x.DistanceFrom(@this, expansion));

    static long DistanceFrom(this Position<char> @this, Position<char> otherGalaxy, long expansion)
    {
        var (rowStart, rowEnd) = @this.Row <= otherGalaxy.Row 
            ? (@this.Row, otherGalaxy.Row) 
            : (otherGalaxy.Row, @this.Row);

        var (colStart, colEnd) = @this.Column <= otherGalaxy.Column
            ? (@this.Column, otherGalaxy.Column)
            : (otherGalaxy.Column, @this.Column);

        var dRow = Math.Abs(@this.Row - otherGalaxy.Row);
        var dCol = Math.Abs(@this.Column - otherGalaxy.Column);

        var cols = @this.Array.GetColumns().Where((column, i) => colStart < i && colEnd > i);
        var rows = @this.Array.GetRows().Where((row, i) => rowStart < i && rowEnd > i);

        var emptySpaceRows = rows
            .Where(x => x.All(x => x == VOID))
            .Count();

        var emptySpaceCols = cols
            .Where(x => x.All(x => x == VOID))
            .Count();

        var distance = 
            dRow - emptySpaceRows + (emptySpaceRows * expansion) +
            dCol - emptySpaceCols + (emptySpaceCols * expansion);

        return distance;
    }

    static long DistanceFrom(this Position<char> @this, Position<char> otherGalaxy)
    {
        var dRow = Math.Abs(@this.Row - otherGalaxy.Row);
        var dCol = Math.Abs(@this.Column - otherGalaxy.Column);

        var distance = dRow + dCol;

        return distance;
    }

    static void FillWithSpace(this char[,] map) 
    { 

        for(long i = 0; i < map.GetLength(0); i++)
        {
            if (map.GetRow(i).All(x => x == VOID))
            {
                for (long j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = SPACE;
                }
            }          
        }

        for (long j = 0; j < map.GetLength(1); j++)
        {
            if (map.GetColumn(j).All(x => x == VOID || x == SPACE))
            {
                for (long i = 0; i < map.GetLength(0); i++)
                {
                    map[i, j] = SPACE;
                }
            }
        }
    }

    static char[,] ExpandRows(this char[,] map)
    {
        var expanded = map;

        for(int i = expanded.GetLength(0) - 1; i > 0; i--)
        {
            var row = expanded.GetRow(i);

            if(row.All(x => x == VOID))
            {
                expanded = expanded.AddRow(row, i);
            }
        }

        return expanded;
    }

    static char[,] ExpandColumns(this char[,] map)
    {
        var expanded = map;

        for (int i = expanded.GetLength(1) - 1; i > 0; i--)
        {
            var col = expanded.GetColumn(i);

            if (col.All(x => x == VOID))
            {
                expanded = expanded.AddColumn(col, i);
            }
        }

        return expanded;
    }
}
