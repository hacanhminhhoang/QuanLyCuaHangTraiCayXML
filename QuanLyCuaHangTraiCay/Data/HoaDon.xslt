<?xml version="1.0" encoding="utf-8"?>
<!--
  XSLT in hóa đơn bán hàng. Đầu vào: HoaDon.xml
  Dùng hàm document() để đọc thêm ChiTietHoaDon.xml, TraiCay.xml, KhachHang.xml, NhanVien.xml (nằm cùng thư mục).
  Tham số: MaHD = mã hóa đơn cần in.
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" encoding="utf-8" indent="yes"/>
  <xsl:param name="MaHD" select="''"/>
  <xsl:variable name="chiTiet" select="document('ChiTietHoaDon.xml')/NewDataSet/ChiTietHoaDon[MaHD = $MaHD]"/>
  <xsl:variable name="traiCay" select="document('TraiCay.xml')/NewDataSet/TraiCay"/>
  <xsl:variable name="khachHang" select="document('KhachHang.xml')/NewDataSet/KhachHang"/>
  <xsl:variable name="nhanVien" select="document('NhanVien.xml')/NewDataSet/NhanVien"/>

  <xsl:template match="/NewDataSet">
    <html>
      <head>
        <meta charset="utf-8"/>
        <title>Hóa đơn <xsl:value-of select="$MaHD"/></title>
        <style>
          body { font-family: "Be Vietnam Pro", "Segoe UI", Arial, sans-serif; color: #1d2a1f; background: #f5f7f2; margin: 0; padding: 32px 16px; }
          .giay { max-width: 720px; margin: 0 auto; background: #fff; border: 1px solid #e2e8dc; border-radius: 12px; padding: 32px; box-shadow: 0 4px 16px rgba(23,48,27,.06); }
          h1 { color: #2e7d32; margin: 0; font-size: 1.6rem; }
          .phu { color: #5c6b5e; margin: 4px 0 20px; }
          table { border-collapse: collapse; width: 100%; font-size: .92rem; }
          th { text-align: left; font-size: .75rem; letter-spacing: .04em; text-transform: uppercase; color: #5c6b5e; padding: 10px 12px; border-bottom: 2px solid #e2e8dc; }
          td { padding: 10px 12px; border-bottom: 1px solid #eef2ea; }
          .so { text-align: right; font-variant-numeric: tabular-nums; }
          @media print { body { background: #fff; padding: 0; } .giay { border: 0; box-shadow: none; } }
          h1, h3 { text-align: center; }
          h3 { margin: 4px 0 20px; letter-spacing: .08em; }
          tfoot th { font-size: 1rem; text-transform: none; letter-spacing: 0; color: #1d2a1f; border-top: 2px solid #e2e8dc; border-bottom: 0; }
          tfoot .tong { color: #ef6c00; font-size: 1.2rem; }
          .cam-on { text-align: center; color: #5c6b5e; margin-top: 24px; }
        </style>
      </head>
      <body>
        <div class="giay">
        <xsl:variable name="hd" select="HoaDon[MaHD = $MaHD]"/>
        <h1>CỬA HÀNG TRÁI CÂY</h1>
        <h3>HÓA ĐƠN BÁN HÀNG</h3>
        <xsl:choose>
          <xsl:when test="not($hd)">
            <p>Không tìm thấy hóa đơn <b><xsl:value-of select="$MaHD"/></b>.</p>
          </xsl:when>
          <xsl:otherwise>
            <p>
              Mã hóa đơn: <b><xsl:value-of select="$hd/MaHD"/></b><br/>
              Ngày lập: <xsl:value-of select="$hd/NgayLap"/><br/>
              Khách hàng: <xsl:value-of select="$khachHang[MaKH = $hd/MaKH]/TenKH"/><br/>
              Nhân viên: <xsl:value-of select="$nhanVien[MaNV = $hd/MaNV]/HoTen"/>
            </p>
            <table>
              <tr><th>STT</th><th>Trái cây</th><th>ĐVT</th><th>Số lượng</th><th>Đơn giá</th><th>Thành tiền</th></tr>
              <xsl:for-each select="$chiTiet">
                <xsl:variable name="tc" select="$traiCay[MaTC = current()/MaTC]"/>
                <tr>
                  <td><xsl:value-of select="position()"/></td>
                  <td><xsl:value-of select="$tc/TenTC"/></td>
                  <td><xsl:value-of select="$tc/DonViTinh"/></td>
                  <td class="so"><xsl:value-of select="SoLuong"/></td>
                  <td class="so"><xsl:value-of select="format-number(DonGia, '#,##0')"/></td>
                  <td class="so"><xsl:value-of select="format-number(ThanhTien, '#,##0')"/></td>
                </tr>
              </xsl:for-each>
              <tfoot><tr>
                <th colspan="5" class="so">Tổng tiền</th>
                <th class="so tong"><xsl:value-of select="format-number($hd/TongTien, '#,##0')"/> đ</th>
              </tr></tfoot>
            </table>
            <p class="cam-on"><i>Cảm ơn quý khách!</i></p>
          </xsl:otherwise>
        </xsl:choose>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
