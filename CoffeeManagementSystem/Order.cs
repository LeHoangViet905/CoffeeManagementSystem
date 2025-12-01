using CoffeeManagementSystem.BLL; // Chỉ cần BLL, không cần DAL trực tiếp
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static CoffeeManagementSystem.AddDrinkForm;

namespace CoffeeManagementSystem
{
    public partial class OrderForm : Form
    {
        // 1. Khai báo BLL
        private DonhangBLL _donhangBLL;
        private DouongBLL _douongBLL;
        private LoaidouongBLL _loaidouongBLL;
        private GiadouongBLL _giadouongBLL;
        public string CurrentManhanvien { get; set; }
        public string CurrentTenNhanvien { get; set; }

        private OrderBLL _orderBLL;

        public OrderForm()
        {
            InitializeComponent();
            _donhangBLL = new DonhangBLL();

            _orderBLL = new OrderBLL(null);
            //Khởi tạo BLL
            _douongBLL = new DouongBLL();
            _loaidouongBLL = new LoaidouongBLL();
            _giadouongBLL = new GiadouongBLL();
            // Gán sự kiện Form_Load (sẽ chạy khi form này được mở)
            this.Load += new EventHandler(OrderForm_Load);
            // Gán sự kiện khi người dùng BẤM vào Tab danh mục
            guna2TabControl1.SelectedIndexChanged += new EventHandler(guna2TabControl1_SelectedIndexChanged);

        }
        private void GenerateAndShowOrderCode()
        {
            string newCode = _donhangBLL.TaoMaDonHangMoi();
            guna2TextBox1.Text = newCode;
        }
        public OrderForm(string manhanvien, string tenNhanVien) : this() // Gọi constructor mặc định
        {
            CurrentManhanvien = manhanvien;
            CurrentTenNhanvien = tenNhanVien;
            // CẬP NHẬT: OrderBLL với mã nhân viên chính xác
            _orderBLL = new OrderBLL(CurrentManhanvien);
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            LoadCategoriesTabs();

            //Gọi hàm tải sản phẩm
            FlowLayoutPanel allPanel = (FlowLayoutPanel)guna2TabControl1.TabPages[0].Controls[0];
            LoadProductsBySearch(allPanel, "");
            GenerateAndShowOrderCode();

        }

        //LẤY danh mục từ BLL và TẠO ra các Tab
        private void LoadCategoriesTabs()
        {
            guna2TabControl1.TabPages.Clear();
            TabPage allTab = new TabPage("Tất cả");
            allTab.Tag = "ALL"; // Đặt Tag đặc biệt

            FlowLayoutPanel flpAll = new FlowLayoutPanel();
            flpAll.Dock = DockStyle.Fill;
            flpAll.AutoScroll = true;
            allTab.Controls.Add(flpAll);

            // Thêm tab "Tất cả" VÀO TRƯỚC
            guna2TabControl1.TabPages.Add(allTab);
            // 1. Lấy danh sách loại (Category) từ BLL
            List<Loaidouong> danhSachLoai = _loaidouongBLL.GetAllLoaidouongs();
            //Dùng vòng lặp để tạo TabPage cho mỗi loại
            foreach (Loaidouong loai in danhSachLoai)
            {
                // Tạo 1 tab mới, Tên tab là tên loại
                TabPage newTab = new TabPage(loai.Tenloai);

                // Lưu "Maloai" (ID) vào thuộc tính Tag
                newTab.Tag = loai.Maloai;

                // Tạo 1 FlowLayoutPanel để chứa sản phẩm
                FlowLayoutPanel flp = new FlowLayoutPanel();
                flp.Dock = DockStyle.Fill;
                flp.AutoScroll = true; // Cho phép cuộn nếu nhiều sản phẩm

                // Thêm FlowLayoutPanel vào BÊN TRONG TabPage
                newTab.Controls.Add(flp);

                // Thêm TabPage (đã chứa flp) vào TabControl
                guna2TabControl1.TabPages.Add(newTab);
            }
        }


        // TẢI SẢN PHẨM (PRODUCTS)

        // Sự kiện này chạy MỖI KHI user bấm vào 1 tab khác
        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2TabControl1.SelectedTab == null) return;

