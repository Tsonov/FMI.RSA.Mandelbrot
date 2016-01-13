namespace RSA.Mandelbrot.Core

module Mandelbrot = 
    open System.Numerics
    
    let mandelbrotExpFun z c = Complex.Exp(z) - c
    let mandelbrotBasicFun (z:Complex) c = z * z + c
