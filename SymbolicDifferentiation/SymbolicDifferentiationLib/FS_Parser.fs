// * **********************************************************************************************
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
open SymbolicDifferentiation.Core.AST;
            
let digitVal = parse { let! c = digitOrLetter
                       return new SymbolicDifferentiation.Core.AST.Expression(c) }                                                  
                           
let addOp = (Literal (TokenBuilder.Symbol("+"))) (fun x y -> x + y) <|> (Literal (TokenBuilder.Symbol("-"))) (fun x y -> x - y)
let mulOp = (Literal (TokenBuilder.Symbol("*"))) (fun x y -> x * y) <|> (Literal (TokenBuilder.Symbol("/"))) (fun x y -> x / y) 
let expOp = (Literal (TokenBuilder.Symbol("^"))) (fun x y -> Expression.Pow(x, y)) 
let cmpOp = (Literal (TokenBuilder.Symbol(">"))) (fun x y -> Expression.op_GreaterThan(x,y)) <|> (Literal (TokenBuilder.Symbol("<"))) (fun x y -> Expression.op_LessThan(x,y))

let rec expr v = p1 v
and p1 =         p2 |> chainl1 <| cmpOp
and p2 =         p3 |> chainl1 <| addOp
and p3 =         p4 |> chainl1 <| mulOp
and p4 =         p5 |> chainr1 <| expOp
and p5 = attempt declWithArgs <|> decl <|> attempt cond <|> app <|> digitVal <|> paren

//Parenthesis around expressions to define precedence
and paren = parse { let! _ = Literal (TokenBuilder.Symbol("(")) 0
                    let! e = expr
                    let! _ = Literal (TokenBuilder.Symbol(")")) 0
                    return e }
                    
//Function application
and app =   parse { let! name = FollowedBy(sat (fun x -> x.Type = MatchType.Variable ), (fun r -> sat (fun x -> x.Value.Equals "("))) 
                    let! _ = Literal (TokenBuilder.Symbol("(")) 0
                    let! e = sepBy expr ((Literal (TokenBuilder.Symbol(","))) 0)
                    let! _ = Literal (TokenBuilder.Symbol(")")) 0
                    return SymbolicDifferentiation.Core.AST.FunctionApplicationExpression.Create(name, Array.of_list e) }
 
//Conditional expression
and cond =   parse { let! _ = Literal (TokenBuilder.Symbol("(")) 0
                     let! condition = expr
                     let! _ = Literal (TokenBuilder.Symbol(")")) 0
                     let! _ = Literal (TokenBuilder.Symbol("?")) 0
                     let! success = expr
                     let! _ = Literal (TokenBuilder.Symbol(":")) 0
                     let! failure = expr
                     return SymbolicDifferentiation.Core.AST.ConditionalExpression.Create(condition, success, failure) }

//Function declaration
and decl =   parse { let! name = FollowedBy(sat (fun x -> x.Type = MatchType.Variable ), (fun r -> sat (fun x -> x.Value.Equals "="))) 
                     let! _ = Literal (TokenBuilder.Symbol("=")) 0
                     let! e = expr
                     return SymbolicDifferentiation.Core.AST.FunctionDeclarationExpression.Create(name, e) }
                     
//Function declaration
and declWithArgs =   parse { let! name = FollowedBy(sat (fun x -> x.Type = MatchType.Variable ), (fun r -> sat (fun x -> x.Value.Equals "("))) 
                             let! _ = Literal (TokenBuilder.Symbol("(")) 0
                             let! args = sepBy expr ((Literal (TokenBuilder.Symbol(","))) 0)
                             let! _ = Literal (TokenBuilder.Symbol(")")) 0
                             let! _ = Literal (TokenBuilder.Symbol("=")) 0
                             let! body = expr
                             return SymbolicDifferentiation.Core.AST.FunctionDeclarationExpression.CreateWithArgs(name, args, body) }

//Parse multiple expressions divided by newlines
let exprs = parse { let! e = sepBy expr ((Literal (new Token(MatchType.EOL, "\n"))) 0)
                    return e }               

let extractParseResult consumed  = 
    match consumed with
    | Consumed (_, result) -> result
    
let extractOutput result  = 
    match result with
    | Success (state, _, _) -> state
    | Fail _ -> failwith "parse failed"
                        
let Execute tokens = exprs( ParserState(Seq.to_list tokens , 0) ) |> extractParseResult |> extractOutput


