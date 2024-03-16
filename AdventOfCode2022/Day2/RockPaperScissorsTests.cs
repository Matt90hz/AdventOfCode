using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day2;

public sealed class RockPaperScissorsTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day2\input_test1.txt");
        var result = RockPaperScissors.TotalScore(input);

        result.Should().Be(15);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day2\input.txt");
        var result = RockPaperScissors.TotalScore(input);

        result.Should().Be(15337);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day2\input_test1.txt");
        var result = RockPaperScissors.TotalScoreRightWay(input);

        result.Should().Be(12);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day2\input.txt");
        var result = RockPaperScissors.TotalScoreRightWay(input);

        result.Should().Be(11696);
    }
}