module Tests

open Xunit
open Parser
    
[<Fact>]
let ``Parse test`` () =
    match parseExpr (tokenize "[ SUM [ DIF a.txt b.txt c.txt ] [ INT b.txt c.txt ] ]") with
    | Some exp -> Assert.True(Set.isEmpty (Set.difference (eval exp) (Set.ofList [1;3;4])))
    | _ -> failwith("didn't parse")

    match parseExpr (tokenize "[ DIF a.txt [ SUM b.txt c.txt ] [ INT a.txt b.txt ] c.txt ]") with
    | Some exp -> Assert.True(Set.isEmpty (Set.difference (eval exp) (Set.ofList [1])))
    | _ -> failwith("didn't parse")

    match parseExpr (tokenize "[ SUM a.txt [ INT a.txt b.txt ] a.txt] ]") with
    | Some exp -> Assert.True(Set.isEmpty (Set.difference (eval exp) (Set.ofList [1;2;3])))
    | _ -> failwith("didn't parse")