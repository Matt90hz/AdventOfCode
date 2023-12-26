namespace AdventOfCode2023.Dayz18;

internal static class Utility
{
    public static bool NotContains<T>(this IEnumerable<T> source, T item) => !source.Contains(item);
}