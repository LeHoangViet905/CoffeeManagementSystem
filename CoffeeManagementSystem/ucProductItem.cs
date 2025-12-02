using System;
using System.Drawing;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class ucProductItem : UserControl
    {
        public string ProductID { get; set; }
        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                lblName.Text = value;
            }
        }
        // --- Cầu nối cho Giá Sản phẩm ---
        private decimal _priceDecimal;
        public decimal ProductPriceDecimal
        {
            get
            {
                return _priceDecimal;
            }
            set
            {
                _priceDecimal = value;
                chipPrice.Text = value.ToString("N0");
            }
        }

        // --- Cầu nối cho Hình ảnh ---
        private Image _productImage;
        public Image ProductImage
        {
            get { return _productImage; }
            set
            {
                _productImage = value;
                picImage.Image = value;
            }
        }
        public ucProductItem()
        {
            InitializeComponent();

            // Gắn sự kiện click cho TẤT CẢ control CON
            picImage.Click += new EventHandler(ChildControl_Click);
            lblName.Click += new EventHandler(ChildControl_Click);
            chipPrice.Click += new EventHandler(ChildControl_Click);

        }

        // 3. Tạo hàm xử lý cho CÁC CON (và cả NỀN)
        private void ChildControl_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }
    }
}
