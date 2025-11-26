// Trong file AddTypeofdrinkForm.cs

using CoffeeManagementSystem.BLL;
using System;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{

    public partial class AddTypeofdrinkForm : Form
    {
        private LoaidouongBLL _loaidouongBLL;
        private Loaidouong _currentLoaidouong;
        private bool _isNewEntry = false;

        public AddTypeofdrinkForm()
        {
            InitializeComponent();
            _loaidouongBLL = new LoaidouongBLL();
            _isNewEntry = true;

            this.Text = "Thêm Loại Đồ Uống Mới";

            // Tự sinh mã loại tiếp theo
            txtMaloai.Text = _loaidouongBLL.GenerateNextMaloai();
            txtMaloai.Enabled = false; // Không cho sửa mã (cho đẹp & tránh trùng)

            btnLuu.Click += btnLuu_Click;
            btnCapNhat.Click += btnCapNhat_Click;
            btnXoa.Click += btnXoa_Click;

            SetButtonState(true, false, false);
        }


        public AddTypeofdrinkForm(string maloai)
        {
            InitializeComponent();
            _loaidouongBLL = new LoaidouongBLL();
            _isNewEntry = false;
            this.Text = "Chi Tiết Loại Đồ Uống";
            txtMaloai.Enabled = false;

            btnLuu.Click += btnLuu_Click;
            btnCapNhat.Click += btnCapNhat_Click;
            btnXoa.Click += btnXoa_Click;

            LoadLoaidouongDetails(maloai);

            SetButtonState(false, true, true);
        }

        private void LoadLoaidouongDetails(string maloai)
        {
            try
            {
                _currentLoaidouong = _loaidouongBLL.GetLoaidouongById(maloai);
                if (_currentLoaidouong != null)
                {
                    txtMaloai.Text = _currentLoaidouong.Maloai;
                    txtTenloai.Text = _currentLoaidouong.Tenloai;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy loại đồ uống này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải chi tiết loại đồ uống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            if (!_isNewEntry)
            {
                MessageBox.Show("Vui lòng sử dụng nút 'Cập nhật' để sửa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(txtMaloai.Text) || string.IsNullOrEmpty(txtTenloai.Text))
            {
                MessageBox.Show("Mã loại và Tên loại không được để trống.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Loaidouong newLoai = new Loaidouong
                {
                    Maloai = txtMaloai.Text.Trim(),
                    Tenloai = txtTenloai.Text.Trim()
                };

                _loaidouongBLL.AddLoaidouong(newLoai);
                MessageBox.Show("Thêm loại đồ uống thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (ArgumentException argEx) // Bắt lỗi nghiệp vụ từ BLL
            {
                MessageBox.Show($"Lỗi nhập liệu: {argEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException invOpEx) // Bắt lỗi nghiệp vụ từ BLL (ví dụ: trùng mã)
            {
                MessageBox.Show($"Lỗi nghiệp vụ: {invOpEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) // Bắt các lỗi khác
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            if (_isNewEntry)
            {
                MessageBox.Show("Vui lòng sử dụng nút 'Lưu' để thêm mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_currentLoaidouong == null)
            {
                MessageBox.Show("Không có loại đồ uống nào được chọn để cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(txtTenloai.Text))
            {
                MessageBox.Show("Tên loại không được để trống.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _currentLoaidouong.Tenloai = txtTenloai.Text.Trim();
                _loaidouongBLL.UpdateLoaidouong(_currentLoaidouong);
                MessageBox.Show("Cập nhật loại đồ uống thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            if (_isNewEntry)
            {
                MessageBox.Show("Không thể xóa khi đang thêm mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_currentLoaidouong == null)
            {
                MessageBox.Show("Không có loại đồ uống nào được chọn để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa loại đồ uống '{_currentLoaidouong.Tenloai}' (Mã: {_currentLoaidouong.Maloai}) không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    _loaidouongBLL.DeleteLoaidouong(_currentLoaidouong.Maloai);
                    MessageBox.Show("Xóa loại đồ uống thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (InvalidOperationException invOpEx) // Bắt lỗi nghiệp vụ từ BLL (ví dụ: có đồ uống đang dùng loại này)
                {
                    MessageBox.Show($"Lỗi nghiệp vụ: {invOpEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SetButtonState(bool luuEnabled, bool capNhatEnabled, bool xoaEnabled)
        {
            btnLuu.Enabled = luuEnabled;
            btnCapNhat.Enabled = capNhatEnabled;
            btnXoa.Enabled = xoaEnabled;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            this.Close();
        }
    }
}