using IncaTechnologies.Collection.Extensions;

namespace AdventOfCode2023.Dayz21;

internal static class StepCounter
{
    public static long InfiniteGardenPlotReachable(string input, int remainingSteps)
    {
        var garden = GetGarden(input);
        var positions = garden.GetPositions();
        var start = positions.First(pos => pos.Value == 'S');
        var size = garden.GetLongLength(0) - 2;
        var diamondWidth = remainingSteps / size - 1;

        var oddTile = (long)Math.Pow(diamondWidth / 2 * 2 + 1, 2);
        var evenTile = (long)Math.Pow((diamondWidth + 1) / 2 * 2, 2);

        var oddReached = PlotsReachable(start, size * 2 + 1);
        var evenReached = PlotsReachable(start, size * 2);

        //corners
        var topCornerStartPosition = garden.GetPosition(size, start.Column);
        var topCornerReached = PlotsReachable(topCornerStartPosition, size - 1);

        var rightCornerStartPosition = garden.GetPosition(start.Row, 1);
        var rightCornerReached = PlotsReachable(rightCornerStartPosition, size - 1);

        var bottomCornerStartPosition = garden.GetPosition(1, start.Column);
        var bottomCornerReached = PlotsReachable(bottomCornerStartPosition, size - 1);

        var leftCornerStartPosition = garden.GetPosition(start.Row, size);
        var leftCornerReached = PlotsReachable(leftCornerStartPosition, size - 1);

        //small triangles
        var smallTopRightStartPosition = garden.GetPosition(size, 1);
        var smallTopRightReached = PlotsReachable(smallTopRightStartPosition, size / 2 - 1);

        var smallTopLeftStartPosition = garden.GetPosition(size, size);
        var smallTopLeftReached = PlotsReachable(smallTopLeftStartPosition, size / 2 - 1);

        var smallBottomRightStartPosition = garden.GetPosition(1, 1);
        var smallBottomRightReached = PlotsReachable(smallBottomRightStartPosition, size / 2 - 1);

        var smallBottomLeftStartPosition = garden.GetPosition(1, size);
        var smallBottomLeftReached = PlotsReachable(smallBottomLeftStartPosition, size / 2 - 1);

        //big triangles
        var bigTopRightStartPosition = garden.GetPosition(size, 1);
        var bigTopRightReached = PlotsReachable(bigTopRightStartPosition, size * 3 / 2 - 1);

        var bigTopLeftStartPosition = garden.GetPosition(size, size);
        var bigTopLeftReached = PlotsReachable(bigTopLeftStartPosition, size * 3 / 2 - 1);

        var bigBottomRightStartPosition = garden.GetPosition(1, 1);
        var bigBottomRightReached = PlotsReachable(bigBottomRightStartPosition, size * 3 / 2 - 1);

        var bigBottomLeftStartPosition = garden.GetPosition(1, size);
        var bigBottomLeftReached = PlotsReachable(bigBottomLeftStartPosition, size * 3 / 2 - 1);

        return
            oddTile * oddReached +
            evenTile * evenReached +
            topCornerReached + rightCornerReached + bottomCornerReached + leftCornerReached +
            (diamondWidth + 1) * (smallTopRightReached + smallTopLeftReached + smallBottomRightReached + smallBottomLeftReached) +
            diamondWidth * (bigTopRightReached + bigTopLeftReached + bigBottomRightReached + bigBottomLeftReached);
    }

    public static int PlotsReachable(string input, int remainingSteps)
    {
        var garden = GetGarden(input);
        var positions = garden.GetPositions();
        var start = positions.First(pos => pos.Value == 'S');
        return PlotsReachable(start, remainingSteps);
    }

    static int PlotsReachable(Position<char> start, long remainingSteps)
    {
        var parity = true;
        var reached = new Dictionary<Position<char>, bool>() { { start, parity } };
        var todo = new Position<char>[] { start };

        while (remainingSteps > 0)
        {
            parity = !parity;

            todo = todo
                .SelectMany(position => position
                    .GetAdjacent()
                    .Where(a => a is { Value: '.' or 'S' } && reached.TryAdd(a, parity)))
                .ToArray();

            remainingSteps--;
        }

        return reached.Where(x => x.Value == parity).Count();     
    }

    static char[,] GetGarden(string input)
    {
        var garden = input
            .Split(Environment.NewLine)
            .ToMultidimensionalArray()
            .SurroundWith('@');

        return garden;
    }
}