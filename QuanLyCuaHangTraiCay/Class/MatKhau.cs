using System.Security.Cryptography;
using System.Text;

namespace QuanLyCuaHangTraiCay.Class
{
    /// <summary>Băm mật khẩu để TaiKhoan.xml không lưu mật khẩu thường.</summary>
    public static class MatKhau
    {
        /// <summary>SHA-256 (64 ký tự hex thường) của "tenDangNhap:matKhau".
        /// Tên đăng nhập đóng vai trò "muối": hai tài khoản cùng mật khẩu sẽ có hai chuỗi băm khác nhau.</summary>
        // ponytail: SHA-256 có muối cố định theo tên đăng nhập, đủ cho đồ án; hệ thống thật nên dùng PBKDF2/bcrypt (muối ngẫu nhiên, chậm có chủ đích)
        public static string Bam(string tenDangNhap, string matKhau)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bam = sha.ComputeHash(Encoding.UTF8.GetBytes(tenDangNhap + ":" + matKhau));
                StringBuilder sb = new StringBuilder(64);
                foreach (byte b in bam) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
