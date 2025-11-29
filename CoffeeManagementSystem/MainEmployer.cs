using Guna.UI2.WinForms;
using System;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class MainEmployer : Form
    {
        private string _loggedInMaNhanVien;
        private string _maNhanVienHienTai;
        private string _tenNhanVienHienTai;

        // Constructor mặc định (dùng cho Designer)
        public MainEmployer()
        {
            InitializeComponent();
            LoadFormCon(new DashboardForm());

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

        // Sự kiện Load khớp với Designer
        private void MainEmployer_Load(object sender, EventArgs e)
        {
            // hiện tại chưa cần làm gì thêm
        }
        private void btnOrder_Click(object sender, EventArgs e)
        {
            SetSidebarActiveColor(btnOrder);
            MainForm.PlayClickSound();
            // Tạo OrderForm và truyền mã nhân viên, tên nhân viên
            OrderForm orderForm = new OrderForm(_maNhanVienHienTai, _tenNhanVienHienTai);
            LoadFormCon(orderForm);
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();

            // Nếu đã có thông tin nhân viên thì truyền vào OrderForm
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

        private void lblNhanVien_Click(object sender, EventArgs e)
        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            LoadFormCon(new CustomerForm());
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            LoadFormCon(new Infor(_loggedInMaNhanVien));
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();

            this.Hide();
            DangNhapForm loginForm = new DangNhapForm();
            loginForm.Show();
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            this.Close();
        }

        private void lblNhanVien_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            // Nếu sau này muốn mở form thông tin nhân viên thì xử lý thêm ở đây
        }

        private void btnLichSuDonHang_Click_1(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();

            Form frm;
            if (!string.IsNullOrEmpty(_maNhanVienHienTai) &&
                !string.IsNullOrEmpty(_tenNhanVienHienTai))
            {
                frm = new OrderHistoryForm(_maNhanVienHienTai, _tenNhanVienHienTai);
            }
            else
            {
                frm = new OrderHistoryForm();
            }

            LoadFormCon(frm);
        }
    }
}
