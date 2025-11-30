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
        private KhachhangBLL khachhangBLL;
        public CustomerForm()
        {
            InitializeComponent();
            khachhangBLL = new KhachhangBLL(); // Khởi tạo đối tượng BLL
            LoadDanhSachKhachHang();
            // Gán sự kiện TextChanged cho TextBox tìm kiếm (để tìm khi gõ)
            this.txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            // Gán sự kiện CellClick cho DataGridView (để hiển thị Form Chi Tiết)
            this.dgvKhachHang.CellClick += new DataGridViewCellEventHandler(dgvKhachHang_CellClick);

            // Gán sự kiện Click cho nút Thêm mới (giả định tên nút là btnAdd)
            this.btnAdd.Click += new EventHandler(btnAdd_Click);
        }

        // Sự kiện Form Load: Tải dữ liệu khi Form được hiển thị
        private void CustomerForm_Load(object sender, EventArgs e)
        {

            LoadDanhSachKhachHang(); // Gọi phương thức tải danh sách ban đầu
        }

        // Phương thức tải danh sách khách hàng và hiển thị lên DataGridView
        private void LoadDanhSachKhachHang()
        {
            try
            {
                // Lấy danh sách khách hàng từ BLL
                List<Khachhang> danhSach = khachhangBLL.GetAllKhachhangs(); // Gọi BLL

                // Gán danh sách làm nguồn dữ liệu cho DataGridView
                dgvKhachHang.DataSource = danhSach;
            }
            catch (InvalidOperationException bllEx) // Bắt lỗi nghiệp vụ từ BLL
            {
                MessageBox.Show("Lỗi nghiệp vụ khi tải danh sách khách hàng: " + bllEx.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu không tải được dữ liệu
                MessageBox.Show("Không thể tải danh sách khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFilteredData(string searchTerm)
        {
            try
            {
                List<Khachhang> ketQuaHienThi;

                // Kiểm tra nếu từ khóa tìm kiếm rỗng hoặc chỉ chứa khoảng trắng
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    // Nếu trống, tải toàn bộ danh sách khách hàng
                    ketQuaHienThi = khachhangBLL.GetAllKhachhangs(); // Gọi BLL
                }
                else
                {
                    // Nếu có từ khóa, gọi phương thức tìm kiếm
                    ketQuaHienThi = khachhangBLL.SearchKhachhangs(searchTerm); // Gọi BLL
                }
                dgvKhachHang.DataSource = ketQuaHienThi;

                if (ketQuaHienThi.Count == 0 && !string.IsNullOrWhiteSpace(searchTerm))
                {
                    MessageBox.Show($"Không tìm thấy khách hàng nào phù hợp với từ khóa '{searchTerm}'.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (InvalidOperationException bllEx) // Bắt lỗi nghiệp vụ từ BLL
            {
                MessageBox.Show($"Lỗi nghiệp vụ khi tìm kiếm khách hàng: {bllEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException argEx) // Bắt lỗi đối số không hợp lệ từ BLL
            {
                MessageBox.Show($"Lỗi nhập liệu: {argEx.Message}", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Lấy từ khóa hiện tại trong TextBox
            string searchTerm = txtSearch.Text.Trim();

            // Gọi phương thức tải dữ liệu đã lọc
            LoadFilteredData(searchTerm);
        }

        // Sự kiện khi click vào một dòng trong DataGridView
        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có phải click vào một dòng dữ liệu hợp lệ không (loại trừ header).
            if (e.RowIndex >= 0 && e.RowIndex < dgvKhachHang.Rows.Count) // Đảm bảo dòng click là hợp lệ
            {
                // Lấy đối tượng Khachhang được liên kết với dòng.
                Khachhang selectedKhachhang = dgvKhachHang.Rows[e.RowIndex].DataBoundItem as Khachhang;

                // Kiểm tra xem selectedKhachhang có phải là một đối tượng Khachhang hợp lệ không.
                if (selectedKhachhang != null)
                {
                    try
                    {

                        FormChitiet formChiTiet = new FormChitiet(selectedKhachhang); // Truyền đối tượng Khachhang

                        // Hiển thị Form Chi Tiết dưới dạng Dialog
                        if (formChiTiet.ShowDialog() == DialogResult.OK)
                        {

                            LoadDanhSachKhachHang();
                        }
                    }
                    catch (InvalidOperationException bllEx)
                    {
                        MessageBox.Show("Lỗi nghiệp vụ khi lấy thông tin chi tiết khách hàng hoặc mở Form: " + bllEx.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (ArgumentException argEx)
                    {
                        MessageBox.Show("Lỗi dữ liệu khi lấy thông tin chi tiết khách hàng: " + argEx.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lấy thông tin chi tiết khách hàng hoặc mở Form: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo một instance mới của Form Chi Tiết ở chế độ Thêm mới (không truyền đối tượng Khachhang)
                FormChitiet formChiTiet = new FormChitiet();

                // Hiển thị Form Chi Tiết dưới dạng Dialog
                if (formChiTiet.ShowDialog() == DialogResult.OK)
                {
                    LoadDanhSachKhachHang();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở Form thêm khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {

        }
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

        private DataTable ReadCSV(string path)
        {
            DataTable dt = new DataTable();

            using (var reader = new StreamReader(path))
            {
                string header = reader.ReadLine();
                if (header == null) return dt;

                var columnNames = header.Split(',');
                foreach (string col in columnNames)
                    dt.Columns.Add(col.Trim());

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    dt.Rows.Add(line.Split(','));
                }
            }

            return dt;
        }
        private DataTable ReadExcel(string path)
        {
            DataTable dt = new DataTable();
            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var ws = package.Workbook.Worksheets[0];
                int colCount = ws.Dimension.End.Column;
                int rowCount = ws.Dimension.End.Row;

                for (int col = 1; col <= colCount; col++)
                    dt.Columns.Add(ws.Cells[1, col].Text);

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
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xlsx;*.xls|CSV Files|*.csv";
            if (ofd.ShowDialog() != DialogResult.OK) return;

            DataTable dt;
            try
            {
                dt = Path.GetExtension(ofd.FileName).ToLower() == ".csv" ? ReadCSV(ofd.FileName) : ReadExcel(ofd.FileName);
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

            var existingMa = khachhangBLL.GetAllMaKH(); // chỉ lấy 1 lần
            var usedMa = new HashSet<string>(existingMa);

            List<Khachhang> list = new List<Khachhang>();

            foreach (DataRow row in dt.Rows)
            {
                string ma;
                if (row.Table.Columns.Contains("Makhachhang") && !string.IsNullOrWhiteSpace(row["Makhachhang"].ToString()))
                {
                    ma = row["Makhachhang"].ToString().Trim();
                    if (usedMa.Contains(ma))
                        ma = khachhangBLL.GenerateNextMaKHInMemory(usedMa);
                }
                else
                {
                    ma = khachhangBLL.GenerateNextMaKHInMemory(usedMa);
                }

                usedMa.Add(ma);

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
                khachhangBLL.ImportKhachhangs(list);
                MessageBox.Show("Import thành công " + list.Count + " dòng.");
                LoadGridKhachhang();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi import: " + ex.Message);
            }
        }

        private void btnThemloaidouong_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            // Mở AddTypeofdrinkForm ở chế độ thêm mới. Form này cũng sẽ tương tác với BLL.
            FormChitiet detailForm = new FormChitiet();
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                LoadDanhSachKhachHang(); // Tải lại danh sách sau khi thêm mới thành công
            }
        }
    }
}