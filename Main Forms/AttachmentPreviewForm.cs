using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Nippy_Notes
{
    public partial class AttachmentPreviewForm : Form
    {
        public AttachmentPreviewForm()
        {
            InitializeComponent();

        }

        public AttachmentPreviewForm(string filePath, string fileType)
        {
            InitializeComponent();
            LoadAttachment(filePath, fileType);
        }

        private void LoadAttachment(string filePath, string fileType)
        {
            if (fileType == ".txt")
            {
                pictureBoxPreview.Visible = false;
                richTextBoxPreview.Visible = true;
                try
                {
                    richTextBoxPreview.Text = File.ReadAllText(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to read text file: {ex.Message}");
                }
            }
            else if (fileType == ".jpg" || fileType == ".jpeg" || fileType == ".png" || fileType == ".bmp" || fileType == ".gif")
            {
                richTextBoxPreview.Visible = false;
                pictureBoxPreview.Visible = true;

                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        var image = Image.FromStream(fs);
                        pictureBoxPreview.Image?.Dispose();  // Dispose any existing image
                        pictureBoxPreview.Image = new Bitmap(image);

                        // Adjust the PictureBox size to fit the image
                        AdjustPictureBoxSize(pictureBoxPreview.Image);

                        pictureBoxPreview.SizeMode = PictureBoxSizeMode.StretchImage;  // Maintain aspect ratio
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load image: {ex.Message}");
                }
            }
            // Add more file type checks as needed
        }


        private void AdjustPictureBoxSize(Image image)
        {
            // Calculate the aspect ratio
            float imageAspectRatio = (float)image.Width / image.Height;
            float containerAspectRatio = (float)pictureBoxPreview.Width / pictureBoxPreview.Height;

            if (imageAspectRatio > containerAspectRatio)
            {
                // Image is wider than the container
                pictureBoxPreview.Height = (int)(pictureBoxPreview.Width / imageAspectRatio);
                pictureBoxPreview.Top += (pictureBoxPreview.Parent.Height - pictureBoxPreview.Height) / 2;  // Center vertically
            }
            else
            {
                // Image is taller than the container
                pictureBoxPreview.Width = (int)(pictureBoxPreview.Height * imageAspectRatio);
                pictureBoxPreview.Left += (pictureBoxPreview.Parent.Width - pictureBoxPreview.Width) / 2;  // Center horizontally
            }
        }

    }
}
