using HotChocolate.Types;
using Snowflake.Filesystem.Library;
using Snowflake.Installation.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Remoting.GraphQL.Model.Filesystem
{
    public sealed class ContentLibraryType
        : ObjectType<IContentLibrary>
    {
        protected override void Configure(IObjectTypeDescriptor<IContentLibrary> descriptor)
        {
            descriptor.Name("ContentLibrary")
                .Description("Describes a content library.");

            descriptor.Field(p => p.LibraryID)
                .Name("libraryId")
                .Description("The unique ID of the content library.")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(p => p.Path)
                .Name("path")
                .Description("The actual path of the library on disk.")
                .Type<NonNullType<OSDirectoryPathType>>();
        }
    }
}
