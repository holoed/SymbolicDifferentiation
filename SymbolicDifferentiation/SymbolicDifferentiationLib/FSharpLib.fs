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
open SimbolicDifferentiation.Core.Tokens

let private (|ParseRegex|_|) pattern input = 
    let regex = Regex(pattern)
    let matched = regex.Match(input)
    if matched.Success then
        Some matched
    else
        None
        
let private parse s =     
       match s with     
       | ParseRegex "^[0-9]+"               result ->  Token.Create(MatchType.Number, result)
       | ParseRegex "^[a-zA-Z]+"            result ->  Token.Create(MatchType.Variable, result)
       | ParseRegex "^[\\^\\+\\*\\(\\)]"    result ->  Token.Create(MatchType.Symbol, result) 
       | ParseRegex "\s+"                   result ->  Token.Whitespace
       | _ -> failwith "unknown token"
  

let rec Tokenize input =   seq {
                            if (not (input.Equals(""))) then
                                    let result = parse(input)  
                                    if (not (result.Type = MatchType.Whitespace)) then 
                                        yield result
                                    yield! Tokenize(input.Remove(0, result.Length))
                          } 
                                                          
                    