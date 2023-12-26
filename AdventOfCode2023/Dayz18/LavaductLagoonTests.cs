namespace AdventOfCode2023.Dayz18;

public static class LavaductLagoonTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz18\\input_test1.txt");
        var result = LavaductLagoon.Capacity(input);
        Assert.Equal(62, result);
    }

    [Fact]
    public static void Part1Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz18\\input.txt");
        var result = LavaductLagoon.Capacity(input);
        Assert.Equal(49897, result);
    }

    [Fact]
    public static void Part2Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz18\\input_test1.txt");
        var result = LavaductLagoon.CorrectCapacity(input);
        Assert.Equal(952408144115, result);
    }

    [Fact]
    public static void Part2Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz18\\input.txt");
        var result = LavaductLagoon.CorrectCapacity(input);
        Assert.Equal(194033958221830, result);
    }
}