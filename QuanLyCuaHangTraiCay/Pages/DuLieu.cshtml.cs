using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.Pages
{
    /// <summary>Chỉ Admin (cấu hình trong Program.cs): đồng bộ SQL Server ↔ XML và kiểm tra hợp lệ theo XSD.</summary>
    public class DuLieuModel : PageModel
    {
        public class KetQua { public string Bang; public int SoDong; public List<string> Loi; }
        public List<KetQua> DanhSach = new List<KetQua>();

        public void OnGet()
        {
            foreach (string bang in FileXml.CacBang)
            {
                List<string> loi = FileXml.KiemTraHopLe(bang);
                DanhSach.Add(new KetQua { Bang = bang, Loi = loi, SoDong = loi.Count == 0 ? FileXml.HienThi(bang).Rows.Count : 0 });
            }
        }

        public IActionResult OnPostSqlSangXml()
        {
            return ThucHien(FileXml.TaoXMLTuSQL, "Đã tạo lại " + FileXml.CacBang.Length + " file XML từ SQL Server.");
        }

        public IActionResult OnPostXmlSangSql()
        {
            return ThucHien(FileXml.CapNhatSQL, "Đã cập nhật CSDL SQL Server từ các file XML.");
        }

        IActionResult ThucHien(Action viec, string thanhCong)
        {
            try
            {
                viec();
                TempData["ThongBao"] = thanhCong;
            }
            catch (Exception ex)
            {
                TempData["Loi"] = "Lỗi: " + ex.Message;
            }
            return RedirectToPage();
        }
    }
}
