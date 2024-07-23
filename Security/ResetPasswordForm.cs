using System;
using System.Net.Mail;
using System.Windows.Forms;

namespace Nippy_Notes.Security
{
    public partial class ResetPasswordForm : Form
    {
        public ResetPasswordForm()
        {
            InitializeComponent();
        }

        private void BtnSendEmail_Click(object sender, EventArgs e)
        {
            string email = TextBoxEmail.Text;
            string memorableWord = TextBoxMemorableWord.Text;

            if (IsValidEmail(email) && DatabaseHelper.VerifyMemorableWord(memorableWord))
            {
                string tempPassword = DatabaseHelper.GenerateTempPassword();
                DatabaseHelper.SaveTempPassword(tempPassword);
                DatabaseHelper.SendTempPasswordEmail(tempPassword, email);

                MessageBox.Show("A temporary password has been sent to your email.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Open NewPasswordForm
                var newPasswordForm = new NewPasswordForm();
                newPasswordForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid email or memorable word. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
