namespace RSA.Mandelbrot.Core

open System.Linq
open System.Threading
open System.Threading.Tasks
open System.Collections.Concurrent

module ParallelMaps = 
    type ParallelizationOptions<'a, 'b> = 
        { DegreeOfParallelism : int
          Mapping : 'a -> 'b }
    
    let mapWithParalellLinq { DegreeOfParallelism = degreeOfParallelism; Mapping = f } (seq : seq<_>) = 
        seq.AsParallel().WithDegreeOfParallelism(degreeOfParallelism).Select(f).AsEnumerable()
    
    let mapWithTPL { DegreeOfParallelism = degreeOfParallelism; Mapping = f } seq = 
        let seqArr = seq |> Array.ofSeq
        let result = Array.zeroCreate seqArr.Length
        let parallelOptions = ParallelOptions(MaxDegreeOfParallelism = degreeOfParallelism)
        Parallel.For(0, result.Length, parallelOptions, (fun i -> result.[i] <- f seqArr.[i])) |> ignore
        result.AsEnumerable()
    
    let mapWithTasks { DegreeOfParallelism = degreeOfParallelism; Mapping = f } seq = 
        let producerQueue = ConcurrentQueue(seq)
        let resultsQueue = ConcurrentQueue()
        
        let tasks = 
            Seq.init degreeOfParallelism (fun _ -> 
                Task.Run(fun () -> 
                    let mutable hasItems = true
                    while hasItems do
                        let (gotItem, item) = producerQueue.TryDequeue()
                        hasItems <- gotItem
                        if gotItem then resultsQueue.Enqueue(f item)))
            |> Array.ofSeq
        Task.WaitAll(tasks)
        resultsQueue.AsEnumerable()
    
    let mapWithPureThreads { DegreeOfParallelism = degreeOfParallelism; Mapping = f } seq = 
        let producerQueue = ConcurrentQueue(seq)
        let resultsQueue = ConcurrentQueue()
        use countdownEvent = new CountdownEvent(degreeOfParallelism)
        Seq.init degreeOfParallelism (fun _ -> 
            Thread(fun () -> 
                let mutable hasItems = true
                while hasItems do
                    let (gotItem, item) = producerQueue.TryDequeue()
                    hasItems <- gotItem
                    if gotItem then resultsQueue.Enqueue(f item)
                countdownEvent.Signal() |> ignore))
        |> Seq.iter (fun thread -> thread.Start())
        countdownEvent.Wait()
        resultsQueue.AsEnumerable()
