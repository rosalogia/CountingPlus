namespace Interpreter
open Parser.Types

module Types =
    let mutable outputString = ""
    type ProgramState = {VariableTable: (Name * Value) list}