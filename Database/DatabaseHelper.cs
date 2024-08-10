using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static Nippy_Notes.NippyNotes;
using Nippy_Notes;
using System.Net.Mail;
using System.Net;
using Nippy_Notes.Security;
using System.Text;

namespace Nippy_Notes
{
    internal static class DatabaseHelper
    {
        private static readonly string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string DatabaseFolder = Path.Combine(AppDataFolder, "NippyNotes");
        private static readonly string DatabasePath = Path.Combine(DatabaseFolder, "NippyDB.db");
        private static readonly string ConnectionString = $"Data Source={DatabasePath};Version=3;";
        private static readonly string DatabaseFilePath = GetDatabaseFilePath();


        static DatabaseHelper()
        {
            // Ensure the database folder exists
            if (!Directory.Exists(DatabaseFolder))
            {
                Directory.CreateDirectory(DatabaseFolder);
            }

            // Ensure the database exists
            if (!File.Exists(DatabasePath))
            {
                SQLiteConnection.CreateFile(DatabasePath);
                InitializeDatabase();
            }
        }

        #region SMTP

        public class SmtpDetails
        {
            public string Host { get; set; }
            public int Port { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string FromEmail { get; set; }
        }

        public static SmtpDetails GetSmtpDetails()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT Host, Port, Username, Password, FromEmail FROM SmtpSettings LIMIT 1";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new SmtpDetails
                            {
                                Host = reader.GetString(0),
                                Port = reader.GetInt32(1),
                                Username = reader.GetString(2),
                                Password = reader.GetString(3),
                                FromEmail = reader.GetString(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        #endregion SMTP

        #region NippyNotes Form


        public static string GetDatabaseID()
        {
            const string predefinedOptionID = "databaseID";
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT OptionValue FROM Options WHERE OptionID = @OptionID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OptionID", predefinedOptionID);
                    return command.ExecuteScalar()?.ToString();
                }
            }
        }

        public static string GetSubcategoryNameById(string subcategoryId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT SubcategoryName FROM Subcategories WHERE SubcategoryID = @SubcategoryID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SubcategoryID", subcategoryId);
                    return command.ExecuteScalar()?.ToString();
                }
            }
        }

        public static void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();
                string createOptionsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Options (
                        OptionID TEXT PRIMARY KEY,
                        OptionValue TEXT NOT NULL,
                        OptionDescription TEXT NOT NULL
                    )";

