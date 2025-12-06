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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CoffeeManagementSystem
{
    /// <summary>
    /// ReportForm:
    /// - Form tổng hợp các loại báo cáo & dashboard:
    ///   + Tab 1: TOP khách hàng tiềm năng (dựa trên điểm tích lũy).
    ///   + Tab 2: Báo cáo doanh thu theo ngày.
    ///   + Tab 3: Báo cáo bán hàng theo đồ uống.
    ///   + Dashboard: doanh thu tổng, số đơn, AOV, biểu đồ cột, pie chart, biểu đồ theo giờ.
    /// - Sử dụng:
    ///   + ReportBLL để lấy các dữ liệu báo cáo (khách, doanh thu, bán hàng).
    ///   + DonhangBLL để lấy các thống kê đơn hàng (số đơn, doanh thu theo giờ).
    /// - Có chức năng in: dùng chung 1 hàm printDocument_PrintPage cho 3 loại DataGridView khác nhau.
    /// </summary>
    public partial class ReportForm : Form
    {
        // Chuỗi kết nối SQLite lấy từ App.config
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["SqliteDbConnection"].ConnectionString;

        private DonhangBLL _donhangBLL;
        private ReportBLL _reportBLL;

        // Các tài liệu in cho 3 loại báo cáo (khách hàng, doanh thu, sản phẩm)
        private PrintDocument printDocumentPotentialCustomers;
        private PrintDocument printDocumentRevenue;
        private PrintDocument printDocumentProductSales;

        // Các dialog xem trước khi in
        private PrintPreviewDialog printPreviewDialogPotentialCustomers;
        private PrintPreviewDialog printPreviewDialogRevenue;
        private PrintPreviewDialog printPreviewDialogProductSales;

        // DGV đang được in (dùng chung cho 3 loại)
        private DataGridView dgvToPrint;

        // Dùng để in nhiều trang: vị trí hàng hiện tại
        private int currentRowIndex = 0;

        // Tiêu đề & khoảng thời gian cho báo cáo (hiển thị trên bản in)
        private string reportTitle = "";
        private string reportDateRange = "";

        private string _signerName;
        private readonly string logoPath = Path.Combine(Application.StartupPath, "Assets", "Images", "logo.png");

        // ===== THÔNG TIN CÔNG TY =====
        private const string CompanyName = "CÀ PHÊ HOÀNG VIỆT";
        private const string CompanySlogan = "Ngon từ hạt, ngọt từ giọt cà phê";


        /// <summary>
        /// Constructor:
        /// - Khởi tạo BLL, cấu hình chart, formsPlot, đối tượng in.
        /// - Gắn event handler cho TabControl, DateTimePicker.
        /// - Gọi load báo cáo tương ứng với tab đầu tiên.
        /// </summary>
        public ReportForm(string maNhanVien, string tenNhanVien)
        {
            InitializeComponent();
            _signerName = tenNhanVien;
            // Chart doanh thu (cột) – dùng sự kiện click để drill-down theo ngày sang biểu đồ theo giờ
            chartDashboard.MouseClick += chartDashboard_MouseClick;

            _donhangBLL = new DonhangBLL();

            // Cố định pie chart: không cho pan/zoom, chỉ xem
            formsPlotPie.Configuration.Pan = false;
            formsPlotPie.Configuration.Zoom = false;
            formsPlotPie.Configuration.ScrollWheelZoom = false;
            formsPlotPie.Configuration.RightClickDragZoom = false;
            formsPlotPie.Configuration.MiddleClickAutoAxis = false;

            _reportBLL = new ReportBLL();

            // Khởi tạo PrintDocument & Preview cho báo cáo TOP khách hàng
            printDocumentPotentialCustomers = new PrintDocument();
            printDocumentPotentialCustomers.PrintPage += new PrintPageEventHandler(this.printDocument_PrintPage);
            printPreviewDialogPotentialCustomers = new PrintPreviewDialog();
            printPreviewDialogPotentialCustomers.Document = printDocumentPotentialCustomers;

            // Khởi tạo PrintDocument & Preview cho báo cáo Doanh thu
            printDocumentRevenue = new PrintDocument();
            printDocumentRevenue.PrintPage += new PrintPageEventHandler(this.printDocument_PrintPage);
            printPreviewDialogRevenue = new PrintPreviewDialog();
            printPreviewDialogRevenue.Document = printDocumentRevenue;

            // Khởi tạo PrintDocument & Preview cho báo cáo Bán hàng theo đồ uống
            printDocumentProductSales = new PrintDocument();
            printDocumentProductSales.PrintPage += new PrintPageEventHandler(this.printDocument_PrintPage);
            printPreviewDialogProductSales = new PrintPreviewDialog();
            printPreviewDialogProductSales.Document = printDocumentProductSales;

            // Giá trị mặc định cho bộ lọc ngày báo cáo doanh thu: 1 tháng gần nhất
            dtpRevenueStartDate.Value = DateTime.Now.AddMonths(-1);
            dtpRevenueEndDate.Value = DateTime.Now;

            // Gắn sự kiện cho TabControl + DateTimePicker để tự reload báo cáo khi thay đổi
            this.tabControlReports.SelectedIndexChanged += new EventHandler(this.tabControlReports_SelectedIndexChanged);
            this.dtpRevenueStartDate.ValueChanged += new EventHandler(this.dtpRevenue_ValueChanged);
            this.dtpRevenueEndDate.ValueChanged += new EventHandler(this.dtpRevenue_ValueChanged);
            this.dtpProductSalesStartDate.ValueChanged += new EventHandler(this.dtpProductSales_ValueChanged);
            this.dtpProductSalesEndDate.ValueChanged += new EventHandler(this.dtpProductSales_ValueChanged);

            // Khi form mở lần đầu: load báo cáo cho tab hiện tại
            tabControlReports_SelectedIndexChanged(tabControlReports, EventArgs.Empty);
        }

        /// <summary>
        /// Hàm in chung cho cả 3 báo cáo:
        /// - Đọc dữ liệu từ dgvToPrint (được gán trước khi ShowDialog print preview).
        /// - Vẽ tiêu đề, khoảng thời gian, header cột, từng dòng dữ liệu, phân trang nếu vượt quá chiều cao trang.
        /// - Dùng currentRowIndex để nhớ đang in tới dòng nào.
        /// </summary>
        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new Font("Arial", 9);              // Font dữ liệu
            Font headerFont = new Font("Arial", 16, FontStyle.Bold);     // Tiêu đề chính
            Font subHeaderFont = new Font("Arial", 12, FontStyle.Bold);  // Tiêu đề phụ (khoảng thời gian)
            Pen borderPen = new Pen(Color.Black, 1);       // Viền bảng
            float y = DrawHeader(graphics, e);
            DrawFooter(graphics, e);
            float x = e.MarginBounds.Left;
            float lineHeight = font.GetHeight() + 5;
            float currentX;

            // === 3. Tính chiều rộng các cột in ra (dựa vào DGV) ===
            List<float> columnWidths = new List<float>();
            float totalPrintableWidth = e.MarginBounds.Width;

            foreach (DataGridViewColumn col in dgvToPrint.Columns)
            {
                if (col.Visible)
                {
                    // Nếu đang dùng AutoSizeMode = Fill → ban đầu ghi -1, sau đó chia đều phần còn lại
                    if (col.AutoSizeMode == DataGridViewAutoSizeColumnMode.Fill)
                    {
                        columnWidths.Add(-1);
                    }
                    else
                    {
                        float actualWidth = (float)col.Width;
                        columnWidths.Add(actualWidth);
                        totalPrintableWidth -= actualWidth;
                    }
                }
                else
                {
                    columnWidths.Add(0);
                }
            }

            // Phân bổ phần còn lại cho các cột Fill
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

            // Tránh trường hợp width âm
            for (int i = 0; i < columnWidths.Count; i++)
            {
                if (columnWidths[i] < 0) columnWidths[i] = 0;
            }

            // === 4. Vẽ header cột ===
            currentX = x;
            float headerRowHeight = subHeaderFont.GetHeight() + 10;
            for (int i = 0; i < dgvToPrint.Columns.Count; i++)
            {
                DataGridViewColumn col = dgvToPrint.Columns[i];
                if (col.Visible)
                {
                    float colWidth = columnWidths[i];
                    RectangleF headerRect = new RectangleF(currentX, y, colWidth, headerRowHeight);

                    graphics.FillRectangle(Brushes.LightGray, headerRect);
                    graphics.DrawRectangle(borderPen, currentX, y, colWidth, headerRowHeight);

                    StringFormat headerSf = new StringFormat();
                    headerSf.Alignment = StringAlignment.Center;
                    headerSf.LineAlignment = StringAlignment.Center;
                    headerSf.Trimming = StringTrimming.EllipsisCharacter;
                    headerSf.FormatFlags = StringFormatFlags.NoWrap;

                    graphics.DrawString(col.HeaderText, subHeaderFont, Brushes.Black, headerRect, headerSf);
                    currentX += colWidth;
                }
            }
            y += headerRowHeight;

            // === 5. In dữ liệu từng dòng, tự động phân trang khi hết chỗ ===
            while (currentRowIndex < dgvToPrint.Rows.Count)
            {
                DataGridViewRow row = dgvToPrint.Rows[currentRowIndex];
                if (row.Visible)
                {
                    // Nếu dòng tiếp theo vượt quá margin dưới → sang trang mới
                    if (y + lineHeight > e.MarginBounds.Bottom)
                    {
                        e.HasMorePages = true;
                        currentRowIndex++;
                        return; // Kết thúc trang hiện tại
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

                            // Một số cột có định dạng đặc biệt
                            if (col.DefaultCellStyle.Format != null && cellValue is IFormattable)
                            {
                                text = ((IFormattable)cellValue).ToString(col.DefaultCellStyle.Format, CultureInfo.CurrentCulture);
                            }
                            else if (col.Name == "STT" && dgvToPrint.Name == "dgvPotentialCustomers")
                            {
                                // STT khách hàng tiềm năng
                                text = (currentRowIndex + 1).ToString();
                            }
                            else if (col.Name == "No" && dgvToPrint.Name == "dgvRevenue")
                            {
                                // STT doanh thu
                                text = (currentRowIndex + 1).ToString();
                            }
                            else if (col.Name == "TyLeDongGopDoanhThu" && dgvToPrint.Name == "dgvProductSales")
                            {
                                // Tính lại % đóng góp doanh thu cho báo cáo bán hàng
                                var dataSourceItem = dgvToPrint.Rows[currentRowIndex].DataBoundItem;
                                if (dataSourceItem is ProductSalesReportItem productItem)
                                {
                                    decimal overallTotalRevenue = ((List<ProductSalesReportItem>)dgvToPrint.DataSource).Sum(item => item.TongDoanhThuMon);
                                    if (overallTotalRevenue > 0)
                                    {
                                        text = ((double)productItem.TongDoanhThuMon / (double)overallTotalRevenue).ToString("P2", CultureInfo.CurrentCulture);
                                    }
                                    else
                                    {
                                        text = "0.00%";
                                    }
                                }
                            }

                            // Căn lề text theo alignment của cột
                            StringFormat cellSf = new StringFormat();
                            cellSf.Trimming = StringTrimming.EllipsisCharacter;
                            cellSf.FormatFlags = StringFormatFlags.NoWrap;

                            if (col.DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight ||
                                col.DefaultCellStyle.Alignment == DataGridViewContentAlignment.TopRight ||
                                col.DefaultCellStyle.Alignment == DataGridViewContentAlignment.BottomRight)
                            {
                                cellSf.Alignment = StringAlignment.Far;
                            }
                            else if (col.DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleCenter ||
                                     col.DefaultCellStyle.Alignment == DataGridViewContentAlignment.TopCenter ||
                                     col.DefaultCellStyle.Alignment == DataGridViewContentAlignment.BottomCenter)
                            {
                                cellSf.Alignment = StringAlignment.Center;
                            }
                            else
                            {
                                cellSf.Alignment = StringAlignment.Near;
                            }

                            RectangleF cellRect = new RectangleF(currentX, y, colWidth, lineHeight);
                            graphics.DrawString(text, font, Brushes.Black, cellRect, cellSf);
                            graphics.DrawRectangle(borderPen, currentX, y, colWidth, lineHeight);

                            currentX += colWidth;
                        }
                    }
                    y += lineHeight;
                }
                currentRowIndex++;
            }
           
            // Hết dữ liệu -> kết thúc in
            e.HasMorePages = false;
            currentRowIndex = 0; // reset cho lần in sau
        }

        /// <summary>
        /// Khi người dùng đổi ngày bắt đầu/kết thúc trong tab doanh thu:
        /// - Chỉ reload báo cáo nếu đang đứng đúng tab doanh thu.
        /// </summary>
        private void dtpRevenue_ValueChanged(object sender, EventArgs e)
        {
            if (tabControlReports.SelectedTab == tabPage2) // Tab Doanh thu
            {
                LoadRevenueReport();
            }
        }

        /// <summary>
        /// Khi người dùng đổi ngày bắt đầu/kết thúc trong tab bán hàng theo đồ uống:
        /// - Chỉ reload khi đứng đúng tab sản phẩm.
        /// </summary>
        private void dtpProductSales_ValueChanged(object sender, EventArgs e)
        {
            if (tabControlReports.SelectedTab == tabPage3)
            {
                LoadProductSalesReport();
            }
        }

        /// <summary>
        /// Khi chuyển tab trên TabControl:
        /// - Tab 1: load TOP khách hàng tiềm năng.
        /// - Tab 2: load báo cáo doanh thu.
        /// - Tab 3: load báo cáo bán hàng theo đồ uống.
        /// </summary>
        private void tabControlReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlReports.SelectedTab == tabPage1)
            {
                LoadPotentialCustomersReport();
            }
            else if (tabControlReports.SelectedTab == tabPage2)
            {
                LoadRevenueReport();
            }
            else if (tabControlReports.SelectedTab == tabPage3)
            {
                LoadProductSalesReport();
            }
        }

        /// <summary>
        /// Load báo cáo TOP khách hàng tiềm năng:
        /// - Gọi ReportBLL.GetPotentialCustomersReport().
        /// - Cấu hình lại cột cho dgvPotentialCustomers (thêm cột STT tự sinh, cột mã, tên, sđt, email, ngày đăng ký, điểm).
        /// - Gán DataSource = List&lt;Khachhang&gt;.
        /// </summary>
        private void LoadPotentialCustomersReport()
        {
            try
            {
                List<Khachhang> potentialCustomers = _reportBLL.GetPotentialCustomersReport();

                dgvPotentialCustomers.DataSource = null;
                dgvPotentialCustomers.Columns.Clear();

                // Cột STT (tự đánh số)
                DataGridViewTextBoxColumn sttColumn = new DataGridViewTextBoxColumn();
                sttColumn.Name = "STT";
                sttColumn.HeaderText = "STT";
                sttColumn.Width = 50;
                sttColumn.ReadOnly = true;
                sttColumn.Resizable = DataGridViewTriState.False;
                sttColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvPotentialCustomers.Columns.Add(sttColumn);

                // Mã khách hàng
                DataGridViewTextBoxColumn maKHColumn = new DataGridViewTextBoxColumn();
                maKHColumn.DataPropertyName = "Makhachhang";
                maKHColumn.Name = "Makhachhang";
                maKHColumn.HeaderText = "Mã Khách hàng";
                maKHColumn.Width = 120;
                maKHColumn.ReadOnly = true;
                maKHColumn.Resizable = DataGridViewTriState.False;
                maKHColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvPotentialCustomers.Columns.Add(maKHColumn);

                // Tên khách hàng
                DataGridViewTextBoxColumn tenKHColumn = new DataGridViewTextBoxColumn();
                tenKHColumn.DataPropertyName = "Hoten";
                tenKHColumn.Name = "TenKhachhang";
                tenKHColumn.HeaderText = "Tên Khách hàng";
                tenKHColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                tenKHColumn.ReadOnly = true;
                tenKHColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvPotentialCustomers.Columns.Add(tenKHColumn);

                // Số điện thoại
                DataGridViewTextBoxColumn sdtColumn = new DataGridViewTextBoxColumn();
                sdtColumn.DataPropertyName = "Sodienthoai";
                sdtColumn.Name = "Sodienthoa";
                sdtColumn.HeaderText = "Số điện thoại";
                sdtColumn.Width = 120;
                sdtColumn.ReadOnly = true;
                sdtColumn.Resizable = DataGridViewTriState.False;
                sdtColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvPotentialCustomers.Columns.Add(sdtColumn);

                // Email
                DataGridViewTextBoxColumn emailColumn = new DataGridViewTextBoxColumn();
                emailColumn.DataPropertyName = "Email";
                emailColumn.Name = "Email";
                emailColumn.HeaderText = "Email";
                emailColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                emailColumn.ReadOnly = true;
                emailColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvPotentialCustomers.Columns.Add(emailColumn);

                // Ngày đăng ký
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

                // Điểm tích lũy
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

                // Gán nguồn dữ liệu
                dgvPotentialCustomers.DataSource = potentialCustomers;

                if (potentialCustomers.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy khách hàng nào có điểm tích lũy.",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo TOP khách hàng tiềm năng: {ex.Message}\nVui lòng kiểm tra kết nối CSDL và dữ liệu.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Định dạng cột STT cho dgvPotentialCustomers (tự đánh số từ 1).
        /// </summary>
        private void dgvPotentialCustomers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvPotentialCustomers.Columns[e.ColumnIndex].Name == "STT" && e.RowIndex >= 0)
            {
                e.Value = e.RowIndex + 1;
                e.FormattingApplied = true;
            }
        }

        /// <summary>
        /// Load báo cáo doanh thu theo ngày:
        /// - Kiểm tra khoảng ngày hợp lệ.
        /// - Gọi ReportBLL.GetRevenueReport(startDate, endDate).
        /// - Cấu hình cột cho dgvRevenue (STT, Ngày giao dịch, Doanh thu).
        /// - Tính tổng doanh thu & hiển thị dưới label lblTotalPrice.
        /// </summary>
        private void LoadRevenueReport()
        {
            try
            {
                DateTime startDate = dtpRevenueStartDate.Value.Date;
                DateTime endDate = dtpRevenueEndDate.Value.Date;

                if (startDate > endDate)
                {
                    MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.",
                                    "Lỗi Ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    dgvRevenue.DataSource = null;
                    dgvRevenue.Rows.Clear();
                    lblTotalPrice.Text = "0 VNĐ";
                    return;
                }

                List<RevenueReportItem> revenueData = _reportBLL.GetRevenueReport(startDate, endDate);

                dgvRevenue.SuspendLayout();

                dgvRevenue.DataSource = null;
                dgvRevenue.Columns.Clear();
                dgvRevenue.AutoGenerateColumns = false;

                // Cột STT (No)
                DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
                colNo.Name = "No";
                colNo.HeaderText = "STT";
                colNo.DataPropertyName = null;
                colNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                colNo.FillWeight = 33.33f;
                colNo.ReadOnly = true;
                colNo.Resizable = DataGridViewTriState.False;
                colNo.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvRevenue.Columns.Add(colNo);

                // Cột Ngày giao dịch
                DataGridViewTextBoxColumn colDate = new DataGridViewTextBoxColumn();
                colDate.Name = "Date";
                colDate.HeaderText = "Ngày giao dịch";
                colDate.DataPropertyName = "Ngay";
                colDate.DefaultCellStyle.Format = "dd/MM/yyyy";
                colDate.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                colDate.FillWeight = 33.33f;
                colDate.ReadOnly = true;
                colDate.Resizable = DataGridViewTriState.False;
                colDate.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvRevenue.Columns.Add(colDate);

                // Cột Doanh thu
                DataGridViewTextBoxColumn colPrice = new DataGridViewTextBoxColumn();
                colPrice.Name = "Price";
                colPrice.HeaderText = "Doanh thu";
                colPrice.DataPropertyName = "Tongtien";
                colPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                colPrice.DefaultCellStyle.Format = "N0";
                colPrice.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                colPrice.FillWeight = 33.33f;
                colPrice.ReadOnly = true;
                colPrice.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvRevenue.Columns.Add(colPrice);

                dgvRevenue.DataSource = revenueData;

                // Gán STT thủ công
                for (int i = 0; i < dgvRevenue.Rows.Count; i++)
                {
                    dgvRevenue.Rows[i].Cells["No"].Value = i + 1;
                }

                dgvRevenue.ResumeLayout();

                decimal totalRevenue = revenueData.Sum(r => r.Tongtien);
                lblTotalPrice.Text = totalRevenue.ToString("N0") + " VNĐ";

                if (revenueData.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu doanh thu trong khoảng thời gian đã chọn.",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Ngày",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo doanh thu: {ex.Message}\nVui lòng kiểm tra kết nối CSDL và dữ liệu.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Định dạng cột STT cho dgvRevenue (No).
        /// </summary>
        private void dgvRevenue_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvRevenue.Columns[e.ColumnIndex].Name == "No" && e.RowIndex >= 0)
            {
                e.Value = e.RowIndex + 1;
                e.FormattingApplied = true;
            }
        }

        /// <summary>
        /// Load báo cáo bán hàng theo đồ uống:
        /// - Lấy dữ liệu 1 tháng gần nhất (startDate = Now - 1 tháng, endDate = Now).
        /// - Gọi ReportBLL.GetProductSalesReport().
        /// - Cấu hình cột: STT, Mã đồ uống, Tên, Loại, Số lượng bán, Tổng doanh thu, Tỷ lệ đóng góp (% trên tổng).
        /// - Tính tổng doanh thu & hiển thị lên lblTong.
        /// </summary>
        private void LoadProductSalesReport()
        {
            try
            {
                DateTime startDate = DateTime.Now.AddMonths(-1).Date;
                DateTime endDate = DateTime.Now.Date;

                dgvProductSales.DataSource = null;
                dgvProductSales.Rows.Clear();
                dgvProductSales.Columns.Clear();

                if (startDate > endDate)
                {
                    MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.",
                                    "Lỗi Ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lblTong.Text = "0 VNĐ";
                    return;
                }

                List<ProductSalesReportItem> productSalesData = _reportBLL.GetProductSalesReport(startDate, endDate);

                decimal overallTotalRevenue = productSalesData.Sum(item => item.TongDoanhThuMon);
                dgvProductSales.AutoGenerateColumns = false;

                // Cột STT
                DataGridViewTextBoxColumn sttColumn = new DataGridViewTextBoxColumn();
                sttColumn.Name = "STT";
                sttColumn.HeaderText = "STT";
                sttColumn.Width = 50;
                sttColumn.ReadOnly = true;
                sttColumn.Resizable = DataGridViewTriState.False;
                sttColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvProductSales.Columns.Add(sttColumn);

                // Mã đồ uống
                DataGridViewTextBoxColumn maDouongColumn = new DataGridViewTextBoxColumn();
                maDouongColumn.DataPropertyName = "Madouong";
                maDouongColumn.Name = "Madouong";
                maDouongColumn.HeaderText = "Mã Đồ uống";
                maDouongColumn.Width = 100;
                maDouongColumn.ReadOnly = true;
                maDouongColumn.Resizable = DataGridViewTriState.False;
                maDouongColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvProductSales.Columns.Add(maDouongColumn);

                // Tên đồ uống
                DataGridViewTextBoxColumn tenDouongColumn = new DataGridViewTextBoxColumn();
                tenDouongColumn.DataPropertyName = "Tendouong";
                tenDouongColumn.Name = "Tendouong";
                tenDouongColumn.HeaderText = "Tên Đồ uống";
                tenDouongColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                tenDouongColumn.ReadOnly = true;
                tenDouongColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvProductSales.Columns.Add(tenDouongColumn);

                // Loại đồ uống
                DataGridViewTextBoxColumn maLoaiColumn = new DataGridViewTextBoxColumn();
                maLoaiColumn.DataPropertyName = "Maloai";
                maLoaiColumn.Name = "Maloai";
                maLoaiColumn.HeaderText = "Loại Đồ uống";
                maLoaiColumn.Width = 120;
                maLoaiColumn.ReadOnly = true;
                maLoaiColumn.Resizable = DataGridViewTriState.False;
                maLoaiColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvProductSales.Columns.Add(maLoaiColumn);

                // Số lượng bán
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

                // Tổng doanh thu món
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

                // Tỷ lệ đóng góp (%) – không bind trực tiếp, tính riêng
                DataGridViewTextBoxColumn tyLeDongGopColumn = new DataGridViewTextBoxColumn();
                tyLeDongGopColumn.Name = "TyLeDongGopDoanhThu";
                tyLeDongGopColumn.HeaderText = "Tỷ lệ đóng góp";
                tyLeDongGopColumn.Width = 120;
                tyLeDongGopColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                tyLeDongGopColumn.DefaultCellStyle.Format = "P2";
                tyLeDongGopColumn.ReadOnly = true;
                tyLeDongGopColumn.Resizable = DataGridViewTriState.False;
                tyLeDongGopColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvProductSales.Columns.Add(tyLeDongGopColumn);

                dgvProductSales.DataSource = productSalesData;

                // Điền STT & % đóng góp
                for (int i = 0; i < dgvProductSales.Rows.Count; i++)
                {
                    ProductSalesReportItem item = productSalesData[i];
                    dgvProductSales.Rows[i].Cells["STT"].Value = i + 1;

                    if (overallTotalRevenue > 0)
                    {
                        dgvProductSales.Rows[i].Cells["TyLeDongGopDoanhThu"].Value =
                            (double)item.TongDoanhThuMon / (double)overallTotalRevenue;
                    }
                    else
                    {
                        dgvProductSales.Rows[i].Cells["TyLeDongGopDoanhThu"].Value = 0d;
                    }
                }

                lblTong.Text = overallTotalRevenue.ToString("N0") + " VNĐ";

                if (productSalesData.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu bán hàng theo đồ uống trong khoảng thời gian đã chọn.",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Ngày",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo bán hàng theo đồ uống: {ex.Message}\nVui lòng kiểm tra kết nối CSDL và dữ liệu.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DrawFooter(Graphics g, PrintPageEventArgs e)
        {
            float y = e.MarginBounds.Bottom + 10;

            Font footerFont = new Font("Arial", 9, FontStyle.Italic);

            // ĐƯỜNG KẺ
            g.DrawLine(Pens.Gray, e.MarginBounds.Left, y, e.MarginBounds.Right, y);
            y += 5;

            // PHẦN MỀM
            g.DrawString(
                "Phần mềm Coffee Management System",
                footerFont,
                Brushes.Gray,
                e.MarginBounds.Left,
                y
            );

            // NGÀY IN
            g.DrawString(
                $"In ngày {DateTime.Now:dd/MM/yyyy HH:mm}",
                footerFont,
                Brushes.Gray,
                e.MarginBounds.Right - 180,
                y
            );

            // CHỮ KÝ
            float signX = e.MarginBounds.Right - 220;
            float signY = y + 30;

            g.DrawString(
                "Người lập báo cáo",
                footerFont,
                Brushes.Black,
                signX,
                signY
            );

            signY += 35;

            // FONT CHỮ KÝ (viết tay)
            Font signatureFont = new Font("Pacifico", 18, FontStyle.Regular);

            g.DrawString(
                signerName,
                signatureFont,
                Brushes.Black,
                signX,
                signY
            );

            g.DrawString(
                _signerName,
                new Font("Arial", 10, FontStyle.Bold),
                Brushes.Black,
                signX,
                signY
            );
        }

        private float DrawHeader(Graphics g, PrintPageEventArgs e)
        {
            float y = e.MarginBounds.Top;

            // ===== LOGO TỪ RESOURCE =====
            Image logo = Properties.Resources.LogoCompany;

            float logoWidth = 80;
            float logoHeight = logo.Height * logoWidth / logo.Width;

            g.DrawImage(
                logo,
                e.MarginBounds.Left,
                y,
                logoWidth,
                logoHeight
            );

            // TÊN CÔNG TY
            g.DrawString(
               CompanyName,
                new Font("Arial", 16, FontStyle.Bold),
                Brushes.Black,
                e.MarginBounds.Left + logoWidth + 15,
                y
            );

            y += 26;

            // SLOGAN
            g.DrawString(
               CompanySlogan,
                new Font("Arial", 9, FontStyle.Italic),
                Brushes.Gray,
                e.MarginBounds.Left + logoWidth + 15,
                y
            );

            y += logoHeight - 10;

            // ĐƯỜNG KẺ
            g.DrawLine(Pens.Black, e.MarginBounds.Left, y, e.MarginBounds.Right, y);
            y += 15;

            // TIÊU ĐỀ BÁO CÁO
            StringFormat center = new StringFormat { Alignment = StringAlignment.Center };

            g.DrawString(
                reportTitle,
                new Font("Arial", 14, FontStyle.Bold),
                Brushes.Black,
                e.PageBounds.Width / 2,
                y,
                center
            );

            y += 25;

            // PHỤ ĐỀ
            if (!string.IsNullOrEmpty(reportDateRange))
            {
                g.DrawString(
                    reportDateRange,
                    new Font("Arial", 10, FontStyle.Italic),
                    Brushes.Black,
                    e.PageBounds.Width / 2,
                    y,
                    center
                );
                y += 20;
            }
            else
            {
                y += 10;
            }

            return y;
        }


        /// <summary>
        /// Nút In báo cáo TOP khách hàng tiềm năng.
        /// - Gán dgvToPrint = dgvPotentialCustomers, set title rồi mở PrintPreview.
        /// </summary>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            dgvToPrint = dgvPotentialCustomers;
            reportTitle = "BÁO CÁO TOP 10 KHÁCH HÀNG TIỀM NĂNG";
            reportDateRange = "";
            currentRowIndex = 0;
            printPreviewDialogPotentialCustomers.ShowDialog();
        }

        /// <summary>
        /// Nút In báo cáo Doanh thu.
        /// </summary>
        private void btnPrintDoanhThu_Click(object sender, EventArgs e)
        {
            dgvToPrint = dgvRevenue;
            reportTitle = "BÁO CÁO DOANH THU";
            reportDateRange = $"Từ ngày: {dtpRevenueStartDate.Value.ToShortDateString()} đến ngày: {dtpRevenueEndDate.Value.ToShortDateString()}";
            currentRowIndex = 0;
            printPreviewDialogRevenue.ShowDialog();
        }

        /// <summary>
        /// Nút In báo cáo Sản phẩm bán chạy.
        /// </summary>
        private void btnPrintBestseller_Click(object sender, EventArgs e)
        {
            dgvToPrint = dgvProductSales;
            reportTitle = "BÁO CÁO SẢN PHẨM BÁN CHẠY NHẤT";
            reportDateRange = $"Từ ngày: {dtpProductSalesStartDate.Value.ToShortDateString()} đến ngày: {dtpProductSalesEndDate.Value.ToShortDateString()}";
            currentRowIndex = 0;
            printPreviewDialogProductSales.ShowDialog();
        }

        /// <summary>
        /// Load Dashboard tổng quan:
        /// - Gọi ReportBLL.GetRevenueReport để tính tổng doanh thu.
        /// - Gọi DonhangBLL.GetOrderCount để đếm số đơn.
        /// - Tính AOV = totalRevenue / totalOrders.
        /// - Load biểu đồ cột doanh thu & doanh thu theo giờ + pie chart.
        /// </summary>
        private void LoadDashboard()
        {
            try
            {
                DateTime start = dtpDashFrom.Value.Date;
                DateTime end = dtpDashTo.Value.Date;

                // 1. Doanh thu
                var revenue = _reportBLL.GetRevenueReport(start, end);
                decimal totalRevenue = revenue.Sum(x => x.Tongtien);
                lblDashRevenue.Text = $"{totalRevenue:N0} VNĐ";

                // 2. Tổng số đơn
                int totalOrders = _donhangBLL.GetOrderCount(start, end);
                lblDashTotalOrders.Text = $"{totalOrders:N0}";

                // 3. AOV
                decimal aov = totalOrders > 0 ? totalRevenue / totalOrders : 0m;
                lblDashAOV.Text = $"{aov:N0} VNĐ";

                // 4. Biểu đồ cột & theo giờ
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
        /// Vẽ biểu đồ cột doanh thu theo ngày trên chartDashboard.
        /// </summary>
        private void LoadChart(DateTime start, DateTime end)
        {
            chartDashboard.Series.Clear();

            var series = new System.Windows.Forms.DataVisualization.Charting.Series("DoanhThu");
            series.ChartType = SeriesChartType.Column;

            var revenueList = _reportBLL.GetRevenueReport(start, end);

            foreach (var item in revenueList)
            {
                series.Points.AddXY(item.Ngay.ToString("dd/MM/yy"), item.Tongtien);
            }

            chartDashboard.Series.Add(series);
        }

        /// <summary>
        /// Lấy doanh thu theo loại đồ uống (DataTable) trong khoảng ngày:
        /// - Hàm riêng cho pie chart ở dashboard, truy vấn trực tiếp CSDL (không qua BLL).
        /// </summary>
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
                cmd.Parameters.AddWithValue("@fromDate", start.Date);
                cmd.Parameters.AddWithValue("@toDatePlus1", end.Date.AddDays(1));

                conn.Open();
                da.Fill(dt);
            }

            return dt;
        }

        /// <summary>
        /// Load pie chart doanh thu theo loại đồ uống lên formsPlotPie.
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
            pie.SliceLabels = labels;
            pie.ShowPercentages = true;
            pie.DonutSize = .5;

            formsPlotPie.Plot.Legend();
            formsPlotPie.Refresh();
        }

        /// <summary>
        /// Khi đổi ngày "To" trên dashboard:
        /// - Reload dashboard + pie chart + cập nhật text tiêu đề pie.
        /// </summary>
        private void dtpDashTo_ValueChanged(object sender, EventArgs e)
        {
            LoadDashboard();
            LoadPieChart();
            lblPieTitle.Visible = true;
            lblPieTitle.Text = $"Doanh thu theo loại đồ uống ({dtpDashFrom.Value:dd/MM/yyyy} - {dtpDashTo.Value:dd/MM/yyyy})";
        }

        /// <summary>
        /// Load biểu đồ doanh thu theo giờ (chartRevenueByHour) cho 1 ngày cụ thể.
        /// - Dùng DonhangBLL.GetRevenueByHour(date).
        /// </summary>
        private void LoadChartRevenueByHour(DateTime date)
        {
            chartRevenueByHour.Series.Clear();
            chartRevenueByHour.ChartAreas[0].AxisX.Interval = 1;

            var dt = _donhangBLL.GetRevenueByHour(date);

            var series = new System.Windows.Forms.DataVisualization.Charting.Series("DoanhThuTheoGio");
            series.ChartType = SeriesChartType.Column;

            foreach (DataRow row in dt.Rows)
            {
                string gio = row["Gio"].ToString() + "h";
                decimal doanhThu = Convert.ToDecimal(row["DoanhThu"]);
                series.Points.AddXY(gio, doanhThu);
            }

            chartRevenueByHour.Series.Add(series);

            // Title cho chart doanh thu theo giờ
            chartRevenueByHour.Titles.Clear();
            var title = chartRevenueByHour.Titles.Add($"Doanh thu theo giờ ngày {date:dd/MM/yyyy}");
            title.Font = new Font("Arial", 14, FontStyle.Bold);
            title.ForeColor = Color.Red;
            title.Alignment = ContentAlignment.TopCenter;
        }

        /// <summary>
        /// Khi click vào 1 cột trên chartDashboard:
        /// - Lấy ngày tương ứng ở AxisLabel (dd/MM/yy).
        /// - Gọi LoadChartRevenueByHour(selectedDate) để drill-down.
        /// </summary>
        private void chartDashboard_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestResult result = chartDashboard.HitTest(e.X, e.Y);

            if (result.ChartElementType == ChartElementType.DataPoint &&
                result.Series != null &&
                result.PointIndex >= 0)
            {
                DataPoint point = result.Series.Points[result.PointIndex];

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

        /// <summary>
        /// Khi đổi ngày "From" trên dashboard:
        /// - Reload dashboard + pie chart + cập nhật text tiêu đề pie.
        /// </summary>
        private void dtpDashFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadDashboard();
            LoadPieChart();
            lblPieTitle.Visible = true;
            lblPieTitle.Text = $"Doanh thu theo loại đồ uống ({dtpDashFrom.Value:dd/MM/yyyy} - {dtpDashTo.Value:dd/MM/yyyy})";
        }

        // Các event handler trống (VS tự generate, hiện không dùng logic):

        private void formsPlotPie_Load(object sender, EventArgs e) { }

        private void dgvProductSales_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void dtpProductSalesStartDate_ValueChanged(object sender, EventArgs e) { }

        private void dtpProductSalesEndDate_ValueChanged(object sender, EventArgs e) { }

        private void label6_Click(object sender, EventArgs e) { }

        private void label10_Click(object sender, EventArgs e) { }

        private void lblTotalPrice_Click(object sender, EventArgs e) { }

        private void Bieudotron_Click(object sender, EventArgs e) { }
    }
}
