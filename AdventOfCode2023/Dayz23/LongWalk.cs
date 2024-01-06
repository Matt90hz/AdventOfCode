using IncaTechnologies.Collection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz23;
internal static class LongWalk
{
    public static int LongestHike(string input)
    {
        char[,] island = GetIsland(input);

        var hikingPaths = GetHikingPaths(island);

        var longestPath = hikingPaths.MinBy(path => path.Count());

        return 0;
    }

    static IEnumerable<IEnumerable<IPosition<char>>> GetHikingPaths(char[,] island)
    {
        var startPosition = island.GetPosition(0, 1);

        List<IEnumerable<IPosition<char>>> hikingPathsToExplore = new() 
        { 
            new[] { startPosition }
        };

        List<IEnumerable<IPosition<char>>> hikingPathsExplored = new();

        while (hikingPathsToExplore.Any())
        {
            var pathExplored = Explore(hikingPathsToExplore.First());

        }

        return hikingPathsToExplore;
    }

    static IEnumerable<IPosition<char>> Explore(IEnumerable<IPosition<char>> enumerable)
    {
        throw new NotImplementedException();
    }

    static char[,] GetIsland(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var island = lines.ToMultidimensionalArray();

        return island;
    }
}
