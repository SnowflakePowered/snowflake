using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Snowflake.Records.Game;
using Snowflake.Tests.Scraper;

namespace Snowflake.Tests.ManualIntegrationTests
{
    public partial class Form1 : Form
    {
        private ScrapingIntegrationTests test;
        public Form1()
        {
            InitializeComponent();
            this.test = new ScrapingIntegrationTests();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void romFileTestButton_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Reset();
            this.openFileDialog1.ShowDialog();
            var information = this.test.GetInformation(this.openFileDialog1.FileName);
            MessageBox.Show(
                $"Mimetype: {information?.Mimetype}, Serial: {information?.Serial}, {Environment.NewLine} Name: {information?.InternalName}");
        }

        private void tgdbScrapeTest_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Reset();
            this.openFileDialog1.ShowDialog();
            var information = this.test.ScrapeTgdb(this.openFileDialog1.FileName);
            var sb = new StringBuilder();
            foreach (string key in information.Metadata.Keys)
            {
                sb.AppendLine(key + ": " + information.Metadata[key]);
            }
            MessageBox.Show(sb.ToString());
        }
    }
}
