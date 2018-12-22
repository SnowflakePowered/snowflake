using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Model.FileSystem
{
    public static class DirectoryExtensions
    {

        public static IFile? CopyFrom(this IDirectory @this, FileInfo file)
        {
            return @this.CopyFileFrom(file);
        }


        public static IDirectory? CopyFrom(this IDirectory @this, DirectoryInfo dir)
        {
            return @this.CopyDirectoryFrom(dir);
        }

        public static IFile? CopyFrom(this IDirectory @this, FileInfo file, string newName)
        {
            return @this.CopyFileFrom(file, newName);
        }


        public static IDirectory? CopyFrom(this IDirectory @this, DirectoryInfo dir, string newName)
        {
            return @this.CopyDirectoryFrom(dir, newName);
        }
    }
}