            TabPage selectedTab = guna2TabControl1.SelectedTab;
            FlowLayoutPanel currentFlowPanel = (FlowLayoutPanel)selectedTab.Controls[0];
            string categoryID = (string)selectedTab.Tag;

            if (categoryID == "ALL")
            {
                // Nếu là tab "Tất cả":
                // 1. Kích hoạt ô tìm kiếm (nếu cần)
                txtTimKiem.Enabled = true;

                // 2. Tải sản phẩm dựa trên nội dung ô tìm kiếm
                // (Nếu ô rỗng, searchTerm sẽ là "", nó sẽ tải TẤT CẢ)
                string searchTerm = txtTimKiem.Text.Trim();
                LoadProductsBySearch(currentFlowPanel, searchTerm);
            }
            else
            {
                // Nếu là tab "Cà phê", "Trà"...
                // 1. Tắt/Xóa ô tìm kiếm
                txtTimKiem.Enabled = false; // Tắt đi
                txtTimKiem.Text = "";       // Xóa chữ đi

                // 2. Tải sản phẩm theo loại (hàm này LỌC)
                LoadProductsIntoPanel(categoryID, currentFlowPanel);
            }
        }
        // Hàm này có nhiệm vụ LẤY sản phẩm từ BLL và TẠO ra các UserControl (ucProductItem)


        // -----------------------------------------------------------------
        // PHẦN 5: THÊM VÀO GIỎ HÀNG (CART)
        // -----------------------------------------------------------------

        // Sự kiện này chạy MỖI KHI user click vào 1 "ô" sản phẩm
        private void ucProduct_Click(object sender, EventArgs e)
        {
            // 1. 'sender' chính là 'ucProductItem' đã được click
            ucProductItem clickedItem = (ucProductItem)sender;

            // 2. Lấy data đã lưu trong nó
            string id = clickedItem.ProductID;
            string name = clickedItem.ProductName;
            decimal price = clickedItem.ProductPriceDecimal; // <-- Lấy giá gốc (decimal)

            // 3. Gọi hàm thêm vào giỏ hàng
            AddProductToOrder(id, name, price);

            // 4. Cập nhật lại tổng tiền
            UpdateOrderTotal();
        }

        // Hàm xử lý logic giỏ hàng
        private void AddProductToOrder(string productID, string name, decimal price)
        {

            // 1. TÌM DÒNG "SẠCH" ĐỂ GỘP (Cùng ID và KHÔNG có ghi chú)
            foreach (DataGridViewRow row in dgvOrder.Rows)
            {
                if (row.IsNewRow) continue;

                string rowID = row.Cells["colID"].Value?.ToString();

                // Lấy ghi chú hiện tại của dòng đó (kiểm tra null)
                string rowNote = "";
                if (row.Cells["colNote"].Value != null)
                    rowNote = row.Cells["colNote"].Value.ToString();

                // ĐIỀU KIỆN GỘP:
                // 1. Cùng Mã món
                // 2. Ghi chú phải RỖNG (Tức là món này chưa thêm topping/sửa đổi gì cả)
                if (rowID == productID && string.IsNullOrEmpty(rowNote))
                {
                    // ==> TÌM THẤY DÒNG SẠCH: Tăng số lượng
                    int currentQty = Convert.ToInt32(row.Cells["colQty"].Value);
                    currentQty++;

                    row.Cells["colQty"].Value = currentQty;

                    // Tính lại tổng tiền (Số lượng * Giá hiện tại)
                    // (Giá hiện tại chính là giá gốc vì chưa có topping)
                    row.Cells["colTotal"].Value = currentQty * price;

                    UpdateOrderTotal();
                    return; // Xong nhiệm vụ, thoát hàm luôn
                }
            }

            // 2. NẾU KHÔNG TÌM THẤY DÒNG SẠCH -> THÊM DÒNG MỚI
            // (Kể cả khi đã có dòng "Trà Đào + Topping" rồi, code sẽ chạy xuống đây để tạo dòng mới)

            int index = dgvOrder.Rows.Add();
            DataGridViewRow newRow = dgvOrder.Rows[index];

            // Gán dữ liệu
            newRow.Cells["colID"].Value = productID;
            newRow.Cells["colQty"].Value = 1;
            newRow.Cells["colName"].Value = name;
            newRow.Cells["colPrice"].Value = price;
            newRow.Cells["colTotal"].Value = price;

            // Gán các nút và cột ẩn
            newRow.Cells["colDecrease"].Value = "-";
            newRow.Cells["colCancel"].Value = "X";

            // QUAN TRỌNG:
            newRow.Cells["colName"].Tag = name; // Lưu tên gốc để sau này sửa ghi chú không bị mất tên
            newRow.Cells["colNote"].Value = ""; // Món mới tinh thì ghi chú rỗng

            UpdateOrderTotal();
        
        }

        private void UpdateOrderTotal()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dgvOrder.Rows)
            {
                // 1. Bỏ qua dòng trống (dòng mới) ở cuối bảng
                if (row.IsNewRow) continue;

                // 2. Kiểm tra null và rỗng
                if (row.Cells["colTotal"].Value != null &&
                    !string.IsNullOrEmpty(row.Cells["colTotal"].Value.ToString()))
                {
                    // 3. SỬA LỖI Ở ĐÂY: Dùng Convert.ToDecimal thay vì (decimal)
                    try
                    {
                        total += Convert.ToDecimal(row.Cells["colTotal"].Value);
                    }
                    catch
                    {
                    }
                }
            }

            txtTongTien.Text = total.ToString("N0");
        }

        private void LoadProductsBySearch(FlowLayoutPanel targetPanel, string searchTerm)
        {
            // Lấy List<Douong> (đã có giá)
            List<Douong> searchResults = _giadouongBLL.SearchAllDouongs(searchTerm);
            if (searchResults == null)
            {
                // Nếu BLL trả về null, hãy tự tạo 1 list rỗng
                // để code không bị crash ở vòng lặp foreach
                searchResults = new List<Douong>();
            }
            LoadProductsFromList(searchResults, targetPanel);
        }

        // Hàm này chỉ hiển thị
        private void LoadProductsFromList(List<Douong> productList, FlowLayoutPanel targetPanel)
        {
            targetPanel.Controls.Clear();

            foreach (Douong product in productList)
            {
                ucProductItem item = new ucProductItem();

                decimal price = product.Giaban;

                item.ProductID = product.Madouong;
                item.ProductName = product.Tendouong;
                item.ProductPriceDecimal = price;

                // === PHẦN TẢI ẢNH ===
                try
                {
                    string imageName = product.Hinhanh; // giờ chỉ là "DU001.jpg"

                    if (!string.IsNullOrEmpty(imageName))
                    {
                        string fullPath = Path.Combine(ImageConfig.DrinkImageFolder, imageName);

                        if (File.Exists(fullPath))
                        {
                            item.ProductImage = Image.FromFile(fullPath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi tải ảnh: " + ex.Message);
                }
                // === KẾT THÚC PHẦN TẢI ẢNH ===

                item.Click += new EventHandler(ucProduct_Click);
                targetPanel.Controls.Add(item);
            }
        }

        // Hàm này tải theo loại
        private void LoadProductsIntoPanel(string categoryID, FlowLayoutPanel targetPanel)
        {
            // 1. Lấy TẤT CẢ sản phẩm (đã có giá)
            List<Douong> allProducts = _giadouongBLL.SearchAllDouongs("");

            // 2. Lọc bằng C#
            List<Douong> filteredList = allProducts
                                            .Where(p => p.Maloai == categoryID)
                                            .ToList();

            // 3. Hiển thị
            LoadProductsFromList(filteredList, targetPanel);
        }

        private void dgvOrder_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Đảm bảo user click vào 1 hàng (không phải tiêu đề)
            if (e.RowIndex < 0 || dgvOrder.Rows[e.RowIndex].IsNewRow) return;
            DataGridViewRow row = dgvOrder.Rows[e.RowIndex];

            // --- CASE A: NÚT XÓA (X) ---
            if (e.ColumnIndex == dgvOrder.Columns["colCancel"].Index)
            {
                dgvOrder.Rows.Remove(row);
                UpdateOrderTotal();
            }
            // --- CASE B: NÚT GIẢM (-) ---
            else if (e.ColumnIndex == dgvOrder.Columns["colDecrease"].Index)
            {
                int currentQty = (int)row.Cells["colQty"].Value;
                decimal price = (decimal)row.Cells["colPrice"].Value;

                if (currentQty > 1)
                {
                    currentQty--;
                    row.Cells["colQty"].Value = currentQty;
                    row.Cells["colTotal"].Value = currentQty * price;
                }
                else
                {
                    dgvOrder.Rows.Remove(row);
                }
                UpdateOrderTotal();
            }

            // --- CASE C: CLICK VÀO TÊN MÓN ĐỂ SỬA ---
            else if (e.ColumnIndex == dgvOrder.Columns["colName"].Index)
            {
                // 1. Lấy thông tin hiện tại
                string maMon = row.Cells["colID"].Value.ToString();

                // Lấy ghi chú cũ từ cột ẩn (nếu có)
                string ghiChuHienTai = "";
                if (row.Cells["colNote"].Value != null)
                    ghiChuHienTai = row.Cells["colNote"].Value.ToString();

                // Lấy tên gốc từ Tag (để tránh lấy nhầm tên kèm ghi chú cũ)
                string tenMonGoc = row.Cells["colName"].Tag?.ToString();
                if (string.IsNullOrEmpty(tenMonGoc))
                    tenMonGoc = row.Cells["colName"].Value.ToString().Split('\n')[0].Trim();

                // 2. Mở NoteForm
                // (Dùng Constructor nhận Ghi chú cũ và Mã món để load option)
                NoteForm noteForm = new NoteForm(ghiChuHienTai, maMon);

                this.Opacity = 0.9;
                DialogResult result = noteForm.ShowDialog();
                this.Opacity = 1;

                // 3. Nhận kết quả và Cập nhật lại chính dòng đó (Không xóa đi thêm lại)
                if (result == DialogResult.OK)
                {
                    // A. Cập nhật Ghi chú ẩn
                    string ghiChuMoi = noteForm.ReturnNote;
                    row.Cells["colNote"].Value = ghiChuMoi;

                    // B. Cập nhật Hiển thị Tên (Tên gốc + Xuống dòng + Ghi chú mới)
                    if (!string.IsNullOrEmpty(ghiChuMoi))
                    {
                        row.Cells["colName"].Value = tenMonGoc + Environment.NewLine + "   " + ghiChuMoi;
                    }
                    else
                    {
                        row.Cells["colName"].Value = tenMonGoc;
                    }
                    // Đảm bảo Tag vẫn giữ tên gốc
                    row.Cells["colName"].Tag = tenMonGoc;

                    // C. Cập nhật Giá tiền
                    // Lấy giá gốc sạch từ DB
                    Giadouong giaGocObj = _giadouongBLL.GetLatestGiaByMadouong(maMon);
                    decimal giaGoc = (decimal)giaGocObj.Giaban;

                    // Giá mới = Giá Gốc + Tiền Topping (từ NoteForm trả về)
                    decimal giaMoi = giaGoc + noteForm.ReturnExtraPrice;

                    // Cập nhật vào ô giá
                    row.Cells["colPrice"].Value = giaMoi;

                    // Tính lại Thành tiền (Số lượng * Giá mới)
                    int soLuong = Convert.ToInt32(row.Cells["colQty"].Value);
                    row.Cells["colTotal"].Value = soLuong * giaMoi;

                    UpdateOrderTotal();
                }
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (guna2TabControl1.SelectedTab.Tag.ToString() != "ALL")
            {
                // Nếu user gõ mà đang ở tab khác, tự động nhảy về "Tất cả"
                guna2TabControl1.SelectedIndex = 0;
                // (Việc này sẽ tự gọi 'SelectedIndexChanged' và chạy đúng)
            }

            // 2. Lấy panel và từ khóa
            FlowLayoutPanel allPanel = (FlowLayoutPanel)guna2TabControl1.TabPages[0].Controls[0];
            string searchTerm = txtTimKiem.Text.Trim();

            // 3. Gọi hàm Search
            LoadProductsBySearch(allPanel, searchTerm);
        }

        private void btnThuTien_Click(object sender, EventArgs e)
        {
            // 1. KIỂM TRA NẾU GIỎ HÀNG BỊ TRỐNG
            if (dgvOrder.Rows.Count == 0 || (dgvOrder.Rows.Count == 1 && dgvOrder.Rows[0].IsNewRow))
            {
                MessageBox.Show("Giỏ hàng đang trống, không thể thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. CHUYỂN DỮ LIỆU TỪ GIỎ HÀNG (DataGridView) SANG List<Chitietdonhang>

            List<Chitietdonhang> dsChiTiet = new List<Chitietdonhang>();

            foreach (DataGridViewRow row in dgvOrder.Rows)
            {
                // Bỏ qua hàng mới (hàng trống) ở cuối DataGridView
                if (row.IsNewRow) continue;
                // Tạo một chi tiết mới
                Chitietdonhang chiTiet = new Chitietdonhang();

                // Lấy dữ liệu từ các cột của dgvOrder
                chiTiet.Madouong = row.Cells["colID"].Value.ToString();
                chiTiet.Tendouong = row.Cells["colName"].Value.ToString();
                chiTiet.Soluong = (int)row.Cells["colQty"].Value;
                chiTiet.Dongia = (decimal)row.Cells["colPrice"].Value;   // Lấy từ cột đơn giá (ẩn)
                chiTiet.Thanhtien = (decimal)row.Cells["colTotal"].Value; // Lấy từ cột thành tiền
                                                                          // chiTiet.Madonhang sẽ được BLL tự tạo
                                                                          // chiTiet.Ghichu có thể thêm sau (nếu bạn có)

                dsChiTiet.Add(chiTiet);
            }

            // 3. LẤY THÔNG TIN NHÂN VIÊN
            string maNV = this.CurrentManhanvien;
            string tenNV = this.CurrentTenNhanvien;
            // LẤY MÃ ĐƠN ĐANG HIỂN THỊ
            string maDonHienTai = guna2TextBox1.Text.Trim();
            // 4. KHỞI TẠO VÀ MỞ PAYMENTFORM
            // SỬA CONSTRUCTOR PaymentForm CHO NHẬN THÊM MÃ ĐƠN
            PaymentForm paymentForm = new PaymentForm(dsChiTiet, maNV, tenNV, maDonHienTai);

            // Dùng ShowDialog() để nó "đóng băng" OrderForm
            // Chờ cho đến khi PaymentForm được đóng lại
            DialogResult result = paymentForm.ShowDialog();

            // 5. XỬ LÝ KẾT QUẢ SAU KHI THANH TOÁN
            // (PaymentForm của bạn trả về DialogResult.OK khi thành công)
            if (result == DialogResult.OK)
            {
                dgvOrder.Rows.Clear();
                UpdateOrderTotal();

                // 🔥 SAU KHI THANH TOÁN XONG VÀ LƯU ĐƠN XUỐNG DB:
                // OrderForm hỏi DB mã cuối + 1 → hiện mã mới
                GenerateAndShowOrderCode();
            }
            // (Nếu DialogResult là Cancel, chúng ta không làm gì cả, 
            //  người dùng đã tự tắt form Payment)
        }

        private void btnHuyOrder_Click(object sender, EventArgs e)
        {
            // BƯỚC 1: Kiểm tra xem có gì để xóa không
            if (dgvOrder.Rows.Count == 0 || (dgvOrder.Rows.Count == 1 && dgvOrder.Rows[0].IsNewRow))
            {
                // Không có gì trong giỏ hàng, không cần làm gì cả
                return;
            }

            // BƯỚC 2: HỎI XÁC NHẬN (Rất quan trọng!)
            // (Để tránh nhân viên bấm nhầm)
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn hủy đơn hàng này không?\nTất cả các món đã chọn sẽ bị xóa.",
                "Xác nhận hủy",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            // BƯỚC 3: XỬ LÝ NẾU NHẤN "YES"
            if (result == DialogResult.Yes)
            {
                // 1. Xóa tất cả các hàng trong giỏ hàng
                dgvOrder.Rows.Clear();

                // 2. Cập nhật lại tổng tiền (về 0)
                UpdateOrderTotal();
            }
            // Nếu nhấn "No", không làm gì cả
        }
    }
}