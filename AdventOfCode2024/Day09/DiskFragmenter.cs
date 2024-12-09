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

        var filesLength = files.Length - 1;

        for (int f = filesLength; f > 0; f--)
        {
            var spacesLength = spaces.Length - (files.Length - f) + 1;

            for (int s = 0; s < spacesLength; s++)
            {
                var size = files[f];

                if (spaces[s] < size) continue;
                
                for (int m = 0; m < size; m++)
                {
                    var fi = fileIndexes[f] - m;

                    blocks[spaceIndexes[s] + m] = blocks[fi];
                    blocks[fi] = -1;
                }

                spaces[s] -= size;
                spaceIndexes[s] += size;

                break;
            }
        }

        var checksum = Checksum(blocks);

        return checksum;
    }

    private static IEnumerable<int> GetSpaceIndexes(int[] blocks)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] == -1 && i > 0 && blocks[i - 1] != -1) yield return i;
            if (i > 0 && blocks[i - 1] >= 0 && blocks[i] >= 0 && blocks[i] != blocks[i - 1]) yield return -1;
        }
    }

    private static IEnumerable<int> GetFileIndexes(int[] blocks)
    {
        yield return blocks.Length - 1;

        for (int i = blocks.Length - 2; i >= 0; i--)
        {
            if (blocks[i] >= 0 && blocks[i + 1] != blocks[i]) yield return i;
        }
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
