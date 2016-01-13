namespace RSA.Mandelbrot.Core

open Mandelbrot
open System.Diagnostics
open System
open System.Drawing
open System.Numerics
open System.Collections.Generic
open System.Threading.Tasks

module Rendering = 
    type ExecutionOptions = 
        { DegreeParallelism : int
          Width : int
          Height : int
          MinX : double
          MaxX : double
          MinY : double
          MaxY : double }
    
    let palette = 
        Array.init 255 (fun iters -> 
            let iters = iters + 1
            let c = 5.0 * log (double iters) / (log (255.0 - 1.0))
            
            let color = 
//                if c < 1.0 then Color.FromArgb(int (255.0 * c), 0, 0)
//                else if c < 2.0 then Color.FromArgb(255, int (255.0 * (c - 1.0)), 0)
//                else Color.FromArgb(255, 255, int (255.0 * (c - 2.0)))
                if c < 1.0 then Color.FromArgb(int (255.0 * c), 0, 0)
                else if c < 2.0 then Color.FromArgb(int (255.0 * (c - 1.0)), int (255.0 * (c - 1.0) / 2.0), 0)
                else if c < 3.0 then Color.FromArgb(int (255), int (255.0 * (c - 2.0)), 0)
                else if c < 4.0 then Color.FromArgb(int (255), int (255.0 * (c - 3.0)), int (255.0 * (c - 3.0) / 2.0))
                else Color.FromArgb(255, 255, int (255.0 * (c - 4.0)))
            color.ToArgb())
    
    let HSVtoRGB h s v = 
        let hi = ((h / 60.0) |> int) % 6
        let f = h / 60.0 - Math.Floor(h / 60.0)
        let vscaled = v * 255.0
        let v = vscaled |> int
        let p = vscaled * (1.0 - s) |> int
        let q = vscaled * (1.0 - f * s) |> int
        let t = vscaled * (1.0 - (1.0 - f) * s) |> int
        match hi with
        | 0 -> Color.FromArgb(v, t, p)
        | 1 -> Color.FromArgb(q, v, p)
        | 2 -> Color.FromArgb(p, v, t)
        | 3 -> Color.FromArgb(p, q, v)
        | 4 -> Color.FromArgb(t, p, v)
        | _ -> Color.FromArgb(v, p, q)

    let fastDrawBitmap (bmap : Bitmap) (colors : int [,]) = 
        Debug.Assert(colors.GetLength(0) = bmap.Height && colors.GetLength(1) = bmap.Width)
        let bmapData = 
            bmap.LockBits
                (Rectangle(0, 0, bmap.Width, bmap.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, 
                 bmap.PixelFormat)
        try 
            let imageData = Array.zeroCreate<byte> (bmapData.Stride * bmapData.Height)
            System.Runtime.InteropServices.Marshal.Copy(bmapData.Scan0, imageData, 0, imageData.Length)
            let bytesPerPixel = Image.GetPixelFormatSize(bmap.PixelFormat) / 8
            let stride = bmapData.Stride
            Parallel.For(0, bmapData.Height, 
                         (fun y -> 
                         Parallel.For(0, bmapData.Width, 
                                      (fun x -> 
                                      let ix = y * stride + x * bytesPerPixel
                                      
                                      // Format in Color is AARRGGBB
                                      // Formatting in the memory region is BBGGRRAA though!
                                      let color = 
                                          if colors.[y, x] = -1 then 0xFF000000
                                          else palette.[colors.[y, x]]
                                      imageData.[ix] <- byte (color &&& 0xFF)
                                      imageData.[ix + 1] <- byte ((color >>> 8) &&& 0xFF)
                                      imageData.[ix + 2] <- byte ((color >>> 16) &&& 0xFF)
                                      imageData.[ix + 3] <- 0xFFuy))
                         |> ignore))
            |> ignore
            System.Runtime.InteropServices.Marshal.Copy(imageData, 0, bmapData.Scan0, imageData.Length)
        finally
            bmap.UnlockBits(bmapData)

    let getIteration c = 
        let rec calcIteration iteration value = 
            match iteration, value with
            | it, _ when it >= 255 -> -1 // Assume it's in the Mandelbrot set
            | it, (z : Complex) when Double.IsNaN(z.Real) || Double.IsInfinity(z.Real) -> 
                it
            | it, z -> calcIteration (it + 1) (Mandelbrot.mandelbrotExpFun z c)
        calcIteration 0 (Complex(0.0, 0.0))
    
    let renderFinal executionOptions (log : Action<string>) = 
        let log = log.Invoke
        
        let iterationForPixel (row, col) = 
            let real = 
                executionOptions.MinX 
                + (executionOptions.MaxX - executionOptions.MinX) * (double col) / (double executionOptions.Width)
            let imaginary = 
                executionOptions.MaxY 
                - (executionOptions.MaxY - executionOptions.MinY) * (double row) / (double executionOptions.Height)
            let color = getIteration (Complex(real, imaginary))
            color
        
        let bmap = new Bitmap(executionOptions.Width, executionOptions.Height)
        let height = executionOptions.Height
        let width = executionOptions.Width
        let degreeParallelism = executionOptions.DegreeParallelism
        let iterations = Array2D.zeroCreate<int> height width
        let segmentHeight = ceil ((double height) / (double degreeParallelism)) |> int
        log "Initializing segments for calculation..."
        let tasks = 
            Array.init degreeParallelism (fun ix -> 
                let row = ix * segmentHeight
                
                let compute() = 
                    let currentSegmentHeight = 
                        if row + segmentHeight >= height then height - row
                        else segmentHeight
                    for y in 0..(currentSegmentHeight - 1) do
                        for x in 0..(width - 1) do
                            let iteration = iterationForPixel (row + y, x)
                            iterations.[row + y, x] <- iteration
                Action(compute))
        log "Starting Mandelbrot calculations"
        Parallel.Invoke(tasks)
        log "Mandelbrot calculations done"
        log "Generating final bitmap..."
        fastDrawBitmap bmap iterations
        log "Done"
        bmap

//module RenderingExploration = 
//    open Rendering
//    open ParallelMaps
//    
//    type ExecutionMode = 
//        | Plinq = 1
//        | Tpl = 2
//        | Tasks = 3
//        | Threads = 4
//    
//    type Algorithm = 
//        | Pixelwise = 1
//        | Segmentwise = 2
//    
//    type ExecutionOptionsExploration = 
//        { DegreeParallelism : int
//          Width : int
//          Height : int
//          MinX : double
//          MaxX : double
//          MinY : double
//          MaxY : double
//          Quiet : bool
//          OutputName : string
//          ExecutionMode : ExecutionMode
//          Algorithm : Algorithm }
//    
//    let render executionOptions log = 
//        let colorForPixel (row, col) = 
//            let real = 
//                executionOptions.MinX 
//                + (executionOptions.MaxX - executionOptions.MinX) * (double col) / (double executionOptions.Width)
//            let imaginary = 
//                executionOptions.MaxY 
//                - (executionOptions.MaxY - executionOptions.MinY) * (double row) / (double executionOptions.Height)
//            let color = getColor (Complex(real, imaginary))
//            color
//        
//        // Set a few aliases
//        let height = executionOptions.Height
//        let width = executionOptions.Width
//        let degreeParallelism = executionOptions.DegreeParallelism
//        
//        let renderPixelByPixel (mapFunc : ParallelizationOptions<_, _> -> IEnumerable<_> -> IEnumerable<_>) = 
//            let seq = 
//                seq { 
//                    for row in 0..1..(height - 1) do
//                        for col in 0..1..(width - 1) do
//                            yield (row, col)
//                }
//            
//            let arr = Array2D.zeroCreate<Color> height width
//            
//            //            let writePixel (row, col, color) = bmap.SetPixel(col, row, color)
//            let mappingFunc (row, col) = 
//                let color = colorForPixel (row, col)
//                arr.[row, col] <- color
//            
//            let options = 
//                { DegreeOfParallelism = executionOptions.DegreeParallelism
//                  Mapping = mappingFunc }
//            
//            seq
//            |> mapFunc options
//            |> Seq.iter ignore // The array is filled by the mapping function, the result is irrelevant
//            arr
//        
//        let renderInSegments (mapFunc : ParallelizationOptions<_, _> -> IEnumerable<_> -> IEnumerable<_>) = 
//            let segmentHeight = ceil ((double height) / (double degreeParallelism)) |> int
//            
//            let segmentStarts = 
//                Seq.init degreeParallelism (fun ix -> 
//                    let (row, col) = (0 + segmentHeight * ix, 0)
//                    (row, col))
//            
//            let arr = Array2D.zeroCreate<Color> height width
//            
//            let calculateColorBlocks (row, col) = 
//                let currentSegmentHeight = 
//                    if row + segmentHeight >= height then height - row
//                    else segmentHeight
//                for y in 0..(currentSegmentHeight - 1) do
//                    for x in 0..(width - 1) do
//                        let color = colorForPixel (row + y, x)
//                        arr.[row + y, x] <- color
//            
//            let options = 
//                { DegreeOfParallelism = degreeParallelism
//                  Mapping = calculateColorBlocks }
//            
//            segmentStarts
//            |> mapFunc options
//            |> Seq.iter ignore // The array is filled by the mapping function, the result is irrelevant
//            arr
//        
//        let mapF options seq = 
//            match executionOptions.ExecutionMode with
//            | ExecutionMode.Plinq -> mapWithParalellLinq options seq
//            | ExecutionMode.Tpl -> mapWithTPL options seq
//            | ExecutionMode.Tasks -> mapWithTasks options seq
//            | ExecutionMode.Threads -> mapWithPureThreads options seq
//            | _ -> failwith "Invalid execution mode"
//        
//        let mutable bmap = new Bitmap(width, height)
//        
//        let colors = 
//            match executionOptions.Algorithm with
//            | Algorithm.Pixelwise -> renderPixelByPixel mapF
//            | Algorithm.Segmentwise -> renderInSegments mapF
//            | _ -> failwith "Invalid algorithm"
//        //fastDrawBitmap bmap colors
//        bmap
