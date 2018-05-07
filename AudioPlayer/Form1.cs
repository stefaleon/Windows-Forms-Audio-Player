using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace AudioPlayer
{
    public partial class Form1 : Form
    {
        List<string> fileNames = new List<string>();
        List<string> filePaths = new List<string>();


        static string[] mediaExtensions = {
            //".PNG", ".JPG", ".JPEG", ".BMP", ".GIF", //etc
            ".WAV", ".MID", ".MIDI", ".WMA", ".MP3", ".OGG", ".RMA", //etc
            //".AVI", ".MP4", ".DIVX", ".WMV", //etc
        };

        static bool IsMediaFile(string path)
        {
            return -1 != Array.IndexOf(mediaExtensions, Path.GetExtension(path).ToUpperInvariant());
        }

        public Form1()
        {
            InitializeComponent();

            selectFilesButton.Click += selectFiles;
            listBox1.SelectedIndexChanged += playSelectedAudioFile;
            listBox1.MouseDown += listBox1MouseDown;
            clearListButton.Click += clearAllFiles;
        }

        private void clearAllFiles(object sender, EventArgs e)
        {
            fileNames.RemoveRange(0, fileNames.Count);
            filePaths.RemoveRange(0, filePaths.Count);
            listBox1.Items.Clear();
        }

        private void listBox1MouseDown(object sender, MouseEventArgs e)
        // use right click to remove items from the list
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (listBox1.SelectedIndex != -1)
                    {
                        listBox1.SelectedIndex = listBox1.IndexFromPoint(e.X, e.Y);
                        fileNames.RemoveAt(listBox1.SelectedIndex);
                        filePaths.RemoveAt(listBox1.SelectedIndex);
                        listBox1.Items.Clear();
                        listBox1.Items.AddRange(fileNames.ToArray());
                    }
                }
            }
            catch
            {
                // todo: handle error
            }


        }

        private void playSelectedAudioFile(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
                axWindowsMediaPlayer1.URL = filePaths[listBox1.SelectedIndex];
        }


        private void selectFiles(object sender, EventArgs e)
        {
            var openSelectFilesDialog = new OpenFileDialog();
            openSelectFilesDialog.Multiselect = true;

            if (openSelectFilesDialog.ShowDialog() == DialogResult.OK)
            {
                if (!IsMediaFile(openSelectFilesDialog.FileName))
                {
                    MessageBox.Show("This is not an audio file! Please select another.");
                    return;
                }

                var newFileNames = openSelectFilesDialog.SafeFileNames; // file names only
                fileNames.AddRange(newFileNames);
                var newFilePaths = openSelectFilesDialog.FileNames; // full paths
                filePaths.AddRange(newFilePaths);
                listBox1.Items.AddRange(newFileNames);
            }
        }


    }
}
