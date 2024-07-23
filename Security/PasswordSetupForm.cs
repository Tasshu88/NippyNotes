using System;
using System.Windows.Forms;
using Nippy_Notes.Security; 

namespace Nippy_Notes
{
    public partial class PasswordSetupForm : Form
    {
        public string EnteredPassword { get; private set; }
        public string MemorableWord { get; private set; }

        public PasswordSetupForm()
        {
            InitializeComponent();
        }

        private void BtnSetPassword_Click(object sender, EventArgs e)
        {
            if (TextBoxPassword.Text == TextBoxConfirmPassword.Text)
            {
                EnteredPassword = TextBoxPassword.Text;
                MemorableWord = TextBoxWord.Text;

                // Hash password and memorable word before storing
                string hashedPassword = SecurityHelper.HashPassword(EnteredPassword);
                string hashedWord = SecurityHelper.HashPassword(MemorableWord);

                // Save hashed password and memorable word in the database
                DatabaseHelper.SaveCredentials(hashedPassword, hashedWord);

                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Passwords do not match. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelPasswordSetup_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
