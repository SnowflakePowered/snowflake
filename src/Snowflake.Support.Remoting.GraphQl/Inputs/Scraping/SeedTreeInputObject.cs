using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.Scraping
{
    public class SeedTreeInputObject
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public IEnumerable<SeedTreeInputObject> Children { get; set; }
    }
}
