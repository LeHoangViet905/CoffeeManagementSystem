using CoffeeManagementSystem.BLL;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class EmployerForm : Form
    {
        private NhanvienBLL nhanvienBLL = new NhanvienBLL();
        public EmployerForm()
        {
            InitializeComponent();
            this.Load += EmployerForm_Load;
            dgvNhanvien.AutoGenerateColumns = false;
            dgvNhanvien.AllowUserToAddRows = false;
            dgvNhanvien.AllowUserToDeleteRows = false;
            dgvNhanvien.EditMode = DataGridViewEditMode.EditProgrammatically;

            // Assign events to Employee tab controls
            this.txtSearch.KeyDown += new KeyEventHandler(txtSearch_KeyDown);
            this.txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            this.dgvNhanvien.CellClick += new DataGridViewCellEventHandler(dgvNhanvien_CellClick);
            this.btnThem.Click += new EventHandler(btnThem_Click);
        }
        private void EmployerForm_Load(object sender, EventArgs e)
        {
            LoadDanhSachNhanvien();
        }
        private void LoadDanhSachNhanvien()
        {
            try
            {
                // Gọi BLL để lấy danh sách nhân viên
                List<Nhanvien> danhSach = nhanvienBLL.GetAllNhanviens();
                dgvNhanvien.DataSource = danhSach;
                dgvNhanvien.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //Thêm mới
        private void btnThem_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            FormChiTietNhanvien formChiTiet = new FormChiTietNhanvien();
            if (formChiTiet.ShowDialog() == DialogResult.OK)
            {
                LoadDanhSachNhanvien(); //Tải lại danh sách nhân viên
            }
        }
        //Tìm kiếm
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                string searchTerm = txtSearch.Text.Trim();
                LoadFilteredNhanvienData(searchTerm);
            }
        }
        //Hiển thị kết quả tìm kiếm
        private void LoadFilteredNhanvienData(string searchTerm)
        {
            try
            {
                List<Nhanvien> ketQuaHienThi;
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    // Gọi BLL để lấy tất cả nhân viên
                    ketQuaHienThi = nhanvienBLL.GetAllNhanviens();
                }
                else
                {
                    // Gọi BLL để tìm kiếm nhân viên
                    ketQuaHienThi = nhanvienBLL.SearchNhanviens(searchTerm);
                }

                dgvNhanvien.DataSource = ketQuaHienThi;
                dgvNhanvien.Refresh();

                if (ketQuaHienThi.Count == 0 && !string.IsNullOrWhiteSpace(searchTerm))
                {
                    MessageBox.Show($"Không tìm thấy nhân viên nào phù hợp với từ khóa '{searchTerm}'.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void loadtabPageNhanvien()
        {
            // ... logic tải UserControl FormNhanvien vào TabPage ...
        }
        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Employer_Load(object sender, EventArgs e)
        {

        }

        private void close_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            this.Close();
        }

        private void dgvNhanvien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            MainForm.PlayClickSound();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            LoadFilteredNhanvienData(searchTerm);
        }


        private void dgvNhanvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MainForm.PlayClickSound();
            if (e.RowIndex >= 0 && e.RowIndex < dgvNhanvien.Rows.Count - (dgvNhanvien.AllowUserToAddRows ? 1 : 0))
            {
                Nhanvien selectedNhanvien = dgvNhanvien.Rows[e.RowIndex].DataBoundItem as Nhanvien;

                if (selectedNhanvien != null)
                {
                    try
                    {
                        FormChiTietNhanvien formChiTiet = new FormChiTietNhanvien(selectedNhanvien);

                        // Hiển thị Form Chi Tiết dưới dạng Dialog
                        if (formChiTiet.ShowDialog() == DialogResult.OK)
                        {
                            // Nếu Form Chi Tiết trả về DialogResult.OK (nghĩa là đã lưu thành công)
                            // Tải lại danh sách nhân viên trên Form chính
                            LoadDanhSachNhanvien();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lấy thông tin chi tiết nhân viên hoặc mở Form: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void tabPageNhanvien_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
        }



        private void lblSearch_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
        }
        public void LoadGridNhanVien()
        {
            try
            {
                var list = nhanvienBLL.GetAllNhanviens();
                dgvNhanvien.DataSource = null;
                dgvNhanvien.DataSource = list;
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
        private void button2_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
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

            var existingMa = nhanvienBLL.GetAllMaNV(); // chỉ lấy 1 lần
            var usedMa = new HashSet<string>(existingMa);

            List<Nhanvien> list = new List<Nhanvien>();

            foreach (DataRow row in dt.Rows)
            {
                string ma;
                if (row.Table.Columns.Contains("Manhanvien") && !string.IsNullOrWhiteSpace(row["Manhanvien"].ToString()))
                {
                    ma = row["Manhanvien"].ToString().Trim();
                    if (usedMa.Contains(ma))
                        ma = nhanvienBLL.GenerateNextMaNVInMemory(usedMa);
                }
                else
                {
                    ma = nhanvienBLL.GenerateNextMaNVInMemory(usedMa);
                }

                usedMa.Add(ma);

                Nhanvien n = new Nhanvien
                {
                    Manhanvien = ma,
                    Hoten = row["Hoten"].ToString().Trim(),
                    Ngaysinh = Convert.ToDateTime(row["Ngaysinh"]),
                    Gioitinh = row["Gioitinh"].ToString().Trim(),
                    Diachi = row["Diachi"].ToString().Trim(),
                    Sodienthoai = row["Sodienthoai"].ToString().Trim(),
                    Email = row["Email"].ToString().Trim(),
                    Ngayvaolam = Convert.ToDateTime(row["Ngayvaolam"]),


                };

                list.Add(n);
            }
            if (list.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu hợp lệ để import.");
                return;
            }

            try
            {
                nhanvienBLL.ImportNhanviens(list);
                MessageBox.Show("Import thành công " + list.Count + " dòng.");
                LoadGridNhanVien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi import: " + ex.Message);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Files|*.xlsx;*.xls|CSV Files|*.csv";
            sfd.FileName = "NhanVien_Export";
            if (sfd.ShowDialog() != DialogResult.OK) return;
            try
            {
                //Lấy danh sách nhân viên từ BLL
                var list = nhanvienBLL.GetAllNhanviens();
                nhanvienBLL.ExportNhanvienToCSV(list, sfd.FileName);
                MessageBox.Show("Xuất file thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file: " + ex.Message);
            }
        }
    }
}