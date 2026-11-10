using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using Microsoft.Data.SqlClient;

namespace QuanLyCuaHangTraiCay.Class
{
    /// <summary>
    /// Lớp dùng chung để thao tác với các file XML trong thư mục Data:
    /// đọc, thêm, sửa, xóa (XmlDocument + XPath), kiểm tra hợp lệ (XSD),
    /// hiển thị ra trình duyệt (XSLT) và đồng bộ với CSDL SQL Server.
    /// Mỗi bảng có 1 file XML (vd: TraiCay.xml) và 1 file lược đồ (TraiCay.xsd).
    /// </summary>
    public static class FileXml
    {
        // Thứ tự bảng từ cha -> con (dùng khi đồng bộ với SQL Server để không vi phạm khóa ngoại)
        public static readonly string[] CacBang =
            { "NhanVien", "TaiKhoan", "NhaCungCap", "TraiCay", "KhachHang", "HoaDon", "ChiTietHoaDon" };

        // Thư mục chứa XML/XSD/XSLT và chuỗi kết nối SQL Server: được gán trong Program.cs khi web khởi động
        public static string ThuMuc = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        public static string ChuoiKetNoi;

        // Web có nhiều request cùng lúc: mọi thao tác đọc-sửa-ghi file XML phải lần lượt
        // ponytail: 1 khóa chung cho mọi file, đủ cho cửa hàng nhỏ; tách khóa theo file nếu cần nhanh hơn
        public static readonly object KhoaGhi = new object();

        // Ghi file có thụt lề. Không dùng XmlDocument.Save(đường dẫn): bộ ghi cũ đó chèn xuống dòng + khoảng trắng
        // vào thẻ rỗng (<SDT></SDT> thành <SDT>\n    </SDT>), làm file sai lược đồ XSD.
        static readonly XmlWriterSettings CaiDatGhi = new XmlWriterSettings { Indent = true };

        public static string DuongDan(string bang)
        {
            return Path.Combine(ThuMuc, bang + ".xml");
        }

        // ===================== ĐỌC DỮ LIỆU =====================

        /// <summary>Đọc file XML vào DataTable để hiển thị lên DataGridView.
        /// Đọc lược đồ XSD trước để các cột có đúng kiểu dữ liệu (số, ngày...).</summary>
        public static DataTable HienThi(string bang)
        {
            DataSet ds = new DataSet();
            ds.ReadXmlSchema(Path.Combine(ThuMuc, bang + ".xsd"));
            ds.ReadXml(DuongDan(bang), XmlReadMode.IgnoreSchema);
            return ds.Tables[bang];
        }

        /// <summary>Kiểm tra một giá trị đã tồn tại chưa (vd: mã trái cây bị trùng).</summary>
        public static bool KiemTraMa(string bang, string truong, string giaTri)
        {
            return Mo(bang).SelectSingleNode(XPath(bang, truong, giaTri)) != null;
        }

        /// <summary>Lấy giá trị trường B của dòng có trường A = giá trị A.
        /// Vd: LayGiaTri("TraiCay", "MaTC", "TC001", "TenTC") -> "Xoài cát Hòa Lộc"</summary>
        public static string LayGiaTri(string bang, string truongA, string giaTriA, string truongB)
        {
            XmlNode nut = Mo(bang).SelectSingleNode(XPath(bang, truongA, giaTriA) + "/" + truongB);
            return nut == null ? "" : nut.InnerText;
        }

        /// <summary>Sinh mã mới tăng dần: TC001, TC002... -> TC009</summary>
        public static string TaoMaMoi(string bang, string truong, string tienTo)
        {
            int max = 0;
            foreach (XmlNode nut in Mo(bang).SelectNodes("/NewDataSet/" + bang + "/" + truong))
            {
                int so;
                string ma = nut.InnerText.Trim();
                if (ma.StartsWith(tienTo) && int.TryParse(ma.Substring(tienTo.Length), out so) && so > max)
                    max = so;
            }
            return tienTo + (max + 1).ToString("D3");
        }

        /// <summary>Tìm bảng khác đang tham chiếu tới giá trị này (khóa ngoại).
        /// Trả về tên bảng, hoặc null nếu không bảng nào dùng -> được phép xóa.</summary>
        public static string DangDuocDung(string bang, string truong, string giaTri)
        {
            foreach (string b in CacBang)
                if (b != bang && KiemTraMa(b, truong, giaTri)) return b;
            return null;
        }

