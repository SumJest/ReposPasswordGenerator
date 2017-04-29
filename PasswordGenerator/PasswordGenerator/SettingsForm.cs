using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordGenerator
{
    public partial class SettingsForm : Form
    {
        public SettingsForm(string text)
        {
            InitializeComponent();
            textBox1.Text = text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0) { MessageBox.Show("Field must be filled!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Hand); return; }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
