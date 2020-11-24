namespace Parser

module Interface =
    open FParsec
    open Parser.Statements
    open System.Text
    
    /// <summary>
    /// Runs a given parser on the text within a specified ASCII encoded file and prints out the resulting AST
    /// <param name="p">The parser to be run on the input file</param>
    /// <param name="fpath">The path of the file to run the parser on</param>
    let test p fpath =
        match runParserOnFile p () fpath Encoding.ASCII with
        | Success(result, _, _)   -> printfn "Success: %A" result
        | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg

    /// <summary>
    /// Runs a given parser on a string and prints out the resulting AST
    /// <param name="p">The parser to be run on the input file</param>
    /// <param name="str">The string to run the parser on</param>
    let testSingle p str =
        match run p str with
        | Success (result, _, _) -> printfn "Success: %A" result
        | Failure (error, _, _)  -> printfn "Failure: %s" error

    /// <summary>
    /// Runs a parser on a string and returns the result as a Result<Block, string> rather than printing the result
    /// <param name="str">The string to parse</param>
    /// <returns>A result type that contains the parsed AST in case of success and a parser error message in case of failure</returns>
    let parseUserInput str =
        match runParserOnString (many pstatement) () "User Input" str with
        | Success (result, _, _)    -> Core.Ok (result)
        | Failure (error, _, _)     -> Core.Error (error)