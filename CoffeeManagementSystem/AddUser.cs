using CoffeeManagementSystem.BLL;
using System;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    /// <summary>
    /// Form chi tiết nhân viên: dùng để thêm mới / chỉnh sửa / xóa một nhân viên.
    /// </summary>
    public partial class FormChiTietNhanvien : Form
    {
        // BLL xử lý nghiệp vụ cho bảng Nhanvien
        private NhanvienBLL nhanvienBLL;

        // Nhân viên hiện đang được chỉnh sửa/xóa (null nếu đang thêm mới)
        private Nhanvien currentNhanvien;

        /// <summary>
        /// Constructor cho CHẾ ĐỘ THÊM MỚI nhân viên.
        /// </summary>
        public FormChiTietNhanvien()
        {
            InitializeComponent();
            nhanvienBLL = new NhanvienBLL();
            this.Text = "Thêm Nhân Viên Mới";

            // Hiển thị các nút nhưng chỉ bật nút Lưu (thêm mới)
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;

            btnSave.Enabled = true;
            btnUpdate.Enabled = false; // Không dùng cập nhật khi thêm mới
            btnDelete.Enabled = false; // Không dùng xóa khi thêm mới

            // Tự sinh mã nhân viên mới từ BLL
            txtMaNV.Text = nhanvienBLL.GenerateNextMaNV();
            txtMaNV.Enabled = false; // Không cho sửa mã NV

            // Khởi tạo danh sách lựa chọn giới tính
            InitializeGioitinhComboBox();
        }

        /// <summary>
        /// Constructor cho CHẾ ĐỘ CẬP NHẬT (sửa/xóa) một nhân viên.
        /// </summary>
        /// <param name="nhanvienToEdit">Đối tượng nhân viên cần chỉnh sửa.</param>
        public FormChiTietNhanvien(Nhanvien nhanvienToEdit)
        {
            InitializeComponent();
            nhanvienBLL = new NhanvienBLL();
            this.Text = "Cập Nhật Thông Tin Nhân Viên";
            currentNhanvien = nhanvienToEdit; // Lưu lại nhân viên đang chỉnh sửa

            // Hiển thị và bật cả 3 nút trong chế độ chỉnh sửa
            btnSave.Visible = true;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;

            btnSave.Enabled = false;  // Không thêm mới trong form sửa
            btnUpdate.Enabled = true; // Cho phép cập nhật
            btnDelete.Enabled = true; // Cho phép xóa

            // Mã NV là khóa → không cho chỉnh sửa
            txtMaNV.Enabled = false;

            // Khởi tạo ComboBox giới tính
            InitializeGioitinhComboBox();

            // Đổ dữ liệu nhân viên lên các control
            DisplayNhanvienInfo();
        }

        /// <summary>
        /// Khởi tạo các lựa chọn cho ComboBox Giới tính.
        /// </summary>
        private void InitializeGioitinhComboBox()
        {
            cbGioiTinh.Items.Add("Nam");
            cbGioiTinh.Items.Add("Nữ");
            cbGioiTinh.Items.Add("Khác");
            cbGioiTinh.SelectedIndex = 0; // Mặc định chọn "Nam"
        }

        /// <summary>
        /// Hiển thị thông tin nhân viên hiện tại lên các control (dùng khi chỉnh sửa).
        /// </summary>
        private void DisplayNhanvienInfo()
        {
            if (currentNhanvien != null)
            {
                txtMaNV.Text = currentNhanvien.Manhanvien;
                txtHoTen.Text = currentNhanvien.Hoten;
                dateTimePickerNgaySinh.Value = currentNhanvien.Ngaysinh;
                cbGioiTinh.SelectedItem = currentNhanvien.Gioitinh; // Chọn lại giới tính trong ComboBox
                txtDiaChi.Text = currentNhanvien.Diachi;
                txtSDT.Text = currentNhanvien.Sodienthoai;
                txtEmail.Text = currentNhanvien.Email;
                dateTimePickerNgayVaoLam.Value = currentNhanvien.Ngayvaolam;
            }
        }

        /// <summary>
        /// Đọc dữ liệu từ các control trên form và tạo/cập nhật đối tượng Nhanvien.
        /// </summary>
        /// <returns>Đối tượng Nhanvien đã gán đầy đủ dữ liệu.</returns>
        private Nhanvien GetNhanvienInfoFromControls()
        {
            // Nếu đang chỉnh sửa → dùng lại currentNhanvien
            // Nếu đang thêm mới → tạo đối tượng mới
            Nhanvien nhanvien = currentNhanvien ?? new Nhanvien();

            // Chỉ gán mã NV khi là thêm mới
            if (currentNhanvien == null)
            {
                nhanvien.Manhanvien = txtMaNV.Text.Trim();
            }

            nhanvien.Hoten = txtHoTen.Text.Trim();
            nhanvien.Ngaysinh = dateTimePickerNgaySinh.Value;
            nhanvien.Gioitinh = cbGioiTinh.SelectedItem?.ToString();
            nhanvien.Diachi = txtDiaChi.Text.Trim();

            // Xử lý các trường có thể để trống (nullable)
            string sdt = txtSDT.Text.Trim();
            nhanvien.Sodienthoai = string.IsNullOrEmpty(sdt) ? null : sdt;

            string email = txtEmail.Text.Trim();
            nhanvien.Email = string.IsNullOrEmpty(email) ? null : email;

            nhanvien.Ngayvaolam = dateTimePickerNgayVaoLam.Value;

            return nhanvien;
        }

        /// <summary>
        /// Nút Lưu – dùng cho CHẾ ĐỘ THÊM MỚI nhân viên.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();

            Nhanvien newNhanvien = GetNhanvienInfoFromControls();

            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrEmpty(newNhanvien.Manhanvien)
                || string.IsNullOrEmpty(newNhanvien.Hoten)
                || string.IsNullOrEmpty(newNhanvien.Gioitinh)
                || string.IsNullOrEmpty(newNhanvien.Diachi))
            {
                MessageBox.Show(
                    "Mã nhân viên, Họ tên, Giới tính và Địa chỉ không được để trống.",
                    "Lỗi nhập liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Gọi BLL để thêm nhân viên, BLL trả về true/false
                if (nhanvienBLL.AddNhanvien(newNhanvien))
                {
                    MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK; // Báo cho form cha biết có thay đổi
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Thêm nhân viên thất bại. Có thể mã nhân viên đã tồn tại hoặc dữ liệu không hợp lệ.",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (ArgumentException ex) // Lỗi validate từ BLL
            {
                MessageBox.Show($"Lỗi nhập liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) // Các lỗi hệ thống/CSDL
            {
                MessageBox.Show("Lỗi khi thêm nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Nút Cập nhật – dùng cho CHẾ ĐỘ SỬA nhân viên.
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();

            Nhanvien updatedNhanvien = GetNhanvienInfoFromControls();

            // Kiểm tra dữ liệu bắt buộc (có thể đưa sang BLL để tái sử dụng)
            if (string.IsNullOrEmpty(updatedNhanvien.Hoten)
                || string.IsNullOrEmpty(updatedNhanvien.Gioitinh)
                || string.IsNullOrEmpty(updatedNhanvien.Diachi))
            {
                MessageBox.Show(
                    "Họ tên, Giới tính và Địa chỉ không được để trống.",
                    "Lỗi nhập liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Gọi BLL để cập nhật nhân viên
                if (nhanvienBLL.UpdateNhanvien(updatedNhanvien))
                {
                    MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Cập nhật nhân viên thất bại. Không tìm thấy nhân viên hoặc dữ liệu không hợp lệ.",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (ArgumentException ex) // Lỗi nghiệp vụ/validate từ BLL
            {
                MessageBox.Show($"Lỗi nhập liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) // Các lỗi khác
            {
                MessageBox.Show("Lỗi khi cập nhật nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Nút Xóa – xóa nhân viên hiện tại.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();

            if (currentNhanvien == null)
            {
                MessageBox.Show("Không có nhân viên nào được chọn để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hộp thoại xác nhận xóa
            DialogResult confirmResult = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa nhân viên '{currentNhanvien.Hoten}' (Mã: {currentNhanvien.Manhanvien}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // Gọi BLL để xóa nhân viên
                    if (nhanvienBLL.DeleteNhanvien(currentNhanvien.Manhanvien))
                    {
                        MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(
                            "Xóa nhân viên thất bại. Không tìm thấy nhân viên.",
                            "Lỗi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
                catch (ArgumentException ex) // Lỗi nghiệp vụ từ BLL
                {
                    MessageBox.Show($"Lỗi nhập liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex) // Các lỗi khác
                {
                    MessageBox.Show("Lỗi khi xóa nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Nút đóng form (Close).
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            this.Close();
        }

        /// <summary>
        /// Click label (nếu dùng để tạo hiệu ứng âm thanh).
        /// </summary>
        private void lblCid_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
        }
    }
}
