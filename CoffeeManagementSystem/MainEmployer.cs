using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class MainEmployer : Form
    {
        private string _loggedInMaNhanVien;
        private string _maNhanVienHienTai;
        private string _tenNhanVienHienTai;

        // Constructor mặc định (đã có sẵn)
        public MainEmployer()
        {
            InitializeComponent();
            LoadFormCon(new OrderForm());
            SetSidebarActiveColor(btnOrder);
        }

        // Constructor MỚI để nhận thông tin nhân viên từ DangNhapForm
        public MainEmployer(string maNhanVien, string tenNhanVien) : this()
        {
            _loggedInMaNhanVien = maNhanVien;
            _maNhanVienHienTai = maNhanVien;
            _tenNhanVienHienTai = tenNhanVien;
            if (lblName != null)
            {
                lblName.Text = tenNhanVien;
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

        private void MainForm_Load(object sender, EventArgs e) // Tên method nên là MainEmployer_Load để đúng với tên Form
        {
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            SetSidebarActiveColor(btnKhachHang);
            MainForm.PlayClickSound();
            LoadFormCon(new CustomerForm());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            this.Close();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            SetSidebarActiveColor(btnOrder);
            MainForm.PlayClickSound();
            // Tạo OrderForm và truyền mã nhân viên, tên nhân viên
            OrderForm orderForm = new OrderForm(_maNhanVienHienTai, _tenNhanVienHienTai);
            LoadFormCon(orderForm);
        }

        private void lblNhanVien_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            // Xử lý sự kiện click cho label nhân viên
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            this.Hide();

            // 2. Tạo một thể hiện mới của DangNhapForm
            DangNhapForm loginForm = new DangNhapForm();

            // 3. Hiển thị DangNhapForm
            loginForm.Show();
            this.Close();
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            SetSidebarActiveColor(btnTaiKhoan);
            MainForm.PlayClickSound();
            LoadFormCon(new Infor(_loggedInMaNhanVien));
        }
        private void btnLichSuDonHang_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();

            // Nếu bạn muốn form lịch sử cũng biết mã & tên nhân viên hiện tại:
            OrderHistoryForm historyForm = new OrderHistoryForm(_maNhanVienHienTai, _tenNhanVienHienTai);

            // Nếu chưa cần dùng thông tin nhân viên trong form lịch sử:
            // OrderHistoryForm historyForm = new OrderHistoryForm();

            LoadFormCon(historyForm);
        }
    }
}
