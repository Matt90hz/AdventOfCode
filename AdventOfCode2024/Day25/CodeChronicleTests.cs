namespace AdventOfCode2024.Day25;

public sealed class CodeChronicleTests
{
    [Fact]
    public void Part1_Test2()
    {
        var input = File.ReadAllText(@".\Day25\input_test.txt");
        var result = CodeChronicle.CountFittingKeys(input);
        Assert.Equal(3, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day25\input.txt");
        var result = CodeChronicle.CountFittingKeys(input);
        Assert.Equal(3338, result);
    }
}