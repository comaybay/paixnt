using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Paixnt
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(userName)) {
                MessageBox.Show("Vui lòng ghi tên người dùng");
                txtUserName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng ghi mật khẩu");
                txtPassword.Focus();
                return;
            }

            Database userExists = new Database("users",
                          $"select username from users where username='{userName}'");
            userExists.DocBang();

            if (userExists.Rows.Count != 0)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại, vui lòng chọn tên khác");
                txtUserName.Focus();
                return;
            }


            //heh 
            string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

            Database insertuser = new Database("users",
                $"insert into users values ('{userName}', '{encodedPassword}', 0)");
            insertuser.DocBang();

            var db = new Database("history",
            $"insert into history values ('{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}', " +
            $"'{userName}', N'user {userName} đã được khởi tạo')");
            db.DocBang();

            MessageBox.Show("Đăng ký thành công!");

            DialogResult = DialogResult.OK;
        }
    }
}
