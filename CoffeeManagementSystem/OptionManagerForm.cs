using CoffeeManagementSystem.bll;
using CoffeeManagementSystem.BLL;
using CoffeeManagementSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class OptionManagerForm : Form
    {
        // Khai báo BLL
        private TuyChonBLL _bll = new TuyChonBLL();
        private DouongBLL _douongBLL = new DouongBLL();

        private int _selectedGroupId = -1;
        private int _selectedDetailId = -1;
        public OptionManagerForm()
        {
            InitializeComponent();
            this.Load += OptionManagerForm_Load;
            dgvNhom.AutoGenerateColumns = false;
            dgvChiTiet.AutoGenerateColumns = false;

            dgvNhom.CellClick += DgvNhom_CellClick;
            cboNhom.SelectedIndexChanged += CboNhom_SelectedIndexChanged;
        }
        private void OptionManagerForm_Load(object sender, EventArgs e)
        {
            LoadGroupList(); 
            LoadTabCauHinh();
        }


        private void LoadGroupList()
        {
            List<NhomTuyChon> list = _bll.GetAllGroups();
            dgvNhom.DataSource = list; 

        }

        private void btnThemNhom_Click(object sender, EventArgs e)
        {
            string ten = txtTenNhom.Text.Trim();
            if (string.IsNullOrEmpty(ten)) return;

            bool chonNhieu = chkChonNhieu.Checked;

            _bll.AddGroup(ten, chonNhieu);

            LoadGroupList();

            txtTenNhom.Clear();
            MessageBox.Show("Thêm nhóm thành công!");
        }


        private void DgvNhom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var selectedItem = dgvNhom.Rows[e.RowIndex].DataBoundItem as NhomTuyChon;

            if (selectedItem != null)
            {
                _selectedGroupId = selectedItem.MaNhom; 
                lblGroupTitle.Text = "CHI TIẾT: " + selectedItem.TenNhom; 

                txtTenNhom.Text = selectedItem.TenNhom;
                chkChonNhieu.Checked = selectedItem.ChonNhieu;

                LoadDetailList(_selectedGroupId);

                _selectedDetailId = -1;
                txtTenChiTiet.Clear();
                numGiaThem.Value = 0;
            }
        }

        private void LoadDetailList(int groupId)
        {
            List<ChiTietTuyChon> list = _bll.GetDetailsByGroupId(groupId);
            dgvChiTiet.DataSource = list;
        }


        private void btnThemChiTiet_Click(object sender, EventArgs e)
        {
            if (_selectedGroupId == -1)
            {
                MessageBox.Show("Vui lòng chọn một Nhóm bên trái trước!");
                return;
            }

            string tenChiTiet = txtTenChiTiet.Text.Trim();
            decimal giaThem = numGiaThem.Value;

            if (string.IsNullOrEmpty(tenChiTiet)) return;

            _bll.AddDetail(_selectedGroupId, tenChiTiet, giaThem);

            LoadDetailList(_selectedGroupId);

            txtTenChiTiet.Clear();
            numGiaThem.Value = 0;
        }
        // 1. Hàm Load Tab (Khởi tạo)
        private void LoadTabCauHinh()
        {
            // A. Load ComboBox Nhóm
            List<NhomTuyChon> listNhom = _bll.GetAllGroups();

            // Thêm mục "Tất cả" vào đầu
            NhomTuyChon tatCa = new NhomTuyChon { MaNhom = -1, TenNhom = "--- Xem Tất Cả & Cấu Hình ---" };
            listNhom.Insert(0, tatCa);

            // Tắt sự kiện để tránh load data 2 lần
            cboNhom.SelectedIndexChanged -= CboNhom_SelectedIndexChanged;

            cboNhom.DataSource = listNhom;
            cboNhom.DisplayMember = "TenNhom";
            cboNhom.ValueMember = "MaNhom";

            cboNhom.SelectedIndexChanged += CboNhom_SelectedIndexChanged;

            // B. Load DataGridView (Lần đầu tiên)
            // Gọi hàm LoadDataToGrid để lấy dữ liệu kèm cột "Các nhóm đã có"
            LoadDataToGrid();

            // Mặc định chọn mục đầu tiên
            if (listNhom.Count > 0)
            {
                cboNhom.SelectedIndex = 0;
                // Tự kích hoạt sự kiện change để update giao diện
                CboNhom_SelectedIndexChanged(null, null);
            }
        }

        // Hàm phụ trợ: Load dữ liệu mới nhất từ DB lên lưới
        private void LoadDataToGrid()
        {
            dgvMon.AutoGenerateColumns = false;

            // Lấy DataTable chứa thông tin món + cột "CacNhomDaCo" (GROUP_CONCAT)
            // (Hàm này bạn đã thêm vào BLL/DAL ở bước trước)
            DataTable dt = _bll.GetProductsWithConfig();

            bsDouong.DataSource = dt;
            dgvMon.DataSource = bsDouong;
        }

        // 2. Sự kiện Chọn Nhóm (Xử lý hiển thị Checkbox)
        private void CboNhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboNhom.SelectedItem == null) return;

            // Lấy ID nhóm đang chọn
            // (Dùng SelectedValue an toàn hơn SelectedItem nếu bạn bind đúng ValueMember)
            int maNhom = -1;
            if (cboNhom.SelectedValue != null && int.TryParse(cboNhom.SelectedValue.ToString(), out int id))
            {
                maNhom = id;
            }

            if (maNhom == -1) // CHẾ ĐỘ: XEM TẤT CẢ
            {
                // Khóa nút lưu
                btnLuuCauHinh.Enabled = false;

                // Duyệt qua grid để bỏ tích và khóa checkbox
                foreach (DataGridViewRow row in dgvMon.Rows)
                {
                    if (row.IsNewRow) continue;

                    // Bỏ tích
                    row.Cells["colChon"].Value = false;

                    // Khóa ô checkbox (ReadOnly) và làm mờ
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["colChon"];
                    chk.ReadOnly = true;
                    chk.Style.BackColor = Color.LightGray;
                    chk.Style.ForeColor = Color.DarkGray;
                }
            }
            else // CHẾ ĐỘ: CẤU HÌNH CHO 1 NHÓM
            {
                // Mở nút lưu
                btnLuuCauHinh.Enabled = true;

                // Lấy danh sách các món ĐÃ GÁN cho nhóm này
                List<string> listDaGan = _bll.GetProductIdsByGroupId(maNhom);

                foreach (DataGridViewRow row in dgvMon.Rows)
                {
                    if (row.IsNewRow) continue;

                    // Mở khóa ô checkbox
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["colChon"];
                    chk.ReadOnly = false;
                    chk.Style.BackColor = Color.White; // Hoặc màu mặc định của bạn

                    // Kiểm tra và tích chọn
                    string maMon = row.Cells["colMa"].Value?.ToString();
                    if (!string.IsNullOrEmpty(maMon))
                    {
                        bool isAssigned = listDaGan.Contains(maMon);
                        row.Cells["colChon"].Value = isAssigned;
                    }
                }
            }
        }

        // 3. Sự kiện Lưu (Cập nhật DB và Refresh giao diện)
        private void btnLuuCauHinh_Click(object sender, EventArgs e)
        {
            if (cboNhom.SelectedValue == null) return;

            int maNhom = -1;
            if (cboNhom.SelectedValue != null && int.TryParse(cboNhom.SelectedValue.ToString(), out int id))
            {
                maNhom = id;
            }

            if (maNhom == -1) return; // Không làm gì nếu đang ở chế độ Xem

            List<string> listMaMonDuocChon = new List<string>();

            // Lấy danh sách các món được tích
            foreach (DataGridViewRow row in dgvMon.Rows)
            {
                if (row.IsNewRow) continue;

                // Lấy giá trị checkbox an toàn
                bool isChecked = false;
                if (row.Cells["colChon"].Value != null)
                {
                    bool.TryParse(row.Cells["colChon"].Value.ToString(), out isChecked);
                }

                if (isChecked)
                {
                    string maMon = row.Cells["colMa"].Value?.ToString();
                    if (!string.IsNullOrEmpty(maMon))
                    {
                        listMaMonDuocChon.Add(maMon);
                    }
                }
            }

            try
            {
                // A. Lưu xuống DB
                _bll.SaveGroupConfiguration(maNhom, listMaMonDuocChon);
                MessageBox.Show("Đã lưu cấu hình thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // B. QUAN TRỌNG: Load lại dữ liệu để cập nhật cột "Các nhóm đã có"
                LoadDataToGrid();

                // C. Gọi lại sự kiện Change để giữ nguyên trạng thái tích chọn của nhóm hiện tại
                // (Nếu không gọi lại, các checkbox có thể bị reset hoặc sai lệch sau khi reload grid)
                CboNhom_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                bsDouong.Filter = null;
            }
            else
            {
                try
                {
                    string filterExpression = string.Format("Tendouong LIKE '*{0}*'", keyword);

                    bsDouong.Filter = filterExpression;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi cú pháp tìm kiếm: " + ex.Message);
                    bsDouong.Filter = null;
                }
            }
        }

        private void btnSuaNhom_Click(object sender, EventArgs e)
        {
            if (_selectedGroupId == -1) { MessageBox.Show("Vui lòng chọn nhóm cần sửa!"); return; }

            try
            {
                string tenMoi = txtTenNhom.Text.Trim();
                bool chonNhieu = chkChonNhieu.Checked;

                _bll.UpdateGroup(_selectedGroupId, tenMoi, chonNhieu);

                MessageBox.Show("Cập nhật nhóm thành công!");
                LoadGroupList(); // Tải lại danh sách
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnXoaNhom_Click(object sender, EventArgs e)
        {
            if (_selectedGroupId == -1) { MessageBox.Show("Vui lòng chọn nhóm cần xóa!"); return; }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhóm này? Tất cả chi tiết bên trong cũng sẽ bị xóa!", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    _bll.DeleteGroup(_selectedGroupId);

                    MessageBox.Show("Đã xóa nhóm!");
                    LoadGroupList();

                    dgvChiTiet.DataSource = null;
                    _selectedGroupId = -1;
                    txtTenNhom.Clear();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void btnSuaChiTiet_Click(object sender, EventArgs e)
        {
            if (_selectedDetailId == -1) { MessageBox.Show("Vui lòng chọn chi tiết cần sửa!"); return; }

            try
            {
                string tenMoi = txtTenChiTiet.Text.Trim();
                decimal giaMoi = numGiaThem.Value;

                _bll.UpdateDetail(_selectedDetailId, tenMoi, giaMoi);

                MessageBox.Show("Cập nhật chi tiết thành công!");
                LoadDetailList(_selectedGroupId); 
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnXoaChiTiet_Click(object sender, EventArgs e)
        {
            if (_selectedDetailId == -1) { MessageBox.Show("Vui lòng chọn chi tiết cần xóa!"); return; }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa chi tiết này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    _bll.DeleteDetail(_selectedDetailId);

                    LoadDetailList(_selectedGroupId);
                    _selectedDetailId = -1;
                    txtTenChiTiet.Clear();
                    numGiaThem.Value = 0;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void dgvChiTiet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvChiTiet.Rows[e.RowIndex];

            if (row.Cells["colMaChiTiet"].Value != null)
            {
                if (int.TryParse(row.Cells["colMaChiTiet"].Value.ToString(), out int maChiTiet))
                {
                    _selectedDetailId = maChiTiet;
                }
            }

            if (row.Cells["colTenChiTiet"].Value != null)
            {
                txtTenChiTiet.Text = row.Cells["colTenChiTiet"].Value.ToString();
            }

            if (row.Cells["colGiaThem"].Value != null && decimal.TryParse(row.Cells["colGiaThem"].Value.ToString(), out decimal giaThem))
            {
                numGiaThem.Value = giaThem;
            }
        }
    }
}
