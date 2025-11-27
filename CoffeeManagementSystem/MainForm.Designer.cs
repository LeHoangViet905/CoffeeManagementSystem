namespace CoffeeManagementSystem
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lblName = new System.Windows.Forms.Label();
            this.lblQuanLy = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.splitContainerNavBar = new System.Windows.Forms.SplitContainer();
            this.btnLogout = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnTaiKhoan = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnReport = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnTrangChu = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnKhachHang = new Guna.UI2.WinForms.Guna2GradientButton();
            this.guna2CirclePictureBox1 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.btnEmployer = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnMenu = new Guna.UI2.WinForms.Guna2GradientButton();
            this.pnlTitleBar = new Guna.UI2.WinForms.Guna2Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2ControlBox3 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2ControlBox2 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2ShadowForm1 = new Guna.UI2.WinForms.Guna2ShadowForm(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerNavBar)).BeginInit();
            this.splitContainerNavBar.Panel1.SuspendLayout();
            this.splitContainerNavBar.Panel2.SuspendLayout();
            this.splitContainerNavBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).BeginInit();
            this.pnlTitleBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.Color.White;
            this.lblName.Location = new System.Drawing.Point(11, 120);
            this.lblName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(156, 68);
            this.lblName.TabIndex = 18;
            this.lblName.Text = "Tên người dùng";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblName.Click += new System.EventHandler(this.lblName_Click);
            // 
            // lblQuanLy
            // 
            this.lblQuanLy.AutoSize = true;
            this.lblQuanLy.BackColor = System.Drawing.Color.Transparent;
            this.lblQuanLy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuanLy.ForeColor = System.Drawing.Color.White;
            this.lblQuanLy.Location = new System.Drawing.Point(62, 100);
            this.lblQuanLy.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQuanLy.Name = "lblQuanLy";
            this.lblQuanLy.Size = new System.Drawing.Size(58, 18);
            this.lblQuanLy.TabIndex = 17;
            this.lblQuanLy.Text = "Quản lý";
            this.lblQuanLy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblQuanLy.Click += new System.EventHandler(this.lblQuanLy_Click);
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.SeaShell;
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1520, 881);
            this.panelMain.TabIndex = 19;
            this.panelMain.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMain_Paint);
            // 
            // splitContainerNavBar
            // 
            this.splitContainerNavBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerNavBar.Location = new System.Drawing.Point(0, 30);
            this.splitContainerNavBar.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainerNavBar.Name = "splitContainerNavBar";
            // 
            // splitContainerNavBar.Panel1
            // 
            this.splitContainerNavBar.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(167)))), ((int)(((byte)(167)))));
            this.splitContainerNavBar.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.splitContainerNavBar.Panel1.Controls.Add(this.btnLogout);
            this.splitContainerNavBar.Panel1.Controls.Add(this.btnTaiKhoan);
            this.splitContainerNavBar.Panel1.Controls.Add(this.lblName);
            this.splitContainerNavBar.Panel1.Controls.Add(this.btnReport);
            this.splitContainerNavBar.Panel1.Controls.Add(this.lblQuanLy);
            this.splitContainerNavBar.Panel1.Controls.Add(this.btnTrangChu);
            this.splitContainerNavBar.Panel1.Controls.Add(this.btnKhachHang);
            this.splitContainerNavBar.Panel1.Controls.Add(this.guna2CirclePictureBox1);
            this.splitContainerNavBar.Panel1.Controls.Add(this.btnEmployer);
            this.splitContainerNavBar.Panel1.Controls.Add(this.btnMenu);
            // 
            // splitContainerNavBar.Panel2
            // 
            this.splitContainerNavBar.Panel2.Controls.Add(this.panelMain);
            this.splitContainerNavBar.Size = new System.Drawing.Size(1688, 881);
            this.splitContainerNavBar.SplitterDistance = 163;
            this.splitContainerNavBar.SplitterWidth = 5;
            this.splitContainerNavBar.TabIndex = 20;
            // 
            // btnLogout
            // 
            this.btnLogout.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLogout.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLogout.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLogout.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLogout.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLogout.FillColor = System.Drawing.Color.Empty;
            this.btnLogout.FillColor2 = System.Drawing.Color.Empty;
            this.btnLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.HoverState.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Image = global::CoffeeManagementSystem.Properties.Resources.logout;
            this.btnLogout.ImageOffset = new System.Drawing.Point(-3, 0);
            this.btnLogout.ImageSize = new System.Drawing.Size(25, 25);
            this.btnLogout.Location = new System.Drawing.Point(0, 801);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(2);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(163, 80);
            this.btnLogout.TabIndex = 20;
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnTaiKhoan
            // 
            this.btnTaiKhoan.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTaiKhoan.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTaiKhoan.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTaiKhoan.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTaiKhoan.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTaiKhoan.FillColor = System.Drawing.Color.Empty;
            this.btnTaiKhoan.FillColor2 = System.Drawing.Color.Empty;
            this.btnTaiKhoan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTaiKhoan.ForeColor = System.Drawing.Color.White;
            this.btnTaiKhoan.HoverState.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTaiKhoan.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnTaiKhoan.Image = global::CoffeeManagementSystem.Properties.Resources.TaiKhoan;
            this.btnTaiKhoan.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnTaiKhoan.ImageSize = new System.Drawing.Size(25, 25);
            this.btnTaiKhoan.Location = new System.Drawing.Point(2, 660);
            this.btnTaiKhoan.Margin = new System.Windows.Forms.Padding(2);
            this.btnTaiKhoan.Name = "btnTaiKhoan";
            this.btnTaiKhoan.Size = new System.Drawing.Size(150, 80);
            this.btnTaiKhoan.TabIndex = 19;
            this.btnTaiKhoan.Text = "Quản lý tài khoản";
            this.btnTaiKhoan.TextOffset = new System.Drawing.Point(5, 0);
            this.btnTaiKhoan.Click += new System.EventHandler(this.btnTaiKhoan_Click);
            // 
            // btnReport
            // 
            this.btnReport.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReport.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnReport.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnReport.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnReport.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnReport.FillColor = System.Drawing.Color.Empty;
            this.btnReport.FillColor2 = System.Drawing.Color.Empty;
            this.btnReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReport.ForeColor = System.Drawing.Color.White;
            this.btnReport.HoverState.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReport.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnReport.Image = global::CoffeeManagementSystem.Properties.Resources.BaoCao;
            this.btnReport.ImageOffset = new System.Drawing.Point(0, -15);
            this.btnReport.ImageSize = new System.Drawing.Size(25, 25);
            this.btnReport.Location = new System.Drawing.Point(0, 566);
            this.btnReport.Margin = new System.Windows.Forms.Padding(2);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(161, 80);
            this.btnReport.TabIndex = 16;
            this.btnReport.Text = "Báo Cáo";
            this.btnReport.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnReport.TextOffset = new System.Drawing.Point(24, 10);
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnTrangChu
            // 
            this.btnTrangChu.Animated = true;
            this.btnTrangChu.BackColor = System.Drawing.Color.Transparent;
            this.btnTrangChu.BorderColor = System.Drawing.Color.Empty;
            this.btnTrangChu.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTrangChu.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTrangChu.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTrangChu.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTrangChu.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTrangChu.FillColor = System.Drawing.Color.Empty;
            this.btnTrangChu.FillColor2 = System.Drawing.Color.Empty;
            this.btnTrangChu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrangChu.ForeColor = System.Drawing.Color.White;
            this.btnTrangChu.HoverState.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrangChu.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnTrangChu.Image = global::CoffeeManagementSystem.Properties.Resources.TrangChu;
            this.btnTrangChu.ImageOffset = new System.Drawing.Point(5, -15);
            this.btnTrangChu.ImageSize = new System.Drawing.Size(25, 25);
            this.btnTrangChu.Location = new System.Drawing.Point(0, 198);
            this.btnTrangChu.Margin = new System.Windows.Forms.Padding(0);
            this.btnTrangChu.Name = "btnTrangChu";
            this.btnTrangChu.Size = new System.Drawing.Size(161, 79);
            this.btnTrangChu.TabIndex = 10;
            this.btnTrangChu.Text = "Trang Chủ";
            this.btnTrangChu.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnTrangChu.TextOffset = new System.Drawing.Point(20, 10);
            this.btnTrangChu.UseTransparentBackground = true;
            this.btnTrangChu.Click += new System.EventHandler(this.btnTrangChu_Click_1);
            // 
            // btnKhachHang
            // 
            this.btnKhachHang.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnKhachHang.BorderColor = System.Drawing.Color.Empty;
            this.btnKhachHang.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnKhachHang.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnKhachHang.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnKhachHang.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnKhachHang.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnKhachHang.FillColor = System.Drawing.Color.Empty;
            this.btnKhachHang.FillColor2 = System.Drawing.Color.Empty;
            this.btnKhachHang.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKhachHang.ForeColor = System.Drawing.Color.White;
            this.btnKhachHang.HoverState.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKhachHang.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnKhachHang.Image = global::CoffeeManagementSystem.Properties.Resources.KhachHang;
            this.btnKhachHang.ImageOffset = new System.Drawing.Point(3, -15);
            this.btnKhachHang.ImageSize = new System.Drawing.Size(25, 25);
            this.btnKhachHang.Location = new System.Drawing.Point(-2, 289);
            this.btnKhachHang.Margin = new System.Windows.Forms.Padding(2);
            this.btnKhachHang.Name = "btnKhachHang";
            this.btnKhachHang.Size = new System.Drawing.Size(163, 80);
            this.btnKhachHang.TabIndex = 11;
            this.btnKhachHang.Text = "Khách Hàng";
            this.btnKhachHang.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnKhachHang.TextOffset = new System.Drawing.Point(17, 10);
            this.btnKhachHang.Click += new System.EventHandler(this.btnKhachHang_Click);
            // 
            // guna2CirclePictureBox1
            // 
            this.guna2CirclePictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("guna2CirclePictureBox1.Image")));
            this.guna2CirclePictureBox1.ImageRotate = 0F;
            this.guna2CirclePictureBox1.Location = new System.Drawing.Point(53, 17);
            this.guna2CirclePictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.guna2CirclePictureBox1.Name = "guna2CirclePictureBox1";
            this.guna2CirclePictureBox1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CirclePictureBox1.Size = new System.Drawing.Size(81, 81);
            this.guna2CirclePictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2CirclePictureBox1.TabIndex = 13;
            this.guna2CirclePictureBox1.TabStop = false;
            // 
            // btnEmployer
            // 
            this.btnEmployer.BorderColor = System.Drawing.Color.Empty;
            this.btnEmployer.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployer.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployer.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEmployer.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEmployer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEmployer.FillColor = System.Drawing.Color.Empty;
            this.btnEmployer.FillColor2 = System.Drawing.Color.Empty;
            this.btnEmployer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmployer.ForeColor = System.Drawing.Color.White;
            this.btnEmployer.HoverState.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmployer.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnEmployer.Image = global::CoffeeManagementSystem.Properties.Resources.NhanVien;
            this.btnEmployer.ImageOffset = new System.Drawing.Point(3, -15);
            this.btnEmployer.ImageSize = new System.Drawing.Size(25, 25);
            this.btnEmployer.Location = new System.Drawing.Point(-2, 383);
            this.btnEmployer.Margin = new System.Windows.Forms.Padding(2);
            this.btnEmployer.Name = "btnEmployer";
            this.btnEmployer.Size = new System.Drawing.Size(163, 72);
            this.btnEmployer.TabIndex = 14;
            this.btnEmployer.Text = "Nhân Viên";
            this.btnEmployer.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnEmployer.TextOffset = new System.Drawing.Point(22, 10);
            this.btnEmployer.Click += new System.EventHandler(this.btnEmployer_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnMenu.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnMenu.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMenu.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMenu.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnMenu.FillColor = System.Drawing.Color.Empty;
            this.btnMenu.FillColor2 = System.Drawing.Color.Empty;
            this.btnMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu.ForeColor = System.Drawing.Color.White;
            this.btnMenu.HoverState.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnMenu.Image = global::CoffeeManagementSystem.Properties.Resources.DoUong;
            this.btnMenu.ImageOffset = new System.Drawing.Point(0, -15);
            this.btnMenu.ImageSize = new System.Drawing.Size(25, 25);
            this.btnMenu.Location = new System.Drawing.Point(0, 478);
            this.btnMenu.Margin = new System.Windows.Forms.Padding(2);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(163, 74);
            this.btnMenu.TabIndex = 13;
            this.btnMenu.Text = "Đồ uống";
            this.btnMenu.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnMenu.TextOffset = new System.Drawing.Point(23, 10);
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // pnlTitleBar
            // 
            this.pnlTitleBar.BackColor = System.Drawing.Color.SeaShell;
            this.pnlTitleBar.Controls.Add(this.label1);
            this.pnlTitleBar.Controls.Add(this.guna2ControlBox3);
            this.pnlTitleBar.Controls.Add(this.guna2ControlBox2);
            this.pnlTitleBar.Controls.Add(this.guna2ControlBox1);
            this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleBar.FillColor = System.Drawing.Color.SeaShell;
            this.pnlTitleBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTitleBar.Name = "pnlTitleBar";
            this.pnlTitleBar.Size = new System.Drawing.Size(1688, 30);
            this.pnlTitleBar.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Ứng dụng quản lý quán cafe";
            // 
            // guna2ControlBox3
            // 
            this.guna2ControlBox3.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.guna2ControlBox3.Dock = System.Windows.Forms.DockStyle.Right;
            this.guna2ControlBox3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(167)))), ((int)(((byte)(167)))));
            this.guna2ControlBox3.ForeColor = System.Drawing.Color.Black;
            this.guna2ControlBox3.HoverState.FillColor = System.Drawing.Color.DimGray;
            this.guna2ControlBox3.HoverState.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox3.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox3.Location = new System.Drawing.Point(1583, 0);
            this.guna2ControlBox3.Name = "guna2ControlBox3";
            this.guna2ControlBox3.Size = new System.Drawing.Size(35, 30);
            this.guna2ControlBox3.TabIndex = 5;
            // 
            // guna2ControlBox2
            // 
            this.guna2ControlBox2.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MaximizeBox;
            this.guna2ControlBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.guna2ControlBox2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(167)))), ((int)(((byte)(167)))));
            this.guna2ControlBox2.ForeColor = System.Drawing.Color.Black;
            this.guna2ControlBox2.HoverState.FillColor = System.Drawing.Color.DimGray;
            this.guna2ControlBox2.HoverState.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox2.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox2.Location = new System.Drawing.Point(1618, 0);
            this.guna2ControlBox2.Name = "guna2ControlBox2";
            this.guna2ControlBox2.Size = new System.Drawing.Size(35, 30);
            this.guna2ControlBox2.TabIndex = 4;
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.guna2ControlBox1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(167)))), ((int)(((byte)(167)))));
            this.guna2ControlBox1.HoverState.FillColor = System.Drawing.Color.DimGray;
            this.guna2ControlBox1.HoverState.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox1.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox1.Location = new System.Drawing.Point(1653, 0);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.Size = new System.Drawing.Size(35, 30);
            this.guna2ControlBox1.TabIndex = 3;
            // 
            // guna2ShadowForm1
            // 
            this.guna2ShadowForm1.BorderRadius = 20;
            this.guna2ShadowForm1.TargetForm = this;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1688, 911);
            this.Controls.Add(this.splitContainerNavBar);
            this.Controls.Add(this.pnlTitleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(732, 452);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Coffee Management System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainerNavBar.Panel1.ResumeLayout(false);
            this.splitContainerNavBar.Panel1.PerformLayout();
            this.splitContainerNavBar.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerNavBar)).EndInit();
            this.splitContainerNavBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).EndInit();
            this.pnlTitleBar.ResumeLayout(false);
            this.pnlTitleBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2GradientButton btnTrangChu;
        private Guna.UI2.WinForms.Guna2GradientButton btnEmployer;
        private Guna.UI2.WinForms.Guna2GradientButton btnMenu;
        private Guna.UI2.WinForms.Guna2GradientButton btnKhachHang;
        private Guna.UI2.WinForms.Guna2CirclePictureBox guna2CirclePictureBox1;
        private Guna.UI2.WinForms.Guna2GradientButton btnReport;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblQuanLy;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.SplitContainer splitContainerNavBar;
        private Guna.UI2.WinForms.Guna2GradientButton btnTaiKhoan;
        private Guna.UI2.WinForms.Guna2GradientButton btnLogout;
        private Guna.UI2.WinForms.Guna2Panel pnlTitleBar;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox3;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox2;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private Guna.UI2.WinForms.Guna2ShadowForm guna2ShadowForm1;
    }
}