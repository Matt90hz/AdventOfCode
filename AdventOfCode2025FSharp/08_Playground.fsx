open System.IO

//let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day08\input_test.txt"
let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day08\input.txt"

let coordinates =
    File.ReadAllLines(path)
    |> Seq.map (fun line ->
        let coo = line.Split(',')
        (float coo[0], float coo[1], float coo[2]))
    |> Seq.toList

let circuits = coordinates |> Seq.map (fun jbox -> Set.ofList [ jbox ]) |> Set.ofSeq

let dist (x, y, z) (x', y', z') =
    (x - x') ** 2.0 + (y - y') ** 2.0 + (z - z') ** 2.0

let connect circuits (jb, jb') =
    let circuit = Seq.find (Set.contains jb) circuits
    let circuit' = Seq.find (Set.contains jb') circuits

    if circuit = circuit' then
        circuits
    else
        circuits
        |> Set.remove circuit
        |> Set.remove circuit'
        |> Set.add (Set.union circuit circuit')

let largestCircuits =
    seq {
        for i in 0 .. (List.length coordinates - 2) do
            for j in (i + 1) .. (List.length coordinates - 1) do
                yield coordinates[i], coordinates[j]
    }
    |> Seq.sortBy ((<||) dist)
    |> Seq.take 1000
    |> Seq.fold connect circuits
    |> Seq.map Set.count
    |> Seq.sortDescending
    |> Seq.take 3
    |> Seq.reduce (*) // 80446

#time

let gigaCircuit =
    seq {
        for i in 0 .. (List.length coordinates - 2) do
            for j in (i + 1) .. (List.length coordinates - 1) do
                yield coordinates[i], coordinates[j]
    }
    |> Seq.sortBy ((<||) dist)
    |> Seq.scan
        (fun (_, circuits) (jb, jb') ->
            let circuit = Seq.find (Set.contains jb) circuits
            let circuit' = Seq.find (Set.contains jb') circuits

            if circuit = circuit' then
                (jb, jb'), circuits
            else
                (jb, jb'),
                circuits
                |> Set.remove circuit
                |> Set.remove circuit'
                |> Set.add (Set.union circuit circuit'))
        (((0.0, 0.0, 0.0), (0.0, 0.0, 0.0)), circuits)
    |> Seq.pick (fun (((x, _, _), (x', _, _)), circuits) ->
        if circuits |> Set.count |> (=) 1 then
            Some(x * x')
        else
            None) // 51294528

#time
