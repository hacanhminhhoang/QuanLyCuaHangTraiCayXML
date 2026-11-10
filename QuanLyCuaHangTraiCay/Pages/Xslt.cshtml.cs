using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.Pages
{
    /// <summary>Hiển thị trình duyệt: biến đổi file XML bằng XSLT và trả HTML về cho trình duyệt.</summary>
    public class XsltModel : PageModel
    {
        public IActionResult OnGet(string bang, string xslt, string tuKhoa, string maHD)
        {
            string html;
            if (xslt == "HoaDon")
                html = FileXml.XuatHTML("HoaDon", "HoaDon", "MaHD", maHD ?? "");
            else
            {
                // Chỉ nhận tên bảng nằm trong danh sách danh mục (không cho đọc file tùy ý)
                CauHinhDanhMuc ch;
                if (bang == null || !DanhMucModel.CacDanhMuc.TryGetValue(bang, out ch)) return NotFound();
                if (ch.ChiAdmin && !User.LaAdmin()) return Forbid();
                html = FileXml.XuatHTML(bang, "DanhSach", "TieuDe", "DANH SÁCH " + ch.TieuDe.ToUpper(), "TuKhoa", tuKhoa ?? "");
            }
            return Content(html, "text/html; charset=utf-8");
        }
    }
}
