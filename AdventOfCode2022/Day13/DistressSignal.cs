using System.Text;

namespace AdventOfCode2022.Day13;
public static class DistressSignal
{
    public const string DIVIDER_1 = "[[6]]";
    public const string DIVIDER_2 = "[[2]]";

    public static int GetDecoderKey(string input)
    {
        var packets = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Append(DIVIDER_1)
            .Append(DIVIDER_2);

        var packetsComparer = Comparer<string>.Create((string left, string right) =>
        {
            bool? isOrdered = IsOrdered(left, right);

            return isOrdered switch
            {
                true => -1,
                false => 1,
                _ => 0
            };
        });

        var orderedPackets = packets
            .Order(packetsComparer)
            .Select((x, i) => (Index: ++i, Packet: x));
        
        var markerPackets = orderedPackets
            .Where(x => x.Packet is DIVIDER_1 or DIVIDER_2);

        var decoderKey = markerPackets
            .Aggregate(1, (result, x) => result *= x.Index);

        return decoderKey;
    }

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

                bool? isOrdered = IsOrdered(left, right);

                return isOrdered ?? true;
            });

        var sumOfOrederePairs = orderdPackets.Sum(x => x.Index);

        return sumOfOrederePairs;
    }

    static bool? IsOrdered(string left, string right, int compare = 0)
    {
        string leftElement = left.GetElement(compare);
        string rightElement = right.GetElement(compare);

        bool isLeftEmpty = string.IsNullOrEmpty(leftElement);
        bool isRightEmpty = string.IsNullOrEmpty(rightElement);

        if (isLeftEmpty && isRightEmpty) return null;
        
        if (isLeftEmpty) return true;     

        if (isRightEmpty) return false;
        
        bool isLeftElementNumber = leftElement.IsNumber();
        bool isRightElementNumber = rightElement.IsNumber();

        if (isLeftElementNumber && isRightElementNumber)
        {
            int leftNumber = int.Parse(leftElement);
            int rightNumber = int.Parse(rightElement);

            return leftNumber == rightNumber 
                ? IsOrdered(left, right, ++compare) 
                : leftNumber < rightNumber;
        }

        if (isLeftElementNumber)
        {
            leftElement = leftElement.ToListElement();
        }

        if (isRightElementNumber)
        {
            rightElement = rightElement.ToListElement();
        }

        bool? isOrdered = IsOrdered(leftElement, rightElement) ?? IsOrdered(left, right, ++compare);

        return isOrdered;
    }

    static string GetElement(this string x, int index)
    {
        if (x[0] != '[' || x[^1] != ']') throw new ArgumentException(x);

        string scope = x[1..^1];

        int start = 0;
        int indentation = 0;
        int found = 0;

        for (int i = 0; i < scope.Length; i++)
        {
            char c = scope[i];

            if (c == '[') indentation++;        

            if (c == ']') indentation--;          

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

        return found == index ? scope[start..] : string.Empty;
    }

    static string ToListElement(this string x) => $"[{x}]";

    static bool IsNumber(this string x) => int.TryParse(x, out _);
}