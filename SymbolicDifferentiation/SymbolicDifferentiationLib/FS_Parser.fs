﻿// * **********************************************************************************************
// * Copyright (c) Edmondo Pentangelo. 
// *
// * This source code is subject to terms and conditions of the Microsoft Public License. 
// * A copy of the license can be found in the License.html file at the root of this distribution. 
// * By using this source code in any fashion, you are agreeing to be bound by the terms of the 
// * Microsoft Public License.
// *
// * You must not remove this notice, or any other, from this software.
// * **********************************************************************************************

#light

open FS_ParserLib;
open SymbolicDifferentiation.Core.Tokens;
            
let digitVal = parse { let! c = digitOrLetter
                       return new SymbolicDifferentiation.Core.AST.Expression(c) }                                                  
                           
let addOp = (Literal (TokenBuilder.Symbol("+"))) (fun x y -> x + y) <|> (Literal (TokenBuilder.Symbol("-"))) (fun x y -> x - y)
let mulOp = (Literal (TokenBuilder.Symbol("*"))) (fun x y -> x * y) <|> (Literal (TokenBuilder.Symbol("/"))) (fun x y -> x / y) 
let expOp = (Literal (TokenBuilder.Symbol("^"))) (fun x y -> SymbolicDifferentiation.Core.AST.Expression.Pow(x, y)) 
let rec expr = chainl1 term addOp
and term = chainl1 factor mulOp
and factor = chainr1 part expOp
and part = app <|> digitVal <|> paren
and paren = parse { let! _ = Literal (TokenBuilder.Symbol("(")) 0
                    let! e = expr
                    let! _ = Literal (TokenBuilder.Symbol(")")) 0
                    return e }
and app =   parse { let! _ = Literal (TokenBuilder.Variable("fun")) 0
                    let! name = sat (fun x -> x.Type = MatchType.Variable )
                    let! _ = Literal (TokenBuilder.Symbol("(")) 0
                    let! e = expr
                    let! _ = Literal (TokenBuilder.Symbol(")")) 0
                    return SymbolicDifferentiation.Core.AST.FunctionApplicationExpression.Create(name, e) }
 
let extractParseResult consumed  = 
    match consumed with
    | Consumed (_, result) -> result
    
let extractOutput result  = 
    match result with
    | Success (state, _, _) -> state
    | Fail _ -> failwith "parse failed"
                        
let Execute tokens = expr( ParserState(Seq.to_list tokens , 0) ) |> extractParseResult |> extractOutput


