using IncaTechnologies.Collection.Extensions;
using System.Data;
using System.Text;

namespace AdventOfCode2022.Day14;
public static class RegolithReservoir
{
    const char ROCK = '#';
    const char SAND = '+';
    const char AIR = '.';
    const char SET_SAND = 'O';
    const char FREE_FLOW = '~';
    const int INLET_ROW = 0;
    const int INLET_COL = 500;

    public static int MaxSendToRest(string input)
    {
        var rocksCoordinates = input.GetRocksCoordinates();
        var caveSystem = rocksCoordinates.DrawCaveSystem();
        var position = caveSystem.GetPosition(INLET_ROW, INLET_COL);

        while (true)
        {
            bool isNotAbyss = position.TryMoveDown(out var next);

            if (isNotAbyss is false) break;

            if (next.Value is AIR)
            {
                position = next;
                continue;
            }

            if (next.TryMoveLeft(out var left) && left.Value is AIR)
            {
                position = left;
                continue;
            }

            if (next.TryMoveRight(out var right) && right.Value is AIR)
            {
                position = right;
                continue;
            }

            position.Value = SET_SAND;
            position = caveSystem.GetPosition(INLET_ROW, INLET_COL);
        }

        var settledSands = caveSystem.Count(x => x is SET_SAND);

        return settledSands;
    }

    public static int CountSandToBlockInlet(string input)
    {
        var rocksCoordinates = input.GetRocksCoordinates();

        int floorRow = rocksCoordinates.SelectMany(x => x).Max(x => x.Row) + 2;

        var rocksCoordinatesWithFloor = rocksCoordinates
            .Append([(floorRow, 0), (floorRow, 1000)])
            .ToArray();

        var caveSystem = rocksCoordinatesWithFloor.DrawCaveSystem();

        int row = INLET_ROW;
        int col = INLET_COL;

        while (true)
        {
            if (row == INLET_ROW && col == INLET_COL && caveSystem[row, col] != AIR) break;

            int nextRow = row + 1;
            int nextCol = col;

            if (caveSystem[nextRow, nextCol] is AIR)
            {
                row = nextRow;
                col = nextCol;
                continue;
            }

            nextCol = col - 1;

            if (caveSystem[nextRow, nextCol] is AIR)
            {
                row = nextRow;
                col = nextCol;
                continue;
            }

            nextCol = col + 1;

            if (caveSystem[nextRow, nextCol] is AIR)
            {
                row = nextRow;
                col = nextCol;
                continue;
            }

            caveSystem[row, col] = SET_SAND;

            row = INLET_ROW;
            col = INLET_COL;
        }

        var settledSands = caveSystem.Count(x => x is SET_SAND);

        return settledSands;
    }

    static (int Row, int Col)[][] GetRocksCoordinates(this string input) => input
        .Replace(" ", string.Empty)
        .Split(Environment.NewLine)
        .Select(line => line
            .Split("->")
            .Select(coordinate =>
            {
                var x = coordinate.Split(',');
                var row = int.Parse(x[1]);
                var col = int.Parse(x[0]);
                return (Row: row, Col: col);
            })
            .ToArray())
        .ToArray();

    static char[,] DrawCaveSystem(this (int Row, int Col)[][] rocksCoordinates)
    {
        int maxRow = rocksCoordinates.SelectMany(x => x).Max(x => x.Row);
        int maxCol = rocksCoordinates.SelectMany(x => x).Max(x => x.Col);

        var caveSystem = new char[maxRow + 1, maxCol + 1].ForEach(_ => AIR);

        var coordinateCouples = rocksCoordinates
            .Select(coordinates => coordinates
                .Zip(coordinates.Skip(1))
                .ToArray())
            .ToArray();

        foreach (var coordinateCoupleLine in coordinateCouples)
        {
            foreach (var (start, end) in coordinateCoupleLine)
            {
                var (startRow, startCol) = start;
                var (endRow, endCol) = end;

                int i = Math.Min(startRow, endRow);
                int j = Math.Min(startCol, endCol);
                int countRow = Math.Abs(startRow - endRow) + i;
                int countCol = Math.Abs(startCol - endCol) + j;

                for (; i <= countRow; i++)
                {
                    for (int k = j; k <= countCol; k++)
                    {
                        caveSystem[i, k] = ROCK;
                    }
                }
            }
        }

        return caveSystem;
    }
}
