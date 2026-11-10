using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.Pages
{
    public class DangNhapModel : PageModel
    {
        [BindProperty] public string TenDangNhap { get; set; }
        [BindProperty] public string MatKhau { get; set; }
        [BindProperty(SupportsGet = true)] public string ReturnUrl { get; set; }
        public string Loi;

        public async Task<IActionResult> OnPostAsync()
        {
            string ten = (TenDangNhap ?? "").Trim();
            // Tìm tài khoản trong TaiKhoan.xml bằng XPath rồi so sánh mật khẩu
            string daBam = FileXml.LayGiaTri("TaiKhoan", "TenDangNhap", ten, "MatKhau");
            if (ten == "" || daBam == "" || daBam != Class.MatKhau.Bam(ten, MatKhau ?? ""))
            {
                Loi = "Sai tên đăng nhập hoặc mật khẩu.";
                return Page();
            }
            string maNV = FileXml.LayGiaTri("TaiKhoan", "TenDangNhap", ten, "MaNV");
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, ten),
                new Claim(ClaimTypes.Role, FileXml.LayGiaTri("TaiKhoan", "TenDangNhap", ten, "Quyen")),
                new Claim("MaNV", maNV),
                new Claim("HoTen", FileXml.LayGiaTri("NhanVien", "MaNV", maNV, "HoTen")),
            };
            await HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
            return LocalRedirect(Url.IsLocalUrl(ReturnUrl) ? ReturnUrl : "/");
        }
    }
}