                using (var command = new SQLiteCommand(createOptionsTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
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

        private static void ExecuteNonQuery(string query)
        {
            // Executes a non-query SQL command (like CREATE TABLE, INSERT, UPDATE, DELETE) against the database.
            // It opens a connection, executes the command, and handles any exceptions that occur.
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
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
                Console.WriteLine("Failed to execute query: " + ex.Message);
            }
        }

        private static void CreateTable_Notes()
        {
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

        private static void CreateTable_Products()
        {
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

        private static void CreateTable_Subcategories()
        {
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

        private static void CreateTable_ProductSub()
        {
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

        private static void CreateTable_Files()
        {
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

        private static void CreateTable_ErrorLog()
        {
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

        private static void CreateTable_Keywords()
        {
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Keywords (
                    KeywordID INTEGER PRIMARY KEY AUTOINCREMENT,
                    NoteID TEXT,
                    Keyword TEXT,
                    FOREIGN KEY(NoteID) REFERENCES Notes(NoteID) ON DELETE CASCADE
                )";
            ExecuteNonQuery(createTableQuery);
        }

        private static void CreateTable_Options()
        {
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

        private static void CreateTable_ImportTranslations()
        {
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

        private static void CreateTable_ActivityLog()
        {
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS ActivityLog (
                    LogID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Timestamp TEXT NOT NULL,
                    ActionType TEXT NOT NULL,
                    Details TEXT NOT NULL
                )";
            ExecuteNonQuery(createTableQuery);
        }

        private static void CreateTable_NoteViews()
        {
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS NoteViews (
                    NoteID TEXT PRIMARY KEY,
                    NoteNumber INTEGER,
                    ViewCount INTEGER DEFAULT 0,
                    FOREIGN KEY(NoteID) REFERENCES Notes(NoteID)
                )";
            ExecuteNonQuery(createTableQuery);
        }

        private static void CreateTable_BackupHistory()
        {
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

        private static void CreateTable_SmtpSettings()
        {
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

        public static void InsertRequiredOptions()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM Options WHERE OptionID = @OptionID";
                using (var command = new SQLiteCommand(checkQuery, connection))
                {
                    string predefinedOptionID = "defaultOption"; 
                    command.Parameters.AddWithValue("@OptionID", predefinedOptionID);
                    long count = (long)command.ExecuteScalar();

                    if (count == 0)
                    {
                        string insertQuery = @"
                            INSERT INTO Options (OptionID, OptionValue, OptionDescription)
                            VALUES (@OptionID, @OptionValue, @OptionDescription)";
                        using (var insertCommand = new SQLiteCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@OptionID", predefinedOptionID);
                            insertCommand.Parameters.AddWithValue("@OptionValue", Guid.NewGuid().ToString());
                            insertCommand.Parameters.AddWithValue("@OptionDescription", Guid.NewGuid().ToString());
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        //Somehow deleted this reference
        public static List<Note> LoadNotesByProduct(string productId, string subcategoryId = null)
        {
            var notesList = new List<Note>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT NoteID, NoteNumber, AddedDate, Subject, Details FROM Notes WHERE ProductID = @ProductID" +
                               (subcategoryId != null ? " AND SubcategoryID = @SubcategoryID" : "") +
                               " ORDER BY NoteNumber ASC";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);
                    if (subcategoryId != null)
                    {
                        command.Parameters.AddWithValue("@SubcategoryID", subcategoryId);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime addedDate;
                            if (!DateTime.TryParseExact(reader["AddedDate"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out addedDate))
                            {
                                throw new FormatException("Invalid date format for AddedDate.");
                            }

                            notesList.Add(new Note
                            {
                                NoteID = reader["NoteID"].ToString(),
                                NoteNumber = Convert.ToInt32(reader["NoteNumber"]),
                                AddedDate = addedDate,
                                Subject = reader["Subject"].ToString(),
                                Details = reader["Details"].ToString()
                            });
                        }
                    }
                }
            }

            return notesList;
        }

        public static int GetNextAvailableNoteNumber()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                string query = "SELECT MAX(NoteNumber) FROM Notes";
                using (var command = new SQLiteCommand(query, connection))
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        return Convert.ToInt32(result) + 1;
                    }
                    return 1;
                }
            }
        }

        public static string GetSubcategoryIdByName(string subcategoryName, string productId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT SubcategoryID FROM Subcategories WHERE SubcategoryName = @SubcategoryName AND ProductID = @ProductID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SubcategoryName", subcategoryName);
                    command.Parameters.AddWithValue("@ProductID", productId);
                    var result = command.ExecuteScalar();
                    return result?.ToString();
                }
            }
        }

        public static string GetProductNameById(string productId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT ProductName FROM Products WHERE ProductID = @ProductID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);
                    return command.ExecuteScalar()?.ToString();
                }
            }
        }

        public static bool NoteExists(string noteId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Notes WHERE NoteID = @NoteID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NoteID", noteId);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public static void SetDefaultFontAndSize(RichTextBox richTextBoxDetails)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string fontQuery = "SELECT OptionValue FROM Options WHERE OptionID = 'defaultFont'";
                string sizeQuery = "SELECT OptionValue FROM Options WHERE OptionID = 'defaultFontSize'";

                string defaultFontName = "Calibri"; // Fallback default font
                int defaultFontSize = 12; // Fallback default size

                using (var command = new SQLiteCommand(fontQuery, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        defaultFontName = result.ToString();
                    }
                }

                using (var command = new SQLiteCommand(sizeQuery, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        defaultFontSize = int.Parse(result.ToString());
                    }
                }

                Font defaultFont = new Font(defaultFontName, defaultFontSize);
                richTextBoxDetails.Font = defaultFont;
            }
        }

        public static void SetDefaultFontInComboboxes(ComboBox comboBoxFont, ComboBox comboBoxSize, RichTextBox richTextBoxDetails)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string fontQuery = "SELECT OptionValue FROM Options WHERE OptionID = 'defaultFont'";
                string sizeQuery = "SELECT OptionValue FROM Options WHERE OptionID = 'defaultFontSize'";

                string defaultFontName = "Calibri"; // Fallback default font
                int defaultFontSize = 12; // Fallback default size

                using (var command = new SQLiteCommand(fontQuery, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        defaultFontName = result.ToString();
                    }
                }

                using (var command = new SQLiteCommand(sizeQuery, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        defaultFontSize = int.Parse(result.ToString());
                    }
                }

                comboBoxFont.SelectedItem = defaultFontName;
                comboBoxSize.SelectedItem = defaultFontSize.ToString();

                // Set the default font for RichTextBoxDetails
                Font defaultFont = new Font(defaultFontName, defaultFontSize);
                richTextBoxDetails.Font = defaultFont;
            }
        }

        public static void LoadAutoCompleteData(ComboBox comboBoxProducts, ComboBox subcategoryComboBox)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                // Fetch products
                string productQuery = "SELECT ProductName FROM Products";
                using (var command = new SQLiteCommand(productQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        AutoCompleteStringCollection productCollection = new AutoCompleteStringCollection();
                        while (reader.Read())
                        {
                            productCollection.Add(reader["ProductName"].ToString());
                        }
                        comboBoxProducts.AutoCompleteCustomSource = productCollection;
                    }
                }

                // Fetch subcategories
                string subcategoryQuery = "SELECT SubcategoryName FROM Subcategories";
                using (var command = new SQLiteCommand(subcategoryQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        AutoCompleteStringCollection subcategoryCollection = new AutoCompleteStringCollection();
                        while (reader.Read())
                        {
                            subcategoryCollection.Add(reader["SubcategoryName"].ToString());
                        }
                        subcategoryComboBox.AutoCompleteCustomSource = subcategoryCollection;
                    }
                }
            }
        }

        public static string GetStartupBehavior(string startupOptionID)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT OptionValue FROM Options WHERE OptionID = @OptionID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OptionID", startupOptionID);
                    return command.ExecuteScalar()?.ToString() ?? "NewBlank";
                }
            }
        }

        public static void LoadLastOpenedNote(Action<Note> loadNoteDetails, Action<bool> clearForm, Action refreshDate, Action populateNextNoteID, ref Guid currentNoteId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = @"
        SELECT * FROM Notes
        ORDER BY NoteNumber DESC
        LIMIT 1";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Note lastNote = new Note
                            {
                                NoteID = reader["NoteID"].ToString(),
                                NoteNumber = Convert.ToInt32(reader["NoteNumber"]),
                                AddedDate = DateTime.Parse(reader["AddedDate"].ToString()),
                                AddedTime = reader["AddedTime"] != DBNull.Value ? (DateTime?)DateTime.Parse(reader["AddedTime"].ToString()) : null,
                                ModifiedTime = reader["ModifiedTime"] != DBNull.Value ? (DateTime?)DateTime.Parse(reader["ModifiedTime"].ToString()) : null,
                                DeletedTime = reader["DeletedTime"] != DBNull.Value ? (DateTime?)DateTime.Parse(reader["DeletedTime"].ToString()) : null,
                                ProductID = reader["ProductID"].ToString(),
                                SubcategoryID = reader["SubcategoryID"].ToString(),
                                Subject = reader["Subject"].ToString(),
                                Details = reader["Details"].ToString(),
                                Keywords = reader["Keywords"].ToString(),
                                Source = reader["Source"].ToString()
                            };
                            loadNoteDetails(lastNote);
                            currentNoteId = Guid.Parse(lastNote.NoteID);  // Ensure currentNoteId is correctly set
                        }
                        else
                        {
                            clearForm(true);
                            refreshDate();
                            populateNextNoteID();
                        }
                    }
                }
            }
        }


        public static void ApplySeasonalEffect(string seasonalOptionID, Action startSnowfallEffect, Action startHalloweenEffect, Action startEasterEffect)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT OptionValue FROM Options WHERE OptionID = @OptionID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OptionID", seasonalOptionID);
                    var seasonalEffectEnabled = command.ExecuteScalar()?.ToString() == "true";
                    if (seasonalEffectEnabled)
                    {
                        var currentMonth = DateTime.Now.Month;
                        if (currentMonth == 12) // December for Christmas
                        {
                            startSnowfallEffect();
                        }
                        else if (currentMonth == 10) // October for Halloween
                        {
                            startHalloweenEffect();
                        }
                        else if (currentMonth == 04) // April for Easter
                        {
                            startEasterEffect();
                        }
                    }
                }
            }
        }

        public static void FetchAndDisplayTopNotes(int topNotesCount, Action<List<Note>> openSelectedNotes, Action updateNavigationButtons)
        {
            string query = $@"
            SELECT n.NoteID, n.NoteNumber, n.ProductID, n.SubcategoryID, n.Subject, n.Details, n.AddedDate, v.ViewCount
            FROM Notes n
            INNER JOIN NoteViews v ON n.NoteID = v.NoteID
            ORDER BY v.ViewCount DESC
            LIMIT @TopNotesCount";

            List<Note> topNotes = new List<Note>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TopNotesCount", topNotesCount);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Note note = new Note
                            {
                                NoteID = reader["NoteID"].ToString(),
                                NoteNumber = Convert.ToInt32(reader["NoteNumber"]),
                                ProductID = reader["ProductID"].ToString(),
                                SubcategoryID = reader["SubcategoryID"].ToString(),
                                Subject = reader["Subject"].ToString(),
                                Details = reader["Details"].ToString(),
                                AddedDate = DateTime.Parse(reader["AddedDate"].ToString()),
                               
                            };
                            topNotes.Add(note);
                        }
                    }
                }
            }

            openSelectedNotes(topNotes);
            updateNavigationButtons();
        }

        public static bool DeleteNoteAndRefresh(string noteId, List<Note> notesList, ref int currentIndex)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        var noteIdParam = new SQLiteParameter("@NoteID", noteId);

                        string deleteFilesQuery = "DELETE FROM Files WHERE NoteID = @NoteID";
                        using (SQLiteCommand cmdFiles = new SQLiteCommand(deleteFilesQuery, connection))
                        {
                            cmdFiles.Parameters.Add(noteIdParam);
                            cmdFiles.ExecuteNonQuery();
                        }

                        string deleteNoteQuery = "DELETE FROM Notes WHERE NoteID = @NoteID";
                        using (SQLiteCommand cmdNote = new SQLiteCommand(deleteNoteQuery, connection))
                        {
                            cmdNote.Parameters.Add(noteIdParam);
                            int rowsAffected = cmdNote.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }

                        transaction.Commit();
                        notesList.RemoveAll(n => n.NoteID == noteId);
                        if (currentIndex >= notesList.Count)
                        {
                            currentIndex = notesList.Count - 1;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the note: {ex.Message}");
                return false;
            }
        }

        public static void LoadAllNotes(List<Note> notesList, Action<int> displayNote, Action<string> showMessage)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string query = "SELECT NoteID, NoteNumber, AddedDate, ProductID, Subject, Details FROM Notes ORDER BY NoteNumber ASC";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    notesList.Clear();
                    while (reader.Read())
                    {
                        string rawDate = reader["AddedDate"].ToString();
                        DateTime addedDate;
                        if (!DateTime.TryParseExact(rawDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out addedDate))
                        {
                            showMessage($"Invalid date format for AddedDate: {rawDate} in record NoteID: {reader["NoteID"]}");
                            continue;
                        }

                        notesList.Add(new Note
                        {
                            NoteID = reader["NoteID"].ToString(),
                            NoteNumber = Convert.ToInt32(reader["NoteNumber"]),
                            AddedDate = addedDate,
                            ProductID = reader["ProductID"].ToString(),
                            Subject = reader["Subject"].ToString(),
                            Details = reader["Details"].ToString()
                        });
                    }

                    if (notesList.Count > 0)
                    {
                        displayNote(0);
                    }
                    else
                    {
                        showMessage("No notes found.");
                    }
                }
            }
        }

        public static void LoadAttachments(string noteId, DataTable attachedFilesTable, DataGridView dataGridView1, Action<string, string, string> logError, Action<string> showMessage)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string query = "SELECT FileID, FileName, FileData, SizeKB, FileType FROM Files WHERE NoteID = @NoteID";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NoteID", noteId);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        attachedFilesTable.Clear();
                        while (reader.Read())
                        {
                            string fileId = reader["FileID"].ToString();
                            string fileName = reader["FileName"].ToString();
                            byte[] fileData = (byte[])reader["FileData"];

                            // Generate a unique temporary file name to prevent conflicts and ensure it's deleted later
                            string tempFileName = Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                            string tempFilePath = Path.Combine(Path.GetTempPath(), tempFileName);

                          
                            try
                            {
                                File.WriteAllBytes(tempFilePath, fileData); // Simpler than using FileStream directly for this purpose

                                DataRow row = attachedFilesTable.NewRow();
                                row["FileID"] = fileId; // Store FileID in DataTable but don't display it
                                row["FileName"] = fileName;
                                row["FilePath"] = tempFilePath;

                                // Read the SizeKB value safely
                                object sizeKBValue = reader["SizeKB"];
                                if (sizeKBValue is long)
                                {
                                    row["SizeKB"] = (long)sizeKBValue;
                                }
                                else if (sizeKBValue is int)
                                {
                                    row["SizeKB"] = (int)sizeKBValue;
                                }
                                else if (sizeKBValue is DBNull)
                                {
                                    row["SizeKB"] = 0;
                                }
                                else
                                {
                                    throw new InvalidCastException("SizeKB column is not a valid integer type.");
                                }

                                row["FileType"] = reader["FileType"].ToString();
                                attachedFilesTable.Rows.Add(row);
                            }
                            catch (IOException ex)
                            {
                                logError(ex.Message, "LoadAttachments", "Files");
                                showMessage($"Error loading attachment: {fileName}. It may be in use by another process.");
                            }
                        }
                    }
                }
            }

            // Bind the DataTable to the DataGridView without showing FileID column
            dataGridView1.DataSource = attachedFilesTable;
            if (dataGridView1.Columns.Contains("FileID"))
            {
                dataGridView1.Columns["FileID"].Visible = false; // Hide the FileID column
            }
            dataGridView1.Refresh();
        }



        public static void LoadNoteByNoteNumber(int noteNumber, Action<Note> setNoteDetails, Action<bool> clearForm, Action<string> logMessage, ref Guid currentNoteId)
        {
            logMessage($"Loading note with number: {noteNumber}");
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Notes WHERE NoteNumber = @NoteNumber";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NoteNumber", noteNumber);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var note = new Note
                            {
                                NoteID = reader["NoteID"].ToString(),
                                NoteNumber = Convert.ToInt32(reader["NoteNumber"]),
                                Subject = reader["Subject"].ToString(),
                                Details = reader["Details"].ToString(),
                           
                            };
                            currentNoteId = Guid.Parse(note.NoteID); // Ensure currentNoteId is set
                            setNoteDetails(note);
                        }
                        else
                        {
                            logMessage("No note found with the specified number.");
                            clearForm(false);
                        }
                    }
                }
            }
        }



        public static void LoadSubcategoryOptions(string productId, ComboBox subcategoryComboBox, Label lblSubcategory, string selectedSubcategory = null)
        {
            subcategoryComboBox.Items.Clear();
            subcategoryComboBox.Items.Add("No Subcategory"); // Add "No Subcategory" option

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT SubcategoryName FROM Subcategories WHERE ProductID = @ProductID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subcategoryComboBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }

            if (selectedSubcategory != null && subcategoryComboBox.Items.Contains(selectedSubcategory))
            {
                subcategoryComboBox.SelectedItem = selectedSubcategory;
            }
            else
            {
                subcategoryComboBox.SelectedItem = "No Subcategory";
            }

            subcategoryComboBox.Visible = true;
            lblSubcategory.Visible = true;
        }

        public static void ApplyDefaultFontSettings(RichTextBox richTextBoxDetails, ComboBox comboBoxFont, ComboBox comboBoxSize, bool isNewNote)
        {
            if (isNewNote)
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    string fontQuery = "SELECT OptionValue FROM Options WHERE OptionID = 'defaultFont'";
                    string sizeQuery = "SELECT OptionValue FROM Options WHERE OptionID = 'defaultFontSize'";

                    string defaultFontName = "Calibri"; // Fallback default font
                    int defaultFontSize = 12; // Fallback default size

                    using (var command = new SQLiteCommand(fontQuery, connection))
                    {
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            defaultFontName = result.ToString();
                        }
                    }

                    using (var command = new SQLiteCommand(sizeQuery, connection))
                    {
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            defaultFontSize = int.Parse(result.ToString());
                        }
                    }

                    Font defaultFont = new Font(defaultFontName, defaultFontSize);
                    richTextBoxDetails.SelectionFont = defaultFont;

                    comboBoxFont.SelectedItem = defaultFontName;
                    comboBoxSize.SelectedItem = defaultFontSize.ToString();
                }
            }
        }



        public static string EnsureTranslation(string translationType, string sourceValue, string databaseId, string productId = null)
        {
            string existingId = null;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = translationType == "Product" ?
                    "SELECT ProductID FROM Products WHERE ProductName = @Value" :
                    "SELECT SubcategoryID FROM Subcategories WHERE SubcategoryName = @Value AND ProductID = @ProductID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Value", sourceValue);
                    if (translationType == "Subcategory")
                    {
                        command.Parameters.AddWithValue("@ProductID", productId);
                    }
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        existingId = result.ToString();
                    }
                }
            }

            if (existingId == null)
            {
                Console.WriteLine($"No existing translation found for {translationType}: {sourceValue}, showing translation form.");
                using (var translationForm = new TranslationForm(sourceValue, translationType, "", "", productId))
                {
                    if (translationForm.ShowDialog() == DialogResult.OK)
                    {
                        if (translationForm.IsNewTranslation)
                        {
                            var newId = Guid.NewGuid().ToString();
                            using (var connection = new SQLiteConnection(ConnectionString))
                            {
                                connection.Open();
                                string insertQuery = translationType == "Product" ?
                                    "INSERT INTO Products (ProductID, ProductName) VALUES (@ID, @Value)" :
                                    "INSERT INTO Subcategories (SubcategoryID, SubcategoryName, ProductID) VALUES (@ID, @Value, @ProductID)";
                                using (var command = new SQLiteCommand(insertQuery, connection))
                                {
                                    command.Parameters.AddWithValue("@ID", newId);
                                    command.Parameters.AddWithValue("@Value", translationForm.TranslationValue);
                                    if (translationType == "Subcategory")
                                    {
                                        command.Parameters.AddWithValue("@ProductID", productId);
                                    }
                                    command.ExecuteNonQuery();
                                }
                            }
                            return newId;
                        }
                        else
                        {
                            return translationForm.SelectedTranslationId;
                        }
                    }
                }
            }

            return existingId;
        }

        public static void EnsureOptionsTable(string seasonalOptionID)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string createTableQuery = @"
        CREATE TABLE IF NOT EXISTS Options (
            OptionID TEXT PRIMARY KEY,
            OptionValue TEXT NOT NULL,
            OptionDescription TEXT NOT NULL
        )";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                string checkSeasonalOptionQuery = "SELECT COUNT(*) FROM Options WHERE OptionID = @OptionID";
                using (var command = new SQLiteCommand(checkSeasonalOptionQuery, connection))
                {
                    command.Parameters.AddWithValue("@OptionID", seasonalOptionID);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count == 0)
                    {
                        string insertSeasonalOptionQuery = "INSERT INTO Options (OptionID, OptionValue, OptionDescription) VALUES (@OptionID, @OptionValue, @OptionDescription)";
                        using (var insertCommand = new SQLiteCommand(insertSeasonalOptionQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@OptionID", seasonalOptionID);
                            insertCommand.Parameters.AddWithValue("@OptionValue", "false");
                            insertCommand.Parameters.AddWithValue("@OptionDescription", "Seasonal effect setting");
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }




        public static bool InsertNewNoteIntoDatabase(Note note)
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    string insertQuery = @"
                        INSERT INTO Notes (NoteID, NoteNumber, AddedDate, AddedTime, ProductID, SubcategoryID, Subject, Details, Source, Keywords, Version)
                        VALUES (@NoteID, @NoteNumber, @AddedDate, @AddedTime, @ProductID, @SubcategoryID, @Subject, @Details, @Source, @Keywords, @Version)";
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NoteID", note.NoteID);
                        command.Parameters.AddWithValue("@NoteNumber", note.NoteNumber);
                        command.Parameters.AddWithValue("@AddedDate", note.AddedDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@AddedTime", note.AddedTime.HasValue ? note.AddedTime.Value.ToString("HH:mm:ss") : (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ProductID", note.ProductID ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@SubcategoryID", note.SubcategoryID ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Subject", note.Subject);
                        command.Parameters.AddWithValue("@Details", note.Details); // Save as RTF
                        command.Parameters.AddWithValue("@Source", note.Source);
                        command.Parameters.AddWithValue("@Keywords", note.Keywords);
                        command.Parameters.AddWithValue("@Version", note.Version); // Pass the note version
                        command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while inserting the note: " + ex.Message);
                return false;
            }
        }

        public static void PopulateNextNoteID(TextBox textBoxID)
        {
            // Populates the next available note ID in the TextBoxID control.
            // It queries the database for the maximum note number and increments it for the next note.
            // If no notes exist, it sd the note number to 1.

            Console.WriteLine("PopulateNextNoteID: Method entered.");
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                string query = "SELECT MAX(NoteNumber) FROM Notes";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                try
                {
                    Console.WriteLine("PopulateNextNoteID: Opening database connection.");
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        int lastNoteNumber = Convert.ToInt32(result);
                        int nextNoteNumber = lastNoteNumber + 1;
                        textBoxID.Text = nextNoteNumber.ToString();
                        Console.WriteLine($"PopulateNextNoteID: Query executed. Result: {lastNoteNumber}");
                        Console.WriteLine($"UpdateTextBoxID: TextBoxID updated to {nextNoteNumber}");
                        Console.WriteLine($"PopulateNextNoteID: Next Note Number set to {nextNoteNumber}");
                    }
                    else
                    {
                        textBoxID.Text = "1";
                        Console.WriteLine("PopulateNextNoteID: Next Note Number set to 1");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error retrieving NoteNumber: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            CheckDatabaseDirectly();
        }

        public static void SaveAttachments(string noteId, DataTable attachedFilesTable)
        {
            // Saves the attached files associated with the specified note ID.
            // It deletes existing files for the note and inserts new file records into the database.

            if (attachedFilesTable == null || attachedFilesTable.Rows.Count == 0) return;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM Files WHERE NoteID = @NoteID";
                using (var deleteCommand = new SQLiteCommand(deleteQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@NoteID", noteId);
                    deleteCommand.ExecuteNonQuery();
                }

                string insertQuery = @"
            INSERT INTO Files (FileID, NoteID, FileName, FileType, FileData, SizeKB) 
            VALUES (@FileID, @NoteID, @FileName, @FileType, @FileData, @SizeKB)";

                foreach (DataRow row in attachedFilesTable.Rows)
                {
                    using (var insertCommand = new SQLiteCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@FileID", Guid.NewGuid().ToString());
                        insertCommand.Parameters.AddWithValue("@NoteID", noteId);
                        insertCommand.Parameters.AddWithValue("@FileName", row["FileName"]);
                        insertCommand.Parameters.AddWithValue("@FileType", row["FileType"]);
                        insertCommand.Parameters.AddWithValue("@FileData", File.ReadAllBytes(row["FilePath"].ToString()));
                        insertCommand.Parameters.AddWithValue("@SizeKB", row["SizeKB"]);
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void PopulateComboBoxProducts(ComboBox comboBox)
        {
            // Populates the ComboBoxProducts control with product names from the database.
            // It queries the database for product names, adds them to the ComboBox, and sets the default text.

            comboBox.Items.Clear();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT ProductName FROM Products ORDER BY ProductName";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                string productName = reader.GetString(0);
                                comboBox.Items.Add(productName);
                            }
                        }

                        comboBox.Text = "-- Select a Product --";
                    }
                }
            }
        }

        public static void CheckDatabaseDirectly()
        {
            // Checks the database directly for the maximum note number and logs the result.
            // It opens a connection, executes the query, and logs the last note number.

            Console.WriteLine("CheckDatabaseDirectly: Method entered.");
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                string query = "SELECT MAX(NoteNumber) FROM Notes";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                try
                {
                    Console.WriteLine("CheckDatabaseDirectly: Opening database connection.");
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        int lastNoteNumber = Convert.ToInt32(result);
                        Console.WriteLine($"CheckDatabaseDirectly: Query executed. Result: {lastNoteNumber}");
                        Console.WriteLine($"CheckDatabaseDirectly: Last Note Number is {lastNoteNumber}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error retrieving NoteNumber: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static void LogError(string errorMessage, string methodName, string tableName, string context = null)
        {
            // Logs an error message in the ErrorLog table.
            // It records the error message, date, time, method/button name, table name, and optional context.

            string query = @"INSERT INTO ErrorLog (ErrorMessage, ErrorDate, MethodOrButton, TableName, Context) VALUES (@ErrorMessage, @ErrorDate, @MethodOrButton, @TableName, @Context)";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ErrorMessage", errorMessage);
                        command.Parameters.AddWithValue("@ErrorDate", DateTime.Now);
                        command.Parameters.AddWithValue("@MethodOrButton", methodName);
                        command.Parameters.AddWithValue("@TableName", tableName);
                        command.Parameters.AddWithValue("@Context", string.IsNullOrEmpty(context) ? DBNull.Value : (object)context);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to log error: {ex.Message}");
            }
        }

        public static void EnsureOptionsTableAndDefaultEntry(string startupOptionID)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS Options (
                OptionID TEXT PRIMARY KEY,
                OptionValue TEXT NOT NULL,
                OptionDescription TEXT NOT NULL
            )";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                string checkEntryQuery = "SELECT COUNT(*) FROM Options WHERE OptionID = @OptionID";
                using (var command = new SQLiteCommand(checkEntryQuery, connection))
                {
                    command.Parameters.AddWithValue("@OptionID", startupOptionID);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count == 0)
                    {
                        string insertEntryQuery = "INSERT INTO Options (OptionID, OptionValue, OptionDescription) VALUES (@OptionID, @OptionValue, @OptionDescription)";
                        using (var insertCommand = new SQLiteCommand(insertEntryQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@OptionID", startupOptionID);
                            insertCommand.Parameters.AddWithValue("@OptionValue", "NewBlank"); // Default value
                            insertCommand.Parameters.AddWithValue("@OptionDescription", "Startup behavior setting");
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static bool InsertOrUpdateNoteIntoDatabase(Note note)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                bool noteExists = NoteExists(note.NoteID);
                string query;

                if (noteExists)
                {
                    query = @"
                UPDATE Notes
                SET NoteNumber = @NoteNumber, AddedDate = @AddedDate, AddedTime = @AddedTime, 
                    ProductID = @ProductID, SubcategoryID = @SubcategoryID, Subject = @Subject, 
                    Details = @Details, Source = @Source, Keywords = @Keywords, ModifiedTime = @ModifiedTime
                WHERE NoteID = @NoteID";
                }
                else
                {
                    query = @"
                INSERT INTO Notes (NoteID, NoteNumber, AddedDate, AddedTime, ProductID, SubcategoryID, Subject, Details, Source, Keywords, ModifiedTime)
                VALUES (@NoteID, @NoteNumber, @AddedDate, @AddedTime, @ProductID, @SubcategoryID, @Subject, @Details, @Source, @Keywords, @ModifiedTime)";
                }

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NoteID", note.NoteID);
                    command.Parameters.AddWithValue("@NoteNumber", note.NoteNumber);
                    command.Parameters.AddWithValue("@AddedDate", note.AddedDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@AddedTime", note.AddedTime?.ToString("HH:mm:ss") ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ProductID", note.ProductID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SubcategoryID", note.SubcategoryID ?? (object)DBNull.Value); // Set to DBNull if null
                    command.Parameters.AddWithValue("@Subject", note.Subject);
                    command.Parameters.AddWithValue("@Details", note.Details); // Save as RTF
                    command.Parameters.AddWithValue("@Source", note.Source);
                    command.Parameters.AddWithValue("@Keywords", note.Keywords);
                    command.Parameters.AddWithValue("@ModifiedTime", note.ModifiedTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? (object)DBNull.Value);

                    command.ExecuteNonQuery();
                }

                // Handle keywords
                HandleKeywords(note.NoteID, note.Keywords);
                return true;
            }
        }

        public static void HandleKeywords(string noteId, string keywords)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                // Delete existing keywords for the note
                string deleteQuery = "DELETE FROM Keywords WHERE NoteID = @NoteID";
                using (var deleteCommand = new SQLiteCommand(deleteQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@NoteID", noteId);
                    deleteCommand.ExecuteNonQuery();
                }

                // Insert new keywords
                var keywordList = keywords.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(k => k.Trim())
                                        .ToList();

                foreach (var keyword in keywordList)
                {
                    string insertQuery = "INSERT INTO Keywords (NoteID, Keyword) VALUES (@NoteID, @Keyword)";
                    using (var insertCommand = new SQLiteCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@NoteID", noteId);
                        insertCommand.Parameters.AddWithValue("@Keyword", keyword);
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void LoadNoteDetailsById(string noteId, Action<Note> setNoteDetails, ref Guid currentNoteId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Notes WHERE NoteID = @NoteID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NoteID", noteId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var note = new Note
                            {
                                NoteID = reader["NoteID"].ToString(),
                                NoteNumber = Convert.ToInt32(reader["NoteNumber"]),
                                Subject = reader["Subject"].ToString(),
                                Details = reader["Details"].ToString(),
                                ProductID = reader["ProductID"].ToString(),
                                SubcategoryID = reader["SubcategoryID"].ToString(),
                                AddedDate = DateTime.Parse(reader["AddedDate"].ToString()),
                                AddedTime = reader["AddedTime"] != DBNull.Value ? (DateTime?)DateTime.Parse(reader["AddedTime"].ToString()) : null,
                                ModifiedTime = reader["ModifiedTime"] != DBNull.Value ? (DateTime?)DateTime.Parse(reader["ModifiedTime"].ToString()) : null,
                                DeletedTime = reader["DeletedTime"] != DBNull.Value ? (DateTime?)DateTime.Parse(reader["DeletedTime"].ToString()) : null,
                                Keywords = reader["Keywords"].ToString(),
                                Source = reader["Source"].ToString()
                            };
                            setNoteDetails(note);
                            currentNoteId = Guid.Parse(note.NoteID);
                        }
                    }
                }
            }
        }

        public static bool NoteNumberExists(int noteNumber)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Notes WHERE NoteNumber = @NoteNumber";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NoteNumber", noteNumber);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public static bool UpdateExistingNoteInDatabase(Note note)
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    string updateQuery = @"
                UPDATE Notes SET 
                    NoteNumber = @NoteNumber, 
                    AddedDate = @AddedDate, 
                    AddedTime = @AddedTime, 
                    ProductID = @ProductID, 
                    SubcategoryID = @SubcategoryID, 
                    Subject = @Subject, 
                    Details = @Details, 
                    Source = @Source, 
                    Keywords = @Keywords, 
                    Version = @Version 
                WHERE NoteID = @NoteID";
                    using (var command = new SQLiteCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NoteNumber", note.NoteNumber);
                        command.Parameters.AddWithValue("@AddedDate", note.AddedDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@AddedTime", note.AddedTime.HasValue ? note.AddedTime.Value.ToString("HH:mm:ss") : (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ProductID", note.ProductID ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@SubcategoryID", note.SubcategoryID ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Subject", note.Subject);
                        command.Parameters.AddWithValue("@Details", note.Details);
                        command.Parameters.AddWithValue("@Source", note.Source);
                        command.Parameters.AddWithValue("@Keywords", note.Keywords);
                        command.Parameters.AddWithValue("@Version", note.Version);
                        command.Parameters.AddWithValue("@NoteID", note.NoteID);
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogError("Error while updating the note: " + ex.Message, "UpdateExistingNoteInDatabase", "Notes");
                MessageBox.Show("Error while updating the note: " + ex.Message);
                return false;
            }
        }

        internal static void InsertDefaultOptions()
        {
            // Inserts default options into the "Options" table if they don't already exist.
            // This ensures that required options, like "seasonalEffect," have default values in the database.

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                // Check if seasonalEffect option exists
                string checkQuery = "SELECT COUNT(*) FROM Options WHERE OptionID = 'seasonalEffect'";
                using (var command = new SQLiteCommand(checkQuery, connection))
                {
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count == 0)
                    {
                        // Insert default value
                        string insertQuery = "INSERT INTO Options (OptionID, OptionValue, OptionDescription) VALUES ('seasonalEffect', 'true', 'Seasonal effect setting')";
                        using (var insertCommand = new SQLiteCommand(insertQuery, connection))
                        {
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }


        public static void LoadNotesByProductSubcategory(string productId, string subcategoryId, List<Note> notesList, Action<int> displayNote, Action<bool> clearForm, Label lblPageAmount)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Notes";
                List<SQLiteParameter> parameters = new List<SQLiteParameter>();

                if (productId != "All")
                {
                    query += " WHERE ProductID = @ProductID";
                    parameters.Add(new SQLiteParameter("@ProductID", productId));

                    if (subcategoryId != null)
                    {
                        query += " AND SubcategoryID = @SubcategoryID";
                        parameters.Add(new SQLiteParameter("@SubcategoryID", subcategoryId));
                    }
                }

                query += " ORDER BY NoteNumber ASC";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());

                    using (var reader = cmd.ExecuteReader())
                    {
                        notesList.Clear();
                        while (reader.Read())
                        {
                            notesList.Add(new Note
                            {
                                NoteID = reader["NoteID"].ToString(),
                                NoteNumber = Convert.ToInt32(reader["NoteNumber"]),
                                ProductID = reader["ProductID"].ToString(),
                                SubcategoryID = reader["SubcategoryID"].ToString(),
                                Subject = reader["Subject"].ToString(),
                                Details = reader["Details"].ToString(),
                                AddedDate = DateTime.Parse(reader["AddedDate"].ToString()),
                                Keywords = reader.IsDBNull(reader.GetOrdinal("Keywords")) ? "" : reader["Keywords"].ToString()
                            });
                        }
                    }
                }
            }

            if (notesList.Count > 0)
            {
                displayNote(0);
            }
            else
            {
                clearForm(false);
                lblPageAmount.Text = "Note 0/0";
            }
        }


        public static string FetchKeywords(string noteId)
        {
            var keywords = new List<string>();
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string query = "SELECT Keyword FROM Keywords WHERE NoteID = @NoteID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NoteID", noteId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            keywords.Add(reader["Keyword"].ToString());
                        }
                    }
                }
            }
            return string.Join(", ", keywords);
        }

        public static Guid GetOrCreateProductId(string productName)
        {
            if (string.IsNullOrEmpty(productName))
            {
                MessageBox.Show("Product name is required.");
                return Guid.Empty;
            }

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT ProductID FROM Products WHERE ProductName = @ProductName";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", productName);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return Guid.Parse(result.ToString());
                    }
                    else
                    {
                        Guid newProductId = Guid.NewGuid();
                        string insertQuery = "INSERT INTO Products (ProductID, ProductName) VALUES (@ProductID, @ProductName)";
                        using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@ProductID", newProductId.ToString());
                            insertCommand.Parameters.AddWithValue("@ProductName", productName);
                            insertCommand.ExecuteNonQuery();
                            return newProductId;
                        }
                    }
                }
            }
        }


        public static void LogNoteView(string noteID, int noteNumber)
        {
            string query = @"
                INSERT INTO NoteViews (NoteID, NoteNumber, ViewCount) 
                VALUES (@NoteID, @NoteNumber, 1) 
                ON CONFLICT(NoteID) DO UPDATE SET ViewCount = ViewCount + 1";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NoteID", noteID);
                    command.Parameters.AddWithValue("@NoteNumber", noteNumber);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void SaveProductSubRelationship(string productId, string subcategoryId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT OR IGNORE INTO ProductSub (ProductID, SubcategoryID) VALUES (@ProductID, @SubcategoryID)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);
                    command.Parameters.AddWithValue("@SubcategoryID", subcategoryId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void LoadNotesByProduct(string productName, string subcategoryName, List<Note> notesList, Action<int> displayNote, Action<string> showMessage, ref int currentIndex)
        {
            Guid productId = GetOrCreateProductId(productName);
            if (productId == Guid.Empty)
            {
                showMessage("Product not found.");
                return;
            }

            string subcategoryId = null;
            if (!string.IsNullOrWhiteSpace(subcategoryName))
            {
                subcategoryId = GetSubcategoryIdByName(subcategoryName, productId.ToString());
                if (string.IsNullOrEmpty(subcategoryId))
                {
                    showMessage("Subcategory not found.");
                    return;
                }
            }

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string query = "SELECT NoteID, NoteNumber, AddedDate, Subject, Details FROM Notes WHERE ProductID = @ProductID" +
                               (subcategoryId != null ? " AND SubcategoryID = @SubcategoryID" : "") +
                               " ORDER BY NoteNumber ASC";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId.ToString());
                    if (subcategoryId != null)
                    {
                        cmd.Parameters.AddWithValue("@SubcategoryID", subcategoryId);
                    }

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        notesList.Clear();
                        while (reader.Read())
                        {
                            DateTime addedDate;
                            if (!DateTime.TryParseExact(reader["AddedDate"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out addedDate))
                            {
                                showMessage("Invalid date format for AddedDate.");
                                continue;
                            }

                            notesList.Add(new Note
                            {
                                NoteID = reader["NoteID"].ToString(),
                                NoteNumber = Convert.ToInt32(reader["NoteNumber"]),
                                AddedDate = addedDate,
                                Subject = reader["Subject"].ToString(),
                                Details = reader["Details"].ToString()
                            });
                        }
                        if (notesList.Count > 0)
                        {
                            currentIndex = 0;
                            displayNote(currentIndex);
                        }
                        else
                        {
                            showMessage("No notes found for the selected product and subcategory.");
                        }
                    }
                }
            }
        }

        public static void DeleteAttachmentFromDatabase(string fileId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string query = "DELETE FROM Files WHERE FileID = @FileID";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FileID", fileId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static string GetProductIdByName(string productName)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT ProductID FROM Products WHERE ProductName = @ProductName";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", productName);
                    var result = command.ExecuteScalar();
                    return result?.ToString();
                }
            }
        }

        public static string GetKeywordsByNoteId(string noteId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT Keyword FROM Keywords WHERE NoteID = @NoteID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NoteID", noteId);
                    using (var reader = command.ExecuteReader())
                    {
                        var keywords = new List<string>();
                        while (reader.Read())
                        {
                            keywords.Add(reader["Keyword"].ToString());
                        }
                        return string.Join(", ", keywords);
                    }
                }
            }
        }

        public static string GetOrCreateProductIdByName(string productName)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT ProductID FROM Products WHERE ProductName = @ProductName";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", productName);
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return result.ToString();
                    }
                }

                // Product doesn't exist, so create it
                string newProductId = Guid.NewGuid().ToString();
                query = "INSERT INTO Products (ProductID, ProductName) VALUES (@ProductID, @ProductName)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", newProductId);
                    command.Parameters.AddWithValue("@ProductName", productName);
                    command.ExecuteNonQuery();
                }
                return newProductId;
            }
        }

        public static string GetOrCreateSubcategoryIdByName(string subcategoryName, string productId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT SubcategoryID FROM Subcategories WHERE SubcategoryName = @SubcategoryName AND ProductID = @ProductID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SubcategoryName", subcategoryName);
                    command.Parameters.AddWithValue("@ProductID", productId);
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return result.ToString();
                    }
                }

                // Subcategory doesn't exist, so create it
                string newSubcategoryId = Guid.NewGuid().ToString();
                query = "INSERT INTO Subcategories (SubcategoryID, SubcategoryName, ProductID) VALUES (@SubcategoryID, @SubcategoryName, @ProductID)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SubcategoryID", newSubcategoryId);
                    command.Parameters.AddWithValue("@SubcategoryName", subcategoryName);
                    command.Parameters.AddWithValue("@ProductID", productId);
                    command.ExecuteNonQuery();
                }
                return newSubcategoryId;
            }
        }

        #endregion NippyNotes

        #region SearchForm


        // SearchForm Specific Methods
        public static void PopulateKeywordsComboBoxSearchForm(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.Items.Add("All");
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT DISTINCT Keyword FROM Keywords ORDER BY Keyword";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                comboBox.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
        }



        public static DataTable GetFilteredNotesSearchForm(string selectedProduct, string selectedSubcategory, string selectedExtension, string selectedKeyword, bool filterByExtension, bool filterByKeyword, bool filterByDate, DateTime dateFrom, DateTime dateTo)
        {
            string query = @"
        SELECT 
            n.NoteID,
            n.NoteNumber, 
            n.AddedDate, 
            p.ProductName, 
            s.SubcategoryName, 
            n.Subject, 
            n.Details, 
            GROUP_CONCAT(DISTINCT k.Keyword) AS Keywords,
            GROUP_CONCAT(DISTINCT f.FileType) AS FileExtensions
        FROM Notes n
        LEFT JOIN Products p ON n.ProductID = p.ProductID
        LEFT JOIN Subcategories s ON n.SubcategoryID = s.SubcategoryID
        LEFT JOIN Keywords k ON n.NoteID = k.NoteID
        LEFT JOIN Files f ON n.NoteID = f.NoteID";

            List<string> conditions = new List<string>();
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();

            if (!string.IsNullOrEmpty(selectedProduct) && selectedProduct != "All")
            {
                conditions.Add("n.ProductID = (SELECT ProductID FROM Products WHERE ProductName = @ProductName)");
                parameters.Add(new SQLiteParameter("@ProductName", selectedProduct));
            }

            if (!string.IsNullOrEmpty(selectedSubcategory))
            {
                conditions.Add("n.SubcategoryID = (SELECT SubcategoryID FROM Subcategories WHERE SubcategoryName = @SubcategoryName)");
                parameters.Add(new SQLiteParameter("@SubcategoryName", selectedSubcategory));
            }

            if (filterByExtension)
            {
                conditions.Add("EXISTS (SELECT 1 FROM Files f2 WHERE f2.NoteID = n.NoteID AND f2.FileType LIKE @FileType)");
                parameters.Add(new SQLiteParameter("@FileType", "%" + selectedExtension + "%"));
            }

            if (filterByKeyword)
            {
                conditions.Add("EXISTS (SELECT 1 FROM Keywords k2 WHERE k2.NoteID = n.NoteID AND k2.Keyword LIKE @Keyword)");
                parameters.Add(new SQLiteParameter("@Keyword", "%" + selectedKeyword + "%"));
            }

            if (filterByDate)
            {
                conditions.Add("n.AddedDate BETWEEN @DateFrom AND @DateTo");
                parameters.Add(new SQLiteParameter("@DateFrom", dateFrom.Date));
                parameters.Add(new SQLiteParameter("@DateTo", dateTo.Date));
            }

            if (conditions.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", conditions);
            }

            query += " GROUP BY n.NoteID, n.NoteNumber, n.AddedDate, p.ProductName, s.SubcategoryName, n.Subject, n.Details ORDER BY n.NoteNumber ASC";

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable notesTable = new DataTable();
                        notesTable.Columns.Add("NoteID", typeof(string));
                        notesTable.Columns.Add("NoteNumber", typeof(int));
                        notesTable.Columns.Add("AddedDate", typeof(DateTime));
                        notesTable.Columns.Add("ProductName", typeof(string));
                        notesTable.Columns.Add("SubcategoryName", typeof(string));
                        notesTable.Columns.Add("Subject", typeof(string));
                        notesTable.Columns.Add("Details", typeof(string)); // Add Details column
                        notesTable.Columns.Add("Keywords", typeof(string));
                        notesTable.Columns.Add("FileExtensions", typeof(string));

                        while (reader.Read())
                        {
                            DataRow row = notesTable.NewRow();
                            row["NoteID"] = reader["NoteID"] != DBNull.Value ? reader["NoteID"] : string.Empty;
                            row["NoteNumber"] = reader["NoteNumber"] != DBNull.Value ? Convert.ToInt32(reader["NoteNumber"]) : 0;
                            row["AddedDate"] = reader["AddedDate"] != DBNull.Value ? Convert.ToDateTime(reader["AddedDate"]) : DateTime.MinValue;
                            row["ProductName"] = reader["ProductName"] != DBNull.Value ? reader["ProductName"].ToString() : string.Empty;
                            row["SubcategoryName"] = reader["SubcategoryName"] != DBNull.Value ? reader["SubcategoryName"].ToString() : string.Empty;
                            row["Subject"] = reader["Subject"] != DBNull.Value ? reader["Subject"].ToString() : string.Empty;
                            row["Details"] = reader["Details"] != DBNull.Value ? reader["Details"].ToString() : string.Empty; // Populate Details column
                            row["Keywords"] = reader["Keywords"] != DBNull.Value ? reader["Keywords"].ToString() : string.Empty;
                            row["FileExtensions"] = reader["FileExtensions"] != DBNull.Value ? reader["FileExtensions"].ToString() : string.Empty;
                            notesTable.Rows.Add(row);
                        }

                        return notesTable;
                    }
                }
            }
        }











        public static void PopulateProductComboBoxSearchForm(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.Items.Add("All");
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT ProductName FROM Products ORDER BY ProductName";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                comboBox.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
        }

        public static DataTable GetAllNotesSearchForm()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = @"
            SELECT Notes.NoteID, Notes.NoteNumber, Notes.AddedDate, Notes.Subject, Notes.Details, 
                   Products.ProductName, Subcategories.SubcategoryName, 
                   GROUP_CONCAT(DISTINCT Keywords.Keyword) AS Keywords,
                   GROUP_CONCAT(DISTINCT Files.FileType) AS FileExtensions
            FROM Notes
            LEFT JOIN Products ON Notes.ProductID = Products.ProductID
            LEFT JOIN Subcategories ON Notes.SubcategoryID = Subcategories.SubcategoryID
            LEFT JOIN Keywords ON Notes.NoteID = Keywords.NoteID
            LEFT JOIN Files ON Notes.NoteID = Files.NoteID
            GROUP BY Notes.NoteID, Notes.NoteNumber, Notes.AddedDate, Products.ProductName, Subcategories.SubcategoryName, Notes.Subject, Notes.Details
            ORDER BY Notes.NoteNumber ASC";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable notesTable = new DataTable();
                    adapter.Fill(notesTable);
                    return notesTable;
                }
            }
        }




        public static void PopulateFileExtensionComboBoxSearchForm(ComboBox comboBox)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT DISTINCT FileType FROM Files";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        comboBox.Items.Clear();
                        comboBox.Items.Add("All");
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                comboBox.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
        }

        public static void PopulateSubcategoryComboBoxSearchForm(ComboBox comboBox, string selectedProduct)
        {
            comboBox.Items.Clear();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = @"
                    SELECT SubcategoryName 
                    FROM Subcategories
                    JOIN Products ON Products.ProductID = Subcategories.ProductID
                    WHERE Products.ProductName = @ProductName";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", selectedProduct);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                comboBox.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
        }

        public static void DeleteNoteAndRefreshSearchForm(string noteId)
        {
            if (string.IsNullOrEmpty(noteId) || !NoteExists(noteId))
            {
                MessageBox.Show("Unable to delete a note that hasn't been created.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        var noteIdParam = new SQLiteParameter("@NoteID", noteId);

                        string deleteFilesQuery = "DELETE FROM Files WHERE NoteID = @NoteID";
                        using (SQLiteCommand cmdFiles = new SQLiteCommand(deleteFilesQuery, connection))
                        {
                            cmdFiles.Parameters.Add(noteIdParam);
                            cmdFiles.ExecuteNonQuery();
                        }

                        string deleteNoteQuery = "DELETE FROM Notes WHERE NoteID = @NoteID";
                        using (SQLiteCommand cmdNote = new SQLiteCommand(deleteNoteQuery, connection))
                        {
                            cmdNote.Parameters.Add(noteIdParam);
                            int rowsAffected = cmdNote.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                MessageBox.Show($"No note found with the specified ID: {noteId}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                transaction.Rollback();
                                return;
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("Note deleted successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the note: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static List<Note> FetchNotesSearchForm(string product)
        {
            List<Note> notes = new List<Note>();
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT * FROM Notes";
                if (product != "All")
                {
                    query += " WHERE ProductID = (SELECT ProductID FROM Products WHERE ProductName = @ProductName)";
                }
                query += " ORDER BY NoteNumber ASC";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    if (product != "All")
                    {
                        cmd.Parameters.AddWithValue("@ProductName", product);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notes.Add(new Note
                            {
                                NoteID = reader["NoteID"].ToString(),
                                NoteNumber = Convert.ToInt32(reader["NoteNumber"]),
                                AddedDate = DateTime.Parse(reader["AddedDate"].ToString()),
                                Subject = reader["Subject"].ToString(),
                                Details = reader["Details"].ToString(),
                                ProductID = reader["ProductID"].ToString(),
                                SubcategoryID = reader["SubcategoryID"].ToString()
                            });
                        }
                    }
                }
            }
            return notes;
        }

        public static List<Note> FetchNotesByNumbersSearchForm(List<int> noteNumbers)
        {
            List<Note> notes = new List<Note>();
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT * FROM Notes WHERE NoteNumber IN (" + string.Join(",", noteNumbers) + ") ORDER BY NoteNumber ASC";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notes.Add(new Note
                            {
                                NoteID = reader["NoteID"].ToString(),
                                NoteNumber = Convert.ToInt32(reader["NoteNumber"]),
                                AddedDate = DateTime.Parse(reader["AddedDate"].ToString()),
                                Subject = reader["Subject"].ToString(),
                                Details = reader["Details"].ToString(),
                                ProductID = reader["ProductID"].ToString(),
                                SubcategoryID = reader["SubcategoryID"].ToString()
                            });
                        }
                    }
                }
            }
            return notes;
        }

        public static void DeleteProductSearchForm(string productId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    string deleteSubcategoriesQuery = "DELETE FROM Subcategories WHERE ProductID = @ProductID";
                    using (var command = new SQLiteCommand(deleteSubcategoriesQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", productId);
                        command.ExecuteNonQuery();
                    }

                    string deleteProductQuery = "DELETE FROM Products WHERE ProductID = @ProductID";
                    using (var command = new SQLiteCommand(deleteProductQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", productId);
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
        }

        public static bool IsProductEmptySearchForm(string productId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Notes WHERE ProductID = @ProductID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count == 0;
                }
            }
        }

        public static string GetProductIdByNameSearchForm(string productName)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT ProductID FROM Products WHERE ProductName = @ProductName";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", productName);
                    return command.ExecuteScalar()?.ToString();
                }
            }
        }

        public static string GetSubcategoryIdByNameSearchForm(string subcategoryName, string productId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT SubcategoryID FROM Subcategories WHERE SubcategoryName = @SubcategoryName AND ProductID = @ProductID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SubcategoryName", subcategoryName);
                    command.Parameters.AddWithValue("@ProductID", productId);
                    return command.ExecuteScalar()?.ToString();
                }
            }
        }

        public static string GetProductNameByIdSearchForm(string productId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT ProductName FROM Products WHERE ProductID = @ProductID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);
                    return command.ExecuteScalar()?.ToString();
                }
            }
        }

        public static DataTable GetQuickSearchNotes(string searchText)
        {
            string query = @"
        SELECT 
            n.NoteID,
            n.NoteNumber, 
            n.AddedDate, 
            p.ProductName, 
            s.SubcategoryName, 
            n.Subject, 
            n.Details, 
            GROUP_CONCAT(DISTINCT k.Keyword) AS Keywords,
            GROUP_CONCAT(DISTINCT f.FileType) AS FileExtensions
        FROM Notes n
        LEFT JOIN Products p ON n.ProductID = p.ProductID
        LEFT JOIN Subcategories s ON n.SubcategoryID = s.SubcategoryID
        LEFT JOIN Keywords k ON n.NoteID = k.NoteID
        LEFT JOIN Files f ON n.NoteID = f.NoteID
        WHERE n.Details LIKE @searchText
        GROUP BY n.NoteID, n.NoteNumber, n.AddedDate, p.ProductName, s.SubcategoryName, n.Subject, n.Details
        ORDER BY n.NoteNumber ASC";

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@searchText", "%" + searchText + "%"));

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable notesTable = new DataTable();
                        notesTable.Columns.Add("NoteID", typeof(string));
                        notesTable.Columns.Add("NoteNumber", typeof(int));
                        notesTable.Columns.Add("AddedDate", typeof(DateTime));
                        notesTable.Columns.Add("ProductName", typeof(string));
                        notesTable.Columns.Add("SubcategoryName", typeof(string));
                        notesTable.Columns.Add("Subject", typeof(string));
                        notesTable.Columns.Add("Details", typeof(string)); // Add Details column
                        notesTable.Columns.Add("Keywords", typeof(string));
                        notesTable.Columns.Add("FileExtensions", typeof(string));

                        while (reader.Read())
                        {
                            DataRow row = notesTable.NewRow();
                            row["NoteID"] = reader["NoteID"] != DBNull.Value ? reader["NoteID"] : string.Empty;
                            row["NoteNumber"] = reader["NoteNumber"] != DBNull.Value ? Convert.ToInt32(reader["NoteNumber"]) : 0;
                            row["AddedDate"] = reader["AddedDate"] != DBNull.Value ? Convert.ToDateTime(reader["AddedDate"]) : DateTime.MinValue;
                            row["ProductName"] = reader["ProductName"] != DBNull.Value ? reader["ProductName"].ToString() : string.Empty;
                            row["SubcategoryName"] = reader["SubcategoryName"] != DBNull.Value ? reader["SubcategoryName"].ToString() : string.Empty;
                            row["Subject"] = reader["Subject"] != DBNull.Value ? reader["Subject"].ToString() : string.Empty;
                            row["Details"] = reader["Details"] != DBNull.Value ? reader["Details"].ToString() : string.Empty; // Populate Details column
                            row["Keywords"] = reader["Keywords"] != DBNull.Value ? reader["Keywords"].ToString() : string.Empty;
                            row["FileExtensions"] = reader["FileExtensions"] != DBNull.Value ? reader["FileExtensions"].ToString() : string.Empty;
                            notesTable.Rows.Add(row);
                        }

                        return notesTable;
                    }
                }
            }
        }


        private static string EscapeLikeValue(string value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                switch (c)
                {
                    case '%':
                    case '_':
                    case '[':
                    case ']':
                    case '\\':
                    case '(':
                    case ')':
                    case '*':
                    case '+':
                    case '?':
                    case '^':
                    case '$':
                    case '{':
                    case '}':
                    case '|':
                    case '.':
                        sb.Append('\\');
                        sb.Append(c);
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }












        #endregion SearchForm

        #region SettingsForm

        public static string GetOptionValue(string optionID)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT OptionValue FROM Options WHERE OptionID = @OptionID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OptionID", optionID);
                    return command.ExecuteScalar()?.ToString();
                }
            }
        }

        public static void UpdateOptionValue(string optionID, string optionValue)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Options SET OptionValue = @OptionValue WHERE OptionID = @OptionID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OptionID", optionID);
                    command.Parameters.AddWithValue("@OptionValue", optionValue);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void InsertOrUpdateOption(string optionID, string optionValue, string optionDescription)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT OR REPLACE INTO Options (OptionID, OptionValue, OptionDescription) VALUES (@OptionID, @OptionValue, @OptionDescription)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OptionID", optionID);
                    command.Parameters.AddWithValue("@OptionValue", optionValue);
                    command.Parameters.AddWithValue("@OptionDescription", optionDescription);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static string GetFontSetting()
        {
            return GetOptionValue("defaultFont") ?? "Calibri";
        }

        public static int GetFontSizeSetting()
        {
            string fontSizeStr = GetOptionValue("defaultFontSize");
            return int.TryParse(fontSizeStr, out int fontSize) ? fontSize : 12;
        }

        public static void LoadStartupAndSeasonalOptions(out string startupBehavior, out bool seasonalEffect)
        {
            startupBehavior = GetOptionValue("startupBehavior") ?? "NewBlank";
            seasonalEffect = GetOptionValue("seasonalEffect") == "true";
        }


        public static void DeleteAllData()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string deleteNotesQuery = "DELETE FROM Notes";
                        using (var command = new SQLiteCommand(deleteNotesQuery, connection))
                        {
                            command.ExecuteNonQuery();
                        }

                        string deleteSubcategoriesQuery = "DELETE FROM Subcategories";
                        using (var command = new SQLiteCommand(deleteSubcategoriesQuery, connection))
                        {
                            command.ExecuteNonQuery();
                        }

                        string deleteProductsQuery = "DELETE FROM Products";
                        using (var command = new SQLiteCommand(deleteProductsQuery, connection))
                        {
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        Logger.LogActivity("Delete", "Deleted all notes, products, and subcategories.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"An error occurred while deleting data: {ex.Message}");
                    }
                }
            }
        }

        public static void DeleteEmptyProductsAndSubcategories()
        {
            bool changesMade = false;
            int subcategoriesDeleted, productsDeleted;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string deleteSubcategoriesQuery = @"
            DELETE FROM Subcategories
            WHERE SubcategoryID NOT IN (SELECT DISTINCT SubcategoryID FROM Notes WHERE SubcategoryID IS NOT NULL)";
                using (var command = new SQLiteCommand(deleteSubcategoriesQuery, connection))
                {
                    subcategoriesDeleted = command.ExecuteNonQuery();
                    if (subcategoriesDeleted > 0) changesMade = true;
                }

                string deleteProductsQuery = @"
            DELETE FROM Products
            WHERE ProductID NOT IN (SELECT DISTINCT ProductID FROM Notes)";
                using (var command = new SQLiteCommand(deleteProductsQuery, connection))
                {
                    productsDeleted = command.ExecuteNonQuery();
                    if (productsDeleted > 0) changesMade = true;
                }
            }

            if (changesMade)
            {
                Logger.LogActivity("Update", $"Deleted {productsDeleted} products and {subcategoriesDeleted} subcategories.");
            }
        }

        #region DatabaseBackups

        public static void ManualProcessDB(string backupDirectory)
        {
            string backupFilePath = Path.Combine(backupDirectory, "NippyDB_Backup_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".db");

            try
            {
                File.Copy(DatabaseFilePath, backupFilePath, true);
                Logger.LogActivity("Backup", "Manual database backup completed successfully.");
                BackupHistoryManager.AddBackupRecord(new BackupRecord
                {
                    BackupName = Path.GetFileName(backupFilePath),
                    Location = "Manual",
                    FilePath = backupFilePath,
                    Date = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred during the manual backup: {ex.Message}");
            }
        }

        public static void AutoProcessDB()
        {
            string backupDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");
            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }

            string backupFilePath = Path.Combine(backupDirectory, "NippyDB_Backup_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".db");

            try
            {
                File.Copy(DatabaseFilePath, backupFilePath, true);
                Logger.LogActivity("Backup", "Automatic database backup completed successfully.");
                BackupHistoryManager.AddBackupRecord(new BackupRecord
                {
                    BackupName = Path.GetFileName(backupFilePath),
                    Location = "Auto",
                    FilePath = backupFilePath,
                    Date = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred during the automatic backup: {ex.Message}");
            }
        }

        public static void AddBackupRecord(BackupRecord record)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string insertQuery = @"
        INSERT INTO BackupHistory (BackupName, Location, FilePath, Date)
        VALUES (@BackupName, @Location, @FilePath, @Date)";
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@BackupName", record.BackupName);
                    command.Parameters.AddWithValue("@Location", record.Location);
                    command.Parameters.AddWithValue("@FilePath", record.FilePath);
                    command.Parameters.AddWithValue("@Date", record.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void RestoreDB(string selectedBackupFile)
        {
            try
            {
                File.Copy(selectedBackupFile, DatabaseFilePath, true);
                Logger.LogActivity("Restore", "Database restored successfully from " + selectedBackupFile);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred during the restore: {ex.Message}");
            }
        }

        public static void PopulateBackupHistory(string backupFilePath, string location, DataGridView dataGridView)
        {
            if (File.Exists(backupFilePath))
            {
                FileInfo fileInfo = new FileInfo(backupFilePath);
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    string insertQuery = @"
            INSERT INTO BackupHistory (BackupName, Location, FilePath, Date)
            VALUES (@BackupName, @Location, @FilePath, @Date)";
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@BackupName", fileInfo.Name);
                        command.Parameters.AddWithValue("@Location", location);
                        command.Parameters.AddWithValue("@FilePath", fileInfo.FullName);
                        command.Parameters.AddWithValue("@Date", fileInfo.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.ExecuteNonQuery();
                    }
                }

                dataGridView.Rows.Add(fileInfo.Name, location, fileInfo.FullName, fileInfo.CreationTime);
            }
        }

        public static void LoadBackupHistory(DataGridView dataGridView)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = @"
        SELECT BackupName, Location, FilePath, Date
        FROM BackupHistory
        ORDER BY Date DESC";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        dataGridView.Rows.Clear();
                        while (reader.Read())
                        {
                            string backupName = reader["BackupName"].ToString();
                            string location = reader["Location"].ToString();
                            string filePath = reader["FilePath"].ToString();
                            string date = reader["Date"].ToString();
                            dataGridView.Rows.Add(backupName, location, filePath, DateTime.Parse(date));
                        }
                    }
                }
            }
        }

        public static int GetCountFromDatabase(string tableName)
        {
            int count = 0;
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = $"SELECT COUNT(*) FROM {tableName}";
                using (var command = new SQLiteCommand(query, connection))
                {
                    count = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return count;
        }

        public static void UploadDB(string selectedFilePath, DataGridView dataGridViewBackups)
        {
            FileInfo fileInfo = new FileInfo(selectedFilePath);

            if (fileInfo.Exists)
            {
                foreach (DataGridViewRow row in dataGridViewBackups.Rows)
                {
                    if (row.Cells["FilePath"].Value != null && row.Cells["FilePath"].Value.ToString() == fileInfo.FullName)
                    {
                        throw new Exception("This backup file already exists in the list.");
                    }
                }

                string backupName = fileInfo.Name;
                string location = "Uploaded";
                string filePath = fileInfo.FullName;
                string date = fileInfo.CreationTime.ToString("yyyy-MM-dd HH:mm:ss");

                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    string insertQuery = @"
                INSERT INTO BackupHistory (BackupName, Location, FilePath, Date)
                VALUES (@BackupName, @Location, @FilePath, @Date)";
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@BackupName", backupName);
                        command.Parameters.AddWithValue("@Location", location);
                        command.Parameters.AddWithValue("@FilePath", filePath);
                        command.Parameters.AddWithValue("@Date", date);
                        command.ExecuteNonQuery();
                    }
                }

                dataGridViewBackups.Rows.Add(backupName, location, filePath, DateTime.Parse(date));
            }
            else
            {
                throw new Exception("The selected file does not exist.");
            }
        }

        public static void DeleteBackup(DataGridViewRow selectedRow, DataGridView dataGridViewBackups)
        {
            if (selectedRow.Cells["FilePath"].Value != null)
            {
                string selectedFilePath = selectedRow.Cells["FilePath"].Value.ToString();

                try
                {
                    if (File.Exists(selectedFilePath))
                    {
                        File.Delete(selectedFilePath);
                    }

                    int rowIndex = selectedRow.Index;
                    dataGridViewBackups.Rows.RemoveAt(rowIndex);

                    using (var connection = new SQLiteConnection(ConnectionString))
                    {
                        connection.Open();
                        string deleteQuery = "DELETE FROM BackupHistory WHERE FilePath = @FilePath";
                        using (var command = new SQLiteCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@FilePath", selectedFilePath);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while deleting the file: {ex.Message}");
                }
            }
            else
            {
                throw new Exception("The selected backup file path is not valid.");
            }
        }

        #endregion Database Backups

        public static void SaveFormPreference(string formType)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Options SET OptionValue = @OptionValue WHERE OptionID = 'FormType'";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OptionValue", formType);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void SavePasswordAndWordToDatabase(string passwordHash, string memorableWord)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string queryPassword = "INSERT OR REPLACE INTO Options (OptionID, OptionValue, OptionDescription) VALUES ('SecurePassword', @PasswordHash, 'The encrypted password for securing the application')";
                using (var command = new SQLiteCommand(queryPassword, connection))
                {
                    command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    command.ExecuteNonQuery();
                }

                string queryWord = "INSERT OR REPLACE INTO Options (OptionID, OptionValue, OptionDescription) VALUES ('MemorableWord', @Word, 'User memorable word for security')";
                using (var command = new SQLiteCommand(queryWord, connection))
                {
                    command.Parameters.AddWithValue("@Word", memorableWord);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void RemovePasswordAndWordFromDatabase()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Options WHERE OptionID = 'UserPassword' OR OptionID = 'MemorableWord'";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static string GetStoredPasswordHash()
        {
            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT OptionValue FROM Options WHERE OptionID = 'UserPassword'";
                using (var command = new SQLiteCommand(query, connection))
                {
                    return command.ExecuteScalar()?.ToString();
                }
            }
        }

        public static void SavePasswordHashToDatabase(string passwordHash)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT OR REPLACE INTO Options (OptionID, OptionValue, OptionDescription) VALUES ('SecurePassword', @PasswordHash, 'The encrypted password for securing the application')";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static string GetStoredMemorableWord()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT OptionValue FROM Options WHERE OptionID = 'MemorableWord'";
                using (var command = new SQLiteCommand(query, connection))
                {
                    return command.ExecuteScalar()?.ToString();
                }
            }
        }




        #endregion SettingsForm

        #region TranslationForm




        public static Dictionary<string, string> LoadExistingTranslations(string translationType, string productId = null)
        {
            var translations = new Dictionary<string, string>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "";

                if (translationType == "Product")
                {
                    query = "SELECT ProductID, ProductName FROM Products";
                }
                else if (translationType == "Subcategory")
                {
                    if (!string.IsNullOrEmpty(productId))
                    {
                        query = "SELECT SubcategoryID, SubcategoryName FROM Subcategories WHERE ProductID = @ProductId";
                    }
                    else
                    {
                        throw new ArgumentException("Product ID is missing for subcategory translation.");
                    }
                }

                using (var command = new SQLiteCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(productId))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            translations.Add(reader.GetString(0), reader.GetString(1));
                        }
                    }
                }
            }

            return translations;
        }

        public static string EnsureTranslation(string translationType, string sourceValue, string databaseId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = translationType == "Product" ?
                    "SELECT ProductID FROM Products WHERE ProductName = @Value" :
                    "SELECT SubcategoryID FROM Subcategories WHERE SubcategoryName = @Value";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Value", sourceValue);
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return result.ToString();
                    }
                }
            }

            return null;
        }

        public static string CreateTranslation(string translationType, string value)
        {
            var newId = Guid.NewGuid().ToString();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string insertQuery = translationType == "Product" ?
                    "INSERT INTO Products (ProductID, ProductName) VALUES (@ID, @Value)" :
                    "INSERT INTO Subcategories (SubcategoryID, SubcategoryName) VALUES (@ID, @Value)";
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@ID", newId);
                    command.Parameters.AddWithValue("@Value", value);
                    command.ExecuteNonQuery();
                }
            }
            return newId;
        }


