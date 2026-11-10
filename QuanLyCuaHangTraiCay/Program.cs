using Microsoft.AspNetCore.Authentication.Cookies;
using QuanLyCuaHangTraiCay.Class;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(o =>
{
    o.Conventions.AuthorizeFolder("/");               // mọi trang đều phải đăng nhập...
    o.Conventions.AllowAnonymousToPage("/DangNhap");  // ...trừ trang đăng nhập
    o.Conventions.AllowAnonymousToPage("/Rss");       // ...và nguồn tin RSS bảng giá (công khai)
    o.Conventions.AuthorizePage("/DuLieu", "Admin");  // đồng bộ dữ liệu: chỉ Admin
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/DangNhap";
        o.AccessDeniedPath = "/KhongCoQuyen";
    });
builder.Services.AddAuthorization(o => o.AddPolicy("Admin", p => p.RequireRole("Admin")));

var app = builder.Build();

// Thư mục chứa XML/XSD/XSLT và chuỗi kết nối SQL Server
FileXml.ThuMuc = Path.Combine(app.Environment.ContentRootPath, "Data");
FileXml.ChuoiKetNoi = app.Configuration.GetConnectionString("QLCuaHangTraiCay");

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
