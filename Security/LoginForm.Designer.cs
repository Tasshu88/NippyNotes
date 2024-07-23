namespace Nippy_Notes.Security
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.LblEnterPassword = new System.Windows.Forms.Label();
            this.TextBoxPassword = new System.Windows.Forms.TextBox();
            this.BtnConfirm = new System.Windows.Forms.Button();
            this.LblPasswordTries = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.CheckBoxPassword = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // LblEnterPassword
            // 
            this.LblEnterPassword.AutoSize = true;
            this.LblEnterPassword.Location = new System.Drawing.Point(136, 137);
            this.LblEnterPassword.Name = "LblEnterPassword";
            this.LblEnterPassword.Size = new System.Drawing.Size(84, 13);
            this.LblEnterPassword.TabIndex = 6;
            this.LblEnterPassword.Text = "Enter Password:";
            // 
            // TextBoxPassword
            // 
            this.TextBoxPassword.Location = new System.Drawing.Point(139, 153);
            this.TextBoxPassword.Name = "TextBoxPassword";
            this.TextBoxPassword.Size = new System.Drawing.Size(179, 20);
            this.TextBoxPassword.TabIndex = 5;
            // 
            // BtnConfirm
            // 
            this.BtnConfirm.Location = new System.Drawing.Point(180, 179);
            this.BtnConfirm.Name = "BtnConfirm";
            this.BtnConfirm.Size = new System.Drawing.Size(75, 23);
            this.BtnConfirm.TabIndex = 7;
            this.BtnConfirm.Text = "Confirm";
            this.BtnConfirm.UseVisualStyleBackColor = true;
            this.BtnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // LblPasswordTries
            // 
            this.LblPasswordTries.AutoSize = true;
            this.LblPasswordTries.Location = new System.Drawing.Point(12, 217);
            this.LblPasswordTries.Name = "LblPasswordTries";
            this.LblPasswordTries.Size = new System.Drawing.Size(35, 13);
            this.LblPasswordTries.TabIndex = 8;
            this.LblPasswordTries.Text = "label1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Nippy_Notes.Properties.Resources.NippyNotesWhite__1_;
            this.pictureBox1.Location = new System.Drawing.Point(31, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 100);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // CheckBoxPassword
            // 
            this.CheckBoxPassword.AutoSize = true;
            this.CheckBoxPassword.Location = new System.Drawing.Point(323, 155);
            this.CheckBoxPassword.Name = "CheckBoxPassword";
            this.CheckBoxPassword.Size = new System.Drawing.Size(108, 17);
            this.CheckBoxPassword.TabIndex = 10;
            this.CheckBoxPassword.Text = "Show Password?";
            this.CheckBoxPassword.UseVisualStyleBackColor = true;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(455, 239);
            this.Controls.Add(this.CheckBoxPassword);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.LblPasswordTries);
            this.Controls.Add(this.BtnConfirm);
            this.Controls.Add(this.LblEnterPassword);
            this.Controls.Add(this.TextBoxPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblEnterPassword;
        private System.Windows.Forms.TextBox TextBoxPassword;
        private System.Windows.Forms.Button BtnConfirm;
        private System.Windows.Forms.Label LblPasswordTries;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox CheckBoxPassword;
    }
}