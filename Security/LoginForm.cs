using System;
using System.Windows.Forms;

namespace Nippy_Notes.Security
{
    public partial class LoginForm : Form
    {
        private int remainingAttempts = 3;

        public string EnteredPassword { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            LblPasswordTries.Visible = false; // Hide the label initially

            // Set PasswordChar to display asterisks
            TextBoxPassword.PasswordChar = '*';

            // Add KeyDown event handler for TextBoxPassword
            TextBoxPassword.KeyDown += new KeyEventHandler(TextBoxPassword_KeyDown);

            // Add CheckedChanged event handler for CheckBoxPassword
            CheckBoxPassword.CheckedChanged += new EventHandler(CheckBoxPassword_CheckedChanged);
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            string enteredPassword = TextBoxPassword.Text;

            if (VerifyPassword(enteredPassword))
            {
                // Open main form
                var mainForm = new NippyNotes();
                mainForm.Show();
                this.Hide();
            }
            else
            {
                remainingAttempts--;

                if (remainingAttempts > 0)
                {
                    LblPasswordTries.Text = $"Remaining Attempts: {remainingAttempts}";
                    LblPasswordTries.ForeColor = System.Drawing.Color.Red;
                    LblPasswordTries.Visible = true;
                }
                else
                {
                    MessageBox.Show("Account locked, please reset your password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Open ResetPasswordForm
                    var resetPasswordForm = new ResetPasswordForm();
                    resetPasswordForm.Show();
                    this.Hide();
                }
            }
        }

        private void TextBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnConfirm_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private bool VerifyPassword(string enteredPassword)
        {
            string storedPasswordHash = DatabaseHelper.GetStoredPasswordHash();
            return SecurityHelper.VerifyPassword(enteredPassword, storedPasswordHash);
        }

        private void CheckBoxPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxPassword.Checked)
            {
                TextBoxPassword.PasswordChar = '\0'; // Show password characters
            }
            else
            {
                TextBoxPassword.PasswordChar = '*'; // Hide password characters
            }
        }
    }
}
