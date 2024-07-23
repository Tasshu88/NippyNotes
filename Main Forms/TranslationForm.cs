using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Nippy_Notes
{
    public partial class TranslationForm : Form
    {
        public bool IsNewTranslation { get; private set; }
        public string SelectedTranslationId { get; private set; }
        public string TranslationValue { get; private set; }

        private string translationType;
        private string sourceValue;

        public TranslationForm(string sourceValue, string translationType, string productName, string subcategoryName, string productId = null)
        {
            InitializeComponent();
            Text = $"Translate {translationType}: {sourceValue}";
            instructionLabel.Text = $"Translate {translationType}: {sourceValue}";
            this.translationType = translationType;
            this.sourceValue = sourceValue;
            this.StartPosition = FormStartPosition.CenterParent;

            // Initialize radio buttons to unchecked
            newRadioButton.Checked = false;
            existingRadioButton.Checked = false;

            
            newRadioButton.CheckedChanged += newRadioButton_CheckedChanged;
            existingRadioButton.CheckedChanged += existingRadioButton_CheckedChanged;
            existingTranslationsListBox.MouseDown += ExistingTranslationsListBox_MouseDown;
            this.Load += TranslationForm_Load;

            // Load translations based on type and potentially a specific product
            LoadExistingTranslations(productId);

            // Adjust label visibility and content based on translation type
            if (translationType == "Product")
            {
                // LblExistingProductSubcategory.Visible = false; // Hide label when translating a product
            }
            else if (translationType == "Subcategory")
            {
                //  LblExistingProductSubcategory.Visible = true;
                //  LblExistingProductSubcategory.Text = $"Product: {productName}"; // Display associated product name when translating a subcategory
            }

            FormUtilities.EnsureFormIsVisible(this); // Ensure form is visible on load
        }

        private void LoadExistingTranslations(string productId = null)
        {
            try
            {
                var translations = DatabaseHelper.LoadExistingTranslations(translationType, productId);
                existingTranslationsListBox.Items.Clear();
                foreach (var translation in translations)
                {
                    existingTranslationsListBox.Items.Add(new KeyValuePair<string, string>(translation.Key, translation.Value));
                }
                existingTranslationsListBox.DisplayMember = "Value";
                existingTranslationsListBox.ValueMember = "Key";
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExistingTranslationsListBox_MouseDown(object sender, MouseEventArgs e)
        {
            int index = existingTranslationsListBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                existingRadioButton.Checked = true;
            }
            else
            {
                this.ActiveControl = null;
            }
        }

        private void newRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (newRadioButton.Checked)
            {
                var result = MessageBox.Show("Do you want to keep the received translation?", "Keep Translation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    newTranslationTextBox.Text = sourceValue;
                }
                else
                {
                    newTranslationTextBox.Text = string.Empty;
                }
            }
        }

        private void existingRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (existingRadioButton.Checked)
            {
                newTranslationTextBox.Text = string.Empty;
            }
        }

        private void existingTranslationsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (existingTranslationsListBox.SelectedItem != null)
            {
                newTranslationTextBox.Text = string.Empty;
                existingRadioButton.Checked = true;
            }
        }

        private string EnsureTranslation(string translationType, string sourceValue, string databaseId)
        {
            string result = DatabaseHelper.EnsureTranslation(translationType, sourceValue, databaseId);
            if (result != null)
            {
                return result;
            }

            using (var translationForm = new TranslationForm(sourceValue, translationType, "", ""))
            {
                if (translationForm.ShowDialog() == DialogResult.OK)
                {
                    if (translationForm.IsNewTranslation)
                    {
                        return DatabaseHelper.CreateTranslation(translationType, translationForm.TranslationValue);
                    }
                    else
                    {
                        return translationForm.SelectedTranslationId;
                    }
                }
            }
            return null;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (newRadioButton.Checked)
            {
                IsNewTranslation = true;
                TranslationValue = newTranslationTextBox.Text;
            }
            else if (existingRadioButton.Checked && existingTranslationsListBox.SelectedItem != null)
            {
                IsNewTranslation = false;
                var selected = (KeyValuePair<string, string>)existingTranslationsListBox.SelectedItem;
                SelectedTranslationId = selected.Key;
                TranslationValue = selected.Value;
            }
            else
            {
                MessageBox.Show("Please select a translation option.");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
            Logger.LogActivity("Translate", $"Translation for {sourceValue} added/updated to {TranslationValue}");
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel the translation?", "Cancel Translation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void TranslationForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            FormUtilities.EnsureFormIsVisible(this); 
        }
    }
}
