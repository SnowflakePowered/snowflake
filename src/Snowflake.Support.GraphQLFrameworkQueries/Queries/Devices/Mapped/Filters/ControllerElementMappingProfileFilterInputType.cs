using HotChocolate.Types.Filters;
using Snowflake.Input.Controller.Mapped;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Devices.Mapped.Filters
{
    public sealed class ControllerElementMappingProfileFilterInputType
        : FilterInputType<IControllerElementMappingProfile>
    {
        protected override void Configure(IFilterInputTypeDescriptor<IControllerElementMappingProfile> descriptor)
        {
            descriptor.Name("ControllerElementMappingProfileFilter")
                .BindFieldsExplicitly();
            descriptor.Filter(c => c.DriverType)
                .AllowIn().And()
                .AllowNotIn().And()
                .AllowEquals().And()
                .AllowNotEquals();
            descriptor.Filter(c => c.ProfileName)
              .BindFiltersImplicitly();

        }
    }
}
