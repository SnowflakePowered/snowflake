using Snowflake.Input.Controller;
using Snowflake.Remoting.Marshalling;
using Snowflake.Services;
using Snowflake.Support.Remoting.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Mappers
{
    public class ControllerLayoutMapping : ITypeMapping<IControllerLayout>
    {
        private readonly IStoneProvider stoneProvider;
        public ControllerLayoutMapping(IStoneProvider stoneProvider)
        {
            this.stoneProvider = stoneProvider;
        }
        public IControllerLayout ConvertValue(string value)
        {
            return this.stoneProvider.Controllers
                .TryGetValue(value, out IControllerLayout layout) 
                ? layout
                : throw new UnknownStoneException(value);
        }
    }
}
