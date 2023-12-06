namespace AdventOfCode2023.Day5;

internal static class RangeExtensions
{
    public static bool IsOutside(this Range range1, Range range2)
        => range1.Start < range2.Start
        || range1.End > range2.End;

    public static bool IsLeftOverlapping(this Range range1, Range range2)
        => range1.Start < range2.Start
        && range1.End >= range2.Start
        && range1.End <= range2.End;

    public static bool IsContained(this Range range1, Range range2)
        => range1.Start >= range2.Start 
        && range1.End <= range2.End;

    public static bool IsRightOverlapping(this Range range1, Range range2)
        => range1.Start <= range2.End
        && range1.Start >= range2.Start
        && range1.End > range2.End;

    public static bool IsCompletelyOverlapping(this Range range1, Range range2)
        => range1.Start < range2.Start
        && range1.End > range2.End;

}





