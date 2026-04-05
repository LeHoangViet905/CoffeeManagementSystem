using CoffeeManagementSystem.BLL;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class CustomerForm : Form
    {
        // BLL xử lý nghiệp vụ cho khách hàng
        private KhachhangBLL khachhangBLL;

        public CustomerForm()
        {
            InitializeComponent();
            khachhangBLL = new KhachhangBLL(); // Khởi tạo đối tượng BLL

            // Tải danh sách khách hàng ban đầu
            LoadDanhSachKhachHang();

            // Tìm kiếm theo thời gian thực khi gõ vào ô tìm kiếm
            this.txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);

            // Click vào một dòng trong DataGridView để mở form chi tiết khách hàng
            this.dgvKhachHang.CellClick += new DataGridViewCellEventHandler(dgvKhachHang_CellClick);

            // Nút thêm mới khách hàng
            this.btnAdd.Click += new EventHandler(btnAdd_Click);
        }

        // Sự kiện Form Load: Tải dữ liệu khi Form được hiển thị
        private void CustomerForm_Load(object sender, EventArgs e)
        {
            LoadDanhSachKhachHang(); // Gọi phương thức tải danh sách ban đầu
        }

        /// <summary>
        /// Lấy toàn bộ khách hàng từ BLL và hiển thị lên DataGridView.
        /// </summary>
        private void LoadDanhSachKhachHang()
        {
            try
            {
                // Lấy danh sách khách hàng từ BLL
                List<Khachhang> danhSach = khachhangBLL.GetAllKhachhangs();

                // Gán danh sách làm nguồn dữ liệu cho DataGridView
                dgvKhachHang.DataSource = danhSach;
            }
            catch (InvalidOperationException bllEx) // Lỗi nghiệp vụ từ BLL
            {
                MessageBox.Show("Lỗi nghiệp vụ khi tải danh sách khách hàng: " + bllEx.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Lỗi hệ thống / lỗi không xác định
                MessageBox.Show("Không thể tải danh sách khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Lọc danh sách khách hàng theo từ khóa và hiển thị lên DataGridView.
        /// </summary>
        private void LoadFilteredData(string searchTerm)
        {
            try
            {
                List<Khachhang> ketQuaHienThi;

                // Nếu từ khóa rỗng → trả lại toàn bộ danh sách
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    ketQuaHienThi = khachhangBLL.GetAllKhachhangs();
                }
                else
                {
                    // Có từ khóa → gọi hàm tìm kiếm
                    ketQuaHienThi = khachhangBLL.SearchKhachhangs(searchTerm);
                }

                dgvKhachHang.DataSource = ketQuaHienThi;

                // Thông báo khi không tìm thấy kết quả (chỉ khi có từ khóa)
                if (ketQuaHienThi.Count == 0 && !string.IsNullOrWhiteSpace(searchTerm))
                {
                    MessageBox.Show(
                        $"Không tìm thấy khách hàng nào phù hợp với từ khóa '{searchTerm}'.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (InvalidOperationException bllEx) // Lỗi nghiệp vụ từ BLL
            {
                MessageBox.Show($"Lỗi nghiệp vụ khi tìm kiếm khách hàng: {bllEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException argEx) // Lỗi dữ liệu đầu vào không hợp lệ
            {
                MessageBox.Show($"Lỗi nhập liệu: {argEx.Message}", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện thay đổi nội dung ô tìm kiếm → lọc dữ liệu ngay
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            LoadFilteredData(searchTerm);
        }

        /// <summary>
        /// Khi click vào một ô trên DataGridView: mở form chi tiết của khách hàng được chọn.
        /// </summary>
        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Bỏ qua khi click vào header hoặc dòng không hợp lệ
            if (e.RowIndex >= 0 && e.RowIndex < dgvKhachHang.Rows.Count)
            {
                // Lấy object Khachhang gắn với dòng đang chọn
                Khachhang selectedKhachhang = dgvKhachHang.Rows[e.RowIndex].DataBoundItem as Khachhang;

                if (selectedKhachhang != null)
                {
                    try
                    {
                        // Mở form chi tiết ở chế độ xem/sửa (truyền khách hàng đang chọn)
                        FormChitiet formChiTiet = new FormChitiet(selectedKhachhang);

                        // Nếu form chi tiết trả về OK (thêm/sửa/xóa thành công) → reload danh sách
                        if (formChiTiet.ShowDialog() == DialogResult.OK)
                        {
                            LoadDanhSachKhachHang();
                        }
                    }
                    catch (InvalidOperationException bllEx)
                    {
                        MessageBox.Show(
                            "Lỗi nghiệp vụ khi lấy thông tin chi tiết khách hàng hoặc mở Form: " + bllEx.Message,
                            "Lỗi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                    catch (ArgumentException argEx)
                    {
                        MessageBox.Show(
                            "Lỗi dữ liệu khi lấy thông tin chi tiết khách hàng: " + argEx.Message,
                            "Cảnh báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            "Lỗi khi lấy thông tin chi tiết khách hàng hoặc mở Form: " + ex.Message,
                            "Lỗi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Nút Thêm: mở FormChiTiet ở chế độ thêm mới khách hàng.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Mở form chi tiết KH ở chế độ thêm mới (không truyền object)
                FormChitiet formChiTiet = new FormChitiet();

                if (formChiTiet.ShowDialog() == DialogResult.OK)
                {
                    // Sau khi thêm mới thành công → tải lại danh sách
                    LoadDanhSachKhachHang();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở Form thêm khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện paint của panel (hiện chưa dùng) → có thể dùng để custom giao diện
        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        // Event handler dư (tự sinh bởi Designer), hiện không dùng đến
        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Reload DataGridView khách hàng từ BLL (dùng lại ở nhiều chỗ).
        /// </summary>
        public void LoadGridKhachhang()
        {
            try
            {
                var list = khachhangBLL.GetAllKhachhangs();
                dgvKhachHang.DataSource = null;
                dgvKhachHang.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load dữ liệu: " + ex.Message);
            }
        }

        /// <summary>
        /// Đọc dữ liệu từ file CSV vào DataTable (phục vụ import).
        /// </summary>
        private DataTable ReadCSV(string path)
        {
            DataTable dt = new DataTable();

            using (var reader = new StreamReader(path))
            {
                // Đọc dòng đầu tiên làm header
                string header = reader.ReadLine();
                if (header == null) return dt;

                var columnNames = header.Split(',');
                foreach (string col in columnNames)
                    dt.Columns.Add(col.Trim());

                // Đọc từng dòng dữ liệu
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    dt.Rows.Add(line.Split(','));
                }
            }

            return dt;
        }

        /// <summary>
        /// Đọc dữ liệu từ file Excel (xlsx/xls) vào DataTable (phục vụ import).
        /// </summary>
        private DataTable ReadExcel(string path)
        {
            DataTable dt = new DataTable();

            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var ws = package.Workbook.Worksheets[0]; // Lấy sheet đầu tiên
                int colCount = ws.Dimension.End.Column;
                int rowCount = ws.Dimension.End.Row;

                // Dòng 1: header → tạo cột
                for (int col = 1; col <= colCount; col++)
                    dt.Columns.Add(ws.Cells[1, col].Text);

                // Từ dòng 2 trở đi: dữ liệu → thêm vào DataTable
                for (int row = 2; row <= rowCount; row++)
                {
                    DataRow dr = dt.NewRow();
                    for (int col = 1; col <= colCount; col++)
                        dr[col - 1] = ws.Cells[row, col].Text;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        /// <summary>
        /// Nút import (button3): đọc file Excel/CSV và import danh sách khách hàng vào CSDL.
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xlsx;*.xls|CSV Files|*.csv";
            if (ofd.ShowDialog() != DialogResult.OK) return;

            DataTable dt;
            try
            {
                // Tùy phần mở rộng để chọn cách đọc file
                dt = Path.GetExtension(ofd.FileName).ToLower() == ".csv"
                    ? ReadCSV(ofd.FileName)
                    : ReadExcel(ofd.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đọc file: " + ex.Message);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("File không có dữ liệu.");
                return;
            }

            // Lấy tất cả mã khách hàng hiện có (chỉ gọi BLL 1 lần)
            var existingMa = khachhangBLL.GetAllMaKH();
            var usedMa = new HashSet<string>(existingMa); // Dùng HashSet để check trùng nhanh hơn

            List<Khachhang> list = new List<Khachhang>();

            // Duyệt từng dòng trong DataTable để map sang đối tượng Khachhang
            foreach (DataRow row in dt.Rows)
            {
                string ma;

                // Nếu file có cột Makhachhang và có giá trị → ưu tiên dùng
                if (row.Table.Columns.Contains("Makhachhang") &&
                    !string.IsNullOrWhiteSpace(row["Makhachhang"].ToString()))
                {
                    ma = row["Makhachhang"].ToString().Trim();

                    // Nếu mã đã dùng rồi → sinh mã mới để tránh trùng
                    if (usedMa.Contains(ma))
                        ma = khachhangBLL.GenerateNextMaKHInMemory(usedMa);
                }
                else
                {
                    // Nếu không có cột/không có mã → tự sinh mã mới
                    ma = khachhangBLL.GenerateNextMaKHInMemory(usedMa);
                }

                usedMa.Add(ma); // Đánh dấu mã đã dùng

                Khachhang k = new Khachhang
                {
                    Makhachhang = ma,
                    Hoten = row["Hoten"].ToString().Trim(),
                    Sodienthoai = row["Sodienthoai"].ToString().Trim(),
                    Email = row["Email"].ToString().Trim(),
                    Ngaydangky = Convert.ToDateTime(row["Ngaydangky"]),
                    Diemtichluy = Convert.ToInt32(row["Diemtichluy"])
                };

                list.Add(k);
            }

            if (list.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu hợp lệ để import.");
                return;
            }

            try
            {
                // Gọi BLL thực hiện import (DAL sẽ insert/update tương ứng)
                khachhangBLL.ImportKhachhangs(list);
                MessageBox.Show("Import thành công " + list.Count + " dòng.");
                LoadGridKhachhang(); // Load lại DataGridView sau khi import
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi import: " + ex.Message);
            }
        }

        /// <summary>
        /// Nút mở form chi tiết khách hàng (tên nút đang là btnThemloaidouong).
        /// Dùng để thêm/sửa khách hàng giống btnAdd, tùy bạn cấu hình UI.
        /// </summary>
        private void btnThemloaidouong_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();

            // Mở FormChiTiet ở chế độ thêm mới khách hàng
            FormChitiet detailForm = new FormChitiet();
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                // Sau khi thêm/sửa thành công → tải lại danh sách
                LoadDanhSachKhachHang();
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Files|*.xlsx;*.xls|CSV Files|*.csv";
            sfd.FileName = "KhachHang_Export";
            if (sfd.ShowDialog() != DialogResult.OK) return;
            try
            {
                //Lấy danh sách khách hàng từ BLL
                var list = khachhangBLL.GetAllKhachhangs();
                khachhangBLL.ExportKhachhangToCSV(list, sfd.FileName);
                MessageBox.Show("Xuất file thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file: " + ex.Message);
            }
        }
    }
}
