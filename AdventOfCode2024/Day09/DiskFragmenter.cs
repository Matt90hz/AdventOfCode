using AdventOfCode2024.Day08;
using System.Collections.Generic;
using System.Drawing;
using System.IO.IsolatedStorage;

namespace AdventOfCode2024.Day09;
public static class DiskFragmenter
{
    public static long CompactedChecksum(string input)
    {
        var blocks = ParseBlocks(input);

        int i = 0;
        int j = blocks.Length - 1;

        var curr = blocks[i];
        var last = blocks[j];

        while (i != j)
        {
            if (curr >= 0)
            {
                curr = blocks[++i];
                continue;
            }

            if (last >= 0)
            {
                blocks[i] = last;
                blocks[j] = -1;
                curr = blocks[++i];
                last = blocks[--j];
                continue;
            }

            last = blocks[--j];
        }

        var checksum = Checksum(blocks);

        return checksum;
    }

    public static long CompactedPreserveFilesChecksum(string input)
    {
        var blocks = ParseBlocks(input);

        var spaces = input
            .Where((x, i) => i % 2 != 0)
            .Select(x => x - '0')
            .ToArray();

        var files = input
            .Where((x, i) => i % 2 == 0)
            .Select(x => x - '0')
            .ToArray();

        var spaceIndexes = GetSpaceIndexes(blocks).ToArray();
        var fileIndexes = GetFileIndexes(blocks).Reverse().ToArray();

        for (int f = files.Length - 1; f > 0; f--)
        {
            for (int s = 0; s < spaces.Length - (files.Length - f) + 1; s++)
            {
                if (spaces[s] < files[f]) continue;
                
                for (int m = 0; m < files[f]; m++)
                {
                    var x = blocks[fileIndexes[f] - m];
                    blocks[spaceIndexes[s] + m] = x;
                    blocks[fileIndexes[f] - m] = -1;
                }

                spaces[s] -= files[f];
                spaceIndexes[s] += files[f];

                break;
            }
        }

        var checksum = Checksum(blocks);

        return checksum;
    }

    public static IEnumerable<int> GetSpaceIndexes(int[] blocks)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] == -1 && i > 0 && blocks[i - 1] != -1) yield return i;
            if (i > 0 && blocks[i - 1] >= 0 && blocks[i] >= 0 && blocks[i] != blocks[i - 1]) yield return -1;
        }
    }

    public static IEnumerable<int> GetFileIndexes(int[] blocks)
    {
        yield return blocks.Length - 1;

        for (int i = blocks.Length - 2; i >= 0; i--)
        {
            if (blocks[i] >= 0 && blocks[i + 1] != blocks[i]) yield return i;
        }
    }

    public static long CompactedPreserveFilesChecksumSlowSlow(string input)
    {
        var blocks = input
            .Select((x, i) => i % 2 == 0
                ? (Id: i / 2, Size: x - '0', IsFile: true)
                : (Id: i / 2, Size: x - '0', IsFile: false))
            .ToList();

        var files = blocks.Where(x => x.IsFile).Reverse().ToArray();
        var spaces = blocks.Where(x => !x.IsFile);

        foreach (var file in files)
        {
            var (idf, size, _) = file;

            foreach (var space in spaces.ToArray())
            {
                var (id, empty, _) = space;

                var indexSpace = blocks.IndexOf(space);
                var indexFile = blocks.IndexOf(file);

                if (indexSpace > indexFile) break;

                var reminder = empty - size;

                if (reminder < 0) continue;

                blocks.RemoveAt(indexSpace);
                blocks.Insert(indexSpace, file);

                if (reminder > 0)
                {
                    blocks.Insert(indexSpace + 1, (id, reminder, false));
                    indexFile++;
                }

                blocks.RemoveAt(indexFile);
                blocks.Insert(indexFile, (id, size, false));

                break;
            }
        }

        var checksum = blocks.SelectMany((x, i) =>
        {
            var (id, size, isFile) = x;

            var block = isFile
                ? Enumerable.Range(0, size).Select(_ => id)
                : Enumerable.Range(0, size).Select(_ => -1);

            return block;
        });

        return Checksum(checksum.ToArray());
    }

    public static long CompactedPreserveFilesChecksumSlow(string input)
    {
        var blocks = ParseBlocks(input);

        int i = 0;
        int j = blocks.Length - 1;
        int size = GetSize(blocks, j);

        while (blocks[j] != 0)
        {
            var curr = blocks[i];
            var last = blocks[j];

            if (curr >= 0)
            {
                i++;
            }

            if (last < 0)
            {
                j--;
                size = GetSize(blocks, j);
            }

            if (curr == last)
            {
                i = 0;
                j -= size;
                size = GetSize(blocks, j);
                continue;
            }

            int space = GetSpace(blocks, i);

            if (space < size)
            {
                i += space;
                continue;
            }

            for (int k = 0; k < size; k++, i++, j--)
            {
                blocks[i] = last;
                blocks[j] = -1;
            }

            size = GetSize(blocks, j);
            i = 0;
        }

        var checksum = Checksum(blocks);

        return checksum;
    }

    private static int GetSize(int[] blocks, int j)
    {
        var id = blocks[j];

        if (id < 0) return 0;

        var count = 0;

        for (; j >= 0; j--)
        {
            if (blocks[j] != id) break;
            count++;
        }

        return count;
    }

    private static int GetSpace(int[] blocks, int i)
    {
        var length = blocks.Length;
        var count = 0;

        for (; i < length; i++)
        {
            if (blocks[i] >= 0) break;
            count++;
        }

        return count;
    }

    public static long Checksum(int[] blocks)
    {
        var checksum = blocks
            .Select((x, i) => (long)(i * x))
            .Where(x => x > 0)
            .Sum();

        return checksum;
    }

    public static int[] ParseBlocks(string input)
    {
        var diskSize = input.Select(x => x - '0').Sum();

        var blocks = new int[diskSize];

        var diskMap = input.AsSpan();
        var diskMapSize = diskMap.Length;

        for (int i = 0, id = 0, j = 0; i < diskMapSize; i++)
        {
            var isFile = i % 2 == 0;
            var len = diskMap[i] - '0';
            var x = isFile ? id : -1;

            for (int k = 0; k < len; k++, j++)
            {
                blocks[j] = x;
            }

            if (isFile) id++;
        }

        return blocks;
    }
}
