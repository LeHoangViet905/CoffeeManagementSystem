// File: Utilities/Logger.cs

using System;
using System.IO;

namespace CoffeeManagementSystem.Utilities
{
    // Enum định nghĩa các cấp độ log
    public enum LogLevel
    {
        DEBUG,   // Thông tin gỡ lỗi chi tiết, thường chỉ dùng trong phát triển
        INFO,    // Thông tin chung về hoạt động của ứng dụng
        WARNING, // Các tình huống có thể có vấn đề, nhưng không làm ứng dụng dừng
        ERROR,   // Lỗi nghiêm trọng, ứng dụng vẫn có thể tiếp tục chạy nhưng có vấn đề
        FATAL    // Lỗi nghiêm trọng nhất, ứng dụng có thể không thể tiếp tục hoạt động
    }

    public static class Logger
    {
        // Đường dẫn đến file log. File này sẽ được tạo trong thư mục Debug/Release của ứng dụng.
        private static readonly string LogFilePath = "application.log";
        // Đối tượng khóa để đảm bảo chỉ một luồng ghi vào file tại một thời điểm (tránh lỗi xung đột)
        private static readonly object _lock = new object();

        /// <summary>
        /// Phương thức chính để ghi một thông điệp log.
        /// </summary>
        /// <param name="message">Nội dung thông điệp log.</param>
        /// <param name="level">Cấp độ của thông điệp log (mặc định là INFO).</param>
        public static void Log(string message, LogLevel level = LogLevel.INFO)
        {
            // Định dạng chuỗi log: [Thời gian] [Cấp độ] Nội dung
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level.ToString()}] {message}";

            // -- Tùy chọn: Ghi log ra Console để dễ dàng theo dõi khi debug --
            Console.WriteLine(logEntry);

            // -- Ghi log vào file --
            lock (_lock) // Sử dụng khóa để an toàn khi ghi từ nhiều luồng
            {
                try
                {
                    // File.AppendAllText sẽ tạo file nếu nó chưa tồn tại, và thêm vào cuối
                    File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu không thể ghi log vào file (ví dụ: quyền truy cập)
                    // Trong trường hợp này, ta ghi lỗi này ra console (hoặc Event Log của Windows)
                    Console.Error.WriteLine($"Lỗi khi ghi log vào file '{LogFilePath}': {ex.Message}");
                    Console.Error.WriteLine($"StackTrace: {ex.StackTrace}");
                }
            }
        }

        // Các phương thức tiện ích để ghi log theo cấp độ cụ thể
        public static void LogInfo(string message)
        {
            Log(message, LogLevel.INFO);
        }

        public static void LogWarning(string message)
        {
            Log(message, LogLevel.WARNING);
        }

        public static void LogError(string message, Exception ex = null)
        {
            string logMessage = message;
            if (ex != null)
            {
                logMessage += Environment.NewLine + $"Exception Type: {ex.GetType().Name}" +
                              Environment.NewLine + $"Message: {ex.Message}" +
                              Environment.NewLine + $"StackTrace: {ex.StackTrace}";
                if (ex.InnerException != null)
                {
                    logMessage += Environment.NewLine + $"Inner Exception Type: {ex.InnerException.GetType().Name}" +
                                  Environment.NewLine + $"Inner Message: {ex.InnerException.Message}";
                }
            }
            Log(logMessage, LogLevel.ERROR);
        }

        public static void LogFatal(string message, Exception ex = null)
        {
            string logMessage = message;
            if (ex != null)
            {
                logMessage += Environment.NewLine + $"Exception Type: {ex.GetType().Name}" +
                              Environment.NewLine + $"Message: {ex.Message}" +
                              Environment.NewLine + $"StackTrace: {ex.StackTrace}";
                if (ex.InnerException != null)
                {
                    logMessage += Environment.NewLine + $"Inner Exception Type: {ex.InnerException.GetType().Name}" +
                                  Environment.NewLine + $"Inner Message: {ex.InnerException.Message}";
                }
            }
            Log(logMessage, LogLevel.FATAL);
        }

        public static void LogDebug(string message)
        {
            // Thông thường, log DEBUG chỉ nên xuất hiện khi đang ở chế độ Debug
            // Bạn có thể dùng #if DEBUG ... #endif để kiểm soát
            Log(message, LogLevel.DEBUG);
        }
    }
}