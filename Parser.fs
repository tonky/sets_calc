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

let (|Op|_|) = function
    | "SUM" -> Some(Sum)
    | "INT" -> Some(Int)
    | "DIF" -> Some(Diff)
    | _ -> None

let (|Datafile|_|) = function
    | Filename f :: tail -> Some(Data(f), tail)
    | _ -> None

let rec (|Expression|_|) = function
    | "[" :: Op op :: Args(args, tail) -> Some(Set(op, args), tail)
    | _ -> None
and (|Args|_|) = function
    | "]" :: tail -> Some([], tail)
    | Datafile(f, Args(args, tail)) -> Some(f :: args, tail)
    | Expression(e, Args(args, tail)) -> Some(e :: args, tail)
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