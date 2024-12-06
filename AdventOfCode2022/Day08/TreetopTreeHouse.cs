using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using IncaTechnologies.Collection.Extensions;
using Xunit;

namespace AdventOfCode2022.Day08;
static class TreetopTreeHouse
{
    public static int CountTreesVisible(string input) => input
        .Split(Environment.NewLine)
        .ToMultidimensionalArray()
        .Select(c => c - '0')
        .GetPositions()
        .Count(position => position.IsVisible());

    public static int GetBestViewScore(string input) => input
        .Split(Environment.NewLine)
        .ToMultidimensionalArray()
        .Select(c => c - '0')
        .GetPositions()
        .Max(position => position.GetViewScore());

    static int GetViewScore(this Position<int> tree) => new[] { tree.GetNorth(), tree.GetSouth(), tree.GetWest(), tree.GetEast() }
        .Select(otherTrees => otherTrees
            .TakeWhile(otherTree => otherTree.IsBorder() is false && otherTree.Value < tree.Value)
            .Count() + 1)
        .Aggregate((total, score) => total *= score);

    static bool IsVisible(this Position<int> tree) => new[] { tree.GetNorth(), tree.GetSouth(), tree.GetWest(), tree.GetEast() }
        .Any(otherTrees => otherTrees.All(otherTree => otherTree.Value < tree.Value));
}


