using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class MainEmployer : Form
    {
        private Color _sidebarDefaultColor = Color.FromArgb(255, 192, 192);
        private Color _sidebarActiveColor = Color.FromArgb(255, 128, 128);

        private string _loggedInMaNhanVien;
        private string _maNhanVienHienTai;
        private string _tenNhanVienHienTai;

        // Constructor mặc định (dùng cho Designer)
        public MainEmployer()
        {
            InitializeComponent();

            // Mặc định mở màn hình Đặt đồ uống
            LoadFormCon(new OrderForm());
        }

        // Constructor nhận thông tin nhân viên từ DangNhapForm
        public MainEmployer(string maNhanVien, string tenNhanVien) : this()
        {
            _loggedInMaNhanVien = maNhanVien;
            _maNhanVienHienTai = maNhanVien;
            _tenNhanVienHienTai = tenNhanVien;

            if (lblName != null)
                lblName.Text = tenNhanVien;
        }

        // ======= HÀM ĐỔI MÀU SIDEBAR =======
        private void SetSidebarActiveColor(Guna2GradientButton activeButton)
        {
            // 1. Reset tất cả về màu mặc định
            ResetSidebarButtonsColor();

            // 2. Tô màu cho nút đang active
            if (activeButton != null)
            {
                activeButton.FillColor = _sidebarActiveColor;
                activeButton.FillColor2 = _sidebarActiveColor;
            }
        }

        private void ResetSidebarButtonsColor()
        {
            // Dùng đúng kiểu Guna2GradientButton
            Guna2GradientButton[] allButtons =
            {
                btnOrder,
                btnLichSuDonHang,
                btnKhachHang,
                btnTaiKhoan,
                btnDangXuat
            };

            foreach (var btn in allButtons)
            {
                if (btn != null)
                {
                    btn.FillColor = _sidebarDefaultColor;
                    btn.FillColor2 = _sidebarDefaultColor;
                }
            }
        }
        // ===================================

        private void LoadFormCon(Form formCon)
        {
            panelMain.Controls.Clear();

            formCon.TopLevel = false;
            formCon.FormBorderStyle = FormBorderStyle.None;
            formCon.Dock = DockStyle.Fill;

            panelMain.Controls.Add(formCon);
            formCon.Show();
        }

        // Nếu Designer đang gắn sự kiện MainEmployer_Load thì đổi tên cho khớp
        private void MainEmployer_Load(object sender, EventArgs e)
        {
            // hiện tại chưa cần làm gì thêm
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            LoadFormCon(new CustomerForm());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            Close();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();

            // Nếu có thông tin nhân viên thì truyền, không thì dùng constructor mặc định
            if (!string.IsNullOrEmpty(_maNhanVienHienTai) &&
                !string.IsNullOrEmpty(_tenNhanVienHienTai))
            {
                LoadFormCon(new OrderForm(_maNhanVienHienTai, _tenNhanVienHienTai));
            }
            else
            {
                LoadFormCon(new OrderForm());
            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();

            Hide();
            var loginForm = new DangNhapForm();
            loginForm.Show();
            Close();
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            LoadFormCon(new Infor(_loggedInMaNhanVien));
        }

        private void btnLichSuDonHang_Click(object sender, EventArgs e)
        {
            SetSidebarActiveColor(btnLichSuDonHang);
            MainForm.PlayClickSound();

            var historyForm = new OrderHistoryForm(_maNhanVienHienTai, _tenNhanVienHienTai);
            LoadFormCon(historyForm);
        }

        private void lblNhanVien_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
        }
    }
}
