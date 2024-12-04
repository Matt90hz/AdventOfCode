using IncaTechnologies.Collection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz21;


internal static class StepCounter
{
    public static long InfiniteGardenPlotReachable(string input, int remainigSteps)
    {
        var garden = GetGarden(input);

        var positions = garden.GetPositions();

        var start = positions.First(pos => pos.Value == 'S');

        var size = garden.GetLongLength(0) - 2;

        var diamondWidth = remainigSteps / size - 1;

        var oddTile = (long)Math.Pow(diamondWidth / 2 * 2 + 1, 2);
        var evenTile = (long)Math.Pow((diamondWidth + 1) / 2 * 2, 2);

        var oddReached = PlotsReachable(start, size * 2 + 1);
        var evenReached = PlotsReachable(start, size * 2);

        //corners
        var topCornerStartPosition = garden.GetPosition(size, start.Column);
        var topCornerReched = PlotsReachable(topCornerStartPosition, size - 1);

        var rightCornerStartPosition = garden.GetPosition(start.Row, 1);
        var rightCornerReched = PlotsReachable(rightCornerStartPosition, size - 1);

        var bottomCornerStartPosition = garden.GetPosition(1, start.Column);
        var bottomCornerReched = PlotsReachable(bottomCornerStartPosition, size - 1);

        var leftCornerStartPosition = garden.GetPosition(start.Row, size);
        var leftCornerReched = PlotsReachable(leftCornerStartPosition, size - 1);

        //small trinagles
        var smallTopRightStartPosition = garden.GetPosition(size, 1);
        var smallTopRightReched = PlotsReachable(smallTopRightStartPosition, size / 2 - 1);

        var smallTopLeftStartPosition = garden.GetPosition(size, size);
        var smallTopLeftReched = PlotsReachable(smallTopLeftStartPosition, size / 2 - 1);

        var smallBottomRightStartPosition = garden.GetPosition(1, 1);
        var smallBottomRightReched = PlotsReachable(smallBottomRightStartPosition, size / 2 - 1);

        var smallBottomLeftStartPosition = garden.GetPosition(1, size);
        var smallBottomLeftReched = PlotsReachable(smallBottomLeftStartPosition, size / 2 - 1);

        //big trinagles
        var bigTopRightStartPosition = garden.GetPosition(size, 1);
        var bigTopRightReched = PlotsReachable(bigTopRightStartPosition, size * 3 / 2 - 1);

        var bigTopLeftStartPosition = garden.GetPosition(size, size);
        var bigTopLeftReched = PlotsReachable(bigTopLeftStartPosition, size * 3 / 2 - 1);

        var bigBottomRightStartPosition = garden.GetPosition(1, 1);
        var bigBottomRightReched = PlotsReachable(bigBottomRightStartPosition, size * 3 / 2 - 1);

        var bigBottomLeftStartPosition = garden.GetPosition(1, size);
        var bigBottomLeftReched = PlotsReachable(bigBottomLeftStartPosition, size * 3 / 2 - 1);

        return
            oddTile * oddReached +
            evenTile * evenReached +
            topCornerReched + rightCornerReched + bottomCornerReched + leftCornerReched +
            (diamondWidth + 1) * (smallTopRightReched + smallTopLeftReched + smallBottomRightReched + smallBottomLeftReched) +
            diamondWidth * (bigTopRightReched + bigTopLeftReched + bigBottomRightReched + bigBottomLeftReched);
    }

    public static int PlotsReachable(string input, int remaningSteps)
    {
        var garden = GetGarden(input);

        var positions = garden.GetPositions();

        var start = positions.First(pos => pos.Value == 'S');

        return PlotsReachable(start, remaningSteps);
    }

    static int PlotsReachable(IPosition<char> start, long remaningSteps)
    {
        var reached = new[] { start };

        while (remaningSteps > 0)
        {
            reached = Explore(reached);
            remaningSteps--;
        }

        return reached.Length;
    }

    static string Print(IPosition<char>[] reached, char[,] garden) => garden
        .Select((x, pos) => reached.Any(r => (r.Row, r.Column) == pos) ? 'O' : x)
        .ToFriendlyString();


