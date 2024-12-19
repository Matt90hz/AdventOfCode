namespace AdventOfCode2024.Day18;

public sealed class RamRunTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@".\Day18\input_test.txt");
        var result = RamRun.MinimumStepToExit(input, 7, 12);
        Assert.Equal(22, result);
    }


    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day18\input.txt");
        var result = RamRun.MinimumStepToExit(input);
        Assert.Equal(260, result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@".\Day18\input_test.txt");
        var result = RamRun.LastCorruptedByte(input, 7);
        Assert.Equal("6,1", result);
    }


    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day18\input.txt");
        var result = RamRun.LastCorruptedByte(input);
        Assert.Equal("24,48", result);
    }
}
