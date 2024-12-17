namespace AdventOfCode2024.Day16;

public sealed class ReindeerMazeTests
{
    [Fact]
    public void Part1_Test1()
    {
        var input = File.ReadAllText(@".\Day16\input_test1.txt");
        var result = ReindeerMaze.LowestScore(input);
        Assert.Equal(7036, result);
    }

    [Fact]
    public void Part1_Test2()
    {
        var input = File.ReadAllText(@".\Day16\input_test2.txt");
        var result = ReindeerMaze.LowestScore(input);
        Assert.Equal(11048, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day16\input.txt");
        var result = ReindeerMaze.LowestScore(input);
        Assert.Equal(143580, result);
    }

    [Fact]
    public void Part2_Test1()
    {
        var input = File.ReadAllText(@".\Day16\input_test1.txt");
        var result = ReindeerMaze.CountTilesBestPaths(input);
        Assert.Equal(45, result);
    }

    [Fact]
    public void Part2_Test2()
    {
        var input = File.ReadAllText(@".\Day16\input_test2.txt");
        var result = ReindeerMaze.CountTilesBestPaths(input);
        Assert.Equal(64, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day16\input.txt");
        var result = ReindeerMaze.CountTilesBestPaths(input);
        Assert.Equal(645, result);
    }
}
