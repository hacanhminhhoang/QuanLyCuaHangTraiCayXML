using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.GUI
{
    /// <summary>Mô tả 1 cột (1 thẻ con trong file XML) của danh mục.</summary>
    public class Cot
    {
        public string Ten;          // tên thẻ XML, vd: MaTC
        public string NhanDe;       // nhãn hiển thị, vd: Mã trái cây
        public string[] LuaChon;    // danh sách cố định -> ComboBox (vd: Nam/Nữ)
        public string BangLienKet;  // khóa ngoại -> ComboBox lấy dữ liệu từ file XML khác
        public string CotHienThi;   // cột hiển thị trong ComboBox khóa ngoại
        public bool LaNgay;         // true -> DateTimePicker

        public Cot(string ten, string nhanDe)
        {
            Ten = ten;
            NhanDe = nhanDe;
        }
    }

    /// <summary>
    /// Form dùng chung cho các danh mục: Trái cây, Nhà cung cấp, Khách hàng, Nhân viên, Tài khoản.
    /// Cột đầu tiên là khóa chính. Các ô nhập được tạo tự động theo danh sách Cot.
    /// </summary>
    public partial class frmDanhMuc : Form
    {
        readonly string bang, tieuDe, tienTo;
        readonly Cot[] cot;
        readonly Control[] oNhap;

        public frmDanhMuc(string bang, string tieuDe, string tienTo, params Cot[] cot)
        {
            InitializeComponent();
            this.bang = bang;
            this.tieuDe = tieuDe;
            this.tienTo = tienTo;
            this.cot = cot;
            Text = "Quản lý " + tieuDe.ToLower();
            grpThongTin.Text = "Thông tin " + tieuDe.ToLower();

            oNhap = new Control[cot.Length];
            tlpNhap.RowCount = cot.Length;
            for (int i = 0; i < cot.Length; i++)
            {
                Control o;
                if (cot[i].LaNgay)
                    o = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };
                else if (cot[i].LuaChon != null || cot[i].BangLienKet != null)
                    o = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
                else
                    o = new TextBox();
                o.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                tlpNhap.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                tlpNhap.Controls.Add(new Label { Text = cot[i].NhanDe, AutoSize = true, Anchor = AnchorStyles.Left }, 0, i);
                tlpNhap.Controls.Add(o, 1, i);
                oNhap[i] = o;
            }
        }

        string Khoa { get { return cot[0].Ten; } }

        private void frmDanhMuc_Load(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < cot.Length; i++)
                {
                    ComboBox cb = oNhap[i] as ComboBox;
                    if (cb == null) continue;
                    if (cot[i].LuaChon != null)
                        cb.Items.AddRange(cot[i].LuaChon);
                    else
                    {
                        cb.DisplayMember = cot[i].CotHienThi;
                        cb.ValueMember = cot[i].Ten;   // khóa ngoại trùng tên khóa chính của bảng liên kết
                        cb.DataSource = FileXml.HienThi(cot[i].BangLienKet);
                    }
                }
                HienThi(FileXml.HienThi(bang));
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không đọc được " + bang + ".xml: " + ex.Message, "Lỗi");
            }
        }

        void HienThi(DataTable dt)
        {
            dgvDuLieu.DataSource = dt;
            foreach (Cot c in cot)
                if (dgvDuLieu.Columns.Contains(c.Ten))
                    dgvDuLieu.Columns[c.Ten].HeaderText = c.NhanDe;
        }

        void LamMoi()
        {
            foreach (Control o in oNhap)
            {
                if (o is DateTimePicker) ((DateTimePicker)o).Value = DateTime.Today;
                else if (o is ComboBox) ((ComboBox)o).SelectedIndex = ((ComboBox)o).Items.Count > 0 ? 0 : -1;
                else o.Text = "";
            }
            // Có tiền tố -> tự sinh mã (TC009...), không cho sửa; không có (Tài khoản) -> người dùng tự nhập
            TextBox txtKhoa = (TextBox)oNhap[0];
            txtKhoa.ReadOnly = tienTo != null;
            txtKhoa.Text = tienTo != null ? FileXml.TaoMaMoi(bang, Khoa, tienTo) : "";
        }

        string LayGiaTri(int i)
        {
            Control o = oNhap[i];
            if (o is DateTimePicker) return ((DateTimePicker)o).Value.ToString("yyyy-MM-dd");
            ComboBox cb = o as ComboBox;
            if (cb != null) return Convert.ToString(cb.ValueMember != "" ? cb.SelectedValue : cb.SelectedItem);
            return o.Text.Trim();
        }

        void GanGiaTri(int i, object giaTri)
        {
            Control o = oNhap[i];
            ComboBox cb = o as ComboBox;
            if (o is DateTimePicker)
                ((DateTimePicker)o).Value = giaTri is DateTime ? (DateTime)giaTri : DateTime.Today;
            else if (cb != null && cb.ValueMember != "")
                cb.SelectedValue = Convert.ToString(giaTri);
            else if (cb != null)
                cb.SelectedItem = Convert.ToString(giaTri);
            else
                o.Text = giaTri is IFormattable ? ((IFormattable)giaTri).ToString(null, CultureInfo.InvariantCulture)
                                                : Convert.ToString(giaTri);
        }

        string[] TenCot(int tu)
        {
            return cot.Skip(tu).Select(c => c.Ten).ToArray();
        }

        string[] GiaTri(int tu)
        {
            return Enumerable.Range(tu, cot.Length - tu).Select(LayGiaTri).ToArray();
        }

        bool HopLe()
        {
            // Kiểm tra trống ở đây; kiểu dữ liệu, số âm, số điện thoại... do lược đồ XSD kiểm tra khi lưu
            for (int i = 0; i < 2; i++)
                if (LayGiaTri(i) == "")
                {
                    MessageBox.Show("Vui lòng nhập " + cot[i].NhanDe.ToLower() + ".", "Thông báo");
                    oNhap[i].Focus();
                    return false;
                }
            return true;
        }

        void TaiLai()
        {
            HienThi(FileXml.HienThi(bang));
        }

        private void dgvDuLieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow dong = dgvDuLieu.Rows[e.RowIndex];
            for (int i = 0; i < cot.Length; i++)
                GanGiaTri(i, dong.Cells[cot[i].Ten].Value);
            ((TextBox)oNhap[0]).ReadOnly = true;   // đang chọn 1 dòng có sẵn: không đổi khóa chính
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!HopLe()) return;
            try
            {
                if (FileXml.KiemTraMa(bang, Khoa, LayGiaTri(0)))
                {
                    MessageBox.Show(cot[0].NhanDe + " '" + LayGiaTri(0) + "' đã tồn tại. Bấm \"Làm mới\" để thêm mới.", "Thông báo");
                    return;
                }
                FileXml.Them(bang, TenCot(0), GiaTri(0));
                TaiLai();
                LamMoi();
                MessageBox.Show("Thêm thành công.", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thêm được", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!HopLe()) return;
            try
            {
                FileXml.Sua(bang, Khoa, LayGiaTri(0), TenCot(1), GiaTri(1));
                TaiLai();
                MessageBox.Show("Sửa thành công.", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không sửa được", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string ma = LayGiaTri(0);
            if (ma == "" || !FileXml.KiemTraMa(bang, Khoa, ma))
            {
                MessageBox.Show("Hãy chọn dòng cần xóa.", "Thông báo");
                return;
            }
            string bangDung = FileXml.DangDuocDung(bang, Khoa, ma);
            if (bangDung != null)
            {
                MessageBox.Show("Không thể xóa: '" + ma + "' đang được dùng trong " + bangDung + ".xml.", "Thông báo");
                return;
            }
            if (bang == "TaiKhoan" && ma == Program.TenDangNhap)
            {
                MessageBox.Show("Không thể xóa tài khoản đang đăng nhập.", "Thông báo");
                return;
            }
            if (MessageBox.Show("Xóa '" + ma + "'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                FileXml.Xoa(bang, Khoa, ma);
                TaiLai();
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không xóa được", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
            txtTimKiem.Clear();
            TaiLai();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();
            DataTable dt = FileXml.HienThi(bang);
            // Giữ lại các dòng có ít nhất một ô chứa từ khóa (không phân biệt hoa thường)
            foreach (DataRow r in dt.Select())
                if (!r.ItemArray.Any(v => Convert.ToString(v).IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0))
                    r.Delete();
            dt.AcceptChanges();
            HienThi(dt);
            if (dt.Rows.Count == 0) MessageBox.Show("Không tìm thấy kết quả.", "Tìm kiếm");
        }

        private void btnTrinhDuyet_Click(object sender, EventArgs e)
        {
            try
            {
                string html = FileXml.XuatHTML(bang, "DanhSach",
                    "TieuDe", "DANH SÁCH " + tieuDe.ToUpper(),
                    "TuKhoa", txtTimKiem.Text.Trim());
                Process.Start(html);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
