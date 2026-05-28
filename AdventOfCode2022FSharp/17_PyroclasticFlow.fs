module Day17

open System

type Pos = { X: int; Y: int }

type Rock =
    | Horiz
    | Vert
    | Cross
    | Cube
    | Leg

type Dir =
    | Lx
    | Rx

let printTower (tower: Pos list) =
    if tower |> List.isEmpty then () else

    let h = tower |> Seq.map (fun x -> x.Y) |> Seq.max
    let w = tower |> Seq.map (fun x -> x.X) |> Seq.max

    let array = Array.init (h + 1) (fun _ -> Array.create 7 '.')

    tower
    |> Seq.iter (
        fun x -> array[x.Y][x.X] <- '#')

    array
    |> Seq.map (fun x -> new String(x))
    |> Seq.rev
    |> Seq.iter (printfn "%s")

let rocksTowerHeight input = //3068 3206
    let rocks = [Horiz; Cross; Leg; Vert; Cube]
    let moves = input |> Seq.map (function | '<' -> Lx | '>' -> Rx | _ -> Exception "What?" |> raise) |> List.ofSeq
    let towerHeight = function | [] -> 0 | t -> t |> Seq.map (fun { X = _; Y = y } -> y) |> Seq.max |> (+) 1
    let mutable jet = 0 
    
    let dropRocks (n: int) =
        let rec dropRocks (dropped: int) (tower: Pos list) = 
            let dropRock (r: Rock) (tower: Pos list) =
                let mutable pos =
                    let top = towerHeight tower
                    match r with
                    | Vert -> [
                        { X = 2; Y = top + 6 }
                        { X = 2; Y = top + 5 }
                        { X = 2; Y = top + 4 }
                        { X = 2; Y = top + 3 } ]
                    | Horiz -> [
                        { X = 2; Y = top + 3 }
                        { X = 3; Y = top + 3 }
                        { X = 4; Y = top + 3 }
                        { X = 5; Y = top + 3 } ]
                    | Cube -> [
                        { X = 2; Y = top + 3 }
                        { X = 2; Y = top + 4 }
                        { X = 3; Y = top + 3 }
                        { X = 3; Y = top + 4 } ]
                    | Cross -> [
                        { X = 2; Y = top + 4 }
                        { X = 3; Y = top + 4 }
                        { X = 4; Y = top + 4 }
                        { X = 3; Y = top + 3 }
                        { X = 3; Y = top + 5 } ]
                    | Leg -> [
                        { X = 2; Y = top + 3 }
                        { X = 3; Y = top + 3 }
                        { X = 4; Y = top + 3 }
                        { X = 4; Y = top + 4 }
                        { X = 4; Y = top + 5 } ]

                let move (rockPos: Pos list) =
                    let jetDirection = moves[jet % moves.Length]
                    //printfn "%s" "Rock is about to move"
                    //printfn "%A" jetDirection
                    //printTower (pos @ tower)

                    let isNotAllowed (rockPos: Pos list) =
                        rockPos |> List.exists ( fun x -> x.X = 7 || x.X = -1 || tower |> List.contains x)

                    jet <- jet + 1

                    match jetDirection with
                    | Rx -> 
                        let nextRockPos = rockPos |> List.map( fun x -> { x with X = x.X + 1 }) 
                        if isNotAllowed nextRockPos then rockPos else nextRockPos
                    | Lx -> 
                        let nextRockPos = rockPos |> List.map( fun x -> { x with X = x.X - 1 })
                        if isNotAllowed nextRockPos then rockPos else nextRockPos

                let drop (p: Pos list) =
                    let nextRockPos = p |> List.map (fun pos -> {pos with Y = pos.Y - 1})
                    if nextRockPos |> List.exists(fun pos -> pos.Y = -1 || tower |> List.contains pos) then None else Some nextRockPos

                Seq.initInfinite (fun i -> i)
                |> Seq.tryFind (fun dir ->
                    pos <- pos |> move
                    //printfn "%s" "Rock has been moved"
                    //printTower (pos @ tower)
                    let next = pos |> drop
                    pos <- next |> Option.defaultValue pos
                    next.IsNone)
                |> ignore

                pos @ tower
                
            //printfn "%s" "ROCK LANDED"
            //printTower tower
            //printfn "%s" ""

            match dropped with
            | _ when dropped = n -> tower
            | _ -> dropRocks (dropped + 1) (dropRock (rocks[dropped % rocks.Length]) tower)

        dropRocks 0 []
    
    dropRocks 2022 |> towerHeight
