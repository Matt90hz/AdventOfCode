open System.IO

let sensorsAndBeacons path =
    path
    |> File.ReadAllLines
    |> Array.map (fun line ->
        let sx =
            line
            |> Seq.skipWhile (fun c -> c <> 'x')
            |> Seq.skip 2
            |> Seq.takeWhile (fun c -> c <> ',')
            |> fun digits -> new string (Seq.toArray digits)
            |> int

        let sy =
            line
            |> Seq.skipWhile (fun c -> c <> 'y')
            |> Seq.skip 2
            |> Seq.takeWhile (fun c -> c <> ':')
            |> fun digits -> new string (Seq.toArray digits)
            |> int

        let by =
            line
            |> Seq.rev
            |> Seq.takeWhile (fun c -> c <> '=')
            |> Seq.rev
            |> fun digits -> new string (Seq.toArray digits)
            |> int

        let bx =
            line
            |> Seq.rev
            |> Seq.skipWhile (fun c -> c <> ',')
            |> Seq.skip 1
            |> Seq.takeWhile (fun c -> c <> '=')
            |> Seq.rev
            |> fun digits -> new string (Seq.toArray digits)
            |> int

        sx, sy, bx, by)

let rec combineRanges (ranges: (int * int) list) =

    let combineTwoRange (r: int * int) (r': int * int) =
        match r, r' with
        | (x, y), (x', y') when y < x' - 1 || x > y' + 1 -> None
        | (x, y), (x', y') -> Some(min x x', max y y')

    match ranges with
    | []
    | [ _ ] -> ranges
    | [ r; r' ] ->
        match (combineTwoRange r r') with
        | Some r'' -> [ r'' ]
        | None -> ranges
    | r :: x ->
        let combined =
            x
            |> Seq.indexed
            |> Seq.tryPick (fun (i, r') ->
                match combineTwoRange r r' with
                | Some c -> Some(i, c)
                | None -> None)

        match combined with
        | Some(i, c) -> x |> List.removeAt i |> (fun x -> combineRanges (c :: x))
        | None -> r :: combineRanges x

let noBeconsPositions y path =
    sensorsAndBeacons path
    |> Seq.choose (fun (sx, sy, bx, by) ->
        let d = abs (sx - bx) + abs (sy - by)
        let dy = abs (sy - y)
        let dx = abs (d - dy)
        if dy > d then None else Some(sx - dx, sx + dx))
    |> Seq.toList
    |> combineRanges
    |> List.sumBy (fun (x, y) -> abs (x - y))


let onlyPossibleBeaconTuningFrequency fieldSize path =
    let minCoo, maxCoo = 0, fieldSize
    let sb = sensorsAndBeacons path

    let gap ranges =
        ranges |> Seq.map (fun (_, y) -> y) |> Seq.min

    let tuningFequency (x, y) = int64 x * 4_000_000L + int64 y

    seq { minCoo..maxCoo }
    |> Seq.pick (fun x ->
        sb
        |> Seq.choose (fun (sx, sy, bx, by) ->
            let d = abs (sx - bx) + abs (sy - by)
            let dx = abs (sx - x)
            let dy = abs (d - dx)
            if dx > d then None else Some(sy - dy, sy + dy))
        |> Seq.toList
        |> combineRanges
        |> fun ranges -> if List.length ranges > 1 then Some(x, gap ranges) else None)
    |> tuningFequency

let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day15\input.txt"
let testPath = @"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day15\input_test.txt"

printfn "%i expected %i" (noBeconsPositions 10 testPath) 26
printfn "%i expected %i" (noBeconsPositions 2_000_000 path) 4_827_924
printfn "%i expected %i" (onlyPossibleBeaconTuningFrequency 20 testPath) 56000010
printfn "%i expected %i" (onlyPossibleBeaconTuningFrequency 4_000_000 path) 12977110973564L
