using AdventOfCode2022.Day07;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day13;
public static class DistressSignal
{
    //6451 too high
    //5981 too high
    //5754 too high
    //5605 ok

    public static int SumOrderedPairIndex(string input)
    {
        var packets = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Chunk(2)
            .Select((x, i) => (Index: ++i, Left: x[0], Right: x[1]));

        var orderdPackets = packets
            .Where(x =>
            {
                var (_, left, right) = x;
                var isOrdered = IsOrdered(left, right);

                return isOrdered ?? true;
            });

        var sumOfOrederePairs = orderdPackets.Sum(x => x.Index);

        return sumOfOrederePairs;
    }

    static bool? IsOrdered(string left, string right, int compare = 0)
    {
        string leftFirstElement = left.GetElement(compare);
        string rightFirstElement = right.GetElement(compare);

        if (string.IsNullOrEmpty(leftFirstElement) && string.IsNullOrEmpty(rightFirstElement))
            return null;

        if (string.IsNullOrEmpty(leftFirstElement)) 
            return true;

        if (string.IsNullOrEmpty(rightFirstElement)) 
            return false;

        if (leftFirstElement.IsNumber() && rightFirstElement.IsNumber())
        {
            int leftNumber = int.Parse(leftFirstElement);
            int rightNumber = int.Parse(rightFirstElement);

            if (leftNumber == rightNumber)
            {
                compare++;
                return IsOrdered(left, right, compare);
            }

            return leftNumber < rightNumber;
        }

        if (leftFirstElement.IsNumber() && rightFirstElement.IsList())
        {
            leftFirstElement = leftFirstElement.ToListElement();
        }

        if (rightFirstElement.IsNumber() && leftFirstElement.IsList())
        {
            rightFirstElement = rightFirstElement.ToListElement();
        }

        bool? areListOrdered = IsOrdered(leftFirstElement, rightFirstElement);

        if (areListOrdered is null)
        {
            compare++;
            return IsOrdered(left, right, compare);
        }

        return areListOrdered;
    }

    static string GetElement(this string x, int index)
    {
        if (x[0] != '[') 
            throw new ArgumentException();
        if (x[^1] != ']') 
            throw new ArgumentException();

        string scope = x[1..^1] + ",";

        int start = 0;
        int indentation = 0;
        int found = 0;

        for (int i = 0; i < scope.Length; i++)
        {
            char c = scope[i];

            if (c == '[')
            {
                indentation++;
            }

            if (c == ']')
            {
                indentation--;
            }

            if (c == ',' && indentation == 0)
            {
                if (found == index)
                {
                    string element = scope[start..i];

                    return element;
                }

                found++;
                start = i + 1;
            }
        }

        return string.Empty;
    }

    static string ToListElement(this string x) => $"[{x}]";

    static bool IsNumber(this string x) => int.TryParse(x, out _);

    static bool IsList(this string x) => x is { Length: > 0 } && x[0] == '[';
}