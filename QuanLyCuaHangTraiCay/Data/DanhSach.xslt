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
          body { font-family: Segoe UI, Arial, sans-serif; margin: 30px; }
          h1 { text-align: center; color: #2e7d32; }
          table { border-collapse: collapse; width: 100%; }
          th { background: #43a047; color: #fff; }
          th, td { border: 1px solid #999; padding: 6px 10px; }
          tr:nth-child(even) td { background: #f1f8e9; }
        </style>
      </head>
      <body>
        <h1><xsl:value-of select="$TieuDe"/></h1>
        <xsl:if test="$TuKhoa != ''">
          <p>Từ khóa tìm kiếm: <b><xsl:value-of select="$TuKhoa"/></b></p>
        </xsl:if>
        <p>Số dòng: <b><xsl:value-of select="count($dong)"/></b></p>
        <table>
          <tr>
            <th>STT</th>
            <!-- Lấy tên cột từ các thẻ con của phần tử đầu tiên -->
            <xsl:for-each select="*[1]/*">
              <th><xsl:value-of select="local-name()"/></th>
            </xsl:for-each>
          </tr>
          <xsl:for-each select="$dong">
            <tr>
              <td><xsl:value-of select="position()"/></td>
              <xsl:for-each select="*">
                <td><xsl:value-of select="."/></td>
              </xsl:for-each>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
