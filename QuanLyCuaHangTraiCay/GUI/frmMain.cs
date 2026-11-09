using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.GUI
{
    public partial class frmMain : Form
    {
        public bool DangXuat { get; private set; }

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblTrangThai.Text = "Người dùng: " + Program.TenDangNhap + " (" + Program.Quyen + ")";
            // Chức năng chỉ dành cho Admin
            mnuNhanVien.Visible = Program.LaAdmin;
            mnuTaiKhoan.Visible = Program.LaAdmin;
            mnuDuLieu.Visible = Program.LaAdmin;
        }

        // ---------------- Hệ thống ----------------
        private void mnuDoiMatKhau_Click(object sender, EventArgs e)
        {
            new frmDoiMatKhau().ShowDialog(this);
        }

        private void mnuDangXuat_Click(object sender, EventArgs e)
        {
            DangXuat = true;
            Close();
        }

        private void mnuThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ---------------- Danh mục (dùng chung frmDanhMuc) ----------------
        private void mnuTraiCay_Click(object sender, EventArgs e)
        {
            new frmDanhMuc("TraiCay", "Trái cây", "TC",
                new Cot("MaTC", "Mã trái cây"),
                new Cot("TenTC", "Tên trái cây"),
                new Cot("XuatXu", "Xuất xứ"),
                new Cot("DonViTinh", "Đơn vị tính") { LuaChon = new[] { "kg", "quả", "nải", "hộp" } },
                new Cot("DonGia", "Đơn giá (VNĐ)"),
                new Cot("SoLuongTon", "Số lượng tồn"),
                new Cot("MaNCC", "Nhà cung cấp") { BangLienKet = "NhaCungCap", CotHienThi = "TenNCC" }
            ).ShowDialog(this);
        }

        private void mnuNhaCungCap_Click(object sender, EventArgs e)
        {
            new frmDanhMuc("NhaCungCap", "Nhà cung cấp", "NCC",
                new Cot("MaNCC", "Mã NCC"),
                new Cot("TenNCC", "Tên nhà cung cấp"),
                new Cot("DiaChi", "Địa chỉ"),
                new Cot("SDT", "Số điện thoại")
            ).ShowDialog(this);
        }

        private void mnuKhachHang_Click(object sender, EventArgs e)
        {
            new frmDanhMuc("KhachHang", "Khách hàng", "KH",
                new Cot("MaKH", "Mã khách hàng"),
                new Cot("TenKH", "Tên khách hàng"),
                new Cot("SDT", "Số điện thoại"),
                new Cot("DiaChi", "Địa chỉ")
            ).ShowDialog(this);
        }

        private void mnuNhanVien_Click(object sender, EventArgs e)
        {
            new frmDanhMuc("NhanVien", "Nhân viên", "NV",
                new Cot("MaNV", "Mã nhân viên"),
                new Cot("HoTen", "Họ tên"),
                new Cot("GioiTinh", "Giới tính") { LuaChon = new[] { "Nam", "Nữ" } },
                new Cot("NgaySinh", "Ngày sinh") { LaNgay = true },
                new Cot("DiaChi", "Địa chỉ"),
                new Cot("SDT", "Số điện thoại")
            ).ShowDialog(this);
        }

        private void mnuTaiKhoan_Click(object sender, EventArgs e)
        {
            // Tiền tố null: tên đăng nhập do người dùng tự nhập
            new frmDanhMuc("TaiKhoan", "Tài khoản", null,
                new Cot("TenDangNhap", "Tên đăng nhập"),
                new Cot("MatKhau", "Mật khẩu"),
                new Cot("Quyen", "Quyền") { LuaChon = new[] { "Admin", "NhanVien" } },
                new Cot("MaNV", "Nhân viên") { BangLienKet = "NhanVien", CotHienThi = "HoTen" }
            ).ShowDialog(this);
        }

        // ---------------- Bán hàng ----------------
        private void mnuLapHoaDon_Click(object sender, EventArgs e)
        {
            new frmHoaDon().ShowDialog(this);
        }

        private void mnuDanhSachHoaDon_Click(object sender, EventArgs e)
        {
            new frmDanhSachHoaDon().ShowDialog(this);
        }

        // ---------------- Dữ liệu: đồng bộ SQL Server <-> XML, kiểm tra XSD ----------------
        private void mnuSqlSangXml_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ghi đè toàn bộ file XML bằng dữ liệu từ SQL Server?", "Database -> XML",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                FileXml.TaoXMLTuSQL();
                MessageBox.Show("Đã tạo " + FileXml.CacBang.Length + " file XML từ CSDL.", "Thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Database -> XML", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuXmlSangSql_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ghi đè dữ liệu trong SQL Server bằng dữ liệu từ các file XML?", "XML -> Database",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                FileXml.CapNhatSQL();
                MessageBox.Show("Đã cập nhật CSDL từ các file XML.", "Thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "XML -> Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuKiemTraHopLe_Click(object sender, EventArgs e)
        {
            List<string> ketQua = new List<string>();
            foreach (string bang in FileXml.CacBang)
            {
                List<string> loi = FileXml.KiemTraHopLe(bang);
                ketQua.Add((loi.Count == 0 ? "[Hợp lệ]     " : "[KHÔNG hợp lệ] ") + bang + ".xml");
                foreach (string l in loi) ketQua.Add("      " + l);
            }
            MessageBox.Show(string.Join("\n", ketQua), "Kiểm tra hợp lệ theo XSD");
        }
    }
}
