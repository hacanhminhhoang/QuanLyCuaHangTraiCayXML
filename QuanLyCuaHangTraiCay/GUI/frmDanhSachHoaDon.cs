using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.GUI
{
    public partial class frmDanhSachHoaDon : Form
    {
        public frmDanhSachHoaDon()
        {
            InitializeComponent();
        }

        private void frmDanhSachHoaDon_Load(object sender, EventArgs e)
        {
            dtpTu.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpDen.Value = DateTime.Today;
            btnXoa.Enabled = Program.LaAdmin;   // chỉ Admin được xóa hóa đơn
            NapHoaDon(false);
        }

        // Tạo từ điển mã -> tên, vd: KH002 -> Phạm Thu Hà
        static Dictionary<string, string> TuDien(string bang, string khoa, string ten)
        {
            return FileXml.HienThi(bang).AsEnumerable()
                .ToDictionary(r => r[khoa].ToString(), r => r[ten].ToString());
        }

        static string Ten(Dictionary<string, string> tuDien, string ma)
        {
            string ten;
            return tuDien.TryGetValue(ma, out ten) ? ten : ma;
        }

        void NapHoaDon(bool locTheoNgay)
        {
            try
            {
                DataTable dt = FileXml.HienThi("HoaDon");
                if (locTheoNgay)
                {
                    foreach (DataRow r in dt.Select())
                    {
                        DateTime ngay = (DateTime)r["NgayLap"];
                        if (ngay < dtpTu.Value.Date || ngay > dtpDen.Value.Date) r.Delete();
                    }
                    dt.AcceptChanges();
                }
                Dictionary<string, string> kh = TuDien("KhachHang", "MaKH", "TenKH");
                Dictionary<string, string> nv = TuDien("NhanVien", "MaNV", "HoTen");
                dt.Columns.Add("TenKH");
                dt.Columns.Add("TenNV");
                foreach (DataRow r in dt.Rows)
                {
                    r["TenKH"] = Ten(kh, r["MaKH"].ToString());
                    r["TenNV"] = Ten(nv, r["MaNV"].ToString());
                }

                dgvHoaDon.DataSource = dt;
                dgvHoaDon.Columns["MaHD"].HeaderText = "Mã HĐ";
                dgvHoaDon.Columns["NgayLap"].HeaderText = "Ngày lập";
                dgvHoaDon.Columns["NgayLap"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvHoaDon.Columns["MaNV"].Visible = false;
                dgvHoaDon.Columns["MaKH"].Visible = false;
                dgvHoaDon.Columns["TenKH"].HeaderText = "Khách hàng";
                dgvHoaDon.Columns["TenNV"].HeaderText = "Nhân viên";
                dgvHoaDon.Columns["TongTien"].HeaderText = "Tổng tiền";
                dgvHoaDon.Columns["TongTien"].DefaultCellStyle.Format = "#,##0";

                decimal doanhThu = dt.AsEnumerable().Sum(r => r.Field<decimal>("TongTien"));
                lblDoanhThu.Text = dt.Rows.Count + " hóa đơn - Doanh thu: " + doanhThu.ToString("#,##0") + " đ";
                NapChiTiet();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        string MaHDDangChon()
        {
            return dgvHoaDon.CurrentRow == null ? null : dgvHoaDon.CurrentRow.Cells["MaHD"].Value.ToString();
        }

        void NapChiTiet()
        {
            string maHD = MaHDDangChon();
            DataTable ct = FileXml.HienThi("ChiTietHoaDon");
            foreach (DataRow r in ct.Select())
                if (r["MaHD"].ToString() != maHD) r.Delete();
            ct.AcceptChanges();
            Dictionary<string, string> tc = TuDien("TraiCay", "MaTC", "TenTC");
            ct.Columns.Add("TenTC");
            foreach (DataRow r in ct.Rows)
                r["TenTC"] = Ten(tc, r["MaTC"].ToString());

            dgvChiTiet.DataSource = ct;
            dgvChiTiet.Columns["MaHD"].Visible = false;
            dgvChiTiet.Columns["MaTC"].HeaderText = "Mã";
            dgvChiTiet.Columns["TenTC"].HeaderText = "Trái cây";
            dgvChiTiet.Columns["TenTC"].DisplayIndex = 2;
            dgvChiTiet.Columns["SoLuong"].HeaderText = "Số lượng";
            dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn giá";
            dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Format = "#,##0";
            dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành tiền";
            dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "#,##0";
        }

        private void dgvHoaDon_SelectionChanged(object sender, EventArgs e)
        {
            NapChiTiet();
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            NapHoaDon(true);
        }

        private void btnTatCa_Click(object sender, EventArgs e)
        {
            NapHoaDon(false);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maHD = MaHDDangChon();
            if (maHD == null) return;
            if (MessageBox.Show("Xóa hóa đơn " + maHD + " và hoàn lại số lượng vào kho?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                HoaDon.XoaHoaDon(maHD);
                NapHoaDon(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không xóa được", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            string maHD = MaHDDangChon();
            if (maHD == null) return;
            try
            {
                Process.Start(FileXml.XuatHTML("HoaDon", "HoaDon", "MaHD", maHD));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
