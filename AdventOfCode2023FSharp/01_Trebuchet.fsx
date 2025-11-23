
open System
open System.IO

let trebuchetCoordinates path = 

    let coordinate line = 
        let firstDigit = Seq.find Char.IsDigit line
        let lastDigit = Seq.findBack Char.IsDigit line
        int $"{firstDigit}{lastDigit}"

    File.ReadAllLines(path)
    |> Seq.sumBy coordinate

@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Day1\Input.txt"
|> File.ReadAllLines
|> Seq.sumBy (fun line -> 
    let firstDigit = Seq.find Char.IsDigit line
    let lastDigit = Seq.findBack Char.IsDigit line
    int $"{firstDigit}{lastDigit}")
|> fun result -> 
    match result = 54951 with
    | true  -> "You got it!"
    | false -> "You failed..."