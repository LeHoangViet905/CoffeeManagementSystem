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
        // 1. Khai báo BLL để lấy tùy chọn
        private TuyChonBLL _tuyChonBLL = new TuyChonBLL();

        // 2. HAI BIẾN QUAN TRỌNG ĐỂ TRẢ VỀ KẾT QUẢ
        public string ReturnNote { get; set; } = "";       // Trả về chuỗi ghi chú
        public decimal ReturnExtraPrice { get; set; } = 0; // Trả về tiền topping

        // 3. CONSTRUCTOR MỚI (Nhận Ghi chú cũ và Mã món)
        // Khớp với lệnh gọi: new NoteForm(ghiChuHienTai, maMon);
        public NoteForm(string ghiChuCu, string maDouong)
        {
            InitializeComponent();

            // Hiển thị ghi chú cũ lên TextBox (nếu có)
            txtGhiChu.Text = ghiChuCu;

            // Tải các nút chọn (Đường/Đá/Topping) từ DB
            GenerateDynamicOptions(maDouong);
        }

        // Hàm vẽ giao diện động (Giữ nguyên logic cũ)
        private void GenerateDynamicOptions(string maDouong)
        {
            flowDynamicOptions.Controls.Clear();
            List<OptionGroupDTO> listGroups = _tuyChonBLL.GetOptionsByProduct(maDouong);

            if (listGroups == null) return;

            foreach (var group in listGroups)
            {
                Guna2GroupBox grp = new Guna2GroupBox();
                grp.Text = group.TenNhom;
                grp.Size = new Size(flowDynamicOptions.Width - 10, 120);

                FlowLayoutPanel flpInner = new FlowLayoutPanel();
                flpInner.Dock = DockStyle.Fill;
                grp.Controls.Add(flpInner);

                foreach (var item in group.Items)
                {
                    Guna2Button btn = new Guna2Button();
                    btn.Text = item.TenChiTiet;
                    if (item.GiaThem > 0) btn.Text += $" (+{item.GiaThem:N0})";

                    btn.Tag = item.GiaThem; // Lưu giá tiền vào Tag
                    btn.Size = new Size(130, 35);
                    btn.BorderRadius = 10;
                    btn.BorderThickness = 1;
                    btn.FillColor = Color.White;
                    btn.BorderColor = Color.Silver;
                    btn.ForeColor = Color.Black;

                    // Logic Checked (Tùy chọn)
                    // (Nếu bạn muốn làm logic: Mở lên tự động tick lại cái cũ thì cần xử lý chuỗi ghiChuCu ở Constructor
                    //  nhưng để đơn giản, tạm thời mở lên là trắng tinh cũng được)

                    if (group.ChonNhieu) btn.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.ToogleButton;
                    else btn.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;

                    btn.CheckedChanged += (s, e) => {
                        if (btn.Checked) { btn.FillColor = Color.MistyRose; btn.BorderColor = Color.Red; }
                        else { btn.FillColor = Color.White; btn.BorderColor = Color.Silver; }
                    };

                    flpInner.Controls.Add(btn);
                }
                flowDynamicOptions.Controls.Add(grp);
            }
        }

        // 4. SỰ KIỆN NÚT LƯU (Tính toán kết quả trả về)
        private void BtnLuu_Click(object sender, EventArgs e)
        {
            decimal tongTienThem = 0;
            List<string> cacLuaChon = new List<string>();

            // Quét tất cả các nút đang được chọn
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
                                    // A. Lấy text để làm Ghi chú
                                    // (Cắt bỏ phần giá tiền hiển thị nếu muốn đẹp: "Trân châu (+5k)" -> "Trân châu")
                                    string tenOption = btn.Text;
                                    cacLuaChon.Add(tenOption);

                                    // B. Cộng tiền
                                    if (btn.Tag != null)
                                    {
                                        decimal price = Convert.ToDecimal(btn.Tag);
                                        tongTienThem += price;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Gộp chuỗi từ nút bấm và chuỗi gõ tay
            string strOptions = string.Join(", ", cacLuaChon);
            string strManualNote = txtGhiChu.Text.Trim();

            if (!string.IsNullOrEmpty(strOptions) && !string.IsNullOrEmpty(strManualNote))
                ReturnNote = strOptions + ", " + strManualNote;
            else if (!string.IsNullOrEmpty(strOptions))
                ReturnNote = strOptions;
            else
                ReturnNote = strManualNote;

            // Gán giá trị tiền thêm
            ReturnExtraPrice = tongTienThem;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Nút Hủy/Quay lại
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void BtnHuy_Click(object sender, EventArgs e)
        {
            // 1. Xóa sạch nội dung ô Ghi chú tay
            txtGhiChu.Clear();

            // 2. Duyệt qua tất cả các GroupBox và Button để bỏ tick
            // Cấu trúc: FlowPanel Chính -> GroupBox -> FlowPanel Con -> Button

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
                                if (c is Guna2Button btn)
                                {
                                    // Bỏ chọn
                                    btn.Checked = false;

                                    // Reset màu sắc về mặc định (Trắng/Xám)
                                    // (Phải set thủ công để đảm bảo giao diện cập nhật ngay)
                                    btn.FillColor = Color.White;
                                    btn.BorderColor = Color.Silver;
                                    btn.ForeColor = Color.Black;
                                }
                            }
                        }
                    }
                }
            }

            // (Tùy chọn) Nếu bạn muốn đóng Form luôn sau khi xóa thì thêm dòng này:
            // this.DialogResult = DialogResult.Cancel;
            // this.Close();
        }
    }
}