    static IPosition<char>[] Explore(IPosition<char>[] reached) => reached
        .SelectMany(MoveAround)
        .DistinctBy(pos => (pos.Row, pos.Column))
        .ToArray();

    static IEnumerable<IPosition<char>> MoveAround(IPosition<char> position) => position
        .GetAdjacent()
        .Where(pos => pos.Value == '.' || pos.Value == 'S');

    static char[,] GetGarden(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var garden = lines
            .ToMultidimensionalArray()
            .SurroundWith('@');

        return garden;
    }
}


//public static long InfiniteGardenPlotReachable(string input, int remainigSteps)
//{
//    var garden = GetGarden(input);

//    var positions = garden.GetPositions();

//    var start = positions.First(pos => pos.Value == 'S');

//    var size = garden.GetLongLength(0);

//    var diamondWidth = remainigSteps / size - 1;

//    var oddTile = (long)Math.Pow(diamondWidth / 2 * 2 + 1, 2);
//    var evenTile = (long)Math.Pow((diamondWidth + 1) / 2 * 2, 2);

//    var oddReached = PlotsReachable(start, size * 2 + 1);
//    var evenReached = PlotsReachable(start, size * 2);

//    //corners
//    var topCornerStartPosition = garden.GetPosition(size - 1, start.Column);
//    var topCornerReched = PlotsReachable(topCornerStartPosition, size - 1);

//    var rightCornerStartPosition = garden.GetPosition(start.Row, 0);
//    var rightCornerReched = PlotsReachable(rightCornerStartPosition, size - 1);

//    var bottomCornerStartPosition = garden.GetPosition(0, start.Column);
//    var bottomCornerReched = PlotsReachable(bottomCornerStartPosition, size - 1);

//    var leftCornerStartPosition = garden.GetPosition(start.Row, size - 1);
//    var leftCornerReched = PlotsReachable(leftCornerStartPosition, size - 1);

//    //small trinagles
//    var smallTopRightStartPosition = garden.GetPosition(size - 1, 0);
//    var smallTopRightReched = PlotsReachable(smallTopRightStartPosition, size / 2 - 1);

//    var smallTopLeftStartPosition = garden.GetPosition(size - 1, size - 1);
//    var smallTopLeftReched = PlotsReachable(smallTopLeftStartPosition, size / 2 - 1);

//    var smallBottomRightStartPosition = garden.GetPosition(0, 0);
//    var smallBottomRightReched = PlotsReachable(smallBottomRightStartPosition, size / 2 - 1);

//    var smallBottomLeftStartPosition = garden.GetPosition(0, size - 1);
//    var smallBottomLeftReched = PlotsReachable(smallBottomLeftStartPosition, size / 2 - 1);

//    //big trinagles
//    var bigTopRightStartPosition = garden.GetPosition(size - 1, 0);
//    var bigTopRightReched = PlotsReachable(bigTopRightStartPosition, size * 3 / 2 - 1);

//    var bigTopLeftStartPosition = garden.GetPosition(size - 1, size - 1);
//    var bigTopLeftReched = PlotsReachable(bigTopLeftStartPosition, size * 3 / 2 - 1);

//    var bigBottomRightStartPosition = garden.GetPosition(0, 0);
//    var bigBottomRightReched = PlotsReachable(bigBottomRightStartPosition, size * 3 / 2 - 1);

//    var bigBottomLeftStartPosition = garden.GetPosition(0, size - 1);
//    var bigBottomLeftReched = PlotsReachable(bigBottomLeftStartPosition, size * 3 / 2 - 1);

//    return
//        oddTile * oddReached +
//        evenTile * evenReached +
//        topCornerReched + rightCornerReched + bottomCornerReched + leftCornerReched +
//        (diamondWidth + 1) * (smallTopRightReched + smallTopLeftReched + smallBottomRightReched + smallBottomLeftReched) +
//        diamondWidth * (bigTopRightReched + bigTopLeftReched + bigBottomRightReched + bigBottomLeftReched);
//}