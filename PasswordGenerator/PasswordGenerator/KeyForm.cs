﻿using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PasswordGenerator
{
    public partial class KeyForm : Form
    {
        string path = "";
        byte[] file;
        bool isSave;
        bool isMatch = false;
        public KeyForm(string path, byte[] file, bool isSave)
        {
            InitializeComponent();
            this.path = path;
            this.file = file;
            this.isSave = isSave;
            if (!isSave)
            {
                button1.Text = "Open";
                label1.Text = "Please enter the key to decrypt";
                textBox2.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                button1.Location = new Point(35, 80);
                panel1.Size = new Size(256, 160);
                this.Size = new Size(272, 199);
            }
            try { InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("en-US")); }
            catch (Exception) { }
        }
        
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (isSave)
            {
                if (textBox2.Text != "" && textBox1.Text != "")
                {
                    if (textBox1.Text.Equals(textBox2.Text))
                    {
                        label3.ForeColor = Color.Green;
                        label3.Text = "Passwords match";
                        isMatch = true;
                    }
                    else
                    {
                        label3.ForeColor = Color.Red;
                        label3.Text = "Passwords do not match";
                        isMatch = false;
                    }
                }
                else if (textBox2.Text == "" || textBox1.Text == "")
                {
                    label3.Text = "";
                    label3.ForeColor = Color.Black;
                    isMatch = false;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (isSave)
            {
                if (textBox2.Text != "" && textBox1.Text != "")
                {
                    if (textBox1.Text.Equals(textBox2.Text))
                    {
                        label3.ForeColor = Color.Green;
                        label3.Text = "Passwords match";
                        isMatch = true;
                    }
                    else
                    {
                        label3.ForeColor = Color.Red;
                        label3.Text = "Passwords do not match";
                        isMatch = false;
                    }
                }
                else if (textBox2.Text == "" || textBox1.Text == "")
                {
                    label3.Text = "";
                    label3.ForeColor = Color.Black;
                    isMatch = false;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (isMatch && isSave)
            {
                try
                {
                    RCC4 crypter = new RCC4(Encoding.ASCII.GetBytes(textBox1.Text));
                    byte[] efile = crypter.Encode(file);
                    FileStream savefile = File.Create(path);
                    savefile.Write(efile, 0, efile.Length);
                    savefile.Close();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }else if (!isSave)
            {
                try
                {
                    RCC4 decrypter = new RCC4(Encoding.ASCII.GetBytes(textBox1.Text));
                    FileStream openfile = File.OpenRead(path);
                    byte[] dfile = new byte[openfile.Length];
                    openfile.Read(dfile, 0, dfile.Length);
                    openfile.Close(); 
                    dfile = decrypter.Decode(dfile);
                    string text = Encoding.ASCII.GetString(dfile);
                    Clipboard.SetText(text);
                    MessageBox.Show("Password: " + text + "\nPassword been copied to the clipboard.", "Decrypted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Password don't match", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void KeyForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { button1_Click(null, null); }
        }
    }
}