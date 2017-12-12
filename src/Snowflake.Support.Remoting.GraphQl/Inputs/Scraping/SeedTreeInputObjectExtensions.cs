using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Scraping.Extensibility;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.Scraping
{
    public static class SeedTreeInputObjectExtensions
    {
        public static SeedTree ToSeedTree(this SeedTreeInputObject @this)
        {
            return (@this.Type, @this.Value, @this.Children?.Select(s => s.ToSeedTree()) ?? Enumerable.Empty<SeedTree>());
        }
    }
}
