open Parser

[<EntryPoint>]
let main argv =
    let print set =
        List.map (printfn "%d") (Set.toList set)

    match (parse (String.concat " " argv)) with
    | Some(exp) ->  print (eval exp) |> ignore
    | _ -> printfn "Can't parse string: '%A'" argv

    0