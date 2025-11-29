using CoffeeManagementSystem.BLL;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class DrinkForm : Form
    {
        // Khai báo các đối tượng BLL thay vì DAL
        private LoaidouongBLL _loaidouongBLL;
        private DouongBLL _douongBLL;

        public DrinkForm()
        {
            InitializeComponent();

            // Khởi tạo các đối tượng BLL
            _loaidouongBLL = new LoaidouongBLL();
            _douongBLL = new DouongBLL();

            // Gán sự kiện Load cho Form chính
            this.Load += DrinkForm_Load;
            dgvLoaidouong.AutoGenerateColumns = false;
            dgvLoaidouong.AllowUserToAddRows = false;
            dgvLoaidouong.AllowUserToDeleteRows = false;
            dgvLoaidouong.EditMode = DataGridViewEditMode.EditProgrammatically; // Không cho phép chỉnh sửa trực tiếp trên DGV

            // Gán sự kiện cho các control trên tab Loại đồ uống
            this.txtTimkiemloaidouong.TextChanged += new EventHandler(txtTimkiemloaidouong_TextChanged);
            this.btnThemloaidouong.Click += new EventHandler(btnThem_Click);
            this.dgvLoaidouong.CellClick += new DataGridViewCellEventHandler(dgvLoaidouong_CellClick);
            dgvDouong.AutoGenerateColumns = false;
            dgvDouong.AllowUserToAddRows = false;
            dgvDouong.AllowUserToDeleteRows = false;
            dgvDouong.EditMode = DataGridViewEditMode.EditProgrammatically; // Không cho phép chỉnh sửa trực tiếp trên DGV

            // Gán sự kiện cho các control trên tab Đồ uống
            this.txtTimkiemdouong.TextChanged += new EventHandler(txtTimkiemdouong_TextChanged);
            this.btnThemdouong.Click += new EventHandler(btnAddDouong_Click);
            this.dgvDouong.CellClick += new DataGridViewCellEventHandler(dgvDouong_CellClick);
        }

        // Sự kiện Form Load: Tải dữ liệu khi Form được hiển thị
        private void DrinkForm_Load(object sender, EventArgs e)
        {
            // Tải dữ liệu cho cả hai tab khi Form chính tải
            LoadDanhSachLoaidouong();
            LoadDanhSachDouong();
        }
        //Tải danh sách loại đồ uống và hiển thị lên DataGridView.
        private void LoadDanhSachLoaidouong()
        {
            List<Loaidouong> danhSach = _loaidouongBLL.GetAllLoaidouongs();
            dgvLoaidouong.DataSource = null; // Clear old data
            dgvLoaidouong.DataSource = danhSach;
            dgvLoaidouong.Refresh();
            dgvLoaidouong.ClearSelection(); // Xóa chọn dòng
        }
        //Tải danh sách loại đồ uống đã lọc và hiển thị lên DataGridView.
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
            dgvLoaidouong.ClearSelection(); // Xóa chọn dòng
        }
        //Xử lý sự kiện TextChanged của ô tìm kiếm loại đồ uống.
        private void txtTimkiemloaidouong_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtTimkiemloaidouong.Text.Trim();
            LoadFilteredLoaidouongData(searchTerm);
        }

        //Xử lý sự kiện click nút "Thêm mới" loại đồ uống.
        private void btnThem_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            // Mở AddTypeofdrinkForm ở chế độ thêm mới. Form này cũng sẽ tương tác với BLL.
            AddTypeofdrinkForm detailForm = new AddTypeofdrinkForm();
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                LoadDanhSachLoaidouong(); // Tải lại danh sách sau khi thêm mới thành công
            }
        }

        //Xử lý sự kiện click vào dòng DataGridView loại đồ uống để mở form chi tiết.
        private void dgvLoaidouong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MainForm.PlayClickSound();
            if (e.RowIndex >= 0 && e.RowIndex < dgvLoaidouong.Rows.Count - (dgvLoaidouong.AllowUserToAddRows ? 1 : 0))
            {
                // Lấy đối tượng Loaidouong từ dòng được click
                Loaidouong selectedLoaidouong = dgvLoaidouong.Rows[e.RowIndex].DataBoundItem as Loaidouong;

                if (selectedLoaidouong != null)
                {
                    AddTypeofdrinkForm detailForm = new AddTypeofdrinkForm(selectedLoaidouong.Maloai);
                    if (detailForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadDanhSachLoaidouong(); // Tải lại danh sách sau khi chỉnh sửa/xóa thành công
                    }
                }
            }
        }

        //Tải danh sách đồ uống và hiển thị lên DataGridView.
        private void LoadDanhSachDouong()
        {
            List<Douong> danhSach = _douongBLL.GetAllDouongs();
            dgvDouong.DataSource = null; // Clear old data
            dgvDouong.DataSource = danhSach;
            dgvDouong.Refresh();
            dgvDouong.ClearSelection(); // Xóa chọn dòng
        }
        //Tải danh sách đồ uống đã lọc và hiển thị lên DataGridView.
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
            dgvDouong.ClearSelection(); // Xóa chọn dòng
        }
        //Xử lý sự kiện TextChanged của ô tìm kiếm đồ uống.      
        private void txtTimkiemdouong_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtTimkiemdouong.Text.Trim();
            LoadFilteredDouongData(searchTerm);
        }
        //Xử lý sự kiện click nút "Thêm mới" đồ uống.
        private void btnAddDouong_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            AddDrinkForm detailForm = new AddDrinkForm();
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                LoadDanhSachDouong(); // Tải lại danh sách sau khi thêm mới thành công
            }
        }
        //Xử lý sự kiện click vào dòng DataGridView đồ uống để mở form chi tiết.       
        private void dgvDouong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MainForm.PlayClickSound();
            if (e.RowIndex >= 0 && e.RowIndex < dgvDouong.Rows.Count - (dgvDouong.AllowUserToAddRows ? 1 : 0))
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

      
        private void LoadGridDoUong()
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
        private void LoadGridLoaiDoUong()
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



        private void dgvLoaidouong_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
           private void button2_Click(object sender, EventArgs e)
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

            var existingMa = _douongBLL.GetAllMaDU(); // chỉ lấy 1 lần
            var usedMa = new HashSet<string>(existingMa);

            List<Douong> list = new List<Douong>();

            foreach (DataRow row in dt.Rows)
            {
                string ma;
                if (row.Table.Columns.Contains("Madouong") && !string.IsNullOrWhiteSpace(row["Madouong"].ToString()))
                {
                    ma = row["Madouong"].ToString().Trim();
                    if (usedMa.Contains(ma))
                        ma = _douongBLL.GenerateNextMaDUInMemory(usedMa);
                }
                else
                {
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
    private void button1_Click(object sender, EventArgs e)
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

            var existingMa = _loaidouongBLL.GetAllMaLD(); // chỉ lấy 1 lần
            var usedMa = new HashSet<string>(existingMa);

            List<Loaidouong> list = new List<Loaidouong>();

            foreach (DataRow row in dt.Rows)
            {
                string ma;
                if (row.Table.Columns.Contains("Maloai") && !string.IsNullOrWhiteSpace(row["Maloai"].ToString()))
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
                LoadGridDoUong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi import: " + ex.Message);
            }
        }
    }

}