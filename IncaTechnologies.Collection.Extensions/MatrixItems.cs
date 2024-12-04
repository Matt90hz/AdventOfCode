using System;
using System.Collections.Generic;

namespace IncaTechnologies.Collection.Extensions
{
    public static class MatrixItems
    {
        public static U[,] Select<T, U>(this T[,] @this, Func<T, (long Row, long Column), U> selector)
        {
            long rowCount = @this.GetLength(0);
            long columnCount = @this.GetLength(1);

            var matrix = new U[rowCount, columnCount];

            for (long i = 0; i < rowCount; i++)
            {
                for (long j = 0; j < columnCount; j++)
                {
                    matrix[i, j] = selector(@this[i, j], (i, j));
                }
            }

            return matrix;
        }

        public static U[,] Select<T, U>(this T[,] @this, Func<T, U> selector) => @this.Select((x, _) => selector(x));

        public static T[,] ForEach<T>(this T[,] @this, Func<T, (long Row, long Column), T> apply)
        {
            long rowCount = @this.GetLength(0);
            long columnCount = @this.GetLength(1);

            for (long i = 0; i < rowCount; i++)
            {
                for (long j = 0; j < columnCount; j++)
                {
                    @this[i, j] = apply(@this[i, j], (i, j));
                }
            }

            return @this;
        }

        public static T[,] ForEach<T>(this T[,] @this, Func<T, T> apply)
        {
            long rowCount = @this.GetLength(0);
            long columnCount = @this.GetLength(1);

            for (long i = 0; i < rowCount; i++)
            {
                for (long j = 0; j < columnCount; j++)
                {
                    @this[i, j] = apply(@this[i, j]);
                }
            }

            return @this;
        }

        public static int Count<T>(this T[,] @this, Func<T, bool> predicate)
        {
            int count = 0;

            foreach (T item in @this)
            {
                if (predicate(item)) count++;
            }

            return count;
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
