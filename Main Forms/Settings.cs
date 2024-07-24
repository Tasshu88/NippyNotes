using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Nippy_Notes;
using Nippy_Notes.Security;

namespace Nippy_Notes
{
    public partial class Settings : Form
    {
        // Const for start up behavur option ID
        private const string startupOptionID = "startupBehavior";
        // Initial and selected startup behaviors as strings
        private string initialStartupBehavior;
        private string selectedStartupBehavior;
        // Const for place holder text in the TextBox
        private const string placeholderText = "Type Delete";
        private const string masterDeletePlaceholderText = "Type MasterDelete";
        // Const for seasonel effect option ID
        private const string seasonalOptionID = "seasonalEffect";
        // Initial seasonal efect as boolean
        private bool initialSeasonalEffect;
        // Event for when settings are applyed
        public event EventHandler SettingsApplied;
        // Refrence to the main form
        private readonly NippyNotes nippyNotesForm;
        // ToolTip to show tips when hovering over controls
        private ToolTip toolTipSettings;

        // Constructor for the settings form
        public Settings(NippyNotes nippyNotes)
        {
            InitializeComponent();
            this.nippyNotesForm = nippyNotes;
            this.Load += new EventHandler(Settings_Load);

            // Set up event handlers for various controls
            TextBoxEmptyProducts.Enter += TextBoxEmptyProducts_Enter;
            TextBoxEmptyProducts.Leave += TextBoxEmptyProducts_Leave;
            TextBoxEmptyProducts.TextChanged += TextBoxEmptyProducts_TextChanged;
            TextBoxMasterDeletion.Enter += TextBoxMasterDeletion_Enter;
            TextBoxMasterDeletion.Leave += TextBoxMasterDeletion_Leave;
            ChkBoxSeasonalOn.CheckedChanged += ChkBoxSeasonal_CheckedChanged;
            ChkBoxSeasonalOff.CheckedChanged += ChkBoxSeasonal_CheckedChanged;
            radioBtnLastOpenedOn.CheckedChanged += radioButtonLastOpened_CheckedChanged;
            radioBtnLastOpenedOff.CheckedChanged += radioButtonLastOpened_CheckedChanged;
            radioBtnNewBlankOn.CheckedChanged += radioButtonNewBlank_CheckedChanged;
            radioBtnNewBlankOff.CheckedChanged += radioButtonNewBlank_CheckedChanged;

            BtnBrowseManualDB.Click += BtnBrowseManualDB_Click;
            BtnManualProcessDB.Click += BtnManualProcessDB_Click;
            BtnAutoProcessDB.Click += BtnAutoProcessDB_Click;
            BtnRestoreDB.Click += BtnRestoreDB_Click;

            // Add the new delete button event handler
            BtnDelete.Click += BtnDelete_Click;

            // Set the placeholder text initially
            SetPlaceholderText();

            // Initialize the DataGridView for backups
            InitializeDataGridView();

            // Ensure the form is vissible on the screen
            FormUtilities.EnsureFormIsVisible(this);
        }

