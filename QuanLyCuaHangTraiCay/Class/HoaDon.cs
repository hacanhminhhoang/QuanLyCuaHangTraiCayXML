using System;
using System.Data;
using System.Globalization;

namespace QuanLyCuaHangTraiCay.Class
{
    /// <summary>Nghiệp vụ bán hàng: lưu hóa đơn (HoaDon.xml + ChiTietHoaDon.xml) và cập nhật tồn kho (TraiCay.xml).</summary>
    public static class HoaDon
    {
        static readonly CultureInfo IC = CultureInfo.InvariantCulture;

        /// <summary>gio: bảng có các cột MaTC, SoLuong (int), DonGia (decimal). Trả về tổng tiền.</summary>
        public static decimal LuuHoaDon(string maHD, DateTime ngayLap, string maNV, string maKH, DataTable gio)
        {
            lock (FileXml.KhoaGhi)   // kiểm tra tồn kho và ghi 3 file trong cùng 1 khóa
            {
                if (gio.Rows.Count == 0) throw new Exception("Hóa đơn chưa có trái cây nào.");

                // 1. Kiểm tra tồn kho cho tất cả các dòng trước khi ghi bất cứ thứ gì
                decimal tong = 0;
                foreach (DataRow r in gio.Rows)
                {
                    string maTC = r["MaTC"].ToString();
                    int soLuong = (int)r["SoLuong"];
                    int ton = int.Parse(FileXml.LayGiaTri("TraiCay", "MaTC", maTC, "SoLuongTon"));
                    if (soLuong > ton)
                        throw new Exception("Trái cây " + maTC + " chỉ còn " + ton + ", không đủ bán " + soLuong + ".");
                    tong += soLuong * (decimal)r["DonGia"];
                }

                // 2. Ghi hóa đơn, chi tiết và trừ tồn kho
                // ponytail: ghi 3 file XML lần lượt, không có transaction; lỗi giữa chừng cần sửa tay file XML
                FileXml.Them("HoaDon", new[] { "MaHD", "NgayLap", "MaNV", "MaKH", "TongTien" },
                    new[] { maHD, ngayLap.ToString("yyyy-MM-dd"), maNV, maKH, tong.ToString(IC) });
                foreach (DataRow r in gio.Rows)
                {
                    string maTC = r["MaTC"].ToString();
                    int soLuong = (int)r["SoLuong"];
                    decimal donGia = (decimal)r["DonGia"];
                    FileXml.Them("ChiTietHoaDon", new[] { "MaHD", "MaTC", "SoLuong", "DonGia", "ThanhTien" },
                        new[] { maHD, maTC, soLuong.ToString(), donGia.ToString(IC), (soLuong * donGia).ToString(IC) });
                    CongTonKho(maTC, -soLuong);
                }
                return tong;
            }
        }

        /// <summary>Xóa hóa đơn và chi tiết, hoàn lại số lượng vào kho.</summary>
        public static void XoaHoaDon(string maHD)
        {
            lock (FileXml.KhoaGhi)   // kiểm tra tồn kho và ghi 3 file trong cùng 1 khóa
            {
                foreach (DataRow r in FileXml.HienThi("ChiTietHoaDon").Select())
                    if (r["MaHD"].ToString() == maHD && FileXml.KiemTraMa("TraiCay", "MaTC", r["MaTC"].ToString()))
                        CongTonKho(r["MaTC"].ToString(), (int)r["SoLuong"]);
                FileXml.Xoa("ChiTietHoaDon", "MaHD", maHD);
                FileXml.Xoa("HoaDon", "MaHD", maHD);
            }
        }

        /// <summary>Cộng (soLuong dương) hoặc trừ (âm) tồn kho của một trái cây trong TraiCay.xml.</summary>
        internal static void CongTonKho(string maTC, int soLuong)
        {
            int ton = int.Parse(FileXml.LayGiaTri("TraiCay", "MaTC", maTC, "SoLuongTon"));
            FileXml.Sua("TraiCay", "MaTC", maTC, new[] { "SoLuongTon" }, new[] { (ton + soLuong).ToString() });
        }
    }
}
