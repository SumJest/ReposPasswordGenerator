using System;
using System.IO;
using System.Windows.Forms;

namespace PasswordGenerator
{
    public partial class ProfileChooser : Form
    {


        public string profile = "";

        public ProfileChooser()
        {
            InitializeComponent();
            string configpath = Application.StartupPath + "\\config\\settings.ini";
            if (!Directory.Exists(Path.GetDirectoryName(configpath)))
            {
                
                Directory.CreateDirectory(Path.GetDirectoryName(configpath));
                Console.WriteLine("Директория: " + Path.GetDirectoryName(configpath) + " создана.");
            }
            if (!File.Exists(configpath))
            {
                File.Create(configpath).Close();
                Console.WriteLine("Файл настроек: " + configpath + " создан.");
            }
            IniFile.InitFile(configpath);
            foreach (string section in IniFile.GetSectionNames()) { comboBox1.Items.Add(section); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBox1.Text))
            {
                profile = comboBox1.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateProfile cp = new CreateProfile();
            if (cp.ShowDialog() == DialogResult.OK)
            {
                comboBox1.Items.Clear();
                foreach (string sec in IniFile.GetSectionNames())
                {
                    comboBox1.Items.Add(sec);
                }
            }
        }
    }
}
