namespace CoffeeManagementSystem
{
    partial class DashboardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashboardForm));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.guna2Panel5 = new Guna.UI2.WinForms.Guna2Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.guna2CirclePictureBox1 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.chartHour = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.dtpNgay = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblQuote = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.formsPlotPieNgay = new ScottPlot.FormsPlot();
            this.lblPieTitle1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.guna2Panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).BeginInit();
            this.guna2Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartHour)).BeginInit();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2Panel5
            // 
            this.guna2Panel5.AutoSize = true;
            this.guna2Panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(167)))), ((int)(((byte)(167)))));
            this.guna2Panel5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(167)))), ((int)(((byte)(167)))));
            this.guna2Panel5.BorderStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.guna2Panel5.Controls.Add(this.label8);
            this.guna2Panel5.Controls.Add(this.label7);
            this.guna2Panel5.Controls.Add(this.guna2CirclePictureBox1);
            this.guna2Panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.guna2Panel5.Location = new System.Drawing.Point(0, 479);
            this.guna2Panel5.Margin = new System.Windows.Forms.Padding(0);
            this.guna2Panel5.Name = "guna2Panel5";
            this.guna2Panel5.Size = new System.Drawing.Size(1495, 75);
            this.guna2Panel5.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("SVN-Gilroy", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Firebrick;
            this.label8.Location = new System.Drawing.Point(71, 41);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 27);
            this.label8.TabIndex = 11;
            this.label8.Text = "Founder";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("SVN-Gilroy Medium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Firebrick;
            this.label7.Location = new System.Drawing.Point(71, 15);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(176, 26);
            this.label7.TabIndex = 10;
            this.label7.Text = "Nguyễn Thái Sơn";
            // 
            // guna2CirclePictureBox1
            // 
            this.guna2CirclePictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.guna2CirclePictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("guna2CirclePictureBox1.Image")));
            this.guna2CirclePictureBox1.ImageRotate = 0F;
            this.guna2CirclePictureBox1.Location = new System.Drawing.Point(5, 6);
            this.guna2CirclePictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.guna2CirclePictureBox1.Name = "guna2CirclePictureBox1";
            this.guna2CirclePictureBox1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CirclePictureBox1.Size = new System.Drawing.Size(62, 62);
            this.guna2CirclePictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2CirclePictureBox1.TabIndex = 0;
            this.guna2CirclePictureBox1.TabStop = false;
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.guna2Panel2.BackColor = System.Drawing.Color.SeaShell;
            this.guna2Panel2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.guna2Panel2.BorderStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.guna2Panel2.BorderThickness = 1;
            this.guna2Panel2.Controls.Add(this.chartHour);
            this.guna2Panel2.Controls.Add(this.panel5);
            this.guna2Panel2.Controls.Add(this.guna2Panel5);
            this.guna2Panel2.Location = new System.Drawing.Point(0, 316);
            this.guna2Panel2.Margin = new System.Windows.Forms.Padding(0);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(1495, 554);
            this.guna2Panel2.TabIndex = 20;
            // 
            // chartHour
            // 
            this.chartHour.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chartHour.BackImageTransparentColor = System.Drawing.Color.White;
            this.chartHour.BorderlineColor = System.Drawing.Color.Transparent;
            chartArea2.Name = "ChartArea1";
            this.chartHour.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartHour.Legends.Add(legend2);
            this.chartHour.Location = new System.Drawing.Point(0, 0);
            this.chartHour.Margin = new System.Windows.Forms.Padding(0);
            this.chartHour.Name = "chartHour";
            this.chartHour.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartHour.Series.Add(series2);
            this.chartHour.Size = new System.Drawing.Size(730, 471);
            this.chartHour.TabIndex = 11;
            this.chartHour.Text = "chart1";
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 5;
            // 
            // dtpNgay
            // 
            this.dtpNgay.BorderRadius = 10;
            this.dtpNgay.Checked = true;
            this.dtpNgay.FillColor = System.Drawing.Color.Firebrick;
            this.dtpNgay.Font = new System.Drawing.Font("SVN-Gilroy Medium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpNgay.ForeColor = System.Drawing.Color.White;
            this.dtpNgay.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtpNgay.Location = new System.Drawing.Point(32, 216);
            this.dtpNgay.Margin = new System.Windows.Forms.Padding(2);
            this.dtpNgay.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpNgay.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpNgay.Name = "dtpNgay";
            this.dtpNgay.Size = new System.Drawing.Size(367, 59);
            this.dtpNgay.TabIndex = 3;
            this.dtpNgay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.dtpNgay.Value = new System.DateTime(2025, 11, 24, 0, 0, 0, 0);
            this.dtpNgay.ValueChanged += new System.EventHandler(this.dtpNgay_ValueChanged);
            // 
            // lblQuote
            // 
            this.lblQuote.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblQuote.Font = new System.Drawing.Font("SVN-Gilroy Medium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuote.ForeColor = System.Drawing.Color.SaddleBrown;
            this.lblQuote.Location = new System.Drawing.Point(27, 78);
            this.lblQuote.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQuote.Name = "lblQuote";
            this.lblQuote.Size = new System.Drawing.Size(905, 212);
            this.lblQuote.TabIndex = 1;
            this.lblQuote.Text = resources.GetString("lblQuote.Text");
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(167)))), ((int)(((byte)(167)))));
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Firebrick;
            this.label1.Location = new System.Drawing.Point(25, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(907, 49);
            this.label1.TabIndex = 0;
            this.label1.Text = "Thêm chút chill cho ngày dài";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Panel1.AutoSize = true;
            this.guna2Panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.guna2Panel1.Controls.Add(this.dtpNgay);
            this.guna2Panel1.Controls.Add(this.guna2PictureBox1);
            this.guna2Panel1.Controls.Add(this.lblQuote);
            this.guna2Panel1.Controls.Add(this.label1);
            this.guna2Panel1.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Panel1.Location = new System.Drawing.Point(0, 7);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(1492, 305);
            this.guna2Panel1.TabIndex = 19;
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2PictureBox1.BorderRadius = 15;
            this.guna2PictureBox1.Image = global::CoffeeManagementSystem.Properties.Resources.CoffeCover;
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(983, 14);
            this.guna2PictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(484, 276);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox1.TabIndex = 2;
            this.guna2PictureBox1.TabStop = false;
            // 
            // formsPlotPieNgay
            // 
            this.formsPlotPieNgay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlotPieNgay.AutoSize = true;
            this.formsPlotPieNgay.Location = new System.Drawing.Point(21, 34);
            this.formsPlotPieNgay.Margin = new System.Windows.Forms.Padding(0);
            this.formsPlotPieNgay.Name = "formsPlotPieNgay";
            this.formsPlotPieNgay.Size = new System.Drawing.Size(735, 467);
            this.formsPlotPieNgay.TabIndex = 7;
            // 
            // lblPieTitle1
            // 
            this.lblPieTitle1.AutoSize = true;
            this.lblPieTitle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPieTitle1.Location = new System.Drawing.Point(79, 19);
            this.lblPieTitle1.Margin = new System.Windows.Forms.Padding(0);
            this.lblPieTitle1.Name = "lblPieTitle1";
            this.lblPieTitle1.Size = new System.Drawing.Size(130, 29);
            this.lblPieTitle1.TabIndex = 8;
            this.lblPieTitle1.Text = "Doanh thu";
            this.lblPieTitle1.Visible = false;
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.AutoSize = true;
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.Controls.Add(this.lblPieTitle1);
            this.panel5.Controls.Add(this.formsPlotPieNgay);
            this.panel5.Location = new System.Drawing.Point(723, -10);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(769, 484);
            this.panel5.TabIndex = 10;
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FloralWhite;
            this.ClientSize = new System.Drawing.Size(1498, 881);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DashboardForm";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.DashboardForm_Load);
            this.guna2Panel5.ResumeLayout(false);
            this.guna2Panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).EndInit();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartHour)).EndInit();
            this.guna2Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel guna2Panel5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private Guna.UI2.WinForms.Guna2CirclePictureBox guna2CirclePictureBox1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpNgay;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private System.Windows.Forms.Label lblQuote;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartHour;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblPieTitle1;
        private ScottPlot.FormsPlot formsPlotPieNgay;
    }
}