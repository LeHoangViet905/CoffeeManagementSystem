using CoffeeManagementSystem.BLL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class KhuyenMai : Form
    {
        private readonly KhuyenmaiBLL _khuyenmaiBLL;

        public KhuyenMai()
        {
            InitializeComponent();

            _khuyenmaiBLL = new KhuyenmaiBLL();

            // Gắn event
            this.Load += KhuyenMai_Load;
            btnTaoChuongTrinhMoi.Click += btnTaoChuongTrinhMoi_Click;
            btnSua.Click += btnSua_Click;
            btnKetThuc.Click += btnKetThuc_Click;
            btnDong.Click += (s, e) => this.Close();
        }

        private void KhuyenMai_Load(object sender, EventArgs e)
        {
            // Cho phép tự tạo cột theo model để đỡ phải thiết kế cột
            dgvSapDienRa.AutoGenerateColumns = true;
            dgvDangDienRa.AutoGenerateColumns = true;
            dgvDaKetThuc.AutoGenerateColumns = true;

            LoadAllTabs();
        }

        private void LoadAllTabs()
        {
            try
            {
                List<Khuyenmai> all = _khuyenmaiBLL.GetAllKhuyenmai();

                dgvSapDienRa.DataSource = all.FindAll(k => k.TrangThai == 0); // Sắp diễn ra
                dgvDangDienRa.DataSource = all.FindAll(k => k.TrangThai == 1); // Đang diễn ra
                dgvDaKetThuc.DataSource = all.FindAll(k => k.TrangThai == 2); // Đã kết thúc
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách khuyến mãi: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTaoChuongTrinhMoi_Click(object sender, EventArgs e)
        {
            using (var f = new KhuyenmaiEditForm())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    LoadAllTabs();
                }
            }
        }

        private Khuyenmai GetSelectedKhuyenmai()
        {
            DataGridView currentGrid;

            if (tabKhuyenMai.SelectedTab == tabSapDienRa)
                currentGrid = dgvSapDienRa;
            else if (tabKhuyenMai.SelectedTab == tabDangDienRa)
                currentGrid = dgvDangDienRa;
            else
                currentGrid = dgvDaKetThuc;

            if (currentGrid.CurrentRow == null)
                return null;

            return currentGrid.CurrentRow.DataBoundItem as Khuyenmai;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedKhuyenmai();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một chương trình khuyến mãi để sửa.");
                return;
            }

            using (var f = new KhuyenmaiEditForm(selected))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    LoadAllTabs();
                }
            }
        }

        private void btnKetThuc_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedKhuyenmai();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một chương trình khuyến mãi để kết thúc.");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn kết thúc chương trình này ngay bây giờ?",
                                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    selected.NgayKetThuc = DateTime.Now;
                    selected.TrangThai = 2; // Đã kết thúc
                    _khuyenmaiBLL.UpdateKhuyenmai(selected);
                    LoadAllTabs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi kết thúc khuyến mãi: " + ex.Message,
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
