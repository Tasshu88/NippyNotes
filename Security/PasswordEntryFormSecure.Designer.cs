namespace Nippy_Notes.Security
{
    partial class PasswordEntryFormSecure
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
            this.LblEnterPassword = new System.Windows.Forms.Label();
            this.TextBoxPassword = new System.Windows.Forms.TextBox();
            this.BtnSubmitPassword = new System.Windows.Forms.Button();
            this.CheckBoxShowPassword = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // LblEnterPassword
            // 
            this.LblEnterPassword.AutoSize = true;
            this.LblEnterPassword.Location = new System.Drawing.Point(25, 15);
            this.LblEnterPassword.Name = "LblEnterPassword";
            this.LblEnterPassword.Size = new System.Drawing.Size(84, 13);
            this.LblEnterPassword.TabIndex = 5;
            this.LblEnterPassword.Text = "Enter Password:";
            // 
            // TextBoxPassword
            // 
            this.TextBoxPassword.Location = new System.Drawing.Point(25, 34);
            this.TextBoxPassword.Name = "TextBoxPassword";
            this.TextBoxPassword.Size = new System.Drawing.Size(179, 20);
            this.TextBoxPassword.TabIndex = 4;
            // 
            // BtnSubmitPassword
            // 
            this.BtnSubmitPassword.Location = new System.Drawing.Point(67, 80);
            this.BtnSubmitPassword.Name = "BtnSubmitPassword";
            this.BtnSubmitPassword.Size = new System.Drawing.Size(75, 23);
            this.BtnSubmitPassword.TabIndex = 6;
            this.BtnSubmitPassword.Text = "Submit";
            this.BtnSubmitPassword.UseVisualStyleBackColor = true;
            this.BtnSubmitPassword.Click += new System.EventHandler(this.BtnSubmitPassword_Click);
            // 
            // CheckBoxShowPassword
            // 
            this.CheckBoxShowPassword.AutoSize = true;
            this.CheckBoxShowPassword.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBoxShowPassword.Location = new System.Drawing.Point(102, 57);
            this.CheckBoxShowPassword.Name = "CheckBoxShowPassword";
            this.CheckBoxShowPassword.Size = new System.Drawing.Size(102, 17);
            this.CheckBoxShowPassword.TabIndex = 11;
            this.CheckBoxShowPassword.Text = "Show Password";
            this.CheckBoxShowPassword.UseVisualStyleBackColor = true;
            this.CheckBoxShowPassword.CheckedChanged += new System.EventHandler(this.CheckBoxShowPassword_CheckedChanged);
            // 
            // PasswordEntryFormSecure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 113);
            this.Controls.Add(this.CheckBoxShowPassword);
            this.Controls.Add(this.BtnSubmitPassword);
            this.Controls.Add(this.LblEnterPassword);
            this.Controls.Add(this.TextBoxPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PasswordEntryFormSecure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PasswordEntryFormSecure";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblEnterPassword;
        private System.Windows.Forms.TextBox TextBoxPassword;
        private System.Windows.Forms.Button BtnSubmitPassword;
        private System.Windows.Forms.CheckBox CheckBoxShowPassword;
    }
}