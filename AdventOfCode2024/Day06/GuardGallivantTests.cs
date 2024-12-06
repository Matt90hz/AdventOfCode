using Xunit;

namespace AdventOfCode2024.Day06;

public sealed class GuardGallivantTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@".\Day06\input_test.txt");
        var result = GuardGallivant.CountPositions(input);

        Assert.Equal(41, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day06\input.txt");
        var result = GuardGallivant.CountPositions(input);

        Assert.Equal(4964, result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@".\Day06\input_test.txt");
        var result = GuardGallivant.CountInfiniteLoops(input);

        Assert.Equal(6, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day06\input.txt");
        var result = GuardGallivant.CountInfiniteLoops(input);

        Assert.Equal(1740, result);
    }
}