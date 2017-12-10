using System;
using System.Collections.Generic;

namespace Snowflake.Scraping
{
    public interface ISeedRootContext
    {
        /// <summary>
        /// Returns the seed with the given GUID. If it does not exists, returns the root.
        /// </summary>
        /// <param name="seedGuid"></param>
        /// <returns></returns>
        ISeed this[Guid seedGuid] { get; }
        ISeed Root { get; }
        Guid SeedCollectionGuid { get; }
        IEnumerable<ISeed> GetAll();
        IEnumerable<ISeed> GetUnculled();
        IEnumerable<ISeed> GetAllOfType(string type);
        IEnumerable<ISeed> GetChildren(ISeed seed);
        IEnumerable<ISeed> GetSiblings(ISeed seed);
        IEnumerable<ISeed> GetRootSeeds();
        IEnumerable<ISeed> GetDescendants(ISeed seed);
        ISeed Add(SeedContent value, ISeed parent, string source);
        void CullSeedTree(ISeed seed);
        void Add(ISeed seed);
        void AddRange(IEnumerable<ISeed> seeds);
        void AddRange(IEnumerable<(SeedContent value, ISeed parent)> seeds, string source);
    }
}