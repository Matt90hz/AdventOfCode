let rotate gift = gift

let flip gift = gift

let place gift pos = []

let rec isSolvable space gifts =
    match gifts with
    | [] -> true
    | [ gift ] ->
        seq {
            gift
            rotate gift
            flip gift
        }
        |> Seq.exists (fun gift ->
            space
            |> Seq.exists (fun pos -> place gift pos |> Seq.forall (fun coo -> space |> Seq.contains coo)))
    | gift :: rest ->
        seq {
            gift
            rotate gift
            flip gift
        }
        |> Seq.map (fun gift -> space |> Seq.map (fun pos -> place gift pos))
        |> Seq.filter (fun gift -> gift |> Seq.forall (fun pos -> space |> Seq.contains pos))
        |> Seq.exists (fun gift -> isSolvable (space |> Seq.except gift) rest)

let combine gift gift' =
    seq {
        gift
        rotate gift
        flip gift
    }
    |> Seq.map (fun gift -> 
        seq {
            gift'
            rotate gift'
            flip gift'
        }
        |> Seq.map (fun gift' -> 
            gift + gift')) // this must move around the two gifts in all possible position to fit one in the other