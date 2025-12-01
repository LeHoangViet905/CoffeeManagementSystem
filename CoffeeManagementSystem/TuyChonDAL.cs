using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

// Đổi namespace cho khớp với dự án của bạn
namespace CoffeeManagementSystem.DAL
{

    public class NhomTuyChon
    {
        public int MaNhom { get; set; }
        public string TenNhom { get; set; }      // Ví dụ: "Mức Đường"
        public bool ChonNhieu { get; set; }      // True: Topping (Checkbox), False: Đường/Đá (Radio)
    }

    public class ChiTietTuyChon
    {
        public int MaChiTiet { get; set; }
        public int MaNhom { get; set; }
        public string TenChiTiet { get; set; }   // Ví dụ: "50% Đường"
        public decimal GiaThem { get; set; }     // Ví dụ: 5000
    }
    // DTO dùng để chứa dữ liệu lấy lên
    public class OptionGroupDTO
    {
        public int MaNhom { get; set; }
        public string TenNhom { get; set; } // "Topping", "Mức Đường"
        public bool ChonNhieu { get; set; } // true: Checkbox, false: Radio
        public List<OptionItemDTO> Items { get; set; } = new List<OptionItemDTO>();
    }

    public class OptionItemDTO
    {
        public string TenChiTiet { get; set; } // "Trân châu đen"
        public decimal GiaThem { get; set; }   // 5000
    }

    public class TuyChonDAL : BaseDataAccess // Kế thừa để dùng lại ConnectionString
    {
        public TuyChonDAL() : base() { }

        // 1. Hàm lấy danh sách Nhóm (Ví dụ: Lấy ra "Mức Đường", "Topping")
        public List<NhomTuyChon> GetAllGroups()
        {
            List<NhomTuyChon> list = new List<NhomTuyChon>();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT * FROM NhomTuyChon";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new NhomTuyChon
                                {
                                    MaNhom = Convert.ToInt32(reader["MaNhom"]),
                                    TenNhom = reader["TenNhom"].ToString(),
                                    // SQLite lưu bool là 1 hoặc 0
                                    ChonNhieu = Convert.ToInt32(reader["ChonNhieu"]) == 1
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi (hoặc throw)
                    throw new Exception("Lỗi DAL lấy nhóm tùy chọn: " + ex.Message);
                }
            }
            return list;
        }

        // 2. Hàm thêm Nhóm mới (Dùng cho Admin)
        public void AddGroup(string tenNhom, bool chonNhieu)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string sql = "INSERT INTO NhomTuyChon (TenNhom, ChonNhieu) VALUES (@Ten, @Multi)";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Ten", tenNhom);
                    cmd.Parameters.AddWithValue("@Multi", chonNhieu ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // 5. Cập nhật tên nhóm và chế độ chọn
        public void UpdateGroup(int maNhom, string tenMoi, bool chonNhieu)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string sql = "UPDATE NhomTuyChon SET TenNhom = @Ten, ChonNhieu = @Multi WHERE MaNhom = @ID";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Ten", tenMoi);
                    cmd.Parameters.AddWithValue("@Multi", chonNhieu ? 1 : 0);
                    cmd.Parameters.AddWithValue("@ID", maNhom);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 6. Xóa nhóm (Lưu ý: Nếu DB có ON DELETE CASCADE, nó sẽ tự xóa các chi tiết con)
        public void DeleteGroup(int maNhom)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string sql = "DELETE FROM NhomTuyChon WHERE MaNhom = @ID";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", maNhom);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 3. Hàm lấy chi tiết của 1 nhóm (Ví dụ: Lấy "Ít đường", "Nhiều đường" của nhóm Đường)
        public List<ChiTietTuyChon> GetDetailsByGroupId(int maNhom)
        {
            List<ChiTietTuyChon> list = new List<ChiTietTuyChon>();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM ChiTietTuyChon WHERE MaNhom = @ID";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", maNhom);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ChiTietTuyChon
                            {
                                MaChiTiet = Convert.ToInt32(reader["MaChiTiet"]),
                                MaNhom = Convert.ToInt32(reader["MaNhom"]),
                                TenChiTiet = reader["TenChiTiet"].ToString(),
                                GiaThem = Convert.ToDecimal(reader["GiaThem"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        // 4. Hàm thêm chi tiết mới (Dùng cho Admin)
        public void AddDetail(int maNhom, string tenChiTiet, decimal giaThem)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string sql = "INSERT INTO ChiTietTuyChon (MaNhom, TenChiTiet, GiaThem) VALUES (@MaNhom, @Ten, @Gia)";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@MaNhom", maNhom);
                    cmd.Parameters.AddWithValue("@Ten", tenChiTiet);
                    cmd.Parameters.AddWithValue("@Gia", giaThem);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // 7. Cập nhật chi tiết
        public void UpdateDetail(int maChiTiet, string tenMoi, decimal giaMoi)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string sql = "UPDATE ChiTietTuyChon SET TenChiTiet = @Ten, GiaThem = @Gia WHERE MaChiTiet = @ID";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Ten", tenMoi);
                    cmd.Parameters.AddWithValue("@Gia", giaMoi);
                    cmd.Parameters.AddWithValue("@ID", maChiTiet);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 8. Xóa chi tiết
        public void DeleteDetail(int maChiTiet)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string sql = "DELETE FROM ChiTietTuyChon WHERE MaChiTiet = @ID";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", maChiTiet);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // 1. Lấy danh sách Mã món đã được gán cho nhóm này
        public List<string> GetProductIdsByGroupId(int maNhom)
        {
            List<string> list = new List<string>();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string sql = "SELECT Madouong FROM CauHinhMon WHERE MaNhom = @ID";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", maNhom);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader["Madouong"].ToString());
                        }
                    }
                }
            }
            return list;
        }

