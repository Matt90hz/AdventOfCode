using System.Collections.Generic;

namespace IncaTechnologies.Collection.Extensions
{
    public static class Coordinates
    {
        public static IEnumerable<Position<T>> GetPositions<T>(this T[,] @this)
        {
            for (long i = 0; i < @this.GetLongLength(0); i++)
            {
                for (long j = 0; j < @this.GetLongLength(1); j++)
                {
                    yield return new Position<T>(@this, i, j);
                }
            }
        }

        public static Position<T> GetPosition<T>(this T[,] @this, long row, long column)
        {
            return new Position<T>(@this, row, column);
        }

        public static Position<T>? FindPosition<T>(this T[,] source, T value, IEqualityComparer<T>? comparer = default)
        {
            if (value is null) return null;

            comparer ??= EqualityComparer<T>.Default;

            for (long i = 0; i < source.GetLongLength(0); i++)
            {
                for (long j = 0; j < source.GetLongLength(1); j++)
                {
                    if (source[i, j] is null) continue;

                    if (comparer.Equals(source[i, j], value)) return new Position<T>(source, i, j);
                }
            }

            return null;
        }
    }
}