        // ===================== THÊM / SỬA / XÓA =====================

        public static void Them(string bang, string[] cot, string[] giaTri)
        {
            lock (KhoaGhi)
            {
                XmlDocument doc = Mo(bang);
                XmlElement dong = doc.CreateElement(bang);
                for (int i = 0; i < cot.Length; i++)
                {
                    XmlElement o = doc.CreateElement(cot[i]);
                    o.InnerText = giaTri[i];   // InnerText tự xử lý ký tự đặc biệt như < > &
                    dong.AppendChild(o);
                }
                doc.DocumentElement.AppendChild(dong);
                Luu(doc, bang);
            }
        }

        /// <summary>Sửa các cột được truyền vào của dòng có truongKhoa = giaTriKhoa.</summary>
        public static void Sua(string bang, string truongKhoa, string giaTriKhoa, string[] cot, string[] giaTri)
        {
            lock (KhoaGhi)
            {
                XmlDocument doc = Mo(bang);
                XmlNode dong = doc.SelectSingleNode(XPath(bang, truongKhoa, giaTriKhoa));
                if (dong == null)
                    throw new Exception("Không tìm thấy " + truongKhoa + " = " + giaTriKhoa);
                for (int i = 0; i < cot.Length; i++)
                    dong[cot[i]].InnerText = giaTri[i];
                Luu(doc, bang);
            }
        }

        /// <summary>Xóa mọi dòng có truong = giaTri. Trả về số dòng đã xóa.</summary>
        public static int Xoa(string bang, string truong, string giaTri)
        {
            lock (KhoaGhi)
            {
                XmlDocument doc = Mo(bang);
                List<XmlNode> ds = doc.SelectNodes(XPath(bang, truong, giaTri)).Cast<XmlNode>().ToList();
                foreach (XmlNode dong in ds)
                    dong.ParentNode.RemoveChild(dong);
                Luu(doc, bang);
                return ds.Count;
            }
        }

        // ===================== KIỂM TRA HỢP LỆ (XSD) =====================

        /// <summary>Kiểm tra file XML có hợp lệ theo lược đồ XSD không. Danh sách rỗng = hợp lệ.</summary>
        public static List<string> KiemTraHopLe(string bang)
        {
            try
            {
                using (XmlReader r = XmlReader.Create(DuongDan(bang)))
                    return KiemTra(r, bang);
            }
            catch (XmlException ex)   // file sai cú pháp (thiếu thẻ đóng...)
            {
                return new List<string> { "Lỗi cú pháp XML: " + ex.Message };
            }
        }

        static List<string> KiemTra(XmlReader nguon, string bang)
        {
            List<string> loi = new List<string>();
            XmlReaderSettings st = new XmlReaderSettings();
            st.ValidationType = ValidationType.Schema;
            st.Schemas.Add(null, Path.Combine(ThuMuc, bang + ".xsd"));
            st.ValidationEventHandler += (s, e) =>
                loi.Add((e.Exception.LineNumber > 0 ? "Dòng " + e.Exception.LineNumber + ": " : "") + e.Message);
            using (XmlReader r = XmlReader.Create(nguon, st))
                while (r.Read()) { }
            return loi;
        }

        // ===================== HIỂN THỊ TRÌNH DUYỆT (XSLT) =====================

        /// <summary>Biến đổi file XML bằng XSLT, trả về chuỗi HTML để gửi thẳng cho trình duyệt.
        /// thamSo: các cặp tên, giá trị. Vd: XuatHTML("TraiCay", "DanhSach", "TuKhoa", "Xoài")</summary>
        public static string XuatHTML(string bang, string tenXslt, params string[] thamSo)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            // XsltSettings(true, ...) cho phép hàm document() để đọc thêm file XML khác
            xslt.Load(Path.Combine(ThuMuc, tenXslt + ".xslt"), new XsltSettings(true, false), new XmlUrlResolver());

            XsltArgumentList args = new XsltArgumentList();
            for (int i = 0; i + 1 < thamSo.Length; i += 2)
                args.AddParam(thamSo[i], "", thamSo[i + 1]);

            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlReader r = XmlReader.Create(DuongDan(bang)))
                using (XmlWriter w = XmlWriter.Create(ms, xslt.OutputSettings))
                    xslt.Transform(r, args, w, new XmlUrlResolver());   // resolver để document() đọc được file khác
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        // ===================== ĐỒNG BỘ VỚI SQL SERVER =====================

