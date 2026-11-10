# Hướng dẫn kiểm thử trên máy

Tài liệu này dùng để mỗi thành viên tự kiểm thử chương trình trên máy Windows của mình, ghi lại kết quả
và đưa vào báo cáo. Phần **G. Đồng bộ SQL Server** bắt buộc phải test trên máy có SQL Server.

## 1. Chuẩn bị môi trường

| Phần mềm | Ghi chú |
|---|---|
| Windows 10/11 | |
| Visual Studio 2022 | Khi cài chọn workload **ASP.NET and web development** (có sẵn .NET 8 SDK) |
| SQL Server 2019/2022 (Express hoặc Developer) + SSMS | Chỉ cần cho phần G |
| Git | Để clone repo và khôi phục dữ liệu mẫu |
| Trình duyệt Chrome / Edge | |

Kiểm tra .NET đã cài đúng (mở Command Prompt):

```bash
dotnet --version
```

Kết quả phải bắt đầu bằng `8.`.

## 2. Cài đặt và chạy

1. Clone repo (branch `main`):

   ```bash
   git clone https://github.com/hacanhminhhoang/QuanLyCuaHangTraiCayXML.git
   ```

2. Tạo CSDL: mở SSMS → **File → Open** `Database/QLCuaHangTraiCay.sql` → nhấn **Execute (F5)**.
   Script tự xóa CSDL cũ (nếu có) rồi tạo lại 7 bảng và dữ liệu mẫu.
