using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using System.Drawing;

namespace Nippy_Notes
{
    public partial class BugForm : Form
    {
        private const int DefaultFormHeight = 271;
        private const int AttachmentFormHeightIncrement = 16;
        private const int MaxAttachments = 3;

        private string[] attachedFilePaths = new string[MaxAttachments];
        private int currentAttachmentIndex = 0;

        private bool isDragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private string defaultFont;
        private int defaultFontSize;

        public BugForm()
        {
            InitializeComponent();
            UpdateFormSize();
            PanelBugFormDrag.MouseDown += PanelBugFormDrag_MouseDown;
            PanelBugFormDrag.MouseMove += PanelBugFormDrag_MouseMove;
            PanelBugFormDrag.MouseUp += PanelBugFormDrag_MouseUp;

            // Load default settings
            LoadDefaultFontAndSizeSettings();

            // Initialize ComboBoxes
            InitializeFontComboBox();
            InitializeSizeComboBox();

            // Apply the settings
            ApplyDefaultFontAndSizeToComboBoxes();
        }

        private void LoadDefaultFontAndSizeSettings()
        {
            defaultFont = DatabaseHelper.GetFontSetting() ?? "Arial";
            defaultFontSize = DatabaseHelper.GetFontSizeSetting();
        }

        private void InitializeFontComboBox()
        {
            ComboBoxFont.Items.Clear();
            foreach (FontFamily font in System.Drawing.FontFamily.Families)
            {
                ComboBoxFont.Items.Add(font.Name);
            }
            ComboBoxFont.SelectedItem = defaultFont;
        }

        private void InitializeSizeComboBox()
        {
            ComboBoxSize.Items.Clear();
            for (int i = 8; i <= 72; i += 2)
            {
                ComboBoxSize.Items.Add(i.ToString());
            }
            ComboBoxSize.SelectedItem = defaultFontSize.ToString();
        }

        private void ApplyDefaultFontAndSizeToComboBoxes()
        {
            if (ComboBoxFont.Items.Contains(defaultFont))
            {
                ComboBoxFont.SelectedItem = defaultFont;
            }

            if (ComboBoxSize.Items.Contains(defaultFontSize.ToString()))
            {
                ComboBoxSize.SelectedItem = defaultFontSize.ToString();
            }

            // Apply the font and size to the RichTextBox or other controls
            RichTextBoxBug.Font = new Font(defaultFont, defaultFontSize);
        }

        private void ComboBoxFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplySelectedFontAndSize();
        }

        private void ComboBoxSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplySelectedFontAndSize();
        }

        private void ApplySelectedFontAndSize()
        {
            string selectedFont = ComboBoxFont.SelectedItem?.ToString() ?? "Arial";
            float selectedFontSize = float.TryParse(ComboBoxSize.SelectedItem?.ToString(), out float size) ? size : 12f;

            RichTextBoxBug.Font = new Font(selectedFont, selectedFontSize);
        }

        private void BtnAttachScreenshotBug_Click(object sender, EventArgs e)
        {
            if (currentAttachmentIndex >= MaxAttachments)
            {
                MessageBox.Show("You can only attach up to 3 screenshots.", "Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openFileDialog.Title = "Select a Screenshot";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    attachedFilePaths[currentAttachmentIndex] = openFileDialog.FileName;
                    currentAttachmentIndex++;
                    UpdateFormSize();
                    MessageBox.Show("Screenshot attached successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void PanelBugFormDrag_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
            }
        }

        private void PanelBugFormDrag_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        private void PanelBugFormDrag_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void BtnReportBug_Click(object sender, EventArgs e)
        {
            string bugReport = RichTextBoxBug.Text;
            if (string.IsNullOrWhiteSpace(bugReport))
            {
                MessageBox.Show("Please enter the bug details before sending.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SendBugReportEmail(bugReport, attachedFilePaths);
                MessageBox.Show("Thank you for reporting the bug!", "Bug Report Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send bug report. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelBug_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SendBugReportEmail(string bugReport, string[] screenshotPaths)
        {
            string smtpHost = "smtp-mail.outlook.com";
            int smtpPort = 587;
            string smtpUsername = "Daniel.daley88@outlook.com";
            string smtpPassword = "Anibase57";
            string fromEmail = "Daniel.daley88@outlook.com";
            string toEmail = "Daniel.daley88@outlook.com";

            MailMessage mail = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = "Nippy Notes Bug Report",
                Body = bugReport,
                IsBodyHtml = false,
                Priority = MailPriority.High
            };
            mail.To.Add(toEmail);

            foreach (var path in screenshotPaths)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    Attachment screenshotAttachment = new Attachment(path);
                    mail.Attachments.Add(screenshotAttachment);
                }
            }

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

        private void UpdateFormSize()
        {
            int newHeight = DefaultFormHeight + currentAttachmentIndex * AttachmentFormHeightIncrement;
            this.Size = new System.Drawing.Size(this.Width, newHeight);

            if (currentAttachmentIndex > 0)
                LblFileAttached01.Text = System.IO.Path.GetFileName(attachedFilePaths[0]);
            if (currentAttachmentIndex > 1)
                LblFileAttached02.Text = System.IO.Path.GetFileName(attachedFilePaths[1]);
            if (currentAttachmentIndex > 2)
                LblFileAttached03.Text = System.IO.Path.GetFileName(attachedFilePaths[2]);
        }
    }
}
