namespace QuanLyCuaHangTraiCay.GUI
{
    partial class frmHoaDon
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtMaHD = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpNgayLap = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.cboKhachHang = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNhanVien = new System.Windows.Forms.TextBox();
            this.grpChon = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboTraiCay = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numSoLuong = new System.Windows.Forms.NumericUpDown();
            this.btnThemVaoGio = new System.Windows.Forms.Button();
            this.lblThongTin = new System.Windows.Forms.Label();
            this.dgvGio = new System.Windows.Forms.DataGridView();
            this.btnXoaKhoiGio = new System.Windows.Forms.Button();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.btnMoi = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnIn = new System.Windows.Forms.Button();
            this.grpChon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSoLuong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGio)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã hóa đơn";
            // 
            // txtMaHD
            // 
            this.txtMaHD.Location = new System.Drawing.Point(100, 12);
            this.txtMaHD.Name = "txtMaHD";
            this.txtMaHD.ReadOnly = true;
            this.txtMaHD.Size = new System.Drawing.Size(120, 20);
            this.txtMaHD.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(250, 16);
            this.label2.Name = "label2";
            this.label2.TabIndex = 2;
            this.label2.Text = "Ngày lập";
            // 
            // dtpNgayLap
            // 
            this.dtpNgayLap.CustomFormat = "dd/MM/yyyy";
            this.dtpNgayLap.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayLap.Location = new System.Drawing.Point(330, 12);
            this.dtpNgayLap.Name = "dtpNgayLap";
            this.dtpNgayLap.Size = new System.Drawing.Size(150, 20);
            this.dtpNgayLap.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 46);
            this.label3.Name = "label3";
            this.label3.TabIndex = 4;
            this.label3.Text = "Khách hàng";
            // 
            // cboKhachHang
            // 
            this.cboKhachHang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKhachHang.Location = new System.Drawing.Point(100, 42);
            this.cboKhachHang.Name = "cboKhachHang";
            this.cboKhachHang.Size = new System.Drawing.Size(220, 21);
            this.cboKhachHang.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(340, 46);
            this.label4.Name = "label4";
            this.label4.TabIndex = 6;
            this.label4.Text = "Nhân viên";
            // 
            // txtNhanVien
            // 
            this.txtNhanVien.Location = new System.Drawing.Point(420, 42);
            this.txtNhanVien.Name = "txtNhanVien";
            this.txtNhanVien.ReadOnly = true;
            this.txtNhanVien.Size = new System.Drawing.Size(200, 20);
            this.txtNhanVien.TabIndex = 7;
            // 
            // grpChon
            // 
            this.grpChon.Controls.Add(this.label5);
            this.grpChon.Controls.Add(this.cboTraiCay);
            this.grpChon.Controls.Add(this.label6);
            this.grpChon.Controls.Add(this.numSoLuong);
            this.grpChon.Controls.Add(this.btnThemVaoGio);
            this.grpChon.Controls.Add(this.lblThongTin);
            this.grpChon.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.grpChon.Location = new System.Drawing.Point(12, 75);
            this.grpChon.Name = "grpChon";
            this.grpChon.Size = new System.Drawing.Size(736, 80);
            this.grpChon.TabIndex = 8;
            this.grpChon.TabStop = false;
            this.grpChon.Text = "Chọn trái cây";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 26);
            this.label5.Name = "label5";
            this.label5.TabIndex = 9;
            this.label5.Text = "Trái cây";
            // 
            // cboTraiCay
            // 
            this.cboTraiCay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTraiCay.Location = new System.Drawing.Point(80, 22);
            this.cboTraiCay.Name = "cboTraiCay";
            this.cboTraiCay.Size = new System.Drawing.Size(220, 21);
            this.cboTraiCay.TabIndex = 10;
            this.cboTraiCay.SelectedIndexChanged += new System.EventHandler(this.cboTraiCay_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(315, 26);
            this.label6.Name = "label6";
            this.label6.TabIndex = 11;
            this.label6.Text = "Số lượng";
            // 
            // numSoLuong
            // 
            this.numSoLuong.Location = new System.Drawing.Point(375, 22);
            this.numSoLuong.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.numSoLuong.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numSoLuong.Name = "numSoLuong";
            this.numSoLuong.Size = new System.Drawing.Size(70, 20);
            this.numSoLuong.TabIndex = 12;
            this.numSoLuong.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnThemVaoGio
            // 
            this.btnThemVaoGio.Location = new System.Drawing.Point(460, 19);
            this.btnThemVaoGio.Name = "btnThemVaoGio";
            this.btnThemVaoGio.Size = new System.Drawing.Size(120, 28);
            this.btnThemVaoGio.TabIndex = 13;
            this.btnThemVaoGio.Text = "Thêm vào hóa đơn";
            this.btnThemVaoGio.UseVisualStyleBackColor = true;
            this.btnThemVaoGio.Click += new System.EventHandler(this.btnThemVaoGio_Click);
            // 
            // lblThongTin
            // 
            this.lblThongTin.AutoSize = true;
            this.lblThongTin.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblThongTin.Location = new System.Drawing.Point(80, 52);
            this.lblThongTin.Name = "lblThongTin";
            this.lblThongTin.TabIndex = 14;
            this.lblThongTin.Text = "";
            // 
            // dgvGio
            // 
            this.dgvGio.AllowUserToAddRows = false;
            this.dgvGio.AllowUserToDeleteRows = false;
            this.dgvGio.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.dgvGio.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGio.BackgroundColor = System.Drawing.Color.White;
            this.dgvGio.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGio.Location = new System.Drawing.Point(12, 165);
            this.dgvGio.MultiSelect = false;
            this.dgvGio.Name = "dgvGio";
            this.dgvGio.ReadOnly = true;
            this.dgvGio.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGio.Size = new System.Drawing.Size(736, 250);
            this.dgvGio.TabIndex = 15;
            // 
            // btnXoaKhoiGio
            // 
            this.btnXoaKhoiGio.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.btnXoaKhoiGio.Location = new System.Drawing.Point(12, 425);
            this.btnXoaKhoiGio.Name = "btnXoaKhoiGio";
            this.btnXoaKhoiGio.Size = new System.Drawing.Size(120, 32);
            this.btnXoaKhoiGio.TabIndex = 16;
            this.btnXoaKhoiGio.Text = "Xóa dòng chọn";
            this.btnXoaKhoiGio.UseVisualStyleBackColor = true;
            this.btnXoaKhoiGio.Click += new System.EventHandler(this.btnXoaKhoiGio_Click);
            // 
            // lblTongTien
            // 
            this.lblTongTien.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.ForeColor = System.Drawing.Color.Firebrick;
            this.lblTongTien.Location = new System.Drawing.Point(145, 432);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.TabIndex = 17;
            this.lblTongTien.Text = "Tổng tiền: 0 đ";
            // 
            // btnMoi
            // 
            this.btnMoi.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btnMoi.Location = new System.Drawing.Point(413, 425);
            this.btnMoi.Name = "btnMoi";
            this.btnMoi.Size = new System.Drawing.Size(105, 32);
            this.btnMoi.TabIndex = 18;
            this.btnMoi.Text = "Hóa đơn mới";
            this.btnMoi.UseVisualStyleBackColor = true;
            this.btnMoi.Click += new System.EventHandler(this.btnMoi_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btnLuu.Location = new System.Drawing.Point(528, 425);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(105, 32);
            this.btnLuu.TabIndex = 19;
            this.btnLuu.Text = "Lưu hóa đơn";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnIn
            // 
            this.btnIn.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btnIn.Location = new System.Drawing.Point(643, 425);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(105, 32);
            this.btnIn.TabIndex = 20;
            this.btnIn.Text = "In hóa đơn";
            this.btnIn.UseVisualStyleBackColor = true;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // frmHoaDon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 470);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMaHD);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtpNgayLap);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboKhachHang);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtNhanVien);
            this.Controls.Add(this.grpChon);
            this.Controls.Add(this.dgvGio);
            this.Controls.Add(this.btnXoaKhoiGio);
            this.Controls.Add(this.lblTongTien);
            this.Controls.Add(this.btnMoi);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnIn);
            this.MinimumSize = new System.Drawing.Size(776, 509);
            this.Name = "frmHoaDon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Lập hóa đơn bán hàng";
            this.Load += new System.EventHandler(this.frmHoaDon_Load);
            this.grpChon.ResumeLayout(false);
            this.grpChon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSoLuong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMaHD;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpNgayLap;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboKhachHang;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNhanVien;
        private System.Windows.Forms.GroupBox grpChon;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboTraiCay;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numSoLuong;
        private System.Windows.Forms.Button btnThemVaoGio;
        private System.Windows.Forms.Label lblThongTin;
        private System.Windows.Forms.DataGridView dgvGio;
        private System.Windows.Forms.Button btnXoaKhoiGio;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Button btnMoi;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnIn;
    }
}
