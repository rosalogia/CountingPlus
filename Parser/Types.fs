namespace Parser

module Types =
    type Name = string

    type Value =
        | Integer   of int
        | String    of string
        | Bool      of bool
        | Float     of float
        | ValArray  of Value array

    type Operator =
        | ADD
        | SUBTRACT
        | MULTIPLY
        | DIVIDE
        | MODULUS
        | GT
        | LT
        | GTE
        | LTE
        | EQ
        | NEQ
        | NOT
        | AND
        | OR
        | SCONCAT
    type Expr =
        | Literal   of Value
        | Variable  of Name
        | Operation of Expr * Operator * Expr

    type Statement =
        | READ      of Name
        | DISPLAY   of bool * Expr
        | SET       of Name * Expr
        | COMPUTE   of Name * Expr
        | IF        of Expr * Block * Block option
        | WHILE     of Expr * Block
        | DOWHILE   of Block * Expr
        | REPEAT    of Block * Expr
        | HALT
    and Block = Statement list
