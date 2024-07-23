namespace Nippy_Notes
{
    partial class PasswordEntryFormDisableSecure
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
            this.TextBoxPassword = new System.Windows.Forms.TextBox();
            this.LblEnterPassword = new System.Windows.Forms.Label();
            this.BtnSubmitPassword = new System.Windows.Forms.Button();
            this.LabelMemorableWord = new System.Windows.Forms.Label();
            this.TextBoxMemorableWord = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TextBoxPassword
            // 
            this.TextBoxPassword.Location = new System.Drawing.Point(32, 41);
            this.TextBoxPassword.Name = "TextBoxPassword";
            this.TextBoxPassword.Size = new System.Drawing.Size(179, 20);
            this.TextBoxPassword.TabIndex = 1;
            // 
            // LblEnterPassword
            // 
            this.LblEnterPassword.AutoSize = true;
            this.LblEnterPassword.Location = new System.Drawing.Point(32, 22);
            this.LblEnterPassword.Name = "LblEnterPassword";
            this.LblEnterPassword.Size = new System.Drawing.Size(84, 13);
            this.LblEnterPassword.TabIndex = 3;
            this.LblEnterPassword.Text = "Enter Password:";
            // 
            // BtnSubmitPassword
            // 
            this.BtnSubmitPassword.Location = new System.Drawing.Point(73, 126);
            this.BtnSubmitPassword.Name = "BtnSubmitPassword";
            this.BtnSubmitPassword.Size = new System.Drawing.Size(75, 23);
            this.BtnSubmitPassword.TabIndex = 3;
            this.BtnSubmitPassword.Text = "Submit";
            this.BtnSubmitPassword.UseVisualStyleBackColor = true;
            this.BtnSubmitPassword.Click += new System.EventHandler(this.BtnSubmitPassword_Click);
            // 
            // LabelMemorableWord
            // 
            this.LabelMemorableWord.AutoSize = true;
            this.LabelMemorableWord.Location = new System.Drawing.Point(32, 75);
            this.LabelMemorableWord.Name = "LabelMemorableWord";
            this.LabelMemorableWord.Size = new System.Drawing.Size(116, 13);
            this.LabelMemorableWord.TabIndex = 6;
            this.LabelMemorableWord.Text = "Enter Memorable Word";
            // 
            // TextBoxMemorableWord
            // 
            this.TextBoxMemorableWord.Location = new System.Drawing.Point(35, 91);
            this.TextBoxMemorableWord.MaxLength = 12;
            this.TextBoxMemorableWord.Name = "TextBoxMemorableWord";
            this.TextBoxMemorableWord.Size = new System.Drawing.Size(91, 20);
            this.TextBoxMemorableWord.TabIndex = 2;
            // 
            // PasswordEntryFormDisableSecure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 161);
            this.Controls.Add(this.TextBoxMemorableWord);
            this.Controls.Add(this.LabelMemorableWord);
            this.Controls.Add(this.BtnSubmitPassword);
            this.Controls.Add(this.LblEnterPassword);
            this.Controls.Add(this.TextBoxPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PasswordEntryFormDisableSecure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PasswordEntryForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxPassword;
        private System.Windows.Forms.Label LblEnterPassword;
        private System.Windows.Forms.Button BtnSubmitPassword;
        private System.Windows.Forms.Label LabelMemorableWord;
        private System.Windows.Forms.TextBox TextBoxMemorableWord;
    }
}