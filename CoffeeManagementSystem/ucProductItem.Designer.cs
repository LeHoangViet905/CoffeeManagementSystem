namespace CoffeeManagementSystem
{
    partial class ucProductItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picImage = new Guna.UI2.WinForms.Guna2PictureBox();
            this.chipPrice = new Guna.UI2.WinForms.Guna2Chip();
            this.lblName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.SuspendLayout();
            // 
            // picImage
            // 
            this.picImage.BorderRadius = 10;
            this.picImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picImage.ImageRotate = 0F;
            this.picImage.Location = new System.Drawing.Point(0, 0);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(180, 180);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picImage.TabIndex = 0;
            this.picImage.TabStop = false;
            // 
            // chipPrice
            // 
            this.chipPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chipPrice.BorderColor = System.Drawing.Color.White;
            this.chipPrice.BorderRadius = 8;
            this.chipPrice.DefaultAutoSize = true;
            this.chipPrice.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chipPrice.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chipPrice.ForeColor = System.Drawing.Color.White;
            this.chipPrice.Location = new System.Drawing.Point(64, 3);
            this.chipPrice.Name = "chipPrice";
            this.chipPrice.Padding = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.chipPrice.Size = new System.Drawing.Size(113, 37);
            this.chipPrice.TabIndex = 1;
            this.chipPrice.Text = "80,000";
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblName.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(0, 158);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(180, 22);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Tên sản phẩm";
            this.lblName.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ucProductItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.chipPrice);
            this.Controls.Add(this.picImage);
            this.Name = "ucProductItem";
            this.Size = new System.Drawing.Size(180, 180);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox picImage;
        private Guna.UI2.WinForms.Guna2Chip chipPrice;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblName;
    }
}
