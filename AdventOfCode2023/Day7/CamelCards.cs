using AdventOfCode2023.Day5;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day7;

internal enum HandType { HighCard, OnePair, TwoPair, ThreeOAK, FullHouse, FourOAK, FiveOAK }

internal record Hand(string Cards, int Bid);

internal static class CamelCards
{
    public static int Winnings(string input, bool useJolly = false)
    {
        var lines = input.Split(Environment.NewLine);
        var hands = lines.Select(GetHand);

        Func<Hand, HandType> handType = useJolly
            ? HandExtensions.GetHandTypeWithJollyListPattern
            : HandExtensions.GetHandTypeListPattern;
            //? HandExtensions.GetHandTypeWithJolly
            //: HandExtensions.GetHandType;

        var cardComparer = useJolly
            ? CardComparer.WithJolly
            : CardComparer.NoJolly;

        var rankedHands = hands.Sort(handType, cardComparer);
        var winnings = rankedHands.Select((hand, rank) => hand.Bid * (rank + 1));

        return winnings.Sum();
    }

    static Hand GetHand(string line)
    {
        var handParts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var hand = handParts[0];
        var bid = int.Parse(handParts[1]);

        return new(hand, bid);
    }
}


internal static class HandExtensions
{
    public static IEnumerable<Hand> Sort(this IEnumerable<Hand> hands, Func<Hand, HandType> handType, CardComparer cardComparer)
    {
        var orderByType = hands.OrderBy(handType);
        var orderByStrenght = orderByType.ThenBy(hand => hand.Cards, cardComparer);

        return orderByStrenght;
    }

    public static HandType GetHandType(this Hand hand)
    {
        var cards = hand.Cards;

        var type = cards.IsFiveOAK() ? HandType.FiveOAK
            : cards.IsFourOAK() ? HandType.FourOAK
            : cards.IsFullHouse() ? HandType.FullHouse
            : cards.IsThreeOAK() ? HandType.ThreeOAK
            : cards.IsTwoPair() ? HandType.TwoPair
            : cards.IsOnePair() ? HandType.OnePair
            : HandType.HighCard;

        return type;
    }

    public static HandType GetHandTypeListPattern(this Hand hand)
    {
        var cards = hand.Cards;
        var groupCards = cards            
            .GroupBy(x => x)
            .Select(x => x.Count())
            .OrderByDescending(x => x)
            .ToArray();

        var type = groupCards switch
        {
            [5] => HandType.FiveOAK,
            [4, ..] => HandType.FourOAK,
            [3, 2] => HandType.FullHouse,
            [3, ..] => HandType.ThreeOAK,
            [2, 2, ..] => HandType.TwoPair,
            [2, ..] => HandType.OnePair,
            _ => HandType.HighCard,
        };     

        return type;
    }

    public static HandType GetHandTypeWithJolly(this Hand hand)
    {
        var cards = hand.Cards;

        if (!cards.Contains('J')) return GetHandType(hand);

        var jollys = cards.Count(card => card == 'J');

        var cardsGroup = cards.GroupBy(x => x);
        var cardsGroupNoJolly = cardsGroup.Where(x => x.Key != 'J');
        var anyCardsGroupNoJollyOfSizeOne = cardsGroupNoJolly.Any(group => group.Count() == 1);

        var type = cardsGroup.Count() switch
        {
            1 or 2 => HandType.FiveOAK,
            3 => jollys switch
            {
                2 or 3 => HandType.FourOAK,
                _ => anyCardsGroupNoJollyOfSizeOne ? HandType.FourOAK : HandType.FullHouse,
            }, 
            4 => HandType.ThreeOAK,
            _ => HandType.OnePair
        };

        return type;
    }

    public static HandType GetHandTypeWithJollyListPattern(this Hand hand)
    {
        var cards = hand.Cards;

        if (!cards.Contains('J')) return GetHandType(hand);

        var groupCards = cards
            .OrderBy(x => CardComparer.CardValuesWithJolly[x])
            .GroupBy(x => x)
            .Select(x => x.Count())
            .ToArray();

        var type = groupCards switch
        {
            [_] or [_, _] => HandType.FiveOAK,
            [1, 2, 2] => HandType.FullHouse,
            [_, _, _] => HandType.FourOAK,
            [_, _, _, _] => HandType.ThreeOAK,
            _ => HandType.OnePair
        };

        return type;
    }

    static bool IsFiveOAK(this string cards) => cards.Distinct().Count() == 1;

    static bool IsFourOAK(this string cards) => cards.Any(card => cards.Count(x => x == card) == 4);

    static bool IsFullHouse(this string cards) => cards
        .GroupBy(card => card)
        .Select(cardGroup => cardGroup.Count())
        .All(count => count == 2 || count == 3);

    static bool IsThreeOAK(this string cards) => cards
        .GroupBy(card => card)
        .Select(cardGroup => cardGroup.Count())
        .Where(count => count != 1)
        .DefaultIfEmpty()
        .All(count => count == 3);

    static bool IsTwoPair(this string cards) => cards
        .GroupBy(card => card)
        .Select(cardGroup => cardGroup.Count())
        .Where(count => count == 2)
        .Count() == 2;

    static bool IsOnePair(this string cards) => cards
        .GroupBy(card => card)
        .Select(cardGroup => cardGroup.Count())
        .Where(count => count == 2)
        .Count() == 1;

    static bool IsHighCard(this string cards) => cards
        .GroupBy(card => card)
        .Select(cardGroup => cardGroup.Count())
        .All(count => count == 1);

}

internal class CardComparer : IComparer<string>
{
    private static readonly IDictionary<char, byte> _cardValueNoJolly = new Dictionary<char, byte>
    {
        { 'A', 14 },
        { 'K', 13 },
        { 'Q', 12 },
        { 'J', 11 },
        { 'T', 10 },
        { '9', 9 },
        { '8', 8 },
        { '7', 7 },
        { '6', 6 },
        { '5', 5 },
        { '4', 4 },
        { '3', 3 },
        { '2', 2 }
    };

    private static readonly IDictionary<char, byte> _cardValueWithJolly = new Dictionary<char, byte>
    {
        { 'A', 14 },
        { 'K', 13 },
        { 'Q', 12 },
        { 'J', 1 },
        { 'T', 10 },
        { '9', 9 },
        { '8', 8 },
        { '7', 7 },
        { '6', 6 },
        { '5', 5 },
        { '4', 4 },
        { '3', 3 },
        { '2', 2 }
    };

    private readonly IDictionary<char, byte> _cardValue;

    public static CardComparer NoJolly { get; } = new CardComparer(_cardValueNoJolly);

    public static CardComparer WithJolly { get; } = new CardComparer(_cardValueWithJolly);

    public static IImmutableDictionary<char, byte> CardValuesWithJolly { get; } = ImmutableDictionary.CreateRange(_cardValueWithJolly);

    private CardComparer(IDictionary<char, byte> cardValue)
    {
        _cardValue = cardValue;
    }

    public int Compare(string? x, string? y)
    {
        if (x is null && y is null) return 0;
        if (x is null) return -1;
        if (y is null) return 1;

        for (int i = 0; i < x.Length; i++)
        {
            if (x[i] == y[i]) continue;

            return _cardValue[x[i]] < _cardValue[y[i]] ? -1 : 1;
        }

        return 0;
    }
}
