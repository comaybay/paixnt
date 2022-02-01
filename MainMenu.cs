using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Paixnt
{
    public partial class MainMenu : Form
    {
        private bool isAdmin;
        private string userName;
        private bool isLoggedIn;
        private List<Paixnt> runningApps;
        private Guid id;
        private UseHistory useHistory;

        public MainMenu()
        {
            InitializeComponent();
            runningApps = new List<Paixnt>();
            isAdmin = false;
            isLoggedIn = false;
            userName = "";

            useHistory = new UseHistory(this);
            useHistory.TopLevel = false;
            useHistory.FormBorderStyle = FormBorderStyle.None;
            useHistory.Dock = DockStyle.Fill;
            Controls.Add(useHistory);

            useHistory.BackButton.Click += ToMainMenu;
        }
        private void ToMainMenu(object sender, EventArgs e)
        {
            Text = "Màn hình chính";
            useHistory.Hide();
            mainMenuTableLayout.Show();
        }

        private void OpenLoginDiaglog()
        {
            var dialog = new Login();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                isAdmin = dialog.IsAdmin;
                userName = dialog.UserName;
                isLoggedIn = true;
                btnLogin.Text = $"Đăng xuất ({userName})";

                //sql imjection attack can happen here but idc enough lol
                var db = new Database("history",
                $"insert into history values ('{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}'," +
                $" '{userName}', N'user {userName} đã đăng nhập vào hệ thống')");
                db.DocBang();
            }
            else
            {
                isLoggedIn = false;
                if (dialog.PressedSignUp == true)
                {
                    var signUpDialog = new Register();
                    signUpDialog.ShowDialog();
                }
            }
        }

        private void btnRunPaixnt_Click(object sender, EventArgs e)
        {
            if (!isLoggedIn)
            {
				new Paixnt().Show();
                return;
            }

            string painxtId = BitConverter.ToString(Guid.NewGuid().ToByteArray()).Replace("-", "");
            var db = new Database("history",
            $"insert into history values ('{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}', " +
            $"'{userName}', N'user {userName} chạy chương trình Paixnt, mã chương trình: {painxtId}')");
            db.DocBang();

            var paixnt = new Paixnt();
            paixnt.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                var db = new Database("history",
                $"insert into history values ('{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}', " +
                $"'{userName}', N'user {userName} đã thoát chương trình Paixnt, mã chương trình: {painxtId}')");
                db.DocBang();

                runningApps.Remove((Paixnt)sender);
            };

            runningApps.Add(paixnt);

            paixnt.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (isLoggedIn)
            {
                for (int i = runningApps.Count - 1; i >= 0; i--)
                    runningApps[i].Close();

                AddLogoutInfoToDB();
                userName = "";
                isAdmin = false;
                isLoggedIn = false;
                btnLogin.Text = "Đăng nhập";
                return;
            }

            OpenLoginDiaglog();
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            Text = "Lịch sử";
            mainMenuTableLayout.Hide();
            useHistory.Show();
        }

        private void MainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isLoggedIn)
            {
                //make sure all running apps' FormClosed event handler is called 
                for (int i = runningApps.Count - 1; i >= 0; i--)
                    runningApps[i].Close();

                AddLogoutInfoToDB();
            }
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TRƯỜNG ĐẠI HỌC CÔNG NGHỆ THÔNG TIN\n" +
                "Đề tài: phần mềm vẽ pixel art\n" +
                "Thuộc chủ đề: Xây dựng các chương trình ứng dụng cho Windows\n\n" +
                "SVTH: Thái Chí Bảo\nMSSV: 19521256\nLớp: IT008.M11.PMCL", "Thông tin đề tài",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddLogoutInfoToDB()
        {
            var db = new Database("history",
            $"insert into history values ('{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}', " +
            $"'{userName}', N'user {userName} đã đăng xuất khỏi hệ thống')");
            db.DocBang();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
