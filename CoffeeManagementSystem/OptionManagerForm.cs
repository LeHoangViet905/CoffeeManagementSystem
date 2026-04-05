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

        // Biến lưu ID nhóm đang được chọn bên trái
        private int _selectedGroupId = -1;
        private int _selectedDetailId = -1;
        public OptionManagerForm()
        {
            InitializeComponent();
            this.Load += OptionManagerForm_Load;
            dgvNhom.AutoGenerateColumns = false;
            dgvChiTiet.AutoGenerateColumns = false;

            // Gán sự kiện click cho bảng bên trái
            dgvNhom.CellClick += DgvNhom_CellClick;
            cboNhom.SelectedIndexChanged += CboNhom_SelectedIndexChanged;
        }
        private void OptionManagerForm_Load(object sender, EventArgs e)
        {
            LoadGroupList(); // Tải danh sách nhóm khi mở form
            LoadTabCauHinh();
        }

        // --- PHẦN 1: QUẢN LÝ NHÓM (BÊN TRÁI) ---

        private void LoadGroupList()
        {
            List<NhomTuyChon> list = _bll.GetAllGroups();
            dgvNhom.DataSource = list; // Đổ dữ liệu vào bảng

            // dgvNhom.Columns["ChonNhieu"].Visible = false;
        }

        private void btnThemNhom_Click(object sender, EventArgs e)
        {
            string ten = txtTenNhom.Text.Trim();
            if (string.IsNullOrEmpty(ten)) return;

            bool chonNhieu = chkChonNhieu.Checked;

            // 1. Gọi DAL thêm vào DB
            _bll.AddGroup(ten, chonNhieu);

            // 2. Load lại bảng để thấy nhóm mới
            LoadGroupList();

            // 3. Xóa trắng ô nhập
            txtTenNhom.Clear();
            MessageBox.Show("Thêm nhóm thành công!");
        }

        // --- PHẦN 2: TƯƠNG TÁC (CHỌN NHÓM ĐỂ XEM CHI TIẾT) ---

        private void DgvNhom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 1. Kiểm tra dòng hợp lệ (không phải header)
            if (e.RowIndex < 0) return;

            // 2. Lấy đối tượng NhomTuyChon từ dòng được click
            // (Lưu ý: Cách lấy này chỉ đúng nếu bạn gán DataSource là List<NhomTuyChon>)
            var selectedItem = dgvNhom.Rows[e.RowIndex].DataBoundItem as NhomTuyChon;

            if (selectedItem != null)
            {
                // --- A. LƯU TRẠNG THÁI & CẬP NHẬT UI ---
                _selectedGroupId = selectedItem.MaNhom; // Lưu ID nhóm đang chọn
                lblGroupTitle.Text = "CHI TIẾT: " + selectedItem.TenNhom; // Cập nhật tiêu đề bên phải

                // --- B. ĐỔ DỮ LIỆU LÊN CONTROL NHẬP LIỆU (Để sửa/xóa nhóm) ---
                txtTenNhom.Text = selectedItem.TenNhom;
                chkChonNhieu.Checked = selectedItem.ChonNhieu;

                // --- C. TẢI DANH SÁCH CHI TIẾT CON (Sang bảng bên phải) ---
                LoadDetailList(_selectedGroupId);

                // (Tùy chọn) Reset form nhập liệu bên phải để tránh nhầm lẫn
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

        // --- PHẦN 3: QUẢN LÝ CHI TIẾT (BÊN PHẢI) ---

        private void btnThemChiTiet_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem đã chọn nhóm nào chưa
            if (_selectedGroupId == -1)
            {
                MessageBox.Show("Vui lòng chọn một Nhóm bên trái trước!");
                return;
            }

            string tenChiTiet = txtTenChiTiet.Text.Trim();
            decimal giaThem = numGiaThem.Value;

            if (string.IsNullOrEmpty(tenChiTiet)) return;

            // 2. Thêm vào DB (gắn với ID nhóm đang chọn)
            _bll.AddDetail(_selectedGroupId, tenChiTiet, giaThem);

            // 3. Load lại bảng chi tiết
            LoadDetailList(_selectedGroupId);

            txtTenChiTiet.Clear();
            numGiaThem.Value = 0;
        }
        // 1. Hàm Load cho Tab 2 (Gọi khi mở Form hoặc chuyển Tab)
        private void LoadTabCauHinh()
        {
            // A. Load danh sách Nhóm vào ComboBox
            List<NhomTuyChon> listNhom = _bll.GetAllGroups();

            // Tạm tắt sự kiện để tránh kích hoạt lung tung khi đang gán DataSource
            cboNhom.SelectedIndexChanged -= CboNhom_SelectedIndexChanged;

            cboNhom.DataSource = listNhom;
            cboNhom.DisplayMember = "TenNhom";
            cboNhom.ValueMember = "MaNhom";

            // Bật lại sự kiện
            cboNhom.SelectedIndexChanged += CboNhom_SelectedIndexChanged;

            // B. Load TẤT CẢ món ăn
            dgvMon.AutoGenerateColumns = false;
            List<Douong> listMon = _douongBLL.GetAllDouongs();

            bsDouong.DataSource = listMon;
            dgvMon.DataSource = bsDouong;

            // Kích hoạt lần đầu
            if (listNhom.Count > 0)
            {
                cboNhom.SelectedIndex = 0; // Dòng này sẽ tự kích hoạt sự kiện
            }
        }

        // 2. Sự kiện khi chọn Nhóm trong ComboBox
        private void CboNhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Nếu chưa có item hợp lệ thì thôi
            if (cboNhom.SelectedItem == null) return;

            // Lấy object nhóm đang chọn
            var nhom = cboNhom.SelectedItem as NhomTuyChon;
            if (nhom == null) return;

            int maNhom = nhom.MaNhom;   // ✅ lúc này chắc chắn là int

            // Lấy danh sách các món đã gán
            List<string> listDaGann = _bll.GetProductIdsByGroupId(maNhom);

            foreach (DataGridViewRow row in dgvMon.Rows)
            {
                if (row.IsNewRow) continue;

                string maMon = row.Cells["colMa"].Value?.ToString();
                if (string.IsNullOrEmpty(maMon)) continue;

                bool isAssigned = listDaGann.Contains(maMon);
                row.Cells["colChon"].Value = isAssigned;
            }
        }

        // 3. Sự kiện nút LƯU
        private void btnLuuCauHinh_Click(object sender, EventArgs e)
        {
            if (cboNhom.SelectedValue == null) return;
            int maNhom = (int)cboNhom.SelectedValue;

            // Tạo list chứa các món được tích
            List<string> listMaMonDuocChon = new List<string>();

            foreach (DataGridViewRow row in dgvMon.Rows)
            {
                // Kiểm tra ô checkbox có được tích không
                bool isChecked = Convert.ToBoolean(row.Cells["colChon"].Value);

                if (isChecked)
                {
                    string maMon = row.Cells["colMa"].Value.ToString();
                    listMaMonDuocChon.Add(maMon);
                }
            }

            try
            {
                // Gọi BLL lưu xuống DB
                _bll.SaveGroupConfiguration(maNhom, listMaMonDuocChon);
                MessageBox.Show("Đã lưu cấu hình thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
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

                    // Reset phần chi tiết
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
                LoadDetailList(_selectedGroupId); // Tải lại danh sách chi tiết
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
            // 1. Kiểm tra có phải dòng hợp lệ (không phải header) không
            if (e.RowIndex < 0) return;

            // 2. Lấy dòng đang chọn
            DataGridViewRow row = dgvChiTiet.Rows[e.RowIndex];

            // 3. Lấy MaChiTiet và lưu lại
            if (row.Cells["colMaChiTiet"].Value != null)
            {
                if (int.TryParse(row.Cells["colMaChiTiet"].Value.ToString(), out int maChiTiet))
                {
                    // Lưu ID để chuẩn bị cho nút Sửa/Xóa Chi Tiết
                    _selectedDetailId = maChiTiet;
                }
            }

            // 4. Đổ dữ liệu lên TextBox và NumericUpDown
            if (row.Cells["colTenChiTiet"].Value != null)
            {
                txtTenChiTiet.Text = row.Cells["colTenChiTiet"].Value.ToString();
            }

            // Đổ giá thêm lên NumericUpDown (numGiaThem)
            if (row.Cells["colGiaThem"].Value != null && decimal.TryParse(row.Cells["colGiaThem"].Value.ToString(), out decimal giaThem))
            {
                numGiaThem.Value = giaThem;
            }
        }
    }
}
