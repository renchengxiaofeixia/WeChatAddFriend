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
            components = new System.ComponentModel.Container();
            btnAdd = new Button();
            btnImportData = new Button();
            label2 = new Label();
            label3 = new Label();
            txtMint = new TextBox();
            label1 = new Label();
            txtLog = new RichTextBox();
            btnForwardMoments = new Button();
            txtWeChatNo = new TextBox();
            label4 = new Label();
            dgPhones = new DataGridView();
            phoneSerial = new DataGridViewTextBoxColumn();
            phoneName = new DataGridViewTextBoxColumn();
            phoneState = new DataGridViewTextBoxColumn();
            ctxMenu = new ContextMenuStrip(components);
            转为WIFI连接ToolStripMenuItem = new ToolStripMenuItem();
            断开连接ToolStripMenuItem = new ToolStripMenuItem();
            txtHour = new TextBox();
            label5 = new Label();
            label6 = new Label();
            lblDeviceNum = new Label();
            ((System.ComponentModel.ISupportInitialize)dgPhones).BeginInit();
            ctxMenu.SuspendLayout();
            SuspendLayout();
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(12, 10);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 31);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "开始加好友";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnImportData
            // 
            btnImportData.Location = new Point(123, 10);
            btnImportData.Name = "btnImportData";
            btnImportData.Size = new Size(100, 31);
            btnImportData.TabIndex = 4;
            btnImportData.Text = "导入数据";
            btnImportData.UseVisualStyleBackColor = true;
            btnImportData.Click += btnImportData_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 52);
            label2.Name = "label2";
            label2.Size = new Size(80, 15);
            label2.TabIndex = 5;
            label2.Text = "每次加人间隔";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(129, 53);
            label3.Name = "label3";
            label3.Size = new Size(32, 15);
            label3.TabIndex = 6;
            label3.Text = "分钟";
            // 
            // txtMint
            // 
            txtMint.Location = new Point(98, 49);
            txtMint.Name = "txtMint";
            txtMint.Size = new Size(23, 23);
            txtMint.TabIndex = 7;
            txtMint.Text = "10";
            txtMint.TextChanged += txtMint_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(584, 11);
            label1.Name = "label1";
            label1.Size = new Size(37, 15);
            label1.TabIndex = 2;
            label1.Text = "日志：";
            // 
            // txtLog
            // 
            txtLog.BorderStyle = BorderStyle.FixedSingle;
            txtLog.Location = new Point(584, 29);
            txtLog.Name = "txtLog";
            txtLog.Size = new Size(484, 361);
            txtLog.TabIndex = 1;
            txtLog.Text = "";
            txtLog.TextChanged += txtLog_TextChanged;
            // 
            // btnForwardMoments
            // 
            btnForwardMoments.Location = new Point(436, 39);
            btnForwardMoments.Name = "btnForwardMoments";
            btnForwardMoments.Size = new Size(102, 28);
            btnForwardMoments.TabIndex = 10;
            btnForwardMoments.Text = "开始发圈";
            btnForwardMoments.UseVisualStyleBackColor = true;
            btnForwardMoments.Click += btnForwardMoments_Click;
            // 
            // txtWeChatNo
            // 
            txtWeChatNo.Location = new Point(383, 10);
            txtWeChatNo.Name = "txtWeChatNo";
            txtWeChatNo.Size = new Size(155, 23);
            txtWeChatNo.TabIndex = 11;
            txtWeChatNo.Text = "wangyaqin1991";
            txtWeChatNo.TextChanged += txtWeChatNo_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(260, 13);
            label4.Name = "label4";
            label4.Size = new Size(117, 15);
            label4.TabIndex = 12;
            label4.Text = "转发朋友圈的微信号";
            // 
            // dgPhones
            // 
            dgPhones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgPhones.Columns.AddRange(new DataGridViewColumn[] { phoneSerial, phoneName, phoneState });
            dgPhones.Location = new Point(12, 86);
            dgPhones.Name = "dgPhones";
            dgPhones.RowTemplate.Height = 25;
            dgPhones.Size = new Size(554, 304);
            dgPhones.TabIndex = 13;
            // 
            // phoneSerial
            // 
            phoneSerial.DataPropertyName = "Serial";
            phoneSerial.HeaderText = "Serial";
            phoneSerial.Name = "phoneSerial";
            phoneSerial.ReadOnly = true;
            // 
            // phoneName
            // 
            phoneName.DataPropertyName = "Name";
            phoneName.HeaderText = "Name";
            phoneName.Name = "phoneName";
            phoneName.ReadOnly = true;
            // 
            // phoneState
            // 
            phoneState.DataPropertyName = "State";
            phoneState.HeaderText = "State";
            phoneState.Name = "phoneState";
            phoneState.ReadOnly = true;
            // 
            // ctxMenu
            // 
            ctxMenu.Items.AddRange(new ToolStripItem[] { 转为WIFI连接ToolStripMenuItem, 断开连接ToolStripMenuItem });
            ctxMenu.Name = "ctxMenu";
            ctxMenu.Size = new Size(149, 48);
            // 
            // 转为WIFI连接ToolStripMenuItem
            // 
            转为WIFI连接ToolStripMenuItem.Name = "转为WIFI连接ToolStripMenuItem";
            转为WIFI连接ToolStripMenuItem.Size = new Size(148, 22);
            转为WIFI连接ToolStripMenuItem.Text = "转为WIFI连接";
            转为WIFI连接ToolStripMenuItem.Click += 转为WIFI连接ToolStripMenuItem_Click;
            // 
            // 断开连接ToolStripMenuItem
            // 
            断开连接ToolStripMenuItem.Name = "断开连接ToolStripMenuItem";
            断开连接ToolStripMenuItem.Size = new Size(148, 22);
            断开连接ToolStripMenuItem.Text = "断开WIFI连接";
            断开连接ToolStripMenuItem.Click += 断开连接ToolStripMenuItem_Click;
            // 
            // txtHour
            // 
            txtHour.Location = new Point(345, 44);
            txtHour.Name = "txtHour";
            txtHour.Size = new Size(23, 23);
            txtHour.TabIndex = 16;
            txtHour.Text = "2";
            txtHour.TextChanged += txtHour_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(371, 47);
            label5.Name = "label5";
            label5.Size = new Size(32, 15);
            label5.TabIndex = 15;
            label5.Text = "小时";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(259, 47);
            label6.Name = "label6";
            label6.Size = new Size(81, 15);
            label6.TabIndex = 14;
            label6.Text = "每次发圈间隔";
            // 
            // lblDeviceNum
            // 
            lblDeviceNum.AutoSize = true;
            lblDeviceNum.Location = new Point(675, 11);
            lblDeviceNum.Name = "lblDeviceNum";
            lblDeviceNum.Size = new Size(0, 15);
            lblDeviceNum.TabIndex = 3;
            // 
            // WeChatAddFriendForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1073, 412);
            Controls.Add(txtHour);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(dgPhones);
            Controls.Add(label4);
            Controls.Add(txtWeChatNo);
            Controls.Add(btnForwardMoments);
            Controls.Add(txtMint);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(btnImportData);
            Controls.Add(lblDeviceNum);
            Controls.Add(label1);
            Controls.Add(txtLog);
            Controls.Add(btnAdd);
            DoubleBuffered = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WeChatAddFriendForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "微信加好友";
            ((System.ComponentModel.ISupportInitialize)dgPhones).EndInit();
            ctxMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnAdd;
        private Button btnImportData;
        private Label label2;
        private Label label3;
        private TextBox txtMint;
        private Label label1;
        private RichTextBox txtLog;
        private Button btnForwardMoments;
        private TextBox txtWeChatNo;
        private Label label4;
        private DataGridView dgPhones;
        private ContextMenuStrip ctxMenu;
        private DataGridViewTextBoxColumn phoneSerial;
        private DataGridViewTextBoxColumn phoneName;
        private DataGridViewTextBoxColumn phoneState;
        private ToolStripMenuItem 转为WIFI连接ToolStripMenuItem;
        private ToolStripMenuItem 断开连接ToolStripMenuItem;
        private TextBox txtHour;
        private Label label5;
        private Label label6;
        private Label lblDeviceNum;
    }
}