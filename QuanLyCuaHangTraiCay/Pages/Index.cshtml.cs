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
        public List<KeyValuePair<DateTime, decimal>> BayNgay;   // doanh thu từng ngày trong 7 ngày gần nhất (cũ -> mới)
        public decimal LonNhat7Ngay;
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
            BayNgay = Enumerable.Range(0, 7).Select(i => homNay.AddDays(i - 6))
                .Select(d => new KeyValuePair<DateTime, decimal>(d, hoaDon.AsEnumerable().Where(r => r.Field<DateTime>("NgayLap").Date == d).Sum(r => r.Field<decimal>("TongTien")))).ToList();
            LonNhat7Ngay = BayNgay.Max(x => x.Value);
        }
    }
}
