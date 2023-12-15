using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz14;
public static class ParabolicReflectorDishTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz14\input_test1.txt");
        var result = ParabolicReflectorDish.LoadOnNorthSupportBeams(input);
        Assert.Equal(136, result);
    }

    [Fact]
    public static void Part1Solution()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz14\input.txt");
        var result = ParabolicReflectorDish.LoadOnNorthSupportBeams(input);
        Assert.Equal(108641, result);
    }

    [Fact]
    public static void Part2Test1()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz14\input_test1.txt");
        var result = ParabolicReflectorDish.LoadOnNorthSupportBeamsAfterAnInsaneAmountOfCycles(input);
        Assert.Equal(64, result);
    }

    [Fact]
    public static void Part2Solution()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz14\input.txt");
        var result = ParabolicReflectorDish.LoadOnNorthSupportBeamsAfterAnInsaneAmountOfCycles(input);
        Assert.Equal(84328, result);
    }
}
