
namespace Paixnt
{
    partial class MainMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenuTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.btnInfo = new System.Windows.Forms.Button();
            this.btnHistory = new System.Windows.Forms.Button();
            this.btnRunPaixnt = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.mainMenuTableLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuTableLayout
            // 
            this.mainMenuTableLayout.BackgroundImage = global::Paixnt.Properties.Resources.bg;
            this.mainMenuTableLayout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.mainMenuTableLayout.ColumnCount = 1;
            this.mainMenuTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainMenuTableLayout.Controls.Add(this.btnInfo, 0, 1);
            this.mainMenuTableLayout.Controls.Add(this.btnHistory, 0, 4);
            this.mainMenuTableLayout.Controls.Add(this.btnRunPaixnt, 0, 3);
            this.mainMenuTableLayout.Controls.Add(this.btnLogin, 0, 2);
            this.mainMenuTableLayout.Controls.Add(this.btnExit, 0, 5);
            this.mainMenuTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainMenuTableLayout.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.mainMenuTableLayout.Location = new System.Drawing.Point(0, 0);
            this.mainMenuTableLayout.Margin = new System.Windows.Forms.Padding(0);
            this.mainMenuTableLayout.Name = "mainMenuTableLayout";
            this.mainMenuTableLayout.RowCount = 7;
            this.mainMenuTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainMenuTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainMenuTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainMenuTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainMenuTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainMenuTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainMenuTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainMenuTableLayout.Size = new System.Drawing.Size(1228, 662);
            this.mainMenuTableLayout.TabIndex = 1;
            // 
            // btnInfo
            // 
            this.btnInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnInfo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnInfo.Location = new System.Drawing.Point(488, 96);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(251, 70);
            this.btnInfo.TabIndex = 3;
            this.btnInfo.Text = "Thông tin đề tài";
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnHistory.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnHistory.Location = new System.Drawing.Point(488, 396);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(251, 70);
            this.btnHistory.TabIndex = 1;
            this.btnHistory.Text = "Lịch Sử";
            this.btnHistory.UseVisualStyleBackColor = true;
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // btnRunPaixnt
            // 
            this.btnRunPaixnt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRunPaixnt.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnRunPaixnt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRunPaixnt.Location = new System.Drawing.Point(488, 296);
            this.btnRunPaixnt.Name = "btnRunPaixnt";
            this.btnRunPaixnt.Size = new System.Drawing.Size(251, 70);
            this.btnRunPaixnt.TabIndex = 0;
            this.btnRunPaixnt.Text = "Chạy Paixnt";
            this.btnRunPaixnt.UseVisualStyleBackColor = true;
            this.btnRunPaixnt.Click += new System.EventHandler(this.btnRunPaixnt_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLogin.Location = new System.Drawing.Point(488, 196);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(251, 70);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Đăng Nhập";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnExit.Location = new System.Drawing.Point(488, 496);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(251, 70);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Thoát";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1228, 662);
            this.Controls.Add(this.mainMenuTableLayout);
            this.DoubleBuffered = true;
            this.Name = "MainMenu";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Màn hình chính";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainMenu_FormClosed);
            this.mainMenuTableLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel mainMenuTableLayout;
        private System.Windows.Forms.Button btnRunPaixnt;
        private System.Windows.Forms.Button btnHistory;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.Button btnExit;
    }
}