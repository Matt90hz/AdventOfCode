open System.IO

module Seq =
    let allPossiblePairs sequence =
        let cachedSequence = Seq.cache sequence

        cachedSequence
        |> Seq.indexed
        |> Seq.collect (fun (i, x) -> cachedSequence |> Seq.skip (i + 1) |> Seq.map (fun x' -> (x, x')))

//let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day09\input_test.txt"
let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day09\input.txt"

let coordinates =
    File.ReadAllLines(path)
    |> Seq.map (_.Split(',') >> Array.map int64)
    |> Seq.map (fun coo -> coo[0], coo[1])

let area (x, y) (x', y') =
    (abs (x - x') + 1L) * (abs (y - y') + 1L)

let largestRectangleArea =
    coordinates |> Seq.allPossiblePairs |> Seq.map ((<||) area) |> Seq.max // 50 4749672288

let greenRectangleArea =

    let corners =
        coordinates
        |> Seq.pairwise
        |> Seq.append [ Seq.head coordinates, Seq.last coordinates ]
        |> Seq.toList

    let verticalTiles =
        corners |> Seq.filter (fun ((x, _), (x', _)) -> x = x') |> Seq.toList

    let isOutside (x, y) (x', y') =
        let x, y = min x x' + 1L, min y y' + 1L

        verticalTiles
        |> Seq.filter (fun ((wx, wy), (_, wy')) -> x > wx && min wy wy' < y && max wy wy' > y)
        |> Seq.length
        |> fun len -> len % 2 = 0

    let isCrossed (x, y) (x', y') =
        let xmin = min x x' + 1L
        let ymin = min y y' + 1L
        let xmax = max x x' - 1L
        let ymax = max y y' - 1L

        let crossVertTiles ((wx, wy), (_, wy')) =
            let wymin = min wy wy'
            let wymax = max wy wy'

            xmin < wx
            && xmax > wx
            && ((wymax >= ymax && wymin <= ymax) || (wymin <= ymin && wymax >= ymin))

        let crossHoriTiles ((wx, wy), (wx', _)) =
            let wxmin = min wx wx'
            let wxmax = max wx wx'

            ymin < wy
            && ymax > wy
            && ((wxmax >= xmax && wxmin <= xmax) || (wxmin <= xmin && wxmax >= xmin))

        corners
        |> Seq.exists (fun corner ->
            match corner with
            | (x, _),(x', _) when x = x' -> crossVertTiles corner
            | _ -> crossHoriTiles corner)

    let isColored corners =
        (corners ||> isOutside || corners ||> isCrossed) |> not

    coordinates
    |> Seq.allPossiblePairs
    |> Seq.sortByDescending ((<||) area)
    |> Seq.find isColored
    ||> area // 24 1479665889