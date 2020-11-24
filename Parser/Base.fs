namespace Parser

module Base =
    open FParsec
    open Parser.Types

    let pword s = pstring s .>> spaces

    let pidentifier: Parser<string, Unit> =
        many1Satisfy2 (System.Char.IsLetter) (System.Char.IsLetterOrDigit)
        .>> spaces

    let pbool: Parser<bool, Unit> =
        pstring "true" <|> pstring "false"
        |>> System.Boolean.Parse

    let pstringliteral: Parser<string, Unit> =
        let words = pchar '\"' >>. manyCharsTill anyChar (pchar '\"')
        words |>> string .>> spaces

    let pnumber: Parser<Value, Unit> =
        let numberFormat =     NumberLiteralOptions.AllowMinusSign
                           ||| NumberLiteralOptions.AllowFraction
                           ||| NumberLiteralOptions.AllowExponent

        numberLiteral numberFormat "number"
        |>> fun nl ->
                if nl.IsInteger then Integer (int nl.String)
                else Float (float nl.String)
            
    let pstringval: Parser<Value, Unit>     =   pstringliteral  |>> String
    let pboolval:   Parser<Value, Unit>     =   pbool           |>> Bool

    let pvalue =
        choice [
            pnumber
            pstringval
            pboolval
        ]

    let createOperation op x y = Operation (x, op, y)

    let pvariable   = pidentifier |>> Variable
    let pliteral    = pvalue |>> Literal

