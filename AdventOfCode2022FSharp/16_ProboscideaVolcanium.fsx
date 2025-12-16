open System
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

                        if time < 29 && Set.contains valve closeValves then
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

let maxPressureReleaseWithElephant =
    let timeLimit = 26
    let memo = Dictionary<(string * string * int * int), int>()

    let rec maxPressureRelease meValve elValve time released (closeValves: string Set) =
        let memoKey = meValve, elValve, time, released

        if memo.ContainsKey(memoKey) then
            memo[memoKey]
        elif time >= timeLimit || Set.count closeValves = 0 then
            released
        else
            let mePressure, meNextValves = valvesAndTunnels[meValve]
            let elPressure, elNextValves = valvesAndTunnels[elValve]

            let nextValves =
                meNextValves
                |> Seq.collect (fun mv -> Seq.map (fun ev -> mv, ev) elNextValves)
                |> Seq.toArray

            let meIsClosed, elIsClosed =
                Set.contains meValve closeValves, Set.contains elValve closeValves

            let pressureReleases =
                seq {

                    yield!
                        nextValves
                        |> Seq.map (fun (mv, ev) -> maxPressureRelease mv ev (time + 1) released closeValves)

                    if meIsClosed then
                        let cv = Set.remove meValve closeValves
                        let t = time + 1
                        let pr = released + ((timeLimit - time - 1) * mePressure)
                        yield! elNextValves |> Seq.map (fun ev -> maxPressureRelease meValve ev t pr cv)

                    if elIsClosed then
                        let cv = Set.remove elValve closeValves
                        let t = time + 1
                        let pr = released + ((timeLimit - time - 1) * elPressure)
                        yield! meNextValves |> Seq.map (fun me -> maxPressureRelease me elValve t pr cv)

                    if meIsClosed && elIsClosed && time < 25 && meValve <> elValve then
                        let cv = closeValves |> Set.remove meValve |> Set.remove elValve
                        let t = time + 2

                        let pr =
                            released
                            + ((timeLimit - time - 1) * mePressure)
                            + ((timeLimit - time - 1) * elPressure)

                        yield! nextValves |> Seq.map (fun (mv, ev) -> maxPressureRelease mv ev t pr cv)

                }


            let maxRelease = Seq.max pressureReleases

            memo[memoKey] <- maxRelease
            maxRelease

    let closed =
        valvesAndTunnels |> Map.filter (fun k (p, n) -> p <> 0) |> Map.keys |> Set.ofSeq

    maxPressureRelease "AA" "AA" 0 0 closed // 1707 2705