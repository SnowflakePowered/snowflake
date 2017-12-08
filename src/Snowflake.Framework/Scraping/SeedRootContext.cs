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
        private List<ISeed> Seeds { get; }

        public SeedRootContext()
        {
            this.Root = new Seed((SeedContent.RootSeedType, "__root"),
                Guid.NewGuid(), this.SeedCollectionGuid, "collection");
            this.Seeds = new List<ISeed>
            {
                this.Root,
            };
        }

        public ISeed this[Guid seedGuid] => this.Seeds.FirstOrDefault(s => s.Guid == seedGuid) ?? this.Root;

        public ISeed Add(SeedContent content, ISeed parent, string source)
        {
            var seed = new Seed(content, Guid.NewGuid(), parent.Guid, source);
            this.Seeds.Add(seed);
            return seed;
        }

        public IEnumerable<ISeed> GetUnculled()
        {
            return this.Seeds.Where(s => !s.IsCulled);
        }

        public IEnumerable<ISeed> GetAllOfType(string type)
        {
            return this.GetUnculled().Where(s => s.Content.Type == type);
        }

        public IEnumerable<ISeed> GetChildren(ISeed seed)
        {
            return this.GetUnculled().Where(p => p.Parent == seed.Guid);
        }

        public IEnumerable<ISeed> GetDescendants(ISeed seed)
        {
            IEnumerable<ISeed> descendants = this.GetChildren(seed);
            foreach (var child in this.GetChildren(seed))
            {
                descendants = descendants.Concat(this.GetChildren(child)); // please don't overflow...
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

        public void Add(ISeed seed)
        {
            this.Seeds.Add(seed);
        }

        public void AddRange(IEnumerable<ISeed> seeds)
        {
            this.Seeds.AddRange(seeds);
        }

        public void AddRange(IEnumerable<(SeedContent value, ISeed parent)> seeds, string source)
        {
            foreach ((SeedContent value, ISeed parent) in seeds)
            {
                this.Add(value, parent, source);
            }
        }
    }
}
