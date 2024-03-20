using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace AdventOfCode2022.Day07;
static class NoSpaceLeftOnDevice
{
    public record FSItem(string Name);

    public record File(string Name, int Size) : FSItem(Name);

    public record Directory(string Name, List<FSItem> Content, Directory? Previous) : FSItem(Name);

    public static int FatSize => 100000;

    public static int DiskSize => 70000000;

    public static int UpdateSize => 30000000;

    public static int SumFatDirectoriesSize(string input) => GetRoot(input)
        .GetAllSubDirectories()
        .Select(dir => dir.GetSize())
        .Where(size => size <= FatSize)
        .Sum();

    public static int GetSizeOfDirectoryToDelete(string input)
    {
        var root = GetRoot(input);
        var spaceToFree = UpdateSize - (DiskSize - root.GetSize());

        return root
            .GetAllSubDirectories()
            .Select(dir => dir.GetSize())
            .Where(size => size >= spaceToFree)
            .Min();
    }

    public static Directory GetRoot(string input) => input
        .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
        .Select(line => line.Split(' '))
        .Aggregate(new Directory("/", [], null),
        (current, cmd) =>
        {
            current = cmd switch
            {
                { Length: 3 } => cmd[2] switch
                {
                    "/" => current.GetTopLevel(),
                    ".." => current.Previous!,
                    var name => current
                        .GetSubDirectories()
                        .First(dir => dir.Name.Equals(name)),
                },
                _ => current
            };

            FSItem? toAdd = cmd switch
            {
                { Length: 2 } => (cmd[0], cmd[1]) switch
                {
                    (var size, var name) when char.IsDigit(size[0]) => new File(name, int.Parse(size)),
                    ("dir", var dirName) => new Directory(dirName, [], current),
                    _ => null
                },
                _ => null
            };

            if (toAdd is not null) current.Content.Add(toAdd);

            return current;
        })
        .GetTopLevel();

    public static Directory GetTopLevel(this Directory directory) => directory.Previous?.GetTopLevel() ?? directory;

    public static IEnumerable<Directory> GetSubDirectories(this Directory directory) => directory.Content
        .Where(fsitem => fsitem is Directory)
        .Cast<Directory>();

    public static int GetSize(this Directory dir) => dir.Content
        .Select(fsitem => fsitem switch
        {
            File file => file.Size,
            Directory dir => dir.GetSize(),
            _ => default,
        })
        .Sum();

    public static IEnumerable<Directory> GetAllSubDirectories(this Directory dir) => dir
        .GetSubDirectories()
        .Aggregate(Enumerable.Empty<Directory>(),
        (sub, dir) => sub
            .Append(dir)
            .Concat(dir.GetAllSubDirectories()));

}
