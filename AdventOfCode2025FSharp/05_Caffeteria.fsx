open System.IO

//let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day05\input_test.txt"
let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day05\input.txt"

let ranges =
    File.ReadAllLines(path)
    |> Seq.takeWhile (fun line -> String.length line > 1)
    |> Seq.map (fun line ->
        line.Split('-')
        |> Seq.pairwise
        |> Seq.map (fun (s, f) -> int64 s, int64 f)
        |> Seq.exactlyOne)
    |> Seq.toList

let ids =
    File.ReadAllLines(path)
    |> Array.skipWhile (fun line -> String.length line > 1)
    |> Array.skip 1
    |> Array.map int64

let (|Fresh|Spoiled|) (id, (s, e)) =
    if id >= s && id <= e then Fresh id else Spoiled id

let freshIngredients =
    ids
    |> Seq.choose (fun id ->
        ranges
        |> Seq.tryPick (fun range ->
            match (id, range) with
            | Fresh id -> Some id
            | _ -> None))
    |> Seq.length // 3 513

let allFreshIngredients =
    let rec combineRanges ranges =

        let combineTwoRange r r' =
            match r, r' with
            | (x, y), (x', y') when y < x' - 1L || x > y' + 1L -> None
            | (x, y), (x', y') -> Some(min x x', max y y')

        match ranges with
        | []
        | [ _ ] -> ranges
        | [ r; r' ] ->
            match (combineTwoRange r r') with
            | Some r'' -> [ r'' ]
            | None -> ranges
        | r :: rest ->
            let combined =
                rest
                |> Seq.indexed
                |> Seq.tryPick (fun (i, r') ->
                    match combineTwoRange r r' with
                    | Some c -> Some(i, c)
                    | None -> None)

            match combined with
            | Some(i, c) -> rest |> List.removeAt i |> (fun x -> combineRanges (c :: x))
            | None -> r :: combineRanges rest

    combineRanges ranges |> Seq.sumBy (fun (x, y) -> y - x + 1L) //14 339668510830757
