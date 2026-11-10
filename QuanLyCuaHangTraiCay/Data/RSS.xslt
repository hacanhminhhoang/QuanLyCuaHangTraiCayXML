<?xml version="1.0" encoding="utf-8"?>
<!--
  XSLT biến TraiCay.xml thành nguồn tin RSS 2.0 (bảng giá trái cây) để người dùng đăng ký theo dõi bằng trình đọc RSS.
  Tham số:
    BaseUrl     : địa chỉ gốc của website (vd: http://localhost:5080), dùng tạo liên kết tuyệt đối
    NgayCapNhat : ngày giờ dạng RFC 822 (XSLT 1.0 không có hàm ngày giờ nên server truyền vào)
  Không đưa số lượng tồn chính xác vào nguồn tin công khai, chỉ ghi "Còn hàng" / "Hết hàng".
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" encoding="utf-8" indent="yes"/>
  <!-- Hiển thị số kiểu Việt Nam: 65.000 -->
  <xsl:decimal-format decimal-separator="," grouping-separator="."/>
  <xsl:param name="BaseUrl" select="'http://localhost'"/>
  <xsl:param name="NgayCapNhat" select="''"/>

  <xsl:template match="/NewDataSet">
    <rss version="2.0">
      <channel>
        <title>Fruit Store – Bảng giá trái cây</title>
        <link><xsl:value-of select="$BaseUrl"/>/</link>
        <description>Giá bán và tình trạng hàng của cửa hàng trái cây, lấy từ TraiCay.xml.</description>
        <language>vi</language>
        <xsl:if test="$NgayCapNhat != ''">
          <lastBuildDate><xsl:value-of select="$NgayCapNhat"/></lastBuildDate>
        </xsl:if>
        <xsl:for-each select="TraiCay">
          <item>
            <title><xsl:value-of select="concat(TenTC, ' – ', format-number(DonGia, '#.##0'), ' đ/', DonViTinh)"/></title>
            <link><xsl:value-of select="concat($BaseUrl, '/DanhMuc/TraiCay?ma=', MaTC)"/></link>
            <description>
              <xsl:value-of select="concat('Xuất xứ: ', XuatXu, '. Giá: ', format-number(DonGia, '#.##0'), ' đ/', DonViTinh, '. ')"/>
              <xsl:choose>
                <xsl:when test="SoLuongTon &gt; 0">Còn hàng.</xsl:when>
                <xsl:otherwise>Hết hàng.</xsl:otherwise>
              </xsl:choose>
            </description>
            <guid isPermaLink="false"><xsl:value-of select="MaTC"/></guid>
          </item>
        </xsl:for-each>
      </channel>
    </rss>
  </xsl:template>
</xsl:stylesheet>
