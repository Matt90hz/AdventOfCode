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
            for (long i = 0; i < @this.GetLongLength(0); i++)
            {
                yield return @this.GetRow(i);
            }
        }

        public static IEnumerable<IEnumerable<T>> GetColumns<T>(this T[,] @this)
        {
            for (long j = 0; j < @this.GetLongLength(1); j++)
            {
                yield return @this.GetColumn(j);
            }
        }

        public static IEnumerable<T> GetRow<T>(this T[,] @this, long index)
        {
            for (long j = 0; j < @this.GetLongLength(1); j++)
            {
                yield return @this[index, j];
            }
        }

        public static IEnumerable<T> GetColumn<T>(this T[,] @this, long index)
        {
            for (long i = 0; i < @this.GetLongLength(0); i++)
            {
                yield return @this[i, index];
            }
        }

        public static U[,] Select<T, U>(this T[,] @this, Func<T,(long Row, long Column), U> selector)
        {
            var matrix = new U[@this.GetLength(0), @this.GetLength(1)];

            for (long i = 0; i < @this.GetLongLength(0); i++)
            {
                for (long j = 0; j < @this.GetLongLength(1); j++)
                {
                    matrix[i, j] = selector(@this[i, j], (i, j));
                }
            }

            return matrix;
        }

        public static U[,] Select<T, U>(this T[,] @this, Func<T, U> selector) => @this.Select((x, _) => selector(x));

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

        public static T[,] AddRow<T>(this T[,] @this, IEnumerable<T> row, int? index = null)
        {
            var r = index ?? @this.GetLongLength(0) + 1;

            var rowArray = row.ToArray();
            var matrix = new T[@this.GetLongLength(0) + 1, @this.GetLongLength(1)];

            for (long i = 0; i < matrix.GetLongLength(0); i++)
            {
                for (long j = 0; j < matrix.GetLongLength(1); j++)
                {
                    if (i == r)
                    {
                        matrix[i, j] = rowArray[i];
                        continue;
                    }

                    if (i < r)
                    {
                        matrix[i, j] = @this[i, j];
                        continue;
                    }

                    matrix[i, j] = @this[i - 1, j];
                }
            }

            return matrix;
        }

        public static T[,] AddColumn<T>(this T[,] @this, IEnumerable<T> column, long? index = null)
        {
            var col = index ?? @this.GetLongLength(1) + 1;

            var columnArray = column.ToArray();
            var matrix = new T[@this.GetLongLength(0), @this.GetLongLength(1) + 1];

            for (long i = 0; i < matrix.GetLongLength(0); i++)
            {
                for (long j = 0; j < matrix.GetLongLength(1); j++)
                {
                    if(j == col)
                    {
                        matrix[i, j] = columnArray[i];
                        continue;
                    }

                    if (j < col)
                    {
                        matrix[i, j] = @this[i, j];
                        continue;
                    }

                    matrix[i, j] = @this[i, j - 1];
                    
                }
            }

            return matrix;
        }

        public static T[,] SurroundWith<T>(this T[,] @this, T item)
        {
            var rows = @this.GetRows();

            var extra = Enumerable
                .Range(1, @this.GetLength(0))
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
            for (long i = 0; i < @this.GetLongLength(0); i++)
            {
                for (long j = 0; j < @this.GetLongLength(1); j++)
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
