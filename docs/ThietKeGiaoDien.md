# Thiết kế giao diện

Giao diện được thiết kế lại bằng skill **UI/UX Pro Max** (v2.15.0, nguồn: <https://github.com/nextlevelbuilder/ui-ux-pro-max-skill>),
theo đúng quy trình của skill: phân tích yêu cầu → sinh hệ thống thiết kế → tra cứu từng mảng → triển khai → kiểm tra.
Toàn bộ nằm trong một file CSS thuần `QuanLyCuaHangTraiCay/wwwroot/css/site.css`, không dùng framework.

## 1. Cài skill để dùng lại

Trong Claude Code (máy của bạn):

```
/plugin marketplace add nextlevelbuilder/ui-ux-pro-max-skill
/plugin install ui-ux-pro-max@ui-ux-pro-max-skill
```

Hoặc dùng CLI (cần Node.js và Python 3; gói npm là `ui-ux-pro-max-cli`, lệnh là `uipro`):

```bash
npm install -g ui-ux-pro-max-cli
uipro init --ai claude --global
```

Sau đó gõ yêu cầu như: *"Dùng skill ui-ux-pro-max để thiết kế trang X, giữ nguyên logic, dùng các token trong site.css"*.

## 2. Phân tích yêu cầu

| Mục | Quyết định |
|---|---|
| Sản phẩm | Ứng dụng quản trị bán hàng và kho cho cửa hàng trái cây nhỏ (không phải trang landing) |
| Người dùng | Nhân viên bán hàng và chủ cửa hàng, không chuyên kỹ thuật; chủ yếu dùng máy tính, thỉnh thoảng điện thoại |
| Ngôn ngữ | Tiếng Việt, nên font phải đủ dấu |
| Phong cách | Flat + Data-Dense Dashboard: phẳng, không đổ bóng, mật độ vừa phải |
| Núm chỉnh của skill | `--density 7 --motion 3 --variance 4` (dày dữ liệu, chuyển động rất nhẹ, bố cục cân bằng) |

Các truy vấn đã chạy (kèm cách dùng kết quả):

| Truy vấn | Kết quả được dùng | Kết quả bị loại |
|---|---|---|
| `--design-system "fruit grocery store inventory sales admin dashboard fresh"` | Phong cách Flat, palette xanh lá + hổ phách ("Fresh green + food amber") | Pattern "Product Demo + Features" (là mẫu trang landing, không hợp ứng dụng quản trị) |
| `--domain typography "friendly food retail clean readable"` | Cặp **Be Vietnam Pro + Noto Sans** ("Vietnamese Friendly") | Fira Code (monospace, quá "kỹ thuật" cho cửa hàng trái cây) |
| `--domain style "admin dashboard data-dense inventory"` | Data-Dense Dashboard: lưới KPI, bảng gọn, hàng sáng khi rê chuột | |
| `--domain ux` (bảng, lỗi biểu mẫu, điều hướng, xác nhận xóa, skip link, target size) | Các quy tắc ở mục 5 | |
| `--domain chart "daily revenue trend comparison"` | Biểu đồ cột có nhãn giá trị trực tiếp + tóm tắt cho trình đọc màn hình | |

## 3. Token thiết kế

Mọi component dùng token ngữ nghĩa (`--color-primary`, `--color-border-strong`, `--sp-4`…), không viết mã màu thô.
Tỉ lệ tương phản WCAG được tính bằng chương trình cho từng cặp màu, cả hai chế độ:

| Cặp | Sáng | Tối | Yêu cầu |
|---|---|---|---|
| Chữ chính trên thẻ | 17.9 : 1 | 14.2 : 1 | ≥ 4.5 |
| Chữ phụ trên thẻ | 7.6 : 1 | 8.1 : 1 | ≥ 4.5 |
| Chữ trên nút chính | 5.5 : 1 (trắng / `#047857`) | 8.4 : 1 | ≥ 4.5 |
| Liên kết trên nền trang | 5.0 : 1 | 12.0 : 1 | ≥ 4.5 |
| Viền ô nhập trên thẻ | 4.6 : 1 | 4.8 : 1 | ≥ 3 (viền cũ chỉ ~1.5 : 1, không đạt) |
| Vòng focus trên thanh bên | 8 : 1 (`#a7f3d0`) | 12.6 : 1 | ≥ 3 |

Palette skill đề xuất dùng `#059669` với chữ đen trên nút; nhóm đổi sang `#047857` để giữ chữ trắng mà vẫn ≥ 4.5 : 1.

- **Font:** Be Vietnam Pro (tiêu đề, số liệu lớn) và Noto Sans (nội dung), cỡ gốc 16 px, dòng 1.5, số dùng `tabular-nums`.
- **Khoảng cách:** thang 4/8 px (`--sp-1` … `--sp-10`); bo góc 8 px (điều khiển) và 12 px (thẻ).
- **Chuyển động:** 150–220 ms, chỉ đổi màu nền/viền/vị trí; tắt hoàn toàn khi người dùng bật *giảm chuyển động*.
- **Chế độ tối:** thiết kế cùng lúc với chế độ sáng (`prefers-color-scheme`), màu tối được chọn lại chứ không đảo ngược.

## 4. Thành phần

- **Thanh bên** có biểu tượng + nhãn (bộ Phosphor, SVG, không dùng emoji), mục hiện tại tô sáng và có `aria-current`; trên màn hình ≤ 1024 px thành menu kéo ra mở bằng nút ☰ (Esc hoặc bấm nền mờ để đóng, focus trả về nút).
- **Thẻ số liệu, biểu đồ cột 7 ngày, danh sách "Cần nhập thêm"** ở trang Tổng quan; số tiền không bị ngắt dòng.
- **Bảng:** có `caption` và `scope` cho trình đọc màn hình, hàng sáng khi rê chuột, vùng cuộn ngang thao tác được bằng bàn phím.
- **Nhãn tồn kho** dùng màu + biểu tượng + chữ ("15 · Sắp hết", "0 · Hết hàng"), không chỉ dựa vào màu.
- **Biểu mẫu:** nhãn luôn hiện, dấu `*` cho ô bắt buộc, lỗi hiện ngay dưới ô (`aria-describedby`), khung tóm tắt lỗi tự nhận focus và có liên kết tới ô sai; nút hiện/ẩn mật khẩu.
- **Thông báo** có biểu tượng (thành công/lỗi), `role="status"` và `role="alert"`.
- Liên kết "Bỏ qua điều hướng", `lang="vi"`, mục tiêu bấm ≥ 24 px (≥ 44 px trên điện thoại).
- Hai trang XSLT (danh sách, hóa đơn) dùng cùng bảng màu và hỗ trợ chế độ tối; bản in luôn nền trắng.

## 5. Kiểm tra

- **axe-core** (WCAG 2.0/2.1/2.2 A–AA và best-practice) quét 15 trang × chế độ sáng/tối × máy tính/điện thoại: ban đầu phát hiện 3 nhóm lỗi cấu trúc (role của biểu đồ, hai vùng "complementary" trùng tên, vùng cuộn ngang không dùng được bằng bàn phím) và 1 nhóm do thiếu `caption`; đã sửa hết, kết quả cuối **0 vi phạm**.
- Xem trực tiếp ảnh chụp từng trang (sáng, tối, điện thoại, menu kéo ra): phát hiện và sửa số tiền bị ngắt dòng, thanh bên bị cắt ở màn hình thấp, bảng nhập hàng bị hẹp.
- Toàn bộ bộ kiểm thử cũ (logic, trình duyệt, RSS, SQL Server) vẫn đạt; thêm 6 test thủ công nhóm L trong [KiemThu.md](KiemThu.md).
