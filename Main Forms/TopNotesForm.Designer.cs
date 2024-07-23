namespace Nippy_Notes
{
    partial class TopNotesForm
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
            this.btnFetchTopNotes = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.LblTopNotes = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFetchTopNotes
            // 
            this.btnFetchTopNotes.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFetchTopNotes.Location = new System.Drawing.Point(104, 178);
            this.btnFetchTopNotes.Name = "btnFetchTopNotes";
            this.btnFetchTopNotes.Size = new System.Drawing.Size(122, 54);
            this.btnFetchTopNotes.TabIndex = 22;
            this.btnFetchTopNotes.Text = "Fetch TOP Notes";
            this.btnFetchTopNotes.UseVisualStyleBackColor = true;
            this.btnFetchTopNotes.Click += new System.EventHandler(this.btnFetchTopNotes_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(12, 100);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(302, 45);
            this.trackBar1.TabIndex = 23;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // LblTopNotes
            // 
            this.LblTopNotes.AutoSize = true;
            this.LblTopNotes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(237)))), ((int)(((byte)(227)))));
            this.LblTopNotes.Location = new System.Drawing.Point(116, 63);
            this.LblTopNotes.Name = "LblTopNotes";
            this.LblTopNotes.Size = new System.Drawing.Size(35, 13);
            this.LblTopNotes.TabIndex = 24;
            this.LblTopNotes.Text = "label1";
            this.LblTopNotes.Click += new System.EventHandler(this.LblTopNotes_Click);
            // 
            // TopNotesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(237)))), ((int)(((byte)(227)))));
            this.ClientSize = new System.Drawing.Size(330, 256);
            this.Controls.Add(this.LblTopNotes);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.btnFetchTopNotes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TopNotesForm";
            this.Text = "Top Notes";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFetchTopNotes;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label LblTopNotes;
    }
}