using System;
using System.Windows.Forms;

namespace Nippy_Notes.Security
{
    public partial class NewPasswordForm : Form
    {
        public NewPasswordForm()
        {
            InitializeComponent();
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            string tempPassword = TextBoxTempPassword.Text;
            string newPassword = TextBoxNewPassword.Text;
            string confirmNewPassword = TextBoxConfirmNewPassword.Text;

            if (newPassword != confirmNewPassword)
            {
                MessageBox.Show("New passwords do not match. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (DatabaseHelper.VerifyTempPassword(tempPassword))
            {
                DatabaseHelper.SaveNewPassword(newPassword);
                MessageBox.Show("Your password has been reset successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Open LoginForm
                var loginForm = new LoginForm();
                loginForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Incorrect temporary password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
