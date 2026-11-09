using System;
using System.Windows.Forms;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.GUI
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string ten = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text;
            if (ten == "" || matKhau == "")
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu.", "Thông báo");
                return;
            }
            try
            {
                // Tìm tài khoản trong TaiKhoan.xml bằng XPath và so sánh mật khẩu
                if (FileXml.LayGiaTri("TaiKhoan", "TenDangNhap", ten, "MatKhau") != matKhau)
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.", "Đăng nhập thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Clear();
                    txtMatKhau.Focus();
                    return;
                }
                Program.TenDangNhap = ten;
                Program.Quyen = FileXml.LayGiaTri("TaiKhoan", "TenDangNhap", ten, "Quyen");
                Program.MaNV = FileXml.LayGiaTri("TaiKhoan", "TenDangNhap", ten, "MaNV");
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không đọc được TaiKhoan.xml: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
