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

Kết quả bắt đầu bằng `8.` là tốt nhất. Nếu máy chỉ có .NET 9/10 vẫn chạy được (project cho phép chạy trên runtime mới hơn); nếu F5 báo "You must install or update .NET" thì xem mục 4.

## 2. Cài đặt và chạy

1. Clone repo (branch `main`):

   ```bash
   git clone https://github.com/hacanhminhhoang/QuanLyCuaHangTraiCayXML.git
   ```

2. Tạo CSDL: mở SSMS → **File → Open** `Database/QLCuaHangTraiCay.sql` → nhấn **Execute (F5)**.
   Script tự xóa CSDL cũ (nếu có) rồi tạo lại 9 bảng và dữ liệu mẫu. Kết quả đúng: không có dòng đỏ, và có 9 lần "rows affected" = 3, 3, 3, 8, 3, 2, 3, 1, 2.
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
| TC08 | **Đổi mật khẩu**: `123` → `456` (nhập lại đúng) → Đăng xuất → đăng nhập `binh/456` | Báo "Đổi mật khẩu thành công."; đăng nhập bằng mật khẩu mới được; trong `Data/TaiKhoan.xml` mật khẩu của `binh` là chuỗi băm 64 ký tự khác trước | |

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
| TC18 | Mở **Tài khoản**, thêm tài khoản `doanh` / `123`, quyền NhanVien, nhân viên NV003 | Thêm thành công; cột Mật khẩu hiển thị `••••••` (file XML chỉ lưu chuỗi băm 64 ký tự); đăng nhập được bằng `doanh/123` | |
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
| TC38 | Mở **Dữ liệu XML** | Bảng kiểm tra hiện 9 file đều "✓ Hợp lệ" kèm số dòng (NhanVien 3, TaiKhoan 3, NhaCungCap 3, TraiCay 8, KhachHang 3, HoaDon 2, ChiTietHoaDon 3, PhieuNhap 1, ChiTietPhieuNhap 2) | |
| TC39 | Thêm 1 khách hàng trên web → **Cập nhật CSDL từ XML** → OK. Trong SSMS chạy câu SQL bên dưới | Báo "Đã cập nhật CSDL SQL Server từ các file XML."; SSMS thấy KhachHang có 4 dòng, có khách hàng vừa thêm | |
| TC40 | Trong SSMS sửa giá rồi trên web bấm **Tạo XML từ CSDL** → OK (câu SQL bên dưới) | Báo "Đã tạo lại 9 file XML từ SQL Server."; trang Trái cây hiện Xoài cát Hòa Lộc giá `70.000`; `Data/TraiCay.xml` có `<DonGia>70000</DonGia>` | |
| TC41 | Mở `Data/TraiCay.xml` bằng Notepad, đổi một `<SoLuongTon>` thành `-5`, lưu → vào **Dữ liệu XML** | TraiCay.xml "✗ 1 lỗi", có ghi số dòng bị lỗi | |
| TC42 | Giữ file lỗi ở TC41, bấm **Cập nhật CSDL từ XML** | Báo "Lỗi: TraiCay.xml không hợp lệ: ..."; trong SSMS dữ liệu **không** thay đổi | |
| TC43 | Mở `Data/KhachHang.xml`, xóa một thẻ đóng `</KhachHang>`, lưu → vào **Dữ liệu XML** | KhachHang.xml báo "Lỗi cú pháp XML: ..." | |
| TC44 | Khôi phục dữ liệu mẫu. Tắt dịch vụ SQL Server (Services → SQL Server → Stop) → bấm **Tạo XML từ CSDL** | Báo "Lỗi: ..." (không kết nối được); các file XML vẫn còn nguyên | |
| TC45 | Bật lại SQL Server, bấm **Cập nhật CSDL từ XML** rồi **Tạo XML từ CSDL** | Cả hai thành công; kiểm tra lại (TC38) vẫn 9 file hợp lệ — dữ liệu đi vòng XML → SQL → XML không bị sai | |

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

### I. Nhập hàng (phiếu nhập)

**Trước khi làm nhóm I, J, K:** khôi phục dữ liệu mẫu (`git checkout -- QuanLyCuaHangTraiCay/Data`) và chạy lại toàn bộ
`Database/QLCuaHangTraiCay.sql` trong SSMS, rồi đăng nhập `admin` / `admin`.

