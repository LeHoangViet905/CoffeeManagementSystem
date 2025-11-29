using CoffeeManagementSystem.BLL;
using CoffeeManagementSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class OrderDetailForm : Form
    {
        private readonly string _maHoaDon;
        private DonhangBLL _donhangBLL;
        private KhachhangDAL _khachhangDAL;
        private ThanhtoanDAL _thanhtoanDAL;

        public OrderDetailForm(string maHoaDon)
        {
            InitializeComponent();
            _maHoaDon = maHoaDon;

            _donhangBLL = new DonhangBLL();
            _khachhangDAL = new KhachhangDAL();
            _thanhtoanDAL = new ThanhtoanDAL();
        }

        private void OrderDetailForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_maHoaDon))
            {
                MessageBox.Show("Không xác định được mã hóa đơn.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            LoadOrderDetail();
        }

        private void LoadOrderDetail()
        {
            // 1. Thông tin đơn hàng
            var donhang = _donhangBLL.GetDonhangById(_maHoaDon);
            if (donhang == null)
            {
                MessageBox.Show("Không tìm thấy đơn hàng.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
                return;
            }

            lblMaDon.Text = donhang.Madonhang;
            lblThoiGian.Text = donhang.Thoigiandat.ToString("dd/MM/yyyy HH:mm");
            lblTrangThai.Text = donhang.Trangthaidon;
            lblTongTien.Text = donhang.Tongtien.ToString("N0") + " VNĐ";
            lblNhanVienLap.Text = donhang.Manhanvien; // nếu bạn có tên NV thì join thêm

            // 2. Khách hàng (có thể null)
            if (!string.IsNullOrWhiteSpace(donhang.Makhachhang))
            {
                var kh = _khachhangDAL.GetKhachhangById(donhang.Makhachhang);
                if (kh != null)
                {
                    lblTenKhach.Text = kh.Hoten;
                    lblSdt.Text = kh.Sodienthoai;
                    lblEmail.Text = kh.Email;
                    lblDiemTichLuy.Text = kh.Diemtichluy.ToString("N0");
                }
                else
                {
                    lblTenKhach.Text = "Khách lẻ";
                }
            }
            else
            {
                lblTenKhach.Text = "Khách lẻ";
            }

            // 3. Thanh toán (nếu có)
            var tt = _thanhtoanDAL.GetLatestByMadonhang(_maHoaDon); // hàm này mình thêm bên dưới
            if (tt != null)
            {
                lblHinhThucThanhToan.Text = tt.Hinhthucthanhtoan;
                lblThoiGianThanhToan.Text = tt.Thoigianthanhtoan.ToString("dd/MM/yyyy HH:mm");
                lblSoTienThanhToan.Text = tt.Sotienthanhtoan.ToString("N0") + " VNĐ";
                lblNhanVienThu.Text = tt.Manhanvienthu;
            }
            else
            {
                lblHinhThucThanhToan.Text = "Chưa thanh toán";
            }

            // 4. Chi tiết món
            var lines = _donhangBLL.GetOrderDetail(_maHoaDon);

            dgvLines.Rows.Clear();
            int stt = 1;
            foreach (var line in lines)
            {
                dgvLines.Rows.Add(
                    stt++,
                    line.Tendouong,
                    line.Soluong,
                    line.Dongia.ToString("N0"),
                    line.Thanhtien.ToString("N0")
                );
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
