using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static Nippy_Notes.NippyNotes;

namespace Nippy_Notes
{
    public partial class SearchForm : Form
    {
        private int expandedWidth = 1180;
        private int expandedHeight = 591;
        private ToolTip toolTipSearchForm;

        private NippyNotes nippyNotesForm;

        public string SelectedProduct { get; set; } // Public property to hold the selection
        public string SelectedSubcategory { get; set; }
        public int NotesCount { get; set; } = 0;

        private ToolTip toolTipSearchFormResult;

        public event EventHandler NoteSelected;



        public SearchForm(NippyNotes nippyNotes)
        {
            InitializeComponent();
            InitializeCustomComponents();
            this.Size = new Size(expandedWidth, expandedHeight);  // Set the form to the expanded size

            toolTipSearchFormResult = new ToolTip(); // Initialize ToolTip control
            toolTipSearchFormResult.AutoPopDelay = 5000;
            toolTipSearchFormResult.InitialDelay = 1000;
            toolTipSearchFormResult.ReshowDelay = 500;
            toolTipSearchFormResult.ShowAlways = true;

            DateTimePickerFrom.ValueChanged += DateTimePickerFrom_ValueChanged;
            DateTimePickerTo.ValueChanged += DateTimePickerTo_ValueChanged;

            // Initialize panels to be hidden on startup
            MidPanel.Visible = false;
            LowerPanel.Visible = false;

            // Add event handlers for checkboxes
            CheckBoxAdvanced.CheckedChanged += CheckBoxAdvanced_CheckedChanged;
            CheckBoxDate.CheckedChanged += CheckBoxDate_CheckedChanged;

            ComboBoxProductSearchSearchForm.SelectedIndexChanged += ComboBoxProductSearch_SelectedIndexChanged;
            ComboBoxSubcategorySearchSearchForm.SelectedIndexChanged += ComboBoxSubcategorySearch_SelectedIndexChanged; // Add event handler for subcategory selection change
            ComboBoxExtensionSearchSearchForm.SelectedIndexChanged += ComboBoxExtensionSearch_SelectedIndexChanged; // Add event handler for extension selection change
            ComboBoxSearchFormKeyword.SelectedIndexChanged += ComboBoxSearchFormKeyword_SelectedIndexChanged; // Add event handler for keyword selection change

            UpdateShowAllNotesButtonState(); // Initial state of the button

            this.nippyNotesForm = nippyNotes;
            this.Owner = nippyNotes;
            Logger.LogActivity("Open", $"Notes opened from SearchForm.");

            FormUtilities.EnsureFormIsVisible(this); // Ensure form is visible on load

            // Set the property to remove the last empty row
            dataGridViewShowAllNotes.AllowUserToAddRows = false;

            TextBoxQuickSearch.TextChanged += TextBoxQuickSearch_TextChanged;
        }


        private void CheckBoxAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            MidPanel.Visible = CheckBoxAdvanced.Checked;
            UpdateDataGridView(); // Update DataGridView based on new filters
        }

        private void CheckBoxDate_CheckedChanged(object sender, EventArgs e)
        {
            LowerPanel.Visible = CheckBoxDate.Checked;
            UpdateDataGridView(); // Update DataGridView based on new filters
        }

        protected virtual void OnNoteSelected(EventArgs e)
        {
            NoteSelected?.Invoke(this, e);
        }

        private void InitializeCustomComponents()
        {
            // Initialize other components
        }

        private void TextBoxQuickSearch_TextChanged(object sender, EventArgs e)
        {
            UpdateDataGridView();

            if (dataGridViewShowAllNotes.Rows.Count == 1)
            {
                DataGridViewRow row = dataGridViewShowAllNotes.Rows[0];
                string details = row.Cells["Details"].Value.ToString();
                string searchText = TextBoxQuickSearch.Text;
                string matchingLine = FindMatchingLine(details, searchText);

                if (!string.IsNullOrEmpty(matchingLine))
                {
                    ShowTooltip(row, matchingLine);
                }
            }
            else
            {
                toolTipSearchFormResult.Hide(dataGridViewShowAllNotes);
            }
        }



