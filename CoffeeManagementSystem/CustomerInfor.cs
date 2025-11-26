using CoffeeManagementSystem.BLL;
using CoffeeManagementSystem.DAL;
using System;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class FormChitiet : Form
    {
        private KhachhangBLL khachhangBLL;
        private Khachhang currentKhachhang;

        // Constructor cho chế độ Thêm mới
        public FormChitiet()
        {
            InitializeComponent();
            khachhangBLL = new KhachhangBLL();

            this.Text = "Thêm Khách Hàng Mới";

            // Hiển thị tất cả các nút
            btnSave1.Visible = true;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;

            // Điều chỉnh trạng thái Enabled cho chế độ Thêm mới
            btnSave1.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            // Tự sinh mã khách hàng mới
            txtMaKH.Text = khachhangBLL.GenerateNextMakhachhang();
            txtMaKH.Enabled = false;
        }

        // Constructor cho chế độ Cập nhật
        public FormChitiet(Khachhang khachhangToEdit)
        {
            InitializeComponent();
            khachhangBLL = new KhachhangBLL();

            this.Text = "Cập Nhật Thông Tin Khách Hàng";
            currentKhachhang = khachhangToEdit;

            // Hiển thị tất cả các nút
            btnSave1.Visible = true;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;

            // Điều chỉnh trạng thái Enabled cho chế độ Cập nhật
            btnSave1.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            // Vô hiệu hóa ô Mã KH khi sửa
            txtMaKH.Enabled = false;

            // Hiển thị thông tin khách hàng lên các control
            DisplayKhachhangInfo();
        }

        // Phương thức hiển thị thông tin khách hàng lên các control
        private void DisplayKhachhangInfo()
        {
            if (currentKhachhang != null)
            {
                txtMaKH.Text = currentKhachhang.Makhachhang;
                txtHoTen.Text = currentKhachhang.Hoten;
                txtSDT.Text = currentKhachhang.Sodienthoai;
                txtEmail.Text = currentKhachhang.Email;
                dateTimePickerNgayDangKy.Value = currentKhachhang.Ngaydangky;
                numericUpDownDiem.Value = currentKhachhang.Diemtichluy;
            }
        }

        // Phương thức lấy thông tin từ các control và tạo/cập nhật đối tượng Khachhang
        private Khachhang GetKhachhangInfoFromControls()
        {
            Khachhang khachhang = currentKhachhang ?? new Khachhang();
            if (currentKhachhang == null) // Chế độ thêm mới
            {
                khachhang.Makhachhang = txtMaKH.Text.Trim();
            }

            khachhang.Hoten = txtHoTen.Text.Trim();
            // Xử lý các cột có thể NULL
            string sdt = txtSDT.Text.Trim();
            khachhang.Sodienthoai = string.IsNullOrEmpty(sdt) ? null : sdt;
            string email = txtEmail.Text.Trim();
            khachhang.Email = string.IsNullOrEmpty(email) ? null : email;
            // Lấy giá trị từ DateTimePicker
            khachhang.Ngaydangky = dateTimePickerNgayDangKy.Value;
            // Lấy giá trị từ NumericUpDown
            khachhang.Diemtichluy = (int)numericUpDownDiem.Value;

            return khachhang;
        }

        // Sự kiện click nút "Lưu" (cho chế độ Thêm mới)
        private void btnSave_Click(object sender, EventArgs e)
        {
            Khachhang newKhachhang = GetKhachhangInfoFromControls();

            if (string.IsNullOrEmpty(newKhachhang.Makhachhang) || string.IsNullOrEmpty(newKhachhang.Hoten))
            {
                MessageBox.Show("Mã khách hàng và Họ tên không được để trống.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                khachhangBLL.AddKhachhang(newKhachhang);
                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show($"Lỗi nhập liệu: {argEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException invOpEx)
            {
                MessageBox.Show($"Lỗi nghiệp vụ: {invOpEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện click nút "Cập Nhật" (cho chế độ Sửa)
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Khachhang updatedKhachhang = GetKhachhangInfoFromControls();
            if (string.IsNullOrEmpty(updatedKhachhang.Hoten))
            {
                MessageBox.Show("Họ tên không được để trống.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                khachhangBLL.UpdateKhachhang(updatedKhachhang); // Gọi phương thức Cập nhật từ BLL
                MessageBox.Show("Cập nhật khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK; // Đặt kết quả Form là OK
                this.Close(); // Đóng Form
            }
            catch (ArgumentException argEx) // Bắt lỗi đối số không hợp lệ từ BLL
            {
                MessageBox.Show($"Lỗi nhập liệu: {argEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException invOpEx) // Bắt lỗi nghiệp vụ từ BLL
            {
                MessageBox.Show($"Lỗi nghiệp vụ: {invOpEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện click nút "Xóa"
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentKhachhang == null)
            {
                MessageBox.Show("Không có khách hàng nào được chọn để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng '{currentKhachhang.Hoten}' (Mã: {currentKhachhang.Makhachhang}) không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    khachhangBLL.DeleteKhachhang(currentKhachhang.Makhachhang);
                    MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (ArgumentException argEx)
                {
                    MessageBox.Show($"Lỗi nhập liệu: {argEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (InvalidOperationException invOpEx)
                {
                    MessageBox.Show($"Lỗi nghiệp vụ: {invOpEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Sự kiện click nút đóng Form (X) ở góc trên bên phải
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}