        // Load event for setting form, initialize tooltips and other settings
        private void Settings_Load(object sender, EventArgs e)
        {
            // Initialize tooltips
            toolTipSettings = new ToolTip();

            // Set up tooltips for each control
            toolTipSettings.SetToolTip(radioBtnLastOpenedOn, "Start with the last opened note.");
            toolTipSettings.SetToolTip(radioBtnLastOpenedOff, "Do not start with the last opened note.");
            toolTipSettings.SetToolTip(radioBtnNewBlankOn, "Start with a new blank note.");
            toolTipSettings.SetToolTip(radioBtnNewBlankOff, "Do not start with a new blank note.");
            toolTipSettings.SetToolTip(ChkBoxSeasonalOn, "Enable seasonal effects.");
            toolTipSettings.SetToolTip(ChkBoxSeasonalOff, "Disable seasonal effects.");
            toolTipSettings.SetToolTip(TextBoxEmptyProducts, "Delete all empty products.");
            toolTipSettings.SetToolTip(TextBoxMasterDeletion, "Master delete everything!");
            toolTipSettings.SetToolTip(ComboBoxFontSetting, "Select the default font.");
            toolTipSettings.SetToolTip(ComboBoxSizeSetting, "Select the default font size.");
            toolTipSettings.SetToolTip(BtnSettingsFormFeedback, "Send feedback.");
            toolTipSettings.SetToolTip(BtnReportBug, "Report a bug.");
            toolTipSettings.SetToolTip(BtnSecure, "Put a password on Nippy Notes.");
            toolTipSettings.SetToolTip(BtnDisableSecure, "Disable the password on Nippy Notes.");
            toolTipSettings.SetToolTip(BtnBrowseManualDB, "Select a directory for manual database backup.");
            toolTipSettings.SetToolTip(BtnManualProcessDB, "Process manual database backup.");
            toolTipSettings.SetToolTip(BtnAutoProcessDB, "Process automatic database backup.");
            toolTipSettings.SetToolTip(BtnUploadDB, "Upload a database file.");
            toolTipSettings.SetToolTip(BtnDelete, "Delete a selected database backup.");
            toolTipSettings.SetToolTip(BtnRestoreDB, "Restore a selected database backup.");

            // Populate font ComboBox
            foreach (FontFamily font in System.Drawing.FontFamily.Families)
            {
                ComboBoxFontSetting.Items.Add(font.Name);
            }

            // Populate size ComboBox
            for (int i = 8; i <= 72; i += 2)
            {
                ComboBoxSizeSetting.Items.Add(i.ToString());
            }

            // Load settings from the database
            ComboBoxFontSetting.SelectedItem = DatabaseHelper.GetFontSetting();
            ComboBoxSizeSetting.SelectedItem = DatabaseHelper.GetFontSizeSetting().ToString();

            // Load startup and seasonal options from the database
            DatabaseHelper.LoadStartupAndSeasonalOptions(out initialStartupBehavior, out initialSeasonalEffect);
            if (initialStartupBehavior == "LastOpened")
            {
                radioBtnLastOpenedOn.Checked = true;
            }
            else
            {
                radioBtnNewBlankOn.Checked = true;
            }
            selectedStartupBehavior = initialStartupBehavior;

            ChkBoxSeasonalOn.Checked = initialSeasonalEffect;
            ChkBoxSeasonalOff.Checked = !initialSeasonalEffect;

            // Initialize the graph panel and load backup history
            InitializeGraphPanel();
            LoadBackupHistory();
            SetMasterDeletePlaceholderText();

            // Ensure the form is visible on the screen
            FormUtilities.EnsureFormIsVisible(this);
            LoadBackupHistoryIntoGridView();
        }


        //Moved to RichTextBoxHelper
        private void InitializeFontComboBox()
        {
            foreach (FontFamily font in System.Drawing.FontFamily.Families)
            {
                ComboBoxFontSetting.Items.Add(font.Name);
            }

            // Set default font if available
            ComboBoxFontSetting.SelectedItem = "Calibri";
        }

        //Moved to RichTextBoxHelper
        private void InitializeSizeComboBox()
        {
            List<int> fontSizes = new List<int> { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            foreach (int size in fontSizes)
            {
                ComboBoxSizeSetting.Items.Add(size);
            }

            // Set default size
            ComboBoxSizeSetting.SelectedItem = 12;
        }


        // Handles change event for the last opened radio bautton
        private void radioButtonLastOpened_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnLastOpenedOn.Checked)
            {
                selectedStartupBehavior = "LastOpened";
                radioBtnNewBlankOff.Checked = true;
            }
            else
            {
                radioBtnNewBlankOn.Checked = true;
            }
        }

