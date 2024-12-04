namespace AdventOfCode2023.Dayz21;

public static class StepCounterTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz21\\input_test1.txt");
        var result = StepCounter.PlotsReachable(input, 6);

        Assert.Equal(16, result);
    }

    [Fact]
    public static void Part1Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz21\\input.txt");
        var result = StepCounter.PlotsReachable(input, 64);

        Assert.Equal(3632, result);
    }

    [Fact]
    public static void Part2Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz21\\input.txt");
        var result = StepCounter.InfiniteGardenPlotReachable(input, 26501365);

        Assert.Equal(600336060511101, result);
    }
}