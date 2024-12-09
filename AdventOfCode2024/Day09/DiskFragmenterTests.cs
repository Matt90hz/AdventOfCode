using Xunit;

namespace AdventOfCode2024.Day09;

public sealed class DiskFragmenterTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@".\Day09\input_test.txt");
        var result = DiskFragmenter.CompactedChecksum(input);
        Assert.Equal(1928L, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day09\input.txt");
        var result = DiskFragmenter.CompactedChecksum(input);
        Assert.Equal(6463499258318L, result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@".\Day09\input_test.txt");
        var result = DiskFragmenter.CompactedPreserveFilesChecksum(input);
        Assert.Equal(2858L, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day09\input.txt");
        var result = DiskFragmenter.CompactedPreserveFilesChecksum(input);
        Assert.Equal(6493634986625L, result);
    }
}