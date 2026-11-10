# Quản Lý Cửa Hàng Trái Cây – Bài tập lớn Công nghệ XML

Ứng dụng web **ASP.NET Core Razor Pages (C#, .NET 8)** mô phỏng quản lý cửa hàng trái cây. Dữ liệu lưu bằng **file XML**,
được kiểm tra bằng **XSD**, hiển thị ra trình duyệt bằng **XSLT** và đồng bộ hai chiều với **SQL Server**.

## Thành viên

| MSSV          | Họ và tên            |
|---------------|----------------------|
| 2415053122206 | Lâm Hưng Thiên Doanh |
| 2415053122219 | Hà Cảnh Minh Hoàng   |

## Chức năng

| Yêu cầu đề bài              | Thực hiện trong chương trình                                                        |
|-----------------------------|-------------------------------------------------------------------------------------|
| Cơ sở dữ liệu               | `Database/QLCuaHangTraiCay.sql` – 9 bảng, khóa chính/khóa ngoại, dữ liệu mẫu        |
| XML                         | `QuanLyCuaHangTraiCay/Data/*.xml` – mỗi bảng một file                               |
| XSD (lược đồ)               | `Data/*.xsd` – kiểu dữ liệu, mẫu mã (TC001...), SĐT 10 số, số không âm, khóa chính |
| XSLT                        | `Data/DanhSach.xslt` (danh sách + lọc), `Data/HoaDon.xslt` (in hóa đơn), `Data/RSS.xslt` (nguồn tin RSS) |
| Form Database → XML         | Trang **Dữ liệu XML → Tạo XML từ CSDL**                                             |
| Thêm / Sửa / Xóa            | Danh mục Trái cây, Nhà cung cấp, Khách hàng, Nhân viên, Tài khoản; Lập hóa đơn; Nhập hàng |
| Hiển thị trình duyệt        | Nút **Xem bằng XSLT** (mọi danh mục), **In hóa đơn** – trang `/Xslt` trả HTML        |
| Cập nhật dữ liệu database   | Trang **Dữ liệu XML → Cập nhật CSDL từ XML**                                        |
| Kiểm tra hợp lệ             | Tự kiểm tra theo XSD mỗi lần lưu (tô đỏ ô sai); trang **Dữ liệu XML** báo lỗi + số dòng |

Ngoài ra: đăng nhập (cookie) với **mật khẩu băm SHA-256**, phân quyền Quản lý/Nhân viên, đổi mật khẩu, trang tổng quan doanh thu,
tìm kiếm, bán hàng tự trừ tồn kho, **nhập hàng tự cộng tồn kho** (phiếu nhập, mỗi phiếu chỉ gồm trái cây của một nhà cung cấp),
lọc hóa đơn/phiếu nhập theo ngày, **nguồn tin RSS bảng giá** công khai tại `/Rss` (RSS 2.0, tạo bằng XSLT),
giao diện responsive (máy tính và điện thoại).

## Cấu trúc thư mục

```
QuanLyCuaHangTraiCay.sln
QuanLyCuaHangTraiCay/
  Class/FileXml.cs       Đọc/Thêm/Sửa/Xóa XML (XmlDocument + XPath), XSD, XSLT, đồng bộ SQL Server
  Class/HoaDon.cs        Nghiệp vụ lưu/xóa hóa đơn, cập nhật tồn kho
  Class/PhieuNhap.cs     Nghiệp vụ lưu/xóa phiếu nhập, cộng/trừ tồn kho
  Class/MatKhau.cs       Băm mật khẩu SHA-256
  Pages/                 Razor Pages: Đăng nhập, Tổng quan, Danh mục (dùng chung 5 bảng), Lập hóa đơn,
                         Hóa đơn, Nhập hàng, Phiếu nhập, Dữ liệu XML, Xslt (hiển thị trình duyệt), Rss, Đổi mật khẩu
  wwwroot/css/site.css   Giao diện (CSS thuần, không framework)
  Program.cs             Cấu hình Razor Pages, đăng nhập cookie, phân quyền
  appsettings.json       Chuỗi kết nối SQL Server
  Data/                  File XML, XSD, XSLT (chương trình đọc/ghi trực tiếp tại đây)
Database/QLCuaHangTraiCay.sql
BaoCao/                  Báo cáo bài tập lớn (.docx)
```

## Cách chạy

**Visual Studio 2022:** mở `QuanLyCuaHangTraiCay.sln`, nhấn **F5** (trình duyệt tự mở `http://localhost:5080`).

**Dòng lệnh** (cần .NET 8 SDK):

```bash
dotnet run --project QuanLyCuaHangTraiCay
```

Đăng nhập: **admin / admin** (Quản lý) hoặc **binh / 123** (Nhân viên).

Chương trình chạy được ngay với dữ liệu XML có sẵn. Muốn dùng chức năng đồng bộ SQL Server:
- Mở `Database/QLCuaHangTraiCay.sql` trong SSMS và chạy (F5).
- Nếu dùng SQL Express, sửa `Data Source=.` thành `Data Source=.\\SQLEXPRESS` trong `appsettings.json` (JSON cần 2 dấu `\`).
- Hướng dẫn kiểm thử đầy đủ: [docs/KiemThu.md](docs/KiemThu.md) · Kế hoạch tiếp theo: [docs/KeHoach.md](docs/KeHoach.md).

> Lưu ý: chương trình ghi thẳng vào các file trong `QuanLyCuaHangTraiCay/Data`, nên mọi thao tác
> thêm/sửa/xóa sẽ hiện ngay trong file XML (dùng `git checkout -- QuanLyCuaHangTraiCay/Data` để khôi phục dữ liệu mẫu).
