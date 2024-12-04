using AdventOfCode2023.Dayz22;

namespace AdventOfCode2023.Dayz25;

public static class SnowverloadTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllText(".\\Dayz25\\input_test1.txt");
        var result = Snowverload.GroupSize(input);

        Assert.Equal(54, result);
    }

    [Fact]
    public static void Part1Solution() 
    {
        var input = File.ReadAllText(".\\Dayz25\\input.txt");
        var result = Snowverload.GroupSize(input);

        Assert.Equal(614655, result);
    }

    [Fact]
    public static void Part1Test1NoCheat()
    {
        var input = File.ReadAllText(".\\Dayz25\\input_test1.txt");
        var result = Snowverload.GroupSizeNoCheat(input);

        Assert.Equal(54, result);
    }

    [Fact]
    public static void Part1Test1NoCheatFast()
    {
        var input = File.ReadAllText(".\\Dayz25\\input_test1.txt");
        var result = SnowverloadFast.GroupSize(input);

        Assert.Equal(54, result);
    }

    [Fact]
    public static void Part1Test1NoCheatFastest()
    {
        var input = File.ReadAllText(".\\Dayz25\\input_test1.txt");
        var result = SnowverloadFastest.GroupSize(input);

        Assert.Equal(54, result);
    }

    [Fact]
    public static void Part1Test1Optimized()
    {
        var input = File.ReadAllText(".\\Dayz25\\input_test1.txt");
        var result = SnowerloadOptimized.GroupSize(input);

        Assert.Equal(54, result);
    }

    [Fact]
    public static void Part1SolutionOptimized()
    {
        var input = File.ReadAllText(".\\Dayz25\\input.txt");
        var result = SnowerloadOptimized.GroupSize(input);

        Assert.Equal(614655, result);
    }
}
