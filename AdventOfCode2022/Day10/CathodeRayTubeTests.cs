using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day10;

public sealed class CathodeRayTubeTests
{
    [Fact] 
    public void Part1_Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day10\\input_test1.txt");
        var result = CathodeRayTube.SumInterestingSignalStranght(input);

        result.Should().Be(13140);
    }

    [Fact] 
    public void Part1_Solution() 
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day10\\input.txt");
        var result = CathodeRayTube.SumInterestingSignalStranght(input);

        result.Should().Be(15020);
    }

    [Fact]
    public void Part2_Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day10\\input_test1.txt");
        var result = CathodeRayTube.GetScreenImage(input);

        result.Should().Be("##..##..##..##..##..##..##..##..##..##.." +
                       "\r\n###...###...###...###...###...###...###." +
                       "\r\n####....####....####....####....####...." +
                       "\r\n#####.....#####.....#####.....#####....." +
                       "\r\n######......######......######......####" +
                       "\r\n#######.......#######.......#######.....\r\n");
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day10\\input.txt");
        var result = CathodeRayTube.GetScreenImage(input);

        result.Should().Be("####.####.#..#..##..#....###...##..###.." +
                       "\r\n#....#....#..#.#..#.#....#..#.#..#.#..#." +
                       "\r\n###..###..#..#.#....#....#..#.#..#.#..#." +
                       "\r\n#....#....#..#.#.##.#....###..####.###.." +
                       "\r\n#....#....#..#.#..#.#....#....#..#.#...." +
                       "\r\n####.#.....##...###.####.#....#..#.#....\r\n");
    }
}