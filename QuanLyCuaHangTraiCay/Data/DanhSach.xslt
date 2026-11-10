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
        <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Be+Vietnam+Pro:wght@700&amp;family=Noto+Sans:wght@400;600&amp;display=swap"/>
        <title><xsl:value-of select="$TieuDe"/></title>
        <style>
          :root { color-scheme: light dark; --bg: #f0f7f3; --card: #ffffff; --fg: #0f172a; --muted: #475569; --border: #d5e5dc; --primary: #047857; --accent: #b45309; --tint: #e7f5ec; }
          @media (prefers-color-scheme: dark) { :root { --bg: #0b1712; --card: #12241c; --fg: #e7f3ec; --muted: #a7bcb1; --border: #27443a; --primary: #6ee7b7; --accent: #fbbf24; --tint: #16301f; } }
          body { font-family: "Noto Sans", "Segoe UI", system-ui, sans-serif; line-height: 1.5; color: var(--fg); background: var(--bg); margin: 0; padding: 32px 16px; }
          .giay { max-width: 1100px; margin: 0 auto; background: var(--card); border: 1px solid var(--border); border-radius: 12px; padding: 32px; }
          h1 { font-family: "Be Vietnam Pro", "Noto Sans", sans-serif; color: var(--primary); margin: 0; font-size: 1.75rem; text-wrap: balance; }
          .phu { color: var(--muted); margin: 4px 0 20px; }
          table { border-collapse: collapse; width: 100%; font-size: .9375rem; }
          th { text-align: left; font-size: .8125rem; letter-spacing: .03em; text-transform: uppercase; color: var(--muted); padding: 10px 12px; border-bottom: 2px solid var(--border); }
          td { padding: 10px 12px; border-bottom: 1px solid var(--border); }
          tbody tr:hover td { background: var(--tint); }
          .so { text-align: right; font-variant-numeric: tabular-nums; }
          @media print { :root { --bg: #fff; --card: #fff; --fg: #000; --muted: #333; --border: #bbb; --primary: #065f46; --accent: #92400e; } body { padding: 0; } .giay { border: 0; } }
          code { font-size: .85em; text-transform: none; color: var(--primary); }
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
