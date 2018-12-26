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
        long Length { get; }
        IDirectory ParentDirectory { get; }
        Stream OpenStream();
        Stream OpenStream(FileAccess rw);
        void Rename(string newName);
        void Delete();

        bool Created { get; }
    }
}
