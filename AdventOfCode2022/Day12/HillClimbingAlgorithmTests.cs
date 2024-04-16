using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day12;

public sealed class HillClimbingAlgorithmTests
{
    [Fact]
    public void Part1_Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day12\\input_test1.txt");
        var result = HillClimbingAlgorithm.GetShortestPathToExit(input);

        result.Should().Be(31);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day12\\input.txt");
        var result = HillClimbingAlgorithm.GetShortestPathToExit(input);

        result.Should().Be(447);
    }

    [Fact]
    public void Part2_Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day12\\input_test1.txt");
        var result = HillClimbingAlgorithm.GetShortestScenicPath(input);

        result.Should().Be(29);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day12\\input.txt");
        var result = HillClimbingAlgorithm.GetShortestScenicPath(input);

        result.Should().Be(446);
    }
}