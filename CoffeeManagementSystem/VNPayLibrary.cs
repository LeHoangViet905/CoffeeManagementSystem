using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace CoffeeManagementSystem
{
    // Thư viện hỗ trợ build URL thanh toán VNPay + tạo chữ ký bảo mật
    public class VnPayLibrary
    {
        // Lưu các cặp key=value gửi sang VNPay, tự sắp xếp theo key nhờ VnPayCompare
        private readonly SortedList<string, string> _requestData =
            new SortedList<string, string>(new VnPayCompare());

        // Hàm thêm tham số gửi sang VNPay
        public void AddRequestData(string key, string value)
        {
            // Chỉ thêm nếu value không rỗng, tránh tham số rác
            if (!string.IsNullOrEmpty(value))
                _requestData.Add(key, value);
        }

        // Tạo URL request sang VNPay (gồm query string và vnp_SecureHash)
        // baseUrl: URL của VNPay (sandbox hoặc production)
        // vnp_HashSecret: chuỗi bí mật do VNPay cấp để tạo chữ ký
        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            // data: dùng để tạo chuỗi ký HMAC
            var data = new StringBuilder();
            // query: dùng để tạo query string gắn vào URL
            var query = new StringBuilder();

            // Duyệt qua từng cặp key=value trong _requestData
            // Lưu ý: SortedList đã sắp xếp key tăng dần theo VnPayCompare
            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                // Bỏ qua các value rỗng (nếu có)
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    // Mã hóa URL để tránh lỗi với ký tự đặc biệt (dấu cách, tiếng Việt, &, ?,...)
                    string encodedValue = WebUtility.UrlEncode(kv.Value);
                    string encodedKey = WebUtility.UrlEncode(kv.Key);

                    // Ghép thành key=value& cho chuỗi ký HMAC
                    data.Append(encodedKey + "=" + encodedValue + "&");
                    // Ghép thành key=value& cho query string
                    query.Append(encodedKey + "=" + encodedValue + "&");
                }
            }

            // Nếu có dữ liệu thì xóa dấu & thừa ở cuối chuỗi data
            if (data.Length > 0)
                data.Length -= 1;

            // Nếu có dữ liệu thì xóa dấu & thừa ở cuối chuỗi query
            if (query.Length > 0)
                query.Length -= 1;

            // Chuỗi dùng để ký HMAC (toàn bộ param đã sort + encode, chưa có vnp_SecureHash)
            string signData = data.ToString();

            // Tạo chữ ký bảo mật vnp_SecureHash bằng HMAC SHA512
            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, signData);

            // Trả về URL hoàn chỉnh:
            // baseUrl?các_param...&vnp_SecureHash=chuoi_chu_ky
            return baseUrl + "?" + query + "&vnp_SecureHash=" + vnp_SecureHash;
        }

        // Hàm tạo chữ ký HMAC SHA512
        // key: vnp_HashSecret do VNPay cấp
        // inputData: chuỗi tham số đã sort & format đúng chuẩn để ký
        private string HmacSHA512(string key, string inputData)
        {
            // Dùng StringBuilder để build chuỗi hexa kết quả
            var hash = new StringBuilder();

            // Chuyển key và inputData sang mảng byte với UTF8
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);

            // Tạo đối tượng HMACSHA512 với keyBytes
            using (var hmac = new HMACSHA512(keyBytes))
            {
                // Tính toán hash HMAC từ inputBytes
                byte[] hashValue = hmac.ComputeHash(inputBytes);

                // Mỗi byte chuyển thành 2 ký tự hexa (x2 → dạng "0a", "ff", ...)
                foreach (var theByte in hashValue)
                    hash.Append(theByte.ToString("x2")); // "x2" → chữ thường, 2 chữ số hexa
            }

            // Trả về chuỗi hash dạng hexa (chính là vnp_SecureHash)
            return hash.ToString();
        }
    }

    // Class dùng để định nghĩa cách so sánh chuỗi key trong SortedList
    // VNPay yêu cầu sort tham số theo thứ tự alphabet (tăng dần) trước khi ký
    public class VnPayCompare : IComparer<string>
    {
        // Hàm so sánh hai chuỗi x và y
        public int Compare(string x, string y)
        {
            // Nếu giống nhau, trả về 0
            if (x == y) return 0;
            // Nếu x null thì nhỏ hơn y
            if (x == null) return -1;
            // Nếu y null thì x lớn hơn
            if (y == null) return 1;

            // Lấy bộ so sánh chuỗi theo ngôn ngữ en-US
            var compare = CompareInfo.GetCompareInfo("en-US");

            // So sánh theo kiểu Ordinal (so sánh từng mã ký tự, không phụ thuộc locale)
            return compare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
