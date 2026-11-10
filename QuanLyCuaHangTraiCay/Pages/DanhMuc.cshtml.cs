using System.Data;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.Pages
{
    /// <summary>Mô tả 1 cột (1 thẻ con trong file XML) của danh mục.</summary>
    public class Cot
    {
        public string Ten, NhanDe;
        public string[] LuaChon;     // danh sách cố định -> <select>
        public string BangLienKet;   // khóa ngoại -> <select> lấy từ file XML khác
        public string CotHienThi;    // cột hiển thị của bảng liên kết
        public string Kieu = "text"; // kiểu ô input: text, number, date, tel, password (băm SHA-256 trước khi lưu)
        public Cot(string ten, string nhanDe) { Ten = ten; NhanDe = nhanDe; }
    }

    public class CauHinhDanhMuc
    {
        public string TieuDe, TienTo;   // TienTo null: khóa do người dùng tự nhập (Tài khoản)
        public bool ChiAdmin;
        public Cot[] Cot;
    }

    /// <summary>Trang dùng chung cho 5 danh mục. Cột đầu tiên là khóa chính.</summary>
    public class DanhMucModel : PageModel
    {
        public static readonly Dictionary<string, CauHinhDanhMuc> CacDanhMuc = new Dictionary<string, CauHinhDanhMuc>
        {
            ["TraiCay"] = new CauHinhDanhMuc { TieuDe = "Trái cây", TienTo = "TC", Cot = new[] {
                new Cot("MaTC", "Mã trái cây"), new Cot("TenTC", "Tên trái cây"), new Cot("XuatXu", "Xuất xứ"),
                new Cot("DonViTinh", "Đơn vị tính") { LuaChon = new[] { "kg", "quả", "nải", "hộp" } },
                new Cot("DonGia", "Đơn giá (VNĐ)") { Kieu = "number" }, new Cot("SoLuongTon", "Số lượng tồn") { Kieu = "number" },
                new Cot("MaNCC", "Nhà cung cấp") { BangLienKet = "NhaCungCap", CotHienThi = "TenNCC" } } },
            ["NhaCungCap"] = new CauHinhDanhMuc { TieuDe = "Nhà cung cấp", TienTo = "NCC", Cot = new[] {
                new Cot("MaNCC", "Mã NCC"), new Cot("TenNCC", "Tên nhà cung cấp"), new Cot("DiaChi", "Địa chỉ"),
                new Cot("SDT", "Số điện thoại") { Kieu = "tel" } } },
            ["KhachHang"] = new CauHinhDanhMuc { TieuDe = "Khách hàng", TienTo = "KH", Cot = new[] {
                new Cot("MaKH", "Mã khách hàng"), new Cot("TenKH", "Tên khách hàng"),
                new Cot("SDT", "Số điện thoại") { Kieu = "tel" }, new Cot("DiaChi", "Địa chỉ") } },
            ["NhanVien"] = new CauHinhDanhMuc { TieuDe = "Nhân viên", TienTo = "NV", ChiAdmin = true, Cot = new[] {
                new Cot("MaNV", "Mã nhân viên"), new Cot("HoTen", "Họ tên"),
                new Cot("GioiTinh", "Giới tính") { LuaChon = new[] { "Nam", "Nữ" } }, new Cot("NgaySinh", "Ngày sinh") { Kieu = "date" },
                new Cot("DiaChi", "Địa chỉ"), new Cot("SDT", "Số điện thoại") { Kieu = "tel" } } },
            ["TaiKhoan"] = new CauHinhDanhMuc { TieuDe = "Tài khoản", ChiAdmin = true, Cot = new[] {
                new Cot("TenDangNhap", "Tên đăng nhập"), new Cot("MatKhau", "Mật khẩu") { Kieu = "password" },
                new Cot("Quyen", "Quyền") { LuaChon = new[] { "Admin", "NhanVien" } },
                new Cot("MaNV", "Nhân viên") { BangLienKet = "NhanVien", CotHienThi = "HoTen" } } },
        };

        [BindProperty(SupportsGet = true)] public string Bang { get; set; }
        [BindProperty(SupportsGet = true)] public string TuKhoa { get; set; }
        [BindProperty(SupportsGet = true)] public string Ma { get; set; }

        public CauHinhDanhMuc CauHinh;
        public DataTable DuLieu;
        public int TongSo;
        public bool DangSua;                                   // đang sửa 1 dòng có sẵn
        public Dictionary<string, string> GiaTri = new Dictionary<string, string>();
        public Dictionary<string, Dictionary<string, string>> LienKet = new Dictionary<string, Dictionary<string, string>>();
        public string Loi, ChiTietLoi, TruongLoi, GoiYLoi;   // TruongLoi: tên cột gây lỗi XSD (để tô đỏ ô nhập)

        string Khoa { get { return CauHinh.Cot[0].Ten; } }

        bool NapCauHinh()
        {
            CauHinh = Bang != null && CacDanhMuc.ContainsKey(Bang) ? CacDanhMuc[Bang] : null;
            return CauHinh != null && (!CauHinh.ChiAdmin || User.LaAdmin());
        }

        void NapDuLieu()
        {
            foreach (Cot c in CauHinh.Cot)
                if (c.BangLienKet != null)
                    LienKet[c.Ten] = DinhDang.TuDien(c.BangLienKet, c.Ten, c.CotHienThi);

            DuLieu = FileXml.HienThi(Bang);
            TongSo = DuLieu.Rows.Count;
            if (!string.IsNullOrWhiteSpace(TuKhoa))
            {
                string k = TuKhoa.Trim();
                // Giữ dòng có ít nhất 1 ô chứa từ khóa (không phân biệt hoa thường), tìm cả theo tên khóa ngoại
                foreach (DataRow r in DuLieu.Select())
                {
                    bool khop = CauHinh.Cot.Where(c => c.Kieu != "password").Any(c =>   // không tìm trong chuỗi băm mật khẩu
                        Convert.ToString(r[c.Ten]).IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (LienKet.ContainsKey(c.Ten) && LienKet[c.Ten].Ten(r[c.Ten]).IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0));
                    if (!khop) r.Delete();
                }
                DuLieu.AcceptChanges();
            }
        }

        public IActionResult OnGet()
        {
            if (!NapCauHinh()) return CauHinh == null ? NotFound() : Forbid();
            NapDuLieu();
            DataRow dong = Ma == null ? null : FileXml.HienThi(Bang).AsEnumerable().FirstOrDefault(r => r[Khoa].ToString() == Ma);
            DangSua = dong != null;
            foreach (Cot c in CauHinh.Cot)
                GiaTri[c.Ten] = DangSua && c.Kieu != "password" ? DinhDang.Goc(dong[c.Ten]) : "";   // không bao giờ điền sẵn mật khẩu
            if (!DangSua && CauHinh.TienTo != null)
                GiaTri[Khoa] = FileXml.TaoMaMoi(Bang, Khoa, CauHinh.TienTo);
            return Page();
        }

        public IActionResult OnPostLuu(bool dangSua)
        {
            if (!NapCauHinh()) return CauHinh == null ? NotFound() : Forbid();
            foreach (Cot c in CauHinh.Cot)
            {
                string v = Request.Form[c.Ten].ToString() ?? "";
                GiaTri[c.Ten] = c.Kieu == "password" ? v : v.Trim();   // mật khẩu giữ nguyên khoảng trắng như lúc đăng nhập
            }
            DangSua = dangSua;
            string ma = GiaTri[Khoa];
            // Đang sửa mà để trống ô mật khẩu = giữ nguyên mật khẩu cũ (bỏ cột đó khỏi lệnh sửa)
            Cot[] cot = CauHinh.Cot.Where(c => !(c.Kieu == "password" && dangSua && GiaTri[c.Ten] == "")).ToArray();
            string[] ten = cot.Select(c => c.Ten).ToArray();
            // Cột mật khẩu được băm (muối = khóa chính, tức tên đăng nhập) trước khi ghi vào XML
            string[] gt = cot.Select(c => c.Kieu == "password" ? Class.MatKhau.Bam(ma, GiaTri[c.Ten]) : GiaTri[c.Ten]).ToArray();
            try
            {
                if (ma == "" || (GiaTri[CauHinh.Cot[1].Ten] == "" && cot.Any(c => c == CauHinh.Cot[1])))   // cột 2 phải có giá trị, trừ mật khẩu khi đang sửa
                    throw new Exception("Vui lòng nhập " + CauHinh.Cot[0].NhanDe.ToLower() + " và " + CauHinh.Cot[1].NhanDe.ToLower() + ".");
                if (dangSua)
                {
                    // Kiểu dữ liệu, số âm, số điện thoại... do lược đồ XSD kiểm tra trong FileXml.Sua
                    FileXml.Sua(Bang, Khoa, ma, ten.Skip(1).ToArray(), gt.Skip(1).ToArray());
                    TempData["ThongBao"] = "Đã cập nhật " + ma + ".";
                }
                else
                {
                    if (FileXml.KiemTraMa(Bang, Khoa, ma))
                        throw new Exception(CauHinh.Cot[0].NhanDe + " '" + ma + "' đã tồn tại.");
                    FileXml.Them(Bang, ten, gt);
                    TempData["ThongBao"] = "Đã thêm " + ma + ".";
                }
            }
            catch (Exception ex)
            {
                GiaiThichLoi(ex.Message);   // hiển thị lại form với dữ liệu vừa nhập
                NapDuLieu();
                return Page();
            }
            return RedirectToPage(new { Bang, Ma = (string)null, TuKhoa = (string)null });
        }

        /// <summary>Đổi thông báo lỗi XSD (tiếng Anh, kỹ thuật) thành câu dễ hiểu, giữ bản gốc để xem chi tiết.</summary>
        void GiaiThichLoi(string thongBao)
        {
            Match m = Regex.Match(thongBao, "The '(\\w+)' element is invalid");
            Cot cot = m.Success ? CauHinh.Cot.FirstOrDefault(c => c.Ten == m.Groups[1].Value) : null;
            if (cot != null)
            {
                TruongLoi = cot.Ten;
                Loi = "«" + cot.NhanDe + "» không hợp lệ theo lược đồ " + Bang + ".xsd" +
                      (cot.Kieu == "tel" ? " (cần 10 chữ số, bắt đầu bằng 0)." : cot.Kieu == "number" ? " (cần số nguyên không âm)." : ".");
                GoiYLoi = cot.Kieu == "tel" ? "Cần 10 chữ số, bắt đầu bằng 0." : cot.Kieu == "number" ? "Cần số nguyên không âm." : "Giá trị không đúng định dạng yêu cầu.";
                ChiTietLoi = thongBao;
            }
            else
                Loi = thongBao;
        }

        public IActionResult OnPostXoa(string maXoa)
        {
            if (!NapCauHinh()) return CauHinh == null ? NotFound() : Forbid();
            string bangDung = FileXml.DangDuocDung(Bang, Khoa, maXoa);
            if (bangDung != null)
                TempData["Loi"] = "Không thể xóa " + maXoa + ": đang được dùng trong " + bangDung + ".xml.";
            else if (Bang == "TaiKhoan" && maXoa == User.Identity.Name)
                TempData["Loi"] = "Không thể xóa tài khoản đang đăng nhập.";
            else
            {
                FileXml.Xoa(Bang, Khoa, maXoa);
                TempData["ThongBao"] = "Đã xóa " + maXoa + ".";
            }
            return RedirectToPage(new { Bang, Ma = (string)null });
        }
    }
}
