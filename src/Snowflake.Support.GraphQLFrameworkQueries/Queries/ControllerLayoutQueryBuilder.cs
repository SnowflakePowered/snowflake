using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Framework.Remoting.GraphQL.Attributes;
using Snowflake.Framework.Remoting.GraphQL.Query;
using Snowflake.Input.Controller;
using Snowflake.Services;
using Snowflake.Support.GraphQLFrameworkQueries.Types.ControllerLayout;
using Snowflake.Support.GraphQLFrameworkQueries.Types.PlatformInfo;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries
{
    public class ControllerLayoutQueryBuilder : QueryBuilder
    {
        private IStoneProvider StoneProvider { get; }

        public ControllerLayoutQueryBuilder(IStoneProvider stoneProvider)
        {
            this.StoneProvider = stoneProvider;
        }

        [Query("controllerLayout", "A Stone Controller Layout", typeof(ControllerLayoutGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "layoutID", "The Stone Layout ID for this controller")]
        public IControllerLayout GetControllerLayout(string layoutID)
        {
            return this.StoneProvider.Controllers[layoutID];
        }

        [Connection("controllerLayouts", "All Stone Controller Layouts", typeof(ControllerLayoutGraphType))]
        public IEnumerable<IControllerLayout> GetControllerLayouts()
        {
            return this.StoneProvider.Controllers.Values;
        }
    }
}
