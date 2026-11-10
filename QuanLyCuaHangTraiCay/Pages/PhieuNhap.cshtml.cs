using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.Pages
{
    public class PhieuNhapModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public DateTime? Tu { get; set; }
        [BindProperty(SupportsGet = true)] public DateTime? Den { get; set; }
        [BindProperty(SupportsGet = true)] public string Ma { get; set; }

        public List<DataRow> DanhSach, ChiTiet;
        public DataRow DangChon;
        public decimal TongNhap;
        public Dictionary<string, string> TenNCC, TenNhanVien, TenTraiCay;

        public void OnGet()
        {
            DanhSach = FileXml.HienThi("PhieuNhap").AsEnumerable()
                .Where(r => (Tu == null || r.Field<DateTime>("NgayNhap") >= Tu) && (Den == null || r.Field<DateTime>("NgayNhap") <= Den))
                .OrderByDescending(r => r.Field<DateTime>("NgayNhap")).ThenByDescending(r => r.Field<string>("MaPN")).ToList();
            TongNhap = DanhSach.Sum(r => r.Field<decimal>("TongTien"));
            TenNCC = DinhDang.TuDien("NhaCungCap", "MaNCC", "TenNCC");
            TenNhanVien = DinhDang.TuDien("NhanVien", "MaNV", "HoTen");
            TenTraiCay = DinhDang.TuDien("TraiCay", "MaTC", "TenTC");

            DangChon = DanhSach.FirstOrDefault(r => r.Field<string>("MaPN") == Ma) ?? DanhSach.FirstOrDefault();
            string maPN = DangChon == null ? null : DangChon.Field<string>("MaPN");
            ChiTiet = FileXml.HienThi("ChiTietPhieuNhap").AsEnumerable().Where(r => r.Field<string>("MaPN") == maPN).ToList();
        }

        public IActionResult OnPostXoa(string maXoa)
        {
            if (!User.LaAdmin()) return Forbid();
            try
            {
                PhieuNhap.Xoa(maXoa);
                TempData["ThongBao"] = "Đã xóa phiếu nhập " + maXoa + " và trừ lại tồn kho.";
            }
            catch (Exception ex)
            {
                TempData["Loi"] = ex.Message;   // vd: tồn kho không đủ để hoàn lại
            }
            return RedirectToPage();
        }
    }
}
