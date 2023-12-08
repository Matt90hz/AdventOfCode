using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace AdventOfCode2023.Day8;

record Node(string Left, string Right);

class Direction
{
    private readonly int _index;
    private readonly string _directions;

    public char This => _directions[_index];

    public Direction Next => _index < _directions.Length - 1
        ? new(_index + 1, _directions)
        : new(0, _directions);

    public Direction(int index, string directions)
    {
        _index = index;
        _directions = directions;
    }
}

internal static class HauntedWasteland
{
    public static int HowFarTheExit(string input)
    {
        var direction = FirstDirection(input);
        var nodes = GetNodes(input);

        var curr = "AAA";
        int steps = 0;

        while (curr != "ZZZ")
        {
            var node = direction.This == 'L'
                ? nodes[curr].Left
                : nodes[curr].Right;

            curr = node;
            direction = direction.Next;
            steps++;
        }

        return steps;
    }

    public static long HowFarTheExitForGhosts(string input)
    {
        var direction = FirstDirection(input);
        var nodes = GetNodes(input);

        var nodesAtoZ = nodes
            .Where(x => x.Key[^1] == 'A')
            .Select(x => CountGhostStep(x.Key, direction, nodes));

        var aggregate = nodesAtoZ
            .Skip(1)
            .Aggregate(nodesAtoZ.First(), LeastCommonMultiple);

        return aggregate;

    }

    static long CountGhostStep(string from, Direction direction, IDictionary<string, Node> nodes)
    {
        var curr = from;
        long steps = 0;

        while (curr is not [.., 'Z'])
        {
            var node = direction.This == 'L'
                ? nodes[curr].Left
                : nodes[curr].Right;

            curr = node;
            direction = direction.Next;
            steps++;
        }

        return steps;
    }

    static long LeastCommonMultiple(long x, long y)
    {
        long num1, num2;

        if (x > y)
        {
            num1 = x; 
            num2 = y;
        }
        else
        {
            num1 = y; 
            num2 = x;
        }

        for (long i = 1; i < num2; i++)
        {
            long mult = num1 * i;
            if (mult % num2 == 0)
            {
                return mult;
            }
        }
        return num1 * num2;
    }

    static Direction FirstDirection(string input)
    {
        var directionsString = input.Split(Environment.NewLine)[0];

        return new Direction(0, directionsString);
    }

    static IDictionary<string, Node> GetNodes(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)[1..];

        var nodes = lines
            .Select(GetNode)
            .ToDictionary(x => x.This, x => x.Node);

        return nodes;
    }

    static (string This, Node Node) GetNode(string line)
    {
        var @this = line[0..3];
        var right = line[7..10];
        var left = line[12..15];

        return (@this, new(right, left));

    }
}
