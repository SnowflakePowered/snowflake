using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Scraping
{
    public class SeedRootContext : ISeedRootContext
    {
        public ISeed Root { get; }
        public Guid SeedCollectionGuid { get; }
        private IList<ISeed> Seeds { get; }

        public SeedRootContext()
        {
            this.Root = new Seed("root", "__root",
                Guid.NewGuid(), this.SeedCollectionGuid);
            this.Seeds = new List<ISeed>();
        }

        public IEnumerable<ISeed> GetAllOfType(string type)
        {
            return this.Seeds.Where(s => s.Type == type);
        }

        public IEnumerable<ISeed> GetChildren(ISeed seed)
        {
            return this.Seeds.Where(p => p.Parent == seed.Guid);
        }

        public IEnumerable<ISeed> GetDescendants(ISeed seed)
        {
            IEnumerable<ISeed> descendants = this.GetChildren(seed);
            foreach (var child in this.GetChildren(seed))
            {
                descendants.Concat(this.GetChildren(child)); // please don't overflow...
            }

            return descendants;
        }

        public IEnumerable<ISeed> GetRootSeeds() => this.GetChildren(this.Root);

        public void CullSeed(ISeed seed)
        {
            seed.Cull();
            foreach (var child in this.GetDescendants(seed))
            {
                child.Cull();
            }
        }
    }
}
