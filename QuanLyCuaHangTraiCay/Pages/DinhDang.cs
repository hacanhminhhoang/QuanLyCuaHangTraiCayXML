using System.Data;
using System.Globalization;
using System.Security.Claims;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.Pages
{
    /// <summary>Hàm hỗ trợ hiển thị dùng chung cho các trang.</summary>
    public static class DinhDang
    {
        static readonly CultureInfo VI = new CultureInfo("vi-VN");

        public static string Tien(object v) { return Convert.ToDecimal(v).ToString("N0", VI) + " đ"; }
        public static string So(object v) { return Convert.ToDecimal(v).ToString("N0", VI); }
        public static string Ngay(object v) { return v is DateTime ? ((DateTime)v).ToString("dd/MM/yyyy") : Convert.ToString(v); }

        /// <summary>Giá trị gốc để đặt vào ô input (số dạng 65000, ngày dạng yyyy-MM-dd).</summary>
        public static string Goc(object v)
        {
            if (v is DateTime) return ((DateTime)v).ToString("yyyy-MM-dd");
            IFormattable f = v as IFormattable;
            return f != null ? f.ToString(null, CultureInfo.InvariantCulture) : Convert.ToString(v);
        }

        /// <summary>Từ điển mã -> tên, vd: KH002 -> Phạm Thu Hà.</summary>
        public static Dictionary<string, string> TuDien(string bang, string khoa, string ten)
        {
            return FileXml.HienThi(bang).AsEnumerable().ToDictionary(r => r[khoa].ToString(), r => r[ten].ToString());
        }

        public static string Ten(this Dictionary<string, string> tuDien, object ma)
        {
            string ten;
            return tuDien.TryGetValue(Convert.ToString(ma), out ten) ? ten : Convert.ToString(ma);
        }

        public static string MaNV(this ClaimsPrincipal u) { return u.FindFirstValue("MaNV"); }
        public static string HoTen(this ClaimsPrincipal u) { return u.FindFirstValue("HoTen"); }
        public static bool LaAdmin(this ClaimsPrincipal u) { return u.IsInRole("Admin"); }
    }
}
