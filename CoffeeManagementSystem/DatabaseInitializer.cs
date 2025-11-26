using System;
using System.Data.SQLite;
using System.IO; // Cần cho Path và File
using System.Windows.Forms; // Cần cho MessageBox.Show (chỉ để hiển thị lỗi ví dụ)

// Đảm bảo bạn đã cài đặt gói NuGet "System.Data.SQLite"
namespace CoffeeManagementSystem
{
    public class DatabaseInitializer // Bạn có thể đặt tên lớp khác
    {
        private string dbFilePath; // Đường dẫn đầy đủ tới file CSDL

        public DatabaseInitializer(string databaseName)
        {
            // Lấy đường dẫn thư mục chạy ứng dụng (|DataDirectory|)
            // Trong WinForms .NET Framework, |DataDirectory| thường trỏ đến thư mục Debug hoặc Release
            string dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory") as string;

            if (string.IsNullOrEmpty(dataDirectory))
            {
                // Nếu |DataDirectory| không được thiết lập, sử dụng thư mục thực thi
                dataDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            this.dbFilePath = Path.Combine(dataDirectory, databaseName + ".db");
        }

        // Hàm này sẽ tạo CSDL và các bảng nếu chúng chưa tồn tại
        public void InitializeDatabase()
        {
            // 1. Tạo file CSDL nếu chưa tồn tại
            if (!File.Exists(dbFilePath))
            {
                try
                {
                    SQLiteConnection.CreateFile(dbFilePath);
                    MessageBox.Show($"File CSDL '{Path.GetFileName(dbFilePath)}' đã được tạo thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tạo file CSDL: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Dừng lại nếu không tạo được file
                }
            }

            // 2. Tạo cấu trúc bảng (Schema)
            // Sử dụng CREATE TABLE IF NOT EXISTS để tránh lỗi nếu bảng đã tồn tại
            string createSchemaSql = @"
        CREATE TABLE IF NOT EXISTS Loaidouong (
            Maloai TEXT PRIMARY KEY,
            Tenloai TEXT
        );

        CREATE TABLE IF NOT EXISTS Douong (
            Madouong TEXT PRIMARY KEY,
            Tendouong TEXT,
            Maloai TEXT,
            Mota TEXT,
            Hinhanh TEXT,
            FOREIGN KEY (Maloai) REFERENCES Loaidouong (Maloai) ON DELETE CASCADE -- Thêm ON DELETE CASCADE nếu muốn xóa đồ uống khi loại đồ uống bị xóa
        );

        CREATE TABLE IF NOT EXISTS Giadouong (
            Magia TEXT PRIMARY KEY,
            Madouong TEXT,
            Giaban REAL, -- REAL dùng cho số thập phân trong SQLite
            Thoigianapdung TEXT, -- Lưu DateTime dưới dạng TEXT hoặc INTEGER (timestamp)
            FOREIGN KEY (Madouong) REFERENCES Douong (Madouong) ON DELETE CASCADE
        );

        CREATE TABLE IF NOT EXISTS Nhanvien (
            Manhanvien TEXT PRIMARY KEY,
            Hoten TEXT,
            Ngaysinh TEXT, -- Lưu DateTime dưới dạng TEXT
            Gioitinh TEXT,
            Diachi TEXT,
            Sodienthoai TEXT,
            Email TEXT,
            Ngayvaolam TEXT -- Lưu DateTime dưới dạng TEXT
        );

        CREATE TABLE IF NOT EXISTS Taikhoan (
            Mataikhoan TEXT PRIMARY KEY,
            Tendangnhap TEXT UNIQUE, -- UNIQUE đảm bảo tên đăng nhập không trùng
            Matkhau TEXT,
            Vaitro TEXT,
            Manhanvien TEXT UNIQUE, -- UNIQUE nếu 1 nhân viên chỉ có 1 tài khoản, nếu 1-nhiều thì bỏ UNIQUE
            FOREIGN KEY (Manhanvien) REFERENCES Nhanvien (Manhanvien) ON DELETE CASCADE
        );

        CREATE TABLE IF NOT EXISTS Khachhang (
            Makhachhang TEXT PRIMARY KEY,
            Hoten TEXT,
            Sodienthoai TEXT UNIQUE,
            Email TEXT,
            Ngaydangky TEXT,
            Diemtichluy INTEGER DEFAULT 0 -- Giá trị mặc định là 0
        );

        CREATE TABLE IF NOT EXISTS Donhang (
            Madonhang TEXT PRIMARY KEY,
            Manhanvien TEXT,
            Makhachhang TEXT, -- Có thể NULL nếu là khách lẻ
            Thoigiandat TEXT,
            Trangthaidon TEXT,
            Tongtien REAL,
            FOREIGN KEY (Manhanvien) REFERENCES Nhanvien (Manhanvien),
            FOREIGN KEY (Makhachhang) REFERENCES Khachhang (Makhachhang) -- Khóa ngoại có thể NULL
        );

        CREATE TABLE IF NOT EXISTS Chitietdonhang (
            Madonhang TEXT,
            Madouong TEXT,
            Soluong INTEGER,
            Dongia REAL,
            Thanhtien REAL,
            Ghichu TEXT,
            FOREIGN KEY (Madonhang) REFERENCES Donhang (Madonhang) ON DELETE CASCADE,
            FOREIGN KEY (Madouong) REFERENCES Douong (Madouong)
            PRIMARY KEY (Madonhang , Madouong)

        );

        CREATE TABLE IF NOT EXISTS Thanhtoan (
            Mathanhtoan TEXT PRIMARY KEY,
            Madonhang TEXT UNIQUE, -- UNIQUE nếu 1 đơn hàng chỉ có 1 thanh toán
            Thoigianthanhtoan TEXT,
            Hinhthucthanhtoan TEXT,
            Sotienthanhtoan REAL,
            Manhanvienthu TEXT,
            Ghichu TEXT,
            FOREIGN KEY (Madonhang) REFERENCES Donhang (Madonhang) ON DELETE CASCADE,
            FOREIGN KEY (Manhanvienthu) REFERENCES Nhanvien (Manhanvien)
        );
        ";

            // Chuỗi kết nối đầy đủ hoặc sử dụng tên chuỗi từ App.config
            // string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SqliteDbConnection"].ConnectionString;
            string connectionString = $"data source={dbFilePath};Version=3;";


            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Mở kết nối đến CSDL

                    using (SQLiteCommand command = new SQLiteCommand(createSchemaSql, connection))
                    {
                        command.ExecuteNonQuery(); // Thực thi toàn bộ chuỗi SQL DDL
                        MessageBox.Show("Cấu trúc CSDL (các bảng) đã được tạo hoặc kiểm tra thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tạo cấu trúc CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}