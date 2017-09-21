using System;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace PasswordGenerator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length>0 && File.Exists(args[0]))
            {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new KeyOpenForm(args[0], null));
            }
            else if(args.Length>0)
            { 
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1(args[0]));
            }
            else
            {
                Application.SetCompatibleTextRenderingDefault(false);
                ProfileChooser pc = new ProfileChooser();
                if (pc.ShowDialog() == DialogResult.OK)
                {
                    Application.EnableVisualStyles();
                    Application.Run(new Form1(pc.profile));
                }
                
            }

        }
    }
}
