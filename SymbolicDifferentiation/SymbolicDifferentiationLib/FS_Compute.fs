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

open FS_AbstractSyntaxTree;
open FS_Utils;
open System.Collections.Generic;
open SymbolicDifferentiation.Core.Computation;

//Sequential
let processBinaryOpArgs f x y = (seq[(f x);(f y)])
let processFuncArgs f args = (Seq.map (fun arg -> f(arg)) args)

//Parallel
let Parallel asyncs = (Seq.of_array((Async.Run(Async.Parallel(asyncs)))))
let Execute x = x |> Array.of_seq |> Seq.of_array
let parallelProcessBinaryOpArgs f x y = (Parallel(seq[async { return Execute(f x) }; async { return Execute(f y) }]))
let parallelProcessFuncArgs f args = (Parallel(Seq.map (fun arg -> async { return Execute(f arg) }) args))

//Compute
let rec private Create (exp, seqMapXY, seqMapArgs, data:IDictionary<string, KeyValuePair<string,double> seq>, functions:Dictionary<string, 'f>) = 
    let Process exp = Create(exp, seqMapXY, seqMapArgs, data, functions)
    match exp with
    | Number n ->            seq[n]
    | Variable x ->          data.Item(x)
    | Add(x, y) ->           functions.Item("Add") (seqMapXY Process x y)
    | Sub(x, y) ->           functions.Item("Sub") (seqMapXY Process x y)
    | Mul(x, y) ->           functions.Item("Mul") (seqMapXY Process x y)
    | Div(x, y) ->           functions.Item("Div") (seqMapXY Process x y)
    | Pow(x, n) ->           functions.Item("Pow") (seq[(Process x);(Process n)])
    | FunApp(name, args) ->  functions.Item(name)  (seqMapArgs Process args)
    | FunDecl(name, body) -> ComputationResult.CreateFunctionResult(name, (Process body))
    
    
type Ret(exps, functions, seqMapXY, seqMapArgs) =
    member x.Execute(data) = ComputationResult.CreateDictionary(Seq.map (fun exp -> Create(exp, seqMapXY, seqMapArgs, data, functions)) exps)

let Build = fun (exps, functions) ->  Ret((Seq.map (fun exp -> (ToFs exp)) exps), functions, processBinaryOpArgs, processFuncArgs)

let BuildParallel = fun (exps, functions) ->  Ret((Seq.map (fun exp -> (ToFs exp)) exps), functions, parallelProcessBinaryOpArgs, parallelProcessFuncArgs)