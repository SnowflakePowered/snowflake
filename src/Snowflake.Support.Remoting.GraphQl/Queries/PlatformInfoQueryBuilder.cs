using GraphQL.Types;
using Snowflake.Platform;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;
using Snowflake.Support.Remoting.GraphQl.Framework.Query;
using Snowflake.Support.Remoting.GraphQl.Types.PlatformInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Queries
{
    public class PlatformInfoQueryBuilder : QueryBuilder
    {
        private IStoneProvider StoneProvider { get; }
        public PlatformInfoQueryBuilder(IStoneProvider stoneProvider)
        {
            this.StoneProvider = stoneProvider;
        }

        [Field("platformInfo", "A Stone Platform", typeof(PlatformInfoGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "platformID", "The Stone PlatformID for this platform")]
        public IPlatformInfo GetPlatform(string platformID)
        {
            return this.StoneProvider.Platforms[platformID];
        }

        [Connection("platformInfos", "All Registered Stone Platforms", typeof(PlatformInfoGraphType))]
        public IEnumerable<IPlatformInfo> GetPlatforms()
        {
            return this.StoneProvider.Platforms.Values;
        }
    }
}
