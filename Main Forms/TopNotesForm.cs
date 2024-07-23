using System;
using System.Windows.Forms;

namespace Nippy_Notes
{
    public partial class TopNotesForm : Form
    {
        public int TopNotesCount { get; private set; }

        public TopNotesForm()
        {
            InitializeComponent();
            trackBar1.Minimum = 5;
            trackBar1.Maximum = 30;
            trackBar1.Value = 5; 
            TopNotesCount = trackBar1.Value;
            LblTopNotes.Text = $"Number of top notes to fetch: {TopNotesCount}";
        }

        private void btnFetchTopNotes_Click(object sender, EventArgs e)
        {
            TopNotesCount = trackBar1.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            TopNotesCount = trackBar1.Value;
            LblTopNotes.Text = $"Number of top notes to fetch: {TopNotesCount}";
        }

        private void LblTopNotes_Click(object sender, EventArgs e)
        {
            
        }
    }
}
