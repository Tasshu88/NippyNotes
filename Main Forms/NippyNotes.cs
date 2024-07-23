using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using ComboBox = System.Windows.Forms.ComboBox;
using ComboBoxStyle = System.Windows.Forms.ComboBoxStyle;
using DrawMode = System.Windows.Forms.DrawMode;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Text;
using System.Reflection;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Diagnostics;
using static Nippy_Notes.NippyNotes;
using Nippy_Notes;
using Nippy_Notes.Helpers;
using DevExpress.Utils.Serializing;


/*
 
Private = an access modifer, this means the method can only be accessed within the class it's declared on.

bool = The return type means the method returns a boolean value (True/False)

void = This indicates that the method does not return any value

async = Asynchronnos - it allows a function to run in the background whilst NippyNotes does other stuff.

Object Sender = The 'Sender' parameter represents the object that raised the event, for example 'RichTextBoxDetails' has the 'Keydown' event.

KeyEventArgs: Clas that contains data for key related stuff, the 'e' parameter I have below provide access to the event data, such as which key is pressed.

eControl; the property of 'KeyEventArgs' indicates if the CTRL key is pressed at same time as another (V) I think this is similar for e.Shift

KeyCode: Property again of 'KeyEventArgs' indicates which key is pressed.

eHandled: Indicates whether the event has been handled, setting the property to 'True'

foreach: a 'foreeach' loop is used to iterate over each keyword.

int: Integer (obvious)

Count: property that gets the number of elements in the list

string: sequence of characters

float: similar to an array? 

dictionary: 

object: defines behaviour that the objects of the class can perform.

property: attributes a charater to the class

if: executes a block of code if a specific scenario is true

loop: Repeats block of code

exception: so we don't have crashes during an execution

inheritance: allows class to inherit properties and methods of different classes

interface: specifies methods that a class has to mandatory implement.

Namespace: helps organize my code and stop conflictss

DataTable: Represents a table of in memory data.

Guid: Globally Unique Identifier, used to identify objects.

Const: Declares a value that cannot be changed after initializing

Timer: Executes a method in intervals

openFileDialog: A dialog that allows users to select files from system.

Var: Allows the compiler to infer the type of variable.

Return: Exits a function and optionally returns a value to the caller. 

ArgumentException: Indicaes that an invalid argumen was passed

Lambada: Way to present a function that can be passed around liek data.

Event: A way for a class to notify other classes something is happening

Indexer: Allows objects to be indexed like arrays.

LINQ: Provides a way to query and manipulate data

Enum: Represents a set of named constraints

Static: Indicates that a member belongs to the type itself rather than to a specific object.






 */


namespace Nippy_Notes
{
    public partial class NippyNotes : Form
    {

        // Windows API function to bring the window to the foreground found on Reddit
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private const string startupOptionID = "startupBehavior";
        // Constant for storing the seasonal effect option ID
        private const string seasonalOptionID = "seasonalEffect";
        // Unique identifier for the current note
        private Guid currentNoteId;
        // Form to preview attachments
        private AttachmentPreviewForm previewForm;
        // Flag to track if initial save was attempted
        private bool initialSaveAttempted = false;
        // Table to hold attached files data
        private DataTable attachedFilesTable;
        // List to store all notes
        private List<Note> notesList = new List<Note>();
        // Index of the currently displayed note
        private int currentIndex = 0;
        // Flag to check if a note is selected from the search results
        private bool isNoteSelectedFromSearch = false;
        // Flag to check if the application is in search mode
        private bool isInSearchMode = false;
        // ID of the loaded note
        private string loadedNoteId = null;
        // Predefined option ID for some configuration
        private const string predefinedOptionID = "f1a1c3e4-5b6d-7f8g-9h0i-123456789abc";
        // Flag to check if the note is an existing one
        private bool isExistingNote = false;
        // Timer to manage seasonal effects
        private Timer seasonalTimer; // Timer for seasonal effects
        // List to store seasonal elements
        private List<SeasonalElement> seasonalElements = new List<SeasonalElement>();
        // Form to handle the search functionality
        private SearchForm searchForm;
        // Flag to track if note number has been incremented
        private bool hasIncrementedNoteNumber = false;
        // Flag to track if the note has changed
        private bool isNoteChanged = false;
        // Index of the last clicked row in the DataGridView
        private int lastClickedRowIndex = -1;
        // Property to store whether the form was maximized before a search
        public bool wasMaximizedBeforeSearch { get; private set; } = false;

        // Regular expressions to match SQL patterns for syntax highlighting, I'm trying to attempt to copy SQL code into the form.. which still isn't working.
        private static readonly Regex SqlCommentPattern = new Regex(@"(--[^\r\n]*|/\*[\s\S]*?\*/)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex SqlStringPattern = new Regex(@"'([^']|'')*'", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex SqlKeywordPattern = new Regex(@"\b(SELECT|FROM|WHERE|AND|OR|JOIN|ON|IN|AS|BETWEEN|ORDER\s+BY|INNER\s+JOIN|LEFT\s+JOIN|RIGHT\s+JOIN|FULL\s+JOIN|INSERT\s+INTO|UPDATE|DELETE|CREATE|ALTER|DROP|TABLE|VIEW|PROCEDURE|FUNCTION|INDEX|TRIGGER)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        // Flag to check if the note is a new one
        private bool isNewNote = false;

        private System.Windows.Forms.Label lblPageAmount;



        // Class to represent  Note object with various properties
        public class Note
        {
            public int NoteNumber { get; set; }
            public string NoteID { get; set; }
            public string Version { get; set; }
            public string ProductID { get; set; }
            public string SubcategoryID { get; set; }
            public string ParentNoteID { get; set; }
            public string Subject { get; set; }
            public string Keywords { get; set; }

            public string Details { get; set; }
            public string RTFDetails { get; set; }
            public DateTime AddedDate { get; set; }
            public DateTime? AddedTime { get; set; }
            public DateTime? ModifiedTime { get; set; }
            public DateTime? DeletedTime { get; set; }
            public DateTime? CopiedDate { get; set; }
            public DateTime? CopiedTime { get; set; }
            public string Source { get; set; }
        }

        // Thi is the Constructor to initialize the form and setup various components
        public NippyNotes()
        {
            // Initialize the form components
            InitializeComponent();

            // Initialize real-time validation for the form fields
            InitializeRealTimeValidation();

            // Create the database and necessary tables if they do not already exist
            //  CreateDatabaseAndTables();
            //Databases now moved to DatabaseHelper Class - 15/07

            // Ensure the options table exists in the database
            EnsureOptionsTable();

            // Initialize the DataTable to store attached files
            attachedFilesTable = new DataTable();
            attachedFilesTable.Columns.Add("FileName", typeof(string));
            attachedFilesTable.Columns.Add("FilePath", typeof(string));
            attachedFilesTable.Columns.Add("SizeKB", typeof(long));
            attachedFilesTable.Columns.Add("FileType", typeof(string));

            // Bind the DataTable to the DataGridView
            dataGridView1.DataSource = attachedFilesTable;

            // Make the TextBoxID and TextBoxDate read-only
            TextBoxID.ReadOnly = true;
            TextBoxDate.ReadOnly = true;
            TextBoxID.TabStop = false;
            TextBoxDate.TabStop = false;

            // Set ComboBoxProducts to dropdown style
            ComboBoxProducts.DropDownStyle = ComboBoxStyle.DropDown;

            // Populate the ComboBox with products from the database
            PopulateComboBoxProducts();

            // Add event handlers for ComboBox and DataGridView
            ComboBoxProducts.SelectionChangeCommitted += ComboBoxProducts_SelectionChangeCommitted;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            dataGridView1.CellClick += dataGridView1_CellClick;

            // Add event handlers for focus events on ComboBox and SubcategoryComboBox
            ComboBoxProducts.Enter += ComboBoxProducts_Enter;
            ComboBoxProducts.Leave += ComboBoxProducts_Leave;
            SubcategoryComboBox.Enter += SubcategoryComboBox_Enter;
            SubcategoryComboBox.Leave += SubcategoryComboBox_Leave;

            // Add event handler for the settings button
            BtnSettings.Click += BtnSettings_Click;

            // Add event handlers for form load, paint, move, and resize events
            this.Load += new EventHandler(NippyNotes_Load);
            this.Paint += new PaintEventHandler(NippyNotes_Paint);
            this.Move += NippyNotes_Move;
            this.Resize += NippyNotes_Resize;

            // Ensure the options table and default entry exist in the database
            EnsureOptionsTableAndDefaultEntry();

            // Generate a new unique identifier for the current note
            currentNoteId = Guid.NewGuid();

            // Set the initial text and color for the SubcategoryComboBox
            SubcategoryComboBox.Text = "-- Select a Subcategory --";
            SubcategoryComboBox.ForeColor = Color.Gray;

            // Set the tab order for the form controls, designer tab doesn't seem to be working?
            //now working
          
            ComboBoxProducts.TabIndex = 0;
            SubcategoryComboBox.TabIndex = 1;
            TextBoxSubject.TabIndex = 2;
            TextBoxKeywords.TabIndex = 3;
            RichTextBoxDetails.TabIndex = 4;
            BtnAttachFiles.TabIndex = 5;
            dataGridView1.TabIndex = 6;
            BtnSaveNote.TabIndex = 7;
            BtnImportNote.TabIndex = 8;
            BtnEmailNote.TabIndex = 9;
            BtnDeleteNote.TabIndex = 11;
            BtnSearchNote.TabIndex = 12;
            BtnCreateNotes.TabIndex = 13;
            BtnExportNote.TabIndex = 14;
            BtnSettings.TabIndex = 15;

            // Apply seasonal effects to the form
            ApplySeasonalEffect();

            // Enable double buffering to reduce flicker
            this.DoubleBuffered = true;

            // Add event handler for mouse move event
            this.MouseMove += new MouseEventHandler(Form_MouseMove);

            // Add event handlers for form load, activated, size changed, and move events
            this.Load += NippyNotes_Load;
            this.Activated += NippyNotes_Activated;
            this.SizeChanged += NippyNotes_SizeChanged;
            this.Move += NippyNotes_Move;

            // Set the start position of the form to the center of the screen - this is because a bug was found when
            // you have NippyNotes half way on one monitor
            this.StartPosition = FormStartPosition.CenterScreen;

            // Add event handler for key down events in the RichTextBoxDetails
            RichTextBoxDetails.KeyDown += RichTextBoxDetails_KeyDown;

            // Add event handlers to track changes in note fields
            TextBoxSubject.TextChanged += (s, e) => isNoteChanged = true;
            RichTextBoxDetails.TextChanged += (s, e) => isNoteChanged = true;
            TextBoxKeywords.TextChanged += (s, e) => isNoteChanged = true;
            ComboBoxProducts.SelectedIndexChanged += (s, e) => isNoteChanged = true;
            SubcategoryComboBox.SelectedIndexChanged += (s, e) => isNoteChanged = true;

            // Initialize the table for attached files
            InitializeAttachedFilesTable();


            // Apply default font and size to the RichTextBoxDetails - Ideally, this should be system option
            RichTextBoxHelper.ApplyDefaultFontAndSize(RichTextBoxDetails);

            // Initialize text formatting controls using the helper method
            RichTextBoxHelper.InitializeTextFormattingControls(this, RichTextBoxDetails,
                                                               BtnBold, BtnItalic, BtnUnderline,
                                                               ComboBoxFont, ComboBoxSize,
                                                               BtnTextColour, BtnStrikeOut,
                                                               BtnLeftAlign, BtnCenterAlign, BtnRightAlign);


        }