        /// <summary>Database -> XML: đọc toàn bộ các bảng trong SQL Server và ghi đè các file XML.</summary>
        public static void TaoXMLTuSQL()
        {
            // Đọc hết dữ liệu trước, nếu lỗi kết nối thì các file XML cũ vẫn còn nguyên
            List<DataTable> ds = new List<DataTable>();
            using (SqlConnection con = new SqlConnection(ChuoiKetNoi))
            {
                foreach (string bang in CacBang)
                {
                    DataTable dt = new DataTable(bang);
                    new SqlDataAdapter("SELECT * FROM " + bang, con).Fill(dt);
                    ds.Add(dt);
                }
            }

            lock (KhoaGhi)
            foreach (DataTable dt in ds)
            {
                using (XmlWriter w = XmlWriter.Create(DuongDan(dt.TableName), CaiDatGhi))
                {
                    w.WriteStartElement("NewDataSet");
                    foreach (DataRow r in dt.Rows)
                    {
                        w.WriteStartElement(dt.TableName);
                        foreach (DataColumn c in dt.Columns)
                            w.WriteElementString(c.ColumnName, DinhDang(r[c]));
                        w.WriteEndElement();
                    }
                    w.WriteEndElement();
                }
            }
        }

        /// <summary>XML -> Database: kiểm tra hợp lệ rồi ghi toàn bộ dữ liệu XML vào SQL Server
        /// (trong 1 transaction: lỗi ở bất kỳ bảng nào thì CSDL giữ nguyên như cũ).</summary>
        public static void CapNhatSQL()
        {
            foreach (string bang in CacBang)
            {
                List<string> loi = KiemTraHopLe(bang);
                if (loi.Count > 0)
                    throw new Exception(bang + ".xml không hợp lệ:\n" + string.Join("\n", loi));
            }

            using (SqlConnection con = new SqlConnection(ChuoiKetNoi))
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    // Xóa từ bảng con -> bảng cha
                    for (int i = CacBang.Length - 1; i >= 0; i--)
                        new SqlCommand("DELETE FROM " + CacBang[i], con, tran).ExecuteNonQuery();

                    // Chèn từ bảng cha -> bảng con, có kiểm tra ràng buộc khóa ngoại
                    foreach (string bang in CacBang)
                    {
                        DataTable dt = HienThi(bang);
                        using (SqlBulkCopy bulk = new SqlBulkCopy(con, SqlBulkCopyOptions.CheckConstraints, tran))
                        {
                            bulk.DestinationTableName = bang;
                            foreach (DataColumn c in dt.Columns)
                                bulk.ColumnMappings.Add(c.ColumnName, c.ColumnName);
                            bulk.WriteToServer(dt);
                        }
                    }
                    tran.Commit();
                }
            }
        }

        // ===================== HÀM HỖ TRỢ =====================

        static XmlDocument Mo(string bang)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(DuongDan(bang));
            return doc;
        }

        /// <summary>Kiểm tra theo XSD trước khi ghi file: dữ liệu sai thì báo lỗi và KHÔNG ghi.</summary>
        static void Luu(XmlDocument doc, string bang)
        {
            List<string> loi = KiemTra(new XmlNodeReader(doc), bang);
            if (loi.Count > 0)
                throw new Exception("Dữ liệu không hợp lệ theo lược đồ " + bang + ".xsd:\n" + string.Join("\n", loi));
            using (XmlWriter w = XmlWriter.Create(DuongDan(bang), CaiDatGhi))
                doc.Save(w);
        }

        // Vd: XPath("TraiCay", "MaTC", "TC001") -> /NewDataSet/TraiCay[MaTC='TC001']
        static string XPath(string bang, string truong, string giaTri)
        {
            string chuoi = giaTri.Contains("'") ? "\"" + giaTri + "\"" : "'" + giaTri + "'";
            return "/NewDataSet/" + bang + "[" + truong + "=" + chuoi + "]";
        }

        static string DinhDang(object giaTri)
        {
            if (giaTri is DateTime) return ((DateTime)giaTri).ToString("yyyy-MM-dd");
            IFormattable f = giaTri as IFormattable;
            return f != null ? f.ToString(null, CultureInfo.InvariantCulture) : Convert.ToString(giaTri).Trim();
        }
    }
}
