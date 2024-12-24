namespace AdventOfCode2024.Day21;

public sealed class KeypadConundrumTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@".\Day21\input_test.txt");
        var result = KeypadConundrum.AirlockPassword(input);
        Assert.Equal(126384, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day21\input.txt");
        var result = KeypadConundrum.AirlockPassword(input);
        Assert.Equal(202648, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day21\input.txt");
        var result = KeypadConundrum.AirlockPassword(input, 25);
        Assert.Equal(248919739734728, result);
    }
}