#endregion

        #region Logger
public static void EnsureActivityLogTable()
        {
            using (var connection = new SQLiteConnection(GetDatabasePath()))
            {
                connection.Open();
                string query = @"
                CREATE TABLE IF NOT EXISTS ActivityLog (
                    LogID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Timestamp TEXT NOT NULL,
                    ActionType TEXT NOT NULL,
                    Details TEXT NOT NULL
                )";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void LogActivity(string actionType, string details)
        {
            using (var connection = new SQLiteConnection(GetDatabasePath()))
            {
                connection.Open();
                string query = "INSERT INTO ActivityLog (Timestamp, ActionType, Details) VALUES (@Timestamp, @ActionType, @Details)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@ActionType", actionType);
                    command.Parameters.AddWithValue("@Details", details);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static string GetDatabasePath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string databasePath = Path.Combine(appDataPath, "NippyNotes", "NippyDB.db");
            if (!Directory.Exists(Path.GetDirectoryName(databasePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(databasePath));
            }
            return $"Data Source={databasePath};Version=3;";
        }

        #endregion Logger

        #region Program


        /*     public static string GetStoredPasswordHash()
             {
                 using (var connection = new SQLiteConnection(GetDatabasePath()))
                 {
                     connection.Open();
                     string query = "SELECT OptionValue FROM Options WHERE OptionID = 'UserPassword'";
                     using (var command = new SQLiteCommand(query, connection))
                     {
                         return command.ExecuteScalar()?.ToString();
                     }
                 } */

        #endregion

        #region Security & Password


        private static string GetDatabaseFilePath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string nippyNotesPath = Path.Combine(appDataPath, "NippyNotes");
            if (!Directory.Exists(nippyNotesPath))
            {
                Directory.CreateDirectory(nippyNotesPath);
            }
            return Path.Combine(nippyNotesPath, "NippyDB.db");
        }

        private static string GetConnectionString()
        {
            return $"Data Source={DatabaseFilePath};Version=3;";
        }

        public static void SaveCredentials(string hashedPassword, string hashedWord)
        {
            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();

                string queryPassword = "INSERT OR REPLACE INTO Options (OptionID, OptionValue, OptionDescription) VALUES ('UserPassword', @Password, 'User password for security')";
                using (var command = new SQLiteCommand(queryPassword, connection))
                {
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.ExecuteNonQuery();
                }

                string queryWord = "INSERT OR REPLACE INTO Options (OptionID, OptionValue, OptionDescription) VALUES ('MemorableWord', @Word, 'User memorable word for security')";
                using (var command = new SQLiteCommand(queryWord, connection))
                {
                    command.Parameters.AddWithValue("@Word", hashedWord);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static string GetStoredTempPasswordHash()
        {
            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT OptionValue FROM Options WHERE OptionID = 'TempPassword'";
                using (var command = new SQLiteCommand(query, connection))
                {
                    return command.ExecuteScalar()?.ToString();
                }
            }
        }

        public static void SaveNewPassword(string newPassword)
        {
            string hashedNewPassword = SecurityHelper.HashPassword(newPassword);
            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();
                string queryNewPassword = "INSERT OR REPLACE INTO Options (OptionID, OptionValue, OptionDescription) VALUES ('UserPassword', @UserPassword, 'User password for security')";
                using (var command = new SQLiteCommand(queryNewPassword, connection))
                {
                    command.Parameters.AddWithValue("@UserPassword", hashedNewPassword);
                    command.ExecuteNonQuery();
                }

                // Remove temporary password
                string queryRemoveTempPassword = "DELETE FROM Options WHERE OptionID = 'TempPassword'";
                using (var command = new SQLiteCommand(queryRemoveTempPassword, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static bool VerifyTempPassword(string tempPassword)
        {
            string storedTempPasswordHash = GetStoredTempPasswordHash();
            return SecurityHelper.VerifyPassword(tempPassword, storedTempPasswordHash);
        }

        public static bool VerifyMemorableWord(string memorableWord)
        {
            string storedMemorableWordHash = GetStoredMemorableWordHash();
            return SecurityHelper.VerifyPassword(memorableWord, storedMemorableWordHash);
        }

        public static string GetStoredMemorableWordHash()
        {
            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT OptionValue FROM Options WHERE OptionID = 'MemorableWord'";
                using (var command = new SQLiteCommand(query, connection))
                {
                    return command.ExecuteScalar()?.ToString();
                }
            }
        }

        public static string GenerateTempPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void SaveTempPassword(string tempPassword)
        {
            string hashedTempPassword = SecurityHelper.HashPassword(tempPassword);
            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();
                string queryTempPassword = "INSERT OR REPLACE INTO Options (OptionID, OptionValue, OptionDescription) VALUES ('TempPassword', @TempPassword, 'Temporary password for reset')";
                using (var command = new SQLiteCommand(queryTempPassword, connection))
                {
                    command.Parameters.AddWithValue("@TempPassword", hashedTempPassword);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void SendTempPasswordEmail(string tempPassword, string email)
        {
            string smtpHost = "smtp-mail.outlook.com";
            int smtpPort = 587;
            string smtpUsername = "Daniel.daley88@outlook.com";
            string smtpPassword = "Anibase57";
            string fromEmail = smtpUsername; // Use the same email address as the authenticated user

            MailMessage mail = new MailMessage
            {
                From = new MailAddress(fromEmail, "NippyNotes"),
                Subject = "NippyNotes Password Reset",
                Body = $"Your temporary password is: {tempPassword}",
                IsBodyHtml = false,
                Priority = MailPriority.High
            };
            mail.To.Add(email);

            SmtpClient smtpClient = new SmtpClient
            {
                Host = smtpHost,
                Port = smtpPort,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            smtpClient.Send(mail);
        }






#endregion Security & Password


        #region FeedbackForm

public static void ApplyDefaultFontAndSize(RichTextBox richTextBox)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string fontQuery = "SELECT OptionValue FROM Options WHERE OptionID = 'defaultFont'";
                string sizeQuery = "SELECT OptionValue FROM Options WHERE OptionID = 'defaultFontSize'";

                string defaultFontName = "Calibri"; // Fallback default font
                int defaultFontSize = 12; // Fallback default size

                using (var command = new SQLiteCommand(fontQuery, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        defaultFontName = result.ToString();
                    }
                }

                using (var command = new SQLiteCommand(sizeQuery, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        defaultFontSize = int.Parse(result.ToString());
                    }
                }

                Font defaultFont = new Font(defaultFontName, defaultFontSize);
                richTextBox.Font = defaultFont;
            }
        }
    }
}

#endregion FeedbackForm




