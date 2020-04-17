using HotChocolate.Types;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Configuration
{
    public sealed class PathTypeEnum
        : EnumType<PathType>
    {
        protected override void Configure(IEnumTypeDescriptor<PathType> descriptor)
        {
            descriptor.Name("PathType")
                .Description("If the option is a PATH option, the type of path the option accepts as values.");
           
            descriptor.Value(PathType.Either)
                .Description("A contextual path that points to either a directory or a file.");
            descriptor.Value(PathType.File)
                .Description("A contextual path that points to a file.");
            descriptor.Value(PathType.Directory)
               .Description("A contextual path that points to a directory.");
            descriptor.Value(PathType.NotPath)
                .Description("Not a path.");
            descriptor.Value(PathType.Raw)
                .Description("A raw, operating-system dependent path on the realized filesystem.");
        }
    }
}
