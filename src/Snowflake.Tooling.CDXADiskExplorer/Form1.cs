using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Snowflake.Plugin.Scraping.FileSignatures.Formats.CDXA;

namespace Snowflake.Tooling.CDXADiskExplorer
{
    public partial class Form1 : Form
    {
        private IList<CDXAFile> Files = null;
        public Form1()
        {
            InitializeComponent();
            this.openFileDialog1.FileOk += OpenFileDialog1_FileOk;
            this.dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var file = this.Files[e.RowIndex];
            using (var fileStream = File.Create("Extracted." + Path.GetFileName(file.Path)))
            {
                byte[] firstFive = new byte[5];
                var cdxaStream = file.OpenFile();
                cdxaStream.CopyTo(fileStream);
                MessageBox.Show($"Copied {file.Path}");
            }
        }

        private void SetupGui(Stream psxStream)
        {
            var psxDisk = new CDXADisk(psxStream);
            this.gameLabel.Text = $"{psxDisk.VolumeDescriptor}";
            this.Files = psxDisk.Files.Values.ToList();
            this.dataGridView1.DataSource = this.Files;
        }

        private void OpenFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            this.SetupGui(this.openFileDialog1.OpenFile());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
