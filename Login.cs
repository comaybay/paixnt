using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Paixnt
{
    public partial class Login : Form
    {
        public bool IsAdmin { get; private set; }
        public string UserName { get; private set; }

        public bool PressedSignUp { get; private set; }

        public Login()
        {
            InitializeComponent();
            PressedSignUp = false;
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(userName))
            {
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

            //heh 
            string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

            Database bangquyen = new Database("users",
                $"select username, priority from users where username='{userName}' and password='{encodedPassword}'");
            bangquyen.DocBang();

            if (bangquyen.Rows.Count == 0)
            {
                MessageBox.Show("Đăng nhập không thành công");
                txtUserName.Focus();
            }
            else
            {
                var row = bangquyen.Rows[0];
                IsAdmin = row["PRIORITY"].ToString() == "1";
                UserName = userName;

                MessageBox.Show($"Chào {userName}.");

                DialogResult = DialogResult.OK;
            }
        }

        private void linkLabel1_MouseClick(object sender, MouseEventArgs e)
        {
            PressedSignUp = true;
            DialogResult = DialogResult.Cancel;
        }
    }
}
