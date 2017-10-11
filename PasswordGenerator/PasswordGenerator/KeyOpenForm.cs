using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;

namespace PasswordGenerator
{
    public partial class KeyOpenForm : Form
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);

        string path = "";
        byte[] file;
        public KeyOpenForm(string path, byte[] file)
        {
            this.path = path;
            this.file = file;
            InitializeComponent();
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            try { InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("en-US")); }
            catch (Exception) { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                RCC5 decrypter = new RCC5(Encoding.ASCII.GetBytes(textBox1.Text));
                FileStream openfile = File.OpenRead(path);
                byte[] dfile = new byte[openfile.Length];
                openfile.Read(dfile, 0, dfile.Length);
                openfile.Close();
                dfile = decrypter.Decode(dfile);
                string text = Encoding.ASCII.GetString(dfile);
                if(checkBox1.Checked)Clipboard.SetText(text);
                MessageBox.Show((checkBox2.Checked ? "Password: " + text + "\n" : "") + (checkBox1.Checked ? "Password been copied to the clipboard." : ""), "Decrypted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Incorrect key or file corrupted."))
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
                        if (checkBox1.Checked) Clipboard.SetText(text);
                        MessageBox.Show((checkBox2.Checked ? "Password: " + text + "\n" : "") + (checkBox1.Checked ? "Password been copied to the clipboard." : ""), "Decrypted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MessageBox.Show("Алгоритм шифрования этого файла устарел и в скором времени его поддержка будет остановлена! \nВо избежании потери пароля, пожалуйста перешифруйте его в этой программе!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Close();
                    }catch(Exception exc)
                    {
                        MessageBox.Show(exc.Message, exc.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
                MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked && !checkBox2.Checked)
            {
                checkBox1.Checked = true;
                MessageBox.Show("You can't turn off all the checkboxes", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked && !checkBox2.Checked)
            {
                checkBox2.Checked = true;
                MessageBox.Show("You can't turn off all the checkboxes", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.PasswordChar = default(char);
        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            textBox1.PasswordChar = '•';
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label8.ForeColor = (((ushort)GetKeyState(0x90)) & 0xffff) == 0 ? Color.Black : Color.LimeGreen;
            label9.ForeColor = (((ushort)GetKeyState(0x14)) & 0xffff) == 0 ? Color.Black : Color.LimeGreen;

        }
    }
}
