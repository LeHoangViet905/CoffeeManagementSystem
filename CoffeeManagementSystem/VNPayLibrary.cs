using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace CoffeeManagementSystem
{
    public class VnPayLibrary
    {
        private readonly SortedList<string, string> _requestData =
            new SortedList<string, string>(new VnPayCompare());

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
                _requestData.Add(key, value);
        }

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            var data = new StringBuilder();
            var query = new StringBuilder();

            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    string encodedValue = WebUtility.UrlEncode(kv.Value);
                    string encodedKey = WebUtility.UrlEncode(kv.Key);

                    data.Append(encodedKey + "=" + encodedValue + "&");
                    query.Append(encodedKey + "=" + encodedValue + "&");
                }
            }

            if (data.Length > 0)
                data.Length -= 1;
            if (query.Length > 0)
                query.Length -= 1;

            string signData = data.ToString();
            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, signData);

            return baseUrl + "?" + query + "&vnp_SecureHash=" + vnp_SecureHash;
        }

        private string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);

            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                    hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            var compare = CompareInfo.GetCompareInfo("en-US");
            return compare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
