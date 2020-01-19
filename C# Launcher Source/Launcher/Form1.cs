using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace Launcher
{
    public partial class Form1 : Form
    {
        private bool _dragging = false;
        private Point _offset;
        private Point _start_point = new Point(0, 0);
        public string Verison;
        public string CurrentVerison;
        public int VerisonNumber;
        public int CurrentVerisonNumber;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _start_point = new Point(e.X, e.Y);
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this._start_point.X, p.Y - this._start_point.Y);
            }
        }

        private void Form1_Load(object sender, EventArgs e)//When Form first loads up
        {
            WebClient client1 = new WebClient();
            client1.DownloadFile("https://dl.dropboxusercontent.com/s/3iv3coq6eaty9lf/news.txt?dl=0.html", "Common/n102s.file");
            client1.DownloadFile("https://dl.dropboxusercontent.com/s/d0tdyrs7uydoaia/BackImage.jpg?dl=0", "Common/Core/LauncherImg.bmp");
            using (System.IO.StreamReader file = new System.IO.StreamReader("Common/n102s.file"))
            {
                string News = file.ReadToEnd();
                richTextBox1.Text = News;
            }
            this.BackgroundImage = new Bitmap("Common/Core/LauncherImg.bmp");
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            MethodInvoker Max = delegate
            {
                try
                {
                    progressBar1.Maximum = (int)e.TotalBytesToReceive / 100;
                }
                catch (Exception)
                {

                }
            };
            MethodInvoker Value = delegate
            {
                try
                {
                    progressBar1.Value = (int)e.BytesReceived / 100;
                }
                catch (Exception)
                {

                }
            };
            MethodInvoker Labelhelp = delegate
            {
                try
                {
                    label1.Text = (int)e.BytesReceived / 1000 + " KB/" + (int)e.TotalBytesToReceive / 1000 + " KB";
                }
                catch (Exception)
                {

                }
            };


            //Show download Progress
            progressBar1.Invoke(Max);
            progressBar1.Invoke(Value);
            label1.Invoke(Labelhelp);
            
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MethodInvoker DownloadComplete = delegate
            {
                try
                {
                    label1.Text = "Download Complete.";
                }
                catch (Exception)
                {

                }
            };
            MethodInvoker ExtractingFiles = delegate
            {
                try
                {
                    label1.Text = "Extracting Files.";
                }
                catch (Exception)
                {

                }
            };
            MethodInvoker DoneExtractingFiles = delegate
            {
                try
                {
                    label1.Text = "Done Extracting Files.";
                }
                catch (Exception)
                {

                }
            };
            MethodInvoker Ready = delegate
            {
                try
                {
                    label1.Text = "Ready to Play.";
                }
                catch (Exception)
                {

                }
            };
            MethodInvoker Upgrade = delegate
            {
                try
                {
                    progressBar1.Value += 20;
                }
                catch (Exception)
                {

                }
            };
            MethodInvoker Done = delegate
            {
                try
                {
                    progressBar1.Value = 100;
                    progressBar1.Maximum = 100;
                }
                catch (Exception)
                {

                }
            };
            MethodInvoker Play = delegate
            {
                try
                {
                    button1.Text = "Play";
                }
                catch (Exception)
                {

                }
            };


            //File done downloading, extract the file.
            label1.Invoke(DownloadComplete);
            label1.Invoke(ExtractingFiles);
            progressBar1.Invoke(Upgrade);
            //Uncomment this if you want to delete directories
            //Directory.Delete("Common/Core/CHeroes_Data", true);
            File.Delete("Common/Core/Program.exe");
            try
            {
                ZipFile.ExtractToDirectory("Update.zip", "Common/Core");
            }
            catch {};
            File.Delete("Update.zip");
            label1.Invoke(DoneExtractingFiles);
            progressBar1.Invoke(Done);
            button1.Invoke(Play);
            label1.Invoke(Ready);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists("Common/v102s.file"))
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("Common/v102s.file"))
                {
                    CurrentVerison = file.ReadLine();
                    CurrentVerisonNumber = Convert.ToInt32(CurrentVerison);
                }
                if (button1.Text == "Check")
                {
                    //Start Update Check
                    label1.Text = "Starting Check";
                    WebClient client2 = new WebClient();
                    client2.DownloadFile("https://dl.dropboxusercontent.com/s/vkves45qoyptroc/version.txt?dl=0.html", "Common/v102s.file");
                    using (System.IO.StreamReader file = new System.IO.StreamReader("Common/v102s.file"))
                    {
                        Verison = file.ReadLine();
                        VerisonNumber = Convert.ToInt32(Verison);
                        label1.Text = "Version Found";
                    }
                    Thread th = new Thread(Updater);
                    th.IsBackground = true;
                    th.Start();
                }
                else if (button1.Text == "Play")
                {
                    Process.Start(@"Common\Core\Program.exe");
                    this.Close();
                }
            }
            else 
            {
                CurrentVerisonNumber = 0;
            }
        }


        public void Updater() //Thread to make Updater Run smoothly
        {
            MethodInvoker Updating = delegate
            {
                try
                {
                    label1.Text = "Update Found. Downloading Now.";
                }
                catch (Exception)
                {

                }
            };
            MethodInvoker Play = delegate
            {
                try
                {
                    button1.Text = "Play";
                }
                catch (Exception)
                {

                }
            };
            MethodInvoker NoUpdate = delegate
            {
                try
                {
                    label1.Text = "No Update Found.";
                }
                catch (Exception)
                {

                }
            };
            MethodInvoker LatestVersion = delegate
            {
                try
                {
                    label1.Text = "Your Files are Up to Date.";
                }
                catch (Exception)
                {

                }
            };
            MethodInvoker Done = delegate
            {
                try
                {
                    progressBar1.Value = 100;
                    progressBar1.Maximum = 100;
                }
                catch (Exception)
                {

                }
            };



            if (VerisonNumber > CurrentVerisonNumber)
            {
                //Download Current Update from DropBox zip file!
                label1.Invoke(Updating);
                WebClient client3 = new WebClient();
                client3.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client3.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                client3.DownloadFileAsync(new Uri("https://dl.dropboxusercontent.com/s/kgxvwe48ddo2fwx/Update.zip?dl=0"), "Update.zip");
            }
            else if (VerisonNumber == CurrentVerisonNumber)
            {
                //No update start game!
                label1.Invoke(NoUpdate);
                progressBar1.Invoke(Done);
                button1.Invoke(Play);
                label1.Invoke(LatestVersion);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure that you want to quit?", "Are you sure?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                DialogResult dialogResult1 = MessageBox.Show("Really?", "Double Checking.", MessageBoxButtons.YesNo);
                if (dialogResult1 == DialogResult.Yes)
                {
                    this.Close();
                }
                else if (dialogResult1 == DialogResult.Yes){ }
            }
            else if (dialogResult == DialogResult.No) { }
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.ForeColor = System.Drawing.Color.Red;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = System.Drawing.Color.White;
        }
    }
}
