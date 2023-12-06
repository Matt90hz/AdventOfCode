namespace AdventOfCode2023.Day5;

internal static class AlmancExtensions
{
    public static Map[] GetMap(this Almanac almanac, MapType type) => type switch
    {
        MapType.SeedToSoil => almanac.SeedToSoil,
        MapType.SoilToFertilizer => almanac.SoilToFertilizer,
        MapType.FertilizerToWater => almanac.FertilizerToWater,
        MapType.WaterToLight => almanac.WaterToLight,
        MapType.LightToTemperature => almanac.LightToTemperature,
        MapType.TemperatureToHumidity => almanac.TemperatureToHumidity,
        MapType.HumidityToLocation => almanac.HumidityToLocation,
        _ => throw new ArgumentException("Invalid mapType."),
    };
}





