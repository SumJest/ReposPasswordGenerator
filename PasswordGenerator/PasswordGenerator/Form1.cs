using System;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PasswordGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MessageBox.Show(symbols.ToString());
        }
        char[] letters = "abcdefghijklmnopqrstuvwxyz".ToCharArray(); //Symbols of alphabet
        char[] symbols = @"!@#$%&-_?".ToCharArray();
        private string newgenPassword(bool upper, bool numbers, bool symbol, int amount)
        {
            Random rand = new Random();
            string candidates = charArray2string(letters) ;
            candidates += upper ? changelength(charArray2string(letters).ToUpper(), 10) : "";
            candidates += symbol ? changelength(charArray2string(symbols).ToString(), 5) : "";
            candidates += numbers ? changelength("1234567890", 5) : "";
            string password = "";
            for (int i = 0; i < amount; i++) { password += candidates.ToCharArray()[rand.Next(0, candidates.Length)].ToString(); }
           
            return password;
        }
        private string changelength(string text, int lenght)
        {
            string newtext = "";
            Random rand = new Random();
            for (int i = 0; i < lenght; i++)
            {
                int r = rand.Next(0, text.Length);
                newtext += text.ToCharArray()[r].ToString();
                text.Remove(r, 1);
            }
            return newtext;
        }
        private string charArray2string(char[] a)
        {
            string b = "";
            foreach(char c in a){ b += c.ToString(); }
            return b;
        }
         private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = newgenPassword(checkBox1.Checked, checkBox2.Checked, checkBox3.Checked, trackBar1.Value);
            
        }
        private string getNumFileName(string filename, int number)
        {
            return Path.GetFileNameWithoutExtension(filename) + " (" + number + ")" + Path.GetExtension(filename);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Text box is empty");
              
                return;
            }
            Clipboard.SetText(textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Text box is empty");
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.StartupPath;
            string filename = "password.password";
            if (File.Exists(Application.StartupPath + @"\" + filename))
            {
                for (int i = 1; i < int.MaxValue; i++)
                {
                    if (File.Exists(Application.StartupPath + @"\" + getNumFileName(filename, i)))
                    {
                        continue;
                    }
                    else
                    {
                        sfd.FileName = getNumFileName(filename, i);
                        break;
                    }
                }
            }
            else
            {
                sfd.FileName = filename;
            }
            sfd.Filter = "Password files | *.password";
            sfd.DefaultExt = "password";

            DialogResult result = sfd.ShowDialog();

            if (result == DialogResult.OK)
            {
                string path = sfd.FileName;
                new KeyForm(path, Encoding.ASCII.GetBytes(textBox1.Text), true).ShowDialog();
            }

        }
   
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label2.Text = trackBar1.Value.ToString();
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string symbols = "";
            foreach (char symbol in this.symbols)
            {
                symbols += symbol.ToString();
            }
            SettingsForm sf = new SettingsForm(symbols);
            DialogResult dr = sf.ShowDialog();
            if (dr == DialogResult.OK)
            {
              
                this.symbols = sf.textBox1.Text.ToCharArray();
            }
        }
    }
}