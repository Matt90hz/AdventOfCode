using System;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace AdventOfCode2023.Day5;

internal static class MapExtensions
{
    public static long Map(this Map[] maps, long source)
    {
        var map = maps.FirstOrDefault(m => source >= m.Source && source <= m.Source + m.Range);

        if (map is null) return source;

        var index = source - map.Source;

        return map.Destination + index;
    }

    public static Range[] Map(this Map[] maps, Range[] ranges)
    {
        var rangesMapped = new List<Range>();
        var rangesToMap = new List<Range>(ranges);

        foreach (var map in maps)
        {
            var (mapped, toMap) = map.Map(rangesToMap.ToArray());

            rangesMapped.AddRange(mapped);
            rangesToMap.Clear();
            rangesToMap.AddRange(toMap);
        }

        rangesMapped.AddRange(rangesToMap);

        return rangesMapped.ToArray();
    }

    public static (Range[] Mapped, Range[] ToMap) Map(this Map map, Range[] ranges)
    {
        var rangesMapped = new List<Range>();
        var rangesToMap = new List<Range>();

        foreach(var range in ranges)
        {
            var (mapped, toMap) = map.Map(range);

            if (mapped != Range.Empty) rangesMapped.Add(mapped);

            rangesToMap.AddRange(toMap);
        }

        return (rangesMapped.ToArray(), rangesToMap.ToArray());
    }

    public static (Range Mapped, Range[] ToMap) Map(this Map map, Range range)
    {
        var source = Range.Create(map.Source, map.Range);
        var destination = Range.Create(map.Destination, map.Range);

        if (range.IsRightOverlapping(source))
        {
            //r    11 12 13 14 16 17 18
            //s 10 11 12 13 14 15
            //d 20 21 22 23 24 25
            //     21 22 23 24 25 17 18
            var innerSection = new Range(destination.Start + range.Start - source.Start, destination.End);
            var rightSection = new Range(range.Start + innerSection.Gap, range.End);

            return (innerSection, new[] { rightSection });
        }

        if (range.IsContained(source))
        {
            //r    11 12 13 14
            //s 10 11 12 13 14 15
            //d 20 21 22 23 24 25
            //     21 22 23 24
            var start = destination.Start + range.Start - source.Start;

            var mapped = Range.Create(start, range.Gap);

            return (mapped, Array.Empty<Range>());
        }

        if (range.IsLeftOverlapping(source))
        {
            //r 7  8  9  10 11 12 13   
            //s          10 11 12 13 14 15
            //d          20 21 22 23 24 25
            //  7  8  9  20 21 22 23
            var leftSection = new Range(range.Start, source.Start - 1);
            var innerSection = new Range(destination.Start, destination.Start - source.Start + range.End);

            return (innerSection, new[] { leftSection });
        }

        if (range.IsCompletelyOverlapping(source))
        {
            //r 10 11 12 13 14 15
            //s       12 13 
            //d       22 23
            //  10 11 21 22 14 15
            var leftSection = new Range(range.Start, source.Start - 1);
            var rightSection = new Range(source.End + 1, range.End);

            return (destination, new[] { leftSection, rightSection });
        }
    

        return (Range.Empty, new Range[] { range });
    }


}





