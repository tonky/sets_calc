module Parser

open System
open FParsec

type Ops = Sum | Int | Diff

type Expr = 
    | Data of string
    | Set of Ops * Expr list
    
let parse str =
    let evalue, evalueRef = createParserForwardedToRef<Expr, unit>()

    let pop = stringReturn "SUM " Sum <|> stringReturn "INT " Int <|> stringReturn "DIF " Diff
    let file = regex "\w+\.\w{3}" |>> Data
    let set = between (pstring "[ ") (pstring "]") (pop .>>. many1 (file <|> evalue .>> spaces)) |>> Set

    do evalueRef := choice [file; set]

    match run set str with
    | Success(result, _, _)   -> Some result
    | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg; None

let loadFile name =
    let lines = List.ofSeq (IO.File.ReadLines(__SOURCE_DIRECTORY__ + "/" + name))
    Set.ofList (List.map Int32.Parse lines)

let rec eval = function
    | Data f -> loadFile f
    | Set(Sum, args) -> Set.unionMany (List.map eval args)
    | Set(Int, args) -> Set.intersectMany (List.map eval args)
    | Set(Diff, h::t) -> Set.difference (eval h) (Set.unionMany (List.map eval t))
    | Set(_) -> Set.empty