namespace RSA.Mandelbrot.Console

module Console = 
    open RSA.Mandelbrot.Core
    open RSA.Mandelbrot.GPU
    open System
    open Rendering
    open System.Drawing
    
    let duration f = 
        let timer = new System.Diagnostics.Stopwatch()
        timer.Start()
        let returnValue = f()
        printfn "Elapsed Time: %A" timer.ElapsedMilliseconds
        returnValue
    
    type InputParams = 
        { DegreeParallelism : int
          Width : int
          Height : int
          MinX : double
          MaxX : double
          MinY : double
          MaxY : double
          OutputName : string
          Quiet : bool
          Mode : string }
    
    let parseCmdParams (argv : string []) = 
        let rec parse (currentState : InputParams) arguments = 
            match arguments with
            | [] -> currentState
            | "-s" :: xs | "-size" :: xs | "/s" :: xs | "/size" :: xs -> 
                match xs with
                | [] -> failwith "Size parameter expects a value afterwards, no value found"
                | dimensions :: xxs -> 
                    let vals = dimensions.ToLower().Split([| "x" |], StringSplitOptions.None)
                    
                    let (width, height) = 
                        if vals.Length <> 2 then 
                            failwith 
                                ("Invalid size parameter, expected format \"WidthxHeight\", value was " + dimensions)
                        else ((int vals.[0]), (int vals.[1]))
                    parse { currentState with Width = width
                                              Height = height } xxs
            | "-r" :: xs | "-rect" :: xs | "/r" :: xs | "/rect" :: xs -> 
                match xs with
                | [] -> failwith "Fractal coordinates parameter expects a value afterwards, no value found"
                | coordinates :: xxs -> 
                    let vals = coordinates.Split([| ":" |], StringSplitOptions.None)
                    
                    let (minX, maxX, minY, maxY) = 
                        if vals.Length <> 4 then 
                            failwith 
                                ("Invalid fractal coordinates parameter, expected format \"XMIN:XMAX:YMIN:YMAX\", value was " 
                                 + coordinates)
                        else ((double vals.[0]), (double vals.[1]), (double vals.[2]), (double vals.[3]))
                    parse { currentState with MinX = minX
                                              MaxX = maxX
                                              MinY = minY
                                              MaxY = maxY } xxs
            | "-t" :: xs | "-tasks" :: xs | "/t" :: xs | "/tasks" :: xs -> 
                match xs with
                | [] -> failwith "Tasks parameter expects a value afterwards, no value found"
                | tasks :: xxs -> parse { currentState with DegreeParallelism = int (tasks) } xxs
            | "-q" :: xs | "-quiet" :: xs | "/q" :: xs | "/quiet" :: xs -> parse { currentState with Quiet = true } xs
            | "-o" :: xs | "-output" :: xs | "/o" :: xs | "/output" :: xs -> 
                match xs with
                | [] -> failwith "No output name provided "
                | outputName :: xxs -> parse { currentState with OutputName = outputName } xxs
            | "-m" :: xs | "-mode" :: xs | "/m" :: xs | "/mode" :: xs -> 
                match xs with
                | [] -> failwith "Expecting CPU or GPU as mode"
                | mode :: xxs -> 
                    if mode = "cpu" || mode = "gpu" then parse { currentState with Mode = mode } xxs
                    else failwith ("Invalid mode " + mode + ", use CPU or GPU as values")
            | _ -> failwith ("Unrecognized argument sequence " + String.Join(" ", arguments))
        
        let defaultOptions = 
            { DegreeParallelism = 1
              Width = 640
              Height = 480
              MinX = -2.0
              MaxX = 2.0
              MinY = -1.0
              MaxY = 1.0
              Quiet = false
              OutputName = "zad15.png"
              Mode = "cpu" }
        
        argv
        |> List.ofArray
        |> List.map (fun x -> x.ToLower())
        |> parse defaultOptions
    
    [<EntryPoint>]
    let main argv = 
        let o = parseCmdParams argv
        if o.Quiet = false then
            printfn "Execution parameters for this run:"
            printfn "%A" o
        
        // Good-looking ranges
        //    let rangeX = (-1.2, -1.15)
        //    let rangeY = (1.50, 1.55)
        //    let rangeX = (-1.3, -1.0)
        //    let rangeY = (1.30, 1.60)
        //    let rangeX = (-1.25, -1.1)
        //    let rangeY = (1.45, 1.60) 
        // Default range
        //    let rangeX = (-2.0, 2.0)
        //    let rangeY = (-2.0, 2.0)
        let (executionOptions : ExecutionOptions) = 
            {   Width = o.Width
                Height = o.Height
                MinX = o.MinX
                MaxX = o.MaxX
                MinY = o.MinY
                MaxY = o.MaxY
                DegreeParallelism = o.DegreeParallelism }
                
        let logger = 
            Action<string>(
                match o.Quiet with
                | true -> ignore
                | false -> printfn "%s")
                
        use bmap = 
            match o.Mode with
            | "cpu" -> duration (fun () -> renderFinal executionOptions logger)
            | "gpu" -> 
                duration (fun () -> 
                    use render = new GpuRenderer()
                    render.Render(executionOptions, logger))
            | _ -> failwith ("Invalid mode " + o.Mode)
                
        bmap.Save(o.OutputName)
        
        0 // return an integer exit code
