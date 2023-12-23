namespace AdventOfCode2023.Dayz16;

public static class TheFloorWillBeLavaTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz16\\input_test1.txt");
        var result = TheFloorWillBeLava.Energized(input);
        Assert.Equal(46, result);
    }

    [Fact]
    public static void Part1Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz16\\input.txt");
        var result = TheFloorWillBeLava.Energized(input);
        Assert.Equal(6740, result);
    }

    [Fact]
    public static void Part2Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz16\\input_test1.txt");
        var result = TheFloorWillBeLava.MaxEnergized(input);
        Assert.Equal(51, result);
    }

    [Fact]
    public static void Part2Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz16\\input.txt");
        var result = TheFloorWillBeLava.MaxEnergized(input);
        Assert.Equal(7041, result);
    }
}