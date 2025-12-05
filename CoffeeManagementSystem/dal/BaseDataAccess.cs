using System.Configuration;
using System.Data.SQLite;

namespace CoffeeManagementSystem.DAL
{
    /// <summary>
    /// Lớp cha dùng chung cho tất cả DAL.
    /// Đọc ConnectionString từ App.config và cung cấp hàm tạo SQLiteConnection.
    /// </summary>
    public  class BaseDataAccess
    {
        /// <summary>
        /// Chuỗi kết nối SQLite đọc từ App.config.
        /// Lớp con có thể dùng, bên ngoài không cần thấy.
        /// </summary>
        public string ConnectionString { get; }

        public BaseDataAccess()
        {
            // Đảm bảo tên "SqliteDbConnection" trùng với App.config
            var cs = ConfigurationManager.ConnectionStrings["SqliteDbConnection"]?.ConnectionString;

            if (string.IsNullOrWhiteSpace(cs))
            {
                throw new ConfigurationErrorsException(
                    "Connection string 'SqliteDbConnection' not found or empty in App.config.");
            }

            ConnectionString = cs;
        }
    }
}
