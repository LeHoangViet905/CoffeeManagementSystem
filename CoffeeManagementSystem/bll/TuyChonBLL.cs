using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeManagementSystem.DAL;

namespace CoffeeManagementSystem.bll
{
    public class TuyChonBLL
    {
        private TuyChonDAL _dal;

        public TuyChonBLL()
        {
            _dal = new TuyChonDAL();
        }

        // --- NHÓM TÙY CHỌN ---

        public List<NhomTuyChon> GetAllGroups()
        {
            return _dal.GetAllGroups();
        }

        public void AddGroup(string tenNhom, bool chonNhieu)
        {
            if (string.IsNullOrWhiteSpace(tenNhom))
                throw new Exception("Tên nhóm không được để trống!");

            _dal.AddGroup(tenNhom, chonNhieu);
        }

        public void UpdateGroup(int maNhom, string tenMoi, bool chonNhieu)
        {
            if (string.IsNullOrWhiteSpace(tenMoi))
                throw new Exception("Tên nhóm không được để trống!");

            _dal.UpdateGroup(maNhom, tenMoi, chonNhieu);
        }

        public void DeleteGroup(int maNhom)
        {
            _dal.DeleteGroup(maNhom);
        }

        // --- CHI TIẾT TÙY CHỌN ---

        public List<ChiTietTuyChon> GetDetailsByGroupId(int maNhom)
        {
            return _dal.GetDetailsByGroupId(maNhom);
        }

        public void AddDetail(int maNhom, string tenChiTiet, decimal giaThem)
        {
            if (string.IsNullOrWhiteSpace(tenChiTiet))
                throw new Exception("Tên chi tiết không được để trống!");

            if (giaThem < 0)
                throw new Exception("Giá thêm không được âm!");

            _dal.AddDetail(maNhom, tenChiTiet, giaThem);
        }

        public void UpdateDetail(int maChiTiet, string tenMoi, decimal giaMoi)
        {
            if (string.IsNullOrWhiteSpace(tenMoi))
                throw new Exception("Tên chi tiết không được để trống!");

            if (giaMoi < 0)
                throw new Exception("Giá thêm không được âm!");

            _dal.UpdateDetail(maChiTiet, tenMoi, giaMoi);
        }

        public void DeleteDetail(int maChiTiet)
        {
            _dal.DeleteDetail(maChiTiet);
        }
        public List<string> GetProductIdsByGroupId(int maNhom)
        {
            return _dal.GetProductIdsByGroupId(maNhom);
        }

        public void SaveGroupConfiguration(int maNhom, List<string> listMaDouong)
        {
            _dal.SaveGroupConfiguration(maNhom, listMaDouong);
        }
        public List<OptionGroupDTO> GetOptionsByProduct(string maDouong)
        {
            return _dal.GetOptionsByProduct(maDouong);
        }
    }
}
