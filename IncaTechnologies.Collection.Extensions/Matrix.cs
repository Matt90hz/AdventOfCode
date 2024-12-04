using System;
using System.Collections.Generic;
using System.Linq;

namespace IncaTechnologies.Collection.Extensions
{
    public static class Matrix
    {
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
            if (columnArray.LongLength < columnCount) throw new ArgumentException("Column too small.");

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
    }
}