        // 2. Lưu cấu hình (Dùng Transaction cho an toàn)
        public void SaveGroupConfiguration(int maNhom, List<string> listMaDouongDuocChon)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (SQLiteTransaction trans = connection.BeginTransaction())
                {
                    try
                    {
                        // BƯỚC A: Xóa sạch các liên kết cũ của nhóm này (Reset)
                        string sqlDelete = "DELETE FROM CauHinhMon WHERE MaNhom = @NhomID";
                        using (SQLiteCommand cmdDel = new SQLiteCommand(sqlDelete, connection, trans))
                        {
                            cmdDel.Parameters.AddWithValue("@NhomID", maNhom);
                            cmdDel.ExecuteNonQuery();
                        }

                        // BƯỚC B: Thêm các món được tích chọn vào
                        string sqlInsert = "INSERT INTO CauHinhMon (Madouong, MaNhom) VALUES (@MonID, @NhomID)";
                        using (SQLiteCommand cmdIns = new SQLiteCommand(sqlInsert, connection, trans))
                        {
                            // Khai báo tham số 1 lần
                            var paramMon = cmdIns.Parameters.Add("@MonID", System.Data.DbType.String);
                            var paramNhom = cmdIns.Parameters.Add("@NhomID", System.Data.DbType.Int32);

                            paramNhom.Value = maNhom; // Gán MaNhom cố định

                            foreach (string maMon in listMaDouongDuocChon)
                            {
                                paramMon.Value = maMon; // Gán MaMon thay đổi
                                cmdIns.ExecuteNonQuery();
                            }
                        }

                        trans.Commit(); // Chốt đơn
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback(); // Có lỗi thì hoàn tác
                        throw new Exception("Lỗi khi lưu cấu hình: " + ex.Message);
                    }
                }
            }
        }
        // Hàm lấy cấu hình của 1 món
        public List<OptionGroupDTO> GetOptionsByProduct(string maDouong)
        {
            var result = new List<OptionGroupDTO>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                // Câu SQL JOIN 3 bảng để lấy: Nhóm -> Chi tiết của món đó
                string sql = @"
            SELECT n.MaNhom, n.TenNhom, n.ChonNhieu, 
                   c.TenChiTiet, c.GiaThem
            FROM CauHinhMon ch
            JOIN NhomTuyChon n ON ch.MaNhom = n.MaNhom
            JOIN ChiTietTuyChon c ON n.MaNhom = c.MaNhom
            WHERE ch.Madouong = @MaMon
            ORDER BY n.MaNhom"; // Sắp xếp theo nhóm

                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@MaMon", maDouong);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        // Xử lý dữ liệu phẳng thành danh sách lồng nhau (Group -> Items)
                        OptionGroupDTO currentGroup = null;

                        while (reader.Read())
                        {
                            int maNhom = Convert.ToInt32(reader["MaNhom"]);

                            // Nếu gặp nhóm mới (hoặc lần đầu)
                            if (currentGroup == null || currentGroup.MaNhom != maNhom)
                            {
                                currentGroup = result.Find(g => g.MaNhom == maNhom);
                                if (currentGroup == null)
                                {
                                    currentGroup = new OptionGroupDTO
                                    {
                                        MaNhom = maNhom,
                                        TenNhom = reader["TenNhom"].ToString(),
                                        ChonNhieu = Convert.ToInt32(reader["ChonNhieu"]) == 1
                                    };
                                    result.Add(currentGroup);
                                }
                            }

                            // Thêm chi tiết vào nhóm hiện tại
                            currentGroup.Items.Add(new OptionItemDTO
                            {
                                TenChiTiet = reader["TenChiTiet"].ToString(),
                                GiaThem = Convert.ToDecimal(reader["GiaThem"])
                            });
                        }
                    }
                }
            }
            return result;
        }
        public DataTable GetProductsWithConfig()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                // Kỹ thuật GROUP_CONCAT: Gộp nhiều dòng thành 1 dòng cách nhau bởi dấu phẩy
                string sql = @"
            SELECT 
                d.Madouong, 
                d.Tendouong, 
                GROUP_CONCAT(n.TenNhom, ', ') AS CacNhomDaCo
            FROM Douong d
            LEFT JOIN CauHinhMon ch ON d.Madouong = ch.Madouong
            LEFT JOIN NhomTuyChon n ON ch.MaNhom = n.MaNhom
            GROUP BY d.Madouong, d.Tendouong";

                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}