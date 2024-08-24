using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace IncaTechnologies.Collection.Extensions
{
    public static class CollectionExtensions
    {
        public static T[,] GetSquare<T>(this T[,] @this, long row, long colum, long widht)
            => @this.GetPortion<T>(row, colum, widht, widht);

        public static T[,] GetPortion<T>(this T[,] @this, long row, long colum, long widht, long height)
        {
            var portion = new T[height, widht];

            for (long i = row; i < height; i++)
            {
                for (long j = colum; j < widht; j++)
                {
                    portion[i - row, j - height] = @this[i, j];
                }
            }

            return portion;
        }

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

        public static U[,] Select<T, U>(this T[,] @this, Func<T,(long Row, long Column), U> selector)
        {
            long rowConut = @this.GetLength(0);
            long columnConut = @this.GetLength(1);

            var matrix = new U[rowConut, columnConut];

            for (long i = 0; i < rowConut; i++)
            {
                for (long j = 0; j < columnConut; j++)
                {
                    matrix[i, j] = selector(@this[i, j], (i, j));
                }
            }

            return matrix;
        }   

        public static U[,] Select<T, U>(this T[,] @this, Func<T, U> selector) => @this.Select((x, _) => selector(x));

        public static T[,] ForEach<T>(this T[,] @this, Func<T, (long Row, long Column), T> apply)
        {
            long rowConut = @this.GetLength(0);
            long columnConut = @this.GetLength(1);

            for (long i = 0; i < rowConut; i++)
            {
                for (long j = 0; j < columnConut; j++)
                {
                    @this[i, j] = apply(@this[i, j], (i, j));
                }
            }

            return @this;
        }

        public static T[,] ForEach<T>(this T[,] @this, Func<T, T> apply)
        {
            long rowConut = @this.GetLength(0);
            long columnConut = @this.GetLength(1);

            for (long i = 0; i < rowConut; i++)
            {
                for (long j = 0; j < columnConut; j++)
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

        public static T[,] RotateCounterClockwise<T>(this T[,] @this)
        {
            var reverse = @this
                .GetRows()
                .Select(x => x.Reverse())
                .ToMultidimensionalArray();

            var turn = reverse.GetColumns().ToMultidimensionalArray();

            return turn;
        }

        public static T[,] RotateClockwise<T>(this T[,] @this)
        {
            var turn = @this
                .GetColumns()
                .Select(x => x.Reverse())
                .ToMultidimensionalArray();

            return turn;
        }

        public static T[,] AddRow<T>(this T[,] @this, IEnumerable<T> row, long? index = null)
        {
            long columnCount = @this.GetLongLength(0) + 1;
            long rowCount = @this.GetLongLength(1);

            long i_index = index ?? columnCount - 1;

            var rowArray = row.ToArray();
            if (rowArray.LongLength < rowCount) throw new ArgumentException("Row too small.");

            var matrix = new T[columnCount, rowCount];

            for (long i = 0; i < i_index; i++)
            {
                for (long j = 0; j < rowCount; j++)
                {
                    matrix[i, j] = @this[i, j];
                }
            }

            for (long i = i_index + 1; i < columnCount; i++)
            {
                for (long j = 0; j < rowCount; j++)
                {
                    matrix[i, j] = @this[i - 1, j];
                }
            }

            for (long j = 0; j < rowCount; j++)
            {
                matrix[i_index, j] = rowArray[j];
            }

            return matrix;
        }

        public static T[,] AddColumn<T>(this T[,] @this, IEnumerable<T> column, long? index = null)
        {
            long columnCount = @this.GetLongLength(0);
            long rowCount = @this.GetLongLength(1) + 1;

            long i_index = index ?? rowCount - 1;

            var columnArray = column.ToArray();
            if(columnArray.LongLength < columnCount) throw new ArgumentException("Column too small.");

            var matrix = new T[columnCount, rowCount];

            for (long i = 0; i < columnCount; i++)
            {
                for (long j = 0; j < i_index; j++)
                {
                    matrix[i, j] = @this[i, j];
                }

                matrix[i, i_index] = columnArray[i];

                for (long j = i_index + 1; j < rowCount; j++)
                {
                    matrix[i, j] = @this[i, j - 1];
                }
            }

            return matrix;
        }

        public static T[,] SurroundWith<T>(this T[,] @this, T item)
        {
            var rows = @this.GetRows();

            var extra = Enumerable
                .Range(1, @this.GetLength(1))
                .Select(x => item);

            var surrounded = rows
                .Prepend(extra)
                .Append(extra)
                .Select(x => x.Prepend(item).Append(item))
                .ToMultidimensionalArray();

            return surrounded;
        }

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
            var coloumns = jagged.Select(x => x.LongLength).Max();

            var matrix = new T[rows, coloumns];
            
            for (long i = 0; i < rows; i++)
            {
                for (long j = 0; j < jagged[i].Length; j++)
                {
                    matrix[i, j] = jagged[i][j];
                }
            }

            return matrix;
        }

        public static T[][] ToJaggedArray<T>(this IEnumerable<IEnumerable<IPosition<T>>> enumerable)
        {
            return enumerable.Select(x => x.Select(x => x.Value)).ToJaggedArray();
        }

        public static T[,] ToMultidimensionalArray<T>(this IEnumerable<IEnumerable<IPosition<T>>> enumerable)
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
                foreach(var item in row)
                {
                    sb.Append(format(item));
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
