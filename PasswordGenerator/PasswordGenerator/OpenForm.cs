using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Windows.Forms;

namespace PasswordGenerator
{
    public partial class OpenForm : Form
    {
        public OpenForm()
        {
            TopMost = true;
            InitializeComponent();
            
        }
        public Color Control { get; } = Color.FromArgb(-986896);
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] filedrop = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (filedrop.Length > 1)
            {
                MessageBox.Show("Пожалуйста перетащите только ОДИН файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!File.Exists(filedrop[0]) || !Path.GetExtension(filedrop[0]).Equals(".password"))
            {
                MessageBox.Show("Пожалуйста перетащите файл с расширением \".password\"", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            label2.BackColor = Control;
            
            System.Diagnostics.Process.Start(Application.ExecutablePath, "\"" + filedrop[0]+ "\"");
            this.Close();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            string[] filedrop = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) && filedrop.Length == 1 && File.Exists(filedrop[0]) && Path.GetExtension(filedrop[0]).Equals(".password"))
            {
                label2.BackColor = Color.Green;
                e.Effect = DragDropEffects.All;
            }
        }

        private void OpenForm_DragLeave(object sender, EventArgs e)
        {
            label2.BackColor = Control;
        }


        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Password's files | *.password";
            ofd.DefaultExt = ".password";
            ofd.Multiselect = false;
            DialogResult dr = ofd.ShowDialog();
            if (dr.Equals(DialogResult.OK))
            {
                System.Diagnostics.Process process = System.Diagnostics.Process.Start(Application.ExecutablePath, "\"" + ofd.FileName + "\"");
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
