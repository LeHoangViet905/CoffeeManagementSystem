using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeManagementSystem
{
    public static class VietnameseNumberReader
    {
        private static readonly string[] ChuSo =
            { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };

        private static string DocHangTram(int so, bool docDayDu)
        {
            string result = "";
            int tram = so / 100;
            int chuc = (so % 100) / 10;
            int donVi = so % 10;

            if (tram > 0 || docDayDu)
            {
                result += ChuSo[tram] + " trăm";
                if (chuc == 0 && donVi > 0)
                {
                    result += " lẻ";
                }
            }

            if (chuc > 1)
            {
                result += (result == "" ? "" : " ") + ChuSo[chuc] + " mươi";
                if (donVi == 1)
                    result += " mốt";
                else if (donVi == 4)
                    result += " bốn";
                else if (donVi == 5)
                    result += " lăm";
                else if (donVi > 1)
                    result += " " + ChuSo[donVi];
            }
            else if (chuc == 1)
            {
                result += (result == "" ? "" : " ") + "mười";
                if (donVi == 1)
                    result += " một";
                else if (donVi == 4)
                    result += " bốn";
                else if (donVi == 5)
                    result += " lăm";
                else if (donVi > 1)
                    result += " " + ChuSo[donVi];
            }
            else if (chuc == 0 && donVi > 0)
            {
                if (tram > 0 || docDayDu)
                {
                    if (donVi == 1)
                        result += " một";
                    else if (donVi == 4)
                        result += " bốn";
                    else if (donVi == 5)
                        result += " lăm";
                    else
                        result += " " + ChuSo[donVi];
                }
                else
                {
                    result += ChuSo[donVi];
                }
            }

            return result;
        }

        public static string DocSoTien(long soTien)
        {
            if (soTien == 0) return "không";

            if (soTien < 0)
            {
                return "âm " + DocSoTien(-soTien);
            }

            string[] donViNhom = { "", " nghìn", " triệu", " tỷ" };
            int nhom = 0;
            string ketQua = "";
            bool docDayDu = false;

            while (soTien > 0 && nhom < donViNhom.Length)
            {
                int block = (int)(soTien % 1000);
                if (block != 0)
                {
                    string blockText = DocHangTram(block, docDayDu);
                    if (!string.IsNullOrEmpty(blockText))
                    {
                        ketQua = blockText + donViNhom[nhom] +
                                 (ketQua == "" ? "" : " " + ketQua);
                    }
                    docDayDu = true;
                }
                soTien /= 1000;
                nhom++;
            }

            return ketQua.Trim();
        }
    }
}