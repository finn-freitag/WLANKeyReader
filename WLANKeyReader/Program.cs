using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WLANKeyReader
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 1 && (args[0].Contains("help") || args[0].Contains("?")))
            {
                Console.WriteLine("Start without parameters to see the GUI.");
                //Debug.WriteLine("Start without parameters to see the GUI.");
                Console.WriteLine("Type \"/help\" or \"/?\" to see this info.");
                //Debug.WriteLine("Type \"/help\" or \"/?\" to see this info.");
                Console.WriteLine("Type \"/view\" to view all passwords.");
                //Debug.WriteLine("Type \"/view\" to view all passwords.");
                Console.WriteLine("Type \"/save\" to append all passwords to \"wlanpasswords.txt\".");
                //Debug.WriteLine("Type \"/save\" to append all passwords to \"wlanpasswords.txt\".");
                Console.WriteLine("Type \"/save YourFileName.txt\" to append all passwords to your file.");
                //Debug.WriteLine("Type \"/save YourFileName.txt\" to append all passwords to your file.");
                Console.WriteLine("Type \"/save C:\\User\\YourFileName.txt\" to append all passwords to your file at a specific location.");
                //Debug.WriteLine("Type \"/save C:\\User\\YourFileName.txt\" to append all passwords to your file at a specific location.");
            }
            if(args.Length == 1 && args[0] == "/view")
            {
                Form1 form = new Form1(false);

                form.LoadPasswords();

                foreach(KeyValuePair<string, string> pair in form.wlanAccessNew)
                {
                    Console.WriteLine(pair.Key + " : " + pair.Value);
                }

                form.Close();
                form.Dispose();
            }
            if ((args.Length == 1 && args[0] == "/save") || (args.Length == 2 && args[0] == "/save"))
            {
                Form1 form = new Form1(false);

                form.LoadPasswords();

                string path = Directory.GetCurrentDirectory() + "\\wlanpasswords.txt";

                if (args.Length == 2)
                {
                    if (Path.IsPathRooted(args[1])) path = args[1]; else path = Path.Combine(Directory.GetCurrentDirectory(), args[1]);
                }

                List<string> lines = new List<string>();
                foreach (KeyValuePair<string, string> pair in form.wlanAccessNew)
                {
                    lines.Add(pair.Key + " : " + pair.Value);
                }
                if (File.Exists(path))
                {
                    File.AppendAllLines(path, lines);
                }
                else
                {
                    File.WriteAllLines(path, lines);
                }

                Console.WriteLine("Passwords Saved!");
                //Debug.WriteLine("Passwords Saved!");

                form.Close();
                form.Dispose();
            }

            if (args.Length > 0)
            {
                Application.Exit();
                Environment.Exit(0);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1(true));
            }
        }
    }
}
