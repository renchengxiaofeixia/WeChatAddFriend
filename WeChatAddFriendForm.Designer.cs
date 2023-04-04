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
            lblDeviceNum = new Label();
            btnImportData = new Button();
            label2 = new Label();
            label3 = new Label();
            txtSecd = new TextBox();
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
            ((System.ComponentModel.ISupportInitialize)dgPhones).BeginInit();
            ctxMenu.SuspendLayout();
            SuspendLayout();
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(345, 26);
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
            btnImportData.Location = new Point(345, 63);
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
            label2.Location = new Point(345, 97);
            label2.Name = "label2";
            label2.Size = new Size(56, 15);
            label2.TabIndex = 5;
            label2.Text = "每次间隔";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(431, 97);
            label3.Name = "label3";
            label3.Size = new Size(19, 15);
            label3.TabIndex = 6;
            label3.Text = "秒";
            // 
            // txtSecd
            // 
            txtSecd.Location = new Point(401, 95);
            txtSecd.Name = "txtSecd";
            txtSecd.Size = new Size(32, 23);
            txtSecd.TabIndex = 7;
            txtSecd.Text = "120";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 8);
            label1.Name = "label1";
            label1.Size = new Size(37, 15);
            label1.TabIndex = 2;
            label1.Text = "日志：";
            // 
            // txtLog
            // 
            txtLog.Location = new Point(12, 26);
            txtLog.Name = "txtLog";
            txtLog.Size = new Size(327, 286);
            txtLog.TabIndex = 1;
            txtLog.Text = "";
            txtLog.TextChanged += txtLog_TextChanged;
            // 
            // btnForwardMoments
            // 
            btnForwardMoments.Location = new Point(391, 153);
            btnForwardMoments.Name = "btnForwardMoments";
            btnForwardMoments.Size = new Size(102, 28);
            btnForwardMoments.TabIndex = 10;
            btnForwardMoments.Text = "转发朋友圈";
            btnForwardMoments.UseVisualStyleBackColor = true;
            btnForwardMoments.Click += btnForwardMoments_Click;
            // 
            // txtWeChatNo
            // 
            txtWeChatNo.Location = new Point(391, 124);
            txtWeChatNo.Name = "txtWeChatNo";
            txtWeChatNo.Size = new Size(155, 23);
            txtWeChatNo.TabIndex = 11;
            txtWeChatNo.Text = "wangyaqin1991";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(343, 127);
            label4.Name = "label4";
            label4.Size = new Size(43, 15);
            label4.TabIndex = 12;
            label4.Text = "微信号";
            // 
            // dgPhones
            // 
            dgPhones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgPhones.Columns.AddRange(new DataGridViewColumn[] { phoneSerial, phoneName, phoneState });
            dgPhones.ContextMenuStrip = ctxMenu;
            dgPhones.Location = new Point(345, 187);
            dgPhones.Name = "dgPhones";
            dgPhones.RowTemplate.Height = 25;
            dgPhones.Size = new Size(348, 209);
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
            ctxMenu.Size = new Size(181, 70);
            // 
            // 转为WIFI连接ToolStripMenuItem
            // 
            转为WIFI连接ToolStripMenuItem.Name = "转为WIFI连接ToolStripMenuItem";
            转为WIFI连接ToolStripMenuItem.Size = new Size(180, 22);
            转为WIFI连接ToolStripMenuItem.Text = "转为WIFI连接";
            转为WIFI连接ToolStripMenuItem.Click += 转为WIFI连接ToolStripMenuItem_Click;
            // 
            // 断开连接ToolStripMenuItem
            // 
            断开连接ToolStripMenuItem.Name = "断开连接ToolStripMenuItem";
            断开连接ToolStripMenuItem.Size = new Size(180, 22);
            断开连接ToolStripMenuItem.Text = "断开WIFI连接";
            断开连接ToolStripMenuItem.Click += 断开连接ToolStripMenuItem_Click;
            // 
            // WeChatAddFriendForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(770, 465);
            Controls.Add(dgPhones);
            Controls.Add(label4);
            Controls.Add(txtWeChatNo);
            Controls.Add(btnForwardMoments);
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
            ((System.ComponentModel.ISupportInitialize)dgPhones).EndInit();
            ctxMenu.ResumeLayout(false);
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
    }
}