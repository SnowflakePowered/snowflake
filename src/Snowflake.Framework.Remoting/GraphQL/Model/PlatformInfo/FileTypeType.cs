using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.PlatformInfo
{
    internal sealed class FileTypeType
        : ObjectType<KeyValuePair<string, string>>
    {
        protected override void Configure(IObjectTypeDescriptor<KeyValuePair<string, string>> descriptor)
        {
            descriptor.Name("FileType")
                .Description("A FileType is defined by its common extension, and a mimetype.");
            descriptor.Field(p => p.Key).Name("extension").Description("The file extension of the file type");
            descriptor.Field(p => p.Value).Name("mimetype").Description("The mimetype of the file type");
        }
    }
}
