using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day07;

public sealed class NoSpaceLeftOnDeviceOtherTests
{
    [Fact]
    public void GetSize_OnDirWithNestedDir_ShouldBeTheSumOfAllFiles()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day07\\input_test1.txt");
        var root = NoSpaceLeftOnDevice.GetRoot(input);
        var subs = root.GetAllSubDirectories();

        root.GetSize().Should().Be(48381165);

        subs.First(dir => dir.Name == "e")
            .GetSize()
            .Should().Be(584);

        subs.First(dir => dir.Name == "a")
            .GetSize()
            .Should().Be(94853);

        subs.First(dir => dir.Name == "d")
            .GetSize()
            .Should().Be(24933642);

    }

    [Fact]
    public void GetAllSubDirectories_OnDirWithNestedDir_ShouldReturnAllNestedDir()
    {
        NoSpaceLeftOnDevice.Directory dir = new("/",
        [
            new NoSpaceLeftOnDevice.File("x", 10),
            new NoSpaceLeftOnDevice.Directory("a",
            [
                new NoSpaceLeftOnDevice.File("y", 20),
                new NoSpaceLeftOnDevice.Directory("b",
                [
                    new NoSpaceLeftOnDevice.File("y", 30)
                ], null)
            ], null),
            new NoSpaceLeftOnDevice.Directory("c",
            [
                new NoSpaceLeftOnDevice.File("y", 20),
                new NoSpaceLeftOnDevice.Directory("d",
                [
                    new NoSpaceLeftOnDevice.File("y", 30)
                ],null)
            ], null)
        ], null);

        var subs = dir.GetAllSubDirectories();

        string[] folderNames = ["a", "b", "c", "d"];

        subs.Should().HaveCount(4)
            .And.OnlyContain(x => folderNames.Contains(x.Name))
            .And.OnlyHaveUniqueItems();
    }

    [Fact]
    public void AllDirInFile_And_AllDirInRoot_ShoubeEqual()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day07\\input.txt");
        var root = NoSpaceLeftOnDevice.GetRoot(input);

        var dirInFile = input
            .Split(Environment.NewLine)
            .Where(line => line.StartsWith("dir"))
            .Select(line => line[4..])
            .ToArray();

        var dirInRoot = root
            .GetAllSubDirectories()
            .Select(dir => dir.Name)
            .ToArray();

        dirInFile.Should().BeEquivalentTo(dirInRoot);
    }

    [Fact]
    public void SumOfAllSizeInFile_And_SumOfAllSizeInRoot_ShoubeEqual()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day07\\input.txt");
        var root = NoSpaceLeftOnDevice.GetRoot(input);

        var sizeInFile = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Where(line => char.IsDigit(line[0]))
            .Select(line => line.Split(' ')[0])
            .Select(int.Parse)
            .Sum();

        var sizeInRoot = root.GetSize();

        sizeInFile.Should().Be(sizeInRoot);
    }

    [Fact]
    public void AllDirSize_SholudBeMoreThanZero()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day07\\input.txt");
        var root = NoSpaceLeftOnDevice.GetRoot(input);

        var folderSizes = root
            .GetAllSubDirectories()
            .Select(dir => dir.GetSize())
            .ToArray();

        var x = root
            .GetAllSubDirectories()
            .Select(dir => (dir.Name, dir.GetSize()))
            .ToArray();

        folderSizes.Should().OnlyContain(size => size > 0);
    }
}