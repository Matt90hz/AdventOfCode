using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncaTechnologies.Collection.Extensions;

namespace AdventOfCode2022.Day08;
static class TreetopTreeHouse
{
    public static int CountTreesVisible(string input) => input
        .Split(Environment.NewLine)
        .ToMultidimensionalArray()
        .Select(c => c - 48)
        .GetPositions()
        .Select(position => position.IsVisible())
        .Count(isVisible => isVisible is true);

    static bool IsVisible(this IPosition<int> tree) => new[] 
    { 
        tree.GetNorth(),
        tree.GetSouth(),
        tree.GetWest(),
        tree.GetEast(),
    }
        .Select(otherTrees => otherTrees.All(otherTree => tree.Value > otherTree.Value))
        .Any(isTallest => isTallest);
}
