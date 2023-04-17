namespace Updator
{
    partial class Form1
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
            label1 = new Label();
            txtFile = new TextBox();
            txtNewVer = new TextBox();
            label2 = new Label();
            txtTip = new TextBox();
            label3 = new Label();
            btnFile = new Button();
            btnFileUpload = new Button();
            chkForceUpdate = new CheckBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 21);
            label1.Name = "label1";
            label1.Size = new Size(31, 15);
            label1.TabIndex = 0;
            label1.Text = "文件";
            // 
            // txtFile
            // 
            txtFile.Location = new Point(71, 18);
            txtFile.Name = "txtFile";
            txtFile.Size = new Size(245, 23);
            txtFile.TabIndex = 1;
            // 
            // txtNewVer
            // 
            txtNewVer.Location = new Point(71, 47);
            txtNewVer.Name = "txtNewVer";
            txtNewVer.Size = new Size(245, 23);
            txtNewVer.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 50);
            label2.Name = "label2";
            label2.Size = new Size(31, 15);
            label2.TabIndex = 2;
            label2.Text = "版本";
            // 
            // txtTip
            // 
            txtTip.Location = new Point(71, 76);
            txtTip.Multiline = true;
            txtTip.Name = "txtTip";
            txtTip.Size = new Size(245, 50);
            txtTip.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(27, 79);
            label3.Name = "label3";
            label3.Size = new Size(31, 15);
            label3.TabIndex = 4;
            label3.Text = "提示";
            // 
            // btnFile
            // 
            btnFile.Location = new Point(324, 18);
            btnFile.Name = "btnFile";
            btnFile.Size = new Size(75, 23);
            btnFile.TabIndex = 6;
            btnFile.Text = "浏览";
            btnFile.UseVisualStyleBackColor = true;
            btnFile.Click += btnFile_Click;
            // 
            // btnFileUpload
            // 
            btnFileUpload.Location = new Point(324, 47);
            btnFileUpload.Name = "btnFileUpload";
            btnFileUpload.Size = new Size(75, 23);
            btnFileUpload.TabIndex = 7;
            btnFileUpload.Text = "上传";
            btnFileUpload.UseVisualStyleBackColor = true;
            btnFileUpload.Click += btnFileUpload_Click;
            // 
            // chkForceUpdate
            // 
            chkForceUpdate.AutoSize = true;
            chkForceUpdate.Location = new Point(326, 79);
            chkForceUpdate.Name = "chkForceUpdate";
            chkForceUpdate.Size = new Size(75, 19);
            chkForceUpdate.TabIndex = 8;
            chkForceUpdate.Text = "强制更新";
            chkForceUpdate.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(413, 148);
            Controls.Add(chkForceUpdate);
            Controls.Add(btnFileUpload);
            Controls.Add(btnFile);
            Controls.Add(txtTip);
            Controls.Add(label3);
            Controls.Add(txtNewVer);
            Controls.Add(label2);
            Controls.Add(txtFile);
            Controls.Add(label1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtFile;
        private TextBox txtNewVer;
        private Label label2;
        private TextBox txtTip;
        private Label label3;
        private Button btnFile;
        private Button btnFileUpload;
        private CheckBox chkForceUpdate;
    }
}