using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace Paixnt
{
    class Database : DataTable
    {
        SqlConnection ketNoi;
        SqlDataAdapter boDocGhi;

        string tenBang;
        string chuoiTruyVan;
        public static string ChuoiKetNoi = "Server=.\\sqlexpress;Database=QLND;Integrated Security=true";

        private void TaoKetNoi()
        {
            if (ketNoi == null)
                ketNoi = new SqlConnection(ChuoiKetNoi);
        }

        public Database(string tenBang, string chuoiTruyVan = "") : base(tenBang)
        {
            this.tenBang = tenBang;
            this.chuoiTruyVan = chuoiTruyVan;
        }

        public void DocBang()
        {
            TaoKetNoi();
            if (string.IsNullOrWhiteSpace(chuoiTruyVan))
                chuoiTruyVan = $"SELECT * FROM {tenBang}";

            boDocGhi = new SqlDataAdapter(chuoiTruyVan, ketNoi);
            try
            {
                boDocGhi.FillSchema(this, SchemaType.Mapped);
                boDocGhi.Fill(this);
                boDocGhi.SelectCommand.CommandText = $"SELECT * FROM {tenBang}";
                var boPhatSinhLenh = new SqlCommandBuilder(boDocGhi);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Kết nối với database thất bại: database server không tồn tại hoặc không thể truy cập đến được",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            };
        }

        public bool Ghi()
        {
            try
            {
                boDocGhi.Update(this);
                AcceptChanges();
                return true;
            }
            catch
            {
                MessageBox.Show("Lỗi cập nhật dữ liệu, vui lòng thử lại.", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RejectChanges();
                return false;
            }
        }
    }
}
