namespace Interpreter
open Parser.Types
open Interpreter.Types
open Interpreter.Operators

module Expressions =
    let lookupVariable state name =
        state.VariableTable
        |> List.filter (fun (n, _) -> n = name)
        |> List.map snd
        |> List.head

    let printValue value =
        match value with
        | Integer i -> outputString <- outputString + sprintf "%i\n" i
        | Bool b    -> outputString <- outputString + sprintf "%b\n" b
        | String s  -> outputString <- outputString + sprintf "%s\n" s

    let interpretConditionalValue = function
        | Bool b    -> b
        | _         -> true
    
    let interpretIntegerValue = function
        | Integer i -> i
        | _         -> 0


    let rec applyOperator state op e1 e2 =
        let reduceToValue exp =
            match exp with
            | Literal value         -> value
            | Variable name         -> lookupVariable state name
            | Operation (x, op', y) -> applyOperator state op' x y

        mapOperator op (reduceToValue e1) (reduceToValue e2)

    let rec evaluateExpression state = function
        | Literal value -> value
        | Variable name -> lookupVariable state name
        | Operation (e1, op, e2) -> applyOperator state op e1 e2

    let evaluateCondition cond state =
        cond |> evaluateExpression state |> interpretConditionalValue