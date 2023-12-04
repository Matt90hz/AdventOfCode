using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day3;
internal static class GearRatios
{
    public static int PartNumbers(string input)
    {
        var lines = input.Split('\n');

        int sum = 0;

        for(int l = 0; l < lines.Length; l++)
        {
            var pn = string.Empty;
            bool isPn = false;

            for(int c = 0; c < lines[l].Length; c++)
            {
                var curr = lines[l][c];

                if (char.IsDigit(curr))
                {
                    pn += curr;
                    isPn = isPn || (l, c) switch 
                    {
                        //first corner
                        (0, 0) =>
                            IsSymbol(lines[l][c + 1])
                            || IsSymbol(lines[l + 1][c])
                            || IsSymbol(lines[l + 1][c+1]),
                        //second corner
                        (0, var x) when x == lines[l].Length - 1 =>
                            IsSymbol(lines[l][c - 1])
                            || IsSymbol(lines[l + 1][c])
                            || IsSymbol(lines[l + 1][c - 1]),
                        //third corner
                        (var x, 0) when x == lines.Length - 1 =>
                            IsSymbol(lines[l][c + 1])
                            || IsSymbol(lines[l - 1][c])
                            || IsSymbol(lines[l - 1][c + 1]),
                        //fourth corner
                        (var x, var y) when x == lines.Length - 1 && y == lines[l].Length - 1 =>
                            IsSymbol(lines[l][c - 1])
                            || IsSymbol(lines[l - 1][c])
                            || IsSymbol(lines[l - 1][c - 1]),
                        //first line
                        (0, _) =>
                            IsSymbol(lines[l][c - 1])
                            || IsSymbol(lines[l][c + 1])
                            || IsSymbol(lines[l + 1][c - 1])
                            || IsSymbol(lines[l + 1][c])
                            || IsSymbol(lines[l + 1][c + 1]),
                        //last line
                        (var x, _) when x == lines.Length - 1 =>
                            IsSymbol(lines[l - 1][c - 1])
                            || IsSymbol(lines[l - 1][c])
                            || IsSymbol(lines[l - 1][c + 1])
                            || IsSymbol(lines[l][c - 1])
                            || IsSymbol(lines[l][c + 1]),
                        //last char
                        (_, var x) when x == lines[l].Length - 1 =>
                            IsSymbol(lines[l - 1][c - 1])
                            || IsSymbol(lines[l - 1][c])
                            || IsSymbol(lines[l][c - 1])
                            || IsSymbol(lines[l + 1][c - 1])
                            || IsSymbol(lines[l + 1][c]),
                        //first char
                        (_, 0) =>
                            IsSymbol(lines[l - 1][c])
                            || IsSymbol(lines[l - 1][c + 1])
                            || IsSymbol(lines[l][c + 1])
                            || IsSymbol(lines[l + 1][c])
                            || IsSymbol(lines[l + 1][c + 1]),
                        //else
                        (_, _) => 
                            IsSymbol(lines[l - 1][c - 1])
                            || IsSymbol(lines[l - 1][c])
                            || IsSymbol(lines[l - 1][c + 1])
                            || IsSymbol(lines[l][c - 1])
                            || IsSymbol(lines[l][c+1])
                            || IsSymbol(lines[l + 1][c - 1])
                            || IsSymbol(lines[l + 1][c])
                            || IsSymbol(lines[l + 1][c + 1]),

                    };

                    continue;
                }
                
                if (isPn)
                {
                    sum += int.Parse(pn);
                }

                isPn = false;
                pn = string.Empty;
            }
        }

        return sum;
    }

    public static int GearRatio(string input)
    {
        var lines = input.Split('\n');

        int ratios = 0;

        for(int l = 0; l < lines.Length; l++) 
        {
            for(int c = 0; c < lines[l].Length; c++)
            {
                if (lines[l][c] != '*') continue;

                int[] numAround = FindPartNumbersAround(l, c, lines);

                if (numAround.Length != 2) continue;

                ratios += numAround[0] * numAround[1];
            }
        }

        return ratios;
    }

    static int[] FindPartNumbersAround(int l, int c, string[] lines)
    {
        var partNumbersAround = new List<int>();
        var firstChar = 0;
        var lastChar = lines[0].Length - 1;
        var firstLine = 0;
        var lastLine = lines.Length - 1;

        //left      
        var isLeftDigit = c > firstChar && char.IsDigit(lines[l][c - 1]);
        
        if(isLeftDigit)
        {
            var pn = GetPartNumber(lines[l], c - 1);
            partNumbersAround.Add(pn);
        }

        //right
        var isRightDigit = c < lastChar &&  char.IsDigit((lines[l][c + 1]));

        if(isRightDigit)
        {
            var pn = GetPartNumber(lines[l], c + 1);
            partNumbersAround.Add(pn);
        }

        //top
        var isTopCenterDigit = l > firstLine && char.IsDigit(lines[l - 1][c]);

        if (isTopCenterDigit)
        {
            var pn = GetPartNumber(lines[l - 1], c);
            partNumbersAround.Add(pn);
        }

        var isTopLeftDigit = l > firstLine && c > firstChar && char.IsDigit(lines[l - 1][c - 1]);

        if (isTopLeftDigit && !isTopCenterDigit)
        {
            var pn = GetPartNumber(lines[l - 1], c - 1);
            partNumbersAround.Add(pn);
        }

        var isTopRightDigit = l > firstLine && c < lastChar && char.IsDigit(lines[l - 1][c + 1]);

        if (isTopRightDigit && !isTopCenterDigit)
        {
            var pn = GetPartNumber(lines[l - 1], c + 1);
            partNumbersAround.Add(pn);
        }

        //bottom
        var isBottomCenterDigit = l <lastLine  && char.IsDigit(lines[l + 1][c]);

        if (isBottomCenterDigit)
        {
            var pn = GetPartNumber(lines[l + 1], c);
            partNumbersAround.Add(pn);
        }

        var isBottomLeftDigit = l < lastLine && c > firstChar && char.IsDigit(lines[l + 1][c - 1]);

        if (isBottomLeftDigit && !isBottomCenterDigit)
        {
            var pn = GetPartNumber(lines[l + 1], c - 1);
            partNumbersAround.Add(pn);
        }

        var isBottomRightDigit = l < lastLine && c < lastChar && char.IsDigit(lines[l + 1][c + 1]);

        if (isBottomRightDigit && !isBottomCenterDigit)
        {
            var pn = GetPartNumber(lines[l + 1], c + 1);
            partNumbersAround.Add(pn);
        }

        return partNumbersAround.ToArray();
    }

    static int GetPartNumber(string line, int character)
    {
        var endOfLine = line.Length - 1;
        var rightPart = line[character..endOfLine];
        var leftPart = line[0..character];
        var right = rightPart.TakeWhile(char.IsDigit);
        var leftPartReverse = leftPart.Reverse();
        var leftReverse = leftPartReverse.TakeWhile(char.IsDigit);
        var left = leftReverse.Reverse();
        var digits = left.Concat(right).ToArray();

        return int.Parse(digits!);
    }

    static bool IsSymbol(char c) => c != '.' && c != '\r' && !char.IsDigit(c);
}
