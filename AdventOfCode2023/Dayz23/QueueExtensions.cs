namespace AdventOfCode2023.Dayz23;

internal static class QueueExtensions
{
    public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
    {
        foreach(var item in items)
        {
            queue.Enqueue(item);
        }
    }
}