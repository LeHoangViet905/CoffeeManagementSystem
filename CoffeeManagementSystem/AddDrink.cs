using CoffeeManagementSystem.BLL; // Đã có sẵn và được giữ nguyên
using CoffeeManagementSystem.DAL;
using System;
using System.Collections.Generic;
using System.IO;                   // Thư viện dùng để làm việc với file
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class AddDrinkForm : Form
    {
        // Các đối tượng tầng nghiệp vụ (Business Logic Layer)
        private DouongBLL _douongBLL = new DouongBLL();
        private GiadouongBLL _giadouongBLL = new GiadouongBLL();
        private LoaidouongBLL _loaidouongBLL = new LoaidouongBLL();

        // Đồ uống hiện tại đang thao tác (khi ở chế độ chỉnh sửa)
        private Douong _currentDouong;
        // Cờ xác định form đang ở chế độ thêm mới hay chỉnh sửa
        private bool _isNewEntry = false;
        // Lưu tên/đường dẫn ảnh đã chọn (thực tế chỉ lưu tên file vào DB)
        private string _selectedImagePath = "";

        // Constructor cho trường hợp thêm mới đồ uống
        public AddDrinkForm()
        {
            InitializeComponent();
            _isNewEntry = true;
            this.Text = "Thêm Đồ Uống Mới";

            // Tự sinh mã đồ uống mới từ BLL
            txtMadouong.Text = _douongBLL.GenerateNextMaDU();
            txtMadouong.Enabled = false; // Không cho chỉnh sửa mã đồ uống

            // Tải danh sách loại đồ uống lên ComboBox
            LoadLoaiDouongComboBox();
            cbLoaiDouong.SelectedIndex = 0;   // Chọn mục đầu tiên mặc định
            cbLoaiDouong.SelectedIndexChanged += cbLoaiDouong_SelectedIndexChanged;

            // Gán sự kiện cho các nút bấm
            btnLuu.Click += btnLuu_Click;
            btnCapNhat.Click += btnCapNhat_Click;
            btnXoa.Click += btnXoa_Click;
            btnSelectImage.Click += btnSelectImage_Click;
            button1.Click += button1_Click; // Nút Hủy/Đóng

            // Khởi tạo trạng thái ban đầu cho PictureBox và đường dẫn ảnh
            pbHinhanh.Image = null;
            _selectedImagePath = "";

            // Đặt trạng thái nút ban đầu cho chế độ thêm mới (chỉ cho phép Lưu)
            SetButtonState(true, false, false);
        }

        // Constructor cho trường hợp xem/chỉnh sửa chi tiết đồ uống
        public AddDrinkForm(string madouong)
        {
            InitializeComponent();
            _isNewEntry = false;
            this.Text = "Chi Tiết Đồ Uống";

            txtMadouong.Enabled = false; // Không cho đổi mã đồ uống

            // Tải danh sách loại đồ uống lên ComboBox
            LoadLoaiDouongComboBox(); // ComboBox để hiển thị các loại đồ uống
            cbLoaiDouong.SelectedIndex = 0;
            cbLoaiDouong.SelectedIndexChanged += cbLoaiDouong_SelectedIndexChanged;

            // Gán sự kiện cho các nút bấm
            btnLuu.Click += btnLuu_Click;
            btnCapNhat.Click += btnCapNhat_Click;
            btnXoa.Click += btnXoa_Click;
            btnSelectImage.Click += btnSelectImage_Click;
            button1.Click += button1_Click; // Nút Hủy/Đóng

            pbHinhanh.Image = null;
            _selectedImagePath = "";

            // Trạng thái ban đầu trước khi load dữ liệu
            SetButtonState(true, false, false);

            // Tải chi tiết đồ uống theo mã
            LoadDouongDetails(madouong);

            // Sau khi load xong, chuyển sang chế độ chỉnh sửa: cho phép Cập nhật và Xóa
            SetButtonState(false, true, true);
        }

        // Sự kiện thay đổi loại đồ uống trong ComboBox
        // → Tự động gợi ý tên món, mô tả, giá và sinh mã đồ uống
        private void cbLoaiDouong_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Nếu ComboBox chưa chọn gì thì thoát
            if (cbLoaiDouong.SelectedItem == null) return;

            try
            {
                // Lấy thông tin loại được chọn (dùng dynamic cho tiện)
                dynamic selectedItem = cbLoaiDouong.SelectedItem;
                string tenLoai = selectedItem.Tenloai;
                string tenLoaiLower = tenLoai.ToLower();

                // PHẦN 1: Tự động sinh mã đồ uống mới theo danh sách hiện có
                txtMadouong.Text = GenerateNextMadouong();

                // PHẦN 2: Thiết lập dữ liệu gợi ý (tên, mô tả, giá) theo loại
                string suggestName = "";
                string suggestDesc = "";
                decimal suggestPrice = 0;

                // Logic gợi ý món theo tên loại đồ uống
                if (tenLoaiLower.Contains("cà phê"))
                {
                    suggestName = "Cà Phê Sữa Đá";
                    suggestPrice = 29000;
                    suggestDesc = "Hương vị cà phê đậm đà quyện cùng sữa đặc ngọt ngào.";
                }
                else if (tenLoaiLower.Contains("trà"))
                {
                    suggestName = "Trà Đào Cam Sả";
                    suggestPrice = 35000;
                    suggestDesc = "Vị thanh mát của trà kết hợp đào giòn và hương sả.";
                }
                else if (tenLoaiLower.Contains("nước ép"))
                {
                    suggestName = "Nước Ép Cam Tươi";
                    suggestPrice = 40000;
                    suggestDesc = "100% cam tươi nguyên chất, bổ sung vitamin C.";
                }
                else if (tenLoaiLower.Contains("đá xay"))
                {
                    suggestName = "Matcha Đá Xay";
                    suggestPrice = 45000;
                    suggestDesc = "Bột matcha Nhật Bản xay nhuyễn cùng đá và kem tươi.";
                }
                else if (tenLoaiLower.Contains("bánh"))
                {
                    suggestName = "Bánh Tiramisu";
                    suggestPrice = 35000;
                    suggestDesc = "Bánh mềm mịn với lớp kem phô mai béo ngậy.";
                }
                else if (tenLoaiLower.Contains("sinh tố"))
                {
                    suggestName = "Sinh Tố Bơ";
                    suggestPrice = 42000;
                    suggestDesc = "Bơ sáp dẻo mịn xay cùng sữa đặc béo ngậy.";
                }
                else if (tenLoaiLower.Contains("sữa chua") || tenLoaiLower.Contains("smoothies"))
                {
                    suggestName = "Sữa Chua Trái Cây";
                    suggestPrice = 42000;
                    suggestDesc = "Sữa chua lên men tự nhiên kết hợp trái cây tươi.";
                }
                else if (tenLoaiLower.Contains("đặc biệt"))
                {
                    suggestName = "Signature Coffee";
                    suggestPrice = 55000;
                    suggestDesc = "Công thức độc quyền chỉ có tại quán.";
                }
                else if (tenLoaiLower.Contains("giải khát"))
                {
                    suggestName = "Coca Cola";
                    suggestPrice = 20000;
                    suggestDesc = "Nước ngọt có gas giải khát tức thì.";
                }
                else if (tenLoaiLower.Contains("thực phẩm") || tenLoaiLower.Contains("nhẹ"))
                {
                    suggestName = "Khô Gà Lá Chanh";
                    suggestPrice = 25000;
                    suggestDesc = "Món ăn vặt giòn tan, vị cay nhẹ hương lá chanh.";
                }
                else
                {
                    // Mặc định nếu loại mới/chung chung
                    suggestName = tenLoai + " Mới";
                    suggestPrice = 30000;
                    suggestDesc = "Mô tả đang cập nhật...";
                }

                // Đổ dữ liệu gợi ý lên các control
                txtGiaBan.Text = suggestPrice.ToString("N0"); // Giá gợi ý
                txtTendouong.Text = suggestName;              // Tên gợi ý
                txtMota.Text = suggestDesc;                   // Mô tả gợi ý
            }
            catch (Exception)
            {
                // Bỏ qua lỗi, tránh crash form nếu có lỗi ép kiểu...
            }
        }

        // Hàm dùng để tạo mới mã đồ uống tự động dựa trên danh sách hiện có
        private string GenerateNextMadouong()
        {
            string prefix = "DU"; // Tiền tố dùng trong việc sinh mã đồ uống tự động

            try
            {
                // Lấy danh sách tất cả đồ uống hiện có từ BLL
                List<Douong> allDrinks = _douongBLL.GetAllDouongs();

                // Nếu chưa có đồ uống nào thì bắt đầu từ DU001
                if (allDrinks == null || allDrinks.Count == 0) return prefix + "001";

                int maxNumber = 0;

                // Duyệt qua tất cả mã đồ uống để tìm số lớn nhất
                foreach (var drink in allDrinks)
                {
                    if (drink.Madouong.StartsWith(prefix))
                    {
                        string numberPart = drink.Madouong.Substring(prefix.Length);
                        if (int.TryParse(numberPart, out int number))
                        {
                            if (number > maxNumber) maxNumber = number;
                        }
                    }
                }

                // Tăng 1 đơn vị so với số lớn nhất hiện tại, format dạng 3 chữ số
                return prefix + (maxNumber + 1).ToString("D3");
            }
            catch
            {
                // Nếu có lỗi, fallback: sinh mã ngẫu nhiên trong khoảng 100–999
                return prefix + new Random().Next(100, 999).ToString();
            }
        }

        /// <summary>
        /// Tải danh sách loại đồ uống vào ComboBox.
        /// Sử dụng LoaidouongBLL để lấy dữ liệu từ DB.
        /// </summary>
        private void LoadLoaiDouongComboBox()
        {
            try
            {
                List<Loaidouong> loaiDouongs = _loaidouongBLL.GetAllLoaidouongs();
                cbLoaiDouong.DataSource = loaiDouongs;
                cbLoaiDouong.DisplayMember = "Tenloai"; // Tên loại hiển thị
                cbLoaiDouong.ValueMember = "Maloai";    // Giá trị lưu là mã loại
                cbLoaiDouong.SelectedIndex = -1;        // Chưa chọn loại nào ban đầu
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi tải danh sách loại đồ uống vào ComboBox: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tải thông tin chi tiết đồ uống vào các control trên Form.
        /// Sử dụng DouongBLL để lấy đồ uống, GiadouongBLL để lấy giá hiện tại.
        /// </summary>
        /// <param name="madouong">Mã đồ uống cần tải.</param>
        private void LoadDouongDetails(string madouong)
        {
            try
            {
                // Lấy đồ uống theo mã
                _currentDouong = _douongBLL.GetDouongById(madouong);
                if (_currentDouong != null)
                {
                    // Gán dữ liệu cơ bản
                    txtMadouong.Text = _currentDouong.Madouong;
                    txtTendouong.Text = _currentDouong.Tendouong;
                    cbLoaiDouong.SelectedValue = _currentDouong.Maloai;

                    // Lấy giá hiện tại và hiển thị
                    _currentDouong.CurrentGia = _giadouongBLL.GetCurrentGia(madouong);
                    txtGiaBan.Text = _currentDouong.CurrentGia.ToString();

                    // Gán mô tả
                    txtMota.Text = _currentDouong.Mota;

                    // Xử lý hiển thị ảnh (nếu có)
                    if (!string.IsNullOrEmpty(_currentDouong.Hinhanh))
                    {
                        // Ghép đường dẫn thư mục ảnh chung + tên file ảnh
                        string fullPath = Path.Combine(ImageConfig.DrinkImageFolder, _currentDouong.Hinhanh);
                        if (File.Exists(fullPath))
                        {
                            pbHinhanh.ImageLocation = fullPath;
                            _selectedImagePath = _currentDouong.Hinhanh; // chỉ lưu tên file
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
                    MessageBox.Show("Không tìm thấy đồ uống này.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi tải chi tiết đồ uống: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút "Chọn" để chọn ảnh cho đồ uống.
        /// Lưu file ảnh vào thư mục Resources chung và chỉ lưu tên file vào DB.
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

                    // Đặt tên file theo mã đồ uống cho dễ quản lý, không bị trùng
                    string fileName = txtMadouong.Text.Trim() + ext;

                    // Đường dẫn đích trong thư mục Resources dùng chung
                    string destPath = Path.Combine(ImageConfig.DrinkImageFolder, fileName);

                    // Copy ảnh vào thư mục Resources (ghi đè nếu đã có)
                    File.Copy(sourcePath, destPath, true);

                    // CHỈ LƯU TÊN FILE vào DB (rất quan trọng để đường dẫn linh hoạt)
                    _selectedImagePath = fileName;

                    // Hiển thị ảnh lên PictureBox
                    pbHinhanh.ImageLocation = destPath;
                }
            }
        }

        /// <summary>
        /// Hàm tạo ID duy nhất cho bảng giá đồ uống (Magia).
        /// (Đã có GiadouongBLL.GenerateNewGiadouongId, hàm này giữ lại nếu cần dùng cục bộ)
        /// </summary>
        private string GenerateNewGiadouongId()
        {
            return "GIA" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        /// <summary>
        /// Đặt trạng thái Enabled cho các nút trên Form theo từng chế độ.
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

        // ================== EVENT HANDLER GỌI HÀM NGHIỆP VỤ ==================

        // Nhấn nút Lưu (dùng cho thêm mới)
        private void btnLuu_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            HandleAddDouong();
        }

        // Nhấn nút Cập nhật (dùng cho chỉnh sửa)
        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            HandleUpdateDouong();
        }

        // Nhấn nút Xóa đồ uống
        private void btnXoa_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            HandleDeleteDouong();
        }

        // Nút Hủy/Đóng form
        private void button1_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ================== XỬ LÝ NGHIỆP VỤ (THÊM / SỬA / XÓA) ==================

        /// <summary>
        /// Xử lý logic thêm một đồ uống mới và tạo bản ghi giá ban đầu.
        /// Sử dụng DouongBLL và GiadouongBLL để làm việc với DB.
        /// </summary>
        private void HandleAddDouong()
        {
            // Kiểm tra giá nhập vào hợp lệ
            decimal newGia;
            if (!decimal.TryParse(txtGiaBan.Text, out newGia) || newGia < 0)
            {
                MessageBox.Show("Giá bán không hợp lệ. Vui lòng nhập một số dương.",
                    "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrEmpty(txtMadouong.Text) ||
                string.IsNullOrEmpty(txtTendouong.Text) ||
                cbLoaiDouong.SelectedValue == null)
            {
                MessageBox.Show("Mã đồ uống, Tên đồ uống và Loại đồ uống không được để trống.",
                    "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Kiểm tra mã đồ uống đã tồn tại chưa
                if (_douongBLL.GetDouongById(txtMadouong.Text.Trim()) != null)
                {
                    MessageBox.Show(
                        $"Mã đồ uống '{txtMadouong.Text.Trim()}' đã tồn tại.",
                        "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng Douong mới từ dữ liệu form
                Douong newDouong = new Douong
                {
                    Madouong = txtMadouong.Text.Trim(),
                    Tendouong = txtTendouong.Text.Trim(),
                    Maloai = cbLoaiDouong.SelectedValue.ToString(),
                    Mota = txtMota.Text.Trim(),
                    Hinhanh = _selectedImagePath   // chỉ lưu tên file, ví dụ "DU001.jpg"
                };

                // Gọi BLL để thêm đồ uống vào DB
                _douongBLL.AddDouong(newDouong);

                // Tạo bản ghi giá ban đầu cho đồ uống mới
                Giadouong initialGia = new Giadouong
                {
                    Magia = _giadouongBLL.GenerateNewGiadouongId(), // Lấy ID từ BLL
                    Madouong = newDouong.Madouong,
                    Giaban = newGia,
                    Thoigianapdung = DateTime.Now
                };

                // Gọi BLL để thêm bản ghi giá
                _giadouongBLL.AddGiadouong(initialGia);

                MessageBox.Show("Thêm đồ uống thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (ArgumentException aex)
            {
                // Lỗi do dữ liệu không hợp lệ (lỗi nghiệp vụ/dữ liệu đầu vào)
                MessageBox.Show(aex.Message, "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ioex)
            {
                // Lỗi do vi phạm nghiệp vụ (quy tắc kinh doanh)
                MessageBox.Show(ioex.Message, "Lỗi nghiệp vụ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                // Lỗi hệ thống chung
                MessageBox.Show(
                    $"Lỗi hệ thống khi thêm đồ uống: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý logic cập nhật thông tin đồ uống và thêm bản ghi giá mới nếu giá thay đổi.
        /// </summary>
        private void HandleUpdateDouong()
        {
            // Kiểm tra đã có đồ uống hiện tại hay chưa
            if (_currentDouong == null)
            {
                MessageBox.Show("Không có đồ uống nào được chọn để cập nhật.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Kiểm tra giá nhập vào hợp lệ
            decimal newGia;
            if (!decimal.TryParse(txtGiaBan.Text, out newGia) || newGia < 0)
            {
                MessageBox.Show("Giá bán không hợp lệ. Vui lòng nhập một số dương.",
                    "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrEmpty(txtTendouong.Text) ||
                cbLoaiDouong.SelectedValue == null)
            {
                MessageBox.Show("Tên đồ uống và Loại đồ uống không được để trống.",
                    "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cập nhật thông tin từ Form vào đối tượng _currentDouong
            _currentDouong.Tendouong = txtTendouong.Text.Trim();
            _currentDouong.Maloai = cbLoaiDouong.SelectedValue.ToString();
            _currentDouong.Mota = txtMota.Text.Trim();
            _currentDouong.Hinhanh = _selectedImagePath; // tên file ảnh hiện tại

            try
            {
                // Gọi BLL để cập nhật đồ uống trong DB
                _douongBLL.UpdateDouong(_currentDouong);

                // Lấy giá hiện tại trong DB để so sánh
                decimal currentGia = _giadouongBLL.GetCurrentGia(_currentDouong.Madouong);
                if (newGia != currentGia)
                {
                    // Nếu giá thay đổi → thêm bản ghi giá mới vào bảng giá
                    Giadouong newGiadouongRecord = new Giadouong
                    {
                        Magia = _giadouongBLL.GenerateNewGiadouongId(), // Lấy ID từ BLL
                        Madouong = _currentDouong.Madouong,
                        Giaban = newGia,
                        Thoigianapdung = DateTime.Now
                    };

                    _giadouongBLL.AddGiadouong(newGiadouongRecord);

                    MessageBox.Show(
                        "Đã cập nhật thông tin đồ uống và giá mới!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Trường hợp chỉ cập nhật thông tin, giá không đổi
                    MessageBox.Show(
                        "Đã cập nhật thông tin đồ uống.",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message, "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ioex)
            {
                MessageBox.Show(ioex.Message, "Lỗi nghiệp vụ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi hệ thống khi cập nhật đồ uống: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý logic xóa một đồ uống (chỉ dùng DouongBLL).
        /// </summary>
        private void HandleDeleteDouong()
        {
            if (_currentDouong == null)
            {
                MessageBox.Show("Không có đồ uống nào được chọn để xóa.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hộp thoại xác nhận xóa
            DialogResult confirmResult = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa đồ uống '{_currentDouong.Tendouong}' (Mã: {_currentDouong.Madouong}) không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // Gọi BLL để xóa đồ uống khỏi DB
                    _douongBLL.DeleteDouong(_currentDouong.Madouong);

                    MessageBox.Show("Xóa đồ uống thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (ArgumentException aex)
                {
                    MessageBox.Show(aex.Message, "Lỗi nhập liệu",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (InvalidOperationException ioex)
                {
                    MessageBox.Show(ioex.Message, "Lỗi nghiệp vụ",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Lỗi hệ thống khi xóa đồ uống: {ex.Message}",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Label Giá được click → chỉ phát âm thanh, không có logic khác
        private void lblGia_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
        }

        // TextChanged của ô Giá bán – hiện tại chưa xử lý gì (để trống nếu sau này cần dùng)
        private void txtGiaBan_TextChanged(object sender, EventArgs e)
        {

        }

        // Event handler khác của nút Lưu (có thể do kéo thả designer tạo ra)
        // → Hiện tại chỉ phát âm thanh, không gọi nghiệp vụ
        private void btnLuu_Click_1(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
        }

        // Cấu hình thư mục lưu ảnh đồ uống
        public static class ImageConfig
        {
            // Thư mục ảnh dùng chung cho đồ uống (Resources nằm cạnh file .exe)
            public static readonly string DrinkImageFolder =
                Path.Combine(Application.StartupPath, "Resources");

            static ImageConfig()
            {
                // Đảm bảo thư mục tồn tại, nếu chưa thì tạo mới
                if (!Directory.Exists(DrinkImageFolder))
                {
                    Directory.CreateDirectory(DrinkImageFolder);
                }
            }
        }
    }
}
