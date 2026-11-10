using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.Pages
{
    public class LapHoaDonModel : PageModel
    {
        public DataTable TraiCay, KhachHang;
        public string MaHD, Loi;
        [BindProperty] public string MaKH { get; set; }
        [BindProperty] public DateTime NgayLap { get; set; } = DateTime.Today;
        public Dictionary<string, int> SoLuong = new Dictionary<string, int>();   // MaTC -> số lượng mua

        void NapDuLieu()
        {
            TraiCay = FileXml.HienThi("TraiCay");
            KhachHang = FileXml.HienThi("KhachHang");
            MaHD = FileXml.TaoMaMoi("HoaDon", "MaHD", "HD");
        }

        public void OnGet() { NapDuLieu(); }

        public IActionResult OnPost()
        {
            NapDuLieu();
            // Giỏ hàng = các dòng có số lượng > 0; đơn giá lấy lại từ TraiCay.xml, không tin dữ liệu gửi lên
            DataTable gio = new DataTable();
            gio.Columns.Add("MaTC", typeof(string));
            gio.Columns.Add("SoLuong", typeof(int));
            gio.Columns.Add("DonGia", typeof(decimal));
            foreach (DataRow tc in TraiCay.Rows)
            {
                string maTC = tc["MaTC"].ToString();
                int sl;
                if (int.TryParse(Request.Form["sl_" + maTC], out sl) && sl > 0)
                {
                    SoLuong[maTC] = sl;
                    gio.Rows.Add(maTC, sl, tc["DonGia"]);
                }
            }
            try
            {
                if (string.IsNullOrEmpty(MaKH) || !FileXml.KiemTraMa("KhachHang", "MaKH", MaKH))
                    throw new Exception("Hãy chọn khách hàng.");
                decimal tong = HoaDon.LuuHoaDon(MaHD, NgayLap, User.MaNV(), MaKH, gio);
                TempData["ThongBao"] = "Đã lưu hóa đơn " + MaHD + " — tổng tiền " + DinhDang.Tien(tong) + ".";
                TempData["InHoaDon"] = MaHD;
                return RedirectToPage("/HoaDon", new { ma = MaHD });
            }
            catch (Exception ex)
            {
                Loi = ex.Message;
                return Page();
            }
        }
    }
}
