using System;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace Nippy_Notes
{
    public partial class FeedbackForm : Form
    {
        private bool isDragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private string defaultFont;
        private int defaultFontSize;

        public FeedbackForm()
        {
            InitializeComponent();
            PanelFeedbackFormDrag.MouseDown += PanelFeedbackFormDrag_MouseDown;
            PanelFeedbackFormDrag.MouseMove += PanelFeedbackFormDrag_MouseMove;
            PanelFeedbackFormDrag.MouseUp += PanelFeedbackFormDrag_MouseUp;

            // Load default settings
            LoadDefaultFontAndSizeSettings();

            // Initialize ComboBoxes
            InitializeFontAndSizeComboBoxes();

            // Apply the settings
            ApplyDefaultFontAndSizeToRichTextBox();
        }

        private void LoadDefaultFontAndSizeSettings()
        {
            defaultFont = DatabaseHelper.GetFontSetting() ?? "Arial";
            defaultFontSize = DatabaseHelper.GetFontSizeSetting();
        }

        private void InitializeFontAndSizeComboBoxes()
        {
            ComboBoxFont.Items.Clear();
            foreach (FontFamily font in System.Drawing.FontFamily.Families)
            {
                ComboBoxFont.Items.Add(font.Name);
            }
            ComboBoxFont.SelectedItem = defaultFont;

            ComboBoxSize.Items.Clear();
            for (int i = 8; i <= 72; i += 2)
            {
                ComboBoxSize.Items.Add(i.ToString());
            }
            ComboBoxSize.SelectedItem = defaultFontSize.ToString();

            // Debugging output
            Console.WriteLine($"Default Font: {defaultFont}, Default Font Size: {defaultFontSize}");
            Console.WriteLine("ComboBoxSize Items:");
            foreach (var item in ComboBoxSize.Items)
            {
                Console.WriteLine(item);
            }
        }

        private void ApplyDefaultFontAndSizeToRichTextBox()
        {
            RichTextBoxFeedback.Font = new Font(defaultFont, defaultFontSize);
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

            RichTextBoxFeedback.Font = new Font(selectedFont, selectedFontSize);
        }

        private void PanelFeedbackFormDrag_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
            }
        }

        private void PanelFeedbackFormDrag_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        private void PanelFeedbackFormDrag_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void BtnSendFeedback_Click(object sender, EventArgs e)
        {
            string feedback = RichTextBoxFeedback.Text;
            if (string.IsNullOrWhiteSpace(feedback))
            {
                MessageBox.Show("Please enter your feedback before sending.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SendFeedbackEmail(feedback);
                MessageBox.Show("Thank you for your feedback!", "Feedback Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send feedback. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelFeedback_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // I don't want this here really.. feels insecure

        private void SendFeedbackEmail(string feedback)
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
                Subject = "Nippy Notes Feedback",
                Body = feedback,
                IsBodyHtml = false,
                Priority = MailPriority.High
            };
            mail.To.Add(toEmail);

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
    }
}
