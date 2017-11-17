using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.PlatformInfo
{
    internal class FileTypeType : ObjectGraphType<FileType>
    {
        public FileTypeType()
        {
            Name = "FileType";
            Field(p => p.Extension).Description("The file extension of the file type");
            Field(p => p.Mime).Description("The mimetype of the file type");
        }
    }

    internal class FileType
    {
        public string Extension { get; set; }
        public string Mime { get; set; }
    }

}
