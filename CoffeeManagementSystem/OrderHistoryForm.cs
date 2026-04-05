using CoffeeManagementSystem.BLL;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class OrderHistoryForm : Form
    {
        private readonly string _maNhanVien;
        private readonly string _tenNhanVien;
        private readonly DonhangBLL _donhangBLL = new DonhangBLL();

        public OrderHistoryForm(string maNhanVien, string tenNhanVien)
        {
            InitializeComponent();
            _maNhanVien = maNhanVien;
            _tenNhanVien = tenNhanVien;

            // KHÔNG cho auto tạo cột
            dgvOrderHistory.AutoGenerateColumns = false;

            // Map DataPropertyName cho từng cột
            colMaHoaDon.DataPropertyName = "Madonhang";
            colThoiGian.DataPropertyName = "Thoigiandat";
            colKhachHang.DataPropertyName = "TenKhachhang";
            colTongTien.DataPropertyName = "Tongtien";
            colHinhThucThanhToan.DataPropertyName = "HinhThucThanhToan";
            colTrangThai.DataPropertyName = "Trangthaidon";
        }

        public OrderHistoryForm() : this(null, null) { }

        private void OrderHistoryForm_Load(object sender, EventArgs e)
        {
            var today = DateTime.Today;
            dtpFrom.Value = today;
            dtpTo.Value = today;

            LoadOrdersByDateRange(today, today);
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            var today = DateTime.Today;
            dtpFrom.Value = today;
            dtpTo.Value = today;
            LoadOrdersByDateRange(today, today);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadOrdersByDateRange(dtpFrom.Value.Date, dtpTo.Value.Date);
        }

        private void dgvOrderHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                OpenSelectedOrderDetail();
        }

        private void btnViewDetail_Click(object sender, EventArgs e)
        {
            OpenSelectedOrderDetail();
        }

        private void LoadOrdersByDateRange(DateTime from, DateTime to)
        {
            if (from > to)
            {
                MessageBox.Show("Từ ngày không được lớn hơn đến ngày.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var data = _donhangBLL.GetOrderHistory(from, to);

            dgvOrderHistory.DataSource = null;
            dgvOrderHistory.DataSource = data;

            lblTotalOrders.Text = $"Tổng đơn: {data.Count} đơn";
            lblTotalRevenue.Text = $"Tổng tiền: {data.Sum(x => x.Tongtien):N0} VNĐ";
        }

        private void OpenSelectedOrderDetail()
        {
            if (dgvOrderHistory.CurrentRow == null)
                return;

            string maHoaDon = dgvOrderHistory.CurrentRow.Cells["colMaHoaDon"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(maHoaDon))
                return;

            using (var frm = new OrderDetailForm(maHoaDon))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }
    }
}
