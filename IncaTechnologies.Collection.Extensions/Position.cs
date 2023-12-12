using System;
using System.Collections.Generic;

namespace IncaTechnologies.Collection.Extensions
{
    public class Position<T>
    {
        public T[,] Array { get; }

        public long Row { get; }

        public long Column { get; }

        public T Value
        { 
            get => Array[Row, Column];
            set => Array[Row, Column] = value;
        }

        internal Position(T[,] array, long row, long column)
        {
            Array = array;
            Row = row;
            Column = column;
        }
    }

    public static class PositionExtensions
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

        public static Position<T>? GetPosition<T>(this T @this, T[,] source) where T : class
        {
            if (@this is null) return null;

            for (long i = 0; i < source.GetLongLength(0); i++)
            {
                for (long j = 0; j < source.GetLongLength(1); j++)
                {
                    if (source[i, j] is null) continue;

                    if (ReferenceEquals(source[i, j], @this)) return new Position<T>(source, i, j);
                }
            }

            return null;
        }

        public static Position<T> MoveUp<T>(this Position<T> position)
            => new Position<T>(position.Array, position.Row + 1, position.Column);

        public static Position<T> MoveDown<T>(this Position<T> position)
            => new Position<T>(position.Array, position.Row - 1, position.Column);

        public static Position<T> MoveLeft<T>(this Position<T> position)
            => new Position<T>(position.Array, position.Row, position.Column - 1);

        public static Position<T> MoveRight<T>(this Position<T> position)
            => new Position<T>(position.Array, position.Row, position.Column + 1);

        public static IEnumerable<Position<T>> GetNeighbours<T>(this Position<T> position)
        {
            yield return position.MoveUp().MoveLeft();
            yield return position.MoveUp();
            yield return position.MoveUp().MoveRight();
            yield return position.MoveDown().MoveRight();
            yield return position.MoveDown();
            yield return position.MoveDown().MoveLeft();
        }
    }
}
