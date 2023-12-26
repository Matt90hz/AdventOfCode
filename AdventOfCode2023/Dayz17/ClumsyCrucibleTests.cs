namespace AdventOfCode2023.Dayz17;

public static class ClumsyCrucibleTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz17\\input_test1.txt");
        var result = ClumsyCrucible.HeatLoss(input);
        Assert.Equal(102, result);
    }

    [Fact]
    public static void Part1Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz17\\input.txt");
        var result = ClumsyCrucible.HeatLoss(input);
        Assert.Equal(771, result);
    }

    [Fact]
    public static void Part2Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz17\\input_test1.txt");
        var result = ClumsyCrucible.UltraHeatLoss(input);
        Assert.Equal(94, result);
    }

    [Fact]
    public static void Part2Test2()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz17\\input_test2.txt");
        var result = ClumsyCrucible.UltraHeatLoss(input);
        Assert.Equal(71, result);
    }

    [Fact]
    public static void Part2Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz17\\input.txt");
        var result = ClumsyCrucible.UltraHeatLoss(input);
        Assert.Equal(930, result);
    }
}