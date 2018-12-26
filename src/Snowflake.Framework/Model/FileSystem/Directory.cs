using System;
using System.Collections.Generic;
using System.IO;
using IO = System.IO;
using System.Text;
using System.Linq;
using Zio;

namespace Snowflake.Model.FileSystem
{
    public class Directory : IDirectory
    {
        internal Directory(string name, IFileSystem fs)
        {
            this.FileSystem = fs;
            this.Name = name;
        }

        private IFileSystem FileSystem { get; }

        public string Name { get; }

        public bool ContainsDirectory(string directory)
        {
            return this.FileSystem.DirectoryExists((UPath)"/" / directory);
        }

        public bool ContainsFile(string file)
        {
            return this.FileSystem.FileExists((UPath)"/" / file);
        }

        public IDirectory OpenDirectory(string name)
        {
            return new Directory(name, 
               this.FileSystem.GetOrCreateSubFileSystem((UPath)"/" / new UPath(name).GetName()));
        }

        public IEnumerable<IDirectory> EnumerateDirectories()
        {
            return this.FileSystem.EnumerateDirectories("/")
                .Select(d => this.OpenDirectory(d.GetName()));
        }

        public IEnumerable<IFile> EnumerateFiles()
        {
            return this.FileSystem.EnumerateFiles("/")
                    .Select(f => this.OpenFile(f));
        }

        private IFile OpenFile(UPath file)
        {
           return new File(this, new FileEntry(this.FileSystem, file));
        }

        public IFile OpenFile(string file)
        {
            return this.OpenFile((UPath)"/" / Path.GetFileName(file));
        }

        public DirectoryInfo GetPath()
        {
            return new DirectoryInfo(this.FileSystem.ConvertPathToInternal("/"));
        }
    }
}
