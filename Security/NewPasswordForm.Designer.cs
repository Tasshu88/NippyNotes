namespace Nippy_Notes.Security
{
    partial class NewPasswordForm
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
            this.LblConfirmPassword = new System.Windows.Forms.Label();
            this.LblEnterPassword = new System.Windows.Forms.Label();
            this.TextBoxTempPassword = new System.Windows.Forms.TextBox();
            this.TextBoxNewPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBoxConfirmNewPassword = new System.Windows.Forms.TextBox();
            this.BtnSendEmail = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LblConfirmPassword
            // 
            this.LblConfirmPassword.AutoSize = true;
            this.LblConfirmPassword.Location = new System.Drawing.Point(16, 59);
            this.LblConfirmPassword.Name = "LblConfirmPassword";
            this.LblConfirmPassword.Size = new System.Drawing.Size(109, 13);
            this.LblConfirmPassword.TabIndex = 9;
            this.LblConfirmPassword.Text = "Enter New Password:";
            // 
            // LblEnterPassword
            // 
            this.LblEnterPassword.AutoSize = true;
            this.LblEnterPassword.Location = new System.Drawing.Point(16, 17);
            this.LblEnterPassword.Name = "LblEnterPassword";
            this.LblEnterPassword.Size = new System.Drawing.Size(114, 13);
            this.LblEnterPassword.TabIndex = 8;
            this.LblEnterPassword.Text = "Enter Temp Password:";
            // 
            // TextBoxTempPassword
            // 
            this.TextBoxTempPassword.Location = new System.Drawing.Point(19, 33);
            this.TextBoxTempPassword.Name = "TextBoxTempPassword";
            this.TextBoxTempPassword.Size = new System.Drawing.Size(179, 20);
            this.TextBoxTempPassword.TabIndex = 6;
            // 
            // TextBoxNewPassword
            // 
            this.TextBoxNewPassword.Location = new System.Drawing.Point(19, 75);
            this.TextBoxNewPassword.Name = "TextBoxNewPassword";
            this.TextBoxNewPassword.Size = new System.Drawing.Size(179, 20);
            this.TextBoxNewPassword.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Confirm New Password:";
            // 
            // TextBoxConfirmNewPassword
            // 
            this.TextBoxConfirmNewPassword.Location = new System.Drawing.Point(19, 114);
            this.TextBoxConfirmNewPassword.Name = "TextBoxConfirmNewPassword";
            this.TextBoxConfirmNewPassword.Size = new System.Drawing.Size(179, 20);
            this.TextBoxConfirmNewPassword.TabIndex = 10;
            // 
            // BtnSendEmail
            // 
            this.BtnSendEmail.Location = new System.Drawing.Point(70, 140);
            this.BtnSendEmail.Name = "BtnSendEmail";
            this.BtnSendEmail.Size = new System.Drawing.Size(75, 23);
            this.BtnSendEmail.TabIndex = 12;
            this.BtnSendEmail.Text = "Confirm";
            this.BtnSendEmail.UseVisualStyleBackColor = true;
            this.BtnSendEmail.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // NewPasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(215, 181);
            this.Controls.Add(this.BtnSendEmail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBoxConfirmNewPassword);
            this.Controls.Add(this.LblConfirmPassword);
            this.Controls.Add(this.LblEnterPassword);
            this.Controls.Add(this.TextBoxTempPassword);
            this.Controls.Add(this.TextBoxNewPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NewPasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NewPasswordForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblConfirmPassword;
        private System.Windows.Forms.Label LblEnterPassword;
        private System.Windows.Forms.TextBox TextBoxTempPassword;
        private System.Windows.Forms.TextBox TextBoxNewPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxConfirmNewPassword;
        private System.Windows.Forms.Button BtnSendEmail;
    }
}