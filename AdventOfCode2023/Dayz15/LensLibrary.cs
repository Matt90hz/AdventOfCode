using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz15;

readonly record struct Step(string Label, char Operation, int FocalLenght)
{
    public static Step Empty { get; } = new(string.Empty, ' ', 0);

    public int Box => LensLibrary.Hash(Label);
}

internal static class LensLibrary
{
    public static int FocusingPower(string input)
    {
        var steps = GetSteps(input);
        var boxes = new List<Step>[256]
            .Select(_ => new List<Step>())
            .ToArray();

        foreach (var step in steps)
        {
            HASHMAP(step, boxes);
        }

        var focusingPower = boxes.Select(FocusingPower);

        var sum = focusingPower.Sum();

        return sum;
    }

    public static int HashSum(string input)
    {
        var steps = input.Split(',');

        var hashes = steps.Select(Hash);

        var sum = hashes.Sum();

        return sum;
    }

    public static int Hash(string value)
    {
        int hash = 0;

        foreach (char x in value)
        {
            hash += x;
            hash *= 17;
            hash %= 256;
        }

        return hash;
    }

    static int FocusingPower(IEnumerable<Step> steps, int box)
    {
        var focusingPower = steps
            .Select((step, i) => (Step: step, Index: i + 1))
            .Aggregate(0, (acc, x) => acc += (box + 1) * x.Index * x.Step.FocalLenght);

        return focusingPower;
    }

    static void HASHMAP(Step step, List<Step>[] boxes)
    {
        switch (step.Operation) 
        {
            case '-':
                HASHMAPSubtract(step, boxes);
                break;
            default:
                HASHMAPAdd(step, boxes);
                break;
        };
    }

    static void HASHMAPAdd(Step step, List<Step>[] boxes)
    {
        var box = boxes[Hash(step.Label)];

        var lensToSwitch = box.FirstOrDefault(x => x.Label == step.Label, Step.Empty);

        var index = lensToSwitch == Step.Empty ? -1 : box.IndexOf(lensToSwitch);

        if(index == -1)
        {
            box.Add(step);
            return;
        }
            
        box.RemoveAt(index);
        box.Insert(index, step);
    }

    static void HASHMAPSubtract(Step step, List<Step>[] boxes)
    {
        var box = boxes[Hash(step.Label)];

        var lensToRemove = box.FirstOrDefault(x => x.Label == step.Label, Step.Empty);

        if (lensToRemove == Step.Empty) return;

        box.Remove(lensToRemove);
    }

    static IEnumerable<Step> GetSteps(string input)
    {
        var steps = input.Split(',').Select(x => 
        {
            var label = x[..x.IndexOfAny(new[] { '=', '-' })];
            var operation = x.EndsWith('-') || x.EndsWith('=') ? x[^1] : x[^2];
            var focusLenght = char.IsDigit(x[^1]) ? x.Last() - '0' : 0;

            return new Step(label, operation, focusLenght);
        });

        return steps;
    }
}
