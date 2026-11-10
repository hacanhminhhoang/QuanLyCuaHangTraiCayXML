# Kế hoạch tiếp theo

**Ngày nộp / bảo vệ:** ……………… — các mốc dưới đây tính ngược từ ngày này (N = ngày nộp).

Phụ trách: **D** = Lâm Hưng Thiên Doanh (2415053122206), **H** = Hà Cảnh Minh Hoàng (2415053122219).

## A. Lộ trình đến ngày nộp

### 1. Dọn repo — ngay (H)
- [ ] GitHub → **Settings → General → Default branch** → chọn `main`
      (hiện vẫn là `feature/quan-ly-cua-hang-trai-cay`).
- [ ] Xóa branch `feature/quan-ly-cua-hang-trai-cay` đã merge (nút *Delete branch* trong PR #1).
- [ ] Từ nay mỗi việc làm trên 1 branch riêng tách từ `main` (`feat/...`, `fix/...`, `docs/...`), mở Pull Request,
      người còn lại review rồi mới merge.

### 2. Kiểm thử trên máy — N-14 (D + H)
- [ ] Mỗi người chạy toàn bộ 47 test trong [KiemThu.md](KiemThu.md) trên máy mình (ít nhất 1 máy có SQL Server cho phần G).
- [ ] Ghi kết quả vào mục 5 của KiemThu.md, chụp ảnh các test quan trọng (TC21, TC26, TC35, TC38, TC39, TC40).
- [ ] Lỗi tìm được → tạo Issue → sửa trên branch `fix/...` → Pull Request.

### 3. Hoàn thiện báo cáo — N-10 (D: chương 1–2, H: chương 2 phần IV, chương 3)
- [ ] Trang bìa: Khoa, Giáo viên hướng dẫn, Nhóm, Lớp.
- [ ] Thêm mục **Kiểm thử** (cuối chương 2): môi trường test, bảng test case rút gọn (15–20 test tiêu biểu) và kết quả Đạt/Không đạt.
- [ ] Thay ảnh chụp bằng ảnh trên máy mình nếu giao diện khác (ví dụ ngày hiển thị dd/MM/yyyy).
- [ ] Cập nhật mục lục (chuột phải → Update Field → Update entire table), kiểm tra đánh số hình.
- [ ] Xem lại bảng phân công cho đúng thực tế; đọc soát chính tả.

### 4. Slide thuyết trình — N-7 (H)
10–12 slide:
1. Tên đề tài, thành viên
2. Bài toán cửa hàng trái cây và mục tiêu
3. Kiến trúc: trình duyệt → Razor Pages → `FileXml` → file XML/XSD/XSLT ⇄ SQL Server
4. Thiết kế CSDL (mô hình quan hệ 7 bảng)
5. XML + XSD: cấu trúc file, các ràng buộc (`pattern`, `minInclusive`, `xs:key`)
6. XPath: thêm/sửa/xóa trong `FileXml.cs`
7. XSLT: `DanhSach.xslt` (tham số lọc), `HoaDon.xslt` (`document()` ghép 5 file)
8. Đồng bộ Database ⇄ XML (transaction, thứ tự bảng cha → con)
9. Demo (ảnh hoặc chạy trực tiếp)
10. Kiểm thử: số test, kết quả
11. Kết luận, hạn chế, hướng phát triển

### 5. Kịch bản demo ~7 phút — N-3 (D + H, tập dượt ít nhất 2 lần)
Trước khi demo: khôi phục dữ liệu mẫu (`git checkout -- QuanLyCuaHangTraiCay/Data`), chạy sẵn SQL Server và web.

| Phút | Nội dung | Yêu cầu đề bài được chứng minh |
|---|---|---|
| 0:00 | Đăng nhập `admin`, giới thiệu trang Tổng quan | |
| 0:45 | Trái cây: thêm `Bơ 034`, mở `Data/TraiCay.xml` cho thấy thẻ mới; sửa giá; xóa | XML, Thêm, Sửa, Xóa |
| 2:00 | Nhà cung cấp: nhập SĐT sai → lỗi XSD, ô tô đỏ; mở `NhaCungCap.xsd` chỉ `pattern` | XSD, Kiểm tra hợp lệ |
| 3:00 | Lập hóa đơn → tồn kho giảm → In hóa đơn | XSLT, Hiển thị trình duyệt |
| 4:15 | Xem bằng XSLT danh sách trái cây có từ khóa | XSLT |
| 5:00 | Dữ liệu XML: Cập nhật CSDL từ XML → mở SSMS cho thấy dữ liệu; sửa giá trong SSMS → Tạo XML từ CSDL | CSDL, Database → XML, Cập nhật database |
| 6:15 | Đăng nhập `binh`: không có menu quản trị, bị chặn trang Dữ liệu XML | Phân quyền |

### 6. Câu hỏi bảo vệ dự kiến — N-2 (D + H)

| Câu hỏi | Gợi ý trả lời (xem lại code tương ứng) |
|---|---|
| Vì sao lưu dữ liệu bằng XML thay vì chỉ dùng SQL Server? | Yêu cầu môn học; XML là văn bản, dễ đọc, dễ trao đổi giữa hệ thống, có XSD/XSLT đi kèm. SQL Server dùng để lưu tập trung và đồng bộ |
| XSD kiểm tra những gì? | Thứ tự và kiểu thẻ, mẫu mã `TC[0-9]{3}`, SĐT `0[0-9]{9}`, số không âm, tiền không có phần thập phân, khóa chính không trùng bằng `xs:key` (`Data/*.xsd`) |
| Kiểm tra XSD xảy ra lúc nào? | Trước mỗi lần ghi file trong `FileXml.Luu` — dữ liệu sai thì không ghi; và khi bấm kiểm tra / trước khi XML → Database |
| XPath được dùng ở đâu? | `FileXml.XPath`: `/NewDataSet/TraiCay[MaTC='TC001']` để tìm dòng khi sửa, xóa, lấy giá trị |
| `document()` trong XSLT để làm gì? | `HoaDon.xslt` đọc thêm ChiTietHoaDon.xml, TraiCay.xml, KhachHang.xml, NhanVien.xml để in hóa đơn đầy đủ tên |
| XML → Database lỗi giữa chừng thì sao? | Chạy trong 1 transaction: lỗi thì rollback, CSDL giữ nguyên (`FileXml.CapNhatSQL`) |
| Vì sao phải xóa bảng con trước, chèn bảng cha trước? | Ràng buộc khóa ngoại (`FileXml.CacBang` xếp từ cha → con) |
| Hai người cùng lưu một lúc có hỏng file không? | Không: mọi thao tác đọc–sửa–ghi file XML chạy tuần tự nhờ `lock (FileXml.KhoaGhi)` |
| Đơn giá ở trang lập hóa đơn lấy từ đâu? | Server lấy lại từ TraiCay.xml, không tin dữ liệu trình duyệt gửi lên; tồn kho cũng được kiểm tra lại khi lưu |

## B. Tính năng mở rộng (nếu còn thời gian)

Ưu tiên theo **giá trị cho điểm số / công sức**. Mỗi tính năng làm trên 1 branch `feat/...` và 1 Pull Request.

### P1 — nên làm

| Tính năng | Lợi ích | Công nghệ XML thể hiện | Công sức | File chính |
|---|---|---|---|---|
| **RSS bảng giá trái cây** | Đề bài có nêu "mô tả ứng dụng web service hoặc RSS"; khách đăng ký theo dõi giá | XSLT biến TraiCay.xml thành RSS 2.0 | ~2 giờ | `Data/RSS.xslt`, `Pages/Rss.cshtml(.cs)`, link trong layout |
| **Phiếu nhập hàng** | Hoàn chỉnh nghiệp vụ: nhập hàng làm tăng tồn kho (hiện chỉ có bán) | 2 file XML + XSD mới (PhieuNhap, ChiTietPhieuNhap), bảng SQL tương ứng | ~4–5 giờ | `Data/PhieuNhap*.xml/.xsd`, `Class/PhieuNhap.cs`, `Pages/NhapHang`, `Database/*.sql`, `FileXml.CacBang` |
| **Băm mật khẩu SHA-256** | Khắc phục hạn chế đã nêu trong báo cáo (mật khẩu lưu dạng thường) | Cập nhật dữ liệu XML hàng loạt | ~1 giờ | `DangNhap`, `DoiMatKhau`, `DanhMuc` (Tài khoản), `TaiKhoan.xml` |

### P2 — làm thêm để nổi bật

| Tính năng | Lợi ích | Công nghệ XML thể hiện | Công sức |
|---|---|---|---|
| **Web service** `/api/traicay` | Hệ thống khác lấy danh sách trái cây dạng XML hoặc JSON | Trả XML theo XSD, đọc bằng XPath | ~2 giờ |
| **Nhập file XML từ máy** | Tải lên file danh sách trái cây của nhà cung cấp | Kiểm tra XSD trước khi gộp, báo lỗi theo dòng | ~3 giờ |
| **Thống kê doanh thu theo tháng / theo mặt hàng** | Báo cáo trực quan cho chủ cửa hàng | XSLT `xsl:key` (nhóm Muenchian), vẽ biểu đồ cột bằng SVG | ~4 giờ |

### P3 — tùy chọn
- Sao lưu / khôi phục thư mục Data (nén thành file .zip có ngày giờ).
- Phân trang và sắp xếp bảng khi dữ liệu nhiều.
- Hạn sử dụng cho từng lô trái cây và cảnh báo sắp hỏng.

Khi chọn làm tính năng nào, cập nhật thêm vào báo cáo (đặc tả yêu cầu, bảng dữ liệu, module, ảnh giao diện)
và bổ sung test case vào [KiemThu.md](KiemThu.md).
