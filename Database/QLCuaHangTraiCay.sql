/* =========================================================
   CSDL: QLCuaHangTraiCay - Bài tập lớn Công nghệ XML
   Đề tài: Quản lý cửa hàng trái cây
   Chạy file này trong SSMS (F5) để tạo CSDL và dữ liệu mẫu.
   ========================================================= */
USE master;
GO
IF DB_ID(N'QLCuaHangTraiCay') IS NOT NULL
BEGIN
    ALTER DATABASE QLCuaHangTraiCay SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QLCuaHangTraiCay;
END
GO
CREATE DATABASE QLCuaHangTraiCay;
GO
USE QLCuaHangTraiCay;
GO

CREATE TABLE NhanVien (
    MaNV      NVARCHAR(10)  NOT NULL PRIMARY KEY,
    HoTen     NVARCHAR(100) NOT NULL,
    GioiTinh  NVARCHAR(5)   NOT NULL,
    NgaySinh  DATE          NOT NULL,
    DiaChi    NVARCHAR(150) NOT NULL,
    SDT       NVARCHAR(10)  NOT NULL
);

CREATE TABLE TaiKhoan (
    TenDangNhap NVARCHAR(50) NOT NULL PRIMARY KEY,
    MatKhau     NVARCHAR(50) NOT NULL,
    Quyen       NVARCHAR(20) NOT NULL CHECK (Quyen IN (N'Admin', N'NhanVien')),
    MaNV        NVARCHAR(10) NOT NULL REFERENCES NhanVien(MaNV)
);

CREATE TABLE NhaCungCap (
    MaNCC   NVARCHAR(10)  NOT NULL PRIMARY KEY,
    TenNCC  NVARCHAR(150) NOT NULL,
    DiaChi  NVARCHAR(150) NOT NULL,
    SDT     NVARCHAR(10)  NOT NULL
);

CREATE TABLE TraiCay (
    MaTC        NVARCHAR(10)  NOT NULL PRIMARY KEY,
    TenTC       NVARCHAR(100) NOT NULL,
    XuatXu      NVARCHAR(100) NOT NULL,
    DonViTinh   NVARCHAR(20)  NOT NULL,
    DonGia      DECIMAL(18,0) NOT NULL CHECK (DonGia >= 0),
    SoLuongTon  INT           NOT NULL CHECK (SoLuongTon >= 0),
    MaNCC       NVARCHAR(10)  NOT NULL REFERENCES NhaCungCap(MaNCC)
);

CREATE TABLE KhachHang (
    MaKH    NVARCHAR(10)  NOT NULL PRIMARY KEY,
    TenKH   NVARCHAR(100) NOT NULL,
    SDT     NVARCHAR(10)  NOT NULL DEFAULT N'',
    DiaChi  NVARCHAR(150) NOT NULL DEFAULT N''
);

CREATE TABLE HoaDon (
    MaHD      NVARCHAR(10)  NOT NULL PRIMARY KEY,
    NgayLap   DATE          NOT NULL,
    MaNV      NVARCHAR(10)  NOT NULL REFERENCES NhanVien(MaNV),
    MaKH      NVARCHAR(10)  NOT NULL REFERENCES KhachHang(MaKH),
    TongTien  DECIMAL(18,0) NOT NULL CHECK (TongTien >= 0)
);

CREATE TABLE ChiTietHoaDon (
    MaHD       NVARCHAR(10)  NOT NULL REFERENCES HoaDon(MaHD),
    MaTC       NVARCHAR(10)  NOT NULL REFERENCES TraiCay(MaTC),
    SoLuong    INT           NOT NULL CHECK (SoLuong > 0),
    DonGia     DECIMAL(18,0) NOT NULL CHECK (DonGia >= 0),
    ThanhTien  DECIMAL(18,0) NOT NULL CHECK (ThanhTien >= 0),
    PRIMARY KEY (MaHD, MaTC)
);

CREATE TABLE PhieuNhap (
    MaPN      NVARCHAR(10)  NOT NULL PRIMARY KEY,
    NgayNhap  DATE          NOT NULL,
    MaNV      NVARCHAR(10)  NOT NULL REFERENCES NhanVien(MaNV),
    MaNCC     NVARCHAR(10)  NOT NULL REFERENCES NhaCungCap(MaNCC),
    TongTien  DECIMAL(18,0) NOT NULL CHECK (TongTien >= 0)
);

