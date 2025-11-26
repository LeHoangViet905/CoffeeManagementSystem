using CoffeeManagementSystem.BLL;
using System;
using System.Windows.Forms;
namespace CoffeeManagementSystem
{
    public partial class FormChiTietNhanvien : Form
    {
        private NhanvienBLL nhanvienBLL;
        private Nhanvien currentNhanvien;

        // Constructor cho chế độ Thêm mới
        public FormChiTietNhanvien()
        {
            InitializeComponent();
            nhanvienBLL = new NhanvienBLL();
            this.Text = "Thêm Nhân Viên Mới";

            // Hiển thị tất cả các nút
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;

            // Điều chỉnh trạng thái Enabled cho chế độ Thêm mới
            btnSave.Enabled = true;
            btnUpdate.Enabled = false; // Không thể cập nhật khi thêm mới
            btnDelete.Enabled = false; // Không thể xóa khi thêm mới

            // Tự sinh mã nhân viên
            txtMaNV.Text = nhanvienBLL.GenerateNextMaNV();
            txtMaNV.Enabled = false;

            // Khởi tạo ComboBox Giới tính
            InitializeGioitinhComboBox();
        }

        // Constructor cho chế độ Cập nhật (NHẬN 1 ĐỐI SỐ)
        public FormChiTietNhanvien(Nhanvien nhanvienToEdit)
        {
            InitializeComponent();
            nhanvienBLL = new NhanvienBLL();
            this.Text = "Cập Nhật Thông Tin Nhân Viên";
            currentNhanvien = nhanvienToEdit; // Lưu đối tượng nhân viên cần sửa

            // Hiển thị tất cả các nút
            btnSave.Visible = true;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;

            // Điều chỉnh trạng thái Enabled cho chế độ Cập nhật
            btnSave.Enabled = false; // Không thể lưu mới khi cập nhật
            btnUpdate.Enabled = true; // Có thể cập nhật
            btnDelete.Enabled = true; // Có thể xóa

            // Vô hiệu hóa ô Mã NV khi sửa
            txtMaNV.Enabled = false;

            // Khởi tạo ComboBox Giới tính
            InitializeGioitinhComboBox();

            // Hiển thị thông tin nhân viên lên các control
            DisplayNhanvienInfo();
        }

        // Phương thức khởi tạo ComboBox Giới tính
        private void InitializeGioitinhComboBox()
        {
            cbGioiTinh.Items.Add("Nam");
            cbGioiTinh.Items.Add("Nữ");
            cbGioiTinh.Items.Add("Khác");
            cbGioiTinh.SelectedIndex = 0; // Chọn mặc định là "Nam"
        }

        // Phương thức hiển thị thông tin nhân viên lên các control
        private void DisplayNhanvienInfo()
        {
            if (currentNhanvien != null)
            {
                txtMaNV.Text = currentNhanvien.Manhanvien;
                txtHoTen.Text = currentNhanvien.Hoten;
                dateTimePickerNgaySinh.Value = currentNhanvien.Ngaysinh;
                cbGioiTinh.SelectedItem = currentNhanvien.Gioitinh; // Chọn giới tính trong ComboBox
                txtDiaChi.Text = currentNhanvien.Diachi;
                txtSDT.Text = currentNhanvien.Sodienthoai;
                txtEmail.Text = currentNhanvien.Email;
                dateTimePickerNgayVaoLam.Value = currentNhanvien.Ngayvaolam;
            }
        }

        // Phương thức lấy thông tin từ các control và tạo/cập nhật đối tượng Nhanvien
        private Nhanvien GetNhanvienInfoFromControls()
        {
            Nhanvien nhanvien = currentNhanvien ?? new Nhanvien();
            if (currentNhanvien == null)
            {
                nhanvien.Manhanvien = txtMaNV.Text.Trim();
            }
            nhanvien.Hoten = txtHoTen.Text.Trim();
            nhanvien.Ngaysinh = dateTimePickerNgaySinh.Value;
            nhanvien.Gioitinh = cbGioiTinh.SelectedItem?.ToString(); // Lấy giá trị từ ComboBox
            nhanvien.Diachi = txtDiaChi.Text.Trim();
            // Xử lý các cột có thể NULL
            string sdt = txtSDT.Text.Trim();
            nhanvien.Sodienthoai = string.IsNullOrEmpty(sdt) ? null : sdt;
            string email = txtEmail.Text.Trim();
            nhanvien.Email = string.IsNullOrEmpty(email) ? null : email;
            nhanvien.Ngayvaolam = dateTimePickerNgayVaoLam.Value;

            return nhanvien;
        }

        // Sự kiện click nút "Lưu" (cho chế độ Thêm mới)
        private void btnSave_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            Nhanvien newNhanvien = GetNhanvienInfoFromControls();

            if (string.IsNullOrEmpty(newNhanvien.Manhanvien) || string.IsNullOrEmpty(newNhanvien.Hoten) || string.IsNullOrEmpty(newNhanvien.Gioitinh) || string.IsNullOrEmpty(newNhanvien.Diachi))
            {
                MessageBox.Show("Mã nhân viên, Họ tên, Giới tính và Địa chỉ không được để trống.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Gọi BLL để thêm nhân viên và kiểm tra kết quả trả về
                if (nhanvienBLL.AddNhanvien(newNhanvien))
                {
                    MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm nhân viên thất bại. Có thể mã nhân viên đã tồn tại hoặc dữ liệu không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Lỗi nhập liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện click nút "Cập Nhật" (cho chế độ Sửa)
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            Nhanvien updatedNhanvien = GetNhanvienInfoFromControls();

            // Kiểm tra dữ liệu bắt buộc (có thể chuyển logic này sang BLL để tái sử dụng)
            if (string.IsNullOrEmpty(updatedNhanvien.Hoten) || string.IsNullOrEmpty(updatedNhanvien.Gioitinh) || string.IsNullOrEmpty(updatedNhanvien.Diachi))
            {
                MessageBox.Show("Họ tên, Giới tính và Địa chỉ không được để trống.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Gọi BLL để cập nhật nhân viên và kiểm tra kết quả trả về
                if (nhanvienBLL.UpdateNhanvien(updatedNhanvien))
                {
                    MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật nhân viên thất bại. Không tìm thấy nhân viên hoặc dữ liệu không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (ArgumentException ex) // Bắt lỗi nghiệp vụ từ BLL
            {
                MessageBox.Show($"Lỗi nhập liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) // Bắt các lỗi khác
            {
                MessageBox.Show("Lỗi khi cập nhật nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện click nút "Xóa"
        private void btnDelete_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            if (currentNhanvien == null)
            {
                MessageBox.Show("Không có nhân viên nào được chọn để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên '{currentNhanvien.Hoten}' (Mã: {currentNhanvien.Manhanvien}) không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // Gọi BLL để xóa nhân viên và kiểm tra kết quả trả về
                    if (nhanvienBLL.DeleteNhanvien(currentNhanvien.Manhanvien))
                    {
                        MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Xóa nhân viên thất bại. Không tìm thấy nhân viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (ArgumentException ex) // Bắt lỗi nghiệp vụ từ BLL
                {
                    MessageBox.Show($"Lỗi nhập liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex) // Bắt các lỗi khác
                {
                    MessageBox.Show("Lỗi khi xóa nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Sự kiện click nút đóng Form (X) ở góc trên bên phải
        private void btnClose_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }



        // Sự kiện click nút đóng Form (X) ở góc trên bên phải

        private void lblCid_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
        }
    }
}