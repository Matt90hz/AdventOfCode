using System;
using System.Collections.Generic;
using System.Linq;

namespace IncaTechnologies.Collection.Extensions
{
    public enum Direction { Up, Down, Left, Right, None }

    public interface IPosition<T>: IEquatable<IPosition<T>>
    {
        T[,] Array { get; }

        long Row { get; }

        long Column { get; }

        T Value
        {
            get => Array[Row, Column];
            set => Array[Row, Column] = value;
        }

    }

    public class Position<T> : IPosition<T>, IEquatable<IPosition<T>>
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

        public bool Equals(IPosition<T> other)
        {
            return Row == other.Row && Column == other.Column;
        }

        public bool Equals(Position<T> other)
        {
            return Row == other.Row && Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            var other = obj as IPosition<T>;

            if(other is null) return false;

            return Equals(other);
        }

        public override string ToString()
        {
            return $"[{Row}, {Column}] {Value}";
        }

        public override int GetHashCode() //to do
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Position<T> x, Position<T> y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Position<T> x, Position<T> y)
        {
            return !x.Equals(y);
        }

    }

    public static class PositionExtensions
    {
        public static IEnumerable<IPosition<T>> GetPositions<T>(this T[,] @this)
        {
            for (long i = 0; i < @this.GetLongLength(0); i++)
            {
                for (long j = 0; j < @this.GetLongLength(1); j++)
                {
                    yield return new Position<T>(@this, i, j);
                }
            }
        }

        public static IPosition<T> GetPosition<T>(this T[,] @this, long row, long column)
        {
            return new Position<T>(@this, row, column);
        }

        public static IPosition<T>? FindPosition<T>(this T @this, T[,] source) where T : class
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

        public static IPosition<T> MoveUp<T>(this IPosition<T> position)
            => new Position<T>(position.Array, position.Row - 1, position.Column);

        public static IPosition<T> MoveDown<T>(this IPosition<T> position)
            => new Position<T>(position.Array, position.Row + 1, position.Column);

        public static IPosition<T> MoveLeft<T>(this IPosition<T> position)
            => new Position<T>(position.Array, position.Row, position.Column - 1);

        public static IPosition<T> MoveRight<T>(this IPosition<T> position)
            => new Position<T>(position.Array, position.Row, position.Column + 1);

        public static IEnumerable<IPosition<T>> GetNeighbours<T>(this IPosition<T> position)
        {
            yield return position.MoveUp().MoveLeft();
            yield return position.MoveUp();
            yield return position.MoveUp().MoveRight();
            yield return position.MoveDown().MoveRight();
            yield return position.MoveDown();
            yield return position.MoveDown().MoveLeft();
        }

        public static IEnumerable<IPosition<T>> GetAdjecents<T>(this IPosition<T> position)
        {
            yield return position.MoveUp();
            yield return position.MoveRight();
            yield return position.MoveDown();
            yield return position.MoveLeft();
        }

        public static Direction GetLastDirection<T>(this IEnumerable<IPosition<T>> path)
        {
            var last = path.Last();
            var beforeLast = path.SkipLast(1).LastOrDefault();

            if(beforeLast is null) return Direction.None;

            return GetDirection(beforeLast, last);
        }

        public static Direction GetDirection<T>(IPosition<T> first, IPosition<T> next) => (first.Row, first.Column, next.Row, next.Column) switch
        {
            (var fr, var fc, var nr, var nc) when fr == nr && fc != nc => fc < nc ? Direction.Left : Direction.Right,
            (var fr, var fc, var nr, var nc) when fc == nc && fr != nr => fr < nr ? Direction.Down : Direction.Up,
            _ => Direction.None
        };

    }
}