        private string FindMatchingLine(string details, string searchText)
        {
            using (StringReader reader = new StringReader(details))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(searchText))
                    {
                        return line;
                    }
                }
            }
            return string.Empty;
        }

        private void ShowTooltip(DataGridViewRow row, string text)
        {
            Rectangle cellRect = dataGridViewShowAllNotes.GetCellDisplayRectangle(row.Index, 0, true);
            Point cellLocation = new Point(cellRect.X + dataGridViewShowAllNotes.Location.X, cellRect.Y + dataGridViewShowAllNotes.Location.Y);
            toolTipSearchFormResult.Show(text, dataGridViewShowAllNotes, cellLocation.X, cellLocation.Y - 20, 5000); // Display tooltip for 5 seconds
        }

        private void PopulateKeywordsComboBoxSearchForm()
        {
            DatabaseHelper.PopulateKeywordsComboBoxSearchForm(ComboBoxSearchFormKeyword);
        }

        private DataTable GetFilteredNotes()
        {
            string selectedProduct = ComboBoxProductSearchSearchForm.SelectedItem?.ToString();
            string selectedSubcategory = ComboBoxSubcategorySearchSearchForm.SelectedItem?.ToString();
            string selectedExtension = ComboBoxExtensionSearchSearchForm.SelectedItem?.ToString();
            string selectedKeyword = ComboBoxSearchFormKeyword.SelectedItem?.ToString();
            bool filterByExtension = selectedExtension != null && selectedExtension != "All";
            bool filterByKeyword = selectedKeyword != null && selectedKeyword != "All";
            bool filterByDate = CheckBoxDate.Checked;

            string quickSearchText = TextBoxQuickSearch.Text;
            if (!string.IsNullOrEmpty(quickSearchText))
            {
                return DatabaseHelper.GetQuickSearchNotes(quickSearchText);
            }

            return DatabaseHelper.GetFilteredNotesSearchForm(
                selectedProduct, selectedSubcategory, selectedExtension, selectedKeyword,
                filterByExtension, filterByKeyword, filterByDate, DateTimePickerFrom.Value.Date, DateTimePickerTo.Value.Date
            );
        }

        private void DateTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            UpdateDataGridView();
        }

        private void DateTimePickerTo_ValueChanged(object sender, EventArgs e)
        {
            UpdateDataGridView();
        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
            PopulateProductComboBox();
            PopulateFileExtensionComboBox();
            PopulateKeywordsComboBoxSearchForm();
            FormUtilities.EnsureFormIsVisible(this); // Ensure form is visible on load

            // Initialize tooltips
            toolTipSearchForm = new ToolTip();

            // Set up tooltips for each control
            toolTipSearchForm.SetToolTip(ComboBoxProductSearchSearchForm, "Search by product.");
            toolTipSearchForm.SetToolTip(ComboBoxSubcategorySearchSearchForm, "Search by subcategory.");
            toolTipSearchForm.SetToolTip(CheckBoxAdvanced, "Enable or disable advanced filtering.");
            toolTipSearchForm.SetToolTip(DateTimePickerFrom, "Filter by start date.");
            toolTipSearchForm.SetToolTip(DateTimePickerTo, "Filter by end date.");
            toolTipSearchForm.SetToolTip(CheckBoxDate, "Enable or disable date filtering.");
            toolTipSearchForm.SetToolTip(ComboBoxSearchFormKeyword, "Search by keyword.");
            toolTipSearchForm.SetToolTip(ComboBoxExtensionSearchSearchForm, "Search by file extension.");

            btnShowAllNotesSearchForm.Enabled = true;

            // Fetch and display all notes on load
            DataTable notesTable = DatabaseHelper.GetAllNotesSearchForm();
            dataGridViewShowAllNotes.DataSource = notesTable;
            ConfigureDataGridViewColumns();
        }

        private void PopulateProductComboBox()
        {
            DatabaseHelper.PopulateProductComboBoxSearchForm(ComboBoxProductSearchSearchForm);
        }

        private void ComboBoxSearchFormKeyword_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDataGridView();
        }

        private void PopulateFileExtensionComboBox()
        {
            DatabaseHelper.PopulateFileExtensionComboBoxSearchForm(ComboBoxExtensionSearchSearchForm);
        }

        private void ComboBoxExtensionSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDataGridView();
        }

        private void ComboBoxProductSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateSubcategoryComboBox();
            UpdateShowAllNotesButtonState();
            UpdateDataGridView();
        }

        private void ComboBoxSubcategorySearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDataGridView();
        }

        private void PopulateSubcategoryComboBox()
        {
            string selectedProduct = ComboBoxProductSearchSearchForm.SelectedItem?.ToString();
            DatabaseHelper.PopulateSubcategoryComboBoxSearchForm(ComboBoxSubcategorySearchSearchForm, selectedProduct);
        }

        private void UpdateShowAllNotesButtonState()
        {
            btnShowAllNotesSearchForm.Enabled = ComboBoxProductSearchSearchForm.SelectedItem != null;
        }

        public void InitializeForm(string selectedProduct, string selectedSubcategory)
        {
            ComboBoxProductSearchSearchForm.SelectedItem = selectedProduct;

            // Check if the selected product has subcategories
            if (!string.IsNullOrEmpty(selectedSubcategory))
            {
                PopulateSubcategoryComboBox(); // Make sure this function handles the current selection
                ComboBoxSubcategorySearchSearchForm.SelectedItem = selectedSubcategory;
            }
        }

        private void UpdateDataGridView()
        {
            dataGridViewShowAllNotes.AllowUserToAddRows = false; // Set this before setting the DataSource
            DataTable notesTable = GetFilteredNotes();
            dataGridViewShowAllNotes.DataSource = notesTable;
            ConfigureDataGridViewColumns();
            dataGridViewShowAllNotes.Refresh(); // Refresh the DataGridView
        }

        private void ConfigureDataGridViewColumns()
        {
            dataGridViewShowAllNotes.Columns["NoteID"].Visible = false;
            dataGridViewShowAllNotes.Columns["NoteNumber"].HeaderText = "Note Number";
            dataGridViewShowAllNotes.Columns["AddedDate"].HeaderText = "Added Date";
            dataGridViewShowAllNotes.Columns["ProductName"].HeaderText = "Product Name";
            dataGridViewShowAllNotes.Columns["SubcategoryName"].HeaderText = "Subcategory Name";
            dataGridViewShowAllNotes.Columns["Subject"].HeaderText = "Subject";
            dataGridViewShowAllNotes.Columns["Keywords"].HeaderText = "Keywords"; // Set the header text for Keywords column
            dataGridViewShowAllNotes.Columns["FileExtensions"].HeaderText = "File Extensions"; // Set the header text for File Extensions column

            // Hide the Details column if it exists
            if (dataGridViewShowAllNotes.Columns.Contains("Details"))
            {
                dataGridViewShowAllNotes.Columns["Details"].Visible = false;
            }
        }

        private void btnSearchNoteSearcForm_Click(object sender, EventArgs e)
        {
            DataTable notesTable = GetFilteredNotes();
            List<Note> notes = new List<Note>();

            foreach (DataRow row in notesTable.Rows)
            {
                Note note = new Note
                {
                    NoteID = row["NoteID"].ToString(),
                    NoteNumber = Convert.ToInt32(row["NoteNumber"]),
                    AddedDate = Convert.ToDateTime(row["AddedDate"]),
                    Subject = row["Subject"].ToString(),
                    Details = row["Details"].ToString(), // Ensure Details is included
                    ProductID = DatabaseHelper.GetProductIdByNameSearchForm(row["ProductName"].ToString()),
                    SubcategoryID = DatabaseHelper.GetSubcategoryIdByNameSearchForm(row["SubcategoryName"].ToString(), DatabaseHelper.GetProductIdByNameSearchForm(row["ProductName"].ToString()))
                };
                notes.Add(note);
            }

            if (notes.Count > 0)
            {
                if (Owner is NippyNotes mainForm)
                {
                    mainForm.OpenSelectedNotes(notes);
                    if (mainForm.wasMaximizedBeforeSearch)
                    {
                        mainForm.WindowState = FormWindowState.Maximized;
                    }
                }
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("No notes found.");
                this.DialogResult = DialogResult.Cancel;
            }
            this.Close();
        }

        private List<Note> FetchNotes(string product)
        {
            return DatabaseHelper.FetchNotesSearchForm(product);
        }

        private string GetProductIdByName(string productName)
        {
            return DatabaseHelper.GetProductIdByNameSearchForm(productName);
        }

        private string GetSubcategoryIdByName(string subcategoryName, string productId)
        {
            return DatabaseHelper.GetSubcategoryIdByNameSearchForm(subcategoryName, productId);
        }

        private bool IsProductEmpty(string productId)
        {
            return DatabaseHelper.IsProductEmptySearchForm(productId);
        }

        private string GetProductNameById(string productId)
        {
            return DatabaseHelper.GetProductNameByIdSearchForm(productId);
        }

        private void DeleteProduct(string productId)
        {
            DatabaseHelper.DeleteProductSearchForm(productId);
        }

        private void DeleteNoteAndRefresh(string noteId)
        {
            DatabaseHelper.DeleteNoteAndRefreshSearchForm(noteId);
        }

        private void btnOpenNotesSearchForm_Click(object sender, EventArgs e)
        {
            if (dataGridViewShowAllNotes.SelectedRows.Count > 0)
            {
                List<Note> selectedNotes = new List<Note>();
                foreach (DataGridViewRow row in dataGridViewShowAllNotes.SelectedRows)
                {
                    // Skip rows that are blank or contain null values
                    if (row.Cells["NoteID"].Value == null ||
                        row.Cells["NoteNumber"].Value == null ||
                        row.Cells["AddedDate"].Value == null ||
                        row.Cells["Subject"].Value == null ||
                        row.Cells["ProductName"].Value == null ||
                        row.Cells["SubcategoryName"].Value == null)
                    {
                        continue;
                    }

                    Note note = new Note
                    {
                        NoteID = row.Cells["NoteID"].Value.ToString(),
                        NoteNumber = Convert.ToInt32(row.Cells["NoteNumber"].Value),
                        AddedDate = Convert.ToDateTime(row.Cells["AddedDate"].Value),
                        Subject = row.Cells["Subject"].Value.ToString(),
                        Details = row.Cells["Details"].Value.ToString(), // Ensure Details is included
                        ProductID = GetProductIdByName(row.Cells["ProductName"].Value.ToString()),
                        SubcategoryID = GetSubcategoryIdByName(row.Cells["SubcategoryName"].Value.ToString(), GetProductIdByName(row.Cells["ProductName"].Value.ToString()))
                    };
                    selectedNotes.Add(note);
                }

                if (selectedNotes.Count > 0)
                {
                    if (Owner is NippyNotes mainForm)
                    {
                        mainForm.OpenSelectedNotes(selectedNotes.OrderBy(note => note.NoteNumber).ToList());
                        if (mainForm.wasMaximizedBeforeSearch)
                        {
                            mainForm.WindowState = FormWindowState.Maximized;
                        }
                    }
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("No valid notes selected.");
                    this.DialogResult = DialogResult.Cancel;
                }
            }
            else
            {
                MessageBox.Show("No notes selected.");
                this.DialogResult = DialogResult.Cancel;
            }
            this.Close();
        }

        public Note GetSelectedNote()
        {
            if (dataGridViewShowAllNotes.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridViewShowAllNotes.SelectedRows[0];
                return new Note
                {
                    NoteID = row.Cells["NoteID"].Value.ToString(),
                    NoteNumber = Convert.ToInt32(row.Cells["NoteNumber"].Value),
                    AddedDate = Convert.ToDateTime(row.Cells["AddedDate"].Value),
                    Subject = row.Cells["Subject"].Value.ToString(),
                    Details = row.Cells["Details"].ToString(),
                    ProductID = GetProductIdByName(row.Cells["ProductName"].Value.ToString()),
                    SubcategoryID = GetSubcategoryIdByName(row.Cells["SubcategoryName"].Value.ToString(), GetProductIdByName(row.Cells["ProductName"].Value.ToString()))
                };
            }
            return null;
        }

        private List<Note> FetchNotesByNumbers(List<int> noteNumbers)
        {
            return DatabaseHelper.FetchNotesByNumbersSearchForm(noteNumbers);
        }

        private void btnDeleteSearcForm_Click(object sender, EventArgs e)
        {
            if (dataGridViewShowAllNotes.SelectedRows.Count == 0)
            {
                MessageBox.Show("No notes selected.");
                return;
            }

            var notesToDelete = new List<Note>();
            foreach (DataGridViewRow row in dataGridViewShowAllNotes.SelectedRows)
            {
                Note note = new Note
                {
                    NoteID = row.Cells["NoteID"].Value.ToString(),
                    NoteNumber = Convert.ToInt32(row.Cells["NoteNumber"].Value),
                    AddedDate = Convert.ToDateTime(row.Cells["AddedDate"].Value),
                    Subject = row.Cells["Subject"].Value.ToString(),
                    Details = row.Cells["Details"].ToString(),
                    ProductID = GetProductIdByName(row.Cells["ProductName"].Value.ToString()),
                    SubcategoryID = GetSubcategoryIdByName(row.Cells["SubcategoryName"].Value.ToString(), GetProductIdByName(row.Cells["ProductName"].Value.ToString()))
                };
                notesToDelete.Add(note);
            }

            if (notesToDelete.Count == 0)
            {
                MessageBox.Show("No notes selected.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete the selected notes?", "Delete Notes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                foreach (var note in notesToDelete)
                {
                    DeleteNoteAndRefresh(note.NoteID);
                }

                // Check if any products are now empty
                var productsChecked = new HashSet<string>();
                foreach (var note in notesToDelete)
                {
                    if (!productsChecked.Contains(note.ProductID))
                    {
                        productsChecked.Add(note.ProductID);
                        if (IsProductEmpty(note.ProductID))
                        {
                            if (MessageBox.Show($"The product '{GetProductNameById(note.ProductID)}' has no more notes. Do you want to delete this product as well?", "Delete Product", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            {
                                DeleteProduct(note.ProductID);
                            }
                        }
                    }
                }

                MessageBox.Show("Selected notes have been deleted.");
                UpdateDataGridView();
            }
        }
    }
}
