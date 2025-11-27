using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class MainForm : Form
    {
        // Thêm các biến để lưu trữ mã và tên nhân viên đăng nhập
        private string _loggedInMaNhanVien;
        private string _loggedInTenNhanVien;

        // Constructor mặc định (đã có sẵn)
        public MainForm()
        {
            InitializeComponent();
            LoadFormCon(new DashboardForm());
            SetSidebarActiveColor(btnTrangChu);
        }

        // Constructor MỚI để nhận thông tin nhân viên từ DangNhapForm
        public MainForm(string maNhanVien, string tenNhanVien) : this()
        {
            _loggedInMaNhanVien = maNhanVien;
            _loggedInTenNhanVien = tenNhanVien;

            if (lblName != null)
            {
                lblName.Text = tenNhanVien;
            }
        }
        public static void PlayClickSound()
        {
            try
            {
                using (var player = new System.Media.SoundPlayer(Properties.Resources.click))
                {
                    player.Play();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
            }
        }

        private void LoadFormCon(Form formCon)
        {
            // Xóa form con hiện tại nếu có
            panelMain.Controls.Clear();

            // Cấu hình form con
            formCon.TopLevel = false;
            formCon.FormBorderStyle = FormBorderStyle.None;
            formCon.Dock = DockStyle.Fill;

            // Thêm vào panel và hiển thị
            panelMain.Controls.Add(formCon);
            formCon.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            SetSidebarActiveColor(btnKhachHang);
            PlayClickSound();
            LoadFormCon(new CustomerForm());
        }

        private void btnEmployer_Click(object sender, EventArgs e)
        {
            SetSidebarActiveColor(btnEmployer);
            PlayClickSound();
            LoadFormCon(new EmployerForm());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            PlayClickSound();
            this.Close();
        }

        private void btnTrangChu_Click_1(object sender, EventArgs e)
        {
            SetSidebarActiveColor(btnTrangChu);
            PlayClickSound();
            LoadFormCon(new DashboardForm());
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            SetSidebarActiveColor(btnMenu);
            PlayClickSound();
            LoadFormCon(new DrinkForm());
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            SetSidebarActiveColor(btnReport);
            PlayClickSound();
            LoadFormCon(new ReportForm());
        }

      
        private void lblName_Click(object sender, EventArgs e)
        {
            PlayClickSound();
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            SetSidebarActiveColor(btnTaiKhoan);
            PlayClickSound();
            LoadFormCon(new Infor(_loggedInMaNhanVien));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            PlayClickSound();

            // 1. Ẩn MainForm hiện tại
            this.Hide();

            // 2. Tạo một thể hiện mới của DangNhapForm
            DangNhapForm loginForm = new DangNhapForm();

            // 3. Hiển thị DangNhapForm
            loginForm.Show();
            this.Close();
        }

        // Màu gốc và màu đậm
        private readonly Color baseColor = Color.FromArgb(224, 167, 167);
        private readonly Color activeColor = Color.FromArgb(164, 107, 107);   // đậm hơn 1 chút

        private void ResetMenuColor()
        {
            // tất cả nút sidebar đều là Guna2GradientButton
            btnTrangChu.FillColor = baseColor;
            btnTrangChu.FillColor2 = baseColor;

            btnKhachHang.FillColor = baseColor;
            btnKhachHang.FillColor2 = baseColor;

            btnEmployer.FillColor = baseColor;
            btnEmployer.FillColor2 = baseColor;

            btnMenu.FillColor = baseColor;
            btnMenu.FillColor2 = baseColor;

            btnReport.FillColor = baseColor;
            btnReport.FillColor2 = baseColor;

            btnTaiKhoan.FillColor = baseColor;
            btnTaiKhoan.FillColor2 = baseColor;

            btnLogout.FillColor = baseColor;
            btnLogout.FillColor2 = baseColor;
        }

        // Nút đang được chọn
        private void SetSidebarActiveColor(Guna2GradientButton activeButton)
        {
            ResetMenuColor();

            activeButton.FillColor = activeColor;
            activeButton.FillColor2 = activeColor;
        }

    }
}
