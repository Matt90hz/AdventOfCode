#r "nuget: Microsoft.Z3.x64"

open Microsoft.Z3
open System
open System.IO
open System.Collections.Generic

let path =
    match fsi.CommandLineArgs |> List.ofArray with
    | _ :: "-t" :: _ -> @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day10\input_test.txt"
    | _ -> @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day10\input.txt"

let machines =
    File.ReadLines(path)
    |> Seq.map (fun line ->
        let diagram =
            line[line.IndexOf('[') .. line.IndexOf(']')]
            |> _.Trim('[', ']')
            |> _.ToCharArray()
            |> Array.map (function
                | '#' -> 1
                | _ -> 0)

        let buttons =
            line[line.IndexOf('(') .. line.LastIndexOf(')')]
            |> _.Split()
            |> Seq.map (fun x -> x.Trim('(', ')').Split(',') |> Array.map int)
            |> List.ofSeq

        let joltage =
            line[line.IndexOf('{') .. line.IndexOf('}')]
            |> _.Trim('{', '}')
            |> _.Split(',')
            |> Array.map int

        diagram, buttons, joltage)
    |> Seq.toList

let pressesToConfigureIndicator =

    let configureIndicator diagram (buttons: int array list) =

        let cache = Dictionary<int, int array seq>()
        let initalState = Array.replicate (Array.length diagram) 0

        let pressButton (state: int array) (button: int seq) =
            state
            |> Array.copy
            |> fun newState ->
                button |> Seq.iter (fun i -> newState[i] <- newState[i] ^^^ 1)
                newState

        let rec pressButtons times =
            match cache.TryGetValue(times) with
            | true, chached -> chached
            | _ ->
                match times with
                | 1 -> buttons |> Seq.map (pressButton initalState)
                | _ ->
                    pressButtons (times - 1)
                    |> Seq.collect (fun state -> buttons |> Seq.map (pressButton state))
                    |> Seq.distinct
                    |> fun states ->
                        cache.Add(times, states)
                        states

        let rec configureMachine presses =
            pressButtons presses
            |> Seq.exists (Array.forall2 (=) diagram)
            |> function
                | false -> configureMachine (presses + 1)
                | true -> presses

        configureMachine 1

    machines |> Seq.map (fun (d, b, _) -> configureIndicator d b) |> Seq.sum

printfn "%i" pressesToConfigureIndicator //7 419

let pressesToConfigureJoltage =

    let configureJoltage buttons (joltage: int array) =
        use context = new Context()

        let buttonsSymbols =
            buttons
            |> List.map (Array.map string >> String.Concat >> context.MkIntConst)
            |> Seq.cast<ArithExpr>
            |> Seq.toArray

        let ``press > 0`` =
            buttonsSymbols |> Seq.map (fun button -> context.MkGe(button, context.MkInt(0)))

        let ``sum of button that contains i = joltage[i]`` =

            let sumButtons index =
                buttons
                |> Seq.indexed
                |> Seq.filter (fun (_, button) -> button |> Array.contains index)
                |> Seq.map (fun (i, _) -> buttonsSymbols[i])
                |> Seq.cast<ArithExpr>
                |> context.MkAdd

            joltage |> Seq.mapi (fun i j -> context.MkEq(sumButtons i, context.MkInt(j)))


        let solver = context.MkOptimize()

        solver.Add(``press > 0``)
        solver.Add(``sum of button that contains i = joltage[i]``)

        let sumOfPress = context.MkAdd(buttonsSymbols)

        ignore <| solver.MkMinimize(sumOfPress)
        ignore <| solver.Check()

        solver.Model.Eval(sumOfPress).ToString() |> int

    machines |> Seq.map (fun (_, b, j) -> configureJoltage b j) |> Seq.sum

printfn "%A" pressesToConfigureJoltage //33 18369