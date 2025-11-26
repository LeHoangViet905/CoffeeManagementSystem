using CoffeeManagementSystem.BLL; // Chỉ cần BLL, không cần DAL trực tiếp
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class OrderForm : Form
    {
        public string CurrentManhanvien { get; set; }
        public string CurrentTenNhanvien { get; set; }

        private Timer _messageTimer;

        private OrderBLL _orderBLL;

        public OrderForm()
        {
            InitializeComponent();

            _orderBLL = new OrderBLL(null);
            if (this.Controls.Find("lblStatusMessage", true).FirstOrDefault() is Label statusLabel)
            {
                lblStatusMessage = statusLabel;
            }
            else
            {
                lblStatusMessage = new Label { Name = "lblStatusMessage", Text = "", AutoSize = true, Location = new Point(10, 10) };
                this.Controls.Add(lblStatusMessage);
            }

            _messageTimer = new Timer();
            _messageTimer.Interval = 3000;
            _messageTimer.Tick += MessageTimer_Tick;

            dgvDouong.AutoGenerateColumns = false;
            dgvDouong.AllowUserToAddRows = false;
            dgvDouong.AllowUserToDeleteRows = false;
            dgvDouong.EditMode = DataGridViewEditMode.EditProgrammatically;

            // Gán sự kiện
            this.dgvDouong.CellDoubleClick += new DataGridViewCellEventHandler(dgvDouong_CellDoubleClick);
            this.btnTaoHoaDon.Click += new EventHandler(btnTaoHoaDon_Click);
            this.txtTimkiemdouong.TextChanged += new EventHandler(txtTimkiemdouong_TextChanged);

            LoadDanhSachDouong();
        }

        public OrderForm(string manhanvien, string tenNhanVien) : this() // Gọi constructor mặc định
        {
            CurrentManhanvien = manhanvien;
            CurrentTenNhanvien = tenNhanVien;
            // CẬP NHẬT: OrderBLL với mã nhân viên chính xác
            _orderBLL = new OrderBLL(CurrentManhanvien);
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            LoadDanhSachDouong();
        }

        //Tải danh sách đồ uống và hiển thị lên DataGridView.
        private void LoadDanhSachDouong()
        {
            try
            {
                // Gọi BLL để lấy danh sách đồ uống
                List<Douong> danhSach = _orderBLL.LoadAllDouongs();
                dgvDouong.DataSource = null;
                dgvDouong.DataSource = danhSach;
                dgvDouong.Refresh();
                dgvDouong.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách đồ uống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tải danh sách đồ uống đã lọc và hiển thị lên DataGridView.
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm.</param>
        private void LoadFilteredDouongData(string searchTerm)
        {
            try
            {
                // Gọi BLL để tìm kiếm đồ uống
                List<Douong> ketQuaHienThi = _orderBLL.SearchDouongs(searchTerm);

                dgvDouong.DataSource = null;
                dgvDouong.DataSource = ketQuaHienThi;
                dgvDouong.Refresh();
                dgvDouong.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm đồ uống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTimkiemdouong_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtTimkiemdouong.Text.Trim();
            LoadFilteredDouongData(searchTerm);
        }

        private void dgvLoaidouong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvDouong.Rows.Count - (dgvDouong.AllowUserToAddRows ? 1 : 0))
            {
                Loaidouong selectedLoaidouong = dgvDouong.Rows[e.RowIndex].DataBoundItem as Loaidouong;

                if (selectedLoaidouong != null)
                {
                    // Mở AddTypeofdrinkForm ở chế độ chỉnh sửa
                    AddTypeofdrinkForm detailForm = new AddTypeofdrinkForm(selectedLoaidouong.Maloai);
                    if (detailForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadDanhSachDouong(); // Tải lại danh sách sau khi chỉnh sửa/xóa thành công
                    }
                }
            }
        }

        private void dgvDouong_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra chỉ số dòng hợp lệ
            if (e.RowIndex >= 0 && e.RowIndex < dgvDouong.Rows.Count)
            {
                // Lấy đối tượng Douong từ dòng được double-click
                Douong selectedDoUong = dgvDouong.Rows[e.RowIndex].DataBoundItem as Douong;

                if (selectedDoUong != null)
                {
                    try
                    {
                        // Gọi BLL để thêm hoặc cập nhật chi tiết vào danh sách tạm thời của BLL
                        int totalItems = _orderBLL.AddOrUpdateChiTietHoaDonTamThoi(selectedDoUong);

                        // Hiển thị thông báo chọn thành công
                        if (lblStatusMessage != null)
                        {
                            lblStatusMessage.Text = $"Đã thêm '{selectedDoUong.Tendouong}' vào hóa đơn tạm thời. Tổng số món đã chọn: {totalItems}";
                            _messageTimer.Stop();
                            _messageTimer.Start();
                        }
                        else
                        {
                            MessageBox.Show($"Đã thêm '{selectedDoUong.Tendouong}' vào hóa đơn tạm thời.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi thêm đồ uống vào hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnTaoHoaDon_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy danh sách chi tiết hóa đơn tạm thời từ BLL để kiểm tra
                List<Chitietdonhang> currentOrderDetails = _orderBLL.GetTemporaryOrderDetails();

                if (currentOrderDetails.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn đồ uống để tạo hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Thông tin nhân viên được truyền qua constructor BLL, không cần kiểm tra lại ở đây
                if (string.IsNullOrEmpty(_orderBLL.GetCurrentMaNhanVien()))
                {
                    MessageBox.Show("Không thể tạo hóa đơn. Thiếu thông tin nhân viên lập hóa đơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Gọi BLL để tạo hóa đơn và lưu chi tiết vào DB
                bool hoaDonCreated = _orderBLL.CreateNewOrder();

                if (hoaDonCreated)
                {
                    // Nếu hóa đơn được tạo thành công trong DB, mở PaymentForm
                    // Truyền danh sách chi tiết hóa đơn từ BLL để PaymentForm xử lý
                    PaymentForm hoaDonForm = new PaymentForm(currentOrderDetails, CurrentManhanvien, CurrentTenNhanvien);

                    if (hoaDonForm.ShowDialog() == DialogResult.OK)
                    {
                        // Nếu hóa đơn được thanh toán/lưu thành công trong PaymentForm,
                        // yêu cầu BLL xóa danh sách tạm thời và cập nhật lại giao diện
                        _orderBLL.ClearTemporaryOrderDetails();
                        if (lblStatusMessage != null)
                        {
                            lblStatusMessage.Text = "Hóa đơn đã được tạo và xử lý thành công!";
                            _messageTimer.Stop();
                            _messageTimer.Start();
                        }
                        else
                        {
                            MessageBox.Show("Hóa đơn đã được tạo và xử lý thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        // Người dùng hủy hóa đơn, hỏi xem có muốn giữ lại danh sách tạm thời không
                        if (MessageBox.Show("Bạn có muốn hủy hóa đơn này và xóa các mục đã chọn không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            _orderBLL.ClearTemporaryOrderDetails(); // Yêu cầu BLL xóa
                            if (lblStatusMessage != null)
                            {
                                lblStatusMessage.Text = "Đã hủy hóa đơn tạm thời.";
                                _messageTimer.Stop();
                                _messageTimer.Start();
                            }
                            else
                            {
                                MessageBox.Show("Đã hủy hóa đơn tạm thời.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show(argEx.Message, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException invOpEx)
            {
                MessageBox.Show(invOpEx.Message, "Lỗi thao tác", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MessageTimer_Tick(object sender, EventArgs e)
        {
            if (lblStatusMessage != null)
            {
                lblStatusMessage.Text = "";
            }
            _messageTimer.Stop();
        }
    }
}