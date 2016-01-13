namespace RSA.Mandelbrot.UI
{
    partial class Mandelbrot
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainTblPanel = new System.Windows.Forms.TableLayoutPanel();
            this.imageBox = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdioCpu = new System.Windows.Forms.RadioButton();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.rdioGPU = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numDegreeParallel = new System.Windows.Forms.NumericUpDown();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.regionAndStart = new System.Windows.Forms.GroupBox();
            this.txtMinX = new System.Windows.Forms.TextBox();
            this.txtMaxX = new System.Windows.Forms.TextBox();
            this.txtMaxY = new System.Windows.Forms.TextBox();
            this.txtMinY = new System.Windows.Forms.TextBox();
            this.lblX = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.mainTblPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDegreeParallel)).BeginInit();
            this.regionAndStart.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTblPanel
            // 
            this.mainTblPanel.ColumnCount = 2;
            this.mainTblPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.mainTblPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.mainTblPanel.Controls.Add(this.imageBox, 0, 1);
            this.mainTblPanel.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.mainTblPanel.Controls.Add(this.txtLog, 1, 0);
            this.mainTblPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTblPanel.Location = new System.Drawing.Point(0, 0);
            this.mainTblPanel.Name = "mainTblPanel";
            this.mainTblPanel.RowCount = 2;
            this.mainTblPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.09658F));
            this.mainTblPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.90342F));
            this.mainTblPanel.Size = new System.Drawing.Size(883, 497);
            this.mainTblPanel.TabIndex = 0;
            // 
            // imageBox
            // 
            this.mainTblPanel.SetColumnSpan(this.imageBox, 2);
            this.imageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox.Location = new System.Drawing.Point(3, 82);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(877, 412);
            this.imageBox.TabIndex = 0;
            this.imageBox.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(612, 73);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.regionAndStart, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(609, 73);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdioCpu);
            this.groupBox1.Controls.Add(this.rdioGPU);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(176, 67);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose rendering mode";
            // 
            // rdioCpu
            // 
            this.rdioCpu.AutoSize = true;
            this.rdioCpu.Location = new System.Drawing.Point(6, 19);
            this.rdioCpu.Name = "rdioCpu";
            this.rdioCpu.Size = new System.Drawing.Size(47, 17);
            this.rdioCpu.TabIndex = 1;
            this.rdioCpu.TabStop = true;
            this.rdioCpu.Text = "CPU";
            this.rdioCpu.UseVisualStyleBackColor = true;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(166, 37);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(67, 23);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            // 
            // rdioGPU
            // 
            this.rdioGPU.AutoSize = true;
            this.rdioGPU.Location = new System.Drawing.Point(6, 42);
            this.rdioGPU.Name = "rdioGPU";
            this.rdioGPU.Size = new System.Drawing.Size(48, 17);
            this.rdioGPU.TabIndex = 2;
            this.rdioGPU.TabStop = true;
            this.rdioGPU.Text = "GPU";
            this.rdioGPU.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numDegreeParallel);
            this.groupBox2.Location = new System.Drawing.Point(185, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(176, 67);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parallelism degree (CPU-only)";
            // 
            // numDegreeParallel
            // 
            this.numDegreeParallel.Location = new System.Drawing.Point(7, 36);
            this.numDegreeParallel.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numDegreeParallel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDegreeParallel.Name = "numDegreeParallel";
            this.numDegreeParallel.Size = new System.Drawing.Size(111, 20);
            this.numDegreeParallel.TabIndex = 0;
            this.numDegreeParallel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(621, 3);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(259, 73);
            this.txtLog.TabIndex = 3;
            // 
            // regionAndStart
            // 
            this.regionAndStart.Controls.Add(this.btnReset);
            this.regionAndStart.Controls.Add(this.lblY);
            this.regionAndStart.Controls.Add(this.lblX);
            this.regionAndStart.Controls.Add(this.txtMinY);
            this.regionAndStart.Controls.Add(this.txtMaxY);
            this.regionAndStart.Controls.Add(this.txtMaxX);
            this.regionAndStart.Controls.Add(this.txtMinX);
            this.regionAndStart.Controls.Add(this.btnGenerate);
            this.regionAndStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.regionAndStart.Location = new System.Drawing.Point(367, 3);
            this.regionAndStart.Name = "regionAndStart";
            this.regionAndStart.Size = new System.Drawing.Size(239, 67);
            this.regionAndStart.TabIndex = 5;
            this.regionAndStart.TabStop = false;
            this.regionAndStart.Text = "Select initial range";
            // 
            // txtMinX
            // 
            this.txtMinX.Location = new System.Drawing.Point(65, 16);
            this.txtMinX.Name = "txtMinX";
            this.txtMinX.Size = new System.Drawing.Size(35, 20);
            this.txtMinX.TabIndex = 2;
            // 
            // txtMaxX
            // 
            this.txtMaxX.Location = new System.Drawing.Point(106, 16);
            this.txtMaxX.Name = "txtMaxX";
            this.txtMaxX.Size = new System.Drawing.Size(37, 20);
            this.txtMaxX.TabIndex = 3;
            // 
            // txtMaxY
            // 
            this.txtMaxY.Location = new System.Drawing.Point(106, 39);
            this.txtMaxY.Name = "txtMaxY";
            this.txtMaxY.Size = new System.Drawing.Size(37, 20);
            this.txtMaxY.TabIndex = 4;
            // 
            // txtMinY
            // 
            this.txtMinY.Location = new System.Drawing.Point(65, 39);
            this.txtMinY.Name = "txtMinY";
            this.txtMinY.Size = new System.Drawing.Size(35, 20);
            this.txtMinY.TabIndex = 5;
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(7, 19);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(52, 13);
            this.lblX.TabIndex = 6;
            this.lblX.Text = "X from to:";
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(6, 42);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(52, 13);
            this.lblY.TabIndex = 7;
            this.lblY.Text = "Y from to:";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(166, 13);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(67, 23);
            this.btnReset.TabIndex = 8;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            // 
            // Mandelbrot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 497);
            this.Controls.Add(this.mainTblPanel);
            this.Name = "Mandelbrot";
            this.Text = "Mandelbrot";
            this.mainTblPanel.ResumeLayout(false);
            this.mainTblPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numDegreeParallel)).EndInit();
            this.regionAndStart.ResumeLayout(false);
            this.regionAndStart.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTblPanel;
        private System.Windows.Forms.PictureBox imageBox;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton rdioCpu;
        private System.Windows.Forms.RadioButton rdioGPU;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numDegreeParallel;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.GroupBox regionAndStart;
        private System.Windows.Forms.TextBox txtMinX;
        private System.Windows.Forms.TextBox txtMaxX;
        private System.Windows.Forms.TextBox txtMinY;
        private System.Windows.Forms.TextBox txtMaxY;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.Button btnReset;



    }
}

