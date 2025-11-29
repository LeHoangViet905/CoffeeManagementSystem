

using CoffeeManagementSystem.BLL; // Đã có sẵn và được giữ nguyên
using CoffeeManagementSystem.DAL;
using System;
using System.Collections.Generic;
using System.IO;                   // Required for File.Exists
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
   
    public partial class AddDrinkForm : Form
    {
        //Hello Viet
        // Khởi tạo các đối tượng BLL
        private DouongBLL _douongBLL = new DouongBLL();
        private GiadouongBLL _giadouongBLL = new GiadouongBLL();
        private LoaidouongBLL _loaidouongBLL = new LoaidouongBLL();

        private Douong _currentDouong;
        private bool _isNewEntry = false;
        private string _selectedImagePath = "";

        // Constructor cho trường hợp thêm mới
        public AddDrinkForm()
        {
            InitializeComponent();
            _isNewEntry = true;
            this.Text = "Thêm Đồ Uống Mới";
            // Tự sinh mã đồ uống
            txtMadouong.Text = _douongBLL.GenerateNextMaDU();
            txtMadouong.Enabled = false;
            LoadLoaiDouongComboBox();

            // Gán sự kiện cho các nút
            btnLuu.Click += btnLuu_Click;
            btnCapNhat.Click += btnCapNhat_Click;
            btnXoa.Click += btnXoa_Click;
            btnSelectImage.Click += btnSelectImage_Click;
            button1.Click += button1_Click; // Nút Hủy/Đóng

            // Khởi tạo trạng thái cho PictureBox và đường dẫn ảnh
            pbHinhanh.Image = null;
            _selectedImagePath = "";

            // Đặt trạng thái nút ban đầu cho chế độ thêm mới
            SetButtonState(true, false, false);
        }

        // Constructor cho trường hợp chỉnh sửa
        public AddDrinkForm(string madouong)
        {
            InitializeComponent();
            _isNewEntry = false;
            this.Text = "Chi Tiết Đồ Uống";
            txtMadouong.Enabled = false;
            LoadLoaiDouongComboBox();

            // Gán sự kiện cho các nút
            btnLuu.Click += btnLuu_Click;
            btnCapNhat.Click += btnCapNhat_Click;
            btnXoa.Click += btnXoa_Click;
            btnSelectImage.Click += btnSelectImage_Click;
            button1.Click += button1_Click; // Nút Hủy/Đóng

            LoadDouongDetails(madouong);

            // Đặt trạng thái nút ban đầu cho chế độ chỉnh sửa
            SetButtonState(false, true, true);
        }

        /// <summary>
        /// Tải danh sách loại đồ uống vào ComboBox.
        /// Sử dụng LoaidouongBLL.
        /// </summary>
        private void LoadLoaiDouongComboBox()
        {
            try
            {
                List<Loaidouong> loaiDouongs = _loaidouongBLL.GetAllLoaidouongs();
                cbLoaiDouong.DataSource = loaiDouongs;
                cbLoaiDouong.DisplayMember = "Tenloai";
                cbLoaiDouong.ValueMember = "Maloai";
                cbLoaiDouong.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách loại đồ uống vào ComboBox: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tải thông tin chi tiết đồ uống vào các control.
        /// Sử dụng DouongBLL và GiadouongBLL.
        /// </summary>
        /// <param name="madouong">Mã đồ uống cần tải.</param>
        private void LoadDouongDetails(string madouong)
        {
            try
            {
                _currentDouong = _douongBLL.GetDouongById(madouong);
                if (_currentDouong != null)
                {
                    txtMadouong.Text = _currentDouong.Madouong;
                    txtTendouong.Text = _currentDouong.Tendouong;
                    cbLoaiDouong.SelectedValue = _currentDouong.Maloai;

                    _currentDouong.CurrentGia = _giadouongBLL.GetCurrentGia(madouong);
                    txtGiaBan.Text = _currentDouong.CurrentGia.ToString();

                    txtMota.Text = _currentDouong.Mota;
                    if (!string.IsNullOrEmpty(_currentDouong.Hinhanh))
                    {
                        string fullPath = Path.Combine(ImageConfig.DrinkImageFolder, _currentDouong.Hinhanh);
                        if (File.Exists(fullPath))
                        {
                            pbHinhanh.ImageLocation = fullPath;
                            _selectedImagePath = _currentDouong.Hinhanh; // tên file
                        }
                        else
                        {
                            pbHinhanh.Image = null;
                            _selectedImagePath = "";
                        }
                    }
                    else
                    {
                        pbHinhanh.Image = null;
                        _selectedImagePath = "";
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy đồ uống này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải chi tiết đồ uống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút "Chọn" để chọn ảnh.
        /// </summary>
        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*";
                openFileDialog.Title = "Chọn ảnh đồ uống";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string sourcePath = openFileDialog.FileName;
                    string ext = Path.GetExtension(sourcePath);

                    // Đặt tên file theo mã đồ uống cho dễ quản lý
                    string fileName = txtMadouong.Text.Trim() + ext;

                    // Đường dẫn đích trong thư mục Resources chung
                    string destPath = Path.Combine(ImageConfig.DrinkImageFolder, fileName);

                    // Copy ảnh vào thư mục Resources (ghi đè nếu đã có)
                    File.Copy(sourcePath, destPath, true);

                    // CHỈ LƯU TÊN FILE vào DB (rất quan trọng)
                    _selectedImagePath = fileName;

                    // Hiển thị lên PictureBox
                    pbHinhanh.ImageLocation = destPath;
                }
            }
        }

        /// <summary>
        /// Helper method để tạo ID duy nhất cho Magia.
        /// Phương thức này được giữ nguyên trong Form theo cấu trúc bạn muốn.
        /// </summary>
        private string GenerateNewGiadouongId()
        {
            return "GIA" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        /// <summary>
        /// Đặt trạng thái Enabled cho các nút trên Form.
        /// </summary>
        /// <param name="luuEnabled">Trạng thái Enabled của nút Lưu.</param>
        /// <param name="capNhatEnabled">Trạng thái Enabled của nút Cập nhật.</param>
        /// <param name="xoaEnabled">Trạng thái Enabled của nút Xóa.</param>
        private void SetButtonState(bool luuEnabled, bool capNhatEnabled, bool xoaEnabled)
        {
            btnLuu.Enabled = luuEnabled;
            btnCapNhat.Enabled = capNhatEnabled;
            btnXoa.Enabled = xoaEnabled;
        }

        // --- Event Handlers (Chỉ gọi các phương thức xử lý logic nghiệp vụ) ---

        private void btnLuu_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            HandleAddDouong();
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            HandleUpdateDouong();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            HandleDeleteDouong();
        }

        private void button1_Click(object sender, EventArgs e) // Nút Hủy/Đóng
        {
            MainForm.PlayClickSound();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // --- Các phương thức xử lý logic nghiệp vụ đã được tách ra ---

        /// <summary>
        /// Xử lý logic thêm một đồ uống mới và giá ban đầu.
        /// Sử dụng DouongBLL và GiadouongBLL.
        /// </summary>
        private void HandleAddDouong()
        {
            decimal newGia;
            if (!decimal.TryParse(txtGiaBan.Text, out newGia) || newGia < 0)
            {
                MessageBox.Show("Giá bán không hợp lệ. Vui lòng nhập một số dương.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtMadouong.Text) || string.IsNullOrEmpty(txtTendouong.Text) || cbLoaiDouong.SelectedValue == null)
            {
                MessageBox.Show("Mã đồ uống, Tên đồ uống và Loại đồ uống không được để trống.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Kiểm tra mã đồ uống đã tồn tại chưa bằng cách gọi BLL
                if (_douongBLL.GetDouongById(txtMadouong.Text.Trim()) != null)
                {
                    MessageBox.Show($"Mã đồ uống '{txtMadouong.Text.Trim()}' đã tồn tại.", "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Douong newDouong = new Douong
                {
                    Madouong = txtMadouong.Text.Trim(),
                    Tendouong = txtTendouong.Text.Trim(),
                    Maloai = cbLoaiDouong.SelectedValue.ToString(),
                    Mota = txtMota.Text.Trim(),
                    Hinhanh = _selectedImagePath   // chỉ còn là tên file, ví dụ "DU001.jpg"
                };


                // Gọi DouongBLL để thêm đồ uống
                _douongBLL.AddDouong(newDouong);

                // Thêm bản ghi giá ban đầu thông qua GiadouongBLL
                Giadouong initialGia = new Giadouong
                {
                    Magia = _giadouongBLL.GenerateNewGiadouongId(), // Lấy ID từ GiadouongBLL
                    Madouong = newDouong.Madouong,
                    Giaban = newGia,
                    Thoigianapdung = DateTime.Now
                };
                _giadouongBLL.AddGiadouong(initialGia); // Gọi GiadouongBLL để thêm giá

                MessageBox.Show("Thêm đồ uống thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ioex)
            {
                MessageBox.Show(ioex.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống khi thêm đồ uống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý logic cập nhật thông tin đồ uống và giá.
        /// Sử dụng DouongBLL và GiadouongBLL.
        /// </summary>
        private void HandleUpdateDouong()
        {
            if (_currentDouong == null)
            {
                MessageBox.Show("Không có đồ uống nào được chọn để cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal newGia;
            if (!decimal.TryParse(txtGiaBan.Text, out newGia) || newGia < 0)
            {
                MessageBox.Show("Giá bán không hợp lệ. Vui lòng nhập một số dương.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtTendouong.Text) || cbLoaiDouong.SelectedValue == null)
            {
                MessageBox.Show("Tên đồ uống và Loại đồ uống không được để trống.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cập nhật thông tin _currentDouong từ Form
            _currentDouong.Tendouong = txtTendouong.Text.Trim();
            _currentDouong.Maloai = cbLoaiDouong.SelectedValue.ToString();
            _currentDouong.Mota = txtMota.Text.Trim();
            _currentDouong.Hinhanh = _selectedImagePath; // tên file

            try
            {
                // Gọi DouongBLL để cập nhật thông tin đồ uống
                _douongBLL.UpdateDouong(_currentDouong);

                // Kiểm tra và cập nhật giá nếu có thay đổi thông qua GiadouongBLL
                decimal currentGia = _giadouongBLL.GetCurrentGia(_currentDouong.Madouong);
                if (newGia != currentGia)
                {
                    Giadouong newGiadouongRecord = new Giadouong
                    {
                        Magia = _giadouongBLL.GenerateNewGiadouongId(), // Lấy ID từ GiadouongBLL
                        Madouong = _currentDouong.Madouong,
                        Giaban = newGia,
                        Thoigianapdung = DateTime.Now
                    };
                    _giadouongBLL.AddGiadouong(newGiadouongRecord); // Gọi GiadouongBLL để thêm giá mới
                    MessageBox.Show("Đã cập nhật thông tin đồ uống và giá mới!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Đã cập nhật thông tin đồ uống.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ioex)
            {
                MessageBox.Show(ioex.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống khi cập nhật đồ uống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý logic xóa một đồ uống.
        /// Sử dụng DouongBLL.
        /// </summary>
        private void HandleDeleteDouong()
        {
            if (_currentDouong == null)
            {
                MessageBox.Show("Không có đồ uống nào được chọn để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa đồ uống '{_currentDouong.Tendouong}' (Mã: {_currentDouong.Madouong}) không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // Gọi DouongBLL để xóa đồ uống
                    _douongBLL.DeleteDouong(_currentDouong.Madouong);
                    MessageBox.Show("Xóa đồ uống thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (ArgumentException aex)
                {
                    MessageBox.Show(aex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (InvalidOperationException ioex)
                {
                    MessageBox.Show(ioex.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi hệ thống khi xóa đồ uống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void lblGia_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
        }

        private void txtGiaBan_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLuu_Click_1(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
        }
        public static class ImageConfig
        {
            // Thư mục ảnh dùng chung cho đồ uống
            public static readonly string DrinkImageFolder =
                Path.Combine(Application.StartupPath, "Resources");

            static ImageConfig()
            {
                // Đảm bảo thư mục tồn tại
                if (!Directory.Exists(DrinkImageFolder))
                {
                    Directory.CreateDirectory(DrinkImageFolder);
                }
            }
        }
    }
}