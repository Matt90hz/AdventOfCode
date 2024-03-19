using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day06;

public sealed class TuningTroubleTests
{
    [Fact]
    public void Part1_Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day06\\input_test1.txt");
        var result = TuningTrouble.GetFirstMarkerPosition(input);

        result.Should().Be(7);
    }

    [Fact]
    public void Part1_Test2()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day06\\input_test2.txt");
        var result = TuningTrouble.GetFirstMarkerPosition(input);

        result.Should().Be(5);
    }

    [Fact]
    public void Part1_Test3()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day06\\input_test3.txt");
        var result = TuningTrouble.GetFirstMarkerPosition(input);

        result.Should().Be(6);
    }

    [Fact]
    public void Part1_Test4()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day06\\input_test4.txt");
        var result = TuningTrouble.GetFirstMarkerPosition(input);

        result.Should().Be(10);
    }

    [Fact]
    public void Part1_Test5()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day06\\input_test5.txt");
        var result = TuningTrouble.GetFirstMarkerPosition(input);

        result.Should().Be(11);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day06\\input.txt");
        var result = TuningTrouble.GetFirstMarkerPosition(input);

        result.Should().Be(1794);
    }

    [Fact]
    public void Part2_Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day06\\input_test1.txt");
        var result = TuningTrouble.GetFirstMessageMarkerPosition(input);

        result.Should().Be(19);
    }

    [Fact]
    public void Part2_Test2()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day06\\input_test2.txt");
        var result = TuningTrouble.GetFirstMessageMarkerPosition(input);

        result.Should().Be(23);
    }

    [Fact]
    public void Part2_Test3()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day06\\input_test3.txt");
        var result = TuningTrouble.GetFirstMessageMarkerPosition(input);

        result.Should().Be(23);
    }

    [Fact]
    public void Part2_Test4()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day06\\input_test4.txt");
        var result = TuningTrouble.GetFirstMessageMarkerPosition(input);

        result.Should().Be(29);
    }

    [Fact]
    public void Part2_Test5()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day06\\input_test5.txt");
        var result = TuningTrouble.GetFirstMessageMarkerPosition(input);

        result.Should().Be(26);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day06\\input.txt");
        var result = TuningTrouble.GetFirstMessageMarkerPosition(input);

        result.Should().Be(2851);
    }
}