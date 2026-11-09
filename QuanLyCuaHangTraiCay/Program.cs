using System;
using System.Windows.Forms;
using QuanLyCuaHangTraiCay.GUI;

namespace QuanLyCuaHangTraiCay
{
    static class Program
    {
        // Thông tin tài khoản đang đăng nhập
        public static string TenDangNhap;
        public static string MaNV;
        public static string Quyen;
        public static bool LaAdmin { get { return Quyen == "Admin"; } }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Đăng nhập -> form chính; nếu người dùng chọn "Đăng xuất" thì quay lại form đăng nhập
            while (true)
            {
                using (frmDangNhap dangNhap = new frmDangNhap())
                {
                    if (dangNhap.ShowDialog() != DialogResult.OK) return;
                }
                frmMain main = new frmMain();
                Application.Run(main);
                if (!main.DangXuat) return;
            }
        }
    }
}
