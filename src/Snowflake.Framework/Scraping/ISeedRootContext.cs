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
        void AddSeed(SeedContent value, ISeed parent);
    }
}