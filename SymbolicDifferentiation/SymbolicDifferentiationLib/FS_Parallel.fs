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

let private CalcChunkStartStopIndex size procNum index = 
    let chunkSize = size / procNum
    let reminder = size % procNum
    index * chunkSize, 
    if index < procNum - 1 then 
        (index + 1) * chunkSize - 1
    else
        size - 1
        
let private Chunks size procNum =
    let Calc = CalcChunkStartStopIndex size procNum
    let rec Chunks'(i, list) = 
        if i < procNum then
            Chunks'(i + 1, list @ [Calc(i)])
        else
            list
    Chunks'(0, [])
    
let private ProcessChunk(f, x: 'a array, y: 'b array) chunk = 
    let ProcessChunk' (startIndex, endIndex) =
        let chunks = Array.init (endIndex - startIndex + 1) (fun index -> index + startIndex)
        Array.map (fun i -> f x.[i] y.[i]) chunks
    ProcessChunk' chunk
             
let pmap2 n size f (x: 'a array) (y: 'b array) =     
    let chunks = Chunks size n
    let asyncs = Seq.map (fun chunk -> async { return (ProcessChunk(f, x, y) chunk) }) chunks
    let results' = Async.Run(Async.Parallel(asyncs))
    //let results' = Seq.map (ProcessChunk(f, x, y)) chunks
    Array.concat results'

    