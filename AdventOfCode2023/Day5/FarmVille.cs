using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day5;

enum MapType { SeedToSoil, SoilToFertilizer, FertilizerToWater, WaterToLight, LightToTemperature, TemperatureToHumidity, HumidityToLocation }

record Almanac(
    Map[] SeedToSoil,
    Map[] SoilToFertilizer,
    Map[] FertilizerToWater,
    Map[] WaterToLight,
    Map[] LightToTemperature,
    Map[] TemperatureToHumidity,
    Map[] HumidityToLocation);

record Range(long Start, long End)
{
    public long Gap => End - Start + 1;

    public static Range Create(long Start, long Count) => new(Start, Start + Count - 1);

    public static Range Empty { get; } = new(-1, -1);

};

record Map(long Destination, long Source, long Range);

internal static class FarmVille
{
    public static long NearestLocation(string input)
    {
        var seeds = GetSeeds(input);
        var almanac = GetAlmanac(input);

        var mappings = Enum
            .GetValues<MapType>()
            .Select(mt => almanac.GetMap(mt))
            .ToArray();

        var locations = seeds.Select(seed => mappings.Aggregate(seed, (x, m) => m.Map(x)));

        return locations.Min();
    }

    public static long NearestLocationWithRangeSeeds(string input)
    {
        var ranges = GetRanges(input);
        var almanac = GetAlmanac(input);

        var mappings = Enum
            .GetValues<MapType>()
            .Select(mt => almanac.GetMap(mt))
            .ToArray();

        var locations = ranges.SelectMany(range => mappings.Aggregate(new [] { range }, (x, m) => m.Map(x)));

        var min = locations.Select(x => x.Start).Min();

        return min;
    }

    static Almanac GetAlmanac(string input)
    {
        var seedToSoil = GetMap(input, MapType.SeedToSoil);
        var soilToFertilizer = GetMap(input, MapType.SoilToFertilizer);
        var fertilizerToWater = GetMap(input, MapType.FertilizerToWater);
        var waterToLight = GetMap(input, MapType.WaterToLight);
        var lightToTemperature = GetMap(input, MapType.LightToTemperature);
        var temperatureToHumidity = GetMap(input, MapType.TemperatureToHumidity);
        var humidityToLocation = GetMap(input, MapType.HumidityToLocation);

        return new Almanac(
            seedToSoil,
            soilToFertilizer,
            fertilizerToWater,
            waterToLight,
            lightToTemperature,
            temperatureToHumidity,
            humidityToLocation);
    }

    static Map[] GetMap(string input, MapType mapType)
    {
        var mapKey = mapType switch
        {
            MapType.SeedToSoil => "seed-to-soil",
            MapType.SoilToFertilizer => "soil-to-fertilizer",
            MapType.FertilizerToWater => "fertilizer-to-water",
            MapType.WaterToLight => "water-to-light",
            MapType.LightToTemperature => "light-to-temperature",
            MapType.TemperatureToHumidity => "temperature-to-humidity",
            MapType.HumidityToLocation => "humidity-to-location",
            _ => throw new ArgumentException("Invalid mapType."),
        };

        var lines = input.Split(Environment.NewLine);
        var mapLines = lines
            .SkipWhile(line => !line.StartsWith(mapKey))
            .Skip(1)
            .TakeWhile(line => !string.IsNullOrEmpty(line));

        var mapStringValues = mapLines.Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        var mapValues = mapStringValues.Select(stringValues => stringValues.Select(x => long.Parse(x)).ToArray());
        var maps = mapValues.Select(mapValue => new Map(mapValue[0], mapValue[1], mapValue[2]));

        return maps.ToArray();
    }

    static long[] GetSeeds(string input)
    {
        var firstLine = input.Split(Environment.NewLine)[0];
        var seedsString = firstLine[6..];
        var seedsStringValues = seedsString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var seeds = seedsStringValues.Select(long.Parse);

        return seeds.ToArray();
    }

    static Range[] GetRanges(string input)
    {
        var seeds = GetSeeds(input);
        var starts = seeds.Where((s, i) => i % 2 == 0);
        var ranges = seeds.Where((s, i) => i % 2 != 0);

        var rangeSeeds = starts.Select((s, i) => Range.Create(s, ranges.ElementAt(i)));

        return rangeSeeds.ToArray();
    }
}





