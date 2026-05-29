module Day17

open System

type Position = { X: int; Y: int }

type Rock =
    | Horiz
    | Vert
    | Cross
    | Cube
    | Leg

type Dir =
    | Lx
    | Rx
    | Up
    | Dw

let height (tower: Position list) = 
    match tower with
    | [] -> 0 
    | tower -> 
        tower 
        |> Seq.map (fun { X = _; Y = y } -> y) 
        |> Seq.max
        |> (+) 1

let (|Coordinates|) (rock: Rock) =
    match rock with
    | Vert -> [
        { X = 2; Y = 3 }
        { X = 2; Y = 2 }
        { X = 2; Y = 1 }
        { X = 2; Y = 0 } ]
    | Horiz -> [
        { X = 2; Y = 0 }
        { X = 3; Y = 0 }
        { X = 4; Y = 0 }
        { X = 5; Y = 0 } ]
    | Cube -> [
        { X = 2; Y = 0 }
        { X = 2; Y = 1 }
        { X = 3; Y = 0 }
        { X = 3; Y = 1 } ]
    | Cross -> [
        { X = 2; Y = 1 }
        { X = 3; Y = 1 }
        { X = 4; Y = 1 }
        { X = 3; Y = 0 }
        { X = 3; Y = 2 } ]
    | Leg -> [
        { X = 2; Y = 0 }
        { X = 3; Y = 0 }
        { X = 4; Y = 0 }
        { X = 4; Y = 1 }
        { X = 4; Y = 2 } ]

let shift (dir: Dir) (steps: int) (postion: Position)  = 
    match dir with
    | Lx -> { postion with X = postion.X - steps }
    | Rx -> { postion with X = postion.X + steps }
    | Up -> { postion with Y = postion.Y + steps }
    | Dw -> { postion with Y = postion.Y - steps }

let printTower (tower: Position list) =
    if tower |> List.isEmpty then () else

    let array = Array.init (height tower) (fun _ -> Array.create 7 '.')

    tower |> Seq.iter (fun x -> array[x.Y][x.X] <- '#')

    array
    |> Seq.map (fun x -> new String(x))
    |> Seq.rev
    |> Seq.iter (printfn "%s")

let rocksTowerHeight input = //3068 3206
    let rocks = [Horiz; Cross; Leg; Vert; Cube]
    let moves = input |> Seq.map (fun x -> if x ='<' then Lx else Rx) |> List.ofSeq
    let mutable jet = 0 
    
    let dropRocks (n: int) =
        let rec dropRocks (dropped: int) (tower: Position list) = 
            let dropRock (Coordinates coordinates: Rock) (tower: Position list) =

                let (|Settled|_|) (coordinates: Position list) =         
                    coordinates |> List.exists (fun c -> c.Y = -1 || tower |> List.contains c) 

                let (|Stuck|_|) (coordinates: Position list) =
                    coordinates|> List.exists (fun c -> c.X = 7 || c.X = -1 || tower |> List.contains c)
                
                let move (coordinates: Position list) =
                    let jetDirection = moves[jet % moves.Length]
                    //printfn "%s" "Rock is about to move"
                    //printfn "%A" jetDirection
                    //printTower (coordinates @ tower)

                    jet <- jet + 1

                    match coordinates |> List.map (shift jetDirection 1) with
                    | Stuck -> coordinates
                    | next -> next

                let drop (coordinates: Position list) =
                    match coordinates |> List.map (shift Dir.Dw 1) with
                    | Settled -> None
                    | next -> Some next

                let rec fall (coordinates: Position list) =
                    let moved = move coordinates
                    let dropped = drop moved

                    match dropped with
                    | Some x -> fall x
                    | None -> moved

                coordinates 
                |> List.map (shift Dir.Up (height tower + 3))
                |> fall
                |> List.append tower
                
            //printfn "%s" "ROCK LANDED"
            //printTower tower
            //printfn "%s" ""

            match dropped with
            | _ when dropped = n -> tower
            | _ -> dropRocks (dropped + 1) (dropRock (rocks[dropped % rocks.Length]) tower)

        dropRocks 0 []
    
    dropRocks 2022 |> height

let rocksTowerInsaneHeight input =
    let rocks = [
            [0,0; 1,0; 2,0; 3,0];
            [1,0; 0,1; 1,1; 1,2; 1,2];
            [0,0; 1,0; 2,0; 2,1; 2, 2];
            [0,0; 0,1; 0,2; 0,3];
            [0,0; 0,1; 1,0; 1,1];
        ]

    let jets = input |> Seq.map(fun x -> if x = '<' then -1 else +1) |> List.ofSeq

    0