<?xml version="1.0" encoding="utf-8"?>
<!--
  XSLT dùng chung để hiển thị BẤT KỲ file XML danh mục nào (TraiCay.xml, KhachHang.xml...) ra bảng HTML.
  Tham số:
    TieuDe : tiêu đề trang
    TuKhoa : từ khóa lọc (rỗng = hiện tất cả); một dòng được giữ lại nếu có ít nhất một ô chứa từ khóa
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" encoding="utf-8" indent="yes"/>
  <xsl:param name="TieuDe" select="'DANH SÁCH'"/>
  <xsl:param name="TuKhoa" select="''"/>

  <xsl:template match="/NewDataSet">
    <xsl:variable name="dong" select="*[$TuKhoa = '' or */text()[contains(., $TuKhoa)]]"/>
    <html>
      <head>
        <meta charset="utf-8"/>
        <title><xsl:value-of select="$TieuDe"/></title>
        <style>
          body { font-family: "Be Vietnam Pro", "Segoe UI", Arial, sans-serif; color: #1d2a1f; background: #f5f7f2; margin: 0; padding: 32px 16px; }
          .giay { max-width: 1100px; margin: 0 auto; background: #fff; border: 1px solid #e2e8dc; border-radius: 12px; padding: 32px; box-shadow: 0 4px 16px rgba(23,48,27,.06); }
          h1 { color: #2e7d32; margin: 0; font-size: 1.6rem; }
          .phu { color: #5c6b5e; margin: 4px 0 20px; }
          table { border-collapse: collapse; width: 100%; font-size: .92rem; }
          th { text-align: left; font-size: .75rem; letter-spacing: .04em; text-transform: uppercase; color: #5c6b5e; padding: 10px 12px; border-bottom: 2px solid #e2e8dc; }
          td { padding: 10px 12px; border-bottom: 1px solid #eef2ea; }
          .so { text-align: right; font-variant-numeric: tabular-nums; }
          @media print { body { background: #fff; padding: 0; } .giay { border: 0; box-shadow: none; } }
          code { font-size: .85em; text-transform: none; color: #1b5e20; }
        </style>
      </head>
      <body>
        <div class="giay">
        <h1><xsl:value-of select="$TieuDe"/></h1>
        <p class="phu">
          Số dòng: <b><xsl:value-of select="count($dong)"/></b>
          <xsl:if test="$TuKhoa != ''"> · từ khóa: <b><xsl:value-of select="$TuKhoa"/></b></xsl:if>
          · biến đổi bằng DanhSach.xslt
        </p>
        <table>
          <tr>
            <th>STT</th>
            <!-- Lấy tên cột từ các thẻ con của phần tử đầu tiên -->
            <xsl:for-each select="*[1]/*">
              <th><code><xsl:value-of select="local-name()"/></code></th>
            </xsl:for-each>
          </tr>
          <xsl:for-each select="$dong">
            <tr>
              <td><xsl:value-of select="position()"/></td>
              <xsl:for-each select="*">
                <td>
                  <!-- Ô là số (không bắt đầu bằng 0 như mã/SĐT) thì căn phải -->
                  <xsl:if test="string(number(.)) != 'NaN' and not(starts-with(., '0'))">
                    <xsl:attribute name="class">so</xsl:attribute>
                  </xsl:if>
                  <xsl:value-of select="."/>
                </td>
              </xsl:for-each>
            </tr>
          </xsl:for-each>
        </table>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
