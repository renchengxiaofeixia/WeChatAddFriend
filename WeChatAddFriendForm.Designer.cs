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
            this.btnAdd = new System.Windows.Forms.Button();
            this.tbPhoneNos = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDeviceNum = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(345, 29);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(142, 35);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "开始添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // tbPhoneNos
            // 
            this.tbPhoneNos.Location = new System.Drawing.Point(12, 29);
            this.tbPhoneNos.Name = "tbPhoneNos";
            this.tbPhoneNos.Size = new System.Drawing.Size(327, 409);
            this.tbPhoneNos.TabIndex = 1;
            this.tbPhoneNos.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "手机号/微信号";
            // 
            // lblDeviceNum
            // 
            this.lblDeviceNum.AutoSize = true;
            this.lblDeviceNum.Location = new System.Drawing.Point(103, 9);
            this.lblDeviceNum.Name = "lblDeviceNum";
            this.lblDeviceNum.Size = new System.Drawing.Size(0, 17);
            this.lblDeviceNum.TabIndex = 3;
            // 
            // WeChatAddFriendForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 450);
            this.Controls.Add(this.lblDeviceNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPhoneNos);
            this.Controls.Add(this.btnAdd);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WeChatAddFriendForm";
            this.Text = "微信添加好友";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnAdd;
        private RichTextBox tbPhoneNos;
        private Label label1;
        private Label lblDeviceNum;
    }
}