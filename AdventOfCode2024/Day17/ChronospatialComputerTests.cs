namespace AdventOfCode2024.Day17;

public sealed class ChronospatialComputerTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@".\Day17\input_test1.txt");
        var result = ChronospatialComputer.Execute(input);
        Assert.Equal("4,6,3,5,6,3,5,2,1,0", result);
    }


    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day17\input.txt");
        var result = ChronospatialComputer.Execute(input);
        Assert.Equal("1,7,2,1,4,1,5,4,0", result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@".\Day17\input_test2.txt");
        var result = ChronospatialComputer.ExecuteDebug(input);
        Assert.Equal(117440, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day17\input.txt");
        var result = ChronospatialComputer.ExecuteDebug(input);
        Assert.Equal(37221261688308, result);
    }
}