using System;
using System.Drawing;
using System.Windows.Forms;

namespace Nippy_Notes.Helpers
{
    public static class RichTextBoxHelper
    {

        // Applys default font and size to the RichTextBox
        public static void ApplyDefaultFontAndSize(RichTextBox richTextBox)
        {
            // Get default font and size from database or use Arial if not set
            string defaultFont = DatabaseHelper.GetFontSetting() ?? "Arial";
            int defaultFontSize = DatabaseHelper.GetFontSizeSetting();

            // Set the font and size to the RichTextBox
            richTextBox.Font = new Font(defaultFont, defaultFontSize);
        }


        public static void InitializeTextFormattingControls(Form form, RichTextBox richTextBox,
                                                            Button btnBold, Button btnItalic, Button btnUnderline,
                                                            ComboBox comboBoxFont, ComboBox comboBoxSize,
                                                            Button btnTextColour, Button btnStrikeOut,
                                                            Button btnLeftAlign, Button btnCenterAlign, Button btnRightAlign)
        {
            // Set event handlers for the buttons to toggle font styles
            btnBold.Click += (s, e) => ToggleFontStyle(richTextBox, FontStyle.Bold);
            btnItalic.Click += (s, e) => ToggleFontStyle(richTextBox, FontStyle.Italic);
            btnUnderline.Click += (s, e) => ToggleFontStyle(richTextBox, FontStyle.Underline);
            btnStrikeOut.Click += (s, e) => ToggleFontStyle(richTextBox, FontStyle.Strikeout);

            // Set event handlers for the comboboxes to change font and size
            comboBoxFont.SelectedIndexChanged += (s, e) => ApplyFontChange(richTextBox, comboBoxFont, comboBoxSize);
            comboBoxSize.SelectedIndexChanged += (s, e) => ApplyFontChange(richTextBox, comboBoxFont, comboBoxSize);

            // Set event handler for the color button to change text color
            btnTextColour.Click += (s, e) => ChangeTextColor(richTextBox);

            // Set event handlers for alignment buttons
            btnLeftAlign.Click += (s, e) => SetTextAlignment(richTextBox, HorizontalAlignment.Left);
            btnCenterAlign.Click += (s, e) => SetTextAlignment(richTextBox, HorizontalAlignment.Center);
            btnRightAlign.Click += (s, e) => SetTextAlignment(richTextBox, HorizontalAlignment.Right);

            // Initialize the font and size comboboxes with values
            InitializeComboBoxes(comboBoxFont, comboBoxSize);

            // Set default font and size in combo boxes
            string defaultFont = DatabaseHelper.GetFontSetting() ?? "Arial";
            int defaultFontSize = DatabaseHelper.GetFontSizeSetting();
            string defaultFontSizeString = defaultFontSize.ToString();


            // Set the selected item of the font combobox if it contains the default font
            if (comboBoxFont.Items.Contains(defaultFont))
            {
                comboBoxFont.SelectedItem = defaultFont;
            }

            // Set the selected item of the size combobox if it contains the default size
            if (comboBoxSize.Items.Contains(defaultFontSizeString))
            {
                comboBoxSize.SelectedItem = defaultFontSizeString;
            }
        }

        // Toggles the font style (bold, italic, underline, strikeout) of the selected text
        private static void ToggleFontStyle(RichTextBox richTextBox, FontStyle style)
        {
            if (richTextBox.SelectionFont != null)
            {
                // Get the current style of the selected text
                FontStyle currentStyle = richTextBox.SelectionFont.Style;
                // Toggle the style
                FontStyle newStyle = currentStyle ^ style;
                // Apply the new style to the selected text
                richTextBox.SelectionFont = new Font(richTextBox.SelectionFont, newStyle);
            }
        }

        // Applies the font and size change from the comboboxes to the selected text
        private static void ApplyFontChange(RichTextBox richTextBox, ComboBox comboBoxFont, ComboBox comboBoxSize)
        {
            // Get the selected font name or use Arial if not set
            string fontName = comboBoxFont.SelectedItem?.ToString() ?? "Arial";
            // Get the selected font size or use 12 if not set
            float fontSize = float.TryParse(comboBoxSize.SelectedItem?.ToString(), out float size) ? size : 12f;
            // Get the current style of the selected text
            FontStyle fontStyle = richTextBox.SelectionFont?.Style ?? FontStyle.Regular;

            // Apply the new font and size to the selected text
            richTextBox.SelectionFont = new Font(fontName, fontSize, fontStyle);
        }

        // Changes the text color of the selected text using a color dialog
        private static void ChangeTextColor(RichTextBox richTextBox)
        {

            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = richTextBox.SelectionColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBox.SelectionColor = colorDialog.Color;
                }
            }
        }

        // Sets the text alignment (left, center, right) for the selected text
        private static void SetTextAlignment(RichTextBox richTextBox, HorizontalAlignment alignment)
        {
            int selectionStart = richTextBox.SelectionStart;
            int selectionLength = richTextBox.SelectionLength;

            richTextBox.SelectionAlignment = alignment;

            richTextBox.Select(selectionStart, selectionLength);
        }

        // Initializes the font and size comboboxes with available fonts and common sizes
        public static void InitializeComboBoxes(ComboBox comboBoxFont, ComboBox comboBoxSize)
        {
            // Populate the font dropdown with available fonts
            foreach (var fontFamily in FontFamily.Families)
            {
                comboBoxFont.Items.Add(fontFamily.Name);
            }
            comboBoxFont.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxFont.Height = 40;

            // Populate the size dropdown with common sizes
            comboBoxSize.Items.AddRange(new object[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 });
            comboBoxSize.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSize.Height = 40;

            // Set default selections if they exist
            if (comboBoxFont.Items.Contains("Arial"))
            {
                comboBoxFont.SelectedItem = "Arial";
            }
            if (comboBoxSize.Items.Contains("12"))
            {
                comboBoxSize.SelectedItem = "12";
            }
        }
    }
}
