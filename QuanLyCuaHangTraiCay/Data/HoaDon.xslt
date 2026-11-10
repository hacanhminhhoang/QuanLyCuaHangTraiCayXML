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
        <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Be+Vietnam+Pro:wght@700&amp;family=Noto+Sans:wght@400;600&amp;display=swap"/>
        <title>Hóa đơn <xsl:value-of select="$MaHD"/></title>
        <style>
          :root { color-scheme: light dark; --bg: #f0f7f3; --card: #ffffff; --fg: #0f172a; --muted: #475569; --border: #d5e5dc; --primary: #047857; --accent: #b45309; --tint: #e7f5ec; }
          @media (prefers-color-scheme: dark) { :root { --bg: #0b1712; --card: #12241c; --fg: #e7f3ec; --muted: #a7bcb1; --border: #27443a; --primary: #6ee7b7; --accent: #fbbf24; --tint: #16301f; } }
          body { font-family: "Noto Sans", "Segoe UI", system-ui, sans-serif; line-height: 1.5; color: var(--fg); background: var(--bg); margin: 0; padding: 32px 16px; }
          .giay { max-width: 720px; margin: 0 auto; background: var(--card); border: 1px solid var(--border); border-radius: 12px; padding: 32px; }
          h1 { font-family: "Be Vietnam Pro", "Noto Sans", sans-serif; color: var(--primary); margin: 0; font-size: 1.75rem; text-wrap: balance; }
          .phu { color: var(--muted); margin: 4px 0 20px; }
          table { border-collapse: collapse; width: 100%; font-size: .9375rem; }
          th { text-align: left; font-size: .8125rem; letter-spacing: .03em; text-transform: uppercase; color: var(--muted); padding: 10px 12px; border-bottom: 2px solid var(--border); }
          td { padding: 10px 12px; border-bottom: 1px solid var(--border); }
          tbody tr:hover td { background: var(--tint); }
          .so { text-align: right; font-variant-numeric: tabular-nums; }
          @media print { :root { --bg: #fff; --card: #fff; --fg: #000; --muted: #333; --border: #bbb; --primary: #065f46; --accent: #92400e; } body { padding: 0; } .giay { border: 0; } }
          h1, h3 { text-align: center; }
          h3 { font-family: "Be Vietnam Pro", "Noto Sans", sans-serif; margin: 4px 0 20px; letter-spacing: .08em; }
          tfoot th { font-size: 1rem; text-transform: none; letter-spacing: 0; color: var(--fg); border-top: 2px solid var(--border); border-bottom: 0; }
          tfoot .tong { color: var(--accent); font-size: 1.25rem; }
          .cam-on { text-align: center; color: var(--muted); margin-top: 24px; }
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
