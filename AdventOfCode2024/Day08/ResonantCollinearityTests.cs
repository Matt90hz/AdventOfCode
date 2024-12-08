using Xunit;

namespace AdventOfCode2024.Day08;

public sealed class ResonantCollinearityTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@".\Day08\input_test.txt");
        var result = ResonantCollinearity.CountAntinodes(input);
        Assert.Equal(14, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day08\input.txt");
        var result = ResonantCollinearity.CountAntinodes(input);
        Assert.Equal(220, result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@".\Day08\input_test.txt");
        var result = ResonantCollinearity.CountResonantAntinodes(input);
        Assert.Equal(34, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day08\input.txt");
        var result = ResonantCollinearity.CountResonantAntinodes(input);
        Assert.Equal(813, result);
    }
}