| Mã | Các bước | Kết quả mong đợi | KQ |
|---|---|---|---|
| TC48 | Mở **Nhập hàng** | Mã phiếu `PN002`; nhà cung cấp mặc định "Vựa trái cây Hòa Khánh" và bảng chỉ có 3 trái cây của nhà này: Nho xanh Ninh Thuận, Thanh long ruột đỏ, Dưa hấu Long An | |
| TC49 | Đổi nhà cung cấp sang "HTX Trái cây Cái Bè" | Trang tải lại, bảng đổi thành Xoài cát Hòa Lộc, Sầu riêng Ri6, Cam sành | |
| TC50 | Sầu riêng Ri6: Số lượng nhập `10`, **bỏ trống** Đơn giá nhập → **Lưu phiếu nhập** | Báo "Hãy nhập đơn giá nhập (số nguyên không âm) cho Sầu riêng Ri6."; ô số lượng vẫn còn `10`; không có phiếu nào được lưu | |
| TC51 | Sầu riêng Ri6: `10` × `100000`; Cam sành: `5` × `20000` | Thành tiền từng dòng hiện ngay (1.000.000 đ, 100.000 đ); **Tổng tiền nhập** `1.100.000 đ` | |
| TC52 | Bấm **Lưu phiếu nhập** | Chuyển sang **Phiếu nhập**, báo "Đã lưu phiếu nhập PN002 — tổng tiền 1.100.000 đ, tồn kho đã được cộng."; chi tiết PN002 hiện đúng 2 dòng | |
| TC53 | Mở **Trái cây** | Tồn kho Sầu riêng Ri6 `30 → 40`, Cam sành `80 → 85` | |
| TC54 | **Phiếu nhập**: xem danh sách; lọc Từ ngày = Đến ngày = 30/10/2026 | Danh sách có PN002 và PN001; sau khi lọc chỉ còn PN001, tổng tiền nhập `1.560.000 đ`; bấm **Tất cả** để bỏ lọc | |
| TC55 | **Nhập hàng** → HTX Trái cây Cái Bè → không nhập số lượng nào → Lưu | Báo "Phiếu nhập chưa có trái cây nào." | |
| TC56 | **Lập hóa đơn** bán 35 Sầu riêng Ri6 (tồn còn `5`), rồi vào **Phiếu nhập** → PN002 → **Xóa phiếu nhập** → OK | Báo "Không thể xóa phiếu PN002: tồn kho Sầu riêng Ri6 chỉ còn 5, ít hơn số đã nhập (10), vì một phần đã được bán."; tồn kho không đổi | |
| TC57 | **Hóa đơn & doanh thu** → xóa hóa đơn vừa lập (tồn Sầu riêng về `40`), rồi xóa phiếu PN002 | Báo "Đã xóa phiếu nhập PN002 và trừ lại tồn kho."; tồn Sầu riêng về `30`, Cam sành về `80` | |
| TC58 | Đăng xuất, đăng nhập `binh` / `123` → **Phiếu nhập**; sau đó **Nhập hàng** → Vựa trái cây Hòa Khánh → Nho xanh `10` × `50000` → Lưu | Không có nút **Xóa phiếu nhập**; nhân viên vẫn lập được phiếu mới | |
| TC59 | Đăng nhập `admin`; **Lập hóa đơn** bán 1 Kiwi vàng (tồn còn `19`); mở **Tổng quan** | Mục "Sắp hết hàng" có Kiwi vàng kèm liên kết **Nhập thêm**; bấm vào mở trang Nhập hàng đã chọn sẵn "Công ty Nhập khẩu Fresh Fruit" | |

### J. RSS bảng giá

| Mã | Các bước | Kết quả mong đợi | KQ |
|---|---|---|---|
| TC60 | Mở cửa sổ ẩn danh (chưa đăng nhập), gõ `http://localhost:5080/Rss` | Hiện nội dung XML bắt đầu bằng `<rss version="2.0">`, **không** bị chuyển sang trang Đăng nhập | |
| TC61 | Xem nội dung nguồn tin | Có 8 thẻ `<item>` (mỗi trái cây một thẻ); ví dụ tiêu đề "Xoài cát Hòa Lộc – 65.000 đ/kg", mô tả "Xuất xứ: Tiền Giang. Giá: 65.000 đ/kg. Còn hàng." | |
| TC62 | Bán hết một loại trái cây (hoặc sửa tồn kho về 0), tải lại `/Rss` | Trái cây đó ghi "Hết hàng."; nguồn tin **không** chứa số lượng tồn cụ thể | |
| TC63 | (Không bắt buộc) Thêm địa chỉ `http://localhost:5080/Rss` vào một trình đọc RSS cài trên máy (QuiteRSS, Thunderbird → Feeds…) | Trình đọc hiện 8 bài với giá và xuất xứ | |
| TC64 | Chưa đăng nhập, gõ `http://localhost:5080/DanhMuc/TraiCay`; sau khi đăng nhập, bấm **RSS bảng giá** ở cuối thanh menu | Trang danh mục vẫn chuyển về Đăng nhập; liên kết RSS mở tab mới tới nguồn tin | |

