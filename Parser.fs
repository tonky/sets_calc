module Parser

open System
open System.Text.RegularExpressions

type Op = Sum | Int | Diff

type Expr = 
    | Data of string
    | Set of Op * Expr list

let (|Filename|_|) input =
   let m = Regex.Match(input,"(\w+\.\w{3})") 
   if (m.Success) then Some m.Groups.[0].Value else None  

let (|Expression|_|) = function
    | "[" :: "SUM" :: tail -> Some(Sum, tail)
    | "[" :: "INT" :: tail -> Some(Int, tail)
    | "[" :: "DIF" :: tail -> Some(Diff, tail)
    | _ -> None

let splitExpr tokens =
    let count x = Seq.filter ((=) x) >> Seq.length

    let rec loop l (xs, ys) =
        if count "[" xs > 0 && count "[" xs = count "]" xs then (xs, ys)
        else
            match l with
            | head::tail -> loop tail (xs @ [head], tail)
            | _ -> (xs, ys)

    loop tokens ([], [])

let rec parseExpr = function
    | Expression (op, tail) -> Some(Set(op, parseArgs tail))
    | _ -> None
and parseArgs tokens =
    match tokens with
    | Expression (_) ->
        // lookahead for the nearest expression end
        let exprTokens, tail = splitExpr tokens

        match parseExpr exprTokens with
        | Some(exp) -> exp :: parseArgs tail
        | None -> []

    | Filename f :: tail -> Data(f) :: parseArgs tail
    | _ -> []

let loadFile name =
    let lines = List.ofSeq (System.IO.File.ReadLines(__SOURCE_DIRECTORY__ + "/" + name))
    Set.ofList (List.map Int32.Parse lines)

let rec eval (exp: Expr) : Set<int> =
    match exp with
    | Data f -> loadFile f
    | Set(_, []) -> Set.empty
    | Set(_, [e]) -> eval e
    | Set(Sum, args) -> Set.unionMany (List.map eval args)
    | Set(Int, args) -> Set.intersectMany (List.map eval args)
    | Set(Diff, h::t) -> Set.difference (eval h) (Set.unionMany (List.map eval t))

let tokenize (input: string) =
    List.ofArray (input.Split [|' '|])