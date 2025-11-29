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
        public OptionManagerForm()
        {
            InitializeComponent();
            this.Load += OptionManagerForm_Load;

            // Gán sự kiện click cho bảng bên trái
            dgvNhom.CellClick += DgvNhom_CellClick;
        }
        private void OptionManagerForm_Load(object sender, EventArgs e)
        {
            LoadGroupList(); // Tải danh sách nhóm khi mở form
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
            if (e.RowIndex < 0) return;

            // 1. Lấy ID của nhóm vừa bấm
            var selectedItem = dgvNhom.Rows[e.RowIndex].DataBoundItem as NhomTuyChon;
            if (selectedItem != null)
            {
                _selectedGroupId = selectedItem.MaNhom; // Lưu ID lại
                lblGroupTitle.Text = "CHI TIẾT: " + selectedItem.TenNhom; // Cập nhật tiêu đề bên phải

                // 2. Tải danh sách chi tiết của nhóm này
                LoadDetailList(_selectedGroupId);
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
            cboNhom.DataSource = listNhom;
            cboNhom.DisplayMember = "TenNhom";
            cboNhom.ValueMember = "MaNhom";

            // B. Load TẤT CẢ món ăn vào DataGridView
            dgvMon.AutoGenerateColumns = false; // Quan trọng
            List<Douong> listMon = _douongBLL.GetAllDouongs();

            // 1. Gán nguồn dữ liệu vào BindingSource
            bsDouong.DataSource = listMon;

            // 2. Gán BindingSource vào DataGridView
            dgvMon.DataSource = bsDouong;

            // Gán sự kiện khi chọn ComboBox (để check các ô tương ứng)
            cboNhom.SelectedIndexChanged += CboNhom_SelectedIndexChanged;

            // Kích hoạt lần đầu
            if (listNhom.Count > 0) CboNhom_SelectedIndexChanged(null, null);
        }

        // 2. Sự kiện khi chọn Nhóm trong ComboBox
        private void CboNhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboNhom.SelectedValue == null) return;

            // Lấy ID nhóm đang chọn (ví dụ: Topping)
            int maNhom = (int)cboNhom.SelectedValue; // Lưu ý ép kiểu int nếu ValueMember là int

            // Lấy danh sách các món ĐÃ ĐƯỢC GÁN cho nhóm này
            List<string> listDaGann = _bll.GetProductIdsByGroupId(maNhom);

            // Duyệt qua DataGridView để Tích (Check) hoặc Bỏ tích
            foreach (DataGridViewRow row in dgvMon.Rows)
            {
                // Lấy mã món của dòng này
                // (Đảm bảo bạn đã map DataPropertyName="Madouong" cho cột colMa)
                string maMon = row.Cells["colMa"].Value.ToString();

                // Kiểm tra xem món này có trong danh sách đã gán không
                bool isAssigned = listDaGann.Contains(maMon);

                // Set giá trị cho ô Checkbox
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

        }
    }
}
