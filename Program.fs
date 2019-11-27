open Parser

[<EntryPoint>]
let main argv =
    let print set =
        List.map (printfn "%d") (Set.toList set)

    match parseExpr (List.ofArray argv) with
    | Some exp -> print (eval exp) |> ignore
    | None -> printfn "Can't parse string: '%A'" argv
    0