using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraper
{
    public class Seed
    {
        public string Type { get; }
        public string Value { get; }
        public Guid Guid { get; }
        public Guid Parent { get; }

        public Seed(string type, string value)
        {
            this.Type = type;
            this.Value = value;
            this.Guid = Guid.NewGuid();
            this.Parent = Guid.Empty;
        }

        public Seed(string type, string value, Seed parent)
        {
            this.Type = type;
            this.Value = value;
            this.Guid = Guid.NewGuid();
            this.Parent = parent.Guid;
        }

        internal Seed(string type, string value, Guid guid, Guid parent)
        {
            this.Type = type;
            this.Value = value;
            this.Guid = guid;
            this.Parent = parent;
        }
    }
}
