using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace IncaTechnologies.Collection.Extensions
{
    public static class Transform
    {
        public static IEnumerable<T> AsEnumerable<T>(this T[,] @this)
        {
            long rowCount = @this.GetLongLength(0);
            long columnCount = @this.GetLongLength(1);

            for (long i = 0; i < rowCount; i++)
            {
                for (long j = 0; j < columnCount; j++)
                {
                    yield return @this[i, j];
                }
            }
        }

        public static T[][] ToJaggedArray<T>(this IEnumerable<IEnumerable<T>> enumerable)
        {
            var array = enumerable.Select(x => x.ToArray()).ToArray();

            return array;
        }

        public static T[,] ToMultidimensionalArray<T>(this IEnumerable<IEnumerable<T>> enumerable)
        {
            //the transformation in jagged is to preserve long indexes
            var jagged = enumerable as T[][] ?? enumerable.ToJaggedArray();
            var rows = jagged.LongLength;
            var columns = jagged.Select(x => x.LongLength).Max();

            var matrix = new T[rows, columns];

            for (long i = 0; i < rows; i++)
            {
                for (long j = 0; j < jagged[i].Length; j++)
                {
                    matrix[i, j] = jagged[i][j];
                }
            }

            return matrix;
        }

        public static T[][] ToJaggedArray<T>(this IEnumerable<IEnumerable<Position<T>>> enumerable)
        {
            return enumerable.Select(x => x.Select(x => x.Value)).ToJaggedArray();
        }

        public static T[,] ToMultidimensionalArray<T>(this IEnumerable<IEnumerable<Position<T>>> enumerable)
        {
            return enumerable.Select(x => x.Select(x => x.Value)).ToMultidimensionalArray();
        }

        public static string ToFriendlyString<T>(this T[,] @this, Func<T, string>? format = null)
        {
            format ??= x => x?.ToString() ?? "null";

            var rows = @this.GetRows();
            var sb = new StringBuilder();

            foreach (var row in rows)
            {
                foreach (var item in row)
                {
                    sb.Append(format(item));
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
