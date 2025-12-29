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

#time
let mutable i = 0

let greenRectangleArea =
    let connect (x, y) (x', y') =
        if x = x' then
            seq { min y y' .. max y y' } |> Seq.map (fun y -> x, y)
        else
            seq { min x x' .. max x x' } |> Seq.map (fun x -> x, y)

    let perimeter coordinates =
        coordinates
        |> Seq.pairwise
        |> Seq.append [ Seq.head coordinates, Seq.last coordinates ]
        |> Seq.collect ((<||) connect)
        |> Set.ofSeq

    let tilesPerimeter = perimeter coordinates

    let verticalWalls =
        coordinates
        |> Seq.pairwise
        |> Seq.append [ Seq.head coordinates, Seq.last coordinates ]
        |> Seq.filter (fun ((x, _), (x', _)) -> x = x')
        |> Seq.collect (fun (coo, coo') -> (coo, coo') ||> connect |> Seq.filter (fun c -> c <> coo && c <> coo'))
        |> Set.ofSeq

    let horizzontalWalls =
        coordinates
        |> Seq.pairwise
        |> Seq.append [ Seq.head coordinates, Seq.last coordinates ]
        |> Seq.filter (fun ((_, y), (_, y')) -> y = y')
        |> Seq.collect (fun (coo, coo') -> (coo, coo') ||> connect |> Seq.filter (fun c -> c <> coo && c <> coo'))
        |> Set.ofSeq

    let xMin = (coordinates |> Seq.map (fun (x, _) -> x) |> Seq.min) - 1L
    let yMin = (coordinates |> Seq.map (fun (_, y) -> y) |> Seq.min) - 1L
    let xMax = (coordinates |> Seq.map (fun (x, _) -> x) |> Seq.max) + 1L
    let yMax = (coordinates |> Seq.map (fun (_, y) -> y) |> Seq.max) + 1L

    let isOutSide (x, y) (x', y') =
        let ray, walls = 
            let xToMin = (x - xMin, (xMin, y), verticalWalls)
            let xToMax = (xMax - x, (xMax, y), verticalWalls)
            let yToMin = (y - yMin, (x, yMin), horizzontalWalls)
            let yToMax = (yMax - y, (x, yMax), horizzontalWalls)

            [xToMin; xToMax; yToMin; yToMax]
            |> List.minBy (fun (dist, _, _) -> dist)
            |> fun (_, border, wall) -> connect (x, y) border, wall

        ray
        |> Set.ofSeq
        |> Set.intersect walls
        |> Set.count
        |> fun len -> len % 2 = 0

    let isCrossed (x, y) (x', y') =
        let xmin = min x x' + 1L
        let ymin = min y y' + 1L
        let xmax = max x x' - 1L
        let ymax = max y y' - 1L
        let lb = xmin, ymin
        let lr = xmax, ymin
        let tr = xmax, ymax
        let tl = xmin, ymax

        perimeter [ lb; lr; tr; tl ]
        |> Seq.exists (fun tile -> Set.contains tile tilesPerimeter)

    let isColored corners =
        printfn "%s" $"{i} ==> {corners}"
        i <- i + 1
        (corners ||> isOutSide || corners ||> isCrossed) |> not

    coordinates
    |> Seq.allPossiblePairs
    |> Seq.sortByDescending ((<||) area)
    |> Seq.find isColored
    ||> area // 1479665889

#time

printfn "%A" greenRectangleArea

//let grid =
//    Array2D.init (int xMax + 1) (int yMax + 1) (fun x y ->
//        if Set.contains (int64 x, int64 y) verticalWalls then
//            'X'
//        else
//            '.')

//printfn "%A" grid
