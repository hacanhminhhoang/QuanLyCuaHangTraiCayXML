# Quản Lý Cửa Hàng Trái Cây – Bài tập lớn Công nghệ XML

Ứng dụng Windows Forms (C#) mô phỏng quản lý cửa hàng trái cây. Dữ liệu lưu bằng **file XML**,
được kiểm tra bằng **XSD**, hiển thị ra trình duyệt bằng **XSLT** và đồng bộ hai chiều với **SQL Server**.

## Thành viên

| MSSV          | Họ và tên            |
|---------------|----------------------|
| 2415053122206 | Lâm Hưng Thiên Doanh |
| 2415053122219 | Hà Cảnh Minh Hoàng   |

## Chức năng

| Yêu cầu đề bài              | Thực hiện trong chương trình                                                        |
|-----------------------------|-------------------------------------------------------------------------------------|
| Cơ sở dữ liệu               | `Database/QLCuaHangTraiCay.sql` – 7 bảng, khóa chính/khóa ngoại, dữ liệu mẫu        |
| XML                         | `QuanLyCuaHangTraiCay/Data/*.xml` – mỗi bảng một file                               |
| XSD (lược đồ)               | `Data/*.xsd` – kiểu dữ liệu, mẫu mã (TC001...), SĐT 10 số, số không âm, khóa chính |
| XSLT                        | `Data/DanhSach.xslt` (danh sách + lọc), `Data/HoaDon.xslt` (in hóa đơn)             |
| Form Database → XML         | Menu **Dữ liệu → Database -> XML**                                                  |
| Thêm / Sửa / Xóa            | Danh mục Trái cây, Nhà cung cấp, Khách hàng, Nhân viên, Tài khoản; Lập hóa đơn      |
| Hiển thị trình duyệt        | Nút **Xem trên trình duyệt** (mọi danh mục), **In hóa đơn**                         |
| Cập nhật dữ liệu database   | Menu **Dữ liệu → XML -> Database**                                                  |
| Kiểm tra hợp lệ             | Tự kiểm tra theo XSD mỗi lần lưu; menu **Dữ liệu → Kiểm tra hợp lệ (XSD)**          |

Ngoài ra: đăng nhập, phân quyền Admin/Nhân viên, đổi mật khẩu, tìm kiếm, bán hàng tự trừ tồn kho,
lọc hóa đơn theo ngày và tính doanh thu.

## Cấu trúc thư mục

```
QuanLyCuaHangTraiCay.sln
QuanLyCuaHangTraiCay/
  Class/FileXml.cs       Đọc/Thêm/Sửa/Xóa XML (XmlDocument + XPath), XSD, XSLT, đồng bộ SQL Server
  Class/HoaDon.cs        Nghiệp vụ lưu/xóa hóa đơn, cập nhật tồn kho
  GUI/                   Các form: Đăng nhập, Main, Danh mục (dùng chung), Hóa đơn, Danh sách HĐ, Đổi MK
  Data/                  File XML, XSD, XSLT (được chép cạnh file .exe khi build)
Database/QLCuaHangTraiCay.sql
BaoCao/                  Báo cáo bài tập lớn (.docx)
```

## Cách chạy

1. Mở `QuanLyCuaHangTraiCay.sln` bằng **Visual Studio 2019/2022** (cần .NET Framework 4.8), nhấn **F5**.
2. Đăng nhập: **admin / admin** (Admin) hoặc **binh / 123** (Nhân viên).
3. Chương trình chạy được ngay với dữ liệu XML có sẵn. Muốn dùng chức năng đồng bộ SQL Server:
   - Mở `Database/QLCuaHangTraiCay.sql` trong SSMS và chạy (F5).
   - Nếu dùng SQL Express, sửa `Data Source=.` thành `Data Source=.\SQLEXPRESS` trong `App.config`.

> Lưu ý: chương trình ghi vào các file XML trong `bin\Debug\net48\Data`. Bản gốc trong
> `QuanLyCuaHangTraiCay/Data` chỉ được chép lại khi nó mới hơn (sửa tay file gốc rồi build lại).