3. Nếu SQL Server là bản Express, mở `QuanLyCuaHangTraiCay/appsettings.json` và đổi
   `Data Source=.` thành `Data Source=.\\SQLEXPRESS` (trong file JSON phải viết 2 dấu `\`).
4. Mở `QuanLyCuaHangTraiCay.sln` bằng Visual Studio → nhấn **F5**. Trình duyệt tự mở `http://localhost:5080`.
5. Tài khoản: `admin / admin` (Quản lý), `binh / 123` (Nhân viên).

**Trước mỗi lần test (hoặc demo), khôi phục dữ liệu mẫu**, vì chương trình ghi thẳng vào các file XML:

```bash
git checkout -- QuanLyCuaHangTraiCay/Data
```

## 3. Các trường hợp kiểm thử

Cột **KQ** ghi `Đạt` / `Không đạt`. Nếu không đạt, ghi lại lỗi ở mục 5.
Các test chạy theo thứ tự từ trên xuống, bắt đầu từ dữ liệu mẫu.

### A. Đăng nhập và phân quyền

| Mã | Các bước | Kết quả mong đợi | KQ |
|---|---|---|---|
| TC01 | Mở `http://localhost:5080` khi chưa đăng nhập | Chuyển sang trang Đăng nhập | |
| TC02 | Đăng nhập `admin` / `sai` | Báo "Sai tên đăng nhập hoặc mật khẩu.", vẫn ở trang Đăng nhập | |
| TC03 | Đăng nhập `admin` / `admin` | Vào trang Tổng quan: "Xin chào, Nguyễn Văn An"; menu có Nhân viên, Tài khoản, Dữ liệu XML | |
| TC04 | Bấm **Đăng xuất**, đăng nhập `binh` / `123` | Menu **không** có Nhân viên, Tài khoản, Dữ liệu XML | |
| TC05 | Đang là `binh`, gõ địa chỉ `http://localhost:5080/DuLieu` | Trang "Bạn không có quyền truy cập" | |
| TC06 | Đang là `binh`, gõ `http://localhost:5080/DanhMuc/TaiKhoan` | Trang "Bạn không có quyền truy cập" | |
| TC07 | **Đổi mật khẩu**: nhập sai mật khẩu hiện tại | Báo "Mật khẩu hiện tại không đúng." | |
| TC08 | **Đổi mật khẩu**: `123` → `456` (nhập lại đúng) → Đăng xuất → đăng nhập `binh/456` | Báo "Đổi mật khẩu thành công."; đăng nhập bằng mật khẩu mới được | |

Từ TC09 trở đi đăng nhập bằng `admin`.

### B. Trang Tổng quan

| Mã | Các bước | Kết quả mong đợi | KQ |
|---|---|---|---|
| TC09 | Xem trang **Tổng quan** | Có 4 thẻ số liệu (doanh thu tháng, hôm nay, loại trái cây, khách hàng), bảng Hóa đơn gần đây, danh sách Sắp hết hàng | |
| TC10 | Bấm vào mã một hóa đơn trong "Hóa đơn gần đây" | Mở trang Hóa đơn và hiện đúng chi tiết hóa đơn đó | |

### C. Danh mục (Trái cây, Nhà cung cấp, Khách hàng, Nhân viên, Tài khoản)

| Mã | Các bước | Kết quả mong đợi | KQ |
|---|---|---|---|
| TC11 | Mở **Trái cây** | Ô Mã trái cây tự điền `TC009` và không sửa được; bảng có 8 dòng | |
| TC12 | Nhập Tên `Bơ 034`, Xuất xứ `Đắk Lắk`, Đơn giá `55000`, Số lượng tồn `15`, chọn NCC → **Thêm mới** | Báo "Đã thêm TC009."; dòng mới xuất hiện; mở `Data/TraiCay.xml` thấy thẻ `<TraiCay>` mới | |
| TC13 | Bấm **Sửa** ở dòng TC009, đổi Đơn giá `60000` → **Lưu thay đổi** | Bảng hiện `60.000`; file XML cập nhật | |
| TC14 | Gõ `Fresh` vào ô tìm → **Tìm** | Còn 2 dòng (Táo Envy, Kiwi vàng — NCC "Công ty Nhập khẩu Fresh Fruit"); bấm **Bỏ lọc** hiện lại đủ | |
| TC15 | Bấm **Xóa** ở dòng TC009 → OK | Dòng bị xóa, báo "Đã xóa TC009." | |
| TC16 | Mở **Nhà cung cấp**, bấm **Xóa** ở NCC001 → OK | Báo "Không thể xóa NCC001: đang được dùng trong TraiCay.xml." | |
| TC17 | Mở **Nhân viên**, thêm nhân viên mới (chọn giới tính, ngày sinh bằng ô ngày) | Thêm thành công, mã tự sinh `NV004`, ngày sinh hiển thị dạng dd/MM/yyyy | |
| TC18 | Mở **Tài khoản**, thêm tài khoản `doanh` / `123`, quyền NhanVien, nhân viên NV003 | Thêm thành công; cột Mật khẩu hiển thị `••••••`; đăng nhập được bằng `doanh/123` | |
| TC19 | Ở **Tài khoản**, thêm lại tài khoản `doanh` | Báo "Tên đăng nhập 'doanh' đã tồn tại." | |
| TC20 | Ở **Tài khoản**, bấm **Xóa** dòng `admin` (đang đăng nhập) | Báo "Không thể xóa tài khoản đang đăng nhập." | |

### D. Kiểm tra hợp lệ theo XSD khi lưu

| Mã | Các bước | Kết quả mong đợi | KQ |
|---|---|---|---|
| TC21 | **Nhà cung cấp** → Sửa NCC001 → Số điện thoại `12345` → **Lưu thay đổi** | Báo "«Số điện thoại» không hợp lệ theo lược đồ NhaCungCap.xsd (cần 10 chữ số, bắt đầu bằng 0)."; ô SĐT viền đỏ; dữ liệu vừa nhập vẫn còn; file XML **không** đổi | |
| TC22 | Ở TC21 bấm **Chi tiết kỹ thuật** | Hiện thông báo gốc của XSD (Pattern constraint failed) | |
| TC23 | **Khách hàng** → thêm khách hàng có SĐT `0905abc123` | Bị từ chối theo KhachHang.xsd | |
| TC24 | **Trái cây** → nhập Số lượng tồn `-5` | Trình duyệt chặn ngay (ô số có `min=0`); kiểm tra phía XSD xem ở TC41 | |

### E. Bán hàng

| Mã | Các bước | Kết quả mong đợi | KQ |
|---|---|---|---|
| TC25 | **Lập hóa đơn**: Sầu riêng Ri6 = `2`, Cam sành = `4`, khách hàng Võ Quốc Huy | Mã hóa đơn `HD003`; thành tiền từng dòng và Tổng tiền `380.000 đ` hiện ngay khi gõ | |
| TC26 | Bấm **Lưu hóa đơn** | Chuyển sang trang Hóa đơn, báo "Đã lưu hóa đơn HD003 — tổng tiền 380.000 đ." kèm link In hóa đơn | |
| TC27 | Mở **Trái cây** | Tồn kho Sầu riêng Ri6 `30 → 28`, Cam sành `80 → 76` | |
| TC28 | **Lập hóa đơn**: Kiwi vàng nhập `999` | Trình duyệt chặn vì vượt tồn kho (`max`); nếu vượt qua được, server báo "...chỉ còn 20, không đủ bán 999." và không lưu gì | |
| TC29 | **Lập hóa đơn** không nhập số lượng nào → Lưu | Báo "Hóa đơn chưa có trái cây nào." | |
| TC30 | **Hóa đơn & doanh thu**: chọn Từ ngày–Đến ngày chỉ chứa ngày 01/11/2026 → **Lọc** | Chỉ còn HD001, doanh thu `280.000 đ`; bấm **Tất cả** để bỏ lọc | |
| TC31 | Chọn HD003 → **Xóa hóa đơn** → OK | Báo "Đã xóa hóa đơn HD003 và hoàn lại tồn kho."; tồn kho Sầu riêng về `30`, Cam sành về `80` | |
| TC32 | Đăng nhập `binh` (mật khẩu `456` từ TC08), mở **Hóa đơn & doanh thu** | Không có nút **Xóa hóa đơn** | |

### F. Hiển thị trình duyệt (XSLT)

| Mã | Các bước | Kết quả mong đợi | KQ |
|---|---|---|---|
| TC33 | **Trái cây** → **Xem bằng XSLT ↗** | Tab mới hiện "DANH SÁCH TRÁI CÂY" do `DanhSach.xslt` tạo, tiêu đề cột là tên thẻ XML | |
| TC34 | Tìm `Mỹ` rồi bấm **Xem bằng XSLT ↗** | Trang XSLT chỉ còn dòng Táo Envy, có ghi "từ khóa: Mỹ" | |
| TC35 | **Hóa đơn** → chọn HD001 → **In hóa đơn (XSLT) ↗** | Hóa đơn có tên khách Phạm Thu Hà, nhân viên Trần Thị Bình, 2 dòng, tổng `280,000 đ` (ghép 5 file XML bằng `document()`) | |
| TC36 | Ở trang hóa đơn bấm **Ctrl+P** | Bản in gọn, nền trắng | |
| TC37 | Gõ `http://localhost:5080/Xslt?bang=abc` | Lỗi 404 (không cho đọc bảng không có trong danh sách) | |

### G. Đồng bộ SQL Server (trang Dữ liệu XML — chỉ Admin)

Chuẩn bị: đã chạy script SQL ở bước 2 và khôi phục dữ liệu mẫu XML.

| Mã | Các bước | Kết quả mong đợi | KQ |
|---|---|---|---|
| TC38 | Mở **Dữ liệu XML** | Bảng kiểm tra hiện 7 file đều "✓ Hợp lệ" kèm số dòng (NhanVien 3, TaiKhoan 3, NhaCungCap 3, TraiCay 8, KhachHang 3, HoaDon 2, ChiTietHoaDon 3) | |
| TC39 | Thêm 1 khách hàng trên web → **Cập nhật CSDL từ XML** → OK. Trong SSMS chạy câu SQL bên dưới | Báo "Đã cập nhật CSDL SQL Server từ các file XML."; SSMS thấy KhachHang có 4 dòng, có khách hàng vừa thêm | |
| TC40 | Trong SSMS sửa giá rồi trên web bấm **Tạo XML từ CSDL** → OK (câu SQL bên dưới) | Báo "Đã tạo lại 7 file XML từ SQL Server."; trang Trái cây hiện Xoài cát Hòa Lộc giá `70.000`; `Data/TraiCay.xml` có `<DonGia>70000</DonGia>` | |
| TC41 | Mở `Data/TraiCay.xml` bằng Notepad, đổi một `<SoLuongTon>` thành `-5`, lưu → vào **Dữ liệu XML** | TraiCay.xml "✗ 1 lỗi", có ghi số dòng bị lỗi | |
| TC42 | Giữ file lỗi ở TC41, bấm **Cập nhật CSDL từ XML** | Báo "Lỗi: TraiCay.xml không hợp lệ: ..."; trong SSMS dữ liệu **không** thay đổi | |
| TC43 | Mở `Data/KhachHang.xml`, xóa một thẻ đóng `</KhachHang>`, lưu → vào **Dữ liệu XML** | KhachHang.xml báo "Lỗi cú pháp XML: ..." | |
| TC44 | Khôi phục dữ liệu mẫu. Tắt dịch vụ SQL Server (Services → SQL Server → Stop) → bấm **Tạo XML từ CSDL** | Báo "Lỗi: ..." (không kết nối được); các file XML vẫn còn nguyên | |
| TC45 | Bật lại SQL Server, bấm **Cập nhật CSDL từ XML** rồi **Tạo XML từ CSDL** | Cả hai thành công; kiểm tra lại (TC38) vẫn 7 file hợp lệ — dữ liệu đi vòng XML → SQL → XML không bị sai | |

Câu SQL dùng trong SSMS:

```sql
USE QLCuaHangTraiCay;
-- TC39: đếm số dòng từng bảng
SELECT 'KhachHang' AS Bang, COUNT(*) AS SoDong FROM KhachHang
UNION ALL SELECT 'TraiCay', COUNT(*) FROM TraiCay
UNION ALL SELECT 'HoaDon', COUNT(*) FROM HoaDon
UNION ALL SELECT 'ChiTietHoaDon', COUNT(*) FROM ChiTietHoaDon;
SELECT * FROM KhachHang;

-- TC40: sửa giá trực tiếp trong CSDL
UPDATE TraiCay SET DonGia = 70000 WHERE MaTC = N'TC001';
```

### H. Giao diện

| Mã | Các bước | Kết quả mong đợi | KQ |
|---|---|---|---|
| TC46 | Nhấn **F12** → biểu tượng điện thoại (Toggle device toolbar) → chọn iPhone 12 Pro | Menu chuyển lên trên, không có thanh cuộn ngang, bảng cuộn ngang bên trong thẻ | |
| TC47 | Dùng phím **Tab** đi qua form Đăng nhập và form Thêm trái cây | Ô đang chọn có viền xanh rõ ràng, thứ tự hợp lý | |

## 4. Xử lý sự cố thường gặp

| Hiện tượng | Cách xử lý |
|---|---|
| Đồng bộ báo lỗi "A network-related or instance-specific error…" | Sai `Data Source` trong `appsettings.json` (bản Express là `.\\SQLEXPRESS`) hoặc dịch vụ SQL Server chưa chạy (Services → SQL Server (…) → Start) |
| "Cannot open database QLCuaHangTraiCay" | Chưa chạy script `Database/QLCuaHangTraiCay.sql` |
| "Login failed for user…" | Tài khoản Windows chưa có quyền trên SQL Server; đăng nhập SSMS bằng Windows Authentication để kiểm tra |
| Lỗi chứng chỉ SSL khi kết nối | Chuỗi kết nối đã có `TrustServerCertificate=True`; kiểm tra lại nếu đã sửa `appsettings.json` |
| Không chạy được vì cổng 5080 đang bận | Đổi `applicationUrl` trong `Properties/launchSettings.json` (ví dụ `http://localhost:5090`) |
| Số liệu khác với mô tả trong test case | Khôi phục dữ liệu mẫu bằng `git checkout -- QuanLyCuaHangTraiCay/Data` |
| Ngày hiển thị dạng mm/dd/yyyy trong ô chọn ngày | Do ngôn ngữ của trình duyệt/Windows, không ảnh hưởng dữ liệu (XML luôn lưu `yyyy-MM-dd`) |

## 5. Ghi kết quả

| Thông tin | |
|---|---|
| Người kiểm thử | |
| Ngày | |
| Máy (Windows, phiên bản SQL Server) | |
| Số test đạt / tổng | ... / 47 |

| Mã test | Mô tả lỗi | Ảnh chụp | Link Issue |
|---|---|---|---|
| | | | |

Mỗi lỗi tìm được: tạo một Issue trên GitHub (tiêu đề ngắn, các bước tái hiện, kết quả mong đợi / thực tế, ảnh chụp),
sửa trên branch `fix/<mo-ta-ngan>` rồi mở Pull Request vào `main`.
