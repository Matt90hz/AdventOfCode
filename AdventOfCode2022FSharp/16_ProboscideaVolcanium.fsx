open System
open System.IO
open System.Collections.Generic

let valvesAndTunnels =
    //@"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day16\input_test.txt"
    @"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day16\input.txt"
    |> File.ReadAllLines
    |> Seq.map (fun line ->
        line[line.IndexOf("Valve ") + 6 .. line.IndexOf(" has") - 1],
        (line[line.IndexOf("rate=") + 5 .. line.IndexOf(';') - 1] |> int,
         line[line.IndexOf("lead") + 15 ..].Split(',', StringSplitOptions.TrimEntries)))
    |> Map.ofSeq

let maxPressureRelease =
    let timeLimit = 30
    let memo = Dictionary<(string * int * int), int>()

    let rec maxPressureRelease valve time released (closeValves: string Set) =
        let memoKey = valve, time, released

        if memo.ContainsKey(memoKey) then
            memo[memoKey]
        elif time >= timeLimit || Set.count closeValves = 0 then
            released
        else
            let pressure, nextValves = valvesAndTunnels[valve]

            let maxRelease =
                nextValves
                |> Seq.collect (fun nextValve ->
                    seq {
                        yield maxPressureRelease nextValve (time + 1) released closeValves

                        if time < 29 && pressure <> 0 && Set.contains valve closeValves then
                            yield
                                maxPressureRelease
                                    nextValve
                                    (time + 2)
                                    (released + (timeLimit - time - 1) * pressure)
                                    (Set.remove valve closeValves)
                    })
                |> Seq.max

            memo[memoKey] <- maxRelease
            maxRelease

    let closed =
        valvesAndTunnels |> Map.filter (fun k (p, n) -> p <> 0) |> Map.keys |> Set.ofSeq

    maxPressureRelease "AA" 0 0 closed // 1651 1991
