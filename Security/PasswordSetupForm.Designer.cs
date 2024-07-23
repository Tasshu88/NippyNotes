namespace Nippy_Notes
{
    partial class PasswordSetupForm
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
            this.TextBoxConfirmPassword = new System.Windows.Forms.TextBox();
            this.TextBoxPassword = new System.Windows.Forms.TextBox();
            this.BtnSetPassword = new System.Windows.Forms.Button();
            this.LblEnterPassword = new System.Windows.Forms.Label();
            this.LblConfirmPassword = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBoxWord = new System.Windows.Forms.TextBox();
            this.LblEmail = new System.Windows.Forms.Label();
            this.TextBoxEmail = new System.Windows.Forms.TextBox();
            this.BtnCancelPasswordSetup = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextBoxConfirmPassword
            // 
            this.TextBoxConfirmPassword.Location = new System.Drawing.Point(23, 71);
            this.TextBoxConfirmPassword.Name = "TextBoxConfirmPassword";
            this.TextBoxConfirmPassword.Size = new System.Drawing.Size(179, 20);
            this.TextBoxConfirmPassword.TabIndex = 2;
            // 
            // TextBoxPassword
            // 
            this.TextBoxPassword.Location = new System.Drawing.Point(23, 29);
            this.TextBoxPassword.Name = "TextBoxPassword";
            this.TextBoxPassword.Size = new System.Drawing.Size(179, 20);
            this.TextBoxPassword.TabIndex = 1;
            // 
            // BtnSetPassword
            // 
            this.BtnSetPassword.Location = new System.Drawing.Point(36, 196);
            this.BtnSetPassword.Name = "BtnSetPassword";
            this.BtnSetPassword.Size = new System.Drawing.Size(75, 23);
            this.BtnSetPassword.TabIndex = 5;
            this.BtnSetPassword.Text = "Confirm";
            this.BtnSetPassword.UseVisualStyleBackColor = true;
            this.BtnSetPassword.Click += new System.EventHandler(this.BtnSetPassword_Click);
            // 
            // LblEnterPassword
            // 
            this.LblEnterPassword.AutoSize = true;
            this.LblEnterPassword.Location = new System.Drawing.Point(20, 13);
            this.LblEnterPassword.Name = "LblEnterPassword";
            this.LblEnterPassword.Size = new System.Drawing.Size(84, 13);
            this.LblEnterPassword.TabIndex = 4;
            this.LblEnterPassword.Text = "Enter Password:";
            // 
            // LblConfirmPassword
            // 
            this.LblConfirmPassword.AutoSize = true;
            this.LblConfirmPassword.Location = new System.Drawing.Point(20, 55);
            this.LblConfirmPassword.Name = "LblConfirmPassword";
            this.LblConfirmPassword.Size = new System.Drawing.Size(94, 13);
            this.LblConfirmPassword.TabIndex = 5;
            this.LblConfirmPassword.Text = "Confirm Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(211, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Enter a Memorable Word (6-12 characters):";
            // 
            // TextBoxWord
            // 
            this.TextBoxWord.Location = new System.Drawing.Point(36, 170);
            this.TextBoxWord.MaxLength = 12;
            this.TextBoxWord.Name = "TextBoxWord";
            this.TextBoxWord.Size = new System.Drawing.Size(91, 20);
            this.TextBoxWord.TabIndex = 4;
            // 
            // LblEmail
            // 
            this.LblEmail.AutoSize = true;
            this.LblEmail.Location = new System.Drawing.Point(20, 104);
            this.LblEmail.Name = "LblEmail";
            this.LblEmail.Size = new System.Drawing.Size(88, 13);
            this.LblEmail.TabIndex = 9;
            this.LblEmail.Text = "Enter Your Email:";
            // 
            // TextBoxEmail
            // 
            this.TextBoxEmail.Location = new System.Drawing.Point(23, 120);
            this.TextBoxEmail.Name = "TextBoxEmail";
            this.TextBoxEmail.Size = new System.Drawing.Size(179, 20);
            this.TextBoxEmail.TabIndex = 3;
            // 
            // BtnCancelPasswordSetup
            // 
            this.BtnCancelPasswordSetup.Location = new System.Drawing.Point(127, 196);
            this.BtnCancelPasswordSetup.Name = "BtnCancelPasswordSetup";
            this.BtnCancelPasswordSetup.Size = new System.Drawing.Size(75, 23);
            this.BtnCancelPasswordSetup.TabIndex = 6;
            this.BtnCancelPasswordSetup.Text = "Cancel";
            this.BtnCancelPasswordSetup.UseVisualStyleBackColor = true;
            this.BtnCancelPasswordSetup.Click += new System.EventHandler(this.BtnCancelPasswordSetup_Click);
            // 
            // PasswordSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(256, 231);
            this.Controls.Add(this.BtnCancelPasswordSetup);
            this.Controls.Add(this.LblEmail);
            this.Controls.Add(this.TextBoxEmail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBoxWord);
            this.Controls.Add(this.LblConfirmPassword);
            this.Controls.Add(this.LblEnterPassword);
            this.Controls.Add(this.BtnSetPassword);
            this.Controls.Add(this.TextBoxPassword);
            this.Controls.Add(this.TextBoxConfirmPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PasswordSetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PasswordSetupForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxConfirmPassword;
        private System.Windows.Forms.TextBox TextBoxPassword;
        private System.Windows.Forms.Button BtnSetPassword;
        private System.Windows.Forms.Label LblEnterPassword;
        private System.Windows.Forms.Label LblConfirmPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxWord;
        private System.Windows.Forms.Label LblEmail;
        private System.Windows.Forms.TextBox TextBoxEmail;
        private System.Windows.Forms.Button BtnCancelPasswordSetup;
    }
}