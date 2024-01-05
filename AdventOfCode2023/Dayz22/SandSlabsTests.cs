namespace AdventOfCode2023.Dayz22;

public static class SandSlabsTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz22\\input_test1.txt");
        var result = SandSlabs.BricksSafelyDisintegrable(input);

        Assert.Equal(5, result);
    }

    [Fact]
    public static void Part1Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz22\\input.txt");
        var result = SandSlabs.BricksSafelyDisintegrable(input);

        Assert.Equal(393, result);
    }

    [Fact]
    public static void Part2Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz22\\input_test1.txt");
        var result = SandSlabs.ChainReaction(input);

        Assert.Equal(7, result);
    }

    [Fact]
    public static void Part2Test2()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz22\\input_test2.txt");
        var result = SandSlabs.ChainReaction(input);

        Assert.Equal(4, result);
    }

    [Fact]
    public static void Part2Test3()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz22\\input_test3.txt");
        var result = SandSlabs.ChainReaction(input);

        Assert.Equal(3, result);
    }

    [Fact]
    public static void Part2Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz22\\input.txt");
        var result = SandSlabs.ChainReaction(input);

        Assert.Equal(58440, result);
    }
}
