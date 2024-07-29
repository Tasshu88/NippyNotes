using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Nippy_Notes.Helpers
{
    public static class EmailHandler
    {
        // Method to check if Outlook is installed
        public static bool IsOutlookInstalled()
        {
            try
            {
                Type outlookType = Type.GetTypeFromProgID("Outlook.Application");
                return outlookType != null;
            }
            catch
            {
                return false;
            }
        }

        // Method to prompt the user to select an email client if Outlook is not installed
        public static string PromptForEmailClient()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Executable files (*.exe)|*.exe",
                Title = "Select Email Client",
                CheckFileExists = true,
                CheckPathExists = true
            };

            return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : null;
        }

        // Method to create and send an email with the XML attachment
        public static void SendEmailWithAttachment(string attachmentPath, string emailClientPath = null)
        {
            if (string.IsNullOrEmpty(emailClientPath) && IsOutlookInstalled())
            {
                SendEmailWithOutlook(attachmentPath);
            }
            else if (!string.IsNullOrEmpty(emailClientPath))
            {
                SendEmailWithCustomClient(attachmentPath, emailClientPath);
            }
            else
            {
                MessageBox.Show("No email client selected or installed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Renamed Subject to show Enviornment User, Product, Filename

        private static void SendEmailWithOutlook(string attachmentPath)
        {
            try
            {
                Outlook.Application outlookApp = new Outlook.Application();
                Outlook.MailItem mailItem = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);

                string fileName = Path.GetFileName(attachmentPath);
                string subject = $"{Environment.UserName} Nippy Notes - {fileName}";

                mailItem.Subject = subject;
                mailItem.Body = "Please find the attached note.";
                mailItem.Attachments.Add(attachmentPath);
                mailItem.Display(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send email with Outlook: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // if no Outlook client installed, use alt
        private static void SendEmailWithCustomClient(string attachmentPath, string emailClientPath)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = emailClientPath,
                    Arguments = attachmentPath,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send email with the selected client: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
