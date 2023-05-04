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
            PhoneId = new DataGridViewTextBoxColumn();
            phoneSerial = new DataGridViewTextBoxColumn();
            phoneName = new DataGridViewTextBoxColumn();
            phoneState = new DataGridViewTextBoxColumn();
            ctxMenu = new ContextMenuStrip(components);
            转为WIFI连接ToolStripMenuItem = new ToolStripMenuItem();
            断开连接ToolStripMenuItem = new ToolStripMenuItem();
            转发朋友圈ToolStripMenuItem1 = new ToolStripMenuItem();
            微信加好友ToolStripMenuItem1 = new ToolStripMenuItem();
            txtHour = new TextBox();
            label5 = new Label();
            label6 = new Label();
            lblDeviceNum = new Label();
            btnStopForwardMoments = new Button();
            btnStopAddFriend = new Button();
            txtAddCount = new TextBox();
            label7 = new Label();
            label8 = new Label();
            btnOpenTasks = new Button();
            label9 = new Label();
            txtMorningTime = new TextBox();
            label10 = new Label();
            txtAfterTime = new TextBox();
            label11 = new Label();
            txtDataFrom = new TextBox();
            label12 = new Label();
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
            btnImportData.Location = new Point(118, 10);
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
            label3.Location = new Point(122, 53);
            label3.Name = "label3";
            label3.Size = new Size(32, 15);
            label3.TabIndex = 6;
            label3.Text = "分钟";
            // 
            // txtMint
            // 
            txtMint.Location = new Point(97, 49);
            txtMint.Name = "txtMint";
            txtMint.Size = new Size(23, 23);
            txtMint.TabIndex = 7;
            txtMint.Text = "10";
            txtMint.TextChanged += txtMint_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(576, 11);
            label1.Name = "label1";
            label1.Size = new Size(37, 15);
            label1.TabIndex = 2;
            label1.Text = "日志：";
            // 
            // txtLog
            // 
            txtLog.BorderStyle = BorderStyle.FixedSingle;
            txtLog.Location = new Point(576, 29);
            txtLog.Name = "txtLog";
            txtLog.Size = new Size(302, 361);
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
            dgPhones.Columns.AddRange(new DataGridViewColumn[] { PhoneId, phoneSerial, phoneName, phoneState });
            dgPhones.ContextMenuStrip = ctxMenu;
            dgPhones.Location = new Point(12, 106);
            dgPhones.Name = "dgPhones";
            dgPhones.RowTemplate.Height = 25;
            dgPhones.Size = new Size(558, 284);
            dgPhones.TabIndex = 13;
            // 
            // PhoneId
            // 
            PhoneId.DataPropertyName = "PhoneId";
            PhoneId.HeaderText = "PhoneId";
            PhoneId.Name = "PhoneId";
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
            ctxMenu.Items.AddRange(new ToolStripItem[] { 转为WIFI连接ToolStripMenuItem, 断开连接ToolStripMenuItem, 转发朋友圈ToolStripMenuItem1, 微信加好友ToolStripMenuItem1 });
            ctxMenu.Name = "ctxMenu";
            ctxMenu.Size = new Size(149, 92);
            // 
            // 转为WIFI连接ToolStripMenuItem
            // 
            转为WIFI连接ToolStripMenuItem.Name = "转为WIFI连接ToolStripMenuItem";
            转为WIFI连接ToolStripMenuItem.Size = new Size(148, 22);
            转为WIFI连接ToolStripMenuItem.Text = "转为WIFI连接";
            转为WIFI连接ToolStripMenuItem.Visible = false;
            转为WIFI连接ToolStripMenuItem.Click += 转为WIFI连接ToolStripMenuItem_Click;
            // 
            // 断开连接ToolStripMenuItem
            // 
            断开连接ToolStripMenuItem.Name = "断开连接ToolStripMenuItem";
            断开连接ToolStripMenuItem.Size = new Size(148, 22);
            断开连接ToolStripMenuItem.Text = "断开WIFI连接";
            断开连接ToolStripMenuItem.Visible = false;
            断开连接ToolStripMenuItem.Click += 断开连接ToolStripMenuItem_Click;
            // 
            // 转发朋友圈ToolStripMenuItem1
            // 
            转发朋友圈ToolStripMenuItem1.Name = "转发朋友圈ToolStripMenuItem1";
            转发朋友圈ToolStripMenuItem1.Size = new Size(148, 22);
            转发朋友圈ToolStripMenuItem1.Text = "转发朋友圈";
            转发朋友圈ToolStripMenuItem1.Click += 转发朋友圈ToolStripMenuItem1_Click;
            // 
            // 微信加好友ToolStripMenuItem1
            // 
            微信加好友ToolStripMenuItem1.Name = "微信加好友ToolStripMenuItem1";
            微信加好友ToolStripMenuItem1.Size = new Size(148, 22);
            微信加好友ToolStripMenuItem1.Text = "微信加好友";
            微信加好友ToolStripMenuItem1.Click += 微信加好友ToolStripMenuItem1_Click;
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
            // btnStopForwardMoments
            // 
            btnStopForwardMoments.Location = new Point(436, 40);
            btnStopForwardMoments.Name = "btnStopForwardMoments";
            btnStopForwardMoments.Size = new Size(102, 28);
            btnStopForwardMoments.TabIndex = 17;
            btnStopForwardMoments.Text = "停止发圈";
            btnStopForwardMoments.UseVisualStyleBackColor = true;
            btnStopForwardMoments.Visible = false;
            btnStopForwardMoments.VisibleChanged += btnStopForwardMoments_VisibleChanged;
            btnStopForwardMoments.Click += btnStopForwardMoments_Click;
            // 
            // btnStopAddFriend
            // 
            btnStopAddFriend.Location = new Point(12, 10);
            btnStopAddFriend.Name = "btnStopAddFriend";
            btnStopAddFriend.Size = new Size(100, 31);
            btnStopAddFriend.TabIndex = 18;
            btnStopAddFriend.Text = "停止加好友";
            btnStopAddFriend.UseVisualStyleBackColor = true;
            btnStopAddFriend.Visible = false;
            btnStopAddFriend.VisibleChanged += btnStopAddFriend_VisibleChanged;
            btnStopAddFriend.Click += btnStopAddFriend_Click;
            // 
            // txtAddCount
            // 
            txtAddCount.Location = new Point(97, 76);
            txtAddCount.Name = "txtAddCount";
            txtAddCount.Size = new Size(23, 23);
            txtAddCount.TabIndex = 21;
            txtAddCount.Text = "15";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(122, 80);
            label7.Name = "label7";
            label7.Size = new Size(19, 15);
            label7.TabIndex = 20;
            label7.Text = "个";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(12, 79);
            label8.Name = "label8";
            label8.Size = new Size(80, 15);
            label8.TabIndex = 19;
            label8.Text = "每次添加号码";
            // 
            // btnOpenTasks
            // 
            btnOpenTasks.Location = new Point(436, 71);
            btnOpenTasks.Name = "btnOpenTasks";
            btnOpenTasks.Size = new Size(100, 31);
            btnOpenTasks.TabIndex = 22;
            btnOpenTasks.Text = "打开任务管理";
            btnOpenTasks.UseVisualStyleBackColor = true;
            btnOpenTasks.Click += btnOpenTasks_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.ForeColor = Color.Red;
            label9.Location = new Point(156, 80);
            label9.Name = "label9";
            label9.Size = new Size(31, 15);
            label9.TabIndex = 23;
            label9.Text = "上午";
            // 
            // txtMorningTime
            // 
            txtMorningTime.ForeColor = Color.Red;
            txtMorningTime.Location = new Point(189, 76);
            txtMorningTime.MaxLength = 2;
            txtMorningTime.Name = "txtMorningTime";
            txtMorningTime.Size = new Size(23, 23);
            txtMorningTime.TabIndex = 24;
            txtMorningTime.Text = "10";
            txtMorningTime.TextChanged += txtMorningTime_TextChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.ForeColor = Color.Red;
            label10.Location = new Point(305, 80);
            label10.Name = "label10";
            label10.Size = new Size(91, 15);
            label10.TabIndex = 25;
            label10.Text = "点以后开始加人";
            // 
            // txtAfterTime
            // 
            txtAfterTime.ForeColor = Color.Red;
            txtAfterTime.Location = new Point(278, 76);
            txtAfterTime.MaxLength = 2;
            txtAfterTime.Name = "txtAfterTime";
            txtAfterTime.Size = new Size(23, 23);
            txtAfterTime.TabIndex = 27;
            txtAfterTime.Text = "15";
            txtAfterTime.TextChanged += txtAfterTime_TextChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.ForeColor = Color.Red;
            label11.Location = new Point(214, 80);
            label11.Name = "label11";
            label11.Size = new Size(61, 15);
            label11.TabIndex = 28;
            label11.Text = "点 和 下午";
            // 
            // txtDataFrom
            // 
            txtDataFrom.Location = new Point(196, 47);
            txtDataFrom.Name = "txtDataFrom";
            txtDataFrom.Size = new Size(57, 23);
            txtDataFrom.TabIndex = 29;
            txtDataFrom.Text = "三维家";
            txtDataFrom.Visible = false;
            txtDataFrom.TextChanged += txtDataFrom_TextChanged;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.ForeColor = Color.MediumBlue;
            label12.Location = new Point(160, 52);
            label12.Name = "label12";
            label12.Size = new Size(32, 15);
            label12.TabIndex = 30;
            label12.Text = "分类";
            label12.Visible = false;
            // 
            // WeChatAddFriendForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(890, 412);
            Controls.Add(label12);
            Controls.Add(txtDataFrom);
            Controls.Add(label11);
            Controls.Add(txtAfterTime);
            Controls.Add(label10);
            Controls.Add(txtMorningTime);
            Controls.Add(label9);
            Controls.Add(btnOpenTasks);
            Controls.Add(txtAddCount);
            Controls.Add(label7);
            Controls.Add(label8);
            Controls.Add(btnAdd);
            Controls.Add(btnStopAddFriend);
            Controls.Add(btnForwardMoments);
            Controls.Add(btnStopForwardMoments);
            Controls.Add(txtHour);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(dgPhones);
            Controls.Add(label4);
            Controls.Add(txtWeChatNo);
            Controls.Add(txtMint);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(btnImportData);
            Controls.Add(lblDeviceNum);
            Controls.Add(label1);
            Controls.Add(txtLog);
            DoubleBuffered = true;
            MaximizeBox = false;
            Name = "WeChatAddFriendForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "微信加好友";
            FormClosing += WeChatAddFriendForm_FormClosing;
            FormClosed += WeChatAddFriendForm_FormClosed;
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
        private ToolStripMenuItem 转为WIFI连接ToolStripMenuItem;
        private ToolStripMenuItem 断开连接ToolStripMenuItem;
        private TextBox txtHour;
        private Label label5;
        private Label label6;
        private Label lblDeviceNum;
        private Button btnStopForwardMoments;
        private Button btnStopAddFriend;
        private TextBox txtAddCount;
        private Label label7;
        private Label label8;
        private Button btnOpenTasks;
        private Label label9;
        private TextBox txtMorningTime;
        private Label label10;
        private TextBox txtAfterTime;
        private Label label11;
        private DataGridViewTextBoxColumn PhoneId;
        private DataGridViewTextBoxColumn phoneSerial;
        private DataGridViewTextBoxColumn phoneName;
        private DataGridViewTextBoxColumn phoneState;
        private ToolStripMenuItem 转发朋友圈ToolStripMenuItem1;
        private ToolStripMenuItem 微信加好友ToolStripMenuItem1;
        private TextBox txtDataFrom;
        private Label label12;
    }
}