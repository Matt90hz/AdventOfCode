using System;
using System.Collections.Generic;
using System.Linq;

namespace IncaTechnologies.Collection.Extensions
{
    public static class Slicer
    {
        public static IEnumerable<IEnumerable<T>> GetRows<T>(this T[,] @this)
        {
            long length = @this.GetLongLength(0);

            for (long i = 0; i < length; i++)
            {
                yield return @this.GetRow(i);
            }
        }

        public static IEnumerable<IEnumerable<T>> GetColumns<T>(this T[,] @this)
        {
            long length = @this.GetLongLength(1);

            for (long j = 0; j < length; j++)
            {
                yield return @this.GetColumn(j);
            }
        }

        public static IEnumerable<T> GetRow<T>(this T[,] @this, long index)
        {
            long length = @this.GetLongLength(1);

            for (long j = 0; j < length; j++)
            {
                yield return @this[index, j];
            }
        }

        public static IEnumerable<T> GetColumn<T>(this T[,] @this, long index)
        {
            long length = @this.GetLongLength(0);

            for (long i = 0; i < length; i++)
            {
                yield return @this[i, index];
            }
        }

        public static IEnumerable<IEnumerable<T>> SplitOn<T>(this IEnumerable<T> source, Func<T, bool> condition)
        {
            var result = new List<IEnumerable<T>>();
            var split = new List<T>();

            foreach (var item in source)
            {
                if (condition(item) is false)
                {
                    split.Add(item);
                    continue;
                }

                if (split.Any() is false) continue;

                result.Add(split.ToArray());
                split.Clear();
            }

            if (split.Any()) result.Add(split.ToArray());

            return result;
        }
    }
}