type Cube =
    | Red
    | Green
    | Blue

type Set = 
    | Cube of int

let possibleGames sets =
    sets
    |> List.groupBy (fun set -> set)
