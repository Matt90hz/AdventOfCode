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

let height (tower: Position Set) =
    match tower with
    | _ when Set.isEmpty tower -> 0
    | _ -> tower |> Seq.map (fun { X = _; Y = y } -> y) |> Seq.max |> (+) 1

let crest (tower: Position Set) =
        (try tower |> Seq.where (fun x -> x.X = 0) |> Seq.maxBy (fun x -> x.Y) with | :? ArgumentException -> { X = 0; Y = 0 }),
        (try tower |> Seq.where (fun x -> x.X = 1) |> Seq.maxBy (fun x -> x.Y) with | :? ArgumentException -> { X = 0; Y = 0 }),
        (try tower |> Seq.where (fun x -> x.X = 2) |> Seq.maxBy (fun x -> x.Y) with | :? ArgumentException -> { X = 0; Y = 0 }),
        (try tower |> Seq.where (fun x -> x.X = 3) |> Seq.maxBy (fun x -> x.Y) with | :? ArgumentException -> { X = 0; Y = 0 }),
        (try tower |> Seq.where (fun x -> x.X = 4) |> Seq.maxBy (fun x -> x.Y) with | :? ArgumentException -> { X = 0; Y = 0 }),
        (try tower |> Seq.where (fun x -> x.X = 5) |> Seq.maxBy (fun x -> x.Y) with | :? ArgumentException -> { X = 0; Y = 0 }),
        (try tower |> Seq.where (fun x -> x.X = 6) |> Seq.maxBy (fun x -> x.Y) with | :? ArgumentException -> { X = 0; Y = 0 })

let (|Coordinates|) (rock: Rock) =
    match rock with
    | Vert -> [ { X = 2; Y = 3 }; { X = 2; Y = 2 }; { X = 2; Y = 1 }; { X = 2; Y = 0 } ]
    | Horiz -> [ { X = 2; Y = 0 }; { X = 3; Y = 0 }; { X = 4; Y = 0 }; { X = 5; Y = 0 } ]
    | Cube -> [ { X = 2; Y = 0 }; { X = 2; Y = 1 }; { X = 3; Y = 0 }; { X = 3; Y = 1 } ]
    | Cross ->
        [ { X = 2; Y = 1 }
          { X = 3; Y = 1 }
          { X = 4; Y = 1 }
          { X = 3; Y = 0 }
          { X = 3; Y = 2 } ]
    | Leg ->
        [ { X = 2; Y = 0 }
          { X = 3; Y = 0 }
          { X = 4; Y = 0 }
          { X = 4; Y = 1 }
          { X = 4; Y = 2 } ]

let shift (dir: Dir) (steps: int) (postion: Position) =
    match dir with
    | Lx -> { postion with X = postion.X - steps }
    | Rx -> { postion with X = postion.X + steps }
    | Up -> { postion with Y = postion.Y + steps }
    | Dw -> { postion with Y = postion.Y - steps }

let cycle (items: 'a array) =
    Seq.initInfinite (fun i -> items[i % items.Length])

let next (items: 'a seq) =
    let enumerator = items.GetEnumerator()

    fun () ->
        enumerator.MoveNext() |> ignore
        enumerator.Current

let printTower (tower: Position Set) =
    if tower |> Set.isEmpty then
        ()
    else
        let array = Array.init (height tower) (fun _ -> Array.create 7 '.')
        tower |> Seq.iter (fun x -> array[x.Y][x.X] <- '#')
        array |> Seq.map (fun x -> new String(x)) |> Seq.rev |> Seq.iter (printfn "%s")

let rocksTowerHeight input = //3068 3206 //1514285714288 1602881844347

    let mutable seen = Set.empty
    let mutable heights = Map.empty
    let mutable jetIndex = 0
    let mutable rockIndex = 0
    let crest = [|0;0;0;0;0;0;0|]

    let jetArray = 
        input
        |> Seq.map (fun x -> if x = '<' then Lx else Rx)
        |> Seq.toArray

    let jets =
        jetArray
        |> cycle
        |> next

    let dropRock (tower: Position Set) (rock: Rock) =

        let (|Settled|_|) (coordinates: Position list) =
            coordinates |> List.exists (fun c -> c.Y = -1 || tower |> Set.contains c)

        let (|Stuck|_|) (coordinates: Position list) =
            coordinates
            |> List.exists (fun c -> c.X = 7 || c.X = -1 || tower |> Set.contains c)

        let move (coordinates: Position list) =
            //let dir = jets[jet % jets.Length]
            //printfn "%s" "Rock is about to move"
            //printfn "%A" jetDirection
            //printTower (coordinates @ tower)
            jetIndex <- jetIndex + 1
            match coordinates |> List.map (shift (jets ()) 1) with
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

        rockIndex <- rockIndex + 1
        rock 
        |> function Coordinates c -> c
        |> List.map (shift Dir.Up (height tower + 3))
        |> fall
        |> fun coordinates -> 
            // I need to rewrite the entire function to just use the values of mem and collect tem untill you find a seen and then repeat
            // the operation tho figure out the loop. Also done like this breaks the previous implementation
            coordinates
            |> Seq.iter (fun c -> crest[c.X] <- c.Y)

            let baseLine = crest |> Array.min
            let mem = jetIndex % jetArray.Length, rock, crest |> Array.map ( fun h -> h - baseLine)

            if Set.contains mem seen then
                let prevh, prevr = heights[mem]
                let currh = coordinates |> Set.ofList |> height
                let currr = rockIndex
                printfn "%A this is found at index %i the tower is %i the previous was %A" mem rockIndex currh heights[mem]

                let repeted, excidingRocks = Math.DivRem(1_000_000_000_000L - int64 prevr, int64 currr - int64 prevr)
                printfn "%i %i" repeted excidingRocks

                let excideingHeight, _ = heights.Values |> Seq.find (fun (_, r') -> r' = prevr + excidingRocks)
                let y = repeted * (int64 currh - int64 prevh) + int64 excideingHeight
                printfn "%i is this it" y
                //printTower tower
                coordinates
            else
                seen <- seen |> Set.add mem
                heights <- heights |> Map.add mem ((height (coordinates |> Set.ofList)), rockIndex)
                coordinates
        |> List.fold (fun t x -> Set.add x t) tower

    [| Horiz; Cross; Leg; Vert; Cube |]
    |> cycle
    |> Seq.take 5000
    |> Seq.fold dropRock Set.empty
    |> height
