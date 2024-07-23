namespace Nippy_Notes
{
    partial class FeedbackForm
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
            this.RichTextBoxFeedback = new System.Windows.Forms.RichTextBox();
            this.BtnSendFeedback = new System.Windows.Forms.Button();
            this.BtnCancelFeedback = new System.Windows.Forms.Button();
            this.PanelFeedbackFormDrag = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.BtnRightAlign = new System.Windows.Forms.Button();
            this.BtnCenterAlign = new System.Windows.Forms.Button();
            this.BtnLeftAlign = new System.Windows.Forms.Button();
            this.PanelTextColour = new System.Windows.Forms.Panel();
            this.BtnTextColour = new System.Windows.Forms.Button();
            this.BtnStrikeOut = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ComboBoxFont = new System.Windows.Forms.ComboBox();
            this.ComboBoxSize = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnUnderline = new System.Windows.Forms.Button();
            this.BtnBold = new System.Windows.Forms.Button();
            this.BtnItalic = new System.Windows.Forms.Button();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RichTextBoxFeedback
            // 
            this.RichTextBoxFeedback.Location = new System.Drawing.Point(12, 83);
            this.RichTextBoxFeedback.Name = "RichTextBoxFeedback";
            this.RichTextBoxFeedback.Size = new System.Drawing.Size(464, 229);
            this.RichTextBoxFeedback.TabIndex = 0;
            this.RichTextBoxFeedback.Text = "";
            // 
            // BtnSendFeedback
            // 
            this.BtnSendFeedback.Location = new System.Drawing.Point(147, 318);
            this.BtnSendFeedback.Name = "BtnSendFeedback";
            this.BtnSendFeedback.Size = new System.Drawing.Size(94, 38);
            this.BtnSendFeedback.TabIndex = 1;
            this.BtnSendFeedback.Text = "Send Feedback";
            this.BtnSendFeedback.UseVisualStyleBackColor = true;
            this.BtnSendFeedback.Click += new System.EventHandler(this.BtnSendFeedback_Click);
            // 
            // BtnCancelFeedback
            // 
            this.BtnCancelFeedback.Location = new System.Drawing.Point(247, 318);
            this.BtnCancelFeedback.Name = "BtnCancelFeedback";
            this.BtnCancelFeedback.Size = new System.Drawing.Size(83, 38);
            this.BtnCancelFeedback.TabIndex = 2;
            this.BtnCancelFeedback.Text = "Cancel";
            this.BtnCancelFeedback.UseVisualStyleBackColor = true;
            this.BtnCancelFeedback.Click += new System.EventHandler(this.BtnCancelFeedback_Click);
            // 
            // PanelFeedbackFormDrag
            // 
            this.PanelFeedbackFormDrag.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelFeedbackFormDrag.Location = new System.Drawing.Point(0, 0);
            this.PanelFeedbackFormDrag.Name = "PanelFeedbackFormDrag";
            this.PanelFeedbackFormDrag.Size = new System.Drawing.Size(488, 40);
            this.PanelFeedbackFormDrag.TabIndex = 11;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.BtnRightAlign);
            this.panel4.Controls.Add(this.BtnCenterAlign);
            this.panel4.Controls.Add(this.BtnLeftAlign);
            this.panel4.Controls.Add(this.PanelTextColour);
            this.panel4.Controls.Add(this.BtnTextColour);
            this.panel4.Controls.Add(this.BtnStrikeOut);
            this.panel4.Location = new System.Drawing.Point(290, 56);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(195, 29);
            this.panel4.TabIndex = 44;
            // 
            // BtnRightAlign
            // 
            this.BtnRightAlign.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnRightAlign.Image = global::Nippy_Notes.Properties.Resources.align_right__1_;
            this.BtnRightAlign.Location = new System.Drawing.Point(155, -1);
            this.BtnRightAlign.Name = "BtnRightAlign";
            this.BtnRightAlign.Size = new System.Drawing.Size(38, 30);
            this.BtnRightAlign.TabIndex = 12;
            this.BtnRightAlign.UseVisualStyleBackColor = true;
            // 
            // BtnCenterAlign
            // 
            this.BtnCenterAlign.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCenterAlign.Image = global::Nippy_Notes.Properties.Resources.align_center__1_;
            this.BtnCenterAlign.Location = new System.Drawing.Point(117, -1);
            this.BtnCenterAlign.Name = "BtnCenterAlign";
            this.BtnCenterAlign.Size = new System.Drawing.Size(38, 30);
            this.BtnCenterAlign.TabIndex = 11;
            this.BtnCenterAlign.UseVisualStyleBackColor = true;
            // 
            // BtnLeftAlign
            // 
            this.BtnLeftAlign.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLeftAlign.Image = global::Nippy_Notes.Properties.Resources.align_left__2_;
            this.BtnLeftAlign.Location = new System.Drawing.Point(79, -1);
            this.BtnLeftAlign.Name = "BtnLeftAlign";
            this.BtnLeftAlign.Size = new System.Drawing.Size(38, 30);
            this.BtnLeftAlign.TabIndex = 10;
            this.BtnLeftAlign.UseVisualStyleBackColor = true;
            // 
            // PanelTextColour
            // 
            this.PanelTextColour.BackColor = System.Drawing.Color.Black;
            this.PanelTextColour.Location = new System.Drawing.Point(-1, 25);
            this.PanelTextColour.Name = "PanelTextColour";
            this.PanelTextColour.Size = new System.Drawing.Size(36, 10);
            this.PanelTextColour.TabIndex = 6;
            // 
            // BtnTextColour
            // 
            this.BtnTextColour.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnTextColour.Image = global::Nippy_Notes.Properties.Resources.font_symbol_of_letter_a__1_;
            this.BtnTextColour.Location = new System.Drawing.Point(-1, -1);
            this.BtnTextColour.Name = "BtnTextColour";
            this.BtnTextColour.Size = new System.Drawing.Size(38, 30);
            this.BtnTextColour.TabIndex = 5;
            this.BtnTextColour.UseVisualStyleBackColor = true;
            // 
            // BtnStrikeOut
            // 
            this.BtnStrikeOut.Font = new System.Drawing.Font("Times New Roman", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Strikeout))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnStrikeOut.Image = global::Nippy_Notes.Properties.Resources.strikethrough__1_;
            this.BtnStrikeOut.Location = new System.Drawing.Point(35, -1);
            this.BtnStrikeOut.Name = "BtnStrikeOut";
            this.BtnStrikeOut.Size = new System.Drawing.Size(38, 30);
            this.BtnStrikeOut.TabIndex = 7;
            this.BtnStrikeOut.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.ComboBoxFont);
            this.panel3.Controls.Add(this.ComboBoxSize);
            this.panel3.Location = new System.Drawing.Point(129, 56);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(162, 29);
            this.panel3.TabIndex = 45;
            // 
            // ComboBoxFont
            // 
            this.ComboBoxFont.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxFont.FormattingEnabled = true;
            this.ComboBoxFont.Location = new System.Drawing.Point(0, 0);
            this.ComboBoxFont.Name = "ComboBoxFont";
            this.ComboBoxFont.Size = new System.Drawing.Size(106, 27);
            this.ComboBoxFont.TabIndex = 3;
            // 
            // ComboBoxSize
            // 
            this.ComboBoxSize.AllowDrop = true;
            this.ComboBoxSize.Font = new System.Drawing.Font("Calibri", 12F);
            this.ComboBoxSize.FormattingEnabled = true;
            this.ComboBoxSize.Location = new System.Drawing.Point(106, 0);
            this.ComboBoxSize.Name = "ComboBoxSize";
            this.ComboBoxSize.Size = new System.Drawing.Size(55, 27);
            this.ComboBoxSize.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.BtnUnderline);
            this.panel1.Controls.Add(this.BtnBold);
            this.panel1.Controls.Add(this.BtnItalic);
            this.panel1.Location = new System.Drawing.Point(5, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(125, 29);
            this.panel1.TabIndex = 43;
            // 
            // BtnUnderline
            // 
            this.BtnUnderline.Font = new System.Drawing.Font("Times New Roman", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnUnderline.Image = global::Nippy_Notes.Properties.Resources.underline_text_option__1___1_;
            this.BtnUnderline.Location = new System.Drawing.Point(86, -1);
            this.BtnUnderline.Name = "BtnUnderline";
            this.BtnUnderline.Size = new System.Drawing.Size(38, 29);
            this.BtnUnderline.TabIndex = 2;
            this.BtnUnderline.UseVisualStyleBackColor = true;
            // 
            // BtnBold
            // 
            this.BtnBold.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnBold.Image = global::Nippy_Notes.Properties.Resources.bold_text_option__1_;
            this.BtnBold.Location = new System.Drawing.Point(-2, -1);
            this.BtnBold.Name = "BtnBold";
            this.BtnBold.Size = new System.Drawing.Size(38, 30);
            this.BtnBold.TabIndex = 0;
            this.BtnBold.UseVisualStyleBackColor = true;
            // 
            // BtnItalic
            // 
            this.BtnItalic.Font = new System.Drawing.Font("Times New Roman", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnItalic.Image = global::Nippy_Notes.Properties.Resources.italic__1_;
            this.BtnItalic.Location = new System.Drawing.Point(42, -1);
            this.BtnItalic.Name = "BtnItalic";
            this.BtnItalic.Size = new System.Drawing.Size(38, 29);
            this.BtnItalic.TabIndex = 1;
            this.BtnItalic.UseVisualStyleBackColor = true;
            // 
            // FeedbackForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(239)))), ((int)(((byte)(231)))));
            this.ClientSize = new System.Drawing.Size(488, 361);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.PanelFeedbackFormDrag);
            this.Controls.Add(this.BtnCancelFeedback);
            this.Controls.Add(this.BtnSendFeedback);
            this.Controls.Add(this.RichTextBoxFeedback);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FeedbackForm";
            this.Text = "FeedbackForm";
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox RichTextBoxFeedback;
        private System.Windows.Forms.Button BtnSendFeedback;
        private System.Windows.Forms.Button BtnCancelFeedback;
        private System.Windows.Forms.Panel PanelFeedbackFormDrag;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button BtnRightAlign;
        private System.Windows.Forms.Button BtnCenterAlign;
        private System.Windows.Forms.Button BtnLeftAlign;
        private System.Windows.Forms.Panel PanelTextColour;
        private System.Windows.Forms.Button BtnTextColour;
        private System.Windows.Forms.Button BtnStrikeOut;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox ComboBoxFont;
        private System.Windows.Forms.ComboBox ComboBoxSize;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtnUnderline;
        private System.Windows.Forms.Button BtnBold;
        private System.Windows.Forms.Button BtnItalic;
    }
}