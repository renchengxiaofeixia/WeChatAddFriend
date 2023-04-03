namespace WeChatAddFriend
{
    partial class WeChatAddFriendForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnAdd = new Button();
            lblDeviceNum = new Label();
            btnImportData = new Button();
            label2 = new Label();
            label3 = new Label();
            txtSecd = new TextBox();
            label1 = new Label();
            txtLog = new RichTextBox();
            btnSaveMoments = new Button();
            btnSendMoments = new MaterialSkin.Controls.MaterialButton();
            materialButton1 = new MaterialSkin.Controls.MaterialButton();
            SuspendLayout();
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(345, 138);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(142, 31);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "开始添加";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // lblDeviceNum
            // 
            lblDeviceNum.AutoSize = true;
            lblDeviceNum.Location = new Point(103, 8);
            lblDeviceNum.Name = "lblDeviceNum";
            lblDeviceNum.Size = new Size(0, 15);
            lblDeviceNum.TabIndex = 3;
            // 
            // btnImportData
            // 
            btnImportData.Location = new Point(345, 175);
            btnImportData.Name = "btnImportData";
            btnImportData.Size = new Size(142, 31);
            btnImportData.TabIndex = 4;
            btnImportData.Text = "导入数据";
            btnImportData.UseVisualStyleBackColor = true;
            btnImportData.Click += btnImportData_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(345, 244);
            label2.Name = "label2";
            label2.Size = new Size(56, 15);
            label2.TabIndex = 5;
            label2.Text = "每次间隔";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(431, 244);
            label3.Name = "label3";
            label3.Size = new Size(19, 15);
            label3.TabIndex = 6;
            label3.Text = "秒";
            // 
            // txtSecd
            // 
            txtSecd.Location = new Point(401, 242);
            txtSecd.Name = "txtSecd";
            txtSecd.Size = new Size(32, 23);
            txtSecd.TabIndex = 7;
            txtSecd.Text = "120";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(21, 311);
            label1.Name = "label1";
            label1.Size = new Size(37, 15);
            label1.TabIndex = 2;
            label1.Text = "日志：";
            // 
            // txtLog
            // 
            txtLog.Location = new Point(12, 336);
            txtLog.Name = "txtLog";
            txtLog.Size = new Size(327, 51);
            txtLog.TabIndex = 1;
            txtLog.Text = "";
            txtLog.TextChanged += txtLog_TextChanged;
            // 
            // btnSaveMoments
            // 
            btnSaveMoments.Location = new Point(345, 271);
            btnSaveMoments.Name = "btnSaveMoments";
            btnSaveMoments.Size = new Size(102, 28);
            btnSaveMoments.TabIndex = 8;
            btnSaveMoments.Text = "复制朋友圈";
            btnSaveMoments.UseVisualStyleBackColor = true;
            btnSaveMoments.Click += btnSaveMoments_Click;
            // 
            // btnSendMoments
            // 
            btnSendMoments.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnSendMoments.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnSendMoments.Depth = 0;
            btnSendMoments.HighEmphasis = true;
            btnSendMoments.Icon = null;
            btnSendMoments.Location = new Point(345, 341);
            btnSendMoments.Margin = new Padding(4, 6, 4, 6);
            btnSendMoments.MouseState = MaterialSkin.MouseState.HOVER;
            btnSendMoments.Name = "btnSendMoments";
            btnSendMoments.NoAccentTextColor = Color.Empty;
            btnSendMoments.Size = new Size(85, 36);
            btnSendMoments.TabIndex = 9;
            btnSendMoments.Text = "发朋友圈";
            btnSendMoments.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnSendMoments.UseAccentColor = false;
            btnSendMoments.UseVisualStyleBackColor = true;
            btnSendMoments.Click += btnSendMoments_Click;
            // 
            // materialButton1
            // 
            materialButton1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            materialButton1.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            materialButton1.Depth = 0;
            materialButton1.HighEmphasis = true;
            materialButton1.Icon = null;
            materialButton1.Location = new Point(356, 81);
            materialButton1.Margin = new Padding(4, 6, 4, 6);
            materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            materialButton1.Name = "materialButton1";
            materialButton1.NoAccentTextColor = Color.Empty;
            materialButton1.Size = new Size(158, 36);
            materialButton1.TabIndex = 10;
            materialButton1.Text = "materialButton1";
            materialButton1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            materialButton1.UseAccentColor = false;
            materialButton1.UseVisualStyleBackColor = true;
            materialButton1.Click += materialButton1_Click;
            // 
            // WeChatAddFriendForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(498, 397);
            Controls.Add(materialButton1);
            Controls.Add(btnSendMoments);
            Controls.Add(btnSaveMoments);
            Controls.Add(txtSecd);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(btnImportData);
            Controls.Add(lblDeviceNum);
            Controls.Add(label1);
            Controls.Add(txtLog);
            Controls.Add(btnAdd);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WeChatAddFriendForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "微信加好友";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnAdd;
        private Label lblDeviceNum;
        private Button btnImportData;
        private Label label2;
        private Label label3;
        private TextBox txtSecd;
        private Label label1;
        private RichTextBox txtLog;
        private Button btnSaveMoments;
        private MaterialSkin.Controls.MaterialButton btnSendMoments;
        private MaterialSkin.Controls.MaterialButton materialButton1;
    }
}