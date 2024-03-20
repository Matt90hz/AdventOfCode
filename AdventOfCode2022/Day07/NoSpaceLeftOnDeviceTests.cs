using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day07;

public sealed class NoSpaceLeftOnDeviceTests
{
    [Fact]
    public void Part1_Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day07\\input_test1.txt");
        var result = NoSpaceLeftOnDevice.SumFatDirectoriesSize(input);

        result.Should().Be(95437);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day07\\input.txt");
        var result = NoSpaceLeftOnDevice.SumFatDirectoriesSize(input);

        result.Should().Be(1792222);
    }

    [Fact]
    public void Part2_Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day07\\input_test1.txt");
        var result = NoSpaceLeftOnDevice.GetSizeOfDirectoryToDelete(input);

        result.Should().Be(24933642);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day07\\input.txt");
        var result = NoSpaceLeftOnDevice.GetSizeOfDirectoryToDelete(input);

        result.Should().Be(1112963);
    }

}
