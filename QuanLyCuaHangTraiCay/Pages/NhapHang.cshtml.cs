using System.Data;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.Pages
{
    public class NhapHangModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public string MaNCC { get; set; }
        [BindProperty] public DateTime NgayNhap { get; set; } = DateTime.Today;
        public string MaPN, Loi;
        public DataTable NhaCungCap;
        public List<DataRow> TraiCay;                                   // chỉ trái cây của nhà cung cấp đang chọn
        public Dictionary<string, string> SoLuong = new Dictionary<string, string>();   // giữ lại giá trị đã nhập khi báo lỗi
        public Dictionary<string, string> DonGia = new Dictionary<string, string>();

        void NapDuLieu()
        {
            NhaCungCap = FileXml.HienThi("NhaCungCap");
            if (string.IsNullOrEmpty(MaNCC) || !FileXml.KiemTraMa("NhaCungCap", "MaNCC", MaNCC))
                MaNCC = NhaCungCap.Rows.Count > 0 ? NhaCungCap.Rows[0]["MaNCC"].ToString() : null;
            TraiCay = FileXml.HienThi("TraiCay").AsEnumerable().Where(r => r.Field<string>("MaNCC") == MaNCC).ToList();
            MaPN = FileXml.TaoMaMoi("PhieuNhap", "MaPN", "PN");
        }

        public void OnGet() { NapDuLieu(); }

        public IActionResult OnPost()
        {
            NapDuLieu();
            // Dòng nào có số lượng > 0 thì vào phiếu; đơn giá nhập phải là số nguyên không âm
            DataTable gio = new DataTable();
            gio.Columns.Add("MaTC", typeof(string));
            gio.Columns.Add("SoLuong", typeof(int));
            gio.Columns.Add("DonGiaNhap", typeof(decimal));
            try
            {
                foreach (DataRow tc in TraiCay)
                {
                    string maTC = tc["MaTC"].ToString();
                    string sl = Request.Form["sl_" + maTC].ToString().Trim(), gia = Request.Form["gia_" + maTC].ToString().Trim();
                    SoLuong[maTC] = sl; DonGia[maTC] = gia;
                    int soLuong; long donGia;
                    if (sl == "" || (int.TryParse(sl, NumberStyles.None, CultureInfo.InvariantCulture, out soLuong) && soLuong == 0)) continue;
                    if (!int.TryParse(sl, NumberStyles.None, CultureInfo.InvariantCulture, out soLuong))
                        throw new Exception("Số lượng nhập của " + tc["TenTC"] + " phải là số nguyên không âm.");
                    if (!long.TryParse(gia, NumberStyles.None, CultureInfo.InvariantCulture, out donGia))
                        throw new Exception("Hãy nhập đơn giá nhập (số nguyên không âm) cho " + tc["TenTC"] + ".");
                    gio.Rows.Add(maTC, soLuong, (decimal)donGia);
                }
                decimal tong = PhieuNhap.Luu(MaPN, NgayNhap, User.MaNV(), MaNCC, gio);
                TempData["ThongBao"] = "Đã lưu phiếu nhập " + MaPN + " — tổng tiền " + DinhDang.Tien(tong) + ", tồn kho đã được cộng.";
                return RedirectToPage("/PhieuNhap", new { ma = MaPN });
            }
            catch (Exception ex)
            {
                Loi = ex.Message;
                return Page();
            }
        }
    }
}
