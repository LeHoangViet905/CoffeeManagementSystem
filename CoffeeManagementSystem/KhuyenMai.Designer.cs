namespace CoffeeManagementSystem
{
    partial class KhuyenMai
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnTaoChuongTrinhMoi = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tabKhuyenMai = new System.Windows.Forms.TabControl();
            this.tabSapDienRa = new System.Windows.Forms.TabPage();
            this.dgvSapDienRa = new System.Windows.Forms.DataGridView();
            this.tabDangDienRa = new System.Windows.Forms.TabPage();
            this.dgvDangDienRa = new System.Windows.Forms.DataGridView();
            this.tabDaKetThuc = new System.Windows.Forms.TabPage();
            this.dgvDaKetThuc = new System.Windows.Forms.DataGridView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnDong = new System.Windows.Forms.Button();
            this.btnKetThuc = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();

            this.panelTop.SuspendLayout();
            this.tabKhuyenMai.SuspendLayout();
            this.tabSapDienRa.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSapDienRa)).BeginInit();
            this.tabDangDienRa.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDangDienRa)).BeginInit();
            this.tabDaKetThuc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDaKetThuc)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();

            // panelTop
            this.panelTop.Controls.Add(this.btnTaoChuongTrinhMoi);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Height = 60;

            // lblTitle
            this.lblTitle.Text = "Chương trình khuyến mãi";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(20, 15);

            // btnTaoChuongTrinhMoi
            this.btnTaoChuongTrinhMoi.Text = "Tạo chương trình khuyến mãi mới";
            this.btnTaoChuongTrinhMoi.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this.btnTaoChuongTrinhMoi.Location = new System.Drawing.Point(650, 15);
            this.btnTaoChuongTrinhMoi.Size = new System.Drawing.Size(220, 30);

            // tabKhuyenMai
            this.tabKhuyenMai.Controls.Add(this.tabSapDienRa);
            this.tabKhuyenMai.Controls.Add(this.tabDangDienRa);
            this.tabKhuyenMai.Controls.Add(this.tabDaKetThuc);
            this.tabKhuyenMai.Dock = System.Windows.Forms.DockStyle.Fill;

            // tabSapDienRa
            this.tabSapDienRa.Text = "Sắp diễn ra";
            this.tabSapDienRa.Controls.Add(this.dgvSapDienRa);

            // dgvSapDienRa
            this.dgvSapDienRa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSapDienRa.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSapDienRa.RowHeadersVisible = false;

            // tabDangDienRa
            this.tabDangDienRa.Text = "Đang diễn ra";
            this.tabDangDienRa.Controls.Add(this.dgvDangDienRa);

            // dgvDangDienRa
            this.dgvDangDienRa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDangDienRa.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDangDienRa.RowHeadersVisible = false;

            // tabDaKetThuc
            this.tabDaKetThuc.Text = "Đã kết thúc";
            this.tabDaKetThuc.Controls.Add(this.dgvDaKetThuc);

            // dgvDaKetThuc
            this.dgvDaKetThuc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDaKetThuc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDaKetThuc.RowHeadersVisible = false;

            // panelBottom
            this.panelBottom.Controls.Add(this.btnDong);
            this.panelBottom.Controls.Add(this.btnKetThuc);
            this.panelBottom.Controls.Add(this.btnSua);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Height = 55;

            // btnSua
            this.btnSua.Text = "Sửa khuyến mãi";
            this.btnSua.Location = new System.Drawing.Point(20, 10);
            this.btnSua.Size = new System.Drawing.Size(150, 35);

            // btnKetThuc
            this.btnKetThuc.Text = "Kết thúc ngay";
            this.btnKetThuc.Location = new System.Drawing.Point(180, 10);
            this.btnKetThuc.Size = new System.Drawing.Size(150, 35);

            // btnDong
            this.btnDong.Text = "Đóng";
            this.btnDong.Location = new System.Drawing.Point(650, 10);
            this.btnDong.Size = new System.Drawing.Size(100, 35);

            // KhuyenMai (Form)
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.tabKhuyenMai);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelBottom);
            this.Text = "Quản lý khuyến mãi";

            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.tabKhuyenMai.ResumeLayout(false);
            this.tabSapDienRa.ResumeLayout(false);
            this.tabDangDienRa.ResumeLayout(false);
            this.tabDaKetThuc.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnTaoChuongTrinhMoi;
        private System.Windows.Forms.TabControl tabKhuyenMai;
        private System.Windows.Forms.TabPage tabSapDienRa;
        private System.Windows.Forms.DataGridView dgvSapDienRa;
        private System.Windows.Forms.TabPage tabDangDienRa;
        private System.Windows.Forms.DataGridView dgvDangDienRa;
        private System.Windows.Forms.TabPage tabDaKetThuc;
        private System.Windows.Forms.DataGridView dgvDaKetThuc;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.Button btnKetThuc;
        private System.Windows.Forms.Button btnSua;
    }
}
