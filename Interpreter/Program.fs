open Parser.Interface
open Interpreter.Types
open Interpreter.Statements

let runProgram statements state =
    runStatements statements state |> ignore
    (outputString, 0)


[<EntryPoint>]
let main argv =
    argv
    |> String.concat "+"
    |> fun input -> match parseUserInput input with
                    | Ok result ->
                        let (output, status) = runProgram result {VariableTable = []}
                        printf "%s" output
                        status
                    | Error err ->
                        printfn "Error: %A" err
                        1
