// Form thêm / chỉnh sửa Loại đồ uống

using CoffeeManagementSystem.BLL;
using System;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class AddTypeofdrinkForm : Form
    {
        // BLL xử lý nghiệp vụ loại đồ uống
        private LoaidouongBLL _loaidouongBLL;

        // Đối tượng loại đồ uống hiện tại (dùng khi chỉnh sửa / xóa)
        private Loaidouong _currentLoaidouong;

        // Cờ đánh dấu đang ở chế độ thêm mới hay không
        private bool _isNewEntry = false;

        /// <summary>
        /// Constructor dùng cho THÊM mới loại đồ uống.
        /// </summary>
        public AddTypeofdrinkForm()
        {
            InitializeComponent();

            _loaidouongBLL = new LoaidouongBLL();
            _isNewEntry = true;

            this.Text = "Thêm Loại Đồ Uống Mới";

            // Tự sinh mã loại tiếp theo từ BLL
            txtMaloai.Text = _loaidouongBLL.GenerateNextMaloai();
            txtMaloai.Enabled = false; // Không cho sửa mã (tránh trùng và đồng bộ format)

            // Gán handler cho các nút
            btnLuu.Click += btnLuu_Click;
            btnCapNhat.Click += btnCapNhat_Click;
            btnXoa.Click += btnXoa_Click;

            // Chế độ thêm mới: chỉ bật nút Lưu
            SetButtonState(true, false, false);
        }

        /// <summary>
        /// Constructor dùng cho CHỈNH SỬA loại đồ uống.
        /// </summary>
        /// <param name="maloai">Mã loại cần xem/chỉnh sửa.</param>
        public AddTypeofdrinkForm(string maloai)
        {
            InitializeComponent();

            _loaidouongBLL = new LoaidouongBLL();
            _isNewEntry = false;

            this.Text = "Chi Tiết Loại Đồ Uống";

            // Mã loại là khóa → không cho sửa
            txtMaloai.Enabled = false;

            // Gán handler cho các nút
            btnLuu.Click += btnLuu_Click;
            btnCapNhat.Click += btnCapNhat_Click;
            btnXoa.Click += btnXoa_Click;

            // Tải dữ liệu loại đồ uống đang chọn
            LoadLoaidouongDetails(maloai);

            // Chế độ chỉnh sửa: tắt Lưu, bật Cập nhật & Xóa
            SetButtonState(false, true, true);
        }

        /// <summary>
        /// Load chi tiết loại đồ uống lên form theo mã.
        /// </summary>
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

        /// <summary>
        /// Xử lý nút Lưu (chỉ dùng khi THÊM mới).
        /// </summary>
        private void btnLuu_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();

            // Nếu form đang ở chế độ chỉnh sửa thì không cho dùng Lưu
            if (!_isNewEntry)
            {
                MessageBox.Show("Vui lòng sử dụng nút 'Cập nhật' để sửa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Kiểm tra dữ liệu bắt buộc
            if (string.IsNullOrEmpty(txtMaloai.Text) || string.IsNullOrEmpty(txtTenloai.Text))
            {
                MessageBox.Show("Mã loại và Tên loại không được để trống.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Tạo đối tượng loại đồ uống mới từ dữ liệu form
                Loaidouong newLoai = new Loaidouong
                {
                    Maloai = txtMaloai.Text.Trim(),
                    Tenloai = txtTenloai.Text.Trim()
                };

                // Gọi BLL để thêm mới
                _loaidouongBLL.AddLoaidouong(newLoai);

                MessageBox.Show("Thêm loại đồ uống thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK; // Cho form cha biết là có thay đổi dữ liệu
                this.Close();
            }
            catch (ArgumentException argEx) // Lỗi validate dữ liệu từ BLL
            {
                MessageBox.Show($"Lỗi nhập liệu: {argEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException invOpEx) // Lỗi nghiệp vụ (vd: mã bị trùng)
            {
                MessageBox.Show($"Lỗi nghiệp vụ: {invOpEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) // Các lỗi khác (CSDL, hệ thống, ...)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý nút Cập nhật (chỉ dùng khi CHỈNH SỬA).
        /// </summary>
        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();

            // Nếu đang ở chế độ thêm mới thì không cho cập nhật
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
                // Cập nhật lại tên loại từ textbox
                _currentLoaidouong.Tenloai = txtTenloai.Text.Trim();

                // Gọi BLL để lưu thay đổi
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

        /// <summary>
        /// Xử lý nút Xóa loại đồ uống.
        /// </summary>
        private void btnXoa_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();

            // Không thể xóa ở chế độ thêm mới
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

            // Hỏi lại người dùng trước khi xóa
            DialogResult confirmResult = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa loại đồ uống '{_currentLoaidouong.Tenloai}' (Mã: {_currentLoaidouong.Maloai}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    _loaidouongBLL.DeleteLoaidouong(_currentLoaidouong.Maloai);
                    MessageBox.Show("Xóa loại đồ uống thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (InvalidOperationException invOpEx) // Ví dụ: loại đang được sử dụng bởi đồ uống
                {
                    MessageBox.Show($"Lỗi nghiệp vụ: {invOpEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Bật/tắt trạng thái các nút theo chế độ form (thêm mới / chỉnh sửa).
        /// </summary>
        private void SetButtonState(bool luuEnabled, bool capNhatEnabled, bool xoaEnabled)
        {
            btnLuu.Enabled = luuEnabled;
            btnCapNhat.Enabled = capNhatEnabled;
            btnXoa.Enabled = xoaEnabled;
        }

        /// <summary>
        /// Nút đóng form (button1).
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            this.Close();
        }
    }
}
