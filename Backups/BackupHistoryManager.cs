using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public static class BackupHistoryManager
{

    // File path for backup history file in ApplicationData folder NOT program files.
    private static readonly string backupHistoryFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NippyNotes", "BackupHistory.json");

    // Save the backup history to a JSON file
    public static void SaveBackupHistory(List<BackupRecord> backupRecords)
    {
        // Convert the list of backup records to JSON format
        var json = JsonConvert.SerializeObject(backupRecords, Formatting.Indented);
        // Write the JSON string to the file
        File.WriteAllText(backupHistoryFilePath, json);
    }

    // Load the backup history from the JSON file
    public static List<BackupRecord> LoadBackupHistory()
    {
        // Check if the backup history file exists
        if (!File.Exists(backupHistoryFilePath))
        {
            // If it doesn't exist, return an empty list
            return new List<BackupRecord>();
        }
        // Read the content of the backup history file
        var json = File.ReadAllText(backupHistoryFilePath);
        // Deserialize the JSON string back to a list of backup records
        return JsonConvert.DeserializeObject<List<BackupRecord>>(json);
    }

    // Add a new backup record to the history
    public static void AddBackupRecord(BackupRecord backupRecord)
    {
        // Load the existing backup history
        var backupRecords = LoadBackupHistory();
        // Add the new backup record to the list
        backupRecords.Add(backupRecord);
        // Save the updated backup history back to the file
        SaveBackupHistory(backupRecords);
    }

    // Delete a backup file and its record from the history
    public static void DeleteBackup(string filePath)
    {
        try
        {
            // Check if the file path is not empty or null
            if (!string.IsNullOrEmpty(filePath))
            {
                // Log file path before attempting deletion
                Console.WriteLine($"Attempting to delete backup: FilePath = {filePath}");

                // Delete the file from the file system
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine($"File deleted from file system: {filePath}");
                }

                // Remove the backup record from the JSON file
                var backupRecords = LoadBackupHistory();
                // Find the record that matches the file path
                var recordToDelete = backupRecords.FirstOrDefault(record => record.FilePath == filePath);
                if (recordToDelete != null)
                {
                    // Log the details of the record being deleted
                    Console.WriteLine($"Record found for deletion: BackupName = {recordToDelete.BackupName}, Location = {recordToDelete.Location}, FilePath = {recordToDelete.FilePath}, Date = {recordToDelete.Date}");

                    // Remove the found record from the list
                    backupRecords.Remove(recordToDelete);
                    Console.WriteLine($"Record removed from backup history: {filePath}");
                    // Save the updated backup history back to the file
                    SaveBackupHistory(backupRecords);
                }
                else
                {
                    Console.WriteLine($"Record not found in backup history for: {filePath}");
                }
            }
            else
            {
                throw new Exception("The selected backup file path is not valid.");
            }
        }
        catch (Exception ex)
        {
            // Log the exception for debugging purposes
            Console.WriteLine($"An error occurred while deleting the file: {ex.Message}");
            // Throw an exception if an error occurs while deleting the file
            throw new Exception($"An error occurred while deleting the file: {ex.Message}");
        }
    }
}