CREATE TABLE ChiTietPhieuNhap (
    MaPN        NVARCHAR(10)  NOT NULL REFERENCES PhieuNhap(MaPN),
    MaTC        NVARCHAR(10)  NOT NULL REFERENCES TraiCay(MaTC),
    SoLuong     INT           NOT NULL CHECK (SoLuong > 0),
    DonGiaNhap  DECIMAL(18,0) NOT NULL CHECK (DonGiaNhap >= 0),
    ThanhTien   DECIMAL(18,0) NOT NULL CHECK (ThanhTien >= 0),
    PRIMARY KEY (MaPN, MaTC)
);
GO

-- ---------------- Dữ liệu mẫu (khớp với các file trong thư mục Data) ----------------
INSERT INTO NhanVien (MaNV, HoTen, GioiTinh, NgaySinh, DiaChi, SDT) VALUES
  (N'NV001', N'Nguyễn Văn An', N'Nam', N'1990-05-12', N'Hải Châu, Đà Nẵng', N'0905111222'),
  (N'NV002', N'Trần Thị Bình', N'Nữ', N'1998-09-20', N'Liên Chiểu, Đà Nẵng', N'0935333444'),
  (N'NV003', N'Lê Minh Châu', N'Nam', N'2000-01-15', N'Thanh Khê, Đà Nẵng', N'0914555666');
GO

INSERT INTO TaiKhoan (TenDangNhap, MatKhau, Quyen, MaNV) VALUES
  (N'admin', N'admin', N'Admin', N'NV001'),
  (N'binh', N'123', N'NhanVien', N'NV002'),
  (N'chau', N'123', N'NhanVien', N'NV003');
GO

INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, SDT) VALUES
  (N'NCC001', N'Vựa trái cây Hòa Khánh', N'Liên Chiểu, Đà Nẵng', N'0236371234'),
  (N'NCC002', N'HTX Trái cây Cái Bè', N'Cái Bè, Tiền Giang', N'0273382345'),
  (N'NCC003', N'Công ty Nhập khẩu Fresh Fruit', N'Quận 1, TP. Hồ Chí Minh', N'0283823456');
GO

INSERT INTO TraiCay (MaTC, TenTC, XuatXu, DonViTinh, DonGia, SoLuongTon, MaNCC) VALUES
  (N'TC001', N'Xoài cát Hòa Lộc', N'Tiền Giang', N'kg', 65000, 48, N'NCC002'),
  (N'TC002', N'Sầu riêng Ri6', N'Bến Tre', N'kg', 120000, 30, N'NCC002'),
  (N'TC003', N'Táo Envy', N'Mỹ', N'kg', 150000, 39, N'NCC003'),
  (N'TC004', N'Nho xanh Ninh Thuận', N'Ninh Thuận', N'kg', 70000, 25, N'NCC001'),
  (N'TC005', N'Thanh long ruột đỏ', N'Bình Thuận', N'kg', 40000, 57, N'NCC001'),
  (N'TC006', N'Cam sành', N'Vĩnh Long', N'kg', 35000, 80, N'NCC002'),
  (N'TC007', N'Kiwi vàng', N'New Zealand', N'kg', 180000, 20, N'NCC003'),
  (N'TC008', N'Dưa hấu Long An', N'Long An', N'quả', 45000, 35, N'NCC001');
GO

INSERT INTO KhachHang (MaKH, TenKH, SDT, DiaChi) VALUES
  (N'KH001', N'Khách lẻ', N'', N''),
  (N'KH002', N'Phạm Thu Hà', N'0905123456', N'Liên Chiểu, Đà Nẵng'),
  (N'KH003', N'Võ Quốc Huy', N'0935987654', N'Hải Châu, Đà Nẵng');
GO

INSERT INTO HoaDon (MaHD, NgayLap, MaNV, MaKH, TongTien) VALUES
  (N'HD001', N'2026-11-01', N'NV002', N'KH002', 280000),
  (N'HD002', N'2026-11-05', N'NV001', N'KH001', 120000);
GO

INSERT INTO ChiTietHoaDon (MaHD, MaTC, SoLuong, DonGia, ThanhTien) VALUES
  (N'HD001', N'TC001', 2, 65000, 130000),
  (N'HD001', N'TC003', 1, 150000, 150000),
  (N'HD002', N'TC005', 3, 40000, 120000);
GO

INSERT INTO PhieuNhap (MaPN, NgayNhap, MaNV, MaNCC, TongTien) VALUES
  (N'PN001', N'2026-10-30', N'NV001', N'NCC002', 1560000);
GO

INSERT INTO ChiTietPhieuNhap (MaPN, MaTC, SoLuong, DonGiaNhap, ThanhTien) VALUES
  (N'PN001', N'TC001', 20, 45000, 900000),
  (N'PN001', N'TC006', 30, 22000, 660000);
GO

