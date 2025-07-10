using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day4;

record Card(int Id, int[] WinningNumbers, int[] Numbers);

internal static class Scratchcards
{
    public static int CalculatePoints(string input)
    {
        var cards = GetCards(input);
        var points = cards.Select(card => card.GetPoints());

        return points.Sum();
    }

    public static int PileUp(string input)
    {
        var cards = GetCards(input);

        var cardsWon = cards.SelectMany(card => card.GetWonCards(cards)).ToArray();

        var cardsWonCount = cards.Length + cardsWon.Length;

        while (cardsWon.Any())
        {
            cardsWon = cardsWon.SelectMany(card => card.GetWonCards(cards)).ToArray();
            cardsWonCount += cardsWon.Length;
        }

        return cardsWonCount;
    }

    public static int PileUp_Fast(string input)
    {
        var cards = GetCards(input);

        var cardsWithNumbers = cards.Select(c => (NumOf: 1, Card: c)).ToArray();

        for(int i = 0; i < cardsWithNumbers.Length; i++)
        {
            var (num, card) = cardsWithNumbers[i];
            var matches = card.GetMatches();

            for(int j = card.Id; j <= matches + i && j < cardsWithNumbers.Length; j++)
            {
                var numOf = num + cardsWithNumbers[j].NumOf;
                cardsWithNumbers[j] = cardsWithNumbers[j] with { NumOf = numOf };
            }
        }

        return cardsWithNumbers.Sum(c => c.NumOf);
    }

    public static int GetPoints(this Card card)
    {
        var drawnNumbers = card.Numbers.Where(x => card.WinningNumbers.Contains(x));
        var points = drawnNumbers.Aggregate(0, (acc, _) => acc += acc == 0 ? 1 : acc);

        return points;
    }

    public static int GetMatches(this Card card)
    {
        var drawnNumbers = card.Numbers.Where(x => card.WinningNumbers.Contains(x));
        var matches = drawnNumbers.Count();

        return matches;
    }

    public static Card[] GetWonCards(this Card card, IEnumerable<Card> cards)
    {
        var nextCards = cards.Skip(card.Id);
        var matches = card.GetMatches();
        var won = nextCards.Take(matches);

        return won.ToArray();
    }

    static Card[] GetCards(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var cards = lines.Select(GetCard);

        return cards.ToArray();
    }

    static Card GetCard(string line)
    {
        var data = line.Split(':', '|');

        var idString = data[0][5..];
        var id = int.Parse(idString);

        var winningNumbersStrings = data[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var winningNumbers = winningNumbersStrings.Select(int.Parse).ToArray();

        var numbersStrings = data[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var numbers = numbersStrings.Select(int.Parse).ToArray();

        var card = new Card(id, winningNumbers, numbers);

        return card;
    }
}