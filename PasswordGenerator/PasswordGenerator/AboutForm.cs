using System;
using System.Windows.Forms;

namespace PasswordGenerator
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            string version = Application.ProductVersion;
            label3.Text = "Version: " + version;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://sumjest.ru");
        }
    }
}
