using CoffeeManagementSystem.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class DashboardForm : Form
    {
        private DonhangBLL _donhangBLL;
        public DashboardForm()
        {
            InitializeComponent();
            _donhangBLL = new DonhangBLL();
            // khoá pan/zoom cho pie (tuỳ chọn)
            formsPlotPieNgay.Configuration.Pan = false;
            formsPlotPieNgay.Configuration.Zoom = false;
            formsPlotPieNgay.Configuration.ScrollWheelZoom = false;
            formsPlotPieNgay.Configuration.RightClickDragZoom = false;
            formsPlotPieNgay.Configuration.MiddleClickAutoAxis = false;
            dtpNgay.Value=DateTime.Today;
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            LoadChartsByDate(dtpNgay.Value.Date);
            lblPieTitle1.Visible = true;
            lblPieTitle1.Text = $"Doanh thu theo loại đồ uống ({dtpNgay.Value:dd/MM/yyyy})";
        }

        private void dtpNgay_ValueChanged(object sender, EventArgs e)
        {
            LoadChartsByDate(dtpNgay.Value.Date);
        }
        private void LoadChartsByDate(DateTime day)
        {
            LoadHourChart(day);
            LoadPieChartByDate(day);
        }

        private void LoadHourChart(DateTime date)
        {
            chartHour.Series.Clear();
            chartHour.ChartAreas[0].AxisX.Interval = 1;

            DataTable dt = _donhangBLL.GetRevenueByHour(date);

            var series = new System.Windows.Forms.DataVisualization.Charting.Series("DoanhThuTheoGio");
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            foreach (DataRow row in dt.Rows)
            {
                string gio = row["Gio"].ToString() + "h";
                decimal doanhThu = Convert.ToDecimal(row["DoanhThu"]);
                series.Points.AddXY(gio, doanhThu);
            }

            chartHour.Series.Add(series);

            chartHour.Titles.Clear();
            var title = chartHour.Titles.Add($"Doanh thu theo giờ ngày {date:dd/MM/yyyy}");
            title.Font = new Font("Arial", 14, FontStyle.Bold);
        }

        private void LoadPieChartByDate(DateTime day)
        {
            var dt = _donhangBLL.GetDoanhThuTheoLoaiTrongNgay(day);

            formsPlotPieNgay.Plot.Clear();

            if (dt.Rows.Count == 0)
            {
                formsPlotPieNgay.Plot.Title($"Không có dữ liệu ngày {day:dd/MM/yyyy}");
                formsPlotPieNgay.Refresh();
                return;
            }

            double[] values = dt.AsEnumerable()
                                .Select(r => Convert.ToDouble(r["DoanhThu"]))
                                .ToArray();

            string[] labels = dt.AsEnumerable()
                                .Select(r => r["Tenloai"].ToString())
                                .ToArray();

            var pie = formsPlotPieNgay.Plot.AddPie(values);
            pie.SliceLabels = labels;
            pie.ShowPercentages = true;
            pie.ShowValues = false;
            pie.DonutSize = 0.5;

            formsPlotPieNgay.Plot.Legend();

            formsPlotPieNgay.Refresh();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblPieTitle1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void formsPlotPieNgay_Load(object sender, EventArgs e)
        {

        }

        private void chartHour_Click(object sender, EventArgs e)
        {

        }
    }
}
