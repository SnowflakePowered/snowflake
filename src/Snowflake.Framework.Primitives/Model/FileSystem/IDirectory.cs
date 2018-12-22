using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Snowflake.Model.FileSystem
{
    public interface IDirectory
    {
        DirectoryInfo RawInfo { get; }

        string Name { get; }

        IDirectory? OpenDirectory(string name);
        IDirectory CreateOrOpenDirectory(string name);

        IDirectory? CopyDirectoryFrom(string fullPath);
        IDirectory? CopyDirectoryFrom(DirectoryInfo directory);

        IDirectory? CopyDirectoryFrom(string fullPath, string newName);
        IDirectory? CopyDirectoryFrom(DirectoryInfo directory, string newName);

        IFile? OpenFile(string file);
        IFile CreateOrOpenFile(string file);

        IFile? CopyFileFrom(string fullPath);
        IFile? CopyFileFrom(FileInfo file);

   
        IFile? CopyFileFrom(string fullPath, string newName);
        IFile? CopyFileFrom(FileInfo file, string newName);

        IDirectory Rename(IDirectory directory, string newName);
        IFile Rename(IFile file, string newName);

        IEnumerable<IDirectory> EnumerateDirectories();
        IEnumerable<IFile> EnumerateFiles();

        bool ContainsFile(string file);
        bool ContainsDirectory(string directory);

        void DeleteChild(IDirectory directory);
        void DeleteChild(IFile file);
    }
}
