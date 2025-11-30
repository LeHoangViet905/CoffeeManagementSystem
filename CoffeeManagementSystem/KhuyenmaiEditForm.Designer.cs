namespace CoffeeManagementSystem
{
    partial class KhuyenmaiEditForm
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
            this.lblMa = new System.Windows.Forms.Label();
            this.txtMaKM = new System.Windows.Forms.TextBox();
            this.lblTen = new System.Windows.Forms.Label();
            this.txtTen = new System.Windows.Forms.TextBox();
            this.lblBatDau = new System.Windows.Forms.Label();
            this.dtpBatDau = new System.Windows.Forms.DateTimePicker();
            this.lblKetThuc = new System.Windows.Forms.Label();
            this.dtpKetThuc = new System.Windows.Forms.DateTimePicker();
            this.lblPhanTram = new System.Windows.Forms.Label();
            this.nudPhanTram = new System.Windows.Forms.NumericUpDown();
            this.lblGhiChu = new System.Windows.Forms.Label();
            this.txtGhiChu = new System.Windows.Forms.TextBox();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.nudPhanTram)).BeginInit();
            this.SuspendLayout();

            // lblMa
            this.lblMa.Text = "Mã khuyến mãi:";
            this.lblMa.Location = new System.Drawing.Point(30, 30);

            // txtMaKM
            this.txtMaKM.Location = new System.Drawing.Point(160, 25);
            this.txtMaKM.Width = 250;

            // lblTen
            this.lblTen.Text = "Tên chương trình:";
            this.lblTen.Location = new System.Drawing.Point(30, 80);

            // txtTen
            this.txtTen.Location = new System.Drawing.Point(160, 75);
            this.txtTen.Width = 250;

            // lblBatDau
            this.lblBatDau.Text = "Thời gian bắt đầu:";
            this.lblBatDau.Location = new System.Drawing.Point(30, 130);

            // dtpBatDau
            this.dtpBatDau.Location = new System.Drawing.Point(160, 125);
            this.dtpBatDau.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpBatDau.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBatDau.Width = 180;

            // lblKetThuc
            this.lblKetThuc.Text = "Thời gian kết thúc:";
            this.lblKetThuc.Location = new System.Drawing.Point(30, 180);

            // dtpKetThuc
            this.dtpKetThuc.Location = new System.Drawing.Point(160, 175);
            this.dtpKetThuc.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpKetThuc.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpKetThuc.Width = 180;

            // lblPhanTram
            this.lblPhanTram.Text = "% giảm giá:";
            this.lblPhanTram.Location = new System.Drawing.Point(30, 230);

            // nudPhanTram
            this.nudPhanTram.Location = new System.Drawing.Point(160, 225);
            this.nudPhanTram.Minimum = 1;
            this.nudPhanTram.Maximum = 50;   // RULE: ≤ 50%
            this.nudPhanTram.Width = 80;

            // lblGhiChu
            this.lblGhiChu.Text = "Ghi chú:";
            this.lblGhiChu.Location = new System.Drawing.Point(30, 280);

            // txtGhiChu
            this.txtGhiChu.Location = new System.Drawing.Point(160, 275);
            this.txtGhiChu.Multiline = true;
            this.txtGhiChu.Width = 300;
            this.txtGhiChu.Height = 100;

            // btnLuu
            this.btnLuu.Text = "Lưu";
            this.btnLuu.Location = new System.Drawing.Point(160, 400);
            this.btnLuu.Size = new System.Drawing.Size(120, 35);

            // btnHuy
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Location = new System.Drawing.Point(300, 400);
            this.btnHuy.Size = new System.Drawing.Size(120, 35);

            // Form 
            this.ClientSize = new System.Drawing.Size(520, 470);
            this.Controls.Add(this.lblMa);
            this.Controls.Add(this.txtMaKM);
            this.Controls.Add(this.lblTen);
            this.Controls.Add(this.txtTen);
            this.Controls.Add(this.lblBatDau);
            this.Controls.Add(this.dtpBatDau);
            this.Controls.Add(this.lblKetThuc);
            this.Controls.Add(this.dtpKetThuc);
            this.Controls.Add(this.lblPhanTram);
            this.Controls.Add(this.nudPhanTram);
            this.Controls.Add(this.lblGhiChu);
            this.Controls.Add(this.txtGhiChu);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnHuy);
            this.Text = "Chương trình khuyến mãi";

            ((System.ComponentModel.ISupportInitialize)(this.nudPhanTram)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMa;
        private System.Windows.Forms.TextBox txtMaKM;
        private System.Windows.Forms.Label lblTen;
        private System.Windows.Forms.TextBox txtTen;
        private System.Windows.Forms.Label lblBatDau;
        private System.Windows.Forms.DateTimePicker dtpBatDau;
        private System.Windows.Forms.Label lblKetThuc;
        private System.Windows.Forms.DateTimePicker dtpKetThuc;
        private System.Windows.Forms.Label lblPhanTram;
        private System.Windows.Forms.NumericUpDown nudPhanTram;
        private System.Windows.Forms.Label lblGhiChu;
        private System.Windows.Forms.TextBox txtGhiChu;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
    }
}
