using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.Pages
{
    public class HoaDonModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public DateTime? Tu { get; set; }
        [BindProperty(SupportsGet = true)] public DateTime? Den { get; set; }
        [BindProperty(SupportsGet = true)] public string Ma { get; set; }

        public List<DataRow> DanhSach, ChiTiet;
        public DataRow DangChon;
        public decimal DoanhThu;
        public Dictionary<string, string> TenKhach, TenNhanVien, TenTraiCay;

        public void OnGet()
        {
            DanhSach = FileXml.HienThi("HoaDon").AsEnumerable()
                .Where(r => (Tu == null || r.Field<DateTime>("NgayLap") >= Tu) && (Den == null || r.Field<DateTime>("NgayLap") <= Den))
                .OrderByDescending(r => r.Field<DateTime>("NgayLap")).ThenByDescending(r => r.Field<string>("MaHD")).ToList();
            DoanhThu = DanhSach.Sum(r => r.Field<decimal>("TongTien"));
            TenKhach = DinhDang.TuDien("KhachHang", "MaKH", "TenKH");
            TenNhanVien = DinhDang.TuDien("NhanVien", "MaNV", "HoTen");
            TenTraiCay = DinhDang.TuDien("TraiCay", "MaTC", "TenTC");

            DangChon = DanhSach.FirstOrDefault(r => r.Field<string>("MaHD") == Ma) ?? DanhSach.FirstOrDefault();
            string maHD = DangChon == null ? null : DangChon.Field<string>("MaHD");
            ChiTiet = FileXml.HienThi("ChiTietHoaDon").AsEnumerable().Where(r => r.Field<string>("MaHD") == maHD).ToList();
        }

        public IActionResult OnPostXoa(string maXoa)
        {
            if (!User.LaAdmin()) return Forbid();
            HoaDon.XoaHoaDon(maXoa);
            TempData["ThongBao"] = "Đã xóa hóa đơn " + maXoa + " và hoàn lại tồn kho.";
            return RedirectToPage();
        }
    }
}
