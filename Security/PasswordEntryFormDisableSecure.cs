using System;
using System.Windows.Forms;
using System.Drawing;

namespace Nippy_Notes
{
    public partial class PasswordEntryFormDisableSecure : Form
    {
        public string EnteredPassword { get; private set; }
        public string EnteredMemorableWord { get; private set; }
        private bool requireMemorableWord;

        public PasswordEntryFormDisableSecure()
        {
            InitializeComponent();
            AdjustFormLayout();
        }

        public PasswordEntryFormDisableSecure(bool requireMemorableWord)
        {
            InitializeComponent();
            this.requireMemorableWord = requireMemorableWord;
            AdjustFormLayout();
        }

        private void AdjustFormLayout()
        {
            if (requireMemorableWord)
            {
                TextBoxMemorableWord.Visible = true;
                LabelMemorableWord.Visible = true;
                BtnSubmitPassword.Location = new Point(73, 126);
                this.Size = new Size(245, 200);
            }
            else
            {
                TextBoxMemorableWord.Visible = false;
                LabelMemorableWord.Visible = false;
                BtnSubmitPassword.Location = new Point(73, 70);
                this.Size = new Size(245, 137);
            }
        }

        private void BtnSubmitPassword_Click(object sender, EventArgs e)
        {
            EnteredPassword = TextBoxPassword.Text;
            EnteredMemorableWord = requireMemorableWord ? TextBoxMemorableWord.Text : string.Empty;

            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
