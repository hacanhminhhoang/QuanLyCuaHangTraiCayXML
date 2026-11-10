using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.Pages
{
    public class DoiMatKhauModel : PageModel
    {
        public string Loi;

        public IActionResult OnPost(string matKhauCu, string matKhauMoi, string nhapLai)
        {
            string ten = User.Identity.Name;
            if (FileXml.LayGiaTri("TaiKhoan", "TenDangNhap", ten, "MatKhau") != matKhauCu)
                Loi = "Mật khẩu hiện tại không đúng.";
            else if (string.IsNullOrEmpty(matKhauMoi) || matKhauMoi != nhapLai)
                Loi = "Mật khẩu mới trống hoặc nhập lại không khớp.";
            if (Loi != null) return Page();
            try
            {
                FileXml.Sua("TaiKhoan", "TenDangNhap", ten, new[] { "MatKhau" }, new[] { matKhauMoi });
            }
            catch (Exception ex)
            {
                Loi = ex.Message;
                return Page();
            }
            TempData["ThongBao"] = "Đổi mật khẩu thành công.";
            return RedirectToPage("/Index");
        }
    }
}
