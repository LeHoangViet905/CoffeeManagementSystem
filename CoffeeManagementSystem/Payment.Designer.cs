namespace CoffeeManagementSystem
{
    partial class PaymentForm
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
            this.lvwChiTietHoaDon = new System.Windows.Forms.ListView();
            this.STT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TenDouong = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Soluong = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Gia = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ThanhTien = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtTongThanhTienValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblHoaDon = new System.Windows.Forms.Label();
            this.lblTen = new System.Windows.Forms.Label();
            this.lblNguoiLapValue = new System.Windows.Forms.Label();
            this.lblNgayValue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMaHoaDon = new System.Windows.Forms.Label();
            this.lblMaHoaDonValue = new System.Windows.Forms.Label();
            this.btnThanhToan = new System.Windows.Forms.Button();
            this.txtKhachHangName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lvwChiTietHoaDon
            // 
            this.lvwChiTietHoaDon.BackColor = System.Drawing.Color.DimGray;
            this.lvwChiTietHoaDon.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.STT,
            this.TenDouong,
            this.Soluong,
            this.Gia,
            this.ThanhTien});
            this.lvwChiTietHoaDon.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvwChiTietHoaDon.ForeColor = System.Drawing.Color.SeaShell;
            this.lvwChiTietHoaDon.FullRowSelect = true;
            this.lvwChiTietHoaDon.GridLines = true;
            this.lvwChiTietHoaDon.HideSelection = false;
            this.lvwChiTietHoaDon.Location = new System.Drawing.Point(11, 148);
            this.lvwChiTietHoaDon.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lvwChiTietHoaDon.Name = "lvwChiTietHoaDon";
            this.lvwChiTietHoaDon.Size = new System.Drawing.Size(884, 282);
            this.lvwChiTietHoaDon.TabIndex = 11;
            this.lvwChiTietHoaDon.UseCompatibleStateImageBehavior = false;
            this.lvwChiTietHoaDon.View = System.Windows.Forms.View.Details;
            // 
            // STT
            // 
            this.STT.Text = "STT";
            // 
            // TenDouong
            // 
            this.TenDouong.Text = "Tên đồ uống";
            this.TenDouong.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TenDouong.Width = 200;
            // 
            // Soluong
            // 
            this.Soluong.Text = "Số lượng";
            this.Soluong.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Soluong.Width = 79;
            // 
            // Gia
            // 
            this.Gia.Text = "Đơn giá";
            this.Gia.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Gia.Width = 115;
            // 
            // ThanhTien
            // 
            this.ThanhTien.Text = "Thành tiền";
            this.ThanhTien.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ThanhTien.Width = 488;
            // 
            // txtTongThanhTienValue
            // 
            this.txtTongThanhTienValue.BackColor = System.Drawing.Color.PeachPuff;
            this.txtTongThanhTienValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTongThanhTienValue.ForeColor = System.Drawing.Color.Red;
            this.txtTongThanhTienValue.Location = new System.Drawing.Point(603, 457);
            this.txtTongThanhTienValue.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTongThanhTienValue.Multiline = true;
            this.txtTongThanhTienValue.Name = "txtTongThanhTienValue";
            this.txtTongThanhTienValue.Size = new System.Drawing.Size(292, 41);
            this.txtTongThanhTienValue.TabIndex = 14;
            this.txtTongThanhTienValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.SeaShell;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Goldenrod;
            this.label1.Location = new System.Drawing.Point(377, 460);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 29);
            this.label1.TabIndex = 13;
            this.label1.Text = "Tổng thành tiền:";
            // 
            // lblHoaDon
            // 
            this.lblHoaDon.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblHoaDon.AutoSize = true;
            this.lblHoaDon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblHoaDon.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoaDon.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblHoaDon.Location = new System.Drawing.Point(361, 9);
            this.lblHoaDon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHoaDon.Name = "lblHoaDon";
            this.lblHoaDon.Size = new System.Drawing.Size(162, 36);
            this.lblHoaDon.TabIndex = 19;
            this.lblHoaDon.Text = "HÓA ĐƠN";
            // 
            // lblTen
            // 
            this.lblTen.AutoSize = true;
            this.lblTen.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTen.Location = new System.Drawing.Point(587, 62);
            this.lblTen.Name = "lblTen";
            this.lblTen.Size = new System.Drawing.Size(105, 25);
            this.lblTen.TabIndex = 20;
            this.lblTen.Text = "Người lập: ";
            // 
            // lblNguoiLapValue
            // 
            this.lblNguoiLapValue.AutoSize = true;
            this.lblNguoiLapValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNguoiLapValue.Location = new System.Drawing.Point(703, 62);
            this.lblNguoiLapValue.Name = "lblNguoiLapValue";
            this.lblNguoiLapValue.Size = new System.Drawing.Size(47, 25);
            this.lblNguoiLapValue.TabIndex = 21;
            this.lblNguoiLapValue.Text = "Tên";
            this.lblNguoiLapValue.Click += new System.EventHandler(this.lblNguoiLapValue_Click);
            // 
            // lblNgayValue
            // 
            this.lblNgayValue.AutoSize = true;
            this.lblNgayValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgayValue.Location = new System.Drawing.Point(703, 107);
            this.lblNgayValue.Name = "lblNgayValue";
            this.lblNgayValue.Size = new System.Drawing.Size(112, 25);
            this.lblNgayValue.TabIndex = 23;
            this.lblNgayValue.Text = "24/05/2025";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(587, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 25);
            this.label3.TabIndex = 22;
            this.label3.Text = "Ngày: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 25);
            this.label2.TabIndex = 24;
            this.label2.Text = "Khách hàng: ";
            // 
            // lblMaHoaDon
            // 
            this.lblMaHoaDon.AutoSize = true;
            this.lblMaHoaDon.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaHoaDon.Location = new System.Drawing.Point(25, 62);
            this.lblMaHoaDon.Name = "lblMaHoaDon";
            this.lblMaHoaDon.Size = new System.Drawing.Size(127, 25);
            this.lblMaHoaDon.TabIndex = 27;
            this.lblMaHoaDon.Text = "Mã hóa đơn: ";
            // 
            // lblMaHoaDonValue
            // 
            this.lblMaHoaDonValue.AutoSize = true;
            this.lblMaHoaDonValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaHoaDonValue.Location = new System.Drawing.Point(163, 62);
            this.lblMaHoaDonValue.Name = "lblMaHoaDonValue";
            this.lblMaHoaDonValue.Size = new System.Drawing.Size(64, 25);
            this.lblMaHoaDonValue.TabIndex = 28;
            this.lblMaHoaDonValue.Text = "label6";
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThanhToan.ForeColor = System.Drawing.Color.Red;
            this.btnThanhToan.Location = new System.Drawing.Point(707, 518);
            this.btnThanhToan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new System.Drawing.Size(189, 36);
            this.btnThanhToan.TabIndex = 29;
            this.btnThanhToan.Text = "Thanh toán";
            this.btnThanhToan.UseVisualStyleBackColor = true;
            // 
            // txtKhachHangName
            // 
            this.txtKhachHangName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKhachHangName.Location = new System.Drawing.Point(167, 103);
            this.txtKhachHangName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtKhachHangName.Name = "txtKhachHangName";
            this.txtKhachHangName.Size = new System.Drawing.Size(228, 30);
            this.txtKhachHangName.TabIndex = 30;
            this.txtKhachHangName.Leave += new System.EventHandler(this.txtKhachHangName_Leave);
            // 
            // PaymentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(912, 567);
            this.Controls.Add(this.txtKhachHangName);
            this.Controls.Add(this.btnThanhToan);
            this.Controls.Add(this.lblMaHoaDonValue);
            this.Controls.Add(this.lblMaHoaDon);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblNgayValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblNguoiLapValue);
            this.Controls.Add(this.lblTen);
            this.Controls.Add(this.lblHoaDon);
            this.Controls.Add(this.txtTongThanhTienValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvwChiTietHoaDon);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PaymentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Payment";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvwChiTietHoaDon;
        private System.Windows.Forms.ColumnHeader STT;
        private System.Windows.Forms.ColumnHeader TenDouong;
        private System.Windows.Forms.ColumnHeader Soluong;
        private System.Windows.Forms.ColumnHeader Gia;
        private System.Windows.Forms.ColumnHeader ThanhTien;
        private System.Windows.Forms.TextBox txtTongThanhTienValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblHoaDon;
        private System.Windows.Forms.Label lblTen;
        private System.Windows.Forms.Label lblNguoiLapValue;
        private System.Windows.Forms.Label lblNgayValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMaHoaDon;
        private System.Windows.Forms.Label lblMaHoaDonValue;
        private System.Windows.Forms.Button btnThanhToan;
        private System.Windows.Forms.TextBox txtKhachHangName;
    }
}