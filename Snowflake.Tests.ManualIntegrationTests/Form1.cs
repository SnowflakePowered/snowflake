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
using Newtonsoft.Json;
using Snowflake.Configuration;
using Snowflake.Configuration.Tests;
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

        private void retroarchcfgbtn_Click(object sender, EventArgs e)
        {
            /*var inst = new RetroArchInstance(cfg);
            inst.Create();*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IConfigurationCollection configuration = new ConfigurationCollection<ExampleConfigurationCollection>();
            var str = JsonConvert.SerializeObject(configuration);
            MessageBox.Show(str);
            Console.WriteLine(str);
        }
    }
}
