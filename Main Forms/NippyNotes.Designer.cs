namespace Nippy_Notes
{
    partial class NippyNotes
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NippyNotes));
            this.PanelTitle = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.PanelTopButtons = new System.Windows.Forms.Panel();
            this.BtnTOPNotes = new System.Windows.Forms.Button();
            this.BtnSettings = new System.Windows.Forms.Button();
            this.BtnExportNote = new System.Windows.Forms.Button();
            this.BtnCreateNotes = new System.Windows.Forms.Button();
            this.BtnSearchNote = new System.Windows.Forms.Button();
            this.BtnEmailNote = new System.Windows.Forms.Button();
            this.BtnImportNote = new System.Windows.Forms.Button();
            this.LblNoteID = new System.Windows.Forms.Label();
            this.TextBoxID = new System.Windows.Forms.TextBox();
            this.TextBoxDate = new System.Windows.Forms.TextBox();
            this.LblDate = new System.Windows.Forms.Label();
            this.LblProduct = new System.Windows.Forms.Label();
            this.ComboBoxProducts = new System.Windows.Forms.ComboBox();
            this.TextBoxSubject = new System.Windows.Forms.TextBox();
            this.LblSubject = new System.Windows.Forms.Label();
            this.LblDetails = new System.Windows.Forms.Label();
            this.RichTextBoxDetails = new System.Windows.Forms.RichTextBox();
            this.lblFiles = new System.Windows.Forms.Label();
            this.SubcategoryComboBox = new System.Windows.Forms.ComboBox();
            this.LblSubcategory = new System.Windows.Forms.Label();
            this.TextBoxKeywords = new System.Windows.Forms.TextBox();
            this.LblKeyword = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.LblStatus = new System.Windows.Forms.Label();
            this.LblSubjectAsterisk = new System.Windows.Forms.Label();
            this.LblProductAsterisk = new System.Windows.Forms.Label();
            this.LblDetailsAsterisk = new System.Windows.Forms.Label();
            this.LblPageAmount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnUnderline = new System.Windows.Forms.Button();
            this.BtnBold = new System.Windows.Forms.Button();
            this.BtnItalic = new System.Windows.Forms.Button();
            this.PanelTextColour = new System.Windows.Forms.Panel();
            this.ComboBoxSize = new System.Windows.Forms.ComboBox();
            this.ComboBoxFont = new System.Windows.Forms.ComboBox();
            this.sqlCommand1 = new Microsoft.Data.SqlClient.SqlCommand();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.BtnRightAlign = new System.Windows.Forms.Button();
            this.BtnCenterAlign = new System.Windows.Forms.Button();
            this.BtnLeftAlign = new System.Windows.Forms.Button();
            this.BtnTextColour = new System.Windows.Forms.Button();
            this.BtnStrikeOut = new System.Windows.Forms.Button();
            this.toolTipImport = new System.Windows.Forms.ToolTip(this.components);
            this.BtnDeleteFiles = new System.Windows.Forms.Button();
            this.BtnRefresh = new System.Windows.Forms.Button();
            this.BtnDeleteNote = new System.Windows.Forms.Button();
            this.BtnSaveNote = new System.Windows.Forms.Button();
            this.BtnLeftPage = new System.Windows.Forms.Button();
            this.BtnRightPage = new System.Windows.Forms.Button();
            this.BtnAttachFiles = new System.Windows.Forms.Button();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.PanelTopButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelTitle
            // 
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.pictureBox1);
            this.PanelTitle.Location = new System.Drawing.Point(1, 0);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(201, 96);
            this.PanelTitle.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Nippy_Notes.Properties.Resources._2024_04_16_18_48_17_Edit_your_own_logo___LogoAI_com__3_;
            this.pictureBox1.Location = new System.Drawing.Point(6, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(185, 82);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // PanelTopButtons
            // 
            this.PanelTopButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTopButtons.Controls.Add(this.BtnTOPNotes);
            this.PanelTopButtons.Controls.Add(this.BtnSettings);
            this.PanelTopButtons.Controls.Add(this.BtnExportNote);
            this.PanelTopButtons.Controls.Add(this.BtnCreateNotes);
            this.PanelTopButtons.Controls.Add(this.BtnSearchNote);
            this.PanelTopButtons.Controls.Add(this.BtnEmailNote);
            this.PanelTopButtons.Controls.Add(this.BtnImportNote);
            this.PanelTopButtons.Location = new System.Drawing.Point(201, 0);
            this.PanelTopButtons.Name = "PanelTopButtons";
            this.PanelTopButtons.Size = new System.Drawing.Size(384, 72);
            this.PanelTopButtons.TabIndex = 5;
            // 
            // BtnTOPNotes
            // 
            this.BtnTOPNotes.FlatAppearance.BorderSize = 0;
            this.BtnTOPNotes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.BtnTOPNotes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTOPNotes.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnTOPNotes.Image = global::Nippy_Notes.Properties.Resources.top_10__1_;
            this.BtnTOPNotes.Location = new System.Drawing.Point(283, 8);
            this.BtnTOPNotes.Name = "BtnTOPNotes";
            this.BtnTOPNotes.Size = new System.Drawing.Size(42, 51);
            this.BtnTOPNotes.TabIndex = 39;
            this.BtnTOPNotes.UseVisualStyleBackColor = true;
            this.BtnTOPNotes.Click += new System.EventHandler(this.BtnTOPNotes_Click);
            // 
            // BtnSettings
            // 
            this.BtnSettings.FlatAppearance.BorderSize = 0;
            this.BtnSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.BtnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSettings.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSettings.Image = global::Nippy_Notes.Properties.Resources.settings__1_;
            this.BtnSettings.Location = new System.Drawing.Point(339, 8);
            this.BtnSettings.Name = "BtnSettings";
            this.BtnSettings.Size = new System.Drawing.Size(42, 51);
            this.BtnSettings.TabIndex = 38;
            this.BtnSettings.UseVisualStyleBackColor = true;
            // 
            // BtnExportNote
            // 
            this.BtnExportNote.FlatAppearance.BorderSize = 0;
            this.BtnExportNote.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.BtnExportNote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnExportNote.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnExportNote.Image = global::Nippy_Notes.Properties.Resources.BtnExport1;
            this.BtnExportNote.Location = new System.Drawing.Point(59, 8);
            this.BtnExportNote.Name = "BtnExportNote";
            this.BtnExportNote.Size = new System.Drawing.Size(42, 46);
            this.BtnExportNote.TabIndex = 37;
            this.BtnExportNote.UseVisualStyleBackColor = true;
            this.BtnExportNote.Click += new System.EventHandler(this.BtnExportNote_Click);
            // 
            // BtnCreateNotes
            // 
            this.BtnCreateNotes.FlatAppearance.BorderSize = 0;
            this.BtnCreateNotes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.BtnCreateNotes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCreateNotes.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCreateNotes.Image = global::Nippy_Notes.Properties.Resources.writing__1_;
            this.BtnCreateNotes.Location = new System.Drawing.Point(227, 7);
            this.BtnCreateNotes.Name = "BtnCreateNotes";
            this.BtnCreateNotes.Size = new System.Drawing.Size(42, 51);
            this.BtnCreateNotes.TabIndex = 36;
            this.BtnCreateNotes.UseVisualStyleBackColor = true;
            this.BtnCreateNotes.Click += new System.EventHandler(this.BtnCreateNotes_Click);
            // 
            // BtnSearchNote
            // 
            this.BtnSearchNote.FlatAppearance.BorderSize = 0;
            this.BtnSearchNote.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.BtnSearchNote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSearchNote.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSearchNote.Image = global::Nippy_Notes.Properties.Resources.magnifying_glass__1_;
            this.BtnSearchNote.Location = new System.Drawing.Point(171, 5);
            this.BtnSearchNote.Name = "BtnSearchNote";
            this.BtnSearchNote.Size = new System.Drawing.Size(42, 51);
            this.BtnSearchNote.TabIndex = 35;
            this.BtnSearchNote.UseVisualStyleBackColor = true;
            this.BtnSearchNote.Click += new System.EventHandler(this.BtnSearchNote_Click);
            // 
            // BtnEmailNote
            // 
            this.BtnEmailNote.FlatAppearance.BorderSize = 0;
            this.BtnEmailNote.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.BtnEmailNote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnEmailNote.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnEmailNote.Image = global::Nippy_Notes.Properties.Resources.mail_2_1_;
            this.BtnEmailNote.Location = new System.Drawing.Point(115, 8);
            this.BtnEmailNote.Name = "BtnEmailNote";
            this.BtnEmailNote.Size = new System.Drawing.Size(42, 46);
            this.BtnEmailNote.TabIndex = 31;
            this.BtnEmailNote.UseVisualStyleBackColor = true;
            this.BtnEmailNote.Click += new System.EventHandler(this.BtnEmailNote_Click);
            this.BtnEmailNote.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            // 
            // BtnImportNote
            // 
            this.BtnImportNote.FlatAppearance.BorderSize = 0;
            this.BtnImportNote.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.BtnImportNote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnImportNote.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnImportNote.Image = global::Nippy_Notes.Properties.Resources.BtnImport;
            this.BtnImportNote.Location = new System.Drawing.Point(3, 7);
            this.BtnImportNote.Name = "BtnImportNote";
            this.BtnImportNote.Size = new System.Drawing.Size(42, 46);
            this.BtnImportNote.TabIndex = 30;
            this.BtnImportNote.UseVisualStyleBackColor = true;
            this.BtnImportNote.Click += new System.EventHandler(this.BtnImportNote_Click);
            // 
            // LblNoteID
            // 
            this.LblNoteID.AutoSize = true;
            this.LblNoteID.BackColor = System.Drawing.Color.Transparent;
            this.LblNoteID.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblNoteID.Location = new System.Drawing.Point(29, 106);
            this.LblNoteID.Name = "LblNoteID";
            this.LblNoteID.Size = new System.Drawing.Size(59, 18);
            this.LblNoteID.TabIndex = 2;
            this.LblNoteID.Text = "Note ID:";
            // 
            // TextBoxID
            // 
            this.TextBoxID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxID.Location = new System.Drawing.Point(94, 104);
            this.TextBoxID.Name = "TextBoxID";
            this.TextBoxID.ReadOnly = true;
            this.TextBoxID.Size = new System.Drawing.Size(87, 20);
            this.TextBoxID.TabIndex = 0;
            // 
            // TextBoxDate
            // 
            this.TextBoxDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxDate.Location = new System.Drawing.Point(94, 130);
            this.TextBoxDate.Name = "TextBoxDate";
            this.TextBoxDate.ReadOnly = true;
            this.TextBoxDate.Size = new System.Drawing.Size(87, 20);
            this.TextBoxDate.TabIndex = 5;
            // 
            // LblDate
            // 
            this.LblDate.AutoSize = true;
            this.LblDate.BackColor = System.Drawing.Color.Transparent;
            this.LblDate.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblDate.Location = new System.Drawing.Point(29, 132);
            this.LblDate.Name = "LblDate";
            this.LblDate.Size = new System.Drawing.Size(41, 18);
            this.LblDate.TabIndex = 4;
            this.LblDate.Text = "Date:";
            // 
            // LblProduct
            // 
            this.LblProduct.AutoSize = true;
            this.LblProduct.BackColor = System.Drawing.Color.Transparent;
            this.LblProduct.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblProduct.Location = new System.Drawing.Point(256, 103);
            this.LblProduct.Name = "LblProduct";
            this.LblProduct.Size = new System.Drawing.Size(60, 18);
            this.LblProduct.TabIndex = 6;
            this.LblProduct.Text = "Product:";
            // 
            // ComboBoxProducts
            // 
            this.ComboBoxProducts.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComboBoxProducts.FormattingEnabled = true;
            this.ComboBoxProducts.Location = new System.Drawing.Point(322, 103);
            this.ComboBoxProducts.Name = "ComboBoxProducts";
            this.ComboBoxProducts.Size = new System.Drawing.Size(112, 21);
            this.ComboBoxProducts.TabIndex = 2;
            // 
            // TextBoxSubject
            // 
            this.TextBoxSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxSubject.Location = new System.Drawing.Point(582, 104);
            this.TextBoxSubject.Name = "TextBoxSubject";
            this.TextBoxSubject.Size = new System.Drawing.Size(171, 20);
            this.TextBoxSubject.TabIndex = 3;
            // 
            // LblSubject
            // 
            this.LblSubject.AutoSize = true;
            this.LblSubject.BackColor = System.Drawing.Color.Transparent;
            this.LblSubject.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSubject.Location = new System.Drawing.Point(517, 106);
            this.LblSubject.Name = "LblSubject";
            this.LblSubject.Size = new System.Drawing.Size(58, 18);
            this.LblSubject.TabIndex = 8;
            this.LblSubject.Text = "Subject:";
            // 
            // LblDetails
            // 
            this.LblDetails.AutoSize = true;
            this.LblDetails.BackColor = System.Drawing.Color.Transparent;
            this.LblDetails.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblDetails.Location = new System.Drawing.Point(29, 189);
            this.LblDetails.Name = "LblDetails";
            this.LblDetails.Size = new System.Drawing.Size(55, 18);
            this.LblDetails.TabIndex = 10;
            this.LblDetails.Text = "Details:";
            // 
            // RichTextBoxDetails
            // 
            this.RichTextBoxDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RichTextBoxDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RichTextBoxDetails.Location = new System.Drawing.Point(15, 235);
            this.RichTextBoxDetails.Name = "RichTextBoxDetails";
            this.RichTextBoxDetails.Size = new System.Drawing.Size(759, 261);
            this.RichTextBoxDetails.TabIndex = 1;
            this.RichTextBoxDetails.Text = "";
            // 
            // lblFiles
            // 
            this.lblFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFiles.AutoSize = true;
            this.lblFiles.BackColor = System.Drawing.Color.Transparent;
            this.lblFiles.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiles.Location = new System.Drawing.Point(26, 558);
            this.lblFiles.Name = "lblFiles";
            this.lblFiles.Size = new System.Drawing.Size(41, 18);
            this.lblFiles.TabIndex = 14;
            this.lblFiles.Text = "Files:";
            // 
            // SubcategoryComboBox
            // 
            this.SubcategoryComboBox.FormattingEnabled = true;
            this.SubcategoryComboBox.Location = new System.Drawing.Point(322, 129);
            this.SubcategoryComboBox.Name = "SubcategoryComboBox";
            this.SubcategoryComboBox.Size = new System.Drawing.Size(112, 21);
            this.SubcategoryComboBox.TabIndex = 3;
            // 
            // LblSubcategory
            // 
            this.LblSubcategory.AutoSize = true;
            this.LblSubcategory.BackColor = System.Drawing.Color.Transparent;
            this.LblSubcategory.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSubcategory.Location = new System.Drawing.Point(228, 132);
            this.LblSubcategory.Name = "LblSubcategory";
            this.LblSubcategory.Size = new System.Drawing.Size(88, 18);
            this.LblSubcategory.TabIndex = 29;
            this.LblSubcategory.Text = "Subcategory:";
            // 
            // TextBoxKeywords
            // 
            this.TextBoxKeywords.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxKeywords.Location = new System.Drawing.Point(582, 130);
            this.TextBoxKeywords.Name = "TextBoxKeywords";
            this.TextBoxKeywords.Size = new System.Drawing.Size(171, 20);
            this.TextBoxKeywords.TabIndex = 8;
            // 
            // LblKeyword
            // 
            this.LblKeyword.AutoSize = true;
            this.LblKeyword.BackColor = System.Drawing.Color.Transparent;
            this.LblKeyword.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblKeyword.Location = new System.Drawing.Point(497, 132);
            this.LblKeyword.Name = "LblKeyword";
            this.LblKeyword.Size = new System.Drawing.Size(79, 18);
            this.LblKeyword.TabIndex = 13;
            this.LblKeyword.Text = "Keyword(s)";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(28, 579);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(733, 201);
            this.dataGridView1.TabIndex = 33;
            // 
            // LblStatus
            // 
            this.LblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblStatus.AutoSize = true;
            this.LblStatus.BackColor = System.Drawing.Color.Transparent;
            this.LblStatus.Font = new System.Drawing.Font("Calibri", 14F);
            this.LblStatus.Location = new System.Drawing.Point(32, 788);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(0, 23);
            this.LblStatus.TabIndex = 34;
            // 
            // LblSubjectAsterisk
            // 
            this.LblSubjectAsterisk.AutoSize = true;
            this.LblSubjectAsterisk.BackColor = System.Drawing.Color.Transparent;
            this.LblSubjectAsterisk.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.LblSubjectAsterisk.ForeColor = System.Drawing.Color.Red;
            this.LblSubjectAsterisk.Location = new System.Drawing.Point(754, 102);
            this.LblSubjectAsterisk.Name = "LblSubjectAsterisk";
            this.LblSubjectAsterisk.Size = new System.Drawing.Size(17, 24);
            this.LblSubjectAsterisk.TabIndex = 35;
            this.LblSubjectAsterisk.Text = "*";
            this.LblSubjectAsterisk.Visible = false;
            // 
            // LblProductAsterisk
            // 
            this.LblProductAsterisk.AutoSize = true;
            this.LblProductAsterisk.BackColor = System.Drawing.Color.Transparent;
            this.LblProductAsterisk.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.LblProductAsterisk.ForeColor = System.Drawing.Color.Red;
            this.LblProductAsterisk.Location = new System.Drawing.Point(435, 102);
            this.LblProductAsterisk.Name = "LblProductAsterisk";
            this.LblProductAsterisk.Size = new System.Drawing.Size(17, 24);
            this.LblProductAsterisk.TabIndex = 4;
            this.LblProductAsterisk.Text = "*";
            this.LblProductAsterisk.Visible = false;
            // 
            // LblDetailsAsterisk
            // 
            this.LblDetailsAsterisk.AutoSize = true;
            this.LblDetailsAsterisk.BackColor = System.Drawing.Color.Transparent;
            this.LblDetailsAsterisk.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.LblDetailsAsterisk.ForeColor = System.Drawing.Color.Red;
            this.LblDetailsAsterisk.Location = new System.Drawing.Point(80, 190);
            this.LblDetailsAsterisk.Name = "LblDetailsAsterisk";
            this.LblDetailsAsterisk.Size = new System.Drawing.Size(17, 24);
            this.LblDetailsAsterisk.TabIndex = 37;
            this.LblDetailsAsterisk.Text = "*";
            this.LblDetailsAsterisk.Visible = false;
            // 
            // LblPageAmount
            // 
            this.LblPageAmount.AutoSize = true;
            this.LblPageAmount.BackColor = System.Drawing.Color.Transparent;
            this.LblPageAmount.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblPageAmount.Location = new System.Drawing.Point(676, 155);
            this.LblPageAmount.Name = "LblPageAmount";
            this.LblPageAmount.Size = new System.Drawing.Size(0, 19);
            this.LblPageAmount.TabIndex = 38;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(613, 793);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 18);
            this.label1.TabIndex = 39;
            this.label1.Text = "Created By Dan Daley";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.BtnUnderline);
            this.panel1.Controls.Add(this.BtnBold);
            this.panel1.Controls.Add(this.BtnItalic);
            this.panel1.Location = new System.Drawing.Point(30, 208);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(125, 29);
            this.panel1.TabIndex = 40;
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
            // PanelTextColour
            // 
            this.PanelTextColour.BackColor = System.Drawing.Color.Black;
            this.PanelTextColour.Location = new System.Drawing.Point(-1, 25);
            this.PanelTextColour.Name = "PanelTextColour";
            this.PanelTextColour.Size = new System.Drawing.Size(36, 10);
            this.PanelTextColour.TabIndex = 6;
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
            // ComboBoxFont
            // 
            this.ComboBoxFont.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxFont.FormattingEnabled = true;
            this.ComboBoxFont.Location = new System.Drawing.Point(0, 0);
            this.ComboBoxFont.Name = "ComboBoxFont";
            this.ComboBoxFont.Size = new System.Drawing.Size(106, 27);
            this.ComboBoxFont.TabIndex = 3;
            // 
            // sqlCommand1
            // 
            this.sqlCommand1.CommandTimeout = 30;
            this.sqlCommand1.EnableOptimizedParameterBinding = false;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.ComboBoxFont);
            this.panel3.Controls.Add(this.ComboBoxSize);
            this.panel3.Location = new System.Drawing.Point(154, 208);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(162, 29);
            this.panel3.TabIndex = 41;
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
            this.panel4.Location = new System.Drawing.Point(315, 208);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(195, 29);
            this.panel4.TabIndex = 41;
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
            // BtnDeleteFiles
            // 
            this.BtnDeleteFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDeleteFiles.BackColor = System.Drawing.Color.Transparent;
            this.BtnDeleteFiles.FlatAppearance.BorderSize = 0;
            this.BtnDeleteFiles.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.BtnDeleteFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDeleteFiles.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDeleteFiles.Image = global::Nippy_Notes.Properties.Resources.delete__2_;
            this.BtnDeleteFiles.Location = new System.Drawing.Point(725, 548);
            this.BtnDeleteFiles.Name = "BtnDeleteFiles";
            this.BtnDeleteFiles.Size = new System.Drawing.Size(28, 30);
            this.BtnDeleteFiles.TabIndex = 20;
            this.BtnDeleteFiles.UseVisualStyleBackColor = false;
            this.BtnDeleteFiles.Click += new System.EventHandler(this.BtnDeleteFiles_Click);
            // 
            // BtnRefresh
            // 
            this.BtnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.BtnRefresh.FlatAppearance.BorderSize = 0;
            this.BtnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.BtnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRefresh.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnRefresh.Image = global::Nippy_Notes.Properties.Resources.sync__2_;
            this.BtnRefresh.Location = new System.Drawing.Point(692, 205);
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new System.Drawing.Size(28, 30);
            this.BtnRefresh.TabIndex = 42;
            this.BtnRefresh.UseVisualStyleBackColor = false;
            this.BtnRefresh.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnDeleteNote
            // 
            this.BtnDeleteNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDeleteNote.BackColor = System.Drawing.Color.Transparent;
            this.BtnDeleteNote.FlatAppearance.BorderSize = 0;
            this.BtnDeleteNote.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.BtnDeleteNote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDeleteNote.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDeleteNote.Image = global::Nippy_Notes.Properties.Resources.delete__2___2_;
            this.BtnDeleteNote.Location = new System.Drawing.Point(719, 205);
            this.BtnDeleteNote.Name = "BtnDeleteNote";
            this.BtnDeleteNote.Size = new System.Drawing.Size(28, 30);
            this.BtnDeleteNote.TabIndex = 34;
            this.BtnDeleteNote.UseVisualStyleBackColor = false;
            this.BtnDeleteNote.Click += new System.EventHandler(this.BtnDeleteNote_Click);
            // 
            // BtnSaveNote
            // 
            this.BtnSaveNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSaveNote.BackColor = System.Drawing.Color.Transparent;
            this.BtnSaveNote.FlatAppearance.BorderSize = 0;
            this.BtnSaveNote.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.BtnSaveNote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSaveNote.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSaveNote.Image = global::Nippy_Notes.Properties.Resources.save__1___1_;
            this.BtnSaveNote.Location = new System.Drawing.Point(747, 205);
            this.BtnSaveNote.Name = "BtnSaveNote";
            this.BtnSaveNote.Size = new System.Drawing.Size(28, 30);
            this.BtnSaveNote.TabIndex = 1;
            this.BtnSaveNote.UseVisualStyleBackColor = false;
            this.BtnSaveNote.Click += new System.EventHandler(this.BtnSaveNote_Click);
            // 
            // BtnLeftPage
            // 
            this.BtnLeftPage.BackColor = System.Drawing.Color.Transparent;
            this.BtnLeftPage.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.BtnLeftPage.Image = global::Nippy_Notes.Properties.Resources.ButtonLeft;
            this.BtnLeftPage.Location = new System.Drawing.Point(676, 177);
            this.BtnLeftPage.Name = "BtnLeftPage";
            this.BtnLeftPage.Size = new System.Drawing.Size(42, 29);
            this.BtnLeftPage.TabIndex = 24;
            this.BtnLeftPage.UseVisualStyleBackColor = false;
            this.BtnLeftPage.Click += new System.EventHandler(this.BtnLeftPage_Click);
            // 
            // BtnRightPage
            // 
            this.BtnRightPage.BackColor = System.Drawing.Color.Transparent;
            this.BtnRightPage.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.BtnRightPage.Image = global::Nippy_Notes.Properties.Resources.Arrow;
            this.BtnRightPage.Location = new System.Drawing.Point(720, 177);
            this.BtnRightPage.Name = "BtnRightPage";
            this.BtnRightPage.Size = new System.Drawing.Size(42, 29);
            this.BtnRightPage.TabIndex = 23;
            this.BtnRightPage.UseVisualStyleBackColor = false;
            this.BtnRightPage.Click += new System.EventHandler(this.BtnRightPage_Click);
            // 
            // BtnAttachFiles
            // 
            this.BtnAttachFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAttachFiles.BackColor = System.Drawing.Color.Transparent;
            this.BtnAttachFiles.FlatAppearance.BorderSize = 0;
            this.BtnAttachFiles.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.BtnAttachFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAttachFiles.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAttachFiles.Image = global::Nippy_Notes.Properties.Resources.paper_clip__1_;
            this.BtnAttachFiles.Location = new System.Drawing.Point(705, 549);
            this.BtnAttachFiles.Name = "BtnAttachFiles";
            this.BtnAttachFiles.Size = new System.Drawing.Size(19, 30);
            this.BtnAttachFiles.TabIndex = 6;
            this.BtnAttachFiles.UseVisualStyleBackColor = false;
            this.BtnAttachFiles.Click += new System.EventHandler(this.BtnAttachFiles_Click);
            // 
            // NippyNotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(788, 820);
            this.Controls.Add(this.BtnDeleteFiles);
            this.Controls.Add(this.BtnRefresh);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LblPageAmount);
            this.Controls.Add(this.BtnDeleteNote);
            this.Controls.Add(this.LblDetailsAsterisk);
            this.Controls.Add(this.BtnSaveNote);
            this.Controls.Add(this.LblProductAsterisk);
            this.Controls.Add(this.LblSubjectAsterisk);
            this.Controls.Add(this.LblStatus);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.TextBoxKeywords);
            this.Controls.Add(this.LblKeyword);
            this.Controls.Add(this.SubcategoryComboBox);
            this.Controls.Add(this.LblSubcategory);
            this.Controls.Add(this.BtnLeftPage);
            this.Controls.Add(this.BtnRightPage);
            this.Controls.Add(this.BtnAttachFiles);
            this.Controls.Add(this.lblFiles);
            this.Controls.Add(this.RichTextBoxDetails);
            this.Controls.Add(this.LblDetails);
            this.Controls.Add(this.TextBoxSubject);
            this.Controls.Add(this.LblSubject);
            this.Controls.Add(this.ComboBoxProducts);
            this.Controls.Add(this.LblProduct);
            this.Controls.Add(this.TextBoxDate);
            this.Controls.Add(this.LblDate);
            this.Controls.Add(this.TextBoxID);
            this.Controls.Add(this.LblNoteID);
            this.Controls.Add(this.PanelTopButtons);
            this.Controls.Add(this.PanelTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NippyNotes";
            this.Text = "NippyNotes";
            this.PanelTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.PanelTopButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.Panel PanelTopButtons;
        private System.Windows.Forms.Button BtnImportNote;
        private System.Windows.Forms.Label LblNoteID;
        private System.Windows.Forms.TextBox TextBoxID;
        private System.Windows.Forms.TextBox TextBoxDate;
        private System.Windows.Forms.Label LblDate;
        private System.Windows.Forms.Label LblProduct;
        private System.Windows.Forms.ComboBox ComboBoxProducts;
        private System.Windows.Forms.TextBox TextBoxSubject;
        private System.Windows.Forms.Label LblSubject;
        private System.Windows.Forms.Label LblDetails;
        private System.Windows.Forms.RichTextBox RichTextBoxDetails;
        private System.Windows.Forms.Label lblFiles;
        private System.Windows.Forms.Button BtnSaveNote;
        private System.Windows.Forms.Button BtnSearchNote;
        private System.Windows.Forms.Button BtnDeleteNote;
        private System.Windows.Forms.Button BtnDeleteFiles;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button BtnAttachFiles;
        private System.Windows.Forms.BindingSource filesBindingSource;
        private System.Windows.Forms.BindingSource filesBindingSource1;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeKBDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button BtnRightPage;
        private System.Windows.Forms.Button BtnLeftPage;
        private System.Windows.Forms.Button BtnCreateNotes;
        public System.Windows.Forms.ComboBox SubcategoryComboBox;
        private System.Windows.Forms.Label LblSubcategory;
        public System.Windows.Forms.TextBox TextBoxKeywords;
        private System.Windows.Forms.Label LblKeyword;
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label LblStatus;
        private System.Windows.Forms.Label LblSubjectAsterisk;
        private System.Windows.Forms.Label LblProductAsterisk;
        private System.Windows.Forms.Label LblDetailsAsterisk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnSettings;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtnBold;
        private System.Windows.Forms.Button BtnItalic;
        private System.Windows.Forms.ComboBox ComboBoxSize;
        private System.Windows.Forms.ComboBox ComboBoxFont;
        private System.Windows.Forms.Button BtnUnderline;
        private System.Windows.Forms.Button BtnStrikeOut;
        private System.Windows.Forms.Panel PanelTextColour;
        private System.Windows.Forms.Button BtnTextColour;
        private Microsoft.Data.SqlClient.SqlCommand sqlCommand1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button BtnTOPNotes;
        private System.Windows.Forms.ToolTip toolTipImport;
        private System.Windows.Forms.Button BtnExportNote;
        private System.Windows.Forms.Button BtnRefresh;
        public System.Windows.Forms.Label LblPageAmount;
        private System.Windows.Forms.Button BtnRightAlign;
        private System.Windows.Forms.Button BtnCenterAlign;
        private System.Windows.Forms.Button BtnLeftAlign;
        private System.Windows.Forms.Button BtnEmailNote;
    }
}

