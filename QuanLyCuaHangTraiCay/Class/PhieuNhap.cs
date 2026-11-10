using System;
using System.Data;
using System.Globalization;

namespace QuanLyCuaHangTraiCay.Class
{
    /// <summary>Nghiệp vụ nhập hàng: lưu phiếu nhập (PhieuNhap.xml + ChiTietPhieuNhap.xml) và cộng tồn kho (TraiCay.xml).</summary>
    public static class PhieuNhap
    {
        static readonly CultureInfo IC = CultureInfo.InvariantCulture;

        /// <summary>gio: bảng có các cột MaTC, SoLuong (int), DonGiaNhap (decimal). Trả về tổng tiền nhập.
        /// Mọi dòng phải là trái cây của đúng nhà cung cấp trên phiếu.</summary>
        public static decimal Luu(string maPN, DateTime ngayNhap, string maNV, string maNCC, DataTable gio)
        {
            lock (FileXml.KhoaGhi)   // kiểm tra và ghi 3 file trong cùng 1 khóa
            {
                if (!FileXml.KiemTraMa("NhaCungCap", "MaNCC", maNCC)) throw new Exception("Hãy chọn nhà cung cấp.");
                if (gio.Rows.Count == 0) throw new Exception("Phiếu nhập chưa có trái cây nào.");

                // 1. Kiểm tra tất cả các dòng trước khi ghi bất cứ thứ gì
                decimal tong = 0;
                foreach (DataRow r in gio.Rows)
                {
                    string maTC = r["MaTC"].ToString();
                    if (!FileXml.KiemTraMa("TraiCay", "MaTC", maTC)) throw new Exception("Không có trái cây " + maTC + ".");
                    if (FileXml.LayGiaTri("TraiCay", "MaTC", maTC, "MaNCC") != maNCC)
                        throw new Exception("Trái cây " + FileXml.LayGiaTri("TraiCay", "MaTC", maTC, "TenTC") +
                            " không thuộc nhà cung cấp " + FileXml.LayGiaTri("NhaCungCap", "MaNCC", maNCC, "TenNCC") + ".");
                    if ((int)r["SoLuong"] <= 0) throw new Exception("Số lượng nhập phải lớn hơn 0.");
                    if ((decimal)r["DonGiaNhap"] < 0) throw new Exception("Đơn giá nhập không được âm.");
                    tong += (int)r["SoLuong"] * (decimal)r["DonGiaNhap"];
                }

                // 2. Ghi phiếu, chi tiết và cộng tồn kho
                // ponytail: ghi 3 file XML lần lượt, không có transaction; lỗi giữa chừng cần sửa tay file XML
                FileXml.Them("PhieuNhap", new[] { "MaPN", "NgayNhap", "MaNV", "MaNCC", "TongTien" },
                    new[] { maPN, ngayNhap.ToString("yyyy-MM-dd"), maNV, maNCC, tong.ToString(IC) });
                foreach (DataRow r in gio.Rows)
                {
                    string maTC = r["MaTC"].ToString();
                    int soLuong = (int)r["SoLuong"];
                    decimal gia = (decimal)r["DonGiaNhap"];
                    FileXml.Them("ChiTietPhieuNhap", new[] { "MaPN", "MaTC", "SoLuong", "DonGiaNhap", "ThanhTien" },
                        new[] { maPN, maTC, soLuong.ToString(IC), gia.ToString(IC), (soLuong * gia).ToString(IC) });
                    HoaDon.CongTonKho(maTC, soLuong);
                }
                return tong;
            }
        }

        /// <summary>Xóa phiếu nhập và chi tiết, trừ lại số lượng khỏi kho.
        /// Từ chối nếu tồn kho hiện tại không đủ để trừ (một phần hàng đã được bán).</summary>
        public static void Xoa(string maPN)
        {
            lock (FileXml.KhoaGhi)
            {
                DataRow[] chiTiet = FileXml.HienThi("ChiTietPhieuNhap").Select("MaPN = '" + maPN.Replace("'", "''") + "'");
                foreach (DataRow r in chiTiet)
                {
                    string maTC = r["MaTC"].ToString();
                    int ton = int.Parse(FileXml.LayGiaTri("TraiCay", "MaTC", maTC, "SoLuongTon"));
                    if (ton < (int)r["SoLuong"])
                        throw new Exception("Không thể xóa phiếu " + maPN + ": tồn kho " + FileXml.LayGiaTri("TraiCay", "MaTC", maTC, "TenTC") +
                            " chỉ còn " + ton + ", ít hơn số đã nhập (" + r["SoLuong"] + "), vì một phần đã được bán.");
                }
                foreach (DataRow r in chiTiet)
                    HoaDon.CongTonKho(r["MaTC"].ToString(), -(int)r["SoLuong"]);
                FileXml.Xoa("ChiTietPhieuNhap", "MaPN", maPN);
                FileXml.Xoa("PhieuNhap", "MaPN", maPN);
            }
        }
    }
}
