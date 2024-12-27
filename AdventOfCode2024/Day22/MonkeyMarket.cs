
namespace AdventOfCode2024.Day22;
public static class MonkeyMarket
{
    public static long SumSecretNumbersGenerated(string input)
    {
        var secretNumbers = input.Split(Environment.NewLine).Select(long.Parse);
        var generated = secretNumbers.Select(x => Enumerable.Range(0, 2000).Aggregate(x, (a, _) => Generate(a)));
        var sum = generated.Sum();
        return sum;
    }

    public static long MaxBananasObtainable(string input)
    {
        var secretNumbers = input.Split(Environment.NewLine).Select(long.Parse);
        var prices = secretNumbers.Select(x => GetSecretsList(x).Select(x => x % 10));
        var variations = prices.Select(x => x.Zip(x.Skip(1)).Select(x => (x.Second - x.First, x.Second)).ToArray());
        var sellSequences = variations.Select(GetSellSequences);
        var max = sellSequences
            .SelectMany(x => x)
            .GroupBy(x => x.Sequence, x => x.Price, (k, x) => (k, Price: x.Sum()))
            .MaxBy(x => x.Price);
        return max.Price;
    }

    private static ((long, long, long, long) Sequence, long Price)[] GetSellSequences((long Variation, long Price)[] variations)
    {
        var list = new List<((long, long, long, long) Sequence, long Price)>(variations.Length);

        for (int i = 0, j = 3; j < variations.Length; i++, j++)
        {
            var a = variations[i].Variation;
            var b = variations[i + 1].Variation;
            var c = variations[i + 2].Variation;
            var d = variations[i + 3].Variation;
            var p = variations[i + 3].Price;

            list.Add(((a, b, c, d), p));        
        }

        var sellSequences = list.GroupBy(x => x.Sequence, x => x.Price, (k, x) => (k, x.First()));

        return sellSequences.ToArray();
    }

    private static long[] GetSecretsList(long secret, int length = 2000)
    {
        var list = new long[length + 1];
        list[0] = secret;

        for (int i = 1; i <= length; i++)
        {
            list[i] = Generate(list[i - 1]);
        }

        return list;
    }

    private static long Generate(long secret)
    {
        var x = secret * 64;
        secret ^= x;
        secret %= 16777216;
        var y = secret / 32;
        secret ^= y;
        secret %= 16777216;
        var z = secret * 2048;
        secret ^= z;
        secret %= 16777216;
        return secret;
    }
}