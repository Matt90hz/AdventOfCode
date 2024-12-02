using Xunit;

namespace AdventOfCode2024.Day02;

public sealed class RedNosedReportsTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(".\\Day02\\input_test.txt");
        var result = RedNosedReports.HowManyReportsAreSafe(input);
        Assert.Equal(2, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(".\\Day02\\input.txt");
        var result = RedNosedReports.HowManyReportsAreSafe(input);
        Assert.Equal(591, result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(".\\Day02\\input_test.txt");
        var result = RedNosedReports.HowManyReportsAreSafeWithProblemDampener(input);
        Assert.Equal(4, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(".\\Day02\\input.txt");
        var result = RedNosedReports.HowManyReportsAreSafeWithProblemDampener(input);
        Assert.Equal(621, result);
    }
}
