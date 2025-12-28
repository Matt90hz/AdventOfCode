open System.IO

module Seq =
    let allPossiblePairs sequence=
        let cachedSequence = Seq.cache sequence
        
        cachedSequence
        |> Seq.indexed
        |> Seq.collect (fun (i, x) ->
            cachedSequence
            |> Seq.skip (i + 1)
            |> Seq.map (fun x' -> (x, x')))

//let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day09\input_test.txt"
let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day09\input.txt"

let coordinates =
    File.ReadAllLines(path)
    |> Seq.map (_.Split(',') >> Array.map int64)
    |> Seq.map (fun coo -> coo[0], coo[1])

let area (x, y) (x', y') =
    (abs (x - x') + 1L) * (abs (y - y') + 1L)

let largetRectangleArea =
    coordinates
    |> Seq.allPossiblePairs
    |> Seq.map ((<||) area)
    |> Seq.max // 50 4749672288