### K. Mật khẩu được băm

| Mã | Các bước | Kết quả mong đợi | KQ |
|---|---|---|---|
| TC65 | Mở `QuanLyCuaHangTraiCay/Data/TaiKhoan.xml` bằng Notepad | Thẻ `<MatKhau>` của mọi tài khoản là chuỗi 64 ký tự (0–9, a–f); không thấy `admin` hay `123` | |
| TC66 | **Tài khoản** → thêm `thu` / `abc123`, quyền NhanVien, nhân viên NV003; mở lại `TaiKhoan.xml` | File có tài khoản `thu` với mật khẩu 64 ký tự hex, **không** chứa `abc123`; đăng nhập `thu` / `abc123` được | |
| TC67 | **Tài khoản** → Sửa `thu`: ô Mật khẩu **để trống**, đổi quyền sang Admin → Lưu; đăng xuất, đăng nhập `thu` / `abc123` | Ô Mật khẩu không điền sẵn gì (gợi ý "Để trống = giữ nguyên"); mật khẩu cũ vẫn dùng được; menu có Dữ liệu XML | |
| TC68 | Sửa `thu`, nhập mật khẩu mới `xyz789` → Lưu; đăng nhập thử `abc123` rồi `xyz789` | `abc123` bị từ chối ("Sai tên đăng nhập hoặc mật khẩu."), `xyz789` đăng nhập được | |
| TC69 | **Tài khoản** → nhập tên đăng nhập mới nhưng **không nhập mật khẩu** → **Thêm mới** | Trình duyệt chặn lại và nhắc điền ô Mật khẩu (nếu vượt qua được, máy chủ cũng từ chối: "Vui lòng nhập tên đăng nhập và mật khẩu.") | |
| TC70 | Trong Notepad sửa `<MatKhau>` của `binh` thành `123` (mật khẩu thường), lưu → vào **Dữ liệu XML** (nhớ khôi phục dữ liệu sau khi xong) | TaiKhoan.xml báo "✗ 1 lỗi" (lược đồ chỉ chấp nhận 64 ký tự hex) | |
| TC71 | **Tài khoản** → ô tìm kiếm gõ `8da1` (đầu chuỗi băm của `admin`) | "Không có dữ liệu phù hợp." — không tìm được theo chuỗi băm | |

## 4. Xử lý sự cố thường gặp

| Hiện tượng | Cách xử lý |
|---|---|
| Đồng bộ báo lỗi "A network-related or instance-specific error…" | Sai `Data Source` trong `appsettings.json` (bản Express là `.\\SQLEXPRESS`) hoặc dịch vụ SQL Server chưa chạy (Services → SQL Server (…) → Start) |
| "Cannot open database QLCuaHangTraiCay" | Chưa chạy script `Database/QLCuaHangTraiCay.sql` |
| "Login failed for user…" | Tài khoản Windows chưa có quyền trên SQL Server; đăng nhập SSMS bằng Windows Authentication để kiểm tra |
| Lỗi chứng chỉ SSL khi kết nối | Chuỗi kết nối đã có `TrustServerCertificate=True`; kiểm tra lại nếu đã sửa `appsettings.json` |
| F5 báo "You must install or update .NET to run this application" (thiếu `Microsoft.AspNetCore.App 8.0.0`) | Cài **ASP.NET Core Runtime 8.0 (x64)** từ https://dotnet.microsoft.com/download/dotnet/8.0, đóng và mở lại Visual Studio. Kiểm tra bằng `dotnet --list-runtimes` |
| Không chạy được vì cổng 5080 đang bận | Đổi `applicationUrl` trong `Properties/launchSettings.json` (ví dụ `http://localhost:5090`) |
| Số liệu khác với mô tả trong test case | Khôi phục dữ liệu mẫu bằng `git checkout -- QuanLyCuaHangTraiCay/Data` |
| Ngày hiển thị dạng mm/dd/yyyy trong ô chọn ngày | Do ngôn ngữ của trình duyệt/Windows, không ảnh hưởng dữ liệu (XML luôn lưu `yyyy-MM-dd`) |

## 5. Ghi kết quả

| Thông tin | |
|---|---|
| Người kiểm thử | |
| Ngày | |
| Máy (Windows, phiên bản SQL Server) | |
| Số test đạt / tổng | ... / 71 |

| Mã test | Mô tả lỗi | Ảnh chụp | Link Issue |
|---|---|---|---|
| | | | |

Mỗi lỗi tìm được: tạo một Issue trên GitHub (tiêu đề ngắn, các bước tái hiện, kết quả mong đợi / thực tế, ảnh chụp),
sửa trên branch `fix/<mo-ta-ngan>` rồi mở Pull Request vào `main`.
