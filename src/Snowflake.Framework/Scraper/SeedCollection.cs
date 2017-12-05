using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Scraper
{
    public class SeedCollection
    {
        public Seed Root { get; }
        public Guid SeedCollectionGuid { get; }
        private IList<Seed> Seeds { get; }

        public SeedCollection()
        {
            this.Root = new Seed("__root", "__root",
                Guid.NewGuid(), this.SeedCollectionGuid);
            this.Seeds = new List<Seed>();
        }

        public IEnumerable<Seed> GetAllOfType(string type)
        {
            return this.Seeds.Where(s => s.Type == type);
        }

        public IEnumerable<Seed> GetChildren(Seed seed)
        {
            return this.Seeds.Where(p => p.Parent == seed.Guid);
        }

        public IEnumerable<Seed> GetRootSeeds() => this.GetChildren(this.Root);
    }
}
