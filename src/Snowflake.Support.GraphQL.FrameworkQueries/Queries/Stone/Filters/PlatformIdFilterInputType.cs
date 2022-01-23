using HotChocolate.Data.Filters;
using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Queries.Stone.Filters
{
    public class PlatformIdFilterInputType : FilterInputType
    {
        protected override void Configure(IFilterInputTypeDescriptor descriptor)
        {
            descriptor.Operation(DefaultFilterOperations.Equals).Type<PlatformIdType>();
            descriptor.Operation(DefaultFilterOperations.NotEquals).Type<PlatformIdType>();
            descriptor.Operation(DefaultFilterOperations.In).Type<ListType<PlatformIdType>>();
            descriptor.Operation(DefaultFilterOperations.NotIn).Type<ListType<PlatformIdType>>();
            descriptor.Operation(DefaultFilterOperations.StartsWith).Type<StringType>();
            descriptor.Operation(DefaultFilterOperations.NotStartsWith).Type<StringType>();
        }
    }
}
