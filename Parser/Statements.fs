namespace Parser

module Statements =
    open FParsec
    open Parser.Types
    open Parser.Base
    open Parser.Operations

    let pstatement, pstatementref = createParserForwardedToRef<Statement, Unit>()
    
    let pblock: Parser<Statement list, Unit> = between (pword "{") (pword "}") (many pstatement)

    let pread: Parser<Statement, Unit> =
        pword "READ"
        >>. pidentifier
        |>> READ
    
    let pdisplay: Parser<Statement, Unit> =
        pword "print"
        >>. between (pword "(") (pword ")") pexpression
        |>> DISPLAY

    let pset: Parser<Statement, Unit> =
        // let ident = between (pword "SET") (pword "TO") pidentifier
        let ident = pidentifier .>> pword "="
        let setval = pexpression

        pipe2 ident setval (fun name value -> SET (name, value))

//    let pcompute: Parser<Statement, Unit> =
//        let ident = between (pword "COMPUTE") (pword "AS") pidentifier
//
//        pipe2 ident pexpression (fun name expression -> COMPUTE (name, expression))

    let pif: Parser<Statement, Unit> =
        let parseElseOrEnd =
            ((pword "}" >>. pword "else") <|> pword "}")
        
        let parseElse =
            pword "else"
            >>. pblock

        let condition =
            pword "if"
            >>. pexpression
        let inner1 = pblock
        let elseOption = opt parseElse
        
        pipe3 condition inner1 elseOption (fun cond in1 in2 -> IF (cond, in1, in2))



    let pwhile: Parser<Statement, Unit> =
        let condition =
            pword "while"
            >>. pexpression
        
        let inner = pblock

        pipe2 condition inner (fun cond block -> WHILE (cond, block))

    let pdowhile: Parser<Statement, Unit> =
        let inner =
            (pword "do")
            >>. pblock
        
        let condition =
            pword "while"
            >>. pexpression

        pipe2 inner condition (fun block cond -> DOWHILE (block, cond))

    //TODO("Deprecate this")
    let prepeat: Parser<Statement, Unit> =
        let inner =
            (pword "REPEAT")
            >>. manyTill (pstatement .>> spaces) (pword "UNTIL")
        
        let condition = between (pstring "(") (pstring ")") pexpression

        pipe2 inner condition (fun block cond -> REPEAT (block, cond))
    
    let phalt: Parser<Statement, Unit> =
        pword "HALT"
        >>. preturn HALT

    do pstatementref := choice [
        pread
        pdisplay
        pif
        // pcompute
        pwhile
        pdowhile
        prepeat
        phalt
        pset
    ]
