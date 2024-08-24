using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IncaTechnologies.Collection.Extensions
{
    public static class Stuff
    {
        public static IEnumerable<IEnumerable<T>> SplitOn<T>(this IEnumerable<T> source, Func<T, bool> condition)
        {
            var splitted = new List<IEnumerable<T>>();
            var split = new List<T>();

            foreach (var item in source)
            {
                if (condition(item) is false)
                {
                    split.Add(item);
                    continue;
                }

                if (split.Any() is false) continue;

                splitted.Add(split.ToArray());
                split.Clear();
            }

            if (split.Any()) splitted.Add(split.ToArray());

            return splitted;
        }

        public static IEnumerable<T> Where<T>(T[,] @this, Func<T, (long Row, long Column), bool> predicate)
        {
            for (long i = 0; i < @this.GetLongLength(0); i++)
            {
                for (long j = 0; j < @this.GetLongLength(1); j++)
                {
                    if (predicate(@this[i, j], (i, j))) yield return @this[i, j];
                }
            }
        }

        public static IEnumerable<T> Where<T>(T[,] @this, Func<T, bool> predicate)
        {
            for (long i = 0; i < @this.GetLongLength(0); i++)
            {
                for (long j = 0; j < @this.GetLongLength(1); j++)
                {
                    if (predicate(@this[i, j])) yield return @this[i, j];
                }
            }
        }
    }
}
