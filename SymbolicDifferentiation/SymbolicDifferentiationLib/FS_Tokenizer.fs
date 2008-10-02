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

open System.Text.RegularExpressions
open SymbolicDifferentiation.Core.Tokens

let private (|ParseRegex|_|) pattern input = 
    let regex = Regex(pattern)
    let matched = regex.Match(input)
    if matched.Success then
        Some matched
    else
        None
        
        
let is_a_token_of_type = fun value -> fun tokenType -> Token.Create(tokenType, value)
        
let private parse s =     
       match s with     
       | ParseRegex "^[0-9]+"               result ->  result |> is_a_token_of_type <|  MatchType.Number
       | ParseRegex "^[A-Za-z]+\d*"            result ->  result |> is_a_token_of_type <|  MatchType.Variable
       | ParseRegex "^[\\^\\+\\*\\(\\)\\,\\=]"    result ->  result |> is_a_token_of_type <|  MatchType.Symbol
       | ParseRegex "^[\n]"                   result ->  result |> is_a_token_of_type <| MatchType.EOL
       | ParseRegex "\s+"                   result ->  Token.Whitespace
       | _ -> failwith "unknown token"
  

let rec Tokenize input =   seq {
                            if (not (input.Equals(""))) then
                                    let result = parse(input)  
                                    if (not (result.Type = MatchType.Whitespace)) then 
                                        yield result
                                    yield! Tokenize(input.Remove(0, result.Length))
                          } 
                                                          
                    