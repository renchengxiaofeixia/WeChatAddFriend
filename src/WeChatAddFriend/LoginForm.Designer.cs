namespace WeChatAddFriend
{
    partial class LoginForm
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
            label1 = new Label();
            txtUserName = new TextBox();
            btnLogin = new Button();
            txtPassword = new TextBox();
            label2 = new Label();
            chkAutoLogin = new CheckBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(32, 15);
            label1.TabIndex = 0;
            label1.Text = "账号";
            // 
            // txtUserName
            // 
            txtUserName.Location = new Point(50, 12);
            txtUserName.Name = "txtUserName";
            txtUserName.Size = new Size(198, 23);
            txtUserName.TabIndex = 0;
            txtUserName.TextChanged += txtUserName_TextChanged;
            txtUserName.KeyUp += txtPassword_KeyUp;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(173, 70);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(75, 23);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "登录";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(50, 41);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '#';
            txtPassword.Size = new Size(198, 23);
            txtPassword.TabIndex = 1;
            txtPassword.TextChanged += txtPassword_TextChanged;
            txtPassword.KeyUp += txtPassword_KeyUp;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 44);
            label2.Name = "label2";
            label2.Size = new Size(32, 15);
            label2.TabIndex = 3;
            label2.Text = "密码";
            // 
            // chkAutoLogin
            // 
            chkAutoLogin.AutoSize = true;
            chkAutoLogin.Location = new Point(91, 73);
            chkAutoLogin.Name = "chkAutoLogin";
            chkAutoLogin.Size = new Size(76, 19);
            chkAutoLogin.TabIndex = 4;
            chkAutoLogin.Text = "自动登录";
            chkAutoLogin.UseVisualStyleBackColor = true;
            chkAutoLogin.Visible = false;
            chkAutoLogin.CheckedChanged += chkAutoLogin_CheckedChanged;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(271, 103);
            Controls.Add(chkAutoLogin);
            Controls.Add(txtPassword);
            Controls.Add(label2);
            Controls.Add(btnLogin);
            Controls.Add(txtUserName);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "登录";
            Load += LoginForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtUserName;
        private Button btnLogin;
        private TextBox txtPassword;
        private Label label2;
        private CheckBox chkAutoLogin;
    }
}