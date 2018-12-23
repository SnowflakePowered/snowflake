using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Model.Records.File;

namespace Snowflake.Model.FileSystem
{
    public interface IFile
    {
        string Name { get; }
        FileInfo RawInfo { get; }
        long Length { get; }
        IDirectory ParentDirectory { get; }
        FileStream OpenStream();
        FileStream OpenStream(FileAccess rw);
    }
}
