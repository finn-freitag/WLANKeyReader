using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using EasyCodeClass;
using System.IO;
using System.Reflection;

namespace WLANKeyReader
{
    public partial class Form1 : Form
    {
        //List<string> wlannames = new List<string>();
        //List<string> wlanpassworts = new List<string>();
        Dictionary<string, string> wlanAccess = new Dictionary<string, string>();
        public Dictionary<string, string> wlanAccessNew = new Dictionary<string, string>();
        int lastindex = -1;

        public Form1(bool init)
        {
            if (init) InitializeComponent();
        }

        public void LoadPasswords()
        {
            wlanAccess.Clear();
            wlanAccessNew.Clear();
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = false;
            p.StartInfo.RedirectStandardInput = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c netsh wlan show profile";
            p.OutputDataReceived += P_OutputDataReceived;
            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();
            p.CancelOutputRead();
            p.Close();
            p.Dispose();

            foreach (KeyValuePair<string, string> pair in wlanAccess)
            {
                wlanAccessNew.Add(pair.Key, pair.Value);
            }

            foreach (KeyValuePair<string, string> pair in wlanAccess)
            {
                TaggedProcess p2 = new TaggedProcess();
                p2.StartInfo.UseShellExecute = false;
                p2.StartInfo.RedirectStandardOutput = true;
                p2.StartInfo.RedirectStandardError = false;
                p2.StartInfo.RedirectStandardInput = false;
                p2.StartInfo.CreateNoWindow = true;
                p2.StartInfo.FileName = "cmd.exe";
                p2.StartInfo.Arguments = "/c netsh wlan show profile name=\"" + pair.Key + "\" key=clear";
                p2.Tag = pair.Key;
                p2.OutputDataReceived += P_OutputDataReceived1;
                p2.Start();
                p2.BeginOutputReadLine();
                p2.WaitForExit();
                p2.CancelOutputRead();
                p2.Close();
                p2.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadPasswords();

            foreach(KeyValuePair<string, string> pair in wlanAccessNew)
            {
                try
                {
                    listBox1.Items.Add(pair.Key + " : " + pair.Value);
                }
                catch { }
            }

            button1.Enabled = false;
        }

        private void P_OutputDataReceived1(object sender, DataReceivedEventArgs e)
        {
            if(e.Data != null)
            {
                if (e.Data.Contains("Schl\u0081sselinhalt"))
                {
                    string password = e.Data.Split(':')[1].Substring(1);
                    Dictionary<string, string> newAccess = new Dictionary<string, string>();
                    foreach(KeyValuePair<string, string> pair in wlanAccess)
                    {
                        if(((TaggedProcess)sender).Tag == pair.Key)
                        {
                            newAccess.Add(pair.Key, password);
                        }
                    }
                    foreach (KeyValuePair<string, string> pair in newAccess)
                    {
                        wlanAccessNew[pair.Key] = pair.Value;
                    }
                }
            }
        }

        private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if(e.Data != null)
            {
                if (e.Data.Contains("Profil f\u0081r alle Benutzer : "))
                {
                    wlanAccess.Add(e.Data.Split(new string[] { " : " }, StringSplitOptions.None)[1], "");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (WindowsTheme.isAppsUseDarkTheme())
            {
                this.BackColor = SystemColors.ControlDarkDark;
                listBox1.BackColor = SystemColors.ControlDark;
                listBox1.ForeColor = Color.White;
                button2.BackColor = SystemColors.ControlDark;
                button2.ForeColor = Color.White;
            }
            button1_Click(this, new EventArgs());
        }

        private void listBox1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                lastindex = listBox1.IndexFromPoint(e.Location);
                contextMenuStrip1.Show(new Point(e.X + this.Left + listBox1.Left, e.Y + this.Top + listBox1.Top));
            }
            catch { }
        }

        private void copyPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastindex > -1) Clipboard.SetText(((string)listBox1.Items[lastindex]).Split(':')[1].Substring(1));
        }

        private void copySSIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = ((string)listBox1.Items[lastindex]).Split(':')[0];
            if (lastindex > -1) Clipboard.SetText(str.Substring(0, str.Length - 1));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\wlanpasswords.txt";
            List<string> lines = new List<string>();
            foreach(string line in listBox1.Items)
            {
                lines.Add(line);
            }
            if (File.Exists(path))
            {
                File.AppendAllLines(path, lines);
            }
            else
            {
                File.WriteAllLines(path, lines);
            }
        }
    }
}
