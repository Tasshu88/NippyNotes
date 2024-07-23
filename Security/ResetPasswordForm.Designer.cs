namespace Nippy_Notes.Security
{
    partial class ResetPasswordForm
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
            this.LblEmail = new System.Windows.Forms.Label();
            this.TextBoxEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBoxMemorableWord = new System.Windows.Forms.TextBox();
            this.BtnSendEmail = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LblEmail
            // 
            this.LblEmail.AutoSize = true;
            this.LblEmail.Location = new System.Drawing.Point(34, 36);
            this.LblEmail.Name = "LblEmail";
            this.LblEmail.Size = new System.Drawing.Size(88, 13);
            this.LblEmail.TabIndex = 14;
            this.LblEmail.Text = "Enter Your Email:";
            // 
            // TextBoxEmail
            // 
            this.TextBoxEmail.Location = new System.Drawing.Point(37, 52);
            this.TextBoxEmail.Name = "TextBoxEmail";
            this.TextBoxEmail.Size = new System.Drawing.Size(179, 20);
            this.TextBoxEmail.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(211, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Enter a Memorable Word (6-12 characters):";
            // 
            // TextBoxMemorableWord
            // 
            this.TextBoxMemorableWord.Location = new System.Drawing.Point(37, 102);
            this.TextBoxMemorableWord.MaxLength = 12;
            this.TextBoxMemorableWord.Name = "TextBoxMemorableWord";
            this.TextBoxMemorableWord.Size = new System.Drawing.Size(91, 20);
            this.TextBoxMemorableWord.TabIndex = 10;
            // 
            // BtnSendEmail
            // 
            this.BtnSendEmail.Location = new System.Drawing.Point(70, 137);
            this.BtnSendEmail.Name = "BtnSendEmail";
            this.BtnSendEmail.Size = new System.Drawing.Size(75, 23);
            this.BtnSendEmail.TabIndex = 11;
            this.BtnSendEmail.Text = "Confirm";
            this.BtnSendEmail.UseVisualStyleBackColor = true;
            this.BtnSendEmail.Click += new System.EventHandler(this.BtnSendEmail_Click);
            // 
            // ResetPasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(250, 186);
            this.Controls.Add(this.LblEmail);
            this.Controls.Add(this.TextBoxEmail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBoxMemorableWord);
            this.Controls.Add(this.BtnSendEmail);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ResetPasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ResetPasswordForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblEmail;
        private System.Windows.Forms.TextBox TextBoxEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxMemorableWord;
        private System.Windows.Forms.Button BtnSendEmail;
    }
}