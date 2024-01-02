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
    public static void Part2Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz21\\input_test1.txt");
        var result = StepCounter.InfiniteGardenPlotReachable(input, 6);

        Assert.Equal(16, result);
    }

    [Fact]
    public static void Part2Test2()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz21\\input_test1.txt");
        var result = StepCounter.InfiniteGardenPlotReachable(input, 10);

        Assert.Equal(50, result);
    }

    [Fact]
    public static void Part2Test3()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz21\\input_test1.txt");
        var result = StepCounter.InfiniteGardenPlotReachable(input, 50);

        Assert.Equal(1594, result);
    }

    [Fact]
    public static void Part2Test4()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz21\\input_test1.txt");
        var result = StepCounter.InfiniteGardenPlotReachable(input, 100);

        Assert.Equal(6536, result);
    }

    [Fact]
    public static void Part2Test5()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz21\\input_test1.txt");
        var result = StepCounter.InfiniteGardenPlotReachable(input, 500);

        Assert.Equal(167004, result);
    }

    [Fact]
    public static void Part2Test6()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz21\\input_test1.txt");
        var result = StepCounter.InfiniteGardenPlotReachable(input, 1000);

        Assert.Equal(668697, result);
    }

    [Fact]
    public static void Part2Test7()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz21\\input_test1.txt");
        var result = StepCounter.InfiniteGardenPlotReachable(input, 5000);

        Assert.Equal(16733044, result);
    }

    [Fact]
    public static void Part2Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz21\\input.txt");
        var result = StepCounter.InfiniteGardenPlotReachable(input, 26501365);

        Assert.Equal(600336060511101, result);
    }
}