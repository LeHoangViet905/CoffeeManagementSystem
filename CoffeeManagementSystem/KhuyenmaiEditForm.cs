using CoffeeManagementSystem.BLL;
using System;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class KhuyenmaiEditForm : Form
    {
        private readonly KhuyenmaiBLL _khuyenmaiBLL;
        private readonly bool _isEditMode;
        private readonly Khuyenmai _original;

        public KhuyenmaiEditForm()
        {
            InitializeComponent();
            _khuyenmaiBLL = new KhuyenmaiBLL();

            this.Load += KhuyenmaiEditForm_Load;
            btnLuu.Click += btnLuu_Click;
            btnHuy.Click += (s, e) => this.Close();
        }

        public KhuyenmaiEditForm(Khuyenmai km) : this()
        {
            _isEditMode = true;
            _original = km;
        }

        private void KhuyenmaiEditForm_Load(object sender, EventArgs e)
        {
            if (_isEditMode && _original != null)
            {
                txtMaKM.Text = _original.MaKM;
                txtMaKM.Enabled = false;

                txtTen.Text = _original.TenChuongTrinh;
                dtpBatDau.Value = _original.NgayBatDau;
                dtpKetThuc.Value = _original.NgayKetThuc;
                nudPhanTram.Value = _original.PhanTramGiam;
                txtGhiChu.Text = _original.GhiChu;
            }
            else
            {
                dtpBatDau.Value = DateTime.Now;
                dtpKetThuc.Value = DateTime.Now.AddDays(7);
                nudPhanTram.Value = 5;
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaKM.Text))
                {
                    MessageBox.Show("Mã khuyến mãi không được để trống.");
                    txtMaKM.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTen.Text))
                {
                    MessageBox.Show("Tên chương trình không được để trống.");
                    txtTen.Focus();
                    return;
                }

                if (dtpKetThuc.Value <= dtpBatDau.Value)
                {
                    MessageBox.Show("Thời gian kết thúc phải lớn hơn thời gian bắt đầu.");
                    return;
                }

                int phanTram = (int)nudPhanTram.Value;
                if (phanTram <= 0 || phanTram > 50)
                {
                    MessageBox.Show("Phần trăm giảm phải > 0 và ≤ 50.");
                    return;
                }

                var km = new Khuyenmai
                {
                    MaKM = txtMaKM.Text.Trim(),
                    TenChuongTrinh = txtTen.Text.Trim(),
                    NgayBatDau = dtpBatDau.Value,
                    NgayKetThuc = dtpKetThuc.Value,
                    PhanTramGiam = phanTram,
                    GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim()
                };

                if (_isEditMode)
                {
                    _khuyenmaiBLL.UpdateKhuyenmai(km);
                }
                else
                {
                    _khuyenmaiBLL.AddKhuyenmai(km);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu khuyến mãi: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
