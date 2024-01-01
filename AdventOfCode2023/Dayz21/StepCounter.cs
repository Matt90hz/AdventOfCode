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
        return 0;
    }

    public static int PlotsReachable(string input, int remaningSteps)
    {
        var garden = GetGarden(input);

        var positions = garden.GetPositions();

        var start = positions.First(pos => pos.Value == 'S');

        var reached =  new[] { start };

        while(remaningSteps > 0)
        {
            reached = Explore(reached);

            Task.Delay(500).Wait();
            Console.Clear();
            Console.WriteLine(Print(reached, garden));
            Console.WriteLine(reached.Length);
            remaningSteps--;

        }

        return reached.Length;
    }

    static string Print(Position<char>[] reached, char[,] garden) => garden
        .Select((x, pos) => reached.Any(r => (r.Row, r.Column) == pos) ? 'O' : x)
        .ToFriendlyString();    
    

    static Position<char>[] Explore(Position<char>[] reached) => reached
        .SelectMany(MoveAround)
        .DistinctBy(pos => (pos.Row, pos.Column))
        .ToArray();

    static IEnumerable<Position<char>> MoveAround(Position<char> position) => position
        .GetAdjecents()
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
