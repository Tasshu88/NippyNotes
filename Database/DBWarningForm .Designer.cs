namespace Nippy_Notes
{
    partial class DBWarningForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBWarningForm));
            this.AhAhPictureBox = new System.Windows.Forms.PictureBox();
            this.LblLewis = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.AhAhPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // AhAhPictureBox
            // 
            this.AhAhPictureBox.Image = global::Nippy_Notes.Properties.Resources.ahah;
            this.AhAhPictureBox.Location = new System.Drawing.Point(12, 12);
            this.AhAhPictureBox.Name = "AhAhPictureBox";
            this.AhAhPictureBox.Size = new System.Drawing.Size(323, 323);
            this.AhAhPictureBox.TabIndex = 0;
            this.AhAhPictureBox.TabStop = false;
            // 
            // LblLewis
            // 
            this.LblLewis.AutoSize = true;
            this.LblLewis.Font = new System.Drawing.Font("Calibri", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblLewis.Location = new System.Drawing.Point(38, 293);
            this.LblLewis.Name = "LblLewis";
            this.LblLewis.Size = new System.Drawing.Size(268, 42);
            this.LblLewis.TabIndex = 1;
            this.LblLewis.Text = "I think not Lewis.";
            // 
            // DBWarningForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(352, 359);
            this.Controls.Add(this.LblLewis);
            this.Controls.Add(this.AhAhPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DBWarningForm";
            ((System.ComponentModel.ISupportInitialize)(this.AhAhPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox AhAhPictureBox;
        private System.Windows.Forms.Label LblLewis;
    }
}
