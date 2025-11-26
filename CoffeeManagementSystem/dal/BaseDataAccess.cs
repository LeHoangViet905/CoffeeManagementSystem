using System.Configuration;
// Có thể cần thêm using System.Windows.Forms; nếu bạn muốn hiển thị MessageBox lỗi ngay trong DAL (thường không khuyến khích trong DAL thực tế, nên ném exception)


//Là class cha (base class) – các lớp DAL khác sẽ kế thừa nó để sử dụng chuỗi kết nối (ConnectionString) đến database SQLite.
public class BaseDataAccess
{
    public string ConnectionString { get; private set; } // Sử dụng protected để lớp con kế thừa truy cập

    public BaseDataAccess()
    {
        // Lấy chuỗi kết nối từ App.config
        // Đảm bảo tên "SqliteDbConnection" khớp với tên bạn đặt trong App.config
        ConnectionString = ConfigurationManager.ConnectionStrings["SqliteDbConnection"].ConnectionString;

        if (string.IsNullOrEmpty(ConnectionString))
        {
            // Nên ném exception thay vì MessageBox trong lớp DAL thực tế
            throw new ConfigurationErrorsException("Connection string 'SqliteDbConnection' not found in App.config.");
            // Nếu muốn dùng MessageBox cho đơn giản khi phát triển:
            // MessageBox.Show("Không tìm thấy chuỗi kết nối 'SqliteDbConnection' trong App.config.", "Lỗi cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // throw new ArgumentNullException("Chuỗi kết nối không được tìm thấy.");
        }
    }

    // Bạn có thể thêm các hàm helper chung ở đây nếu cần, ví dụ:
    // Protected SQLiteConnection GetConnection() { return new SQLiteConnection(ConnectionString); }
}