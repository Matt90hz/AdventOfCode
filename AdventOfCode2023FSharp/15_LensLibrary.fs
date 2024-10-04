namespace AdventOfCode2023FSharp

module LensLibrary =

    open System

    let hash (x: string) = 
        let op (acc: int) (c: char) = 
            ((acc + (int)c) * 17) % 256
        Seq.fold op 0 x

    let hashSum (x: string) =
        x.Split(',')
        |> Seq.map hash 
        |> Seq.sum

    type Operation =
        | Add
        | Subtract

    type Step = 
        {   Label: string
            Operation: Operation
            FocalLenght: int }

        member this.Box = hash this.Label
    
        static member Empty = 
            {   Label = " "
                Operation = Add
                FocalLenght = 0 }

    let steps (input: string) =
        let step (x: string) =
            let label = x[..x.IndexOfAny([| '='; '-' |])]
            let operation = if x.EndsWith('-') then Add else Subtract
            let lastChar = Seq.last x
            let focus = if Char.IsDigit(lastChar) then (int)(Char.GetNumericValue(lastChar)) else 0
        
            {   Label = label
                Operation = operation
                FocalLenght = focus }
       
        input.Split(',')
        |> Seq.map step

    let hashmap (steps: Step seq) =
        let boxes = [|for i in 1..256 -> [] |]
        let add (step: Step) = 
            let box = boxes[hash step.Label]
            let lens = box |> List.tryFindIndex (fun x -> step.Label = x.Label)
            let index = if lens.IsSome then lens.Value else -1
            if index = -1 then 
                box |> List.insertAt box.Length step
            else 
                box 
                |> List.removeAt index 
                |> List.insertAt index step 
        let subtract (step: Step) =
            let box = boxes[hash step.Label]
            let lens = box |> List.tryFindIndex (fun x -> step.Label = x.Label)
            if lens.IsSome then
                List.removeAt lens.Value box
            else box
        steps