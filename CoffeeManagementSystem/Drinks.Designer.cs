namespace CoffeeManagementSystem
{
    partial class DrinkForm
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
        /// 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrinkForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvDouong = new Guna.UI2.WinForms.Guna2DataGridView();
            this.MaNV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ten = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GioiTinh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiaChi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.txtTimkiemloaidouong = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnThemloaidouong = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvLoaidouong = new Guna.UI2.WinForms.Guna2DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTimkiemdouong = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnThemdouong = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDouong)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoaidouong)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvDouong
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvDouong.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDouong.BackgroundColor = System.Drawing.Color.SeaShell;
            this.dgvDouong.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.DarkCyan;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DarkCyan;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDouong.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDouong.ColumnHeadersHeight = 40;
            this.dgvDouong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvDouong.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MaNV,
            this.Ten,
            this.GioiTinh,
            this.Column1,
            this.DiaChi});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDouong.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvDouong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDouong.GridColor = System.Drawing.Color.IndianRed;
            this.dgvDouong.Location = new System.Drawing.Point(3, 1);
            this.dgvDouong.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.dgvDouong.Name = "dgvDouong";
            this.dgvDouong.RowHeadersVisible = false;
            this.dgvDouong.RowHeadersWidth = 51;
            this.dgvDouong.RowTemplate.Height = 24;
            this.dgvDouong.Size = new System.Drawing.Size(1218, 588);
            this.dgvDouong.TabIndex = 16;
            this.dgvDouong.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDouong.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvDouong.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvDouong.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvDouong.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvDouong.ThemeStyle.BackColor = System.Drawing.Color.SeaShell;
            this.dgvDouong.ThemeStyle.GridColor = System.Drawing.Color.IndianRed;
            this.dgvDouong.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvDouong.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvDouong.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDouong.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvDouong.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvDouong.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvDouong.ThemeStyle.ReadOnly = false;
            this.dgvDouong.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDouong.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvDouong.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDouong.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvDouong.ThemeStyle.RowsStyle.Height = 24;
            this.dgvDouong.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvDouong.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // MaNV
            // 
            this.MaNV.DataPropertyName = "Madouong";
            this.MaNV.HeaderText = "Mã Đồ Uống";
            this.MaNV.MinimumWidth = 6;
            this.MaNV.Name = "MaNV";
            // 
            // Ten
            // 
            this.Ten.DataPropertyName = "Tendouong";
            this.Ten.HeaderText = "Tên Đồ Uống";
            this.Ten.MinimumWidth = 6;
            this.Ten.Name = "Ten";
            // 
            // GioiTinh
            // 
            this.GioiTinh.DataPropertyName = "Maloai";
            this.GioiTinh.HeaderText = "Loại Đồ Uống";
            this.GioiTinh.MinimumWidth = 6;
            this.GioiTinh.Name = "GioiTinh";
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "CurrentGia";
            this.Column1.HeaderText = "Giá Bán";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            // 
            // DiaChi
            // 
            this.DiaChi.DataPropertyName = "Mota";
            this.DiaChi.HeaderText = "Mô Tả";
            this.DiaChi.MinimumWidth = 6;
            this.DiaChi.Name = "DiaChi";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("SVN-Gilroy", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1232, 630);
            this.tabControl1.TabIndex = 24;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.dgvLoaidouong);
            this.tabPage1.Location = new System.Drawing.Point(4, 36);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tabPage1.Size = new System.Drawing.Size(1224, 590);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Loại đồ uống";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkCyan;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.txtTimkiemloaidouong);
            this.panel1.Controls.Add(this.btnThemloaidouong);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 478);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1218, 111);
            this.panel1.TabIndex = 26;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackColor = System.Drawing.Color.Tomato;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Segoe UI Variable Small", 10.75F, System.Drawing.FontStyle.Bold);
            this.button3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button3.Location = new System.Drawing.Point(917, 18);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(149, 79);
            this.button3.TabIndex = 105;
            this.button3.Text = "Tải lên tệp";
            this.button3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtTimkiemloaidouong
            // 
            this.txtTimkiemloaidouong.AutoSize = true;
            this.txtTimkiemloaidouong.BackColor = System.Drawing.Color.Transparent;
            this.txtTimkiemloaidouong.BorderRadius = 15;
            this.txtTimkiemloaidouong.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimkiemloaidouong.DefaultText = "";
            this.txtTimkiemloaidouong.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTimkiemloaidouong.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTimkiemloaidouong.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTimkiemloaidouong.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTimkiemloaidouong.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTimkiemloaidouong.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimkiemloaidouong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtTimkiemloaidouong.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTimkiemloaidouong.Location = new System.Drawing.Point(177, 26);
            this.txtTimkiemloaidouong.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtTimkiemloaidouong.Name = "txtTimkiemloaidouong";
            this.txtTimkiemloaidouong.PlaceholderText = "";
            this.txtTimkiemloaidouong.SelectedText = "";
            this.txtTimkiemloaidouong.ShadowDecoration.BorderRadius = 15;
            this.txtTimkiemloaidouong.ShadowDecoration.Depth = 5;
            this.txtTimkiemloaidouong.ShadowDecoration.Enabled = true;
            this.txtTimkiemloaidouong.Size = new System.Drawing.Size(641, 53);
            this.txtTimkiemloaidouong.TabIndex = 104;
            this.txtTimkiemloaidouong.TextChanged += new System.EventHandler(this.txtTimkiemloaidouong_TextChanged);
            // 
            // btnThemloaidouong
            // 
            this.btnThemloaidouong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThemloaidouong.BackColor = System.Drawing.Color.DarkCyan;
            this.btnThemloaidouong.FlatAppearance.BorderSize = 0;
            this.btnThemloaidouong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemloaidouong.Font = new System.Drawing.Font("Segoe UI Variable Small", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemloaidouong.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnThemloaidouong.Image = ((System.Drawing.Image)(resources.GetObject("btnThemloaidouong.Image")));
            this.btnThemloaidouong.Location = new System.Drawing.Point(1074, 17);
            this.btnThemloaidouong.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnThemloaidouong.Name = "btnThemloaidouong";
            this.btnThemloaidouong.Size = new System.Drawing.Size(127, 80);
            this.btnThemloaidouong.TabIndex = 23;
            this.btnThemloaidouong.Text = "Thêm";
            this.btnThemloaidouong.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnThemloaidouong.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnThemloaidouong.UseVisualStyleBackColor = false;
            this.btnThemloaidouong.Click += new System.EventHandler(this.btnThemloaidouong_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Snow;
            this.label1.Location = new System.Drawing.Point(4, 41);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 31);
            this.label1.TabIndex = 22;
            this.label1.Text = "Tìm kiếm:";
            // 
            // dgvLoaidouong
            // 
            this.dgvLoaidouong.AllowUserToResizeColumns = false;
            this.dgvLoaidouong.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgvLoaidouong.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvLoaidouong.BackgroundColor = System.Drawing.Color.SeaShell;
            this.dgvLoaidouong.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.DarkCyan;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("SVN-Gilroy", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.DarkCyan;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLoaidouong.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvLoaidouong.ColumnHeadersHeight = 40;
            this.dgvLoaidouong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvLoaidouong.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn3});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("SVN-Gilroy", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLoaidouong.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvLoaidouong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLoaidouong.GridColor = System.Drawing.Color.Firebrick;
            this.dgvLoaidouong.Location = new System.Drawing.Point(3, 1);
            this.dgvLoaidouong.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.dgvLoaidouong.Name = "dgvLoaidouong";
            this.dgvLoaidouong.RowHeadersVisible = false;
            this.dgvLoaidouong.RowHeadersWidth = 51;
            this.dgvLoaidouong.RowTemplate.Height = 24;
            this.dgvLoaidouong.Size = new System.Drawing.Size(1218, 588);
            this.dgvLoaidouong.TabIndex = 17;
            this.dgvLoaidouong.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvLoaidouong.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvLoaidouong.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvLoaidouong.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvLoaidouong.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvLoaidouong.ThemeStyle.BackColor = System.Drawing.Color.SeaShell;
            this.dgvLoaidouong.ThemeStyle.GridColor = System.Drawing.Color.Firebrick;
            this.dgvLoaidouong.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvLoaidouong.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvLoaidouong.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvLoaidouong.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvLoaidouong.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvLoaidouong.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvLoaidouong.ThemeStyle.ReadOnly = false;
            this.dgvLoaidouong.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvLoaidouong.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLoaidouong.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvLoaidouong.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvLoaidouong.ThemeStyle.RowsStyle.Height = 24;
            this.dgvLoaidouong.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvLoaidouong.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Maloai";
            this.dataGridViewTextBoxColumn1.HeaderText = "Mã Loại";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Tenloai";
            this.dataGridViewTextBoxColumn3.HeaderText = "Tên Loại";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Controls.Add(this.dgvDouong);
            this.tabPage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage2.Location = new System.Drawing.Point(4, 36);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tabPage2.Size = new System.Drawing.Size(1224, 590);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Đồ uống";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DarkCyan;
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtTimkiemdouong);
            this.panel2.Controls.Add(this.btnThemdouong);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 478);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1218, 111);
            this.panel2.TabIndex = 26;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.Tomato;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI Variable Small", 10.75F, System.Drawing.FontStyle.Bold);
            this.button2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button2.Location = new System.Drawing.Point(912, 17);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(144, 80);
            this.button2.TabIndex = 25;
            this.button2.Text = "Tải lên tệp";
            this.button2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SVN-Gilroy", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(16, 41);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 36);
            this.label2.TabIndex = 106;
            this.label2.Text = "Tìm kiếm:";
            // 
            // txtTimkiemdouong
            // 
            this.txtTimkiemdouong.AutoSize = true;
            this.txtTimkiemdouong.BackColor = System.Drawing.Color.Transparent;
            this.txtTimkiemdouong.BorderRadius = 15;
            this.txtTimkiemdouong.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimkiemdouong.DefaultText = "";
            this.txtTimkiemdouong.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTimkiemdouong.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTimkiemdouong.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTimkiemdouong.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTimkiemdouong.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTimkiemdouong.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimkiemdouong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtTimkiemdouong.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTimkiemdouong.Location = new System.Drawing.Point(199, 30);
            this.txtTimkiemdouong.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtTimkiemdouong.Name = "txtTimkiemdouong";
            this.txtTimkiemdouong.PlaceholderText = "";
            this.txtTimkiemdouong.SelectedText = "";
            this.txtTimkiemdouong.ShadowDecoration.BorderRadius = 15;
            this.txtTimkiemdouong.ShadowDecoration.Depth = 5;
            this.txtTimkiemdouong.ShadowDecoration.Enabled = true;
            this.txtTimkiemdouong.Size = new System.Drawing.Size(571, 53);
            this.txtTimkiemdouong.TabIndex = 105;
            this.txtTimkiemdouong.TextChanged += new System.EventHandler(this.txtTimkiemdouong_TextChanged);
            // 
            // btnThemdouong
            // 
            this.btnThemdouong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThemdouong.BackColor = System.Drawing.Color.DarkCyan;
            this.btnThemdouong.FlatAppearance.BorderSize = 0;
            this.btnThemdouong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemdouong.Font = new System.Drawing.Font("Segoe UI Variable Small", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemdouong.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnThemdouong.Image = ((System.Drawing.Image)(resources.GetObject("btnThemdouong.Image")));
            this.btnThemdouong.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnThemdouong.Location = new System.Drawing.Point(1073, 17);
            this.btnThemdouong.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnThemdouong.Name = "btnThemdouong";
            this.btnThemdouong.Size = new System.Drawing.Size(127, 80);
            this.btnThemdouong.TabIndex = 23;
            this.btnThemdouong.Text = "Thêm";
            this.btnThemdouong.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnThemdouong.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnThemdouong.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            // 
            // DrinkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(1232, 630);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.Name = "DrinkForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Drinks";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDouong)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoaidouong)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dgvDouong;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Guna.UI2.WinForms.Guna2DataGridView dgvLoaidouong;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnThemloaidouong;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnThemdouong;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaNV;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ten;
        private System.Windows.Forms.DataGridViewTextBoxColumn GioiTinh;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiaChi;
        private Guna.UI2.WinForms.Guna2TextBox txtTimkiemloaidouong;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2TextBox txtTimkiemdouong;
    }
}