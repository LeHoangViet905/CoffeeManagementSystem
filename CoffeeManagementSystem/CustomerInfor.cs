using CoffeeManagementSystem.BLL;
using CoffeeManagementSystem.DAL; // nếu Khachhang model nằm trong DAL namespace
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

            // Trạng thái Enabled cho chế độ Thêm mới
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

            // Trạng thái Enabled cho chế độ Cập nhật
            btnSave1.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            // Vô hiệu hóa ô Mã KH khi sửa
            txtMaKH.Enabled = false;

            // Đổ dữ liệu lên form
            DisplayKhachhangInfo();
        }

        // Hiển thị thông tin khách hàng lên các control
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

        // Lấy thông tin từ control -> object Khachhang
        private Khachhang GetKhachhangInfoFromControls()
        {
            Khachhang khachhang = currentKhachhang ?? new Khachhang();

            // Chỉ set Mã KH khi thêm mới
            if (currentKhachhang == null)
            {
                khachhang.Makhachhang = txtMaKH.Text.Trim();
            }

            khachhang.Hoten = txtHoTen.Text.Trim();

            string sdt = txtSDT.Text.Trim();
            khachhang.Sodienthoai = string.IsNullOrEmpty(sdt) ? null : sdt;

            string email = txtEmail.Text.Trim();
            khachhang.Email = string.IsNullOrEmpty(email) ? null : email;

            khachhang.Ngaydangky = dateTimePickerNgayDangKy.Value;
            khachhang.Diemtichluy = (int)numericUpDownDiem.Value;

            return khachhang;
        }

        // ========== LƯU (Thêm mới) ==========
        private void btnSave_Click(object sender, EventArgs e)
        {
            // VALIDATION TẠI FORM (chỉ kiểm tra những thứ thuộc UI)
            // SDT / Email được phép trống, nhưng nếu có thì phải đúng format

            string phone = txtSDT.Text.Trim();
            if (!string.IsNullOrEmpty(phone) && !khachhangBLL.IsValidPhone(phone))
            {
                MessageBox.Show("Số điện thoại chỉ chứa số và phải đúng 10 ký tự!",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string email = txtEmail.Text.Trim();
            if (!string.IsNullOrEmpty(email) && !khachhangBLL.IsValidEmail(email))
            {
                MessageBox.Show("Email không hợp lệ! Hãy nhập đúng dạng: xxx@mail.com",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Khachhang newKhachhang = GetKhachhangInfoFromControls();

            if (string.IsNullOrEmpty(newKhachhang.Makhachhang) ||
                string.IsNullOrEmpty(newKhachhang.Hoten))
            {
                MessageBox.Show("Mã khách hàng và Họ tên không được để trống.",
                                "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Gọi BLL để thêm
                khachhangBLL.AddKhachhang(newKhachhang);

                // Nếu BLL không hiện MessageBox thì dùng message này;
                // nếu bạn vẫn để MessageBox trong BLL, có thể bỏ dòng này để tránh báo 2 lần.
                MessageBox.Show("Thêm khách hàng thành công!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show($"Lỗi nhập liệu: {argEx.Message}",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException invOpEx)
            {
                MessageBox.Show($"Lỗi nghiệp vụ: {invOpEx.Message}",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm khách hàng: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== CẬP NHẬT ==========
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string phone = txtSDT.Text.Trim();
            if (!string.IsNullOrEmpty(phone) && !khachhangBLL.IsValidPhone(phone))
            {
                MessageBox.Show("Số điện thoại chỉ chứa số và phải đúng 10 ký tự!",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string email = txtEmail.Text.Trim();
            if (!string.IsNullOrEmpty(email) && !khachhangBLL.IsValidEmail(email))
            {
                MessageBox.Show("Email không hợp lệ! Hãy nhập đúng dạng: xxx@mail.com",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Khachhang updatedKhachhang = GetKhachhangInfoFromControls();

            if (string.IsNullOrEmpty(updatedKhachhang.Hoten))
            {
                MessageBox.Show("Họ tên không được để trống.",
                                "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                khachhangBLL.UpdateKhachhang(updatedKhachhang);
                MessageBox.Show("Cập nhật khách hàng thành công!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show($"Lỗi nhập liệu: {argEx.Message}",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException invOpEx)
            {
                MessageBox.Show($"Lỗi nghiệp vụ: {invOpEx.Message}",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật khách hàng: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== XÓA ==========
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentKhachhang == null)
            {
                MessageBox.Show("Không có khách hàng nào được chọn để xóa.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmResult = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa khách hàng '{currentKhachhang.Hoten}' (Mã: {currentKhachhang.Makhachhang}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    khachhangBLL.DeleteKhachhang(currentKhachhang.Makhachhang);
                    MessageBox.Show("Xóa khách hàng thành công!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (ArgumentException argEx)
                {
                    MessageBox.Show($"Lỗi nhập liệu: {argEx.Message}",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (InvalidOperationException invOpEx)
                {
                    MessageBox.Show($"Lỗi nghiệp vụ: {invOpEx.Message}",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa khách hàng: " + ex.Message,
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Đóng form
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
