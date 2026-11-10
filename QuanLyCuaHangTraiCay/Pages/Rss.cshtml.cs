using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuanLyCuaHangTraiCay.Class;

namespace QuanLyCuaHangTraiCay.Pages
{
    /// <summary>Nguồn tin RSS 2.0 bảng giá trái cây, công khai (không cần đăng nhập): biến đổi TraiCay.xml bằng RSS.xslt.</summary>
    public class RssModel : PageModel
    {
        public IActionResult OnGet()
        {
            string goc = Request.Scheme + "://" + Request.Host;
            // Ngày cập nhật nguồn tin = lần cuối file TraiCay.xml thay đổi (định dạng RFC 822/1123 như RSS yêu cầu)
            string ngay = System.IO.File.GetLastWriteTimeUtc(FileXml.DuongDan("TraiCay")).ToString("R", CultureInfo.InvariantCulture);
            string rss = FileXml.XuatHTML("TraiCay", "RSS", "BaseUrl", goc, "NgayCapNhat", ngay);
            return Content(rss.TrimStart('﻿'), "application/rss+xml; charset=utf-8");
        }
    }
}
