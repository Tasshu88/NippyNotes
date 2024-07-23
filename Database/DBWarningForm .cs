using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;
using Nippy_Notes;

namespace Nippy_Notes
{
    public partial class DBWarningForm : Form
    {
        private WaveOutEvent waveOutDevice;
        private AudioFileReader audioFileReader;

        public DBWarningForm()
        {
            InitializeComponent();
            LoadGif();
            PlaySound();
            FormUtilities.EnsureFormIsVisible(this);
        }

        private void LoadGif()
        {
            try
            {
                AhAhPictureBox.Image = Properties.Resources.ahah; 
                AhAhPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load GIF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PlaySound()
        {
            try
            {
                // Write the .WAV resource to a temporary file
                string tempWavPath = Path.Combine(Path.GetTempPath(), "AHAH.wav");
                using (var stream = Properties.Resources.AHAH1)
                {
                    using (var fileStream = new FileStream(tempWavPath, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }
                }

                waveOutDevice = new WaveOutEvent();
                audioFileReader = new AudioFileReader(tempWavPath);
                waveOutDevice.Init(audioFileReader);
                waveOutDevice.PlaybackStopped += OnPlaybackStopped;
                waveOutDevice.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to play sound: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            audioFileReader.Dispose();
            waveOutDevice.Dispose();
        }
    }
}
