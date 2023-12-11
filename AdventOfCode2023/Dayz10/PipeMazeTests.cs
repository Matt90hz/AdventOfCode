using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz10;
public static class PipeMazeTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllLines("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz10\\input_test1.txt");
        var result = PipeMaze.Depth(input);
        Assert.Equal(4, result);
    }

    [Fact]
    public static void Part1Test2() 
    {
        var input = File.ReadAllLines("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz10\\input_test2.txt");
        var result = PipeMaze.Depth(input);
        Assert.Equal(8, result);
    }

    [Fact]
    public static void Part1Solution()
    {
        var input = File.ReadAllLines("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz10\\input.txt");
        var result = PipeMaze.Depth(input);
        Assert.Equal(6867, result);
    }

    [Fact]
    public static void Part2Test1()
    {
        var input = File.ReadAllLines("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz10\\input_test3.txt");
        var result = PipeMaze.AnimalDens(input);
        Assert.Equal(10, result);
    }

    [Fact]
    public static void Part2Test2()
    {
        var input = File.ReadAllLines("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz10\\input_test4.txt");
        var result = PipeMaze.AnimalDens(input);
        Assert.Equal(4, result);
    }

    [Fact]
    public static void Part2Solution()
    {
        var input = File.ReadAllLines("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz10\\input.txt");
        var result = PipeMaze.AnimalDens(input);
        Assert.Equal(595, result);
    }

}
