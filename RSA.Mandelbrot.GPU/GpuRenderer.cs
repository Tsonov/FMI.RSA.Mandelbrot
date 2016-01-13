using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSA.Mandelbrot.Core;
using Cudafy.Translator;
using Cudafy.Host;
using Cudafy;
using System.Diagnostics;

namespace RSA.Mandelbrot.GPU
{
    public sealed class GpuRenderer : IDisposable
    {
        readonly CudafyModule Module;
        readonly dim3 BlockSize;
        readonly GPGPU gpu;

        public GpuRenderer()
        {
            var availableOpenCLDevices = CudafyHost.GetDeviceProperties(eGPUType.OpenCL);
            if (availableOpenCLDevices.Any() == false)
            {
                throw new Exception("No OpenCL devices found...");
            }
            var device = availableOpenCLDevices.First();
            Module = CudafyTranslator.Cudafy(eArchitecture.OpenCL12);
            var blockSide =
                Enumerable
                .Range(1, 15)
                .Reverse()
                .First(count => count * count <= device.MaxThreadsPerBlock);
            BlockSize = new dim3(blockSide, blockSide);

            // Initialize gpu and load the module (avoids reloading every time)
            gpu = CudafyHost.GetDevice(eGPUType.OpenCL);
            gpu.LoadModule(Module);
        }

        public Bitmap Render(Rendering.ExecutionOptions options, Action<string> log)
        {
            try
            {
                var result = new Bitmap(options.Width, options.Height);
                int width = options.Width;
                int height = options.Height;
                log("Initializing and copying data to GPU memory");
                int[,] iterations = new int[height, width];
                var dev_iterations = gpu.CopyToDevice(iterations);
                var gridSize = new dim3(height, width);
                var blockSize = BlockSize;
                var minX = (float)options.MinX;
                var maxX = (float)options.MaxX;
                var minY = (float)options.MinY;
                var maxY = (float)options.MaxY;
                var stepX = (maxX - minX) / ((float)width);
                var stepY = (maxY - minY) / ((float)height);

                log("Launching Mandelbrot calculations");
                gpu.Launch(gridSize, blockSize, "CalculateMandelbrot", minX, maxY, stepX, stepY, dev_iterations);
                log("Mandelbrot calculations done, fetching results from GPU memory");
                gpu.CopyFromDevice(dev_iterations, iterations);

                log("Generating the final image");
                Rendering.fastDrawBitmap(result, iterations);
                return result;
            }
            finally
            {
                gpu.FreeAll();
            }

        }

        [Cudafy]
        public static void CalculateMandelbrot(
            GThread thread,
            float minX,
            float maxY,
            float stepX,
            float stepY,
            int[,] result)
        {
            var y = thread.get_global_id(0);
            var x = thread.get_global_id(1);
            if (x >= result.GetLength(1) || y >= result.GetLength(0))
                return;
            float real = minX + x * stepX;
            float imaginary = maxY - y * stepY;
            result[y, x] = GetMandelbrotIterationsFor(real, imaginary);
        }

        [Cudafy]
        public static int GetMandelbrotIterationsFor(float real, float imaginary)
        {
            int iterations = 0;
            float realZ = 0.0f;
            float imagZ = 0.0f;
            bool inSet = true;
            while (iterations < 255 && inSet)
            {
                if (Single.IsNaN(realZ) || Single.IsInfinity(realZ))
                {
                    inSet = false;
                    break;
                }
                else
                {
                    // F(Z) = e^z - c
                    float exp = GMath.Exp(realZ);
                    float cos = GMath.Cos(imagZ);
                    float sin = GMath.Sin(imagZ);
                    float newReal = exp * cos;
                    float newImag = exp * sin;
                    realZ = newReal - real;
                    imagZ = newImag - imaginary;
                }
                iterations++;
            }
            if (inSet)
            {
                return -1;
            }
            else
            {
                return iterations;
            }
        }

        public void Dispose()
        {
            gpu.Dispose();
        }
    }
}
