using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.GUI
{
    public partial class frmHoaDon : Form
    {
        // Giỏ hàng của hóa đơn đang lập
        readonly DataTable gio = new DataTable();

        public frmHoaDon()
        {
            InitializeComponent();
            gio.Columns.Add("MaTC", typeof(string));
            gio.Columns.Add("TenTC", typeof(string));
            gio.Columns.Add("SoLuong", typeof(int));
            gio.Columns.Add("DonGia", typeof(decimal));
            gio.Columns.Add("ThanhTien", typeof(decimal), "SoLuong * DonGia");   // cột tính toán
        }

        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            try
            {
                cboKhachHang.DisplayMember = "TenKH";
                cboKhachHang.ValueMember = "MaKH";
                cboKhachHang.DataSource = FileXml.HienThi("KhachHang");
                cboTraiCay.DisplayMember = "TenTC";
                cboTraiCay.ValueMember = "MaTC";
                cboTraiCay.DataSource = FileXml.HienThi("TraiCay");
                txtNhanVien.Text = FileXml.LayGiaTri("NhanVien", "MaNV", Program.MaNV, "HoTen");

                dgvGio.DataSource = gio;
                dgvGio.Columns["MaTC"].HeaderText = "Mã";
                dgvGio.Columns["TenTC"].HeaderText = "Trái cây";
                dgvGio.Columns["SoLuong"].HeaderText = "Số lượng";
                dgvGio.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvGio.Columns["ThanhTien"].HeaderText = "Thành tiền";
                dgvGio.Columns["DonGia"].DefaultCellStyle.Format = "#,##0";
                dgvGio.Columns["ThanhTien"].DefaultCellStyle.Format = "#,##0";
                HoaDonMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void HoaDonMoi()
        {
            txtMaHD.Text = FileXml.TaoMaMoi("HoaDon", "MaHD", "HD");
            dtpNgayLap.Value = DateTime.Today;
            gio.Clear();
            numSoLuong.Value = 1;
            btnLuu.Enabled = true;
            btnIn.Enabled = false;
            CapNhatTongTien();
        }

        void CapNhatTongTien()
        {
            decimal tong = gio.AsEnumerable().Sum(r => r.Field<decimal>("ThanhTien"));
            lblTongTien.Text = "Tổng tiền: " + tong.ToString("#,##0") + " đ";
        }

        private void cboTraiCay_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView tc = cboTraiCay.SelectedItem as DataRowView;
            lblThongTin.Text = tc == null ? "" :
                "Đơn giá: " + ((decimal)tc["DonGia"]).ToString("#,##0") + " đ/" + tc["DonViTinh"] +
                "   -   Tồn kho: " + tc["SoLuongTon"];
        }

        private void btnThemVaoGio_Click(object sender, EventArgs e)
        {
            DataRowView tc = cboTraiCay.SelectedItem as DataRowView;
            if (tc == null || !btnLuu.Enabled) return;
            string maTC = tc["MaTC"].ToString();
            int soLuong = (int)numSoLuong.Value;
            DataRow dong = gio.AsEnumerable().FirstOrDefault(r => r.Field<string>("MaTC") == maTC);
            int tongSoLuong = soLuong + (dong == null ? 0 : (int)dong["SoLuong"]);
            if (tongSoLuong > (int)tc["SoLuongTon"])
            {
                MessageBox.Show("Không đủ hàng. Tồn kho chỉ còn " + tc["SoLuongTon"] + ".", "Thông báo");
                return;
            }
            if (dong == null)
                gio.Rows.Add(maTC, tc["TenTC"], soLuong, tc["DonGia"]);
            else
                dong["SoLuong"] = tongSoLuong;
            CapNhatTongTien();
        }

        private void btnXoaKhoiGio_Click(object sender, EventArgs e)
        {
            if (dgvGio.CurrentRow == null || !btnLuu.Enabled) return;
            gio.Rows.RemoveAt(dgvGio.CurrentRow.Index);
            CapNhatTongTien();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cboKhachHang.SelectedValue == null)
            {
                MessageBox.Show("Hãy chọn khách hàng.", "Thông báo");
                return;
            }
            try
            {
                decimal tong = HoaDon.LuuHoaDon(txtMaHD.Text, dtpNgayLap.Value, Program.MaNV,
                    cboKhachHang.SelectedValue.ToString(), gio);
                MessageBox.Show("Đã lưu hóa đơn " + txtMaHD.Text + ", tổng tiền " + tong.ToString("#,##0") + " đ.", "Thành công");
                btnLuu.Enabled = false;
                btnIn.Enabled = true;
                cboTraiCay.DataSource = FileXml.HienThi("TraiCay");   // nạp lại tồn kho mới
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không lưu được", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(FileXml.XuatHTML("HoaDon", "HoaDon", "MaHD", txtMaHD.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMoi_Click(object sender, EventArgs e)
        {
            HoaDonMoi();
        }
    }
}
