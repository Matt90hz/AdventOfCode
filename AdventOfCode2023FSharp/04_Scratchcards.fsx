open System
open System.IO

let input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Day4\input.txt")

let parseValue (str:string) = 
    str
    |> String.filter Char.IsDigit 
    |> int

let parseValues (str:string) =
    str.Split(' ', StringSplitOptions.RemoveEmptyEntries)
    |> Array.map parseValue
     
let parseCard (line:string) =
    match line.Split(':', '|') with
    | [| id; winning; numbers |] -> (parseValue id, parseValues winning, parseValues numbers)
    | _ -> failwith "fuck"

let parseCards (input:string) =
    input.Split(Environment.NewLine)
    |> Array.map parseCard
    
let getPoints (_, numbers, winning) = 
    numbers
    |> Array.filter (fun x -> Array.contains x winning)
    |> Array.fold (fun a _ -> if a = 0 then 1 else a + a ) 0

let calculatePoints =
    input
    |> parseCards
    |> Array.map getPoints
    |> Array.sum

let pileUp = // this is not finished
    let cards = parseCards input

    let getMatches (_, num, win) =
        num
        |> Array.filter (fun x -> Array.contains x win)
        |> Array.length

    let getWonCards card cards =
        let (id, _, _) = card 
        let matches = getMatches card
        let next = cards |> Array.skip id

        next |> Array.take matches

    cards
    |> Array.collect (fun card -> getWonCards card cards)

calculatePoints = 26914