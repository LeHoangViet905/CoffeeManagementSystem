using CoffeeManagementSystem.BLL;
using CoffeeManagementSystem.DAL;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CoffeeManagementSystem
{
    public partial class ReportForm : Form
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["SqliteDbConnection"].ConnectionString;
        private DonhangBLL _donhangBLL;
        private ReportBLL _reportBLL;
        private PrintDocument printDocumentPotentialCustomers;
        private PrintDocument printDocumentRevenue;
        private PrintDocument printDocumentProductSales;

        // Các đối tượng PrintPreviewDialog để xem trước khi in
        private PrintPreviewDialog printPreviewDialogPotentialCustomers;
        private PrintPreviewDialog printPreviewDialogRevenue;
        private PrintPreviewDialog printPreviewDialogProductSales;

        // Biến để lưu trữ DataGridView hiện tại đang được in
        private DataGridView dgvToPrint;
        // Biến để theo dõi số trang hiện tại khi in
        private int currentRowIndex = 0;
        private string reportTitle = ""; // Tiêu đề báo cáo
        private string reportDateRange = ""; // Khoảng thời gian báo cáo
        public ReportForm()
        {
            InitializeComponent();

            //Thêm 2 dòng để làm chart tính doanh thu theo giờ
            chartDashboard.MouseClick += chartDashboard_MouseClick;
            _donhangBLL = new DonhangBLL();
            //Cố định pie chart
            formsPlotPie.Configuration.Pan = false;
            formsPlotPie.Configuration.Zoom = false;
            formsPlotPie.Configuration.ScrollWheelZoom = false;
            formsPlotPie.Configuration.RightClickDragZoom = false;
            formsPlotPie.Configuration.MiddleClickAutoAxis = false;

            _reportBLL = new ReportBLL();
            printDocumentPotentialCustomers = new PrintDocument();
            printDocumentPotentialCustomers.PrintPage += new PrintPageEventHandler(this.printDocument_PrintPage);
            printPreviewDialogPotentialCustomers = new PrintPreviewDialog();
            printPreviewDialogPotentialCustomers.Document = printDocumentPotentialCustomers;

            // Khởi tạo PrintDocument và PrintPreviewDialog cho báo cáo Doanh thu
            printDocumentRevenue = new PrintDocument();
            printDocumentRevenue.PrintPage += new PrintPageEventHandler(this.printDocument_PrintPage);
            printPreviewDialogRevenue = new PrintPreviewDialog();
            printPreviewDialogRevenue.Document = printDocumentRevenue;

            // Khởi tạo PrintDocument và PrintPreviewDialog cho báo cáo Bán hàng theo đồ uống
            printDocumentProductSales = new PrintDocument();
            printDocumentProductSales.PrintPage += new PrintPageEventHandler(this.printDocument_PrintPage);
            printPreviewDialogProductSales = new PrintPreviewDialog();
            printPreviewDialogProductSales.Document = printDocumentProductSales;


            // Thiết lập giá trị mặc định cho DateTimePicker của báo cáo doanh thu
            dtpRevenueStartDate.Value = DateTime.Now.AddMonths(-1);
            dtpRevenueEndDate.Value = DateTime.Now;

            // Gắn sự kiện SelectedIndexChanged cho TabControl chính
            this.tabControlReports.SelectedIndexChanged += new EventHandler(this.tabControlReports_SelectedIndexChanged);
            this.dtpRevenueStartDate.ValueChanged += new EventHandler(this.dtpRevenue_ValueChanged);
            this.dtpRevenueEndDate.ValueChanged += new EventHandler(this.dtpRevenue_ValueChanged);
            this.dtpProductSalesStartDate.ValueChanged += new EventHandler(this.dtpProductSales_ValueChanged);
            this.dtpProductSalesEndDate.ValueChanged += new EventHandler(this.dtpProductSales_ValueChanged);
            // Tải báo cáo cho tab được chọn mặc định khi form được mở lần đầu
            tabControlReports_SelectedIndexChanged(tabControlReports, EventArgs.Empty);
        }
        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new Font("Arial", 9); // Giảm kích thước font cho dữ liệu để có nhiều dòng hơn
            Font headerFont = new Font("Arial", 16, FontStyle.Bold); // Tăng kích thước tiêu đề chính
            Font subHeaderFont = new Font("Arial", 12, FontStyle.Bold); // Tăng kích thước tiêu đề phụ
            Pen borderPen = new Pen(Color.Black, 1); // Bút vẽ đường viền bảng

            float lineHeight = font.GetHeight() + 5; // Tăng khoảng cách dòng
            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;
            float currentX;

            // In tiêu đề báo cáo
            StringFormat sfCenter = new StringFormat();
            sfCenter.Alignment = StringAlignment.Center;
            sfCenter.LineAlignment = StringAlignment.Center;
            graphics.DrawString(reportTitle, headerFont, Brushes.Black, e.PageBounds.Width / 2, y, sfCenter);
            y += headerFont.GetHeight() + 15; // Khoảng cách sau tiêu đề chính

            // In khoảng thời gian (nếu có)
            if (!string.IsNullOrEmpty(reportDateRange))
            {
                graphics.DrawString(reportDateRange, subHeaderFont, Brushes.Black, e.PageBounds.Width / 2, y, sfCenter);
                y += subHeaderFont.GetHeight() + 25; // Khoảng cách sau khoảng thời gian
            }
            else
            {
                y += 25; // Khoảng cách nếu không có khoảng thời gian
            }

            // Tính toán chiều rộng cột
            List<float> columnWidths = new List<float>();
            float totalPrintableWidth = e.MarginBounds.Width;
            int visibleColumnCount = 0;

            // Tính tổng chiều rộng của các cột cố định và số cột Fill
            foreach (DataGridViewColumn col in dgvToPrint.Columns)
            {
                if (col.Visible)
                {
                    visibleColumnCount++;
                    if (col.AutoSizeMode == DataGridViewAutoSizeColumnMode.Fill)
                    {
                        columnWidths.Add(-1); // Đánh dấu là cột Fill, sẽ tính sau
                    }
                    else
                    {
                        // Sử dụng chiều rộng thực tế của cột trong DataGridView
                        float actualWidth = (float)col.Width;
                        columnWidths.Add(actualWidth);
                        totalPrintableWidth -= actualWidth;
                    }
                }
                else
                {
                    columnWidths.Add(0); // Cột ẩn không chiếm không gian
                }
            }

            // Phân bổ chiều rộng cho các cột Fill
            int fillColumnCount = columnWidths.Count(w => w == -1);
            if (fillColumnCount > 0 && totalPrintableWidth > 0)
            {
                float fillColumnWidth = totalPrintableWidth / fillColumnCount;
                for (int i = 0; i < columnWidths.Count; i++)
                {
                    if (columnWidths[i] == -1)
                    {
                        columnWidths[i] = fillColumnWidth;
                    }
                }
            }
            // Đảm bảo không có chiều rộng âm nếu totalPrintableWidth < 0 (trường hợp hiếm)
            for (int i = 0; i < columnWidths.Count; i++)
            {
                if (columnWidths[i] < 0) columnWidths[i] = 0;
            }


            // In tiêu đề cột và vẽ đường kẻ
            currentX = x;
            float headerRowHeight = subHeaderFont.GetHeight() + 10; // Chiều cao của hàng tiêu đề
            for (int i = 0; i < dgvToPrint.Columns.Count; i++)
            {
                DataGridViewColumn col = dgvToPrint.Columns[i];
                if (col.Visible)
                {
                    float colWidth = columnWidths[i];
                    RectangleF headerRect = new RectangleF(currentX, y, colWidth, headerRowHeight);

                    // Vẽ nền tiêu đề (tùy chọn)
                    graphics.FillRectangle(Brushes.LightGray, headerRect);
                    // Vẽ đường viền ô tiêu đề
                    graphics.DrawRectangle(borderPen, currentX, y, colWidth, headerRowHeight);

                    StringFormat headerSf = new StringFormat();
                    headerSf.Alignment = StringAlignment.Center;
                    headerSf.LineAlignment = StringAlignment.Center;
                    headerSf.Trimming = StringTrimming.EllipsisCharacter; // Thêm dấu ... nếu văn bản quá dài
                    headerSf.FormatFlags = StringFormatFlags.NoWrap; // Không cho phép xuống dòng

                    graphics.DrawString(col.HeaderText, subHeaderFont, Brushes.Black, headerRect, headerSf);
                    currentX += colWidth;
                }
            }
            y += headerRowHeight;

            // In dữ liệu và vẽ đường kẻ
            while (currentRowIndex < dgvToPrint.Rows.Count)
            {
                DataGridViewRow row = dgvToPrint.Rows[currentRowIndex];
                if (row.Visible)
                {
                    // Kiểm tra xem có đủ chỗ cho dòng tiếp theo không
                    if (y + lineHeight > e.MarginBounds.Bottom)
                    {
                        e.HasMorePages = true;
                        currentRowIndex++; // Chuẩn bị cho trang tiếp theo
                        return;
                    }

                    currentX = x;
                    for (int i = 0; i < dgvToPrint.Columns.Count; i++)
                    {
                        DataGridViewColumn col = dgvToPrint.Columns[i];
                        if (col.Visible)
                        {
                            float colWidth = columnWidths[i];
                            object cellValue = row.Cells[col.Name].Value;
                            string text = (cellValue == null) ? "" : cellValue.ToString();

                            // Áp dụng định dạng cho các cột số/ngày nếu cần
                            if (col.DefaultCellStyle.Format != null && cellValue is IFormattable)
                            {
                                text = ((IFormattable)cellValue).ToString(col.DefaultCellStyle.Format, System.Globalization.CultureInfo.CurrentCulture);
                            }
                            else if (col.Name == "STT" && dgvToPrint.Name == "dgvPotentialCustomers") // Xử lý STT cho dgvPotentialCustomers
                            {
                                text = (currentRowIndex + 1).ToString();
                            }
                            else if (col.Name == "No" && dgvToPrint.Name == "dgvRevenue") // Xử lý No cho dgvRevenue
                            {
                                text = (currentRowIndex + 1).ToString();
                            }
                            else if (col.Name == "TyLeDongGopDoanhThu" && dgvToPrint.Name == "dgvProductSales") // Xử lý tỷ lệ đóng góp
                            {
                                // Lấy giá trị gốc từ DataSource
                                var dataSourceItem = dgvToPrint.Rows[currentRowIndex].DataBoundItem;
                                if (dataSourceItem is ProductSalesReportItem productItem)
                                {
                                    // Tính lại tỷ lệ đóng góp ở đây để đảm bảo khớp với logic hiển thị trên DGV
                                    decimal overallTotalRevenue = ((List<ProductSalesReportItem>)dgvToPrint.DataSource).Sum(item => item.TongDoanhThuMon);
                                    if (overallTotalRevenue > 0)
                                    {
                                        text = ((double)productItem.TongDoanhThuMon / (double)overallTotalRevenue).ToString("P2", System.Globalization.CultureInfo.CurrentCulture);
                                    }
                                    else
                                    {
                                        text = "0.00%";
                                    }
                                }
                            }

                            // Căn lề cho cột
                            StringFormat cellSf = new StringFormat();
                            cellSf.Trimming = StringTrimming.EllipsisCharacter;
                            cellSf.FormatFlags = StringFormatFlags.NoWrap;

                            if (col.DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight || col.DefaultCellStyle.Alignment == DataGridViewContentAlignment.TopRight || col.DefaultCellStyle.Alignment == DataGridViewContentAlignment.BottomRight)
                            {
                                cellSf.Alignment = StringAlignment.Far;
                            }
                            else if (col.DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleCenter || col.DefaultCellStyle.Alignment == DataGridViewContentAlignment.TopCenter || col.DefaultCellStyle.Alignment == DataGridViewContentAlignment.BottomCenter)
                            {
                                cellSf.Alignment = StringAlignment.Center;
                            }
                            else
                            {
                                cellSf.Alignment = StringAlignment.Near;
                            }

                            RectangleF cellRect = new RectangleF(currentX, y, colWidth, lineHeight);
                            graphics.DrawString(text, font, Brushes.Black, cellRect, cellSf);

                            // Vẽ đường viền ô dữ liệu
                            graphics.DrawRectangle(borderPen, currentX, y, colWidth, lineHeight);

                            currentX += colWidth;
                        }
                    }
                    y += lineHeight;
                }
                currentRowIndex++;
            }

            e.HasMorePages = false; // Đã in hết dữ liệu
            currentRowIndex = 0; // Reset lại cho lần in tiếp theo
        }
        private void dtpRevenue_ValueChanged(object sender, EventArgs e)
        {
            // Chỉ tải lại báo cáo doanh thu nếu người dùng đang ở tab doanh thu
            if (tabControlReports.SelectedTab == tabPage2) // Tab Doanh thu
            {
                LoadRevenueReport();
            }
        }
        private void dtpProductSales_ValueChanged(object sender, EventArgs e)
        {
            // Chỉ tải lại báo cáo bán hàng theo đồ uống nếu người dùng đang ở tab đó
            if (tabControlReports.SelectedTab == tabPage3)
            {
                LoadProductSalesReport();
            }
        }
        //Xử lý sự kiện khi người dùng thay đổi TabPage trên TabControl.
        //Tải dữ liệu báo cáo tương ứng với TabPage được chọn.
        private void tabControlReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlReports.SelectedTab == tabPage1) // Tab Khách hàng tiềm năng
            {
                LoadPotentialCustomersReport();
            }
            else if (tabControlReports.SelectedTab == tabPage2) // Tab Doanh thu
            {
                LoadRevenueReport();
            }
            // Thêm điều kiện cho TabPage báo cáo bán hàng theo đồ uống
            // Đảm bảo tên TabPage của bạn trong Designer là 'tabPageProductSales'
            else if (tabControlReports.SelectedTab == tabPage3)
            {
                LoadProductSalesReport();
            }
            // Thêm các điều kiện 'else if' cho các TabPage báo cáo khác nếu có
        }

        //Tải dữ liệu báo cáo 10 khách hàng có điểm tích lũy cao nhất và hiển thị lên DataGridView.
        private void LoadPotentialCustomersReport()
        {
            try
            {
                List<Khachhang> potentialCustomers = _reportBLL.GetPotentialCustomersReport();

                // 1. Đảm bảo dgvPotentialCustomers không có nguồn dữ liệu cũ
                dgvPotentialCustomers.DataSource = null;

                // 2. Xóa tất cả các cột cũ (đảm bảo không bị trùng lặp cột)
                dgvPotentialCustomers.Columns.Clear();

                // Cột "STT" - KHÔNG GÁN DataPropertyName, sẽ điền thủ công hoặc qua CellFormatting
                DataGridViewTextBoxColumn sttColumn = new DataGridViewTextBoxColumn();
                sttColumn.Name = "STT";
                sttColumn.HeaderText = "STT";
                sttColumn.Width = 50;
                sttColumn.ReadOnly = true;
                sttColumn.Resizable = DataGridViewTriState.False;
                sttColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvPotentialCustomers.Columns.Add(sttColumn);

                // Các cột dữ liệu khác
                DataGridViewTextBoxColumn maKHColumn = new DataGridViewTextBoxColumn();
                maKHColumn.DataPropertyName = "Makhachhang"; // DGV sẽ tự lấy giá trị từ đây
                maKHColumn.Name = "Makhachhang";
                maKHColumn.HeaderText = "Mã Khách hàng";
                maKHColumn.Width = 120;
                maKHColumn.ReadOnly = true;
                maKHColumn.Resizable = DataGridViewTriState.False;
                maKHColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvPotentialCustomers.Columns.Add(maKHColumn);

                DataGridViewTextBoxColumn tenKHColumn = new DataGridViewTextBoxColumn();
                tenKHColumn.DataPropertyName = "Hoten"; // DGV sẽ tự lấy giá trị từ đây
                tenKHColumn.Name = "TenKhachhang";
                tenKHColumn.HeaderText = "Tên Khách hàng";
                tenKHColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                tenKHColumn.ReadOnly = true;
                tenKHColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvPotentialCustomers.Columns.Add(tenKHColumn);

                DataGridViewTextBoxColumn sdtColumn = new DataGridViewTextBoxColumn();
                sdtColumn.DataPropertyName = "Sodienthoai";
                sdtColumn.Name = "Sodienthoa";
                sdtColumn.HeaderText = "Số điện thoại";
                sdtColumn.Width = 120;
                sdtColumn.ReadOnly = true;
                sdtColumn.Resizable = DataGridViewTriState.False;
                sdtColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvPotentialCustomers.Columns.Add(sdtColumn);

                DataGridViewTextBoxColumn emailColumn = new DataGridViewTextBoxColumn();
                emailColumn.DataPropertyName = "Email";
                emailColumn.Name = "Email";
                emailColumn.HeaderText = "Email";
                emailColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                emailColumn.ReadOnly = true;
                emailColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvPotentialCustomers.Columns.Add(emailColumn);

                DataGridViewTextBoxColumn ngayDangKyColumn = new DataGridViewTextBoxColumn();
                ngayDangKyColumn.DataPropertyName = "Ngaydangky";
                ngayDangKyColumn.Name = "Ngaydangky";
                ngayDangKyColumn.HeaderText = "Ngày đăng ký";
                ngayDangKyColumn.Width = 150;
                ngayDangKyColumn.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                ngayDangKyColumn.ReadOnly = true;
                ngayDangKyColumn.Resizable = DataGridViewTriState.False;
                ngayDangKyColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvPotentialCustomers.Columns.Add(ngayDangKyColumn);

                DataGridViewTextBoxColumn diemTichLuyColumn = new DataGridViewTextBoxColumn();
                diemTichLuyColumn.DataPropertyName = "DiemTichLuy";
                diemTichLuyColumn.Name = "DiemTichLuy";
                diemTichLuyColumn.HeaderText = "Điểm Tích Lũy";
                diemTichLuyColumn.Width = 100;
                diemTichLuyColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                diemTichLuyColumn.ReadOnly = true;
                diemTichLuyColumn.Resizable = DataGridViewTriState.False;
                diemTichLuyColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvPotentialCustomers.Columns.Add(diemTichLuyColumn);
                dgvPotentialCustomers.DataSource = potentialCustomers;


                if (potentialCustomers.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy khách hàng nào có điểm tích lũy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo TOP khách hàng tiềm năng: {ex.Message}\nVui lòng kiểm tra kết nối CSDL và dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Thêm sự kiện CellFormatting vào Form của bạn
        private void dgvPotentialCustomers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvPotentialCustomers.Columns[e.ColumnIndex].Name == "STT" && e.RowIndex >= 0)
            {
                e.Value = e.RowIndex + 1;
                e.FormattingApplied = true; // Đánh dấu là đã xử lý định dạng
            }
        }
        private void LoadRevenueReport()
        {
            try
            {
                DateTime startDate = dtpRevenueStartDate.Value.Date;
                DateTime endDate = dtpRevenueEndDate.Value.Date;

                // Kiểm tra lỗi ngày trước khi gọi BLL
                if (startDate > endDate)
                {
                    MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Lỗi Ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lblTotalPrice.Text = "0 VNĐ"; // Reset tổng tiền
                    return; // Thoát khỏi phương thức
                }

                // Gọi BLL để lấy dữ liệu, BLL sẽ ném exception nếu ngày không hợp lệ
                List<RevenueReportItem> revenueData = _reportBLL.GetRevenueReport(startDate, endDate);

                // Quan trọng: Ngắt kết nối nguồn dữ liệu cũ và xóa cột cũ
                dgvRevenue.DataSource = null;
                dgvRevenue.Columns.Clear(); // Xóa tất cả các cột cũ

                // Đảm bảo AutoGenerateColumns là false
                dgvRevenue.AutoGenerateColumns = false;
                DataGridViewTextBoxColumn noColumn = new DataGridViewTextBoxColumn();
                noColumn.Name = "No";
                noColumn.HeaderText = "No";
                noColumn.Width = 100;
                noColumn.ReadOnly = true;
                noColumn.Resizable = DataGridViewTriState.False;
                noColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvRevenue.Columns.Add(noColumn);

                // Cột "Date" (Ngày)
                DataGridViewTextBoxColumn dateColumn = new DataGridViewTextBoxColumn();
                dateColumn.DataPropertyName = "Ngay"; // DGV sẽ tự lấy giá trị từ đây
                dateColumn.Name = "Date";
                dateColumn.HeaderText = "Date";
                dateColumn.Width = 200;
                dateColumn.DefaultCellStyle.Format = "dd/MM/yyyy";
                dateColumn.ReadOnly = true;
                dateColumn.Resizable = DataGridViewTriState.False;
                dateColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvRevenue.Columns.Add(dateColumn);

                // Cột "Price" (Giá)
                DataGridViewTextBoxColumn priceColumn = new DataGridViewTextBoxColumn();
                priceColumn.DataPropertyName = "Tongtien"; // DGV sẽ tự lấy giá trị từ đây
                priceColumn.Name = "Price";
                priceColumn.HeaderText = "Price";
                priceColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                priceColumn.DefaultCellStyle.Format = "N0";
                priceColumn.ReadOnly = true;
                priceColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                priceColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvRevenue.Columns.Add(priceColumn);
                dgvRevenue.DataSource = revenueData;

                // Tính tổng doanh thu và hiển thị lên lblTotalPrice
                decimal totalRevenue = revenueData.Sum(item => item.Tongtien);
                lblTotalPrice.Text = totalRevenue.ToString("N0") + " VNĐ";

                if (revenueData.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu doanh thu trong khoảng thời gian đã chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo doanh thu: {ex.Message}\nVui lòng kiểm tra kết nối CSDL và dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Thêm sự kiện CellFormatting vào Form của bạn
        private void dgvRevenue_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvRevenue.Columns[e.ColumnIndex].Name == "No" && e.RowIndex >= 0)
            {
                e.Value = e.RowIndex + 1;
                e.FormattingApplied = true; // Đánh dấu là đã xử lý định dạng
            }
        }

        //Tải dữ liệu báo cáo bán hàng theo đồ uống và hiển thị lên DataGridView.
        private void LoadProductSalesReport()
        {
            try
            {

                DateTime startDate = DateTime.Now.AddMonths(-1).Date; // Ví dụ: 1 tháng trước
                DateTime endDate = DateTime.Now.Date; // Ví dụ: Hôm nay
                dgvProductSales.DataSource = null; // Ngắt kết nối nguồn dữ liệu
                dgvProductSales.Rows.Clear();     // Xóa tất cả các hàng
                dgvProductSales.Columns.Clear();  // Xóa tất cả các cột

                // Kiểm tra lỗi ngày trước khi gọi BLL
                if (startDate > endDate)
                {
                    MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Lỗi Ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lblTong.Text = "0 VNĐ";
                    return; // Thoát khỏi phương thức
                }

                // Gọi BLL để lấy dữ liệu báo cáo bán hàng theo đồ uống
                List<ProductSalesReportItem> productSalesData = _reportBLL.GetProductSalesReport(startDate, endDate);

                // Tính tổng doanh thu toàn bộ để tính tỷ lệ đóng góp
                decimal overallTotalRevenue = productSalesData.Sum(item => item.TongDoanhThuMon);
                dgvProductSales.AutoGenerateColumns = false;

                // Xóa tất cả các hàng và cột hiện có để đảm bảo cấu trúc mới
                dgvProductSales.Rows.Clear();
                dgvProductSales.Columns.Clear();

                // Thêm và cấu hình các cột
                // Cột "STT"
                DataGridViewTextBoxColumn sttColumn = new DataGridViewTextBoxColumn();
                sttColumn.Name = "STT";
                sttColumn.HeaderText = "STT";
                sttColumn.Width = 50;
                sttColumn.ReadOnly = true;
                sttColumn.Resizable = DataGridViewTriState.False;
                sttColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvProductSales.Columns.Add(sttColumn);

                // Cột "Madouong"
                DataGridViewTextBoxColumn maDouongColumn = new DataGridViewTextBoxColumn();
                maDouongColumn.DataPropertyName = "Madouong";
                maDouongColumn.Name = "Madouong";
                maDouongColumn.HeaderText = "Mã Đồ uống";
                maDouongColumn.Width = 100;
                maDouongColumn.ReadOnly = true;
                maDouongColumn.Resizable = DataGridViewTriState.False;
                maDouongColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvProductSales.Columns.Add(maDouongColumn);

                // Cột "Tendouong" - Tự động lấp đầy
                DataGridViewTextBoxColumn tenDouongColumn = new DataGridViewTextBoxColumn();
                tenDouongColumn.DataPropertyName = "Tendouong";
                tenDouongColumn.Name = "Tendouong";
                tenDouongColumn.HeaderText = "Tên Đồ uống";
                tenDouongColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                tenDouongColumn.ReadOnly = true;
                tenDouongColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvProductSales.Columns.Add(tenDouongColumn);

                // Cột "Maloai"
                DataGridViewTextBoxColumn maLoaiColumn = new DataGridViewTextBoxColumn();
                maLoaiColumn.DataPropertyName = "Maloai";
                maLoaiColumn.Name = "Maloai";
                maLoaiColumn.HeaderText = "Loại Đồ uống";
                maLoaiColumn.Width = 120;
                maLoaiColumn.ReadOnly = true;
                maLoaiColumn.Resizable = DataGridViewTriState.False;
                maLoaiColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvProductSales.Columns.Add(maLoaiColumn);

                // Cột "SoLuongBan"
                DataGridViewTextBoxColumn soLuongBanColumn = new DataGridViewTextBoxColumn();
                soLuongBanColumn.DataPropertyName = "SoLuongBan";
                soLuongBanColumn.Name = "SoLuongBan";
                soLuongBanColumn.HeaderText = "Số lượng bán";
                soLuongBanColumn.Width = 100;
                soLuongBanColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                soLuongBanColumn.ReadOnly = true;
                soLuongBanColumn.Resizable = DataGridViewTriState.False;
                soLuongBanColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvProductSales.Columns.Add(soLuongBanColumn);

                // Cột "TongDoanhThuMon"
                DataGridViewTextBoxColumn tongDoanhThuMonColumn = new DataGridViewTextBoxColumn();
                tongDoanhThuMonColumn.DataPropertyName = "TongDoanhThuMon";
                tongDoanhThuMonColumn.Name = "TongDoanhThuMon";
                tongDoanhThuMonColumn.HeaderText = "Tổng Doanh thu";
                tongDoanhThuMonColumn.Width = 150;
                tongDoanhThuMonColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                tongDoanhThuMonColumn.DefaultCellStyle.Format = "N0";
                tongDoanhThuMonColumn.ReadOnly = true;
                tongDoanhThuMonColumn.Resizable = DataGridViewTriState.False;
                tongDoanhThuMonColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvProductSales.Columns.Add(tongDoanhThuMonColumn);

                // Cột "TyLeDongGopDoanhThu" (Tính toán ở đây, không phải DataPropertyName trực tiếp từ ProductSalesReportItem)
                DataGridViewTextBoxColumn tyLeDongGopColumn = new DataGridViewTextBoxColumn();
                tyLeDongGopColumn.Name = "TyLeDongGopDoanhThu";
                tyLeDongGopColumn.HeaderText = "Tỷ lệ đóng góp";
                tyLeDongGopColumn.Width = 120;
                tyLeDongGopColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                tyLeDongGopColumn.DefaultCellStyle.Format = "P2"; // Định dạng phần trăm với 2 chữ số thập phân
                tyLeDongGopColumn.ReadOnly = true;
                tyLeDongGopColumn.Resizable = DataGridViewTriState.False;
                tyLeDongGopColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvProductSales.Columns.Add(tyLeDongGopColumn);


                // Gán dữ liệu vào DataGridView
                dgvProductSales.DataSource = productSalesData;

                // Điền STT và tính toán Tỷ lệ đóng góp doanh thu
                for (int i = 0; i < dgvProductSales.Rows.Count; i++)
                {
                    ProductSalesReportItem item = productSalesData[i];
                    dgvProductSales.Rows[i].Cells["STT"].Value = i + 1;

                    // Tính toán và gán tỷ lệ đóng góp
                    if (overallTotalRevenue > 0)
                    {
                        dgvProductSales.Rows[i].Cells["TyLeDongGopDoanhThu"].Value = (double)item.TongDoanhThuMon / (double)overallTotalRevenue;
                    }
                    else
                    {
                        dgvProductSales.Rows[i].Cells["TyLeDongGopDoanhThu"].Value = 0d;
                    }
                }
                lblTong.Text = overallTotalRevenue.ToString("N0") + " VNĐ";
                if (productSalesData.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu bán hàng theo đồ uống trong khoảng thời gian đã chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (ArgumentException ex) // Bắt lỗi từ BLL nếu ngày không hợp lệ
            {
                MessageBox.Show(ex.Message, "Lỗi Ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo bán hàng theo đồ uống: {ex.Message}\nVui lòng kiểm tra kết nối CSDL và dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            // Báo cáo Khách hàng tiềm năng
            dgvToPrint = dgvPotentialCustomers; // Gán DataGridView cần in
            reportTitle = "BÁO CÁO TOP 10 KHÁCH HÀNG TIỀM NĂNG";
            reportDateRange = ""; // Không có khoảng thời gian cho báo cáo này
            currentRowIndex = 0; // Reset số trang
            printPreviewDialogPotentialCustomers.ShowDialog();
        }

        private void btnPrintDoanhThu_Click(object sender, EventArgs e)
        {
            // Báo cáo Doanh thu
            dgvToPrint = dgvRevenue; // Gán DataGridView cần in
            reportTitle = "BÁO CÁO DOANH THU";
            reportDateRange = $"Từ ngày: {dtpRevenueStartDate.Value.ToShortDateString()} đến ngày: {dtpRevenueEndDate.Value.ToShortDateString()}";
            currentRowIndex = 0; // Reset số trang
            printPreviewDialogRevenue.ShowDialog();
        }

        private void btnPrintBestseller_Click(object sender, EventArgs e)
        {
            // Báo cáo Bán hàng theo đồ uống
            dgvToPrint = dgvProductSales; // Gán DataGridView cần in
            reportTitle = "BÁO CÁO SẢN PHẨM BÁN CHẠY NHẤT";
            reportDateRange = $"Từ ngày: {dtpProductSalesStartDate.Value.ToShortDateString()} đến ngày: {dtpProductSalesEndDate.Value.ToShortDateString()}";
            currentRowIndex = 0; // Reset số trang
            printPreviewDialogProductSales.ShowDialog();
        }
        private void LoadDashboard()
        {
            try
            {
                DateTime start = dtpDashFrom.Value.Date;
                DateTime end = dtpDashTo.Value.Date;

                // 1. Doanh thu
                var revenue = _reportBLL.GetRevenueReport(start, end);
                decimal totalRevenue = revenue.Sum(x => x.Tongtien);
                lblDashRevenue.Text = $"Tổng doanh thu: {totalRevenue:N0} VNĐ";

                // 2. Tổng số đơn trong khoảng ngày
                int totalOrders = _donhangBLL.GetOrderCount(start, end);
                lblDashTotalOrders.Text = $"Tổng số đơn: {totalOrders:N0}";

                // 3. Giá trị đơn trung bình (AOV)
                decimal aov = totalOrders > 0 ? totalRevenue / totalOrders : 0m;
                lblDashAOV.Text = $"Giá trị đơn trung bình: {aov:N0} VNĐ";

                // 4. Load biểu đồ nếu có
                LoadChart(start, end);
                LoadChartRevenueByHour(end);

                if (revenue.Any())
                {
                    DateTime lastDate = revenue.Last().Ngay;
                    LoadChartRevenueByHour(lastDate);
                }
                else
                {
                    chartRevenueByHour.Series.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Dashboard: " + ex.Message);
            }
        }
        /// <summary>
        /// Hàm dùng để load chart
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void LoadChart(DateTime start, DateTime end)
        {
            chartDashboard.Series.Clear();

            var series = new System.Windows.Forms.DataVisualization.Charting.Series("DoanhThu");
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            var revenueList = _reportBLL.GetRevenueReport(start, end);

            foreach (var item in revenueList)
            {
                var p = series.Points.AddXY(item.Ngay.ToString("dd/MM/yy"), item.Tongtien);
            }

            chartDashboard.Series.Add(series);
        }
        /// <summary>
        /// Hàm load doanh thu cho pie chart    
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private DataTable GetDoanhThuTheoLoai(DateTime start, DateTime end)
        {
            string query = @"
SELECT l.Tenloai, SUM(ct.Soluong * ct.Dongia) AS Doanhthu
FROM Loaidouong l
JOIN Douong d ON d.Maloai = l.Maloai
JOIN Chitietdonhang ct ON ct.Madouong = d.Madouong
JOIN Donhang dh ON dh.Madonhang = ct.Madonhang
WHERE dh.Thoigiandat >= @fromDate AND dh.Thoigiandat < @toDatePlus1
GROUP BY l.Tenloai";

            var dt = new DataTable();

            using (var conn = new SQLiteConnection(connectionString))
            using (var cmd = new SQLiteCommand(query, conn))
            using (var da = new SQLiteDataAdapter(cmd))
            {
                // dùng BETWEEN dạng [start, end+1) để không bị mất phần giờ
                cmd.Parameters.AddWithValue("@fromDate", start.Date);
                cmd.Parameters.AddWithValue("@toDatePlus1", end.Date.AddDays(1));

                conn.Open();
                da.Fill(dt);
            }

            return dt;
        }
        /// <summary>
        /// Hàm load pie chart
        /// </summary>
        private void LoadPieChart()
        {
            DateTime start = dtpDashFrom.Value.Date;
            DateTime end = dtpDashTo.Value.Date;

            DataTable dt = GetDoanhThuTheoLoai(start, end);

            if (dt.Rows.Count == 0)
            {
                formsPlotPie.Plot.Clear();
                formsPlotPie.Plot.Title("Không có dữ liệu trong khoảng ngày đã chọn");
                formsPlotPie.Refresh();
                return;
            }

            double[] values = dt.AsEnumerable()
                                .Select(r => Convert.ToDouble(r["Doanhthu"]))
                                .ToArray();

            string[] labels = dt.AsEnumerable()
                                .Select(r => r["Tenloai"].ToString())
                                .ToArray();

            formsPlotPie.Plot.Clear();

            var pie = formsPlotPie.Plot.AddPie(values);
            pie.SliceLabels = labels;       // Tên loại đồ uống
            pie.ShowPercentages = true;     // Hiện %
            pie.DonutSize = .5;

            formsPlotPie.Plot.Legend();

            formsPlotPie.Refresh();
        }


        private void dtpDashTo_ValueChanged(object sender, EventArgs e)
        {
            LoadDashboard();
            LoadPieChart();
            lblPieTitle.Visible = true;
            lblPieTitle.Text = $"Doanh thu theo loại đồ uống ({dtpDashFrom.Value:dd/MM/yyyy} - {dtpDashTo.Value:dd/MM/yyyy})";
        }

        private void LoadChartRevenueByHour(DateTime date)
        {
            chartRevenueByHour.Series.Clear();
            chartRevenueByHour.ChartAreas[0].AxisX.Interval = 1;

            var dt = _donhangBLL.GetRevenueByHour(date);

            var series = new System.Windows.Forms.DataVisualization.Charting.Series("DoanhThuTheoGio");
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            foreach (DataRow row in dt.Rows)
            {
                string gio = row["Gio"].ToString() + "h";
                decimal doanhThu = Convert.ToDecimal(row["DoanhThu"]);
                series.Points.AddXY(gio, doanhThu);
            }

            chartRevenueByHour.Series.Add(series);

            //THÊM TITLE CÓ NGÀY ĐƯỢC CHỌN
            chartRevenueByHour.Titles.Clear();
            var title = chartRevenueByHour.Titles.Add(
                $"Doanh thu theo giờ ngày {date:dd/MM/yyyy}");
            //CHỈNH FONT ĐẬM + KÍCH THƯỚC
            title.Font = new Font("Arial", 14, FontStyle.Bold);
            title.ForeColor = Color.Red; // (tuỳ chọn)
            title.Alignment = ContentAlignment.TopCenter; // căn giữa
        }
        private void chartDashboard_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestResult result = chartDashboard.HitTest(e.X, e.Y);

            if (result.ChartElementType == ChartElementType.DataPoint &&
                result.Series != null &&
                result.PointIndex >= 0)
            {
                DataPoint point = result.Series.Points[result.PointIndex];

                // AxisLabel đang là "dd/MM/yy" → convert lại DateTime
                string label = point.AxisLabel;

                if (DateTime.TryParseExact(label, "dd/MM/yy",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out DateTime selectedDate))
                {
                    LoadChartRevenueByHour(selectedDate);
                }
            }
        }

        private void dtpDashFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadDashboard();
            LoadPieChart();
            lblPieTitle.Visible = true;
            lblPieTitle.Text = $"Doanh thu theo loại đồ uống ({dtpDashFrom.Value:dd/MM/yyyy} - {dtpDashTo.Value:dd/MM/yyyy})";
        }

        private void lblDashAOV_Click(object sender, EventArgs e)
        {

        }

        private void lblDashTotalOrders_Click(object sender, EventArgs e)
        {

        }

        private void formsPlotPie_Load(object sender, EventArgs e)
        {

        }

        private void dgvProductSales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dtpProductSalesStartDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpProductSalesEndDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void lblTotalPrice_Click(object sender, EventArgs e)
        {

        }
    }
}