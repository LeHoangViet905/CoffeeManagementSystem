using System;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    partial class OrderDetailForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.components = new System.ComponentModel.Container();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.lblThoiGian = new System.Windows.Forms.Label();
            this.lblMaDon = new System.Windows.Forms.Label();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.groupBoxOrderLines = new System.Windows.Forms.GroupBox();
            this.dgvLines = new System.Windows.Forms.DataGridView();
            this.colStt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenDoUong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelRight = new System.Windows.Forms.Panel();
            this.groupBoxPayment = new System.Windows.Forms.GroupBox();
            this.lblNhanVienThu = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblSoTienThanhToan = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblThoiGianThanhToan = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblHinhThucThanhToan = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBoxCustomer = new System.Windows.Forms.GroupBox();
            this.lblDiemTichLuy = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblSdt = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTenKhach = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.lblNhanVienLap = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.panelHeader.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.groupBoxOrderLines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLines)).BeginInit();
            this.panelRight.SuspendLayout();
            this.groupBoxPayment.SuspendLayout();
            this.groupBoxCustomer.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.DarkCyan;
            this.panelHeader.Controls.Add(this.lblTrangThai);
            this.panelHeader.Controls.Add(this.lblThoiGian);
            this.panelHeader.Controls.Add(this.lblMaDon);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1058, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new System.Drawing.Font("SVN-Gilroy", 9.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrangThai.ForeColor = System.Drawing.Color.Honeydew;
            this.lblTrangThai.Location = new System.Drawing.Point(903, 18);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(132, 23);
            this.lblTrangThai.TabIndex = 2;
            this.lblTrangThai.Text = "Hoàn thành ✔";
            // 
            // lblThoiGian
            // 
            this.lblThoiGian.AutoSize = true;
            this.lblThoiGian.Font = new System.Drawing.Font("SVN-Gilroy", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThoiGian.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblThoiGian.Location = new System.Drawing.Point(14, 32);
            this.lblThoiGian.Name = "lblThoiGian";
            this.lblThoiGian.Size = new System.Drawing.Size(143, 22);
            this.lblThoiGian.TabIndex = 1;
            this.lblThoiGian.Text = "12/12/2025 10:00";
            // 
            // lblMaDon
            // 
            this.lblMaDon.AutoSize = true;
            this.lblMaDon.Font = new System.Drawing.Font("SVN-Gilroy", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaDon.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblMaDon.Location = new System.Drawing.Point(12, 0);
            this.lblMaDon.Name = "lblMaDon";
            this.lblMaDon.Size = new System.Drawing.Size(132, 32);
            this.lblMaDon.TabIndex = 0;
            this.lblMaDon.Text = "HĐ000001";
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.groupBoxOrderLines);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeft.Location = new System.Drawing.Point(0, 60);
            this.panelLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.panelLeft.Size = new System.Drawing.Size(765, 391);
            this.panelLeft.TabIndex = 1;
            // 
            // groupBoxOrderLines
            // 
            this.groupBoxOrderLines.Controls.Add(this.dgvLines);
            this.groupBoxOrderLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxOrderLines.Font = new System.Drawing.Font("SVN-Gilroy", 9.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxOrderLines.Location = new System.Drawing.Point(11, 10);
            this.groupBoxOrderLines.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxOrderLines.Name = "groupBoxOrderLines";
            this.groupBoxOrderLines.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxOrderLines.Size = new System.Drawing.Size(743, 371);
            this.groupBoxOrderLines.TabIndex = 0;
            this.groupBoxOrderLines.TabStop = false;
            this.groupBoxOrderLines.Text = "Chi tiết đơn hàng";
            // 
            // dgvLines
            // 
            this.dgvLines.AllowUserToAddRows = false;
            this.dgvLines.AllowUserToDeleteRows = false;
            this.dgvLines.AllowUserToResizeRows = false;
            this.dgvLines.BackgroundColor = System.Drawing.Color.White;
            this.dgvLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStt,
            this.colTenDoUong,
            this.colSoLuong,
            this.colDonGia,
            this.colThanhTien});
            this.dgvLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLines.Location = new System.Drawing.Point(3, 25);
            this.dgvLines.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvLines.MultiSelect = false;
            this.dgvLines.Name = "dgvLines";
            this.dgvLines.ReadOnly = true;
            this.dgvLines.RowHeadersVisible = false;
            this.dgvLines.RowHeadersWidth = 62;
            this.dgvLines.RowTemplate.Height = 28;
            this.dgvLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLines.Size = new System.Drawing.Size(737, 344);
            this.dgvLines.TabIndex = 0;
            // 
            // colStt
            // 
            this.colStt.HeaderText = "STT";
            this.colStt.MinimumWidth = 50;
            this.colStt.Name = "colStt";
            this.colStt.ReadOnly = true;
            this.colStt.Width = 150;
            // 
            // colTenDoUong
            // 
            this.colTenDoUong.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTenDoUong.HeaderText = "Đồ uống";
            this.colTenDoUong.MinimumWidth = 100;
            this.colTenDoUong.Name = "colTenDoUong";
            this.colTenDoUong.ReadOnly = true;
            // 
            // colSoLuong
            // 
            this.colSoLuong.HeaderText = "SL";
            this.colSoLuong.MinimumWidth = 60;
            this.colSoLuong.Name = "colSoLuong";
            this.colSoLuong.ReadOnly = true;
            this.colSoLuong.Width = 60;
            // 
            // colDonGia
            // 
            this.colDonGia.HeaderText = "Đơn giá";
            this.colDonGia.MinimumWidth = 90;
            this.colDonGia.Name = "colDonGia";
            this.colDonGia.ReadOnly = true;
            this.colDonGia.Width = 90;
            // 
            // colThanhTien
            // 
            this.colThanhTien.HeaderText = "Thành tiền";
            this.colThanhTien.MinimumWidth = 110;
            this.colThanhTien.Name = "colThanhTien";
            this.colThanhTien.ReadOnly = true;
            this.colThanhTien.Width = 110;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.groupBoxPayment);
            this.panelRight.Controls.Add(this.groupBoxCustomer);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(765, 60);
            this.panelRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelRight.Name = "panelRight";
            this.panelRight.Padding = new System.Windows.Forms.Padding(5, 10, 11, 10);
            this.panelRight.Size = new System.Drawing.Size(293, 391);
            this.panelRight.TabIndex = 2;
            // 
            // groupBoxPayment
            // 
            this.groupBoxPayment.Controls.Add(this.lblNhanVienThu);
            this.groupBoxPayment.Controls.Add(this.label15);
            this.groupBoxPayment.Controls.Add(this.lblSoTienThanhToan);
            this.groupBoxPayment.Controls.Add(this.label13);
            this.groupBoxPayment.Controls.Add(this.lblThoiGianThanhToan);
            this.groupBoxPayment.Controls.Add(this.label11);
            this.groupBoxPayment.Controls.Add(this.lblHinhThucThanhToan);
            this.groupBoxPayment.Controls.Add(this.label9);
            this.groupBoxPayment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPayment.Font = new System.Drawing.Font("SVN-Gilroy", 9.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxPayment.Location = new System.Drawing.Point(5, 184);
            this.groupBoxPayment.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxPayment.Name = "groupBoxPayment";
            this.groupBoxPayment.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxPayment.Size = new System.Drawing.Size(277, 197);
            this.groupBoxPayment.TabIndex = 1;
            this.groupBoxPayment.TabStop = false;
            this.groupBoxPayment.Text = "Thanh toán";
            // 
            // lblNhanVienThu
            // 
            this.lblNhanVienThu.AutoSize = true;
            this.lblNhanVienThu.Font = new System.Drawing.Font("SVN-Gilroy", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNhanVienThu.Location = new System.Drawing.Point(174, 142);
            this.lblNhanVienThu.Name = "lblNhanVienThu";
            this.lblNhanVienThu.Size = new System.Drawing.Size(33, 20);
            this.lblNhanVienThu.TabIndex = 7;
            this.lblNhanVienThu.Text = "---";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("SVN-Gilroy", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(14, 140);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(106, 20);
            this.label15.TabIndex = 6;
            this.label15.Text = "Nhân viên thu:";
            // 
            // lblSoTienThanhToan
            // 
            this.lblSoTienThanhToan.AutoSize = true;
            this.lblSoTienThanhToan.Font = new System.Drawing.Font("SVN-Gilroy SemiBold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSoTienThanhToan.ForeColor = System.Drawing.Color.Tomato;
            this.lblSoTienThanhToan.Location = new System.Drawing.Point(174, 107);
            this.lblSoTienThanhToan.Name = "lblSoTienThanhToan";
            this.lblSoTienThanhToan.Size = new System.Drawing.Size(53, 20);
            this.lblSoTienThanhToan.TabIndex = 5;
            this.lblSoTienThanhToan.Text = "0 VNĐ";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("SVN-Gilroy", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(14, 107);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(141, 20);
            this.label13.TabIndex = 4;
            this.label13.Text = "Số tiền thanh toán:";
            // 
            // lblThoiGianThanhToan
            // 
            this.lblThoiGianThanhToan.AutoSize = true;
            this.lblThoiGianThanhToan.Font = new System.Drawing.Font("SVN-Gilroy", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThoiGianThanhToan.Location = new System.Drawing.Point(174, 73);
            this.lblThoiGianThanhToan.Name = "lblThoiGianThanhToan";
            this.lblThoiGianThanhToan.Size = new System.Drawing.Size(33, 20);
            this.lblThoiGianThanhToan.TabIndex = 3;
            this.lblThoiGianThanhToan.Text = "---";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("SVN-Gilroy", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(14, 71);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 20);
            this.label11.TabIndex = 2;
            this.label11.Text = "Thời gian TT:";
            // 
            // lblHinhThucThanhToan
            // 
            this.lblHinhThucThanhToan.AutoSize = true;
            this.lblHinhThucThanhToan.Font = new System.Drawing.Font("SVN-Gilroy", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHinhThucThanhToan.Location = new System.Drawing.Point(174, 41);
            this.lblHinhThucThanhToan.Name = "lblHinhThucThanhToan";
            this.lblHinhThucThanhToan.Size = new System.Drawing.Size(33, 20);
            this.lblHinhThucThanhToan.TabIndex = 1;
            this.lblHinhThucThanhToan.Text = "---";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("SVN-Gilroy", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(14, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 20);
            this.label9.TabIndex = 0;
            this.label9.Text = "Hình thức TT:";
            // 
            // groupBoxCustomer
            // 
            this.groupBoxCustomer.Controls.Add(this.lblDiemTichLuy);
            this.groupBoxCustomer.Controls.Add(this.label8);
            this.groupBoxCustomer.Controls.Add(this.lblEmail);
            this.groupBoxCustomer.Controls.Add(this.label6);
            this.groupBoxCustomer.Controls.Add(this.lblSdt);
            this.groupBoxCustomer.Controls.Add(this.label4);
            this.groupBoxCustomer.Controls.Add(this.lblTenKhach);
            this.groupBoxCustomer.Controls.Add(this.label2);
            this.groupBoxCustomer.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustomer.Font = new System.Drawing.Font("SVN-Gilroy", 9.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxCustomer.Location = new System.Drawing.Point(5, 10);
            this.groupBoxCustomer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxCustomer.Name = "groupBoxCustomer";
            this.groupBoxCustomer.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxCustomer.Size = new System.Drawing.Size(277, 174);
            this.groupBoxCustomer.TabIndex = 0;
            this.groupBoxCustomer.TabStop = false;
            this.groupBoxCustomer.Text = "Khách hàng";
            // 
            // lblDiemTichLuy
            // 
            this.lblDiemTichLuy.AutoSize = true;
            this.lblDiemTichLuy.Font = new System.Drawing.Font("SVN-Gilroy", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiemTichLuy.Location = new System.Drawing.Point(174, 140);
            this.lblDiemTichLuy.Name = "lblDiemTichLuy";
            this.lblDiemTichLuy.Size = new System.Drawing.Size(18, 20);
            this.lblDiemTichLuy.TabIndex = 7;
            this.lblDiemTichLuy.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("SVN-Gilroy", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(14, 140);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 20);
            this.label8.TabIndex = 6;
            this.label8.Text = "Điểm tích lũy:";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("SVN-Gilroy", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(174, 105);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(33, 20);
            this.lblEmail.TabIndex = 5;
            this.lblEmail.Text = "---";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("SVN-Gilroy", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(14, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 20);
            this.label6.TabIndex = 4;
            this.label6.Text = "Email:";
            // 
            // lblSdt
            // 
            this.lblSdt.AutoSize = true;
            this.lblSdt.Font = new System.Drawing.Font("SVN-Gilroy", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSdt.Location = new System.Drawing.Point(174, 71);
            this.lblSdt.Name = "lblSdt";
            this.lblSdt.Size = new System.Drawing.Size(33, 20);
            this.lblSdt.TabIndex = 3;
            this.lblSdt.Text = "---";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SVN-Gilroy", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(14, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Điện thoại:";
            // 
            // lblTenKhach
            // 
            this.lblTenKhach.AutoSize = true;
            this.lblTenKhach.Font = new System.Drawing.Font("SVN-Gilroy", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenKhach.Location = new System.Drawing.Point(174, 39);
            this.lblTenKhach.Name = "lblTenKhach";
            this.lblTenKhach.Size = new System.Drawing.Size(68, 20);
            this.lblTenKhach.TabIndex = 1;
            this.lblTenKhach.Text = "Khách lẻ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SVN-Gilroy", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Khách hàng:";
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelBottom.Controls.Add(this.lblTongTien);
            this.panelBottom.Controls.Add(this.lblNhanVienLap);
            this.panelBottom.Controls.Add(this.btnClose);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 451);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1058, 50);
            this.panelBottom.TabIndex = 3;
            // 
            // lblTongTien
            // 
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("SVN-Gilroy SemiBold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongTien.ForeColor = System.Drawing.Color.Tomato;
            this.lblTongTien.Location = new System.Drawing.Point(12, 14);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(118, 24);
            this.lblTongTien.TabIndex = 2;
            this.lblTongTien.Text = "Tổng cộng: 0";
            // 
            // lblNhanVienLap
            // 
            this.lblNhanVienLap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNhanVienLap.AutoSize = true;
            this.lblNhanVienLap.Font = new System.Drawing.Font("SVN-Gilroy SemiBold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNhanVienLap.Location = new System.Drawing.Point(382, 14);
            this.lblNhanVienLap.Name = "lblNhanVienLap";
            this.lblNhanVienLap.Size = new System.Drawing.Size(126, 24);
            this.lblNhanVienLap.TabIndex = 1;
            this.lblNhanVienLap.Text = "NV lập: NV001";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Tomato;
            this.btnClose.Font = new System.Drawing.Font("SVN-Gilroy", 9.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(882, 2);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(173, 48);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.BorderRadius = 30;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // OrderDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1058, 501);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chi tiết đơn hàng";
            this.Load += new System.EventHandler(this.OrderDetailForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.groupBoxOrderLines.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLines)).EndInit();
            this.panelRight.ResumeLayout(false);
            this.groupBoxPayment.ResumeLayout(false);
            this.groupBoxPayment.PerformLayout();
            this.groupBoxCustomer.ResumeLayout(false);
            this.groupBoxCustomer.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblMaDon;
        private System.Windows.Forms.Label lblThoiGian;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.GroupBox groupBoxOrderLines;
        private System.Windows.Forms.DataGridView dgvLines;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.GroupBox groupBoxPayment;
        private System.Windows.Forms.Label lblNhanVienThu;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblSoTienThanhToan;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblThoiGianThanhToan;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblHinhThucThanhToan;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBoxCustomer;
        private System.Windows.Forms.Label lblDiemTichLuy;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblSdt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTenKhach;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Label lblNhanVienLap;
        private DataGridViewTextBoxColumn colStt;
        private DataGridViewTextBoxColumn colTenDoUong;
        private DataGridViewTextBoxColumn colSoLuong;
        private DataGridViewTextBoxColumn colDonGia;
        private DataGridViewTextBoxColumn colThanhTien;
        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
    }
}
