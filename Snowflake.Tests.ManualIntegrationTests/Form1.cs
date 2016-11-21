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
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Nestopia.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Plugin.Emulators.RetroArch;
using Snowflake.Services;

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
            var x = this.BuildConfiguration(new ConfigurationCollection<NestopiaConfiguration>());
            foreach(var config in x)
            {
                Console.WriteLine(config.Key);
                Console.WriteLine(config.Value);
            }
        }
        protected IDictionary<string, string> BuildConfiguration(IConfigurationCollection<RetroArchConfiguration> retroArchConfiguration)
        {
            //build the configuration
            IDictionary<string, string> configurations = new Dictionary<string, string>();
            foreach (var output in retroArchConfiguration.Descriptor.Outputs.Values)
            {
                var sectionBuilder = new StringBuilder();
                var serializer = new KeyValuePairConfigurationSerializer(output.BooleanMapping, "nul", "=");
                foreach (var section in retroArchConfiguration.Where(c => retroArchConfiguration.Descriptor.GetDestination(c.Key) == output.Key))
                {
                    sectionBuilder.Append(serializer.Serialize(section.Value));
                    sectionBuilder.AppendLine();
                }
                configurations[output.Key] = sectionBuilder.ToString();
            }

            return configurations;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IConfigurationCollection<NestopiaConfiguration> configuration = new ConfigurationCollection<NestopiaConfiguration>();
            var str = JsonConvert.SerializeObject(configuration);
            MessageBox.Show(str);
            Console.WriteLine(str);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var stone = new StoneProvider();

            //IGameRecord r = new GameRecord(stone.Platforms["NINTENDO_SNES"], "christmascraze", "christmascraze.smc", "application/x-romfile-snes-magiccard");
            //var x = new 

        }
    }
}
