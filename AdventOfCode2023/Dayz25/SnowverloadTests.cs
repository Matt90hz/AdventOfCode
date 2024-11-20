using AdventOfCode2023.Dayz22;
using System.Diagnostics;
using Xunit.Abstractions;

namespace AdventOfCode2023.Dayz25;

public class SnowverloadTests(ITestOutputHelper _output)
{
    [Fact]
    public void Part1Test1()
    {
        var input = File.ReadAllText(".\\Dayz25\\input_test1.txt");
        var result = Snowverload.GroupSize(input);

        Assert.Equal(54, result);
    }

    [Fact]
    public void Part1Solution() 
    {
        var input = File.ReadAllText(".\\Dayz25\\input.txt");
        var result = Snowverload.GroupSize(input);

        Assert.Equal(614655, result);
    }

    [Fact]
    public void Part1Test1NoCheat()
    {
        var input = File.ReadAllText(".\\Dayz25\\input_test1.txt");
        var result = Snowverload.GroupSizeNoCheat(input);

        Assert.Equal(54, result);
    }

    [Fact]
    public void Part1SolutionNoCheat()
    {
        var input = File.ReadAllText(".\\Dayz25\\input.txt");
        var result = Snowverload.GroupSizeNoCheat(input);

        Assert.Equal(614655, result);
    }

    [Fact]
    public void Part1Test1NoCheatFast()
    {
        var input = File.ReadAllText(".\\Dayz25\\input_test1.txt");
        var result = SnowverloadFast.GroupSize(input);

        Assert.Equal(54, result);
    }

    [Fact]
    public void Part1SolutionNoCheatFast()
    {
        var input = File.ReadAllText(".\\Dayz25\\input.txt");
        var result = SnowverloadFast.GroupSize(input);

        Assert.Equal(614655, result);
    }

    [Fact]
    public void Part1Test1NoCheatFastest()
    {
        var input = File.ReadAllText(".\\Dayz25\\input_test1.txt");
        var result = SnowverloadFastest.GroupSize(input);

        Assert.Equal(54, result);
    }

    [Fact]
    public void Part1SolutionNoCheatFastest()
    {
        var input = File.ReadAllText(".\\Dayz25\\input.txt");
        var result = SnowverloadFastest.GroupSize(input);

        Assert.Equal(614655, result);
    }

    [Fact]
    public void Part1Test1Optimized()
    {
        var input = File.ReadAllText(".\\Dayz25\\input_test1.txt");
        var result = SnowerloadOptimized.GroupSize(input);

        Assert.Equal(54, result);
    }

    [Fact]
    public void Part1SolutionOptimized()
    {
        var input = File.ReadAllText(".\\Dayz25\\input.txt");

        var sw = Stopwatch.StartNew();
        var result = SnowerloadOptimized.GroupSize(input);
        sw.Stop();
        _output.WriteLine(sw.ElapsedMilliseconds.ToString());

        Assert.Equal(614655, result);
    }
}
