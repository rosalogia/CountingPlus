namespace Interpreter
open Parser.Types

module Operators =
    let numOperation op x y =
        match x with
        | Integer a ->  match y with
                        | Float b -> Float (op (float a) b)
                        | Integer b -> Integer (int (op (float a) (float b)))
        | Float   a ->  match y with
                        | Integer b -> Float (op a (float b))
                        | Float b   -> Float (op a b)

    
    let stringOperation x y =
        match x with
        | Integer a ->  match y with
                        | String b -> String (sprintf "%i%s" a b)
        | Float a ->    match y with
                        | String b -> String (sprintf "%f%s" a b)
        | Bool a ->     match y with
                        | String b -> String (sprintf "%b%s" a b)
        | String a ->   match y with
                        | Integer b  -> String (sprintf "%s%i" a b)
                        | Float b    -> String (sprintf "%s%f" a b)
                        | Bool b     -> String (sprintf "%s%b" a b)
                        | String b   -> String (sprintf "%s%s" a b)

    let add         = numOperation (+)
    let subtract    = numOperation (-)
    let modulus     = numOperation (%)
    let multiply    = numOperation (*)
    let divide      = numOperation (/)

    let compare comparison x y =
        match x with
        | Integer a ->  match y with
                        | Integer b -> Bool (comparison (float a) (float b))
                        | Float   b -> Bool (comparison (float a) b)
        | Float a ->    match y with
                        | Integer b -> Bool (comparison a (float b))
                        | Float   b -> Bool (comparison a b)
                        
    let junction f x y =
        match x with
        | Bool a -> match y with
                    | Bool b -> Bool (f a b)

    let conjunction = junction (&&)
    let disjunction = junction (||)

    let gt  = compare (>)
    let lt  = compare (<)
    let gte = compare (>=)
    let lte = compare (<=)
    let eq  = compare (=)
    let neq = compare (<>)

    let mapOperator = function
        | ADD       -> add
        | SUBTRACT  -> subtract
        | MULTIPLY  -> multiply
        | DIVIDE    -> divide
        | MODULUS   -> modulus
        | GT        -> gt
        | LT        -> lt
        | GTE       -> gte
        | LTE       -> lte
        | EQ        -> eq
        | NEQ       -> neq
        | AND       -> conjunction
        | OR        -> disjunction
        | SCONCAT   -> stringOperation