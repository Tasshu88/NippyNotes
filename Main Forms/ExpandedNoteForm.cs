using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Nippy_Notes
{
    public partial class ExpandedNoteForm : Form
    {
        // Property to access or modify the RTF content of the RichTextBox within this form.
        public string NoteRtf
        {
            get { return richTextBox1.Rtf; }
            set { richTextBox1.Rtf = value; }
        }

        // Constructor that initializes the form and sets the current RTF content of the RichTextBox.
        public ExpandedNoteForm(string currentRtf)
        {
            InitializeComponent();
            this.NoteRtf = currentRtf; // Set the initial RTF content of the richTextBox1 to the passed currentRtf.
            InitializeTextFormattingControls(); // Call to setup text formatting controls like font size and style.
            AttachControlEvents(); // Call to attach event handlers for text formatting controls.
        }

        // Event handler for the close and save button.
        private void BtnCloseAndSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK; // Set the dialog result to OK to indicate successful closure.
            this.Close(); // Close the form.
        }

        // Method to initialize text formatting controls.
        private void InitializeTextFormattingControls()
        {
            // Setup for font names and sizes
            foreach (var fontFamily in FontFamily.Families)
            {
                ComboBoxFontExpanded.Items.Add(fontFamily.Name);
            }
            ComboBoxFontExpanded.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBoxSizeExpanded.Items.AddRange(new object[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 });
            ComboBoxSizeExpanded.DropDownStyle = ComboBoxStyle.DropDownList;

            // Default selection if available
            if (ComboBoxFontExpanded.Items.Contains("Arial"))
                ComboBoxFontExpanded.SelectedItem = "Arial";
            if (ComboBoxSizeExpanded.Items.Contains(12))
                ComboBoxSizeExpanded.SelectedItem = 12;
        }

        // Method to attach event handlers to controls.
        private void AttachControlEvents()
        {
            // Event handlers for font style buttons
            BtnBoldExpanded.Click += (s, e) => ToggleFontStyle(FontStyle.Bold);
            BtnItalicExpanded.Click += (s, e) => ToggleFontStyle(FontStyle.Italic);
            BtnUnderlineExpanded.Click += (s, e) => ToggleFontStyle(FontStyle.Underline);
            BtnStrikeOutExpanded.Click += (s, e) => ToggleFontStyle(FontStyle.Strikeout);

            // Handlers for font and size changes
            ComboBoxFontExpanded.SelectedIndexChanged += (s, e) => ApplyFontChange();
            ComboBoxSizeExpanded.SelectedIndexChanged += (s, e) => ApplyFontChange();

            // Color picker button
            BtnTextColourExpanded.Click += BtnTextColourExpanded_Click;
        }

        // Toggle the specified font style
        private void ToggleFontStyle(FontStyle style)
        {
            if (richTextBox1.SelectionFont != null)
            {
                FontStyle currentStyle = richTextBox1.SelectionFont.Style;
                FontStyle newStyle = currentStyle ^ style; // Toggle the specified style
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, newStyle);
            }
        }

        // Apply font change based on selected font and size
        private void ApplyFontChange()
        {
            if (ComboBoxFontExpanded.SelectedItem != null && ComboBoxSizeExpanded.SelectedItem != null)
            {
                string fontName = ComboBoxFontExpanded.SelectedItem.ToString();
                float fontSize = float.Parse(ComboBoxSizeExpanded.SelectedItem.ToString());
                FontStyle fontStyle = richTextBox1.SelectionFont?.Style ?? FontStyle.Regular;
                richTextBox1.SelectionFont = new Font(fontName, fontSize, fontStyle);
            }
        }

        // Handler for color picker button
        private void BtnTextColourExpanded_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionColor = colorDialog.Color; // Apply the selected color to the selected text
            }
        }
    }
}
