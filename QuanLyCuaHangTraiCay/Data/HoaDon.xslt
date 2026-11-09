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
          body { font-family: Segoe UI, Arial, sans-serif; width: 700px; margin: 30px auto; }
          h1, h3 { text-align: center; margin: 4px; }
          h1 { color: #2e7d32; }
          table { border-collapse: collapse; width: 100%; margin-top: 15px; }
          th, td { border: 1px solid #999; padding: 6px 10px; }
          th { background: #43a047; color: #fff; }
          .so { text-align: right; }
        </style>
      </head>
      <body>
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
              <tr>
                <th colspan="5" class="so">TỔNG TIỀN</th>
                <th class="so"><xsl:value-of select="format-number($hd/TongTien, '#,##0')"/> đ</th>
              </tr>
            </table>
            <p style="text-align:center"><i>Cảm ơn quý khách!</i></p>
          </xsl:otherwise>
        </xsl:choose>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
