using Snowflake.Input.Controller;
using Snowflake.Platform;
using Snowflake.Remoting.Marshalling;
using Snowflake.Services;
using Snowflake.Support.Remoting.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Mappers
{
    public class PlatformInfoMapping : ITypeMapping<IPlatformInfo>
    {
        private readonly IStoneProvider stoneProvider;
        public PlatformInfoMapping(IStoneProvider stoneProvider)
        {
            this.stoneProvider = stoneProvider;
        }
        public IPlatformInfo ConvertValue(string value)
        {
            return this.stoneProvider.Platforms
                .TryGetValue(value, out IPlatformInfo platform) 
                ? platform
                : throw new UnknownStoneException(value);
        }
    }
}
