using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PasswordGenerator
{
    public partial class Settings : Form
    {
        public Settings(string path)
        {
            InitializeComponent();
            textBox1.Text = path;
        }

        public string workpath;

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fbd.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox1.Text))
            {
                workpath = textBox1.Text;
                DialogResult = DialogResult.OK;
                Close();
            }else { MessageBox.Show("Выбранного пути не существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            
        }
    }
}
