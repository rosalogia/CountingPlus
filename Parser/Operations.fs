namespace Parser

module Operations =
    open FParsec
    open Parser.Types
    open Parser.Base

    let numberOperatorParser = OperatorPrecedenceParser<Expr, Unit, Unit>()
    let numexpr = numberOperatorParser.ExpressionParser
    let numterm = (pnumber .>> spaces |>> Literal <|> pvariable) <|> between (pword "(") (pword ")") numexpr
    numberOperatorParser.TermParser <- numterm
    
    let boolOperatorParser = OperatorPrecedenceParser<Expr,Unit,Unit>()
    let boolexpr = boolOperatorParser.ExpressionParser
    let boolterm = (pboolval .>> spaces |>> Literal <|> numexpr) <|> between (pword "(") (pword ")") boolexpr
    boolOperatorParser.TermParser <- boolterm
    
    let stringOperatorParser = OperatorPrecedenceParser<Expr, Unit, Unit>()
    let sexpr = stringOperatorParser.ExpressionParser
    let sterm = (pliteral <|> pvariable) <|> between (pword "(") (pword ")") sexpr
    stringOperatorParser.TermParser <- sterm

    let numericalOperators = [
        (">", 1, GT)
        ("<", 1, LT)
        (">=", 1, GTE)
        ("<=", 1, LTE)
        ("==", 1, EQ)
        ("!=", 1, NEQ)
        ("+", 2, ADD)
        ("-", 2, SUBTRACT)
        ("*", 3, MULTIPLY)
        ("/", 3, DIVIDE)
        ("%", 3, MODULUS)
    ]
    
    let boolOperators = [
            ("and", 1, AND)
            ("or", 1, OR)
        ]
    
    let stringOperators = [
        ("++", 1, SCONCAT)
    ]
    
    let operators = [
        (numberOperatorParser, numericalOperators)
        (boolOperatorParser, boolOperators)
        (stringOperatorParser, stringOperators)
    ]
    
    operators
    |> List.iter (fun (parser, operatorList) ->
        operatorList
        |> List.iter (fun (symbol, precedence, operator) ->
            parser.AddOperator(InfixOperator(symbol, spaces, precedence, Associativity.Left, (createOperation operator)))))

    let poperation  = choice [ boolexpr ; numexpr ; sexpr]

    let pexpression: Parser<Expr, Unit> = choice [poperation ; pliteral ; pvariable]