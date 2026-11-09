using System;
using System.Windows.Forms;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.GUI
{
    public partial class frmDoiMatKhau : Form
    {
        public frmDoiMatKhau()
        {
            InitializeComponent();
        }

        private void btnDoi_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileXml.LayGiaTri("TaiKhoan", "TenDangNhap", Program.TenDangNhap, "MatKhau") != txtMatKhauCu.Text)
                {
                    MessageBox.Show("Mật khẩu cũ không đúng.", "Thông báo");
                    return;
                }
                if (txtMatKhauMoi.Text == "" || txtMatKhauMoi.Text != txtNhapLai.Text)
                {
                    MessageBox.Show("Mật khẩu mới trống hoặc nhập lại không khớp.", "Thông báo");
                    return;
                }
                FileXml.Sua("TaiKhoan", "TenDangNhap", Program.TenDangNhap,
                    new[] { "MatKhau" }, new[] { txtMatKhauMoi.Text });
                MessageBox.Show("Đổi mật khẩu thành công.", "Thông báo");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
