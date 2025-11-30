using CoffeeManagementSystem.bll;
using CoffeeManagementSystem.DAL;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class NoteForm : Form
    {
        // Khai báo biến
        private TuyChonBLL _tuyChonBLL = new TuyChonBLL();
        public decimal ReturnExtraPrice { get; set; } = 0; // Biến trả về tiền thêm (Topping)
        public string ReturnNote { get; set; } = "";
        public NoteForm(string ghiChu, string maDouong)
        {
            InitializeComponent();

            // 1. Gán ghi chú cũ vào TextBox
            txtGhiChu.Text = ghiChu;

            // 2. Vẽ các tùy chọn ĐỘNG
            GenerateDynamicOptions(maDouong);
        }
        // Hàm vẽ giao diện động (Chỉ tác động vào flowDynamicOptions)
        private void GenerateDynamicOptions(string maDouong)
        {
            // CHỈ XÓA PHẦN TÙY CHỌN, KHÔNG XÓA TEXTBOX GHI CHÚ
            flowDynamicOptions.Controls.Clear();

            // Lấy dữ liệu từ DB
            List<OptionGroupDTO> listGroups = _tuyChonBLL.GetOptionsByProduct(maDouong);

            if (listGroups == null || listGroups.Count == 0) return;

            foreach (var group in listGroups)
            {
                // Tạo GroupBox
                Guna2GroupBox grp = new Guna2GroupBox();
                grp.Text = group.TenNhom;
                grp.BorderColor = Color.LightGray;
                // Chỉnh chiều rộng bằng với panel cha trừ đi margin để đẹp
                grp.Size = new Size(flowDynamicOptions.Width - 10, 120);

                FlowLayoutPanel flpInner = new FlowLayoutPanel();
                flpInner.Dock = DockStyle.Fill;
                flpInner.BackColor = Color.White;
                grp.Controls.Add(flpInner);

                foreach (var item in group.Items)
                {
                    Guna2Button btn = new Guna2Button();
                    btn.Text = item.TenChiTiet;
                    if (item.GiaThem > 0) btn.Text += $" (+{item.GiaThem:N0})";

                    btn.Size = new Size(130, 35);
                    btn.BorderRadius = 10;
                    btn.BorderThickness = 1;
                    btn.FillColor = Color.White;
                    btn.BorderColor = Color.Silver;
                    btn.ForeColor = Color.Black;
                    btn.Margin = new Padding(5);

                    btn.Tag = item.GiaThem; // Lưu tiền

                    // Logic chọn Radio/Toggle
                    if (group.ChonNhieu)
                        btn.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.ToogleButton;
                    else
                        btn.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;

                    // Sự kiện đổi màu
                    btn.CheckedChanged += (s, e) => {
                        if (btn.Checked)
                        {
                            btn.FillColor = Color.MistyRose;
                            btn.BorderColor = Color.Red;
                        }
                        else
                        {
                            btn.FillColor = Color.White;
                            btn.BorderColor = Color.Silver;
                        }
                    };

                    flpInner.Controls.Add(btn);
                }

                // Thêm GroupBox vào khu vực ĐỘNG
                flowDynamicOptions.Controls.Add(grp);
            }
        }

        // Sự kiện nút LƯU (Kết hợp cả 2 nguồn dữ liệu)
        private void BtnLuu_Click(object sender, EventArgs e)
        {
            decimal tongTienThem = 0;
            List<string> cacLuaChon = new List<string>();

            // 1. Quét các nút ĐỘNG để lấy Topping/Đường/Đá
            foreach (Control grp in flowDynamicOptions.Controls)
            {
                if (grp is Guna2GroupBox groupBox)
                {
                    foreach (Control flp in groupBox.Controls)
                    {
                        if (flp is FlowLayoutPanel flowPanel)
                        {
                            foreach (Control c in flowPanel.Controls)
                            {
                                if (c is Guna2Button btn && btn.Checked)
                                {
                                    cacLuaChon.Add(btn.Text);
                                    if (btn.Tag != null)
                                        tongTienThem += Convert.ToDecimal(btn.Tag);
                                }
                            }
                        }
                    }
                }
            }

            // 2. Xử lý chuỗi ghi chú trả về
            string strOptions = string.Join(", ", cacLuaChon);
            string strManualNote = txtGhiChu.Text.Trim();

            if (!string.IsNullOrEmpty(strOptions) && !string.IsNullOrEmpty(strManualNote))
                ReturnNote = strOptions + ", " + strManualNote;
            else if (!string.IsNullOrEmpty(strOptions))
                ReturnNote = strOptions;
            else
                ReturnNote = strManualNote;

            // 3. Trả về giá tiền thêm (cho 1 đơn vị sản phẩm)
            ReturnExtraPrice = tongTienThem;

            // (Không trả về Quantity nữa)

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
