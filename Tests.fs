module Tests

open Xunit
open Parser
    
let checkParse str lst =
    match tokenize str with
    | Expression(exp, []) ->
        // printfn "parsed: %A" exp
        // printfn "eval  : %A" (eval exp)
        // printfn "diff  : %A" (Set.difference (eval exp) (Set.ofList lst))
        Assert.True(Set.isEmpty (Set.difference (eval exp) (Set.ofList lst)))
    | _ -> failwith("didn't parse")

[<Fact>]
let ``Basic set ops`` () =
    checkParse "[ SUM a.txt ]" [1;2;3]
    checkParse "[ SUM a.txt b.txt c.txt ]" [1;2;3;4;5]
    checkParse "[ INT a.txt b.txt c.txt ]" [3]
    checkParse "[ DIF a.txt b.txt c.txt ]" [1]

[<Fact>]
let ``Basic ops combinations`` () =
    checkParse "[ SUM a.txt [ INT b.txt ] ]" [1;2;3;4]
    checkParse "[ INT [ SUM b.txt c.txt ] a.txt ]" [2; 3]
    checkParse "[ DIF a.txt [ INT b.txt c.txt ] c.txt ]" [1; 2]

[<Fact>]
let ``Other tests`` () =
    checkParse "[ SUM [ DIF a.txt b.txt c.txt ] [ INT b.txt c.txt ] ]" [1;3;4]
    checkParse "[ DIF a.txt [ SUM b.txt c.txt ] [ INT a.txt b.txt ] c.txt b.txt ]" [1]
    checkParse "[ SUM a.txt [ INT a.txt b.txt ] a.txt] ]" [1;2;3]