        //Tried to adjust this to allow Images to be pasted, if we can do it in WorkTracker why not here? is it a dev express thing???
        // Nope, ignore above, idiot. Resolved.
        private void RichTextBoxDetails_KeyDown(object sender, KeyEventArgs e)
        {
            


            if (e.Control && e.KeyCode == Keys.V)
            {
                if (Clipboard.ContainsText(TextDataFormat.Rtf))
                {
                    RichTextBoxDetails.SelectedRtf = Clipboard.GetText(TextDataFormat.Rtf);
                }
                else if (Clipboard.ContainsImage())
                {
                    PasteImageContent();
                }
                else if (Clipboard.ContainsText())
                {
                    string pastedText = Clipboard.GetText();
                    if (IsSqlCode(pastedText))
                    {
                        PasteSqlCodeWithHighlighting(pastedText);
                    }
                    else
                    {
                        RichTextBoxDetails.SelectedText = pastedText;
                    }
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Tab)
            {
                if (RichTextBoxDetails.SelectionLength > 0)
                {
                    if (e.Shift)
                    {
                        OutdentSelectedText();
                    }
                    else
                    {
                        IndentSelectedText();
                    }
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                // Save the current formatting
                Font currentFont = RichTextBoxDetails.SelectionFont;
                Color currentColor = RichTextBoxDetails.SelectionColor;
                HorizontalAlignment currentAlignment = RichTextBoxDetails.SelectionAlignment;

                // Insert a new line
                int selectionStart = RichTextBoxDetails.SelectionStart;
                RichTextBoxDetails.SelectionStart = selectionStart;
                RichTextBoxDetails.SelectedText = Environment.NewLine;
                RichTextBoxDetails.SelectionStart = selectionStart + Environment.NewLine.Length;
                RichTextBoxDetails.SelectionLength = 0;

                // Apply the saved formatting to the new line
                RichTextBoxDetails.SelectionFont = currentFont;
                RichTextBoxDetails.SelectionColor = currentColor;
                RichTextBoxDetails.SelectionAlignment = currentAlignment;

                e.Handled = true;
            }
        }

        private void RichTextBoxDetails_TextChanged(object sender, EventArgs e)
        {
                   ApplySyntaxHighlighting(0, RichTextBoxDetails.Text.Length);
        }


        private void IndentSelectedText()
        {
            // Indents the selected text by adding a tab character at the beginning of each line.
            // It splits the selected text into lines, adds a tab to each line, and then joins the lines back together.
            string selectedText = RichTextBoxDetails.SelectedText;
            string[] lines = selectedText.Split(new[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = "\t" + lines[i];
            }
            RichTextBoxDetails.SelectedText = string.Join("\r\n", lines);
        }


        //broken 01/07/2024 cannot figure this out, potentially Reddit or pay again for ChatGPT Subscription for debug.
        // Why can't you figure this out, more time burnt on something simple. 
        // Figured this out 04/07/2024 now working.
        private void OutdentSelectedText()
        {
            // Outdents the selected text by removing a tab character from the beginning of each line, if present.
            // It splits the selected text into lines, removes the leading tab from each line, and joins the lines back together.
            string selectedText = RichTextBoxDetails.SelectedText;
            string[] lines = selectedText.Split(new[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("\t"))
                {
                    lines[i] = lines[i].Substring(1);
                }
            }
            RichTextBoxDetails.SelectedText = string.Join("\r\n", lines);
        }

        private void PasteImageContent()
        {
            // Pastes an image from the clipboard into the RichTextBox.
            // It checks if the clipboard contains an image, converts it to a PNG format, and then pastes it.
            if (Clipboard.ContainsImage())
            {
                Image image = Clipboard.GetImage();
                Clipboard.Clear();

                // Insert the image into the RichTextBox
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                    DataObject dataObject = new DataObject();
                    dataObject.SetData(DataFormats.Bitmap, true, image);
                    dataObject.SetData(DataFormats.Text, Convert.ToBase64String(imageBytes));
                    Clipboard.SetDataObject(dataObject, true);

                    RichTextBoxDetails.Paste();
                }
            }
        }

        private bool IsSqlCode(string text)
        {
            // Checks if the provided text contains SQL keywords.
            // It looks for common SQL keywords like SELECT, FROM, WHERE, etc., to determine if the text is SQL code.
            // Basic check for SQL keywords
            

            //Still rogered, if I use SQL code from Notepad++ it looks different to what I'm pasting from SQL.. Don't think this is possible

            string[] sqlKeywords = { "SELECT", "FROM", "WHERE", "INSERT", "UPDATE", "DELETE", "JOIN" };
            foreach (string keyword in sqlKeywords)
            {
                if (text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void PasteSqlCodeWithHighlighting(string sqlCode)
        {
            // Pastes SQL code into the RichTextBox with syntax highlighting.
            // It sets the text color to blue for the SQL code and then resets it to the default color.
            RichTextBoxDetails.SelectionColor = Color.Blue; // Change color to blue for SQL code
            RichTextBoxDetails.SelectedText = sqlCode;
            RichTextBoxDetails.SelectionColor = RichTextBoxDetails.ForeColor; // Reset color to default
        }

        private void ApplySyntaxHighlighting(int startIndex, int length)
        {
            // Applies syntax highlighting to the specified range of text in the RichTextBox.
            // It highlights SQL comments, strings, and keywords with different colors and styles.
            // It temporarily disables the TextChanged event to prevent 'recursive calls' according to Reddit


            // According to Google, a recursive call occurs when a function calls itself, a recursive call in this cause would happen if 'ApplySyntaxHighlighting' method is
            //triggered in the RichTextBox_TextChanged event which in turn called this method. (Basically, similar to a network storm on a network switch - infinite loop)

            // Temporarily disable event to prevent recursive calls
            RichTextBoxDetails.TextChanged -= RichTextBoxDetails_TextChanged;

            int originalSelectionStart = RichTextBoxDetails.SelectionStart;
            int originalSelectionLength = RichTextBoxDetails.SelectionLength;

            RichTextBoxDetails.SuspendLayout();

            // Store original formatting
            Color originalColor = RichTextBoxDetails.SelectionColor;
            Font originalFont = RichTextBoxDetails.SelectionFont;

            RichTextBoxDetails.Select(startIndex, length);
            RichTextBoxDetails.SelectionColor = Color.Black; // Default color
            RichTextBoxDetails.SelectionFont = new Font(RichTextBoxDetails.Font, FontStyle.Regular);

            // Apply comment highlighting
            foreach (Match match in SqlCommentPattern.Matches(RichTextBoxDetails.Text))
            {
                RichTextBoxDetails.Select(startIndex + match.Index, match.Length);
                RichTextBoxDetails.SelectionColor = Color.Green;
                RichTextBoxDetails.SelectionFont = new Font(RichTextBoxDetails.Font, FontStyle.Italic);
            }

            // Apply string highlighting
            foreach (Match match in SqlStringPattern.Matches(RichTextBoxDetails.Text))
            {
                RichTextBoxDetails.Select(startIndex + match.Index, match.Length);
                RichTextBoxDetails.SelectionColor = Color.Brown;
            }

            // Apply keyword highlighting
            foreach (Match match in SqlKeywordPattern.Matches(RichTextBoxDetails.Text))
            {
                RichTextBoxDetails.Select(startIndex + match.Index, match.Length);
                RichTextBoxDetails.SelectionColor = Color.Blue;
                RichTextBoxDetails.SelectionFont = new Font(RichTextBoxDetails.Font, FontStyle.Bold);
            }

            RichTextBoxDetails.Select(originalSelectionStart, originalSelectionLength);
            RichTextBoxDetails.SelectionColor = originalColor;
            RichTextBoxDetails.SelectionFont = originalFont;

            RichTextBoxDetails.ResumeLayout();

            // Re-enable the event
            RichTextBoxDetails.TextChanged += new EventHandler(this.RichTextBoxDetails_TextChanged);
        }


        #region Database&Tables Setup



        /*
private void CreateDatabaseAndTables()
{
    // Creates the database file if it doesn't exist and sets up all necessary tables.
    // This includes tables for notes, products, subcategories, files, error logs, keywords, options, etc.
    string databaseFilePath = Path.Combine(Application.StartupPath, "NippyDB.db");
    if (!File.Exists(databaseFilePath))
    {
        SQLiteConnection.CreateFile(databaseFilePath); // This creates the database file
    }

    CreateTable_Notes();
    CreateTable_Products();
    CreateTable_Subcategories();
    CreateTable_ProductSub();
    CreateTable_Files();
    CreateTable_ErrorLog();
    CreateTable_Keywords();
    CreateTable_Options();
    CreateTable_ImportTranslations();
    CreateTable_ActivityLog();
    CreateTable_NoteViews();
    CreateTable_BackupHistory();
    CreateTable_SmtpSettings();

    InsertRequiredOptions();
}

private void ExecuteNonQuery(string query)
{
    // Executes a non-query SQL command (like CREATE TABLE, INSERT, UPDATE, DELETE) against the database.
    // It opens a connection, executes the command, and handles any exceptions that occur.
    try
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Failed to execute query: " + ex.Message);
    }
}

private void CreateTable_Notes()
{
    // Creates the "Notes" table in the database if it doesn't already exist.
    // This table stores information about notes, including ID, version, number, dates, times, product and subcategory IDs, subject, details, source, and keywords.
    string createTableQuery = @"
CREATE TABLE IF NOT EXISTS Notes (
       NoteID TEXT PRIMARY KEY,
Version TEXT,
    NoteNumber INTEGER,
    AddedDate TEXT,
    AddedTime TIME,
    ModifiedTime TIME,
    DeletedTime TIME,
    CopiedDate DATE,
    CopiedTime TIME,
    ProductID TEXT,
    SubcategoryID TEXT,
    ParentNoteID TEXT,
    Subject TEXT,
    Details TEXT,
    Source TEXT,
    Keywords TEXT,
    FOREIGN KEY(ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY(SubcategoryID) REFERENCES Subcategories(SubcategoryID),
    FOREIGN KEY(ParentNoteID) REFERENCES Notes(NoteID)
)";
    ExecuteNonQuery(createTableQuery);
}

private void CreateTable_Products()
{
    // Creates the "Products" table in the database if it doesn't already exist.
    // This table stores information about products, including ID, name, and dates/times for added, modified, and deleted records.
    string createTableQuery = @"
CREATE TABLE IF NOT EXISTS Products (
    ProductID TEXT PRIMARY KEY,
    ProductName VARCHAR(255) NOT NULL,
    AddedDate DATE,
    AddedTime TIME,
    ModifiedTime TIME,
    DeletedTime TIME
)";
    ExecuteNonQuery(createTableQuery);
}

private void CreateTable_Subcategories()
{
    // Creates the "Subcategories" table in the database if it doesn't already exist.
    // This table stores information about subcategories, including ID, name, product ID, and dates/times for added, modified, and deleted records.
    string createTableQuery = @"
CREATE TABLE IF NOT EXISTS Subcategories (
    SubcategoryID TEXT PRIMARY KEY,
    SubcategoryName VARCHAR(255) NOT NULL,
    ProductID TEXT,
    AddedDate DATE,
    AddedTime TIME,
    ModifiedTime TIME,
    DeletedTime TIME,
    FOREIGN KEY(ProductID) REFERENCES Products(ProductID)
)";
    ExecuteNonQuery(createTableQuery);
}

private void CreateTable_ProductSub()
{
    // Creates the "ProductSub" table in the database if it doesn't already exist.
    // This table establishes a many-to-many relationship between products and subcategories.
    string createTableQuery = @"
CREATE TABLE IF NOT EXISTS ProductSub (
    ProductID TEXT,
    SubcategoryID TEXT,
    PRIMARY KEY (ProductID, SubcategoryID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (SubcategoryID) REFERENCES Subcategories(SubcategoryID)
)";
    ExecuteNonQuery(createTableQuery);
}

private void CreateTable_Files()
{
    // Creates the "Files" table in the database if it doesn't already exist.
    // This table stores information about files attached to notes, including ID, note ID, file name, type, path, size, added and deleted times, and file data (blob).
    string createTableQuery = @"
CREATE TABLE IF NOT EXISTS Files (
    FileID TEXT PRIMARY KEY,
    NoteID TEXT,
    FileName NVARCHAR(255),
    FileType TEXT,
    FilePath TEXT,
    SizeKB INTEGER,
    AddedTime TIME,
    DeletedTime TIME,
    FileData BLOB,  -- New column for file contents
    FOREIGN KEY(NoteID) REFERENCES Notes(NoteID)
)";
    ExecuteNonQuery(createTableQuery);
}

private void CreateTable_ErrorLog()
{
    // Creates the "ErrorLog" table in the database if it doesn't already exist.
    // This table logs errors, including error message, date, time, method/button, table name, and context.
    string createTableQuery = @"
CREATE TABLE IF NOT EXISTS ErrorLog (
    ErrorID INTEGER PRIMARY KEY AUTOINCREMENT,
    ErrorDate DATE,
    ErrorTime TIME,
    ErrorMessage TEXT,
    MethodOrButton NVARCHAR(100),
    TableName TEXT, 
    Context TEXT
)";
    ExecuteNonQuery(createTableQuery);
}

private void CreateTable_Keywords()
{
    // Creates the "Keywords" table in the database if it doesn't already exist.
    // This table stores keywords associated with notes, including ID, note ID, and keyword.
    string createTableQuery = @"
CREATE TABLE IF NOT EXISTS Keywords (
    KeywordID INTEGER PRIMARY KEY AUTOINCREMENT,
    NoteID TEXT,
    Keyword TEXT,
    FOREIGN KEY(NoteID) REFERENCES Notes(NoteID) ON DELETE CASCADE
)";
    ExecuteNonQuery(createTableQuery);
}

private void CreateTable_Options()
{
    // Creates the "Options" table in the database if it doesn't already exist.
    // This table stores configuration options, including option ID, value, and description.
    // It also inserts default options if they don't exist.
    string createTableQuery = @"
    CREATE TABLE IF NOT EXISTS Options (
        OptionID TEXT PRIMARY KEY,
        OptionValue TEXT NOT NULL DEFAULT 'false',
        OptionDescription TEXT NOT NULL
    )";
    ExecuteNonQuery(createTableQuery);

    // Insert default values if they do not exist
    InsertDefaultOptions();
}

private void CreateTable_ImportTranslations()
{
    // Creates the "ImportTranslations" table in the database if it doesn't already exist.
    // This table stores translations for imported data, including translation ID, type, source database ID, source and destination translation IDs and values.
    string createTableQuery = @"
CREATE TABLE IF NOT EXISTS ImportTranslations (
  TranslationID TEXT PRIMARY KEY,
    TranslationType TEXT,
    SourceDatabaseID TEXT NOT NULL,
    SourceTranslationID TEXT,
    SourceTranslationValue TEXT,
    DestinationTranslationID TEXT,
    DestinationTranslationValue TEXT
)";
    ExecuteNonQuery(createTableQuery);
}

private void CreateTable_ActivityLog()
{
    // Creates the "ActivityLog" table in the database if it doesn't already exist.
    // This table logs user activities, including log ID, timestamp, action type, and details.
    string createTableQuery = @"
CREATE TABLE IF NOT EXISTS ActivityLog (
    LogID INTEGER PRIMARY KEY AUTOINCREMENT,
    Timestamp TEXT NOT NULL,
    ActionType TEXT NOT NULL,
    Details TEXT NOT NULL
)";
    ExecuteNonQuery(createTableQuery);
}

private void CreateTable_NoteViews()
{
    // Creates the "NoteViews" table in the database if it doesn't already exist.
    // This table tracks the number of views for each note, including note ID, number, and view count.
    string createTableQuery = @"
CREATE TABLE IF NOT EXISTS NoteViews (
    NoteID TEXT PRIMARY KEY,
    NoteNumber INTEGER,
    ViewCount INTEGER DEFAULT 0,
    FOREIGN KEY(NoteID) REFERENCES Notes(NoteID)
)";
    ExecuteNonQuery(createTableQuery);
}

private void CreateTable_BackupHistory()
{
    // Creates the "BackupHistory" table in the database if it doesn't already exist.
    // This table stores information about database backups, including backup ID, name, location, file path, and date.
    string createTableQuery = @"
        CREATE TABLE IF NOT EXISTS BackupHistory (
            BackupID INTEGER PRIMARY KEY AUTOINCREMENT,
            BackupName TEXT,
            Location TEXT,
            FilePath TEXT,
            Date TEXT
        )";
    ExecuteNonQuery(createTableQuery);
}

private void CreateTable_SmtpSettings()
{
    // Creates the "SmtpSettings" table in the database if it doesn't already exist.
    // This table stores SMTP settings for sending emails, including host, port, username, password, and from email address.
    string createTableQuery = @"
CREATE TABLE IF NOT EXISTS SmtpSettings (
SettingID INTEGER PRIMARY KEY AUTOINCREMENT,
    Host TEXT NOT NULL,
    Port INTEGER NOT NULL,
    Username TEXT NOT NULL,
    Password TEXT NOT NULL,
    FromEmail TEXT NOT NULL
)";
    ExecuteNonQuery(createTableQuery);
}
*/




        #endregion


        // Method to insert default options
        //This no longer works 17/07/2024
        //Is it because I moved everything to DatabaseHelper?????!!
        private void InsertDefaultOptions()
        {
            DatabaseHelper.InsertDefaultOptions();
        }

        //This no longer works 17/07/2024
        private void InsertRequiredOptions()
        {

            DatabaseHelper.InsertRequiredOptions();
        }

     
        private string GetDatabaseID()
        {
            return DatabaseHelper.GetDatabaseID();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handles double-click events on cells in the DataGridView.
            // Opens files associated with the clicked cell in the appropriate application (e.g., Outlook, browser).
            // If an attachment preview form is open, it closes it and opens a new preview for the selected file.

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string filePath = row.Cells["FilePath"].Value.ToString();
                string fileType = row.Cells["FileType"].Value.ToString();

                if ((fileType.Equals(".msg", StringComparison.OrdinalIgnoreCase) || fileType.Equals(".eml", StringComparison.OrdinalIgnoreCase)))
                {
                    // Assuming Outlook is installed and this block can execute successfully without checking
                    try
                    {
                        System.Diagnostics.Process.Start("OUTLOOK.EXE", filePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to open the file with Outlook: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // Here, handle error without asking user (since that method is removed)
                    }
                    return;
                }

                if (fileType.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    OpenFileInBrowser(filePath);
                    return;
                }

                // If SQL files need specific handling, consider simplifying or handling differently here

                if (previewForm != null)
                {
                    previewForm.Close();
                }

                previewForm = new AttachmentPreviewForm(filePath, fileType);
                previewForm.StartPosition = FormStartPosition.Manual;

                int offsetX = 8;
                int offsetY = this.Height / 9;
                Point previewLocation = new Point(this.Location.X + this.Width - offsetX, this.Location.Y + offsetY);

                if (IsOffScreen(previewLocation, previewForm.Size))
                {
                    CenterFormOnScreen(this);
                    previewLocation = new Point(this.Location.X + this.Width - offsetX, this.Location.Y + offsetY);
                }

                previewForm.Location = previewLocation;
                previewForm.Show();
            }
        }



        private void OpenFileInBrowser(string filePath)
        {

            // Tries to open the specified file in the default web browser.
            // If it fails, it shows an error message with the reason for the failure.

            try
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open the file in the browser: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private bool IsOffScreen(Point location, Size formSize)
        {

            // Checks if the specified location and form size would place the form off the screen.
            // It returns true if any part of the form would be outside the screen bounds.

            Rectangle screenBounds = Screen.FromPoint(location).WorkingArea;
            return location.X < screenBounds.Left || location.Y < screenBounds.Top ||
                   location.X + formSize.Width > screenBounds.Right || location.Y + formSize.Height > screenBounds.Bottom;
        }

        private void CenterFormOnScreen(Form form)
        {

            // Centers the specified form on the screen.
            // It calculates the position to center the form within the working area of the screen.

            Rectangle screenBounds = Screen.FromPoint(form.Location).WorkingArea;
            int x = Math.Max(screenBounds.X, screenBounds.X + (screenBounds.Width - form.Width) / 2);
            int y = Math.Max(screenBounds.Y, screenBounds.Y + (screenBounds.Height - form.Height) / 2);
            form.Location = new Point(x, y);
        }



        private void ComboBoxProducts_SelectionChangeCommitted(object sender, EventArgs e)
        {

            // Handles selection changes in the ComboBoxProducts control.
            // Loads subcategory options based on the selected product and updates the SubcategoryComboBox.
            // If no product is selected, it clears and hides the subcategory options.

            // Ensure this event doesn't reset TextBoxID
            //confirmed it doesn't
            if (ComboBoxProducts.SelectedItem != null)
            {
                var selectedProduct = ComboBoxProducts.SelectedItem.ToString();
                var productId = GetProductIdByName(selectedProduct);

                SubcategoryComboBox.Items.Clear();
                SubcategoryComboBox.Items.Add("-- Select a Subcategory --");
                SubcategoryComboBox.SelectedIndex = 0;

                LoadSubcategoryOptions(productId);
            }
            else
            {
                SubcategoryComboBox.Items.Clear();
                SubcategoryComboBox.Items.Add("-- Select a Subcategory --");
                SubcategoryComboBox.SelectedIndex = 0;
                SubcategoryComboBox.Visible = false;
                LblSubcategory.Visible = true;
            }
            Console.WriteLine($"ComboBoxProducts_SelectionChangeCommitted: TextBoxID is {TextBoxID.Text}");
        }

        // Method to populate next note ID
        private void PopulateNextNoteID()
        {
            DatabaseHelper.PopulateNextNoteID(TextBoxID);
        }

        private void UpdateTextBoxID(string text)
        {

            // Updates the TextBoxID control with the provided text.
            // It uses the Invoke method if required, ensuring thread safety.

            if (TextBoxID.InvokeRequired)
            {
                TextBoxID.Invoke(new Action<string>(UpdateTextBoxID), text);
            }
            else
            {
                TextBoxID.Text = text;
                Console.WriteLine($"UpdateTextBoxID: TextBoxID updated to {text}");
            }
        }



        // Method to save attachments
        private void SaveAttachments(string noteId)
        {
            DatabaseHelper.SaveAttachments(noteId, attachedFilesTable);
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            // Handles click events on cells in the DataGridView.
            // If the same row is clicked again, it closes the preview form; otherwise, it updates the last clicked row index.

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.RowIndex == lastClickedRowIndex)
                {
                    ClosePreviewForm();
                    lastClickedRowIndex = -1;
                    return;
                }

                lastClickedRowIndex = e.RowIndex;
            }
        }


        // Method to populate combo box products
        private void PopulateComboBoxProducts()
        {
            DatabaseHelper.PopulateComboBoxProducts(ComboBoxProducts);
        }


        //Not implemented, v2 if needed
        private void SaveProductSubRelationship(string productId, string subcategoryId)
        {
            DatabaseHelper.SaveProductSubRelationship(productId, subcategoryId);
        }


        public void ClearForm(bool isNewNote = true)
        {

            // Clears the form fields and resets the controls for a new or existing note.
            // It also updates the note ID, reloads product options, and resets various controls.

            TextBoxSubject.Clear();
            RichTextBoxDetails.Clear();
            TextBoxDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            TextBoxKeywords.Clear();

            ComboBoxProducts.SelectedIndex = -1;
            ComboBoxProducts.Text = "-- Select a Product --";
            ComboBoxProducts.ForeColor = Color.Gray;

            SubcategoryComboBox.Items.Clear();
            SubcategoryComboBox.Text = "-- Select a Subcategory --";
            SubcategoryComboBox.ForeColor = Color.Gray;
            SubcategoryComboBox.Visible = true;
            LblSubcategory.Visible = true;

            if (dataGridView1.DataSource is DataTable dt)
            {
                dt.Rows.Clear();
                dataGridView1.Refresh();
            }

            PopulateComboBoxProducts();

            if (isNewNote)
            {
                PopulateNextNoteID();
                loadedNoteId = null;
                currentNoteId = Guid.Empty;
                isExistingNote = false;
            }

            // Clear the page amount label
            LblPageAmount.Text = string.Empty;

            UpdateNavigationButtons();  // Ensure buttons are enabled if needed
        }

     
        // Method to check database directly
        //Moved to DatabaseHelper instead 16/07
        private void CheckDatabaseDirectly()
        {
            DatabaseHelper.CheckDatabaseDirectly();
        }

        public void RefreshDate()
        {
            // Refreshes the TextBoxDate control with the current date in "dd/MM/yyyy" format.

            TextBoxDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }

        // Method to log error
        //This is logging errors twice...
        private void LogError(string errorMessage, string methodName, string tableName, string context = null)
        {
            DatabaseHelper.LogError(errorMessage, methodName, tableName, context);
        }

        //No longer required.
        private Guid GetOrCreateProductId(string productName)
        {
            return DatabaseHelper.GetOrCreateProductId(productName);
        }

        private void BtnAttachFiles_Click(object sender, EventArgs e)
        {

            // Handles the Click event for the BtnAttachFiles button.
            // It opens a file dialog for selecting files to attach, validates the selected files, and adds them to the attachedFilesTable.
            // If a .db file is selected, it shows a warning dialog and stops further processing. (Thanks to Lewis I've created DBWarningForm)

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                openFileDialog.Title = "Select Files to Attach";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var dataTable = dataGridView1.DataSource as DataTable;
                    if (dataTable == null)
                    {
                        MessageBox.Show("Data source is not set correctly.");
                        return;
                    }

                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        if (Path.GetExtension(filePath).Equals(".db", StringComparison.OrdinalIgnoreCase))
                        {
                            DBWarningForm dbWarningForm = new DBWarningForm();
                            dbWarningForm.ShowDialog();
                            return;
                        }

                        string fileName = Path.GetFileName(filePath);
                        long fileSizeBytes = new FileInfo(filePath).Length;
                        long fileSizeKB = (fileSizeBytes + 1023) / 1024; // Round up to the nearest KB
                        string fileType = Path.GetExtension(filePath);

                        DataRow newRow = dataTable.NewRow();
                        newRow["FileName"] = fileName;
                        newRow["FilePath"] = filePath;
                        newRow["SizeKB"] = fileSizeKB;
                        newRow["FileType"] = fileType;
                        dataTable.Rows.Add(newRow);
                    }

                    dataGridView1.Refresh();
                    isNoteChanged = true; // Mark note as changed
                }
            }
        }

        // Method to ensure options table and default entry
        private void EnsureOptionsTableAndDefaultEntry()
        {
            DatabaseHelper.EnsureOptionsTableAndDefaultEntry(startupOptionID);
        }

        // Method to insert or update note into database
        //Removed
        private bool InsertOrUpdateNoteIntoDatabase(Note note)
        {
            return DatabaseHelper.InsertOrUpdateNoteIntoDatabase(note);
        }


        //Implemented but not finalised - more to come v2
        private void HandleKeywords(string noteId, string keywords)
        {
            DatabaseHelper.HandleKeywords(noteId, keywords);
        }




        private void BtnSaveNote_Click(object sender, EventArgs e)
        {

            // Handles the Click event for the BtnSaveNote button.
            // It validates the required fields, constructs a Note object, and inserts or updates the note in the database.
            // It also handles saving attachments and keywords, updating the status label, and reloading the note details.

            List<string> missingFields = new List<string>();

            if (string.IsNullOrEmpty(ComboBoxProducts.Text) || ComboBoxProducts.Text == "-- Select a Product --")
                missingFields.Add("Product");
            if (string.IsNullOrEmpty(TextBoxSubject.Text))
                missingFields.Add("Subject");
            if (string.IsNullOrEmpty(RichTextBoxDetails.Text))
                missingFields.Add("Details");

            if (missingFields.Count > 0)
            {
                LblStatus.Text = "Missing Field(s): " + string.Join(", ", missingFields);
                LblStatus.ForeColor = Color.Red;
                return; // Stop further execution if validation fails
            }

            Note note = GetCurrentNote();
            if (note == null)
            {
                // This block will not be reached if mandatory fields are not filled due to the above validation.
                return;
            }

            bool isNewNote = currentNoteId == Guid.Empty;

            if (isNewNote && NoteNumberExists(note.NoteNumber))
            {
                MessageBox.Show($"A note with number {note.NoteNumber} already exists. Please use a different number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (isNewNote)
            {
                if (InsertNewNoteIntoDatabase(note))
                {
                    LblStatus.ForeColor = Color.Green;
                    LblStatus.Text = $"Saved new note number {note.NoteNumber}";
                }
                else
                {
                    MessageBox.Show("Failed to save the note.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (UpdateExistingNoteInDatabase(note))
                {
                    LblStatus.ForeColor = Color.Green;
                    LblStatus.Text = $"Amended note number {note.NoteNumber}";
                }
                else
                {
                    MessageBox.Show("Failed to amend the note.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Ensure keywords are saved
            HandleKeywords(note.NoteID, note.Keywords);

            isNoteChanged = false;  // Reset the flag
            SaveAttachments(note.NoteID);
            PopulateComboBoxProducts();

            // Reload the note to allow further editing
            LoadNoteDetailsById(note.NoteID);
        }


        // Method to load note details by ID
        //Moved to DatabaseHelper as code looked shit
        private void LoadNoteDetailsById(string noteId)
        {
            DatabaseHelper.LoadNoteDetailsById(noteId, SetNoteDetails, ref currentNoteId);
        }


        // Method to check if note number exists
        //Moved to DatabaseHelper as code looked shit
        private bool NoteNumberExists(int noteNumber)
        {
            return DatabaseHelper.NoteNumberExists(noteNumber);
        }


        // Method to update existing note in database
        //Moved to DatabaseHelper as code looked shit
        private bool UpdateExistingNoteInDatabase(Note note)
        {
            return DatabaseHelper.UpdateExistingNoteInDatabase(note);
        }




        private void InitializeRealTimeValidation()
        {

            // Initializes real-time validation for form fields.
            // It adds TextChanged event handlers for subject, product, and details fields to validate and update the status label as the user types.

            TextBoxSubject.TextChanged += RealTimeValidationHandler;
            ComboBoxProducts.TextChanged += RealTimeValidationHandler;
            RichTextBoxDetails.TextChanged += RealTimeValidationHandler;
        }

        private void RealTimeValidationHandler(object sender, EventArgs e)
        {

            // Handles real-time validation for form fields.
            // It checks if required fields are missing and updates the status label accordingly.
            // It also sets a flag indicating that the note has changed.

            if (initialSaveAttempted && LblStatus.ForeColor != Color.Green)
            {
                List<string> missingFields = new List<string>();

                if (string.IsNullOrEmpty(TextBoxSubject.Text.Trim())) missingFields.Add("Subject");
                if (string.IsNullOrEmpty(ComboBoxProducts.Text.Trim()) || ComboBoxProducts.Text == "-- Select a Product --") missingFields.Add("Product");
                if (string.IsNullOrEmpty(RichTextBoxDetails.Text.Trim())) missingFields.Add("Details");

                if (missingFields.Any())
                {
                    LblStatus.Text = "Missing: " + string.Join(", ", missingFields);
                    LblStatus.ForeColor = Color.Red;
                    LblStatus.Visible = true;
                }
                else
                {
                    LblStatus.Visible = false;
                }
            }
            isNoteChanged = true;  // Set the flag when changes are detected
        }

        private void BtnSearchNote_Click(object sender, EventArgs e)
        {

            // Handles the Click event for the BtnSearchNote button.
            // It opens the search form and logs whether the main form was maximized before opening the search form.
            wasMaximizedBeforeSearch = this.WindowState == FormWindowState.Maximized;

            if (wasMaximizedBeforeSearch)
            {
                this.WindowState = FormWindowState.Normal;
            }

            OpenSearchForm();
        }

        private void OpenSearchForm()
        {

            // Opens the search form and subscribes to the NoteSelected event.
            SearchForm searchForm = new SearchForm(this);
            searchForm.NoteSelected += SearchForm_NoteSelected;
            searchForm.Show();
        }

        private void SearchForm_NoteSelected(object sender, EventArgs e)
        {

            // Handles the NoteSelected event from the search form.
            // This method can be customized to load the selected note into the main form.

            if (sender is SearchForm searchForm)
            {

            }
        }

        private void ComboBoxProducts_Enter(object sender, EventArgs e)
        {

            // Handles the Enter event for the ComboBoxProducts control.
            // It clears the default text and sets the text color to black when the control gains focus.

            if (ComboBoxProducts.Text == "-- Select a Product --")
            {
                ComboBoxProducts.Text = "";
                ComboBoxProducts.ForeColor = Color.Black;
            }
        }

        private void ComboBoxProducts_Leave(object sender, EventArgs e)
        {

            // Handles the Leave event for the ComboBoxProducts control.
            // It sets the default text and gray color if the control loses focus and is empty.

            if (string.IsNullOrWhiteSpace(ComboBoxProducts.Text))
            {
                ComboBoxProducts.Text = "-- Select a Product --";
                ComboBoxProducts.ForeColor = Color.Gray;
            }
        }

        private void SubcategoryComboBox_Enter(object sender, EventArgs e)
        {
            // Handles the Enter event for the SubcategoryComboBox control.
            // It clears the default text and sets the text color to black when the control gains focus.

            if (SubcategoryComboBox.Text == "-- Select a Subcategory --")
            {
                SubcategoryComboBox.Text = "";
                SubcategoryComboBox.ForeColor = Color.Black;
            }
        }

        private void SubcategoryComboBox_Leave(object sender, EventArgs e)
        {
            // Handles the Leave event for the SubcategoryComboBox control.
            // It sets the default text and gray color if the control loses focus and is empty.

            if (string.IsNullOrWhiteSpace(SubcategoryComboBox.Text))
            {
                SubcategoryComboBox.Text = "-- Select a Subcategory --";
                SubcategoryComboBox.ForeColor = Color.Gray;
            }
        }

        //Moved to DatabaseHelper as code looked shit
        private void LoadNotesByProductSubcategory(string productId, string subcategoryId)
        {
            DatabaseHelper.LoadNotesByProductSubcategory(productId, subcategoryId, notesList, DisplayNote, ClearForm, lblPageAmount);
        }

        //Moved to DatabaseHelper as code looked shit
        private string FetchKeywords(string noteId)
        {
            return DatabaseHelper.FetchKeywords(noteId);
        }

        public void LoadNoteDetails(Note selectedNote)
        {

            // Loads the details of the selected note into the form fields.
            // It sets the note ID, date, product, subcategory, subject, details, and keywords.
            // It also enables editing controls and updates the current note ID.

            if (selectedNote != null)
            {
                loadedNoteId = selectedNote.NoteID;  // Ensure loadedNoteId is set
                isExistingNote = true;  // Set the flag to true

                TextBoxID.Text = selectedNote.NoteNumber.ToString();
                TextBoxDate.Text = selectedNote.AddedDate.ToString("dd/MM/yyyy");

                // Correctly set the product
                ComboBoxProducts.SelectedItem = GetProductNameById(selectedNote.ProductID);
                if (ComboBoxProducts.SelectedItem == null && !string.IsNullOrEmpty(selectedNote.ProductID))
                {
                    ComboBoxProducts.Text = GetProductNameById(selectedNote.ProductID);
                }

                LoadSubcategoryOptions(selectedNote.ProductID);

                string subcategoryName = GetSubcategoryNameById(selectedNote.SubcategoryID);
                if (string.IsNullOrEmpty(subcategoryName))
                {
                    SubcategoryComboBox.SelectedIndex = SubcategoryComboBox.Items.IndexOf("No Subcategory");
                }
                else
                {
                    SubcategoryComboBox.SelectedIndex = SubcategoryComboBox.Items.IndexOf(subcategoryName);
                    if (SubcategoryComboBox.SelectedIndex == -1 && !string.IsNullOrEmpty(subcategoryName))
                    {
                        SubcategoryComboBox.Text = subcategoryName;
                    }
                }
                SubcategoryComboBox.Visible = true;
                LblSubcategory.Visible = true;

                TextBoxSubject.Text = selectedNote.Subject;

                try
                {
                    RichTextBoxDetails.Rtf = selectedNote.Details; // Try to load as RTF
                }
                catch (ArgumentException)
                {
                    RichTextBoxDetails.Text = selectedNote.Details; // If RTF format is invalid, load as plain text
                }

                TextBoxKeywords.Text = FetchKeywords(selectedNote.NoteID);
                LoadAttachments(selectedNote.NoteID);

                UpdateNavigationButtons();
            }
            else
            {
                ClearForm(isInSearchMode);
                UpdateNavigationButtons();
            }
            // Clear the page amount label
            LblPageAmount.Text = string.Empty;
        }


        // Method to load all notes
        //Moved to DatabaseHelper as code looked shit
        private void LoadAllNotes()
        {
            DatabaseHelper.LoadAllNotes(notesList, DisplayNote, message => MessageBox.Show(message));
        }

        private void DisplayNote(int index)
        {

            // 'Index out of range' occurs when I'm attempting to access the list using an index outside of the valid range of indices.
            // In stupid terms so my two brain cells would understand - The index is either negative or greater than or equal to the number of elements.
            // In the DisplayNote, the index is checking against the bounds of 'NoteList' to prevent the error.


            // if (index >= 0 && index < notesList.Count)

            // The line above ensures that the index is within the valid range
            // index >= 0 ensures that the index is NOT negative
            // Index < notelist.Count ensures that the index is less than the total number of items in 'notesList'

            // If the index is not valid (It is negative or grater than or equial to the notelist.Count the code inside the else block will execute)

            // else
            // {
            // ClearForm(False);
            // LblPageAmouint.Text = 'Note 0/0';

            //This clears the form and updates the page amnount to indicate that no note is being displayed


            //For information

            //Prevention: Always check index bounds: before accessing alements in a list, ensure the index si within the valid range as done below.
            // Validate User Input; If the index comes from user input, validate the input to ensure it is within the expected range.
          


      

            if (index >= 0 && index < notesList.Count)
            {
                var note = notesList[index];
                currentNoteId = Guid.Parse(note.NoteID);
                TextBoxID.Text = note.NoteNumber.ToString();
                TextBoxDate.Text = note.AddedDate.ToString("dd/MM/yyyy");
                ComboBoxProducts.SelectedItem = GetProductNameById(note.ProductID);

                LoadSubcategoryOptions(note.ProductID, GetSubcategoryNameById(note.SubcategoryID));

                TextBoxSubject.Text = note.Subject;
                RichTextBoxDetails.Text = note.Details;
                LoadAttachments(note.NoteID);
                TextBoxKeywords.Text = FetchKeywords(note.NoteID);

                LblPageAmount.Text = $"Note {index + 1}/{notesList.Count}";

                isNoteChanged = false;  // Reset the flag after displaying a note
            }
            else
            //Index is not valid
            {
                ClearForm(false);
                LblPageAmount.Text = "Note 0/0";
            }
        }

        private string GetSubcategoryNameById(string subcategoryId)
        {
            return DatabaseHelper.GetSubcategoryNameById(subcategoryId);
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {

            // Handles the Click event for the BtnSettings button.
            // It opens the settings form and centers it relative to the main form.

            Settings settingsForm = new Settings(this);
            settingsForm.StartPosition = FormStartPosition.CenterParent;
            settingsForm.ShowDialog(this);
        }

        // Method to load attachments
        private void LoadAttachments(string noteId)
        {
            DatabaseHelper.LoadAttachments(noteId, attachedFilesTable, dataGridView1, (msg, method, table) => LogError(msg, method, table), message => MessageBox.Show(message));
        }






        private void BtnRightPage_Click(object sender, EventArgs e)
        {

            // Handles the Click event for the BtnRightPage button.
            // It navigates to the next note in the notesList, wrapping around to the first note if necessary.
            // It also closes any open preview form and updates the navigation buttons.

            if (notesList.Count < 2)
                return;

            ClosePreviewForm();

            if (currentIndex < notesList.Count - 1)
            {
                currentIndex++;
            }
            else
            {
                currentIndex = 0; // Wrap around to the first note
            }
            DisplayNoteInOrder();
            UpdateNavigationButtons();  // Update navigation buttons state
        }

        private void BtnLeftPage_Click(object sender, EventArgs e)
        {

            // Handles the Click event for the BtnLeftPage button.
            // It navigates to the previous note in the notesList, wrapping around to the last note if necessary.
            // It also closes any open preview form and updates the navigation buttons.

            if (notesList.Count < 2)
                return;

            ClosePreviewForm();

            if (currentIndex > 0)
            {
                currentIndex--;
            }
            else
            {
                currentIndex = notesList.Count - 1; // Wrap around to the last note
            }
            DisplayNoteInOrder();
            UpdateNavigationButtons();  // Update navigation buttons state
        }


        /*
         
        I Learnt how to mass comment!! wahey!

        Below has been moved so I can use the change detection on multiple RichTextBox areas with RichTextBoxHelper class.


        private void InitializeChangeDetection()
        {
            // Add event handlers for all the controls you want to track
            ComboBoxProducts.TextChanged += OnNoteChanged;
            SubcategoryComboBox.TextChanged += OnNoteChanged;
            TextBoxSubject.TextChanged += OnNoteChanged;
            TextBoxKeywords.TextChanged += OnNoteChanged;
            RichTextBoxDetails.TextChanged += OnNoteChanged;

            BtnBold.Click += OnNoteChanged;
            BtnItalic.Click += OnNoteChanged;
            BtnUnderline.Click += OnNoteChanged;
            ComboBoxFont.SelectedIndexChanged += OnNoteChanged;
            ComboBoxSize.SelectedIndexChanged += OnNoteChanged;
            BtnTextColour.Click += OnNoteChanged;
            BtnStrikeOut.Click += OnNoteChanged;

            BtnAttachFiles.Click += OnNoteChanged;

            // Initialize the flag
            hasIncrementedNoteNumber = false;
        }

        */


        private void OnNoteChanged(object sender, EventArgs e)
        {
            if (!hasIncrementedNoteNumber)
            {
                IncrementNoteNumber();
                hasIncrementedNoteNumber = true;
            }
        }


        private void IncrementNoteNumber()
        {

            // Increments the note number by retrieving the next available number from the database.
            // It updates the TextBoxID with the new note number.

            int nextNoteNumber = GetNextAvailableNoteNumber();
            TextBoxID.Text = nextNoteNumber.ToString();
        }

        private void InitializeAttachedFilesTable()
        {

            // Initializes the attachedFilesTable with columns for file ID, name, path, size, and type.

            attachedFilesTable = new DataTable();
            attachedFilesTable.Columns.Add("FileID", typeof(string)); // Add FileID column
            attachedFilesTable.Columns.Add("FileName", typeof(string));
            attachedFilesTable.Columns.Add("FilePath", typeof(string));
            attachedFilesTable.Columns.Add("SizeKB", typeof(long));
            attachedFilesTable.Columns.Add("FileType", typeof(string));
        }

        //Moved to DatabaseHelper due to shitty code
        private void LoadNotesByProduct(string productName, string subcategoryName = null)
        {
            DatabaseHelper.LoadNotesByProduct(productName, subcategoryName, notesList, DisplayNote, message => MessageBox.Show(message), ref currentIndex);
        }

        private string GetSubcategoryIdByName(string subcategoryName, string productId)
        {
            return DatabaseHelper.GetSubcategoryIdByName(subcategoryName, productId);
        }

        private string GetProductNameById(string productId)
        {
            return DatabaseHelper.GetProductNameById(productId);
        }

        private void BtnDeleteNote_Click(object sender, EventArgs e)
        {

            // Handles the Click event for the BtnDeleteNote button.
            // It deletes the current note if it exists and refreshes the note list.
            // If the note does not exist, it shows an error message.

            if (currentNoteId == Guid.Empty)
            {
                MessageBox.Show("Unable to delete a note that hasn't been created.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Console.WriteLine($"Attempting to delete note with ID: {currentNoteId}");

            if (!NoteExists(currentNoteId.ToString()))
            {
                MessageBox.Show("Note does not exist in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this note?", "Delete Note", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (DeleteNoteAndRefresh(currentNoteId.ToString()))
                {
                    MessageBox.Show("Note deleted successfully.");
                    LoadFirstAvailableNote();
                    UpdatePageAmount();
                }
            }
        }

        private void LoadFirstAvailableNote()
        {

            // Loads the first available note from the notesList.
            // If no notes are available, it clears the form and updates the page amount label.

            if (notesList.Count == 0)
            {
                ClearForm();
                LblPageAmount.Text = "Note 0/0";
                return;
            }

            currentIndex = 0; // Load the first note
            DisplayNoteInOrder();
        }


        private void RefreshToPreviousNote()
        {
            // Refreshes the form to display the previous note based on the current note number.
            // If no previous note is found, it clears the form.

            // Assuming TextBoxID holds the current note number
            int currentNoteNumber = int.TryParse(TextBoxID.Text, out int result) ? result : 0;
            if (currentNoteNumber > 1)
            {
                LoadNoteByNoteNumber(currentNoteNumber - 1);
            }
            else if (notesList.Count > 0)
            {
                currentIndex = 0;
                DisplayNoteInOrder();
            }
            else
            {
                ClearForm();
            }
        }

        private void UpdatePageAmount()
        {

            // Updates the page amount label based on the number of notes in the notesList.

            if (notesList.Count > 0)
            {
                LblPageAmount.Text = $"Note {currentIndex + 1}/{notesList.Count}";
            }
            else
            {
                LblPageAmount.Text = "Note 0/0";
            }
        }

        // Method to load note by note number
        private void LoadNoteByNoteNumber(int noteNumber)
        {
            DatabaseHelper.LoadNoteByNoteNumber(noteNumber, SetNoteDetails, ClearForm, Console.WriteLine, ref currentNoteId);
        }

        private void SetNoteDetails(Note note)
        {

            // Sets the form fields with the details of the specified note.
            // It updates the text boxes, combo boxes, and enables editing controls.

            TextBoxID.Text = note.NoteNumber.ToString();
            TextBoxSubject.Text = note.Subject;
            RichTextBoxDetails.Rtf = note.Details; // Load RTF content
            TextBoxKeywords.Text = GetKeywordsByNoteId(note.NoteID);
            ComboBoxProducts.SelectedItem = GetProductNameById(note.ProductID);
            LoadSubcategoryOptions(note.ProductID, GetSubcategoryNameById(note.SubcategoryID));

            // Enable controls for editing
            ComboBoxProducts.Enabled = true;
            SubcategoryComboBox.Enabled = true;
            TextBoxSubject.Enabled = true;
            BtnBold.Enabled = true;
            BtnItalic.Enabled = true;
            BtnUnderline.Enabled = true;
            ComboBoxFont.Enabled = true;
            ComboBoxSize.Enabled = true;
            BtnTextColour.Enabled = true;
            BtnStrikeOut.Enabled = true;
            RichTextBoxDetails.Enabled = true;
            BtnAttachFiles.Enabled = true;
            BtnSaveNote.Enabled = true;

            // Ensure currentNoteId is set
            currentNoteId = Guid.Parse(note.NoteID);
        }

        // Method to get keywords by note ID
        private string GetKeywordsByNoteId(string noteId)
        {
            return DatabaseHelper.GetKeywordsByNoteId(noteId);
        }

        // Method to delete note and refresh
        private bool DeleteNoteAndRefresh(string noteId)
        {
            return DatabaseHelper.DeleteNoteAndRefresh(noteId, notesList, ref currentIndex);
        }


        private bool NoteExists(string noteId)
        {
            return DatabaseHelper.NoteExists(noteId);
        }

        public void RefreshNotesList()
        {

            // Refreshes the notes list based on the selected product and subcategory.
            // If no product is selected, it loads all notes from the database.


            if (ComboBoxProducts.SelectedItem != null)
            {
                string selectedProduct = ComboBoxProducts.SelectedItem.ToString();
                string productId = GetProductIdByName(selectedProduct);
                string subcategoryId = !string.IsNullOrEmpty(SubcategoryComboBox.Text) && SubcategoryComboBox.Text != "-- Select a Subcategory --"
                                       ? GetSubcategoryIdByName(SubcategoryComboBox.Text, productId)
                                       : null;

                LoadNotesByProductSubcategory(productId, subcategoryId);
            }
            else
            {
                LoadAllNotes();
            }
        }

        public void OpenSelectedNotes(List<Note> notes)
        {

            // Opens the selected notes and displays them in the main form.
            // It sorts the notes by note number and updates the notesList and currentIndex.
            // If no notes are selected, it shows a message indicating no notes are available.

            if (notes == null || notes.Count == 0)
            {
                MessageBox.Show("No notes selected.");
                return;
            }

            // Sort the notes based on NoteNumber to ensure they are in the correct order
            notes = notes.OrderBy(note => note.NoteNumber).ToList();

            notesList = notes;
            currentIndex = 0;  // Start from the first note

            DisplayNoteInOrder();
            UpdateNavigationButtons();
        }


        private void DisplayNoteInOrder()
        {

            // Displays the note at the current index in the notesList.
            // It updates the form fields with the note's details and logs the note view.
            // If no note is found, it clears the form and updates the page amount label.

            if (currentIndex >= 0 && currentIndex < notesList.Count)
            {
                var note = notesList[currentIndex];
                currentNoteId = Guid.Parse(note.NoteID);
                TextBoxID.Text = note.NoteNumber.ToString();
                TextBoxDate.Text = note.AddedDate.ToString("dd/MM/yyyy");
                ComboBoxProducts.SelectedItem = GetProductNameById(note.ProductID);

                LoadSubcategoryOptions(note.ProductID, GetSubcategoryNameById(note.SubcategoryID));

                TextBoxSubject.Text = note.Subject;
                try
                {
                    RichTextBoxDetails.Rtf = note.Details; // Try to load as RTF
                }
                catch (ArgumentException)
                {
                    // If RTF format is invalid, load as plain text
                    RichTextBoxDetails.Text = note.Details;
                }
                LoadAttachments(note.NoteID);
                TextBoxKeywords.Text = FetchKeywords(note.NoteID);

                LblPageAmount.Text = $"Note {currentIndex + 1}/{notesList.Count}";

                LogNoteView(note.NoteID, note.NoteNumber);  // Log the view each time a note is displayed
            }
            else
            {
                ClearForm(false);
                LblPageAmount.Text = "Note 0/0";
            }
        }


        // Method to load subcategory options
        private void LoadSubcategoryOptions(string productId, string selectedSubcategory = null)
        {
            DatabaseHelper.LoadSubcategoryOptions(productId, SubcategoryComboBox, LblSubcategory, selectedSubcategory);
        }


        //Moved to DatabaseHelper to avoid DB clutter.
        private string GetProductIdByName(string productName)
        {
            return DatabaseHelper.GetProductIdByName(productName);
        }

        //Moved to DatabaseHelper to avoid DB clutter.
        private int GetNextAvailableNoteNumber()
        {
            return DatabaseHelper.GetNextAvailableNoteNumber();
        }


        //Moved to DatabaseHelper to avoid DB clutter.
        public void LogNoteView(string noteID, int noteNumber)
        {
            DatabaseHelper.LogNoteView(noteID, noteNumber);
        }

        //Moved to DatabaseHelper to avoid DB clutter.
        private bool InsertNewNoteIntoDatabase(Note note)
        {
            return DatabaseHelper.InsertOrUpdateNoteIntoDatabase(note);
        }



        private Note GetCurrentNote()
        {

            // Retrieves the current note details from the form fields.
            // It handles creating new product and subcategory IDs if necessary.
            // It returns a Note object with the current note details or null if validation fails.

            if (string.IsNullOrEmpty(ComboBoxProducts.Text) || string.IsNullOrEmpty(TextBoxSubject.Text) || string.IsNullOrEmpty(RichTextBoxDetails.Text))
            {
                MessageBox.Show("Please ensure a product is selected and all fields are properly set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            // Handle new product creation if necessary
            string productId = GetOrCreateProductIdByName(ComboBoxProducts.Text);

            // Handle new subcategory creation if necessary
            string subcategoryId = null;
            if (!string.IsNullOrWhiteSpace(SubcategoryComboBox.Text) && SubcategoryComboBox.Text != "-- Select a Subcategory --" && SubcategoryComboBox.Text != "No Subcategory")
            {
                subcategoryId = GetOrCreateSubcategoryIdByName(SubcategoryComboBox.Text, productId);
            }

            return new Note
            {
                NoteID = currentNoteId != Guid.Empty ? currentNoteId.ToString() : Guid.NewGuid().ToString(),
                NoteNumber = int.TryParse(TextBoxID.Text, out int noteNumber) ? noteNumber : 0,
                AddedDate = DateTime.Now,
                AddedTime = DateTime.Now,
                ProductID = productId,
                SubcategoryID = subcategoryId,
                Subject = TextBoxSubject.Text,
                Keywords = TextBoxKeywords.Text,
                Details = RichTextBoxDetails.Rtf, // Save as RTF
                ModifiedTime = DateTime.Now
            };
        }

        // Method to get or create product ID by name
        //Moved to DatabaseHelper to avoid DB clutter.
        private string GetOrCreateProductIdByName(string productName)
        {
            return DatabaseHelper.GetOrCreateProductIdByName(productName);
        }

        // Method to get or create subcategory ID by name
        //Moved to DatabaseHelper to avoid DB clutter.
        private string GetOrCreateSubcategoryIdByName(string subcategoryName, string productId)
        {
            return DatabaseHelper.GetOrCreateSubcategoryIdByName(subcategoryName, productId);
        }

        private void BtnCreateNotes_Click(object sender, EventArgs e)
        {

            // Handles the Click event for the BtnCreateNotes button.
            // It clears the form for creating a new note, refreshes the date, and applies default font settings.
            // It also updates the navigation buttons.

            isNewNote = true;
            ClearForm(true);
            isInSearchMode = false;
            RefreshDate();

            // Apply default font and size
            ApplyDefaultFontSettings();

            // Always update navigation buttons after creating a new note
            UpdateNavigationButtons();
        }

        // Method to apply default font settings
        //Moved to DatabaseHelper to avoid DB clutter.
        private void ApplyDefaultFontSettings()
        {
            DatabaseHelper.ApplyDefaultFontSettings(RichTextBoxDetails, ComboBoxFont, ComboBoxSize, isNewNote);
        }

        private void BtnExportNote_Click(object sender, EventArgs e)
        {

            // Handles the Click event for the BtnExportNote button.
            // It exports the current note to an XML file, generating a unique and user-friendly filename.
            // If the note ID is empty, it shows an error message.

            if (currentNoteId == Guid.Empty)
            {
                MessageBox.Show("No note selected to export.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Note note = GetCurrentNote();

            if (note == null)
            {
                MessageBox.Show("Failed to retrieve the note details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get the product name
            string productName = GetProductNameById(note.ProductID);

            // Generate a more user-friendly and unique filename 
            // Still not great 3/07/2024
            string dateStamp = DateTime.Now.ToString("ddMMyyyy"); // Use current date
            string noteNumber = note.NoteNumber.ToString("D2"); // Format note number with leading zeroes
            string sanitizedProductName = new string(productName.Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray()); // Remove invalid file name characters
            string fileName = $"Note{noteNumber}_{sanitizedProductName}_{dateStamp}.xml"; // Combine note number, product name, and date stamp

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "XML files (*.xml)|*.xml",
                DefaultExt = "xml",
                AddExtension = true,
                FileName = fileName // Use the generated filename
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                ExportNoteToXml(note, filePath);
                Logger.LogActivity("Export", $"Note {TextBoxID.Text} exported to {filePath}.");
            }
        }




        private void InitializeToolTip()
        {

            // Initializes tooltips for various buttons in the form.
            // It sets the tooltip text for each button to provide additional information to the user.

            toolTipImport.SetToolTip(BtnImportNote, "Import XML");
            toolTipImport.SetToolTip(BtnExportNote, "Export Note");
            toolTipImport.SetToolTip(BtnEmailNote, "Coming Soon");
            toolTipImport.SetToolTip(BtnDeleteNote, "Delete Note");
            toolTipImport.SetToolTip(BtnSearchNote, "Search Note");
            toolTipImport.SetToolTip(BtnCreateNotes, "Create A New Note");
            toolTipImport.SetToolTip(BtnSaveNote, "Save Note");
            toolTipImport.SetToolTip(BtnTOPNotes, "TOP Notes");
            toolTipImport.SetToolTip(BtnSettings, "Settings");
            toolTipImport.SetToolTip(BtnRefresh, "Refresh Note");
            toolTipImport.SetToolTip(BtnAttachFiles, "Attach File(s)");
            toolTipImport.SetToolTip(BtnDeleteFiles, "Delete Attached File(s)");

            // Adding tooltips for new controls
            toolTipImport.SetToolTip(BtnBold, "Bold");
            toolTipImport.SetToolTip(BtnItalic, "Italic");
            toolTipImport.SetToolTip(BtnUnderline, "Underline");
            toolTipImport.SetToolTip(ComboBoxFont, "Font");
            toolTipImport.SetToolTip(ComboBoxSize, "Font Size");
            toolTipImport.SetToolTip(BtnTextColour, "Text Colour");
            toolTipImport.SetToolTip(BtnStrikeOut, "Strikeout");
            toolTipImport.SetToolTip(BtnLeftAlign, "Left Align");
            toolTipImport.SetToolTip(BtnCenterAlign, "Center Align");
            toolTipImport.SetToolTip(BtnRightAlign, "Right Align");
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {

            // Handles mouse move events on the form.
            // If the mouse is over the BtnEmailNote button, it shows a tooltip if the button is disabled.
            // Otherwise, it hides the tooltip.

            if (BtnEmailNote.Bounds.Contains(e.Location) && !BtnEmailNote.Enabled)
            {
                toolTipImport.Show("Email Note", BtnEmailNote, BtnEmailNote.Width / 2, BtnEmailNote.Height / 2);
            }
            else
            {
                toolTipImport.Hide(BtnEmailNote);
            }
        }

        private void ExportNoteToXml(Note note, string filePath, bool showMessage = true)
        {

            // Exports the specified note to an XML file.
            // It constructs an XML document with the note details, including attachments, and saves it to the specified file path.
            // If showMessage is true, it shows a success message upon completion.

            try
            {
                string databaseId = GetDatabaseID();
                string productName = GetProductNameById(note.ProductID);
                string subcategoryName = GetSubcategoryNameById(note.SubcategoryID);

                // Ensure the version is set, defaulting to "1.0" if not already set
                string version = string.IsNullOrEmpty(note.Version) ? "1.0" : note.Version;

                XDocument doc = new XDocument(
                    new XElement("Note",
                        new XElement("Version", version),  // Ensure Version is included
                        new XElement("DatabaseID", databaseId),
                        new XElement("NoteID", note.NoteID),
                        new XElement("NoteNumber", note.NoteNumber),
                        new XElement("AddedDate", note.AddedDate.ToString("yyyy-MM-dd")),
                        new XElement("AddedTime", note.AddedTime.HasValue ? note.AddedTime.Value.ToString("HH:mm:ss") : string.Empty),
                        new XElement("ModifiedTime", note.ModifiedTime.HasValue ? note.ModifiedTime.Value.ToString("HH:mm:ss") : string.Empty),
                        new XElement("DeletedTime", note.DeletedTime.HasValue ? note.DeletedTime.Value.ToString("HH:mm:ss") : string.Empty),
                        new XElement("ProductID", note.ProductID),
                        new XElement("ProductName", productName),
                        new XElement("SubcategoryID", note.SubcategoryID),
                        new XElement("SubcategoryName", subcategoryName),
                        new XElement("ParentNoteID", note.ParentNoteID),
                        new XElement("Subject", note.Subject),
                        new XElement("Details", Convert.ToBase64String(Encoding.UTF8.GetBytes(note.Details))),
                        new XElement("Source", note.Source),
                        new XElement("Keywords", note.Keywords),
                        new XElement("Attachments",
                            from DataRow row in attachedFilesTable.Rows
                            select new XElement("Attachment",
                                new XElement("FileName", row["FileName"]),
                                new XElement("FileData", Convert.ToBase64String(File.ReadAllBytes(row["FilePath"].ToString())))
                            )
                        )
                    )
                );

                doc.Save(filePath);
                if (showMessage)
                {
                    MessageBox.Show("Note exported successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message, "ExportNoteToXml", "Notes");
                MessageBox.Show("Failed to export note.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnImportNote_Click(object sender, EventArgs e)
        {

            // Handles the Click event for the BtnImportNote button.
            // It opens a file dialog for selecting XML files to import and processes each selected file.
            // If all notes are imported successfully, it shows a success message; otherwise, it shows a warning message.

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml",
                DefaultExt = "xml",
                AddExtension = true,
                Multiselect = true // Allow multiple file selection
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bool allImported = true;
                foreach (string filePath in openFileDialog.FileNames)
                {
                    if (!ImportNoteFromXml(filePath))
                    {
                        allImported = false;
                    }
                    Logger.LogActivity("Import", $"Note imported from file: {Path.GetFileName(filePath)}.");
                }

                if (allImported)
                {
                    MessageBox.Show("All notes imported successfully.", "Import Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Some notes failed to import. Please check the log for details.", "Import Partial Success", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public bool ImportNoteFromXml(string filePath)
        {

            // Imports a note from an XML file.
            // It parses the XML, translates product and subcategory names, and inserts the note and attachments into the database.
            // If the note already exists, it shows a warning message.
            // If successful, it returns true; otherwise, it logs the error and returns false.

            try
            {
                // Clear the form completely before importing
                ClearForm(true);

                XDocument doc = XDocument.Load(filePath);
                XElement noteElement = doc.Element("Note");
                if (noteElement == null)
                {
                    MessageBox.Show("Invalid XML format: No 'Note' element found.", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                string noteVersion = noteElement.Element("Version")?.Value ?? "1.0"; // Default to 1.0 if version is not specified
                string noteId = noteElement.Element("NoteID")?.Value;

                if (NoteExists(noteId))
                {
                    MessageBox.Show("The note already exists in the database.", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                string productName = noteElement.Element("ProductName")?.Value;
                string subcategoryName = noteElement.Element("SubcategoryName")?.Value;

                // Ensure product translation
                string productId = EnsureTranslation("Product", productName, GetDatabaseID());
                if (productId == null)
                {
                    MessageBox.Show("Product translation was cancelled or failed. Import aborted.", "Translation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                Console.WriteLine($"Product ID: {productId}, Product Name: {productName}");

                // Ensure subcategory translation
                string subcategoryId = EnsureTranslation("Subcategory", subcategoryName, GetDatabaseID(), productId);
                if (subcategoryId == null)
                {
                    MessageBox.Show("Subcategory translation was cancelled or failed. Import aborted.", "Translation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                Console.WriteLine($"Subcategory ID: {subcategoryId}, Subcategory Name: {subcategoryName}");

                DateTime addedDate = DateTime.Now;
                DateTime addedTime = DateTime.Now;

                if (DateTime.TryParse(noteElement.Element("AddedDate")?.Value, out DateTime parsedAddedDate))
                {
                    addedDate = parsedAddedDate;
                }

                string addedTimeStr = noteElement.Element("AddedTime")?.Value;
                if (!string.IsNullOrEmpty(addedTimeStr) && DateTime.TryParse(addedTimeStr, out DateTime parsedAddedTime))
                {
                    addedTime = parsedAddedTime;
                }

                string details = noteElement.Element("Details")?.Value;
                string rtfDetails;

                try
                {
                    rtfDetails = Encoding.UTF8.GetString(Convert.FromBase64String(details));
                }
                catch (FormatException)
                {
                    // If base64 decoding fails, treat it as plain text
                    rtfDetails = details;
                }

                Note newNote = new Note
                {
                    NoteID = noteId,
                    Version = "1.0",  // Set the version here
                    NoteNumber = GetNextAvailableNoteNumber(),
                    ProductID = productId,
                    SubcategoryID = subcategoryId,
                    Subject = noteElement.Element("Subject")?.Value,
                    Details = rtfDetails, // Use the RTF details
                    Keywords = noteElement.Element("Keywords")?.Value,
                    AddedDate = addedDate,
                    AddedTime = addedTime,
                    Source = Environment.UserName,
                };

                if (InsertNewNoteIntoDatabase(newNote))
                {
                    ImportAttachments(noteElement.Element("Attachments"), newNote.NoteID);

                    // Load the imported note details into the form
                    LoadNoteDetails(newNote);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message, "ImportNoteFromXml", "Notes");
                MessageBox.Show("Failed to import the note: " + ex.Message, "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        private string EnsureTranslation(string translationType, string sourceValue, string databaseId, string productId = null)
        {
            return DatabaseHelper.EnsureTranslation(translationType, sourceValue, databaseId, productId);
        }


        private void EnsureOptionsTable()
        {
            string seasonalOptionID = "seasonalEffect"; 
            DatabaseHelper.EnsureOptionsTable(seasonalOptionID);
        }

        // Updates the navigation buttons based on the current state of the notes list.
        private void UpdateNavigationButtons()
        {

            // Updates the navigation buttons based on the current state of the notes list.
            // It enables or disables buttons depending on whether there are multiple notes to navigate through.

            BtnLeftPage.Enabled = notesList.Count > 1;
            BtnRightPage.Enabled = notesList.Count > 1;
        }

        private void NippyNotes_Load(object sender, EventArgs e)
        {

            // Initializes the form with default settings, loads the last opened note or prepares a new note, and sets up the form elements.
            // This is the starting point when the application loads, ensuring all elements are set up correctly.

            // Initialize ComboBoxes with font names and sizes
            InitializeComboBoxes();

            // Ensure ComboBoxFont and ComboBoxSize reflect the default font settings
            SetDefaultFontInComboboxes();

            // Apply default font and size to the RichTextBox
            SetDefaultFontAndSize();

            string startupBehavior = GetStartupBehavior();
            if (startupBehavior == "LastOpened")
            {
                LoadLastOpenedNote();
            }
            else
            {
                ClearForm(true);
                PopulateNextNoteID();
                RefreshDate();
            }
            Console.WriteLine($"NippyNotes_Load: TextBoxID is {TextBoxID.Text}");
            UpdateNavigationButtons();
            InitializeToolTip();

            CenterFormOnScreen(this);

            // Load autocomplete data
            LoadAutoCompleteData();

            // Attach event handlers if needed
            ComboBoxProducts.KeyDown += ComboBox_KeyDown;
            SubcategoryComboBox.KeyDown += ComboBox_KeyDown;
        }

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {

            // Handles key down events for combo boxes to enable selection using the Enter key.
            // It allows users to select items in the combo box by pressing Enter.

            if (e.KeyCode == Keys.Enter)
            {
                ComboBox comboBox = sender as ComboBox;
                if (comboBox != null && comboBox.AutoCompleteMode == AutoCompleteMode.SuggestAppend)
                {
                    comboBox.SelectedItem = comboBox.Text;
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void SetDefaultFontAndSize()
        {
            DatabaseHelper.SetDefaultFontAndSize(RichTextBoxDetails);
        }


        // Existing method without parameters
        public void SetDefaultFontInComboboxes()
        {
            DatabaseHelper.SetDefaultFontInComboboxes(ComboBoxFont, ComboBoxSize, RichTextBoxDetails);
        }

        private void InitializeComboBoxes()
        {

            // Initializes the font and size comboboxes with available fonts and sizes.
            // Populates the dropdown lists with all possible fonts and sizes for user selection.

            ComboBoxFont.Items.Clear();
            foreach (FontFamily font in System.Drawing.FontFamily.Families)
            {
                ComboBoxFont.Items.Add(font.Name);
            }

            // Populate size ComboBox
            ComboBoxSize.Items.Clear();
            for (int i = 8; i <= 72; i += 2)
            {
                ComboBoxSize.Items.Add(i.ToString());
            }

            // Enable AutoComplete for ComboBoxProducts
            ComboBoxProducts.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ComboBoxProducts.AutoCompleteSource = AutoCompleteSource.CustomSource;

            // Enable AutoComplete for SubcategoryComboBox
            SubcategoryComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            SubcategoryComboBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        // Method to load autocomplete data
        //Moved to DatabaseHelper to avoid DB clutter.
        private void LoadAutoCompleteData()
        {
            DatabaseHelper.LoadAutoCompleteData(ComboBoxProducts, SubcategoryComboBox);
        }

        // Method to get startup behavior
        //Moved to DatabaseHelper to avoid DB clutter.
        private string GetStartupBehavior()
        {
            return DatabaseHelper.GetStartupBehavior(startupOptionID);
        }

        // Method to load last opened note
        //Moved to DatabaseHelper to avoid DB clutter.
        public void LoadLastOpenedNote()
        {
            DatabaseHelper.LoadLastOpenedNote(LoadNoteDetails, ClearForm, RefreshDate, PopulateNextNoteID, ref currentNoteId);
        }


        private void NippyNotes_Activated(object sender, EventArgs e)
        {

            // Brings the search form to the front when NippyNotes is activated.
            // Ensures the search form is visible and accessible when the main form is in focus.

            if (searchForm != null && !searchForm.IsDisposed)
            {
                searchForm.BringToFront();
            }
        }

        private void NippyNotes_SizeChanged(object sender, EventArgs e)
        {

            // Positions the search form when the main form size changes.
            // Adjusts the location of the search form whenever the main form is resized.

            PositionSearchForm();
        }

        private void NippyNotes_Move(object sender, EventArgs e)
        {

            // Positions the search form when the main form is moved.
            // Ensures the search form follows the main form's new position.

            PositionSearchForm();
        }

        private void NippyNotes_Resize(object sender, EventArgs e)
        {

            // Closes the preview form when the main form is resized to minimized or maximized.
            // This is to ensure that the preview form is closed in these specific states.

            if (this.WindowState == FormWindowState.Minimized || this.WindowState == FormWindowState.Maximized)
            {
                ClosePreviewForm();
            }
        }

        private void ClosePreviewForm()
        {

            // Closes and disposes the preview form.
            // Ensures resources are released when the preview form is no longer needed due to finding memory leak.

            if (previewForm != null && !previewForm.IsDisposed)
            {
                previewForm.Close();
                previewForm.Dispose();
                previewForm = null;
            }
        }

        private void PositionSearchForm()
        {

            // Positions the search form next to the main form.
            // Keeps the search form aligned with the main form for easy access.

            if (searchForm != null && !searchForm.IsDisposed)
            {
                int offsetX = 16;
                int offsetY = 32;
                searchForm.Location = new Point(this.Location.X + this.Width + offsetX, this.Location.Y + offsetY);
            }
        }


        private void ImportAttachments(XElement attachmentsElement, string noteId)
        {

            // Imports attachments from an XElement and saves them to the specified note.
            // Handles the extraction and saving of attachments included in an XML element to a note.

            if (attachmentsElement == null) return;

            try
            {
                foreach (XElement attachmentElement in attachmentsElement.Elements("Attachment"))
                {
                    string fileName = attachmentElement.Element("FileName")?.Value;
                    string fileDataBase64 = attachmentElement.Element("FileData")?.Value;

                    byte[] fileData = Convert.FromBase64String(fileDataBase64);

                    string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
                    File.WriteAllBytes(tempFilePath, fileData);

                    DataRow row = attachedFilesTable.NewRow();
                    row["FileName"] = fileName;
                    row["FilePath"] = tempFilePath;
                    row["SizeKB"] = fileData.Length / 1024;
                    row["FileType"] = Path.GetExtension(fileName);
                    attachedFilesTable.Rows.Add(row);
                }

                SaveAttachments(noteId);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, "ImportAttachments", "Files");
                MessageBox.Show("Failed to import attachments: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #region SeasonalEffects

        // Method to apply seasonal effect
        //Christmas,Halloween,Easter,
        private void ApplySeasonalEffect()
        {
            DatabaseHelper.ApplySeasonalEffect(seasonalOptionID, StartSnowfallEffect, StartHalloweenEffect, StartEasterEffect);
        }

        private void InitializeSeasonalTimer()
        {

            // Initializes the timer for seasonal effects animation.
            // Sets up a timer to create smooth animations for the seasonal effects.

            if (seasonalTimer != null)
            {
                seasonalTimer.Stop();
                seasonalTimer.Tick -= SeasonalTimer_Tick;
            }
            seasonalTimer = new Timer();
            seasonalTimer.Interval = 30; // Set a consistent interval for smooth animation due to looking rough
            seasonalTimer.Tick += SeasonalTimer_Tick;
            seasonalTimer.Start();
        }

        private void StartSnowfallEffect()
        {

            // Starts the snowfall effect for the Christmas season.
            // Adds various Christmas-themed elements like snowflakes, snowmen, and Santa to the form.

            Random rand = new Random();
            Image snowflake1 = Properties.Resources.snowflake1;
            Image snowman = Properties.Resources.snowman;
            Image santa = Properties.Resources.santa;

            int numberOfElements = 20; // Adjust the number of elements for performance
            seasonalElements.Clear();

            for (int i = 0; i < numberOfElements; i++)
            {
                int size = rand.Next(20, 50);
                PointF speed = new PointF((float)(rand.NextDouble() - 0.5) * 4, (float)(rand.NextDouble() - 0.5) * 4); // Random speed and direction
                PointF initialPosition = new PointF(rand.Next(this.Width), rand.Next(this.Height));

                Image image;
                int randomChoice = rand.Next(4);
                switch (randomChoice)
                {
                    case 0:
                        image = snowflake1;
                        break;
                    case 1:
                        image = snowman;
                        break;
                    case 3:
                        image = santa;
                        break;
                    default:
                        image = snowflake1;
                        break;
                }

                seasonalElements.Add(new SeasonalElement(initialPosition, size, speed, Color.Transparent, image));
            }

            InitializeSeasonalTimer();
        }

        private void StartHalloweenEffect()
        {

            // Starts the Halloween effect with pumpkins and ghosts.
            // Adds Halloween-themed elements to the form for visual effect.

            Random rand = new Random();
            Image pumpkinImage = Properties.Resources.pumpkin1;
            Image pumpkinImage2 = Properties.Resources.pumpkin2;
            Image ghostImage1 = Properties.Resources.ghost1;
            Image ghostImage2 = Properties.Resources.ghost2;
            Image ghostImage3 = Properties.Resources.ghost3;

            int numberOfElements = 20; // Adjust the number of elements for performance
            seasonalElements.Clear();

            for (int i = 0; i < numberOfElements; i++)
            {
                int size = rand.Next(20, 50);
                PointF speed = new PointF((float)(rand.NextDouble() - 0.5) * 4, (float)(rand.NextDouble() - 0.5) * 4); // Random speed and direction
                PointF initialPosition = new PointF(rand.Next(this.Width), rand.Next(this.Height));

                Image image;
                if (i % 4 == 0)
                {
                    image = pumpkinImage;
                }
                else if (i % 4 == 1)
                {
                    image = pumpkinImage2;
                }
                else if (i % 4 == 1)
                {
                    image = ghostImage1;
                }
                else if (i % 4 == 2)
                {
                    image = ghostImage2;
                }
                else
                {
                    image = ghostImage3;
                }

                seasonalElements.Add(new SeasonalElement(initialPosition, size, speed, Color.Transparent, image));
            }

            InitializeSeasonalTimer();
        }

        private void StartEasterEffect()
        {

            // Starts the Easter effect with eggs.
            // Adds Easter-themed elements like eggs to the form.

            Random rand = new Random();
            Image eggImage1 = Properties.Resources.egg1;
            Image eggImage2 = Properties.Resources.egg2;

            int numberOfElements = 20; // Adjust the number of elements for performance
            seasonalElements.Clear();

            for (int i = 0; i < numberOfElements; i++)
            {
                int size = rand.Next(20, 50);
                PointF speed = new PointF((float)(rand.NextDouble() - 0.5) * 4, (float)(rand.NextDouble() - 0.5) * 4); // Random speed and direction
                PointF initialPosition = new PointF(rand.Next(this.Width), rand.Next(this.Height));
                Image image = (i % 2 == 0) ? eggImage1 : eggImage2;
                seasonalElements.Add(new SeasonalElement(initialPosition, size, speed, Color.Transparent, image));
            }

            InitializeSeasonalTimer();
        }




        private void SeasonalTimer_Tick(object sender, EventArgs e)
        {

            // Handles the seasonal timer tick event to update the positions of seasonal elements.
            // Moves the seasonal elements around the form on each timer tick.

            for (int i = 0; i < seasonalElements.Count; i++)
            {
                var element = seasonalElements[i];
                element.Position = new PointF(element.Position.X + element.Speed.X, element.Position.Y + element.Speed.Y);

                // Bounce off the edges of the form to look like the awesome 'DVD' screensavers
                if (element.Position.X <= 0 || element.Position.X + element.Size >= this.Width)
                {
                    element.Speed = new PointF(-element.Speed.X, element.Speed.Y);
                }
                if (element.Position.Y <= 0 || element.Position.Y + element.Size >= this.Height)
                {
                    element.Speed = new PointF(element.Speed.X, -element.Speed.Y);
                }

                seasonalElements[i] = element;
            }
            this.Invalidate();
        }

        private void NippyNotes_Paint(object sender, PaintEventArgs e)
        {

            // Paints the seasonal elements on the form.
            // Renders the seasonal elements to the screen.

            Graphics g = e.Graphics;
            foreach (var element in seasonalElements)
            {
                if (element.Image != null)
                {
                    g.DrawImage(element.Image, element.Position.X, element.Position.Y, element.Size, element.Size);
                }
                else
                {
                    using (Brush brush = new SolidBrush(element.Color))
                    {
                        g.FillEllipse(brush, element.Position.X, element.Position.Y, element.Size, element.Size);
                    }
                }
            }
        }


        public void UpdateSeasonalEffects(bool enable)
        {

            // Updates seasonal effects based on the enable parameter.
            // Turns the seasonal effects on or off.

            if (enable)
            {
                ApplySeasonalEffect(); // This should start the seasonal effects
            }
            else
            {
                ClearSeasonalEffects(); // This should stop and clear the effects
            }
        }


        private void ClearSeasonalEffects()
        {

            // Clears all seasonal effects and stops the timer.
            // Removes all seasonal elements and stops the animation.

            seasonalElements.Clear();
            if (seasonalTimer != null)
            {
                seasonalTimer.Stop();
                seasonalTimer.Tick -= SeasonalTimer_Tick;
                seasonalTimer = null;
            }
            this.Invalidate();
        }

        public class SeasonalElement
        {

            // Class representing a seasonal element with position, size, speed, color, and optional image.
            // Used to define properties for each seasonal decoration element.

            public PointF Position { get; set; }
            public int Size { get; set; }
            public PointF Speed { get; set; }
            public Color Color { get; set; }
            public Image Image { get; set; }

            public SeasonalElement(PointF position, int size, PointF speed, Color color, Image image = null)
            {
                Position = position;
                Size = size;
                Speed = speed;
                Color = color;
                Image = image;
            }
        }

        private void BtnTOPNotes_Click(object sender, EventArgs e)
        {

            // Opens a form to select the number of top notes to display and fetches them.
            // Allows the user to choose how many top notes they want to see.

            using (TopNotesForm topNotesForm = new TopNotesForm())
            {
                if (topNotesForm.ShowDialog() == DialogResult.OK)
                {
                    int topNotesCount = topNotesForm.TopNotesCount;
                    FetchAndDisplayTopNotes(topNotesCount);
                }
            }
        }

        // Method to fetch and display top notes
        //Moved to DatabaseHelper to avoid DB clutter.
        private void FetchAndDisplayTopNotes(int topNotesCount)
        {
            DatabaseHelper.FetchAndDisplayTopNotes(topNotesCount, OpenSelectedNotes, UpdateNavigationButtons);
        }

        // It shows as button1 but not sure why..
        private void button1_Click(object sender, EventArgs e)
        {

            // Clears the form and resets labels.
            // Prepares the form for a new note or to be blank.

            ClearForm(true);

            LblPageAmount.Text = " ";
            LblStatus.Text = " ";
        }

        //This doesn't work and is disabled in v1 even with ChatGPT - Mailto; is an issue I believe.


        private void BtnEmailNote_Click(object sender, EventArgs e)
        {
            // Opens the current note in an email client as an attachment.
            // Prepares and sends the note via email.

            IntegerContextAttribute x = new IntegerContextAttribute(null, 0, 0);

            // Check if a note is currently selected/opened
            if (currentNoteId == Guid.Empty || string.IsNullOrEmpty(TextBoxID.Text))
            {
                MessageBox.Show("Please open a note before attempting to email it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get the current note
            Note currentNote = GetCurrentNote();

            if (currentNote == null)
            {
                MessageBox.Show("Failed to retrieve the note details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Generate a more user-friendly and unique filename
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss"); // Use current date and time
            string sanitizedSubject = new string(currentNote.Subject.Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray()); // Remove invalid file name characters
            string productName = GetProductNameById(currentNote.ProductID);
            string fileName = $"{productName}_{sanitizedSubject}_{timestamp}.xml"; // Combine product name, subject, and timestamp

            // Create the temporary file path
            string tempFolderPath = Path.Combine(Application.StartupPath, "EmailTempXML");
            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }
            string tempFilePath = Path.Combine(tempFolderPath, fileName);

            // Export the note to the XML file
            ExportNoteToXml(currentNote, tempFilePath, false); // Do not show the success message

            // Open the default email client with the attachment

        }

        private void BtnDeleteFiles_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please select a file to delete.", "No File Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete the selected file(s)?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }

            foreach (DataGridViewCell selectedCell in dataGridView1.SelectedCells)
            {
                int rowIndex = selectedCell.RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[rowIndex];

                string filePath = selectedRow.Cells["FilePath"].Value.ToString();
                string fileId = selectedRow.Cells["FileID"].Value.ToString(); // Assuming you have a FileID column to uniquely identify the file

                // Remove the attachment from the database
                DeleteAttachmentFromDatabase(fileId);

                // Remove the attachment from the DataGridView
                dataGridView1.Rows.Remove(selectedRow);

                // Optionally, remove the physical file (if necessary)
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            // Refresh the DataGridView and any related data
            LoadAttachments(currentNoteId.ToString());
        }

        //Moved to DatabaseHelper to avoid DB clutter.
        private void DeleteAttachmentFromDatabase(string fileId)
        {
            DatabaseHelper.DeleteAttachmentFromDatabase(fileId);
        }
    }
}
#endregion