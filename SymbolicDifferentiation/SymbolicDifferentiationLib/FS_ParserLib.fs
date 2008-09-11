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

//TODO: Refactor and add more tests

open SymbolicDifferentiation.Core.Tokens;

type ParserState = Token list * int
    
let getPosition parserState = snd parserState

type ErrorInfo = { Position: int; Expectations: string list; Message : string }

let makeError pos = { Position = pos; Expectations = []; Message = System.String.Empty }

let mergeError error1 error2 = { Position = error2.Position; 
                                 Expectations = error1.Expectations @ error2.Expectations;
                                 Message = error2.Message }
  
type ParseResult<'a> = 
    | Success of 'a * ParserState * ErrorInfo
    | Fail of ErrorInfo  
                         
let getError result =
    match result with
    | Success(_,_,error) -> error
    | Fail error -> error

let mergeErrorResults result1 result2 = 
    match result2 with
    | Success (output,state,error) -> Success(output, state, mergeError (getError result1) error)
    | Fail error -> Fail (mergeError (getError result1) error)
    
type Consumed<'a> = Consumed of bool * ParseResult<'a>

type ParserType<'a> = ParserState -> Consumed<'a>

type ParseMonad() = class
    member p.Return output = fun state -> Consumed(false,Success(output, state, makeError (getPosition state)))
    member p.Let(output,f) = f output
    member p.Bind(m,f) = fun output -> match m output with
                                       | Consumed(b,result) -> match result with
                                                               | Success(output,state,error) -> 
                                                                 let (Consumed(b2,result2)) = f output state
                                                                 Consumed(b || b2, if b2 then result2 else mergeErrorResults result result2)

                                                               | Fail e -> Consumed(b,Fail e)
end

let (<|>) p1 p2 = fun output -> let Consumed(b,result) as consumed = p1 output
                                if b then 
                                    consumed 
                                else 
                                    match result with
                                      | Success _ -> consumed
                                      | Fail error -> let Consumed(b2,result2) as consumed2 = p2 output
                                                      if b2 then 
                                                        consumed2 
                                                      else 
                                                        Consumed(b2,mergeErrorResults result result2)
                                                        
let parse = ParseMonad()

let sat pred = fun (i,pos) -> 
    match i with
    | [] -> Consumed(false,Fail { Position = pos; Expectations = []; Message = "unexpected end of input" })
    | c :: cs -> if pred c 
                 then Consumed(true, Success(c, (cs,pos+1), makeError pos))
                 else Consumed(false, Fail { Position = pos; Expectations = []; Message = sprintf "unexpected character '%A'" c.Value })

let digitOrLetter : ParserType<Token> = sat Token.IsLetterOrDigit 

let Literal (s : Token) r = 
    let rec help l =
            match l with
            | [] -> parse { return r }
            | c :: cs -> parse { let! _ = sat (fun x -> x = c)
                                 return! help cs }
    [s] |> List.to_array |> List.of_array |> help   
    
    
let chainl1 p op = 
    let rec help x = parse { let! f = op
                             let! y = p
                             return! help (f x y) } 
                     <|> parse { return x }
    parse { let! x = p
            return! help x } 
           
let rec chainr1 p op =
    parse { let! x = p
            return! parse { let! f = op
                            let! y = chainr1 p op
                            return f x y }
                    <|> parse { return x }
          }

