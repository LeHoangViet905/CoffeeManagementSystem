using CoffeeManagementSystem.Properties;

namespace CoffeeManagementSystem.Config
{
    public class VNPayConfig
    {
        public static string Vnp_Url => Settings.Default.Vnp_Url;
        public static string Vnp_TmnCode => Settings.Default.Vnp_TmnCode;
        public static string Vnp_HashSecret => Settings.Default.Vnp_HashSecret;
        public static string Vnp_ReturnUrl => Settings.Default.Vnp_ReturnUrl;
    }
}
