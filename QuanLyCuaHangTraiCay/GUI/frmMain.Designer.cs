namespace QuanLyCuaHangTraiCay.GUI
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuHeThong = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDoiMatKhau = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDangXuat = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuThoat = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDanhMuc = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTraiCay = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNhaCungCap = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuKhachHang = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNhanVien = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTaiKhoan = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBanHang = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLapHoaDon = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDanhSachHoaDon = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDuLieu = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSqlSangXml = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuXmlSangSql = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuKiemTraHopLe = new System.Windows.Forms.ToolStripMenuItem();
            this.lblChaoMung = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblTrangThai = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHeThong,
            this.mnuDanhMuc,
            this.mnuBanHang,
            this.mnuDuLieu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            // 
            // mnuHeThong
            // 
            this.mnuHeThong.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDoiMatKhau,
            this.mnuDangXuat,
            this.mnuThoat});
            this.mnuHeThong.Name = "mnuHeThong";
            this.mnuHeThong.Size = new System.Drawing.Size(80, 20);
            this.mnuHeThong.Text = "Hệ thống";
            // 
            // mnuDoiMatKhau
            // 
            this.mnuDoiMatKhau.Name = "mnuDoiMatKhau";
            this.mnuDoiMatKhau.Size = new System.Drawing.Size(200, 22);
            this.mnuDoiMatKhau.Text = "Đổi mật khẩu";
            this.mnuDoiMatKhau.Click += new System.EventHandler(this.mnuDoiMatKhau_Click);
            // 
            // mnuDangXuat
            // 
            this.mnuDangXuat.Name = "mnuDangXuat";
            this.mnuDangXuat.Size = new System.Drawing.Size(200, 22);
            this.mnuDangXuat.Text = "Đăng xuất";
            this.mnuDangXuat.Click += new System.EventHandler(this.mnuDangXuat_Click);
            // 
            // mnuThoat
            // 
            this.mnuThoat.Name = "mnuThoat";
            this.mnuThoat.Size = new System.Drawing.Size(200, 22);
            this.mnuThoat.Text = "Thoát";
            this.mnuThoat.Click += new System.EventHandler(this.mnuThoat_Click);
            // 
            // mnuDanhMuc
            // 
            this.mnuDanhMuc.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTraiCay,
            this.mnuNhaCungCap,
            this.mnuKhachHang,
            this.mnuNhanVien,
            this.mnuTaiKhoan});
            this.mnuDanhMuc.Name = "mnuDanhMuc";
            this.mnuDanhMuc.Size = new System.Drawing.Size(80, 20);
            this.mnuDanhMuc.Text = "Danh mục";
            // 
            // mnuTraiCay
            // 
            this.mnuTraiCay.Name = "mnuTraiCay";
            this.mnuTraiCay.Size = new System.Drawing.Size(200, 22);
            this.mnuTraiCay.Text = "Trái cây";
            this.mnuTraiCay.Click += new System.EventHandler(this.mnuTraiCay_Click);
            // 
            // mnuNhaCungCap
            // 
            this.mnuNhaCungCap.Name = "mnuNhaCungCap";
            this.mnuNhaCungCap.Size = new System.Drawing.Size(200, 22);
            this.mnuNhaCungCap.Text = "Nhà cung cấp";
            this.mnuNhaCungCap.Click += new System.EventHandler(this.mnuNhaCungCap_Click);
            // 
            // mnuKhachHang
            // 
            this.mnuKhachHang.Name = "mnuKhachHang";
            this.mnuKhachHang.Size = new System.Drawing.Size(200, 22);
            this.mnuKhachHang.Text = "Khách hàng";
            this.mnuKhachHang.Click += new System.EventHandler(this.mnuKhachHang_Click);
            // 
            // mnuNhanVien
            // 
            this.mnuNhanVien.Name = "mnuNhanVien";
            this.mnuNhanVien.Size = new System.Drawing.Size(200, 22);
            this.mnuNhanVien.Text = "Nhân viên";
            this.mnuNhanVien.Click += new System.EventHandler(this.mnuNhanVien_Click);
            // 
            // mnuTaiKhoan
            // 
            this.mnuTaiKhoan.Name = "mnuTaiKhoan";
            this.mnuTaiKhoan.Size = new System.Drawing.Size(200, 22);
            this.mnuTaiKhoan.Text = "Tài khoản";
            this.mnuTaiKhoan.Click += new System.EventHandler(this.mnuTaiKhoan_Click);
            // 
            // mnuBanHang
            // 
            this.mnuBanHang.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLapHoaDon,
            this.mnuDanhSachHoaDon});
            this.mnuBanHang.Name = "mnuBanHang";
            this.mnuBanHang.Size = new System.Drawing.Size(80, 20);
            this.mnuBanHang.Text = "Bán hàng";
            // 
            // mnuLapHoaDon
            // 
            this.mnuLapHoaDon.Name = "mnuLapHoaDon";
            this.mnuLapHoaDon.Size = new System.Drawing.Size(200, 22);
            this.mnuLapHoaDon.Text = "Lập hóa đơn";
            this.mnuLapHoaDon.Click += new System.EventHandler(this.mnuLapHoaDon_Click);
            // 
            // mnuDanhSachHoaDon
            // 
            this.mnuDanhSachHoaDon.Name = "mnuDanhSachHoaDon";
            this.mnuDanhSachHoaDon.Size = new System.Drawing.Size(200, 22);
            this.mnuDanhSachHoaDon.Text = "Danh sách hóa đơn";
            this.mnuDanhSachHoaDon.Click += new System.EventHandler(this.mnuDanhSachHoaDon_Click);
            // 
            // mnuDuLieu
            // 
            this.mnuDuLieu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSqlSangXml,
            this.mnuXmlSangSql,
            this.mnuKiemTraHopLe});
            this.mnuDuLieu.Name = "mnuDuLieu";
            this.mnuDuLieu.Size = new System.Drawing.Size(80, 20);
            this.mnuDuLieu.Text = "Dữ liệu";
            // 
            // mnuSqlSangXml
            // 
            this.mnuSqlSangXml.Name = "mnuSqlSangXml";
            this.mnuSqlSangXml.Size = new System.Drawing.Size(200, 22);
            this.mnuSqlSangXml.Text = "Database -> XML";
            this.mnuSqlSangXml.Click += new System.EventHandler(this.mnuSqlSangXml_Click);
            // 
            // mnuXmlSangSql
            // 
            this.mnuXmlSangSql.Name = "mnuXmlSangSql";
            this.mnuXmlSangSql.Size = new System.Drawing.Size(200, 22);
            this.mnuXmlSangSql.Text = "XML -> Database";
            this.mnuXmlSangSql.Click += new System.EventHandler(this.mnuXmlSangSql_Click);
            // 
            // mnuKiemTraHopLe
            // 
            this.mnuKiemTraHopLe.Name = "mnuKiemTraHopLe";
            this.mnuKiemTraHopLe.Size = new System.Drawing.Size(200, 22);
            this.mnuKiemTraHopLe.Text = "Kiểm tra hợp lệ (XSD)";
            this.mnuKiemTraHopLe.Click += new System.EventHandler(this.mnuKiemTraHopLe_Click);
            // 
            // lblChaoMung
            // 
            this.lblChaoMung.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChaoMung.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblChaoMung.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblChaoMung.Location = new System.Drawing.Point(0, 24);
            this.lblChaoMung.Name = "lblChaoMung";
            this.lblChaoMung.Size = new System.Drawing.Size(800, 404);
            this.lblChaoMung.TabIndex = 1;
            this.lblChaoMung.Text = "PHẦN MỀM QUẢN LÝ CỬA HÀNG TRÁI CÂY\r\n(Công nghệ XML)";
            this.lblChaoMung.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblTrangThai});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 2;
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(70, 17);
            this.lblTrangThai.Text = "Người dùng:";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblChaoMung);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý cửa hàng trái cây";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuHeThong;
        private System.Windows.Forms.ToolStripMenuItem mnuDoiMatKhau;
        private System.Windows.Forms.ToolStripMenuItem mnuDangXuat;
        private System.Windows.Forms.ToolStripMenuItem mnuThoat;
        private System.Windows.Forms.ToolStripMenuItem mnuDanhMuc;
        private System.Windows.Forms.ToolStripMenuItem mnuTraiCay;
        private System.Windows.Forms.ToolStripMenuItem mnuNhaCungCap;
        private System.Windows.Forms.ToolStripMenuItem mnuKhachHang;
        private System.Windows.Forms.ToolStripMenuItem mnuNhanVien;
        private System.Windows.Forms.ToolStripMenuItem mnuTaiKhoan;
        private System.Windows.Forms.ToolStripMenuItem mnuBanHang;
        private System.Windows.Forms.ToolStripMenuItem mnuLapHoaDon;
        private System.Windows.Forms.ToolStripMenuItem mnuDanhSachHoaDon;
        private System.Windows.Forms.ToolStripMenuItem mnuDuLieu;
        private System.Windows.Forms.ToolStripMenuItem mnuSqlSangXml;
        private System.Windows.Forms.ToolStripMenuItem mnuXmlSangSql;
        private System.Windows.Forms.ToolStripMenuItem mnuKiemTraHopLe;
        private System.Windows.Forms.Label lblChaoMung;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblTrangThai;
    }
}
