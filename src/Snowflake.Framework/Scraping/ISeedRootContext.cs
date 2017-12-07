using System;
using System.Collections.Generic;

namespace Snowflake.Scraping
{
    public interface ISeedRootContext
    {
        ISeed Root { get; }
        Guid SeedCollectionGuid { get; }
        IEnumerable<ISeed> GetAllOfType(string type);
        IEnumerable<ISeed> GetChildren(ISeed seed);
        IEnumerable<ISeed> GetRootSeeds();
        ISeed Add(SeedContent value, ISeed parent, string source);
        void Add(ISeed seed);
        void AddRange(IEnumerable<ISeed> seeds);
        void AddRange(IEnumerable<(SeedContent value, ISeed parent)> seeds, string source);
    }
}