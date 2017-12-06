using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping
{
    public class Seed : ISeed
    {
        public string Type { get; }
        public string Value { get; }
        public string Source { get; }
        public bool IsCulled { get; private set; }
        public Guid Guid { get; }
        public Guid Parent { get; }

        public Seed(string type, string value, string source, ISeed parent)
        {
            this.Type = type;
            this.Value = value;
            this.Guid = Guid.NewGuid();
            this.Parent = parent.Guid;
            this.IsCulled = false;
        }

        internal Seed(string type, string value, Guid guid, Guid parent)
        {
            this.Type = type;
            this.Value = value;
            this.Guid = guid;
            this.Parent = parent;
            this.IsCulled = false;
        }

        public void Cull()
        {
            this.IsCulled = true;
        }
    }
}
