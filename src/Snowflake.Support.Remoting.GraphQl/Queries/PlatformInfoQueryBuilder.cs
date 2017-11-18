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

        [Field("platformInfo", "A Stone Platform", typeof(PlatformInfoType))]
        [Parameter(typeof(string), typeof(StringGraphType), "platformId", "The Stone PlatformID for this platform")]
        public IPlatformInfo GetPlatform(string platformId)
        {
            return this.StoneProvider.Platforms[platformId];
        }

        [Connection("platformInfos", "All registered Stone Platforms", typeof(PlatformInfoType))]
        public IEnumerable<IPlatformInfo> GetPlatforms()
        {
            return this.StoneProvider.Platforms.Values;
        }
    }
}
