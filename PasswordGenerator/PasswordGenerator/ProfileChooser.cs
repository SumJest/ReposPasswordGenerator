﻿using System;
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
    public partial class ProfileChooser : Form
    {
        IniFile file;

        public string profile = "";

        public ProfileChooser()
        {
            InitializeComponent();
            file = new IniFile("Configuration\\settings.ini");
            foreach (string section in file.GetSectionNames()) { comboBox1.Items.Add(section); }
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
                foreach (string sec in file.GetSectionNames())
                {
                    comboBox1.Items.Add(sec);
                }
            }
        }
    }
}