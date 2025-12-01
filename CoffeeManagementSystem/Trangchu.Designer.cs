using System.Windows.Forms.DataVisualization.Charting;

namespace CoffeeManagementSystem
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashboardForm));
            this.chartHour = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblPieTitle1 = new System.Windows.Forms.Label();
            this.formsPlotPieNgay = new ScottPlot.FormsPlot();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.dtpNgay = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelQuote = new System.Windows.Forms.Panel();
            this.lblQuote = new System.Windows.Forms.Label();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.pictureUEH = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureRectangle20 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartHour)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelQuote.SuspendLayout();
            this.panelFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureUEH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureRectangle20)).BeginInit();
            this.SuspendLayout();
            // 
            // chartHour
            // 
            this.chartHour.BackImageTransparentColor = System.Drawing.Color.White;
            this.chartHour.BorderlineColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chartHour.ChartAreas.Add(chartArea1);
            this.chartHour.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartHour.Legends.Add(legend1);
            this.chartHour.Location = new System.Drawing.Point(17, 483);
            this.chartHour.Margin = new System.Windows.Forms.Padding(17, 8, 8, 17);
            this.chartHour.Name = "chartHour";
            this.chartHour.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.EarthTones;
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartHour.Series.Add(series1);
            this.chartHour.Size = new System.Drawing.Size(948, 363);
            this.chartHour.TabIndex = 11;
            this.chartHour.Text = "chart1";
            this.chartHour.Click += new System.EventHandler(this.chartHour_Click);
            // 
            // lblPieTitle1
            // 
            this.lblPieTitle1.Location = new System.Drawing.Point(0, 0);
            this.lblPieTitle1.Name = "lblPieTitle1";
            this.lblPieTitle1.Size = new System.Drawing.Size(100, 23);
            this.lblPieTitle1.TabIndex = 0;
            // 
            // formsPlotPieNgay
            // 
            this.formsPlotPieNgay.BackColor = System.Drawing.Color.White;
            this.formsPlotPieNgay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlotPieNgay.Location = new System.Drawing.Point(981, 483);
            this.formsPlotPieNgay.Margin = new System.Windows.Forms.Padding(8, 8, 17, 17);
            this.formsPlotPieNgay.Name = "formsPlotPieNgay";
            this.formsPlotPieNgay.Size = new System.Drawing.Size(500, 363);
            this.formsPlotPieNgay.TabIndex = 7;
            this.formsPlotPieNgay.Load += new System.EventHandler(this.formsPlotPieNgay_Load);
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 5;
            // 
            // dtpNgay
            // 
            this.dtpNgay.BorderRadius = 10;
            this.dtpNgay.Checked = true;
            this.dtpNgay.Dock = System.Windows.Forms.DockStyle.Left;
            this.dtpNgay.FillColor = System.Drawing.Color.Tomato;
            this.dtpNgay.Font = new System.Drawing.Font("SVN-Gilroy", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpNgay.ForeColor = System.Drawing.Color.White;
            this.dtpNgay.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtpNgay.Location = new System.Drawing.Point(17, 4);
            this.dtpNgay.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtpNgay.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpNgay.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpNgay.Name = "dtpNgay";
            this.dtpNgay.Size = new System.Drawing.Size(474, 107);
            this.dtpNgay.TabIndex = 0;
            this.dtpNgay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.dtpNgay.Value = new System.DateTime(2025, 11, 24, 0, 0, 0, 0);
            this.dtpNgay.ValueChanged += new System.EventHandler(this.dtpNgay_ValueChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FloralWhite;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.Controls.Add(this.panelQuote, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chartHour, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.formsPlotPieNgay, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.pictureRectangle20, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1498, 881);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // panelQuote
            // 
            this.panelQuote.BackColor = System.Drawing.Color.White;
            this.panelQuote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelQuote.Controls.Add(this.lblQuote);
            this.panelQuote.Controls.Add(this.panelFooter);
            this.panelQuote.Controls.Add(this.label1);
            this.panelQuote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelQuote.Location = new System.Drawing.Point(17, 17);
            this.panelQuote.Margin = new System.Windows.Forms.Padding(17, 17, 8, 8);
            this.panelQuote.Name = "panelQuote";
            this.panelQuote.Size = new System.Drawing.Size(948, 450);
            this.panelQuote.TabIndex = 0;
            // 
            // lblQuote
            // 
            this.lblQuote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQuote.Font = new System.Drawing.Font("SVN-Gilroy", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuote.ForeColor = System.Drawing.Color.DarkCyan;
            this.lblQuote.Location = new System.Drawing.Point(0, 58);
            this.lblQuote.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQuote.Name = "lblQuote";
            this.lblQuote.Padding = new System.Windows.Forms.Padding(17, 8, 17, 8);
            this.lblQuote.Size = new System.Drawing.Size(946, 271);
            this.lblQuote.TabIndex = 0;
            this.lblQuote.Text = resources.GetString("lblQuote.Text");
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelFooter.Controls.Add(this.pictureUEH);
            this.panelFooter.Controls.Add(this.dtpNgay);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 329);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(17, 4, 17, 8);
            this.panelFooter.Size = new System.Drawing.Size(946, 119);
            this.panelFooter.TabIndex = 1;
            // 
            // pictureUEH
            // 
            this.pictureUEH.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureUEH.Image = global::CoffeeManagementSystem.Properties.Resources.bo_nhan_dien_thuong_hieu_ueh_5;
            this.pictureUEH.Location = new System.Drawing.Point(741, 4);
            this.pictureUEH.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureUEH.Name = "pictureUEH";
            this.pictureUEH.Size = new System.Drawing.Size(188, 107);
            this.pictureUEH.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureUEH.TabIndex = 0;
            this.pictureUEH.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.DarkCyan;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("SVN-Gilroy Heavy", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.label1.Size = new System.Drawing.Size(946, 58);
            this.label1.TabIndex = 3;
            this.label1.Text = "“Chúc bạn có một ngày làm việc vui vẻ”";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(2, 865);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(83, 12);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // pictureRectangle20
            // 
            this.pictureRectangle20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureRectangle20.Image = global::CoffeeManagementSystem.Properties.Resources.Rectangle_20;
            this.pictureRectangle20.Location = new System.Drawing.Point(981, 17);
            this.pictureRectangle20.Margin = new System.Windows.Forms.Padding(8, 17, 17, 8);
            this.pictureRectangle20.Name = "pictureRectangle20";
            this.pictureRectangle20.Size = new System.Drawing.Size(500, 450);
            this.pictureRectangle20.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureRectangle20.TabIndex = 1;
            this.pictureRectangle20.TabStop = false;
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FloralWhite;
            this.ClientSize = new System.Drawing.Size(1498, 881);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DashboardForm";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.DashboardForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartHour)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelQuote.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureUEH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureRectangle20)).EndInit();
            this.ResumeLayout(false);

        }





        #endregion

        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpNgay;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartHour;
        private System.Windows.Forms.Label lblPieTitle1;
        private ScottPlot.FormsPlot formsPlotPieNgay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        private System.Windows.Forms.Panel panelQuote;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.PictureBox pictureUEH;
        private System.Windows.Forms.PictureBox pictureRectangle20;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblQuote;
        private System.Windows.Forms.Label label1;
    }
}
