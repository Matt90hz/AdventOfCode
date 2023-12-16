using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz15;
public static class LensLibraryTest
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz15\input_test1.txt");
        var result = LensLibrary.HashSum(input);
        Assert.Equal(1320, result);
    }

    [Fact]
    public static void Part1Solution()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz15\input.txt");
        var result = LensLibrary.HashSum(input);
        Assert.Equal(511416, result);
    }

    [Fact]
    public static void Part2Test1()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz15\input_test1.txt");
        var result = LensLibrary.FocusingPower(input);
        Assert.Equal(145, result);
    }

    [Fact]
    public static void Part2Solution()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz15\input.txt");
        var result = LensLibrary.FocusingPower(input);
        Assert.Equal(290779, result);
    }
}
