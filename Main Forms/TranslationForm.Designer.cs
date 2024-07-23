namespace Nippy_Notes
{
    partial class TranslationForm
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
            this.instructionLabel = new System.Windows.Forms.Label();
            this.existingRadioButton = new System.Windows.Forms.RadioButton();
            this.existingTranslationsListBox = new System.Windows.Forms.ListBox();
            this.newTranslationTextBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.newRadioButton = new System.Windows.Forms.RadioButton();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // instructionLabel
            // 
            this.instructionLabel.AutoSize = true;
            this.instructionLabel.Font = new System.Drawing.Font("Calibri", 11.25F);
            this.instructionLabel.Location = new System.Drawing.Point(7, 36);
            this.instructionLabel.Name = "instructionLabel";
            this.instructionLabel.Size = new System.Drawing.Size(197, 18);
            this.instructionLabel.TabIndex = 0;
            this.instructionLabel.Text = "Translate Product/Subcategory";
            // 
            // existingRadioButton
            // 
            this.existingRadioButton.AutoSize = true;
            this.existingRadioButton.Font = new System.Drawing.Font("Calibri", 11.25F);
            this.existingRadioButton.Location = new System.Drawing.Point(10, 79);
            this.existingRadioButton.Name = "existingRadioButton";
            this.existingRadioButton.Size = new System.Drawing.Size(202, 22);
            this.existingRadioButton.TabIndex = 1;
            this.existingRadioButton.TabStop = true;
            this.existingRadioButton.Text = "Select an existing translation";
            this.existingRadioButton.UseVisualStyleBackColor = true;
            // 
            // existingTranslationsListBox
            // 
            this.existingTranslationsListBox.FormattingEnabled = true;
            this.existingTranslationsListBox.Location = new System.Drawing.Point(132, 163);
            this.existingTranslationsListBox.Name = "existingTranslationsListBox";
            this.existingTranslationsListBox.Size = new System.Drawing.Size(155, 95);
            this.existingTranslationsListBox.TabIndex = 2;
            // 
            // newTranslationTextBox
            // 
            this.newTranslationTextBox.Location = new System.Drawing.Point(154, 126);
            this.newTranslationTextBox.Name = "newTranslationTextBox";
            this.newTranslationTextBox.Size = new System.Drawing.Size(100, 20);
            this.newTranslationTextBox.TabIndex = 3;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(308, 277);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(91, 35);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // newRadioButton
            // 
            this.newRadioButton.AutoSize = true;
            this.newRadioButton.Font = new System.Drawing.Font("Calibri", 11.25F);
            this.newRadioButton.Location = new System.Drawing.Point(208, 79);
            this.newRadioButton.Name = "newRadioButton";
            this.newRadioButton.Size = new System.Drawing.Size(176, 22);
            this.newRadioButton.TabIndex = 5;
            this.newRadioButton.TabStop = true;
            this.newRadioButton.Text = "Create a new translation";
            this.newRadioButton.UseVisualStyleBackColor = true;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(10, 277);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(91, 35);
            this.BtnCancel.TabIndex = 6;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // TranslationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(175)))), ((int)(((byte)(127)))));
            this.ClientSize = new System.Drawing.Size(411, 324);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.newRadioButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.newTranslationTextBox);
            this.Controls.Add(this.existingTranslationsListBox);
            this.Controls.Add(this.existingRadioButton);
            this.Controls.Add(this.instructionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TranslationForm";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label instructionLabel;
        private System.Windows.Forms.RadioButton existingRadioButton;
        private System.Windows.Forms.ListBox existingTranslationsListBox;
        private System.Windows.Forms.TextBox newTranslationTextBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.RadioButton newRadioButton;
        private System.Windows.Forms.Button BtnCancel;
    }
}