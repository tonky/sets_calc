module Parser

open System

type Op = Sum | Int | Diff

type Expr = 
    | Data of string
    | Set of Op * Expr list

let (|Op|_|) = function
    | "SUM" -> Some(Sum)
    | "INT" -> Some(Int)
    | "DIF" -> Some(Diff)
    | _ -> None

let rec (|Expression|_|) = function
    | "[" :: Op op :: Args(args, tail) -> Some(Set(op, args), tail)
    | _ -> None
and (|Args|_|) = function
    | "]" :: tail -> Some([], tail)
    | Expression(e, Args(args, tail)) -> Some(e :: args, tail)
    | f :: Args(args, tail) -> Some(Data(f) :: args, tail)
    | _ -> None

let loadFile name =
    let lines = List.ofSeq (IO.File.ReadLines(__SOURCE_DIRECTORY__ + "/" + name))
    Set.ofList (List.map Int32.Parse lines)

let rec eval = function
    | Data f -> loadFile f
    | Set(Sum, args) -> Set.unionMany (List.map eval args)
    | Set(Int, args) -> Set.intersectMany (List.map eval args)
    | Set(Diff, h::t) -> Set.difference (eval h) (Set.unionMany (List.map eval t))
    | Set(_) -> Set.empty