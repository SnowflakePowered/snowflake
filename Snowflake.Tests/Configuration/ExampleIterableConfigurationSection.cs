using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration.Tests
{
    public sealed class ExampleIterableConfigurationSection : IterableConfigurationSection
    {
        [ConfigurationOption("SomeIterableOption{N}", IsIterable = true)]
        public int MyOption { get; set; } = 480;

        public ExampleIterableConfigurationSection(int iterationNumber)
        {
            this.SectionName = "Display";
            this.ConfigurationFileName = "Dolphin.ini";
            this.DisplayName = "Display Options";
            this.InterationNumber = iterationNumber;
        }
    }
}
