open System.IO

let sensorsAndBeacons =
    @"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day15\input.txt"
    //@"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day15\input_test.txt"
    |> File.ReadAllLines
    |> Array.map (fun line -> 
        let sx = 
            line 
            |> Seq.skipWhile (fun c -> c <> 'x')
            |> Seq.skip 2
            |> Seq.takeWhile (fun c -> c <> ',') 
            |> fun digits -> new string(Seq.toArray digits)
            |> int

        let sy = 
            line 
            |> Seq.skipWhile (fun c -> c <> 'y')
            |> Seq.skip 2
            |> Seq.takeWhile (fun c -> c <> ':') 
            |> fun digits -> new string(Seq.toArray digits)
            |> int

        let by = 
            line 
            |> Seq.rev
            |> Seq.takeWhile (fun c -> c <> '=') 
            |> Seq.rev
            |> fun digits -> new string(Seq.toArray digits)
            |> int

        let bx = 
            line 
            |> Seq.rev
            |> Seq.skipWhile (fun c -> c <> ',')
            |> Seq.skip 1
            |> Seq.takeWhile (fun c -> c <> '=')
            |> Seq.rev
            |> fun digits -> new string(Seq.toArray digits)
            |> int

        sx, sy, bx, by)

let rec combineRanges (ranges: (int*int) list)=
    let combineTwoRange (r: int*int) (r': int*int)=
        match r, r' with
        | (x, y), (x', y') when y < x' || x > y' -> None
        | (x, y), (x', y') -> Some(min x x', max y y')

    match ranges with
    | [] | [_] -> ranges
    | [r; r'] -> 
        match (combineTwoRange r r') with 
        | Some r'' -> [r'']
        | None -> ranges
    | r :: x ->
        let combined = x |> Seq.indexed |> Seq.tryPick (fun (i, r') ->
            match combineTwoRange r r' with
            | Some c -> Some(i, c)
            | None -> None)
        match combined with
        | Some (i, c) -> x |> List.removeAt i |> fun x -> combineRanges (c :: x)
        | None -> r :: combineRanges x

let noBeconsPositions=
    sensorsAndBeacons
    |> Seq.choose (fun (sx, sy, bx, by) -> 
        let y = 2_000_000
        let d = abs(sx - bx) + abs(sy - by)
        let dy = abs(sy - y) 
        let dx = abs(d - dy)    
        if dy > d then None else Some(sx - dx, sx + dx))
    |> Seq.toList
    |> combineRanges
    |> List.sumBy (fun (x, y) -> abs(x - y)) // = 4_827_924

let onlyPossibleBeaconTuningFrequency = 
    let minCoo, maxCoo = 0, 4_000_000
    //let minCoo, maxCoo = 0, 20
    let tuningFrequency (x, y) = int64 x * 4_000_000L + int64 y

    let gap = function
        | [(x, y); (x', y')] when x < x' -> y + 1 
        | [(x, y); (x', y')] when x > x' -> y' + 1 
        | _ -> -69

    seq { minCoo..maxCoo }
    |> Seq.map (fun x -> 
        sensorsAndBeacons
        |> Seq.choose (fun (sx, sy, bx, by) -> 
            let d = abs(sx - bx) + abs(sy - by)
            let dx = abs(sx - x) 
            let dy = abs(d - dx)    
            if dx > d then None else Some(sy - dy, sy + dy))
        |> Seq.toList
        |> combineRanges
        |> fun r -> x, r)
    |> Seq.find (fun (y,r) -> List.length r > 1)
    |> fun (x, r) -> x, r, tuningFrequency (x, gap r)
  
    // 12977110973564
    // 3244277 2973564
       