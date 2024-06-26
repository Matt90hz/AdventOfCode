﻿using System;
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

        public bool Equals(IPosition<T> other) => Row == other.Row && Column == other.Column;
        

        public bool Equals(Position<T> other) => Row == other.Row && Column == other.Column;
        
        public override bool Equals(object obj) => obj is IPosition<T> other && Equals(other);

        public override string ToString()
        {
            return $"[{Row}, {Column}] {Value}";
        }

        public override int GetHashCode() //to do
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Position<T> x, Position<T> y) => x.Equals(y);

        public static bool operator !=(Position<T> x, Position<T> y) => !x.Equals(y);

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

        public static bool TryMoveUp<T>(this IPosition<T> position, out IPosition<T> @out)
        {
            bool isInbound = position.Row > 0;

            if (isInbound)
            {
                @out = position.MoveUp();
                return isInbound;
            }
            else
            {
                @out = position;
                return isInbound;
            }
        }

        public static bool TryMoveDown<T>(this IPosition<T> position, out IPosition<T> @out)
        {
            bool isInbound = position.Row < position.Array.GetLongLength(0) - 1;

            if (isInbound)
            {
                @out = position.MoveDown();
                return isInbound;
            }
            else
            {
                @out = position;
                return isInbound;
            }
        }

        public static bool TryMoveLeft<T>(this IPosition<T> position, out IPosition<T> @out)
        {
            bool isInbound = position.Column > 0;

            if (isInbound)
            {
                @out = position.MoveLeft();
                return isInbound;
            }
            else
            {
                @out = position;
                return isInbound;
            }
        }

        public static bool TryMoveRight<T>(this IPosition<T> position, out IPosition<T> @out)
        {
            bool isInbound = position.Column < position.Array.GetLongLength(1) - 1;

            if (isInbound)
            {
                @out = position.MoveRight();
                return isInbound;
            }
            else
            {
                @out = position;
                return isInbound;
            }
        }

        public static bool TryGetValue<T>(this IPosition<T> position, out T @out)
        {
            @out = default!;

            if(position.Row <= 0 
                || position.Row >= position.Array.GetLongLength(0) - 1 
                || position.Column <= 0 
                || position.Column >= position.Array.GetLongLength(1) - 1) return false;
            

            @out = position.Value;

            return true;
        }

        public static IEnumerable<IPosition<T>> GetNeighbours<T>(this IPosition<T> position)
        {
            if (position.TryMoveUp(out var up)) 
            { 
                if (up.TryMoveLeft(out var upLeft)) yield return upLeft;
                yield return up;
                if (up.TryMoveRight(out var upRight)) yield return upRight;
            }

            if (position.TryMoveRight(out var right)) yield return right;

            if (position.TryMoveDown(out var down)) 
            {
                if (down.TryMoveRight(out var downRight)) yield return downRight;
                yield return down;
                if (down.TryMoveRight(out var downLeft)) yield return downLeft;
            }

            if (position.TryMoveLeft(out var left)) yield return left;
        }

        public static IEnumerable<IPosition<T>> GetAdjecents<T>(this IPosition<T> position)
        {
            if (position.TryMoveUp(out var up)) yield return up;
            if (position.TryMoveRight(out var right)) yield return right;
            if (position.TryMoveDown(out var down)) yield return down;
            if (position.TryMoveLeft(out var left)) yield return left;
        }

        public static IEnumerable<IPosition<T>> GetNorth<T>(this IPosition<T> position)
        {
            for (long i = position.Row - 1; i >= 0; i--)
            {
                yield return position.Array.GetPosition(i, position.Column);
            }        
        }

        public static IEnumerable<IPosition<T>> GetSouth<T>(this IPosition<T> position)
        {
            for (long i = position.Row + 1; i < position.Array.GetLongLength(0); i++)
            {
                yield return position.Array.GetPosition(i, position.Column);
            }
        }

        public static IEnumerable<IPosition<T>> GetEast<T>(this IPosition<T> position)
        {
            for (long i = position.Column + 1; i < position.Array.GetLongLength(1); i++)
            {
                yield return position.Array.GetPosition(position.Row, i);
            }
        }

        public static IEnumerable<IPosition<T>> GetWest<T>(this IPosition<T> position)
        {
            for (long i = position.Column - 1; i >= 0; i--)
            {
                yield return position.Array.GetPosition(position.Row, i);
            }
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

        public static bool IsBorder<T>(this IPosition<T> position) =>
            position.Row == 0
            || position.Column == 0
            || position.Row == position.Array.GetLongLength(0) - 1
            || position.Column == position.Array.GetLongLength(1) - 1;

    }
}
