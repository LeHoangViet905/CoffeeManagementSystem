using CoffeeManagementSystem.BLL;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    /// <summary>
    /// DrinkForm (Form quản lý ĐỒ UỐNG & LOẠI ĐỒ UỐNG)
    /// ------------------------------------------------
    /// - Đây là tầng UI, giao tiếp trực tiếp với người dùng.
    /// - KHÔNG làm việc trực tiếp với DAL mà thông qua:
    ///     + LoaidouongBLL  (quản lý bảng Loaidouong)
    ///     + DouongBLL      (quản lý bảng Douong)
    ///
    /// Luồng chính:
    ///   + Khi Form Load → gọi LoadDanhSachLoaidouong(), LoadDanhSachDouong().
    ///   + Khi gõ tìm kiếm → gọi BLL.Search... để lọc.
    ///   + Khi bấm Thêm/Sửa → mở form chi tiết (AddTypeofdrinkForm, AddDrinkForm),
    ///     các form đó lại gọi BLL để thêm/sửa/xóa trong CSDL.
    ///
    /// Ngoài ra Form còn hỗ trợ:
    ///   - Import danh sách đồ uống / loại đồ uống từ Excel/CSV.
    ///   - Tự sinh mã nếu trùng hoặc thiếu mã trong file Excel.
    /// </summary>
    public partial class DrinkForm : Form
    {
        // Khai báo các đối tượng BLL thay vì DAL
        private LoaidouongBLL _loaidouongBLL;
        private DouongBLL _douongBLL;

        /// <summary>
        /// Constructor DrinkForm:
        /// - Khởi tạo các BLL.
        /// - Gắn các event handler cho Form, DataGridView, TextBox, Button.
        /// </summary>
        public DrinkForm()
        {
            InitializeComponent();

            // Khởi tạo các đối tượng BLL
            _loaidouongBLL = new LoaidouongBLL();
            _douongBLL = new DouongBLL();

            // Gán sự kiện Load cho Form chính
            this.Load += DrinkForm_Load;

            // Cấu hình DataGridView LOẠI ĐỒ UỐNG
            dgvLoaidouong.AutoGenerateColumns = false;
            dgvLoaidouong.AllowUserToAddRows = false;
            dgvLoaidouong.AllowUserToDeleteRows = false;
            // Không cho phép chỉnh sửa trực tiếp trên DGV, chỉ sửa qua form chi tiết
            dgvLoaidouong.EditMode = DataGridViewEditMode.EditProgrammatically;

            // Gán sự kiện cho các control trên tab Loại đồ uống
            this.txtTimkiemloaidouong.TextChanged += new EventHandler(txtTimkiemloaidouong_TextChanged);
            this.btnThemloaidouong.Click += new EventHandler(btnThem_Click);
            this.dgvLoaidouong.CellClick += new DataGridViewCellEventHandler(dgvLoaidouong_CellClick);

            // Cấu hình DataGridView ĐỒ UỐNG
            dgvDouong.AutoGenerateColumns = false;
            dgvDouong.AllowUserToAddRows = false;
            dgvDouong.AllowUserToDeleteRows = false;
            dgvDouong.EditMode = DataGridViewEditMode.EditProgrammatically;

            // Gán sự kiện cho các control trên tab Đồ uống
            this.txtTimkiemdouong.TextChanged += new EventHandler(txtTimkiemdouong_TextChanged);
            this.btnThemdouong.Click += new EventHandler(btnAddDouong_Click);
            this.dgvDouong.CellClick += new DataGridViewCellEventHandler(dgvDouong_CellClick);
        }

        /// <summary>
        /// Sự kiện Form Load:
        /// - Khi DrinkForm hiển thị lần đầu → load dữ liệu cho cả 2 tab.
        /// </summary>
        private void DrinkForm_Load(object sender, EventArgs e)
        {
            // Tải dữ liệu cho cả hai tab khi Form chính tải
            LoadDanhSachLoaidouong();
            LoadDanhSachDouong();
        }

        /// <summary>
        /// Tải danh sách LOẠI ĐỒ UỐNG & hiển thị lên dgvLoaidouong.
        /// Gọi BLL: LoaidouongBLL.GetAllLoaidouongs()
        /// </summary>
        private void LoadDanhSachLoaidouong()
        {
            List<Loaidouong> danhSach = _loaidouongBLL.GetAllLoaidouongs();

            dgvLoaidouong.DataSource = null;   // Clear old data
            dgvLoaidouong.DataSource = danhSach;
            dgvLoaidouong.Refresh();
            dgvLoaidouong.ClearSelection();    // Xóa chọn dòng để UI sạch hơn
        }

        /// <summary>
        /// Tải danh sách LOẠI ĐỒ UỐNG đã lọc theo searchTerm.
        /// - Nếu searchTerm rỗng → lấy toàn bộ.
        /// - Nếu có chữ → gọi BLL.SearchLoaidouongs(searchTerm).
        /// </summary>
        private void LoadFilteredLoaidouongData(string searchTerm)
        {
            List<Loaidouong> ketQuaHienThi;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ketQuaHienThi = _loaidouongBLL.GetAllLoaidouongs();
            }
            else
            {
                ketQuaHienThi = _loaidouongBLL.SearchLoaidouongs(searchTerm);
            }

            dgvLoaidouong.DataSource = null;
            dgvLoaidouong.DataSource = ketQuaHienThi;
            dgvLoaidouong.Refresh();
            dgvLoaidouong.ClearSelection();
        }

        /// <summary>
        /// Event TextChanged của ô tìm kiếm LOẠI ĐỒ UỐNG.
        /// - Mỗi lần người dùng gõ → filter lại danh sách.
        /// </summary>
        private void txtTimkiemloaidouong_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtTimkiemloaidouong.Text.Trim();
            LoadFilteredLoaidouongData(searchTerm);
        }

        /// <summary>
        /// Event click nút "Thêm mới" LOẠI ĐỒ UỐNG.
        /// - Mở AddTypeofdrinkForm ở chế độ thêm mới.
        /// - Nếu form chi tiết trả về OK → reload lại danh sách.
        /// </summary>
        private void btnThem_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();

            // Form thêm mới Loại đồ uống (tự làm việc với BLL bên trong)
            AddTypeofdrinkForm detailForm = new AddTypeofdrinkForm();
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                LoadDanhSachLoaidouong(); // Tải lại danh sách sau khi thêm mới thành công
            }
        }

        /// <summary>
        /// Event click vào 1 dòng trong dgvLoaidouong:
        /// - Lấy Loaidouong tương ứng (DataBoundItem).
        /// - Mở AddTypeofdrinkForm ở chế độ CHỈNH SỬA theo Maloai.
        /// - Nếu chỉnh sửa/xóa thành công → load lại danh sách.
        /// </summary>
        private void dgvLoaidouong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MainForm.PlayClickSound();

            if (e.RowIndex >= 0 &&
                e.RowIndex < dgvLoaidouong.Rows.Count - (dgvLoaidouong.AllowUserToAddRows ? 1 : 0))
            {
                // Lấy đối tượng Loaidouong từ dòng được click
                Loaidouong selectedLoaidouong = dgvLoaidouong.Rows[e.RowIndex].DataBoundItem as Loaidouong;

                if (selectedLoaidouong != null)
                {
                    // Mở form chi tiết với mã loại hiện tại → chế độ EDIT
                    AddTypeofdrinkForm detailForm = new AddTypeofdrinkForm(selectedLoaidouong.Maloai);
                    if (detailForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadDanhSachLoaidouong(); // Tải lại danh sách sau khi chỉnh sửa/xóa thành công
                    }
                }
            }
        }

        /// <summary>
        /// Tải danh sách ĐỒ UỐNG & hiển thị lên dgvDouong.
        /// Gọi BLL: DouongBLL.GetAllDouongs()
        /// </summary>
        private void LoadDanhSachDouong()
        {
            List<Douong> danhSach = _douongBLL.GetAllDouongs();

            dgvDouong.DataSource = null;
            dgvDouong.DataSource = danhSach;
            dgvDouong.Refresh();
            dgvDouong.ClearSelection();
        }

        /// <summary>
        /// Tải danh sách ĐỒ UỐNG đã lọc theo searchTerm.
        /// - Nếu searchTerm rỗng → lấy toàn bộ.
        /// - Nếu có chữ → gọi BLL.SearchDouongs(searchTerm).
        /// </summary>
        private void LoadFilteredDouongData(string searchTerm)
        {
            List<Douong> ketQuaHienThi;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ketQuaHienThi = _douongBLL.GetAllDouongs();
            }
            else
            {
                ketQuaHienThi = _douongBLL.SearchDouongs(searchTerm);
            }

            dgvDouong.DataSource = null;
            dgvDouong.DataSource = ketQuaHienThi;
            dgvDouong.Refresh();
            dgvDouong.ClearSelection();
        }

        /// <summary>
        /// Event TextChanged của ô tìm kiếm ĐỒ UỐNG.
        /// - Gõ tới đâu lọc tới đó.
        /// </summary>
        private void txtTimkiemdouong_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtTimkiemdouong.Text.Trim();
            LoadFilteredDouongData(searchTerm);
        }

        /// <summary>
        /// Event click nút "Thêm mới" ĐỒ UỐNG.
        /// - Mở AddDrinkForm ở chế độ thêm mới.
        /// - Nếu OK → load lại danh sách đồ uống.
        /// </summary>
        private void btnAddDouong_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();

            AddDrinkForm detailForm = new AddDrinkForm();
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                LoadDanhSachDouong(); // Tải lại danh sách sau khi thêm mới thành công
            }
        }

        /// <summary>
        /// Event click dòng trong dgvDouong:
        /// - Lấy Douong tương ứng.
        /// - Mở AddDrinkForm theo Madouong (chế độ EDIT).
        /// - Nếu chỉnh sửa/xóa thành công → load lại danh sách.
        /// </summary>
        private void dgvDouong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MainForm.PlayClickSound();

            if (e.RowIndex >= 0 &&
                e.RowIndex < dgvDouong.Rows.Count - (dgvDouong.AllowUserToAddRows ? 1 : 0))
            {
                // Lấy đối tượng Douong từ dòng được click
                Douong selectedDouong = dgvDouong.Rows[e.RowIndex].DataBoundItem as Douong;

                if (selectedDouong != null)
                {
                    AddDrinkForm detailForm = new AddDrinkForm(selectedDouong.Madouong);
                    if (detailForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadDanhSachDouong();
                    }
                }
            }
        }

        /// <summary>
        /// Hàm helper load lại GRID ĐỒ UỐNG từ BLL.
        /// - Dùng lại ở nhiều chỗ (sau import, sau chỉnh sửa...).
        /// </summary>
        public void LoadGridDoUong()
        {
            try
            {
                var list = _douongBLL.GetAllDouongs();
                dgvDouong.DataSource = null;
                dgvDouong.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load dữ liệu: " + ex.Message);
            }
        }

        /// <summary>
        /// Hàm helper load lại GRID LOẠI ĐỒ UỐNG từ BLL.
        /// </summary>
        public void LoadGridLoaiDoUong()
        {
            try
            {
                var list = _loaidouongBLL.GetAllLoaidouongs();
                dgvLoaidouong.DataSource = null;
                dgvLoaidouong.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load dữ liệu: " + ex.Message);
            }
        }

        /// <summary>
        /// Đọc file CSV → đưa vào DataTable.
        /// - Dòng đầu: header → tên cột.
        /// - Các dòng sau: dữ liệu.
        /// - Dùng chung cho import Đồ uống / Loại đồ uống.
        /// </summary>
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

        /// <summary>
        /// Đọc file Excel (.xlsx/.xls) → DataTable.
        /// - Dòng 1: header (tên cột).
        /// - Từ dòng 2 trở đi: dữ liệu.
        /// - Dùng thư viện EPPlus (OfficeOpenXml).
        /// </summary>
        private DataTable ReadExcel(string path)
        {
            DataTable dt = new DataTable();

            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var ws = package.Workbook.Worksheets[0];
                int colCount = ws.Dimension.End.Column;
                int rowCount = ws.Dimension.End.Row;

                // Tạo cột từ dòng tiêu đề
                for (int col = 1; col <= colCount; col++)
                    dt.Columns.Add(ws.Cells[1, col].Text);

                // Đọc từng dòng dữ liệu
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

        private void dgvLoaidouong_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Hiện tại chưa dùng – để sẵn nếu sau này cần xử lý click nội dung ô.
        }

        /// <summary>
        /// Nút import ĐỒ UỐNG từ file (button2_Click_1):
        /// - Chọn file Excel/CSV.
        /// - Đọc thành DataTable (ReadCSV/ReadExcel).
        /// - Sinh mã Madouong nếu trùng hoặc thiếu.
        /// - Gọi DouongBLL.ImportDouongs(list) để lưu xuống DB.
        /// - Sau cùng: LoadGridDoUong() để refresh UI.
        /// </summary>
        private void button2_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xlsx;*.xls|CSV Files|*.csv";
            if (ofd.ShowDialog() != DialogResult.OK) return;

            DataTable dt;
            try
            {
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

            // Lấy sẵn tất cả mã đồ uống hiện có để tránh trùng
            var existingMa = _douongBLL.GetAllMaDU();
            var usedMa = new HashSet<string>(existingMa);

            List<Douong> list = new List<Douong>();

            foreach (DataRow row in dt.Rows)
            {
                string ma;

                // Nếu file có cột Madouong và không trống → ưu tiên dùng, nhưng phải tránh trùng
                if (row.Table.Columns.Contains("Madouong") &&
                    !string.IsNullOrWhiteSpace(row["Madouong"].ToString()))
                {
                    ma = row["Madouong"].ToString().Trim();
                    if (usedMa.Contains(ma))
                        ma = _douongBLL.GenerateNextMaDUInMemory(usedMa);
                }
                else
                {
                    // Nếu không có mã, sinh mã mới
                    ma = _douongBLL.GenerateNextMaDUInMemory(usedMa);
                }

                usedMa.Add(ma);

                Douong d = new Douong
                {
                    Madouong = ma,
                    Tendouong = row["Tendouong"].ToString().Trim(),
                    Maloai = row["Maloai"].ToString().Trim(),
                    Mota = row.Table.Columns.Contains("Mota") ? row["Mota"].ToString() : null,
                    Hinhanh = row.Table.Columns.Contains("Hinhanh") ? row["Hinhanh"].ToString() : null
                };

                list.Add(d);
            }

            if (list.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu hợp lệ để import.");
                return;
            }

            try
            {
                _douongBLL.ImportDouongs(list);
                MessageBox.Show("Import thành công " + list.Count + " dòng.");
                LoadGridDoUong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi import: " + ex.Message);
            }
        }

        private void btnThemloaidouong_Click(object sender, EventArgs e)
        {
            // Event này đang không dùng (vì đã dùng btnThem_Click ở trên).
            // Có thể xoá hoặc map lại để tránh trùng logic.
        }

        /// <summary>
        /// Nút import LOẠI ĐỒ UỐNG từ file (button3_Click):
        /// - Cách làm tương tự import Đồ uống nhưng cho bảng Loaidouong.
        /// - Sinh mã Maloai nếu trùng/thiếu.
        /// - Gọi LoaidouongBLL.ImportLoaidouongs(list) để lưu DB.
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xlsx;*.xls|CSV Files|*.csv";
            if (ofd.ShowDialog() != DialogResult.OK) return;

            DataTable dt;
            try
            {
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

            // Lấy sẵn danh sách mã loại hiện có
            var existingMa = _loaidouongBLL.GetAllMaLD();
            var usedMa = new HashSet<string>(existingMa);

            List<Loaidouong> list = new List<Loaidouong>();

            foreach (DataRow row in dt.Rows)
            {
                string ma;

                if (row.Table.Columns.Contains("Maloai") &&
                    !string.IsNullOrWhiteSpace(row["Maloai"].ToString()))
                {
                    ma = row["Maloai"].ToString().Trim();
                    if (usedMa.Contains(ma))
                        ma = _loaidouongBLL.GenerateNextMaLDInMemory(usedMa);
                }
                else
                {
                    ma = _loaidouongBLL.GenerateNextMaLDInMemory(usedMa);
                }

                usedMa.Add(ma);

                Loaidouong l = new Loaidouong
                {
                    Maloai = ma,
                    Tenloai = row["Tenloai"].ToString().Trim()
                };

                list.Add(l);
            }

            if (list.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu hợp lệ để import.");
                return;
            }

            try
            {
                _loaidouongBLL.ImportLoaidouongs(list);
                MessageBox.Show("Import thành công " + list.Count + " dòng.");
                // Có thể là LoadGridLoaiDoUong(), nhưng code gốc gọi LoadGridDoUong()
                LoadGridDoUong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi import: " + ex.Message);
            }
        }
    }
}
