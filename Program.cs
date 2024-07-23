using System;
using System.Windows.Forms;

namespace Nippy_Notes
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check if a password is set
            if (IsPasswordSet())
            {
                // Show the login form if a password is set
                Application.Run(new Security.LoginForm());
            }
            else
            {
                // Run the main form directly if no password is set
                Application.Run(new NippyNotes());
            }
        }

        private static bool IsPasswordSet()
        {
            DatabaseHelper.InitializeDatabase();
            string storedPasswordHash = DatabaseHelper.GetStoredPasswordHash();
            return !string.IsNullOrEmpty(storedPasswordHash);
        }
    }
}
