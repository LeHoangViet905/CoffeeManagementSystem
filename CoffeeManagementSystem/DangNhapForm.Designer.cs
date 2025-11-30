namespace CoffeeManagementSystem
{
    partial class DangNhapForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.chkHienMatKhau = new System.Windows.Forms.CheckBox();
            this.lblMatKhau = new System.Windows.Forms.Label();
            this.lblTenDangNhap = new System.Windows.Forms.Label();
            this.lblDangNhap = new System.Windows.Forms.Label();
            this.lblClose = new System.Windows.Forms.Label();
            this.lblXinChao = new System.Windows.Forms.Label();
            this.lblDauGach = new System.Windows.Forms.Label();
            this.txtTenTaiKhoan = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtMatkhau = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(this.components);
            this.btnDangNhap = new Guna.UI2.WinForms.Guna2Button();
            this.panelHinhDangNhap = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelHinhDangNhap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // chkHienMatKhau
            // 
            this.chkHienMatKhau.AutoSize = true;
            this.chkHienMatKhau.Font = new System.Drawing.Font("SVN-Gilroy", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHienMatKhau.Location = new System.Drawing.Point(414, 367);
            this.chkHienMatKhau.Margin = new System.Windows.Forms.Padding(4);
            this.chkHienMatKhau.Name = "chkHienMatKhau";
            this.chkHienMatKhau.Size = new System.Drawing.Size(156, 28);
            this.chkHienMatKhau.TabIndex = 16;
            this.chkHienMatKhau.Text = "Hiện mật khẩu";
            this.chkHienMatKhau.UseVisualStyleBackColor = true;
            this.chkHienMatKhau.CheckedChanged += new System.EventHandler(this.chkHienMatKhau_CheckedChanged);
            // 
            // lblMatKhau
            // 
            this.lblMatKhau.AutoSize = true;
            this.lblMatKhau.Font = new System.Drawing.Font("SVN-Gilroy", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatKhau.ForeColor = System.Drawing.Color.Tomato;
            this.lblMatKhau.Location = new System.Drawing.Point(410, 293);
            this.lblMatKhau.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMatKhau.Name = "lblMatKhau";
            this.lblMatKhau.Size = new System.Drawing.Size(97, 22);
            this.lblMatKhau.TabIndex = 14;
            this.lblMatKhau.Text = "Mật khẩu:";
            // 
            // lblTenDangNhap
            // 
            this.lblTenDangNhap.AutoSize = true;
            this.lblTenDangNhap.Font = new System.Drawing.Font("SVN-Gilroy", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenDangNhap.ForeColor = System.Drawing.Color.Tomato;
            this.lblTenDangNhap.Location = new System.Drawing.Point(410, 219);
            this.lblTenDangNhap.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTenDangNhap.Name = "lblTenDangNhap";
            this.lblTenDangNhap.Size = new System.Drawing.Size(139, 22);
            this.lblTenDangNhap.TabIndex = 12;
            this.lblTenDangNhap.Text = "Tên tài khoản:";
            // 
            // lblDangNhap
            // 
            this.lblDangNhap.AutoSize = true;
            this.lblDangNhap.Font = new System.Drawing.Font("SVN-Gilroy", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDangNhap.ForeColor = System.Drawing.Color.Tomato;
            this.lblDangNhap.Location = new System.Drawing.Point(468, 62);
            this.lblDangNhap.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDangNhap.Name = "lblDangNhap";
            this.lblDangNhap.Size = new System.Drawing.Size(308, 71);
            this.lblDangNhap.TabIndex = 11;
            this.lblDangNhap.Text = "Đăng nhập";
            // 
            // lblClose
            // 
            this.lblClose.AutoSize = true;
            this.lblClose.Font = new System.Drawing.Font("Segoe UI Variable Display", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClose.ForeColor = System.Drawing.Color.Red;
            this.lblClose.Location = new System.Drawing.Point(752, 2);
            this.lblClose.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(30, 32);
            this.lblClose.TabIndex = 10;
            this.lblClose.Text = "X";
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            // 
            // lblXinChao
            // 
            this.lblXinChao.AutoSize = true;
            this.lblXinChao.Font = new System.Drawing.Font("SVN-Gilroy", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXinChao.Location = new System.Drawing.Point(410, 154);
            this.lblXinChao.Name = "lblXinChao";
            this.lblXinChao.Size = new System.Drawing.Size(463, 22);
            this.lblXinChao.TabIndex = 18;
            this.lblXinChao.Text = "Chào mừng trở lại. Đăng nhập để bắt đầu làm việc.\r\n";
            // 
            // lblDauGach
            // 
            this.lblDauGach.AutoSize = true;
            this.lblDauGach.Font = new System.Drawing.Font("Segoe UI Variable Text", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDauGach.Location = new System.Drawing.Point(410, 174);
            this.lblDauGach.Name = "lblDauGach";
            this.lblDauGach.Size = new System.Drawing.Size(444, 24);
            this.lblDauGach.TabIndex = 19;
            this.lblDauGach.Text = "______________________________________________________________\r\n";
            // 
            // txtTenTaiKhoan
            // 
            this.txtTenTaiKhoan.BackColor = System.Drawing.Color.Transparent;
            this.txtTenTaiKhoan.BorderRadius = 15;
            this.txtTenTaiKhoan.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTenTaiKhoan.DefaultText = "";
            this.txtTenTaiKhoan.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTenTaiKhoan.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTenTaiKhoan.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTenTaiKhoan.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTenTaiKhoan.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTenTaiKhoan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTenTaiKhoan.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTenTaiKhoan.Location = new System.Drawing.Point(414, 249);
            this.txtTenTaiKhoan.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtTenTaiKhoan.Name = "txtTenTaiKhoan";
            this.txtTenTaiKhoan.PlaceholderText = "";
            this.txtTenTaiKhoan.SelectedText = "";
            this.txtTenTaiKhoan.ShadowDecoration.BorderRadius = 15;
            this.txtTenTaiKhoan.ShadowDecoration.Depth = 10;
            this.txtTenTaiKhoan.ShadowDecoration.Enabled = true;
            this.txtTenTaiKhoan.Size = new System.Drawing.Size(333, 40);
            this.txtTenTaiKhoan.TabIndex = 20;
            this.txtTenTaiKhoan.TextChanged += new System.EventHandler(this.txtTenTaiKhoan_TextChanged);
            // 
            // txtMatkhau
            // 
            this.txtMatkhau.BackColor = System.Drawing.Color.Transparent;
            this.txtMatkhau.BorderRadius = 15;
            this.txtMatkhau.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMatkhau.DefaultText = "";
            this.txtMatkhau.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtMatkhau.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtMatkhau.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtMatkhau.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtMatkhau.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtMatkhau.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMatkhau.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtMatkhau.Location = new System.Drawing.Point(412, 319);
            this.txtMatkhau.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtMatkhau.Name = "txtMatkhau";
            this.txtMatkhau.PlaceholderText = "";
            this.txtMatkhau.SelectedText = "";
            this.txtMatkhau.ShadowDecoration.BorderRadius = 15;
            this.txtMatkhau.ShadowDecoration.Depth = 10;
            this.txtMatkhau.ShadowDecoration.Enabled = true;
            this.txtMatkhau.Size = new System.Drawing.Size(334, 40);
            this.txtMatkhau.TabIndex = 21;
            this.txtMatkhau.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMatkhau_KeyDown);
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.BorderRadius = 20;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // btnDangNhap
            // 
            this.btnDangNhap.BorderRadius = 15;
            this.btnDangNhap.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDangNhap.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDangNhap.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDangNhap.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDangNhap.FillColor = System.Drawing.Color.DarkCyan;
            this.btnDangNhap.Font = new System.Drawing.Font("SVN-Gilroy", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDangNhap.ForeColor = System.Drawing.Color.White;
            this.btnDangNhap.Location = new System.Drawing.Point(412, 415);
            this.btnDangNhap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDangNhap.Name = "btnDangNhap";
            this.btnDangNhap.Size = new System.Drawing.Size(334, 45);
            this.btnDangNhap.TabIndex = 22;
            this.btnDangNhap.Text = "Đăng nhập";
            // 
            // panelHinhDangNhap
            // 
            this.panelHinhDangNhap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHinhDangNhap.BackColor = System.Drawing.Color.White;
            this.panelHinhDangNhap.BackgroundImage = global::CoffeeManagementSystem.Properties.Resources.hinhdangnhap;
            this.panelHinhDangNhap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelHinhDangNhap.Controls.Add(this.pictureBox1);
            this.panelHinhDangNhap.Location = new System.Drawing.Point(0, 0);
            this.panelHinhDangNhap.Margin = new System.Windows.Forms.Padding(4);
            this.panelHinhDangNhap.Name = "panelHinhDangNhap";
            this.panelHinhDangNhap.Size = new System.Drawing.Size(374, 544);
            this.panelHinhDangNhap.TabIndex = 9;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CoffeeManagementSystem.Properties.Resources.bo_nhan_dien_thuong_hieu_ueh_5;
            this.pictureBox1.Location = new System.Drawing.Point(3, 486);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(71, 56);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            // 
            // DangNhapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FloralWhite;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(782, 544);
            this.ControlBox = false;
            this.Controls.Add(this.btnDangNhap);
            this.Controls.Add(this.txtTenTaiKhoan);
            this.Controls.Add(this.txtMatkhau);
            this.Controls.Add(this.lblDauGach);
            this.Controls.Add(this.lblXinChao);
            this.Controls.Add(this.panelHinhDangNhap);
            this.Controls.Add(this.chkHienMatKhau);
            this.Controls.Add(this.lblMatKhau);
            this.Controls.Add(this.lblTenDangNhap);
            this.Controls.Add(this.lblDangNhap);
            this.Controls.Add(this.lblClose);
            this.Font = new System.Drawing.Font("SVN-Gilroy", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DangNhapForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.panelHinhDangNhap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelHinhDangNhap;
        private System.Windows.Forms.CheckBox chkHienMatKhau;
        private System.Windows.Forms.Label lblMatKhau;
        private System.Windows.Forms.Label lblTenDangNhap;
        private System.Windows.Forms.Label lblDangNhap;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Label lblXinChao;
        private System.Windows.Forms.Label lblDauGach;
        private Guna.UI2.WinForms.Guna2TextBox txtTenTaiKhoan;
        private Guna.UI2.WinForms.Guna2TextBox txtMatkhau;
        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private Guna.UI2.WinForms.Guna2Button btnDangNhap;
        private Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

