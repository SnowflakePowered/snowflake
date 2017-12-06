using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping
{
    public class Seed : ISeed
    {
        public string Source { get; }
        public bool IsCulled { get; private set; }
        public Guid Guid { get; }
        public Guid Parent { get; }

        public SeedContent Content { get; }

        public Seed(SeedContent content, string source, ISeed parent)
        {
            this.Content = content;
            this.Guid = Guid.NewGuid();
            this.Parent = parent.Guid;
            this.IsCulled = false;
        }

        internal Seed(SeedContent content, Guid guid, Guid parent)
        {
            this.Content = content;
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
