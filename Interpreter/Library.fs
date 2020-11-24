namespace Interpreter

module PlusPlusSharp =
    open Parser.Interface
    open Interpreter.Types
    open Parser.Types
    open Interpreter.Statements

    let innerRunProgram statements state =
        runStatements statements state |> ignore
        (outputString, 0)
        
    let execute codeString x i =
        match parseUserInput codeString with
        | Ok result ->
            let (output, _ ) = innerRunProgram result {VariableTable = [("x", Integer x); ("i", Integer i)]}
            outputString <- ""
            printfn "%s" codeString
            printfn "%A" result
            sprintf "%s" output
        | Error err ->
            err