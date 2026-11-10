using System.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.Pages
{
    public class IndexModel : PageModel
    {
        public const int NguongSapHet = 20;
        public int SoTraiCay, SoKhachHang, SoHoaDonThang;
        public decimal DoanhThuThang, DoanhThuHomNay;
        public List<DataRow> SapHetHang, HoaDonGanDay;
        public Dictionary<string, string> TenKhach;

        public void OnGet()
        {
            DataTable traiCay = FileXml.HienThi("TraiCay");
            DataTable hoaDon = FileXml.HienThi("HoaDon");
            DateTime homNay = DateTime.Today;
            var trongThang = hoaDon.AsEnumerable().Where(r => r.Field<DateTime>("NgayLap") >= new DateTime(homNay.Year, homNay.Month, 1)).ToList();

            SoTraiCay = traiCay.Rows.Count;
            SoKhachHang = FileXml.HienThi("KhachHang").Rows.Count;
            SoHoaDonThang = trongThang.Count;
            DoanhThuThang = trongThang.Sum(r => r.Field<decimal>("TongTien"));
            DoanhThuHomNay = trongThang.Where(r => r.Field<DateTime>("NgayLap") == homNay).Sum(r => r.Field<decimal>("TongTien"));
            SapHetHang = traiCay.AsEnumerable().Where(r => r.Field<int>("SoLuongTon") < NguongSapHet)
                .OrderBy(r => r.Field<int>("SoLuongTon")).ToList();
            HoaDonGanDay = hoaDon.AsEnumerable().OrderByDescending(r => r.Field<DateTime>("NgayLap"))
                .ThenByDescending(r => r.Field<string>("MaHD")).Take(5).ToList();
            TenKhach = DinhDang.TuDien("KhachHang", "MaKH", "TenKH");
        }
    }
}
