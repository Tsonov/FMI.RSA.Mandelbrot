using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RSA.Mandelbrot.Core;
using RSA.Mandelbrot.GPU;
using System.Diagnostics;

namespace RSA.Mandelbrot.UI
{
    public partial class Mandelbrot : Form
    {
        private readonly Stopwatch timer = new Stopwatch();
        private const double DefaultMinX = -2.0;
        private const double DefaultMaxX = 2.0;
        private const double DefaultMinY = -1.0;
        private const double DefaultMaxY = 1.0;
        private double currentMinX = DefaultMinX;
        private double currentMaxX = DefaultMaxX;
        private double currentMinY = DefaultMinY;
        private double currentMaxY = DefaultMaxY;

        private readonly GpuRenderer renderer;

        public Mandelbrot()
        {
            InitializeComponent();
            this.btnReset.Click += (sender, e) =>
            {
                currentMinX = DefaultMinX;
                currentMaxX = DefaultMaxX;
                currentMinY = DefaultMinY;
                currentMaxY = DefaultMaxY;
                this.txtMinX.Text = currentMinX.ToString("0.0######");
                this.txtMaxX.Text = currentMaxX.ToString("0.0######");
                this.txtMinY.Text = currentMinY.ToString("0.0######");
                this.txtMaxY.Text = currentMaxY.ToString("0.0######");
                RegenerateImage();
            };
            this.btnGenerate.Click += (sender, e) =>
            {
                currentMinX = double.Parse(txtMinX.Text);
                currentMaxX = double.Parse(txtMaxX.Text);
                currentMinY = double.Parse(txtMinY.Text);
                currentMaxY = double.Parse(txtMaxY.Text);
                RegenerateImage();
            };
            this.Resize += (sender, args) =>
            {
                if (imageBox.Image != null)
                {
                    RegenerateImage();
                }
            };
            this.imageBox.MouseDoubleClick += imageBox_MouseDoubleClick;
            this.txtMinX.Leave += (sender, e) => currentMinX = double.Parse(txtMinX.Text);
            this.txtMaxX.Leave += (sender, e) => currentMaxX = double.Parse(txtMaxX.Text);
            this.txtMinY.Leave += (sender, e) => currentMinY = double.Parse(txtMinY.Text);
            this.txtMaxY.Leave += (sender, e) => currentMaxY = double.Parse(txtMaxY.Text);
            this.rdioCpu.CheckedChanged += (sender, args) =>
            {
                if (imageBox.Image != null)
                {
                    RegenerateImage();
                }
            };

            this.imageBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.rdioGPU.Select();
            this.numDegreeParallel.Value = Environment.ProcessorCount;
            renderer = new GpuRenderer();
            this.txtMinX.Text = currentMinX.ToString("0.0######");
            this.txtMaxX.Text = currentMaxX.ToString("0.0######");
            this.txtMinY.Text = currentMinY.ToString("0.0######");
            this.txtMaxY.Text = currentMaxY.ToString("0.0######");
        }

        void imageBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Grab a new view from the center of the selected region
            double centerX = e.X;
            double centerY = e.Y;

            double centerXCoordinateInPlane =
                currentMinX + (currentMaxX - currentMinX) * centerX / ((double)imageBox.Width);
            double centerYCoordinateInPlane =
                currentMaxY - (currentMaxY - currentMinY) * centerY / ((double)imageBox.Height);

            // Shrink the viewport for "zooming"
            double complexPlaneWidth = (currentMaxX - currentMinX) / 2.0;
            double complexPlaneHeight = (currentMaxY - currentMinY) / 2.0;

            // Update the coords and regenerate an image
            currentMinX = centerXCoordinateInPlane - (complexPlaneWidth / 2.0);
            currentMaxX = centerXCoordinateInPlane + (complexPlaneWidth / 2.0);
            currentMinY = centerYCoordinateInPlane - (complexPlaneHeight / 2.0);
            currentMaxY = centerYCoordinateInPlane + (complexPlaneHeight / 2.0);

            RegenerateImage();
        }

        private void RegenerateImage()
        {
            this.txtMinX.Text = currentMinX.ToString("0.0######");
            this.txtMaxX.Text = currentMaxX.ToString("0.0######");
            this.txtMinY.Text = currentMinY.ToString("0.0######");
            this.txtMaxY.Text = currentMaxY.ToString("0.0######");
            this.btnGenerate.Enabled = false;
            this.imageBox.Enabled = false;
            this.btnReset.Enabled = false;
            AppendLogLine("Rendering...");
            timer.Reset();
            timer.Start();
            var executionOptions =
                new Rendering
                    .ExecutionOptions(
                    (int)numDegreeParallel.Value,
                    (int)(imageBox.Width * 1.5),
                    (int)(imageBox.Height * 1.5),
                    currentMinX,
                    currentMaxX,
                    currentMinY,
                    currentMaxY);
            if (rdioGPU.Checked)
            {
                imageBox.Image = renderer.Render(executionOptions, AppendLogLine);
            }
            else if (rdioCpu.Checked)
            {
                imageBox.Image = Rendering.renderFinal(executionOptions, AppendLogLine);
            }
            timer.Stop();
            AppendLogLine("Rendering took " + timer.Elapsed);
            this.btnGenerate.Enabled = true;
            this.imageBox.Enabled = true;
            this.btnReset.Enabled = true;
        }

        private void AppendLogLine(string text)
        {
            this.txtLog.AppendText(text + Environment.NewLine);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing && (renderer != null))
            {
                renderer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
