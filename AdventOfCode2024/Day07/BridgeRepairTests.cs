using Xunit;

namespace AdventOfCode2024.Day07;

public sealed class BridgeRepairTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@".\Day07\input_test.txt");
        var result = BridgeRepair.TotalCalibrationResult(input);
        Assert.Equal(3749L, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day07\input.txt");
        var result = BridgeRepair.TotalCalibrationResult(input);
        Assert.Equal(21572148763543L, result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@".\Day07\input_test.txt");
        var result = BridgeRepair.TotalCalibrationResultFix(input);
        Assert.Equal(11387L, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day07\input.txt");
        var result = BridgeRepair.TotalCalibrationResultFix(input);
        Assert.Equal(581941094529163L, result);
    }
}