        // Handles change event for the new blank radio button
        private void radioButtonNewBlank_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnNewBlankOn.Checked)
            {
                selectedStartupBehavior = "NewBlank";
                radioBtnLastOpenedOff.Checked = true;
            }
            else
            {
                radioBtnLastOpenedOn.Checked = true;
            }
        }

        // Handles the change event for seasonal checkbox
        private void ChkBoxSeasonal_CheckedChanged(object sender, EventArgs e)
        {
            DatabaseHelper.UpdateOptionValue(seasonalOptionID, ChkBoxSeasonalOn.Checked ? "true" : "false");

            // Apply the changes in the main form
            if (Owner is NippyNotes mainForm)
            {
                mainForm.UpdateSeasonalEffects(ChkBoxSeasonalOn.Checked);
            }
        }

        // Handles the apply settings button click event
        private void BtnSettingsApply_Click(object sender, EventArgs e)
        {
            bool changesMade = false;

            if (selectedStartupBehavior != initialStartupBehavior)
            {
                DatabaseHelper.UpdateOptionValue(startupOptionID, selectedStartupBehavior);
                Logger.LogActivity("Update", $"Startup behavior set to {selectedStartupBehavior}.");
                changesMade = true;
                initialStartupBehavior = selectedStartupBehavior; // Update initial state
            }

            if (ChkBoxSeasonalOn.Checked != initialSeasonalEffect)
            {
                DatabaseHelper.UpdateOptionValue(seasonalOptionID, ChkBoxSeasonalOn.Checked ? "true" : "false");
                Logger.LogActivity("Update", $"Seasonal effects set to {ChkBoxSeasonalOn.Checked}");

                if (Owner is NippyNotes mainForm)
                {
                    mainForm.UpdateSeasonalEffects(ChkBoxSeasonalOn.Checked);
                }

                initialSeasonalEffect = ChkBoxSeasonalOn.Checked;
                changesMade = true;
            }

            // Update font settings
            string selectedFont = ComboBoxFontSetting.SelectedItem?.ToString();
            string selectedFontSize = ComboBoxSizeSetting.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedFont) && !string.IsNullOrEmpty(selectedFontSize))
            {
                DatabaseHelper.InsertOrUpdateOption("defaultFont", selectedFont, "Default font for notes");
                DatabaseHelper.InsertOrUpdateOption("defaultFontSize", selectedFontSize, "Default font size for notes");

                changesMade = true;

                // I'll Notify the main form of the changes
                if (Owner is NippyNotes mainForm)
                {
                    mainForm.SetDefaultFontInComboboxes();
                }
            }

            if (TextBoxEmptyProducts.Text.Trim().Equals("Delete", StringComparison.OrdinalIgnoreCase))
            {
                DeleteEmptyProductsAndSubcategories();
                changesMade = true;
            }

            if (TextBoxMasterDeletion.Text.Trim().Equals("MasterDelete", StringComparison.OrdinalIgnoreCase))
            {
                var result = MessageBox.Show("Are you sure you want to completely delete all notes, products, and subcategories? This action cannot be undone.", "Confirm Master Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    DeleteAllData();
                    changesMade = true;
                }
                TextBoxMasterDeletion.Text = string.Empty; // Clear the textbox after operation
            }

            if (changesMade)
            {
                MessageBox.Show("Settings applied successfully.");
                OnSettingsApplied(EventArgs.Empty);

                // Apply the startup behavior immediately
                ApplyStartupBehavior();
            }
            else
            {
                MessageBox.Show("No changes were made to apply.");
            }

            this.Close();
        }

        // Deletes all data from the database
        private void DeleteAllData()
        {
            try
            {
                DatabaseHelper.DeleteAllData();
                Logger.LogActivity("Delete", "Deleted all notes, products, and subcategories.");
                MessageBox.Show("All notes, products, and subcategories have been deleted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting data: {ex.Message}", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Triggers the SettingsApplied event
        protected virtual void OnSettingsApplied(EventArgs e)
        {
            SettingsApplied?.Invoke(this, e);
        }

        // Applies the selected startup behavior
        private void ApplyStartupBehavior()
        {
            if (Owner is NippyNotes mainForm)
            {
                if (selectedStartupBehavior == "LastOpened")
                {
                    mainForm.LoadLastOpenedNote();
                }
                else
                {
                    mainForm.ClearForm(true);
                    mainForm.RefreshDate();
                }
                // Clear the page amount label
                mainForm.LblPageAmount.Text = string.Empty;
            }
        }

        // Event handler for when the empty products TextBox is entered
        private void TextBoxEmptyProducts_Enter(object sender, EventArgs e)
        {
            if (TextBoxEmptyProducts.Text == placeholderText)
            {
                TextBoxEmptyProducts.Text = "";
                TextBoxEmptyProducts.ForeColor = Color.Black;
            }
        }

        // Event handler for when the empty products TextBox loses focus
        private void TextBoxEmptyProducts_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxEmptyProducts.Text))
            {
                SetPlaceholderText();
            }
        }

        // Event handler for changes in the empty products TextBox
        private void TextBoxEmptyProducts_TextChanged(object sender, EventArgs e)
        {
            
        }

        // Sets the placeholder text for the empty products TextBox
        private void SetPlaceholderText()
        {
            TextBoxEmptyProducts.Text = placeholderText;
            TextBoxEmptyProducts.ForeColor = Color.Gray;
        }

        // Event handler for when the master deletion TextBox is entered - REMOVE ALL
        private void TextBoxMasterDeletion_Enter(object sender, EventArgs e)
        {
            if (TextBoxMasterDeletion.Text == masterDeletePlaceholderText)
            {
                TextBoxMasterDeletion.Text = "";
                TextBoxMasterDeletion.ForeColor = Color.Black;
            }
        }

        // Event handler for when the master deletion TextBox loses focus
        private void TextBoxMasterDeletion_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxMasterDeletion.Text))
            {
                SetMasterDeletePlaceholderText();
            }
        }

        // Sets the placeholder text for the master deletion TextBox
        private void SetMasterDeletePlaceholderText()
        {
            TextBoxMasterDeletion.Text = masterDeletePlaceholderText;
            TextBoxMasterDeletion.ForeColor = Color.Gray;
        }

        // Deletes empty products and subcategories
        private void DeleteEmptyProductsAndSubcategories()
        {
            try
            {
                DatabaseHelper.DeleteEmptyProductsAndSubcategories();

                // Refresh the main form to reflect the changes
                if (Owner is NippyNotes mainForm)
                {
                    mainForm.RefreshNotesList();
                }

                MessageBox.Show("Deleted all orphaned products/subcategories.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting orphaned products/subcategories: {ex.Message}", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Event handler for browsing manual database backup directory
        private void BtnBrowseManualDB_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    TextBoxManualDir.Text = folderDialog.SelectedPath;
                }
            }
        }

        // Event handler for processing manual database backup 
        private void BtnManualProcessDB_Click(object sender, EventArgs e)
        {
            string backupDirectory = TextBoxManualDir.Text;
            if (!string.IsNullOrEmpty(backupDirectory) && Directory.Exists(backupDirectory))
            {
                try
                {
                    DatabaseHelper.ManualProcessDB(backupDirectory);
                    MessageBox.Show("Database backed up successfully.", "Backup Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshBackupHistory(); // Refresh backup history immediately
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Backup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a valid backup directory.", "Invalid Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Event handler for processing automatic database backup - it inserts into APPDATA
        private void BtnAutoProcessDB_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseHelper.AutoProcessDB();
                MessageBox.Show("Database backed up successfully.", "Backup Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshBackupHistory(); // Refresh backup history immediately
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Backup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load backup history into the DataGridView
        private void LoadBackupHistoryIntoGridView()
        {
            var backupRecords = BackupHistoryManager.LoadBackupHistory();
            dataGridViewBackups.Rows.Clear();
            foreach (var record in backupRecords)
            {
                dataGridViewBackups.Rows.Add(record.BackupName, record.Location, record.FilePath, record.Date);
            }
        }

        // Event handler for restoring database from a backup
        private void BtnRestoreDB_Click(object sender, EventArgs e)
        {
            if (dataGridViewBackups.SelectedRows.Count > 0)
            {
                string selectedBackupFile = dataGridViewBackups.SelectedRows[0].Cells["FilePath"].Value.ToString();

                try
                {
                    DatabaseHelper.RestoreDB(selectedBackupFile);
                    MessageBox.Show("Database restored successfully.", "Restore Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DatabaseHelper.LoadBackupHistory(dataGridViewBackups);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Restore Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a backup file to restore.", "No Backup Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Loads the backup history
        private void LoadBackupHistory()
        {
            try
            {
                DatabaseHelper.LoadBackupHistory(dataGridViewBackups);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading backup history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshBackupHistory()
        {
            LoadBackupHistoryIntoGridView();
        }

        // Initializes the DataGridView for backups
        private void InitializeDataGridView()
        {
            dataGridViewBackups.Columns.Add("BackupName", "Backup Name");
            dataGridViewBackups.Columns.Add("Location", "Location");
            dataGridViewBackups.Columns.Add("FilePath", "File Path");
            dataGridViewBackups.Columns.Add("Date", "Date");

            dataGridViewBackups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewBackups.MultiSelect = false; // Ensure only one row can be selected at a time
            dataGridViewBackups.CellClick += DataGridViewBackups_CellClick;
        }

        // Handles cell click event for the backup DataGridView
        private void DataGridViewBackups_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridViewBackups.Rows[e.RowIndex].Selected = true;
            }
        }

        // Initializes the graph panel
        private void InitializeGraphPanel()
        {
            PanelGraph.Controls.Clear(); // Clear any existing controls

            int notesCount = GetCountFromDatabase("Notes");
            int productsCount = GetCountFromDatabase("Products");
            int subcategoriesCount = GetCountFromDatabase("Subcategories");

            Chart pieChart = new Chart();

            // Set the size of the chart to match the panel
            pieChart.Size = new Size(PanelGraph.Width, PanelGraph.Height);
            pieChart.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            ChartArea chartArea = new ChartArea();
            pieChart.ChartAreas.Add(chartArea);

            Series series = new Series
            {
                Name = "Data",
                ChartType = SeriesChartType.Pie
            };
            pieChart.Series.Add(series);

            series.Points.Add(new DataPoint(0, notesCount) { AxisLabel = "Notes", LegendText = "Notes" });
            series.Points.Add(new DataPoint(0, productsCount) { AxisLabel = "Products", LegendText = "Products" });
            series.Points.Add(new DataPoint(0, subcategoriesCount) { AxisLabel = "Subcategories", LegendText = "Subcategories" });

            series.Points[0].Label = $"{notesCount}";
            series.Points[1].Label = $"{productsCount}";
            series.Points[2].Label = $"{subcategoriesCount}";

            series["PieLabelStyle"] = "Outside";
            series.BorderColor = Color.Black;
            series.BorderWidth = 1;

            pieChart.Legends.Add(new Legend("Legend") { Docking = Docking.Right, Alignment = StringAlignment.Center });
            pieChart.Titles.Add(new Title("Notes, Products, and Subcategories", Docking.Top, new Font("Verdana", 12), Color.Black));

            PanelGraph.Controls.Add(pieChart);
            pieChart.BringToFront(); // Ensure the chart is at the front
        }


        // Gets count of records from the database
        private int GetCountFromDatabase(string tableName)
        {
            try
            {
                return DatabaseHelper.GetCountFromDatabase(tableName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while counting records in {tableName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        // Closes the settings form
        private void BtnCloseSettings_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Handles database upload button click event
        private void BtnUploadDB_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "SQLite Database files (*.db)|*.db";
                openFileDialog.Title = "Select Database File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;

                    try
                    {
                        DatabaseHelper.UploadDB(selectedFilePath, dataGridViewBackups);
                        MessageBox.Show("Database file uploaded and added to backup history.", "Upload Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Upload Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //Doesn't work, this deletes more than one backup (manual and an auto) how will this integrate into Task Schedular?!
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewBackups.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewBackups.SelectedRows[0];
                var filePath = selectedRow.Cells["FilePath"].Value?.ToString();
                //it's deleting all rows and not the one I'm selecting
                var backupName = selectedRow.Cells["BackupName"].Value?.ToString();
                var location = selectedRow.Cells["location"].Value?.ToString();
                var date = selectedRow.Cells["date"].Value?.ToString();

                //debug if fails
                Console.WriteLine($"Selected backupd for deletion: BackupName = {backupName}, Location = {location}, FilePath = {filePath}, Date = {date}");


                try
                {
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        // Delete the file and update the backup history
                        BackupHistoryManager.DeleteBackup(filePath);
                        dataGridViewBackups.Rows.Remove(selectedRow); // Remove the selected row from DataGridView
                        MessageBox.Show("Backup file deleted successfully.", "Delete Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        throw new Exception("The selected backup file path is not valid.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a backup file to delete.", "No Backup Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Opens the feedback form
        private void BtnSettingsFormFeedback_Click(object sender, EventArgs e)
        {
            FeedbackForm feedbackForm = new FeedbackForm();
            feedbackForm.Show();
        }



        // Opens the bug report form
        private void BtnReportBug_Click(object sender, EventArgs e)
        {
            BugForm bugForm = new BugForm();
            bugForm.Show();
        }

        // Handles the secure button click event (Password )
        private void BtnSecure_Click(object sender, EventArgs e)
        {
            string storedPasswordHash = GetStoredPasswordHash();

            if (string.IsNullOrEmpty(storedPasswordHash))
            {
                // First-time setup: ask the user to create a password
                using (var passwordForm = new PasswordSetupForm())
                {
                    if (passwordForm.ShowDialog() == DialogResult.OK)
                    {
                        string newPassword = passwordForm.EnteredPassword;
                        string newPasswordHash = SecurityHelper.HashPassword(newPassword);
                        SavePasswordHashToDatabase(newPasswordHash);
                        MessageBox.Show("Password set successfully.");
                     
                    }
                }
            }
            else
            {
                // Ask for the password without requiring the memorable word
                using (var passwordForm = new PasswordEntryFormSecure())
                {
                    if (passwordForm.ShowDialog() == DialogResult.OK)
                    {
                        string enteredPassword = passwordForm.EnteredPassword;
                        if (SecurityHelper.VerifyPassword(enteredPassword, storedPasswordHash))
                        {
                            
                        }
                        else
                        {
                            MessageBox.Show("Incorrect password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        //Moved to DatabaseHelper - currently got an issue
        private void SavePasswordAndWordToDatabase(string passwordHash, string memorableWord)
        {
            DatabaseHelper.SavePasswordAndWordToDatabase(passwordHash, memorableWord);
        }

        //Moved to DatabaseHelper
        private void RemovePasswordAndWordFromDatabase()
        {
            DatabaseHelper.RemovePasswordAndWordFromDatabase();
        }

        //Moved to DatabaseHelper
        private string GetStoredPasswordHash()
        {
            return DatabaseHelper.GetStoredPasswordHash();
        }

        //Moved to DatabaseHelper
        private void SavePasswordHashToDatabase(string passwordHash)
        {
            DatabaseHelper.SavePasswordHashToDatabase(passwordHash);
        }

        //Moved to DatabaseHelper
        private string GetStoredMemorableWord()
        {
            return DatabaseHelper.GetStoredMemorableWord();
        }


        // Handles disable secure button click event (no password)
        private void BtnDisableSecure_Click(object sender, EventArgs e)
        {
            string storedPasswordHash = GetStoredPasswordHash();
            string storedMemorableWordHash = GetStoredMemorableWord();

            if (!string.IsNullOrEmpty(storedPasswordHash))
            {
                // Ask for the password and memorable word
                using (var passwordForm = new PasswordEntryFormDisableSecure(true))
                {
                    if (passwordForm.ShowDialog() == DialogResult.OK)
                    {
                        string enteredPassword = passwordForm.EnteredPassword;
                        string enteredMemorableWord = passwordForm.EnteredMemorableWord;

                        if (SecurityHelper.VerifyPassword(enteredPassword, storedPasswordHash) && SecurityHelper.VerifyPassword(enteredMemorableWord, storedMemorableWordHash))
                        {
                            var confirmResult = MessageBox.Show("Are you sure you want to disable security? This will remove your password and memorable word.", "Confirm Disable Security", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (confirmResult == DialogResult.Yes)
                            {
                                RemovePasswordAndWordFromDatabase();
                                MessageBox.Show("Security disabled successfully.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Incorrect password or memorable word. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Security is not enabled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


    }
}
