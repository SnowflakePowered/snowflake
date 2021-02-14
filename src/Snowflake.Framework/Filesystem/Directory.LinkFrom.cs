using System;
using System.Collections.Generic;
using System.IO;
using Emet.FileSystems;

namespace Snowflake.Filesystem
{
    internal sealed partial class Directory
    {
        public IFile LinkFrom(FileInfo source)
            => this.LinkFrom(source, source.Name);

        public IFile LinkFrom(FileInfo source, bool overwrite)
            => this.LinkFrom(source, source.Name, overwrite);

        public IFile LinkFrom(FileInfo source, string linkName) => this.LinkFrom(source, linkName, false);

        public IFile LinkFrom(FileInfo source, string linkName, bool overwrite)
        {
            this.CheckDeleted();
            string dest = Path.GetFileName(linkName);
            if (!Directory.IsValidFileName(dest))
                throw new DirectoryNotFoundException($"Filename {dest} is invalid.");
            if (!source.Exists()) throw new FileNotFoundException($"{source.FullName} could not be found.");
            if (this.ContainsFile(dest)
                && !overwrite) throw new IOException($"{source.Name} already exists in the target directory");

            var linkPath = this.ThisDirectory.Path / dest;

            FileSystem.CreateSymbolicLink(source.FullName, this.RootFileSystem.ConvertPathToInternal(linkPath), FileType.File);
            return this.OpenFile(dest);
        }

        public IDeletableDirectory LinkFrom(DirectoryInfo source)
            => this.LinkFrom(source, source.Name);

        public IDeletableDirectory LinkFrom(DirectoryInfo source, bool overwrite)
            => this.LinkFrom(source, source.Name, overwrite);

        public IDeletableDirectory LinkFrom(DirectoryInfo source, string linkName) => this.LinkFrom(source, linkName, false);

        public IDeletableDirectory LinkFrom(DirectoryInfo source, string linkName, bool overwrite)
        {
            this.CheckDeleted();
            string dest = Path.GetFileName(linkName);
            if (!Directory.IsValidFileName(dest))
                throw new DirectoryNotFoundException($"Filename {dest} is invalid.");
            if (!source.Exists()) throw new FileNotFoundException($"{source.FullName} could not be found.");
            if (this.ContainsFile(dest) && !overwrite)
                throw new IOException($"{source.Name} already exists in the target directory");

            var linkPath = this.ThisDirectory.Path / dest;

            FileSystem.CreateSymbolicLink(source.FullName, this.RootFileSystem.ConvertPathToInternal(linkPath), FileType.Directory);
            return this.OpenDirectory(dest);
        }
    }
}
