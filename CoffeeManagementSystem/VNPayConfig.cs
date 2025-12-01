using CoffeeManagementSystem.Properties;

namespace CoffeeManagementSystem.Config
{
    // Lớp cấu hình chứa các thông tin VNPay
    // Lấy giá trị từ Settings của project (Properties/Settings.settings)
    public class VNPayConfig
    {
        // Địa chỉ cổng thanh toán VNPay (sandbox hoặc production)
        public static string Vnp_Url => Settings.Default.Vnp_Url;

        // Mã website/terminal do VNPay cấp (vnp_TmnCode)
        public static string Vnp_TmnCode => Settings.Default.Vnp_TmnCode;

        // Chuỗi bí mật dùng để tạo chữ ký HMAC (vnp_HashSecret)
        public static string Vnp_HashSecret => Settings.Default.Vnp_HashSecret;

        // URL để VNPay redirect về sau khi thanh toán xong (vnp_ReturnUrl)
        public static string Vnp_ReturnUrl => Settings.Default.Vnp_ReturnUrl;
    }
}
