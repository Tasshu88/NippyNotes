using System;
using System.Windows.Forms;

namespace Nippy_Notes.Security
{
    public partial class PasswordEntryFormSecure : Form
    {
        public string EnteredPassword { get; private set; }

        public PasswordEntryFormSecure()
        {
            InitializeComponent();

            // Ensure password is masked by default
            TextBoxPassword.UseSystemPasswordChar = true;

            // Add event handler for Enter key press
            TextBoxPassword.KeyDown += new KeyEventHandler(TextBoxPassword_KeyDown);
        }

        private void BtnSubmitPassword_Click(object sender, EventArgs e)
        {
            SubmitPassword();
        }

        private void TextBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SubmitPassword();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void CheckBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SubmitPassword()
        {
            EnteredPassword = TextBoxPassword.Text;
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
