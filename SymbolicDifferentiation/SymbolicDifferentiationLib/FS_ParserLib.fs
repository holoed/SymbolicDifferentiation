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

// Parser State

type ParserState(input:Token list, position:int) = 
    member s.Position = position
    member s.Input = input
   
// Error Info
 
type ErrorInfo(position:int, expectations:string list, message: string) = 
    member s.Position = position
    member s.Expectations = expectations
    member s.Message = message
    static member Make(position: int) = ErrorInfo(position, [], System.String.Empty)
    static member Merge(error1 : ErrorInfo, error2 : ErrorInfo) = 
                    ErrorInfo(
                        error2.Position, 
                        error1.Expectations @ error2.Expectations,
                        error2.Message)

// Parse Result
  
type ParseResult<'a> = 
    | Success of 'a * ParserState * ErrorInfo
    | Fail of ErrorInfo 
    member s.Error = match s with 
                        | Success(_,_,error) -> error
                        | Fail error -> error
    static member Merge (result1:ParseResult<'a>, result2) = 
                        match result2 with
                        | Success (output,state,error) -> Success(output, state, ErrorInfo.Merge ((result1.Error), error))
                        | Fail error -> Fail (ErrorInfo.Merge( (result1.Error), error ))
    
type Consumed<'a> = Consumed of bool * ParseResult<'a>

type ParserType<'a> = ParserState -> Consumed<'a>

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
                                                        Consumed(b2,ParseResult.Merge( result, result2 ))

type ParseMonad() = 
    member p.Return output = fun state -> Consumed(false,Success(output, state, ErrorInfo.Make (state.Position)))
    member p.Let(output,f) = f output
    member p.Bind(m,f) = fun output -> match m output with
                                       | Consumed(b,result) -> match result with
                                                               | Success(output,state,error) -> 
                                                                 let (Consumed(b2,result2)) = f output state
                                                                 Consumed(b || b2, if b2 then result2 else ParseResult.Merge(result, result2))

                                                               | Fail e -> Consumed(b,Fail e)
                                                       
let parse = ParseMonad()

let FollowedBy (m,f) =
    fun output -> match m output with
                                       | Consumed(b,result) -> match result with
                                                               | Success(output,state,error) -> 
                                                                 let (Consumed(b2,result2)) = f output state
                                                                 Consumed(b && b2, if b2 then result else ParseResult.Merge(result, result2))

                                                               | Fail e -> Consumed(b,Fail e)
                                                               
                                                               
let attempt p = fun input -> let Consumed(b,result) as c = p input
                             match b, result with
                                     | true, Fail(_) | false, _ -> Consumed(false,result) 
                                     | true, Success (_,_,_) -> c

/// To allow conditional parsing, we define a combinator sat that takes a predicate, 
/// and yields a parser that consumes a single character if it satisfies the predicate, 
/// and fails otherwise
let sat pred = fun (state:ParserState) -> 
    match state.Input with
    | [] -> Consumed(false,Fail (ErrorInfo(state.Position,  [], "unexpected end of input" )))
    | c :: cs -> if pred c 
                 then Consumed(true, Success(c, ParserState(cs,state.Position + 1), ErrorInfo.Make state.Position))
                 else Consumed(false, Fail (ErrorInfo( state.Position, [], sprintf "unexpected character '%A'" c.Value )))

let digitOrLetter : ParserType<Token> = sat Token.IsLetterOrDigit 

let Literal (s : Token) r = 
    let rec help l =
            match l with
            | [] -> parse { return r }
            | c :: cs -> parse { let! _ = sat (fun x -> x = c)
                                 return! help cs }
    [s] |> List.to_array |> List.of_array |> help   
    
/// Parse repeated applications of a parser p, separated by applications of a parser
/// op whose result value is an operator that is assumed to associate to the left,
/// and which is used to combine the results from the p parsers 
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

let Then p1 p2 = parse { let! x = p1
                         let! y = p2 x
                         return y }

let Then_ p1 p2 = Then p1 (fun dummy -> p2)


let rec sepBy1 p sep = 
    let sepByHelper p sep = (Then_ sep (sepBy1 p sep)) <|> parse { return [] }
    Then p (fun x -> (Then (sepByHelper p sep) (fun xs -> parse { return x :: xs })))

let sepBy p sep = sepBy1 p sep <|> parse { return [] }