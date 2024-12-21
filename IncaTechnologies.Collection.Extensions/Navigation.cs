using System.Collections.Generic;
using System.Linq;
using System;

namespace IncaTechnologies.Collection.Extensions
{
    public enum Direction { Up, Down, Left, Right, None }

    public static class Navigation
    {
        public static Position<T> Move<T>(this Position<T> position, Direction direction) => direction switch
        {
            Direction.Up => position.MoveUp(),
            Direction.Down => position.MoveDown(),
            Direction.Left => position.MoveLeft(),
            Direction.Right => position.MoveRight(),
            Direction.None => position,
        };

        public static Position<T> MoveUp<T>(this Position<T> position)
        {
            position.Row--;
            return position;
        }

        public static Position<T> MoveDown<T>(this Position<T> position)
        {
            position.Row++;
            return position;
        }

        public static Position<T> MoveLeft<T>(this Position<T> position)
        {
            position.Column--;
            return position;
        }

        public static Position<T> MoveRight<T>(this Position<T> position)
        {
            position.Column++;
            return position;
        }

        public static bool TryMove<T>(this Position<T> position, Direction direction, out Position<T> @out)
        {
            @out = position;

            var success = direction switch
            {
                Direction.Up => position.TryMoveUp(out @out),
                Direction.Down => position.TryMoveDown(out @out),
                Direction.Left => position.TryMoveLeft(out @out),
                Direction.Right => position.TryMoveRight(out @out),
                Direction.None => true,
                _ => throw new NotImplementedException($"{direction} not implemented")
            };

            return success;
        }

        public static bool TryMoveUp<T>(this Position<T> position, out Position<T> @out)
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

        public static bool TryMoveDown<T>(this Position<T> position, out Position<T> @out)
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

        public static bool TryMoveLeft<T>(this Position<T> position, out Position<T> @out)
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

        public static bool TryMoveRight<T>(this Position<T> position, out Position<T> @out)
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

        public static bool TryGetValue<T>(this Position<T> position, out T @out)
        {
            @out = default!;

            if (position.Row <= 0
                || position.Row >= position.Array.GetLongLength(0) - 1
                || position.Column <= 0
                || position.Column >= position.Array.GetLongLength(1) - 1) return false;


            @out = position.Value;

            return true;
        }

        public static Direction GetLastDirection<T>(this IEnumerable<Position<T>> path)
        {
            var last = path.Last();
            var beforeLast = path.SkipLast(1);

            return beforeLast.Any() is false 
                ? Direction.None 
                : GetDirection(beforeLast.Last(), last);
        }

        public static Direction GetDirection<T>(this Position<T> first, Position<T> next)
        {
            var direction = (first.Row, first.Column, next.Row, next.Column) switch
            {
                (var fr, var fc, var nr, var nc) when fr == nr && fc != nc => fc < nc ? Direction.Right : Direction.Left,
                (var fr, var fc, var nr, var nc) when fc == nc && fr != nr => fr < nr ? Direction.Down : Direction.Up,
                _ => Direction.None
            };

            return direction;
        }

        public static bool IsBorder<T>(this Position<T> position)
        {
            var isBorder = position.Row == 0
                || position.Column == 0
                || position.Row == position.Array.GetLongLength(0) - 1
                || position.Column == position.Array.GetLongLength(1) - 1;

            return isBorder;
        }

        public static bool IsOutOfBound<T>(this Position<T> position)
        {
            var isBorder = position.Row < 0
                || position.Column < 0
                || position.Row >= position.Array.GetLongLength(0)
                || position.Column >= position.Array.GetLongLength(1);

            return isBorder;
        }
    }
}
