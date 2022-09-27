using Microsoft.EntityFrameworkCore;
using Snowflake.Filesystem.Library;
using Snowflake.Model.Database.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zio;

namespace Snowflake.Model.Database
{
    internal class ContentLibraryStore : IContentLibraryStore
    {
        private DbContextOptionsBuilder<DatabaseContext> Options { get; set; }
        public IFileSystem FileSystem { get; }

        public ContentLibraryStore(IFileSystem baseFileSystem, DbContextOptionsBuilder<DatabaseContext> options)
        {
            this.Options = options;
            using var context = new DatabaseContext(Options.Options);
            context.Database.Migrate();

            this.FileSystem = baseFileSystem;
        }

        public IEnumerable<IContentLibrary> GetLibraries()
        {
            using var context = new DatabaseContext(this.Options.Options);
            // model path is Zio-format
            return context.ContentLibraries.AsEnumerable()
                .Select(model => new ContentLibrary(
                    this.FileSystem.GetOrCreateSubFileSystem(model.Path)));
        }

        public IContentLibrary CreateLibrary(DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                directory.Create();
            }

            UPath path = this.FileSystem.ConvertPathFromInternal(directory.FullName);
            using var context = new DatabaseContext(this.Options.Options);
            if (context.ContentLibraries.Find(path) is ContentLibraryModel library)
            {
                return new ContentLibrary(this.FileSystem.GetOrCreateSubFileSystem(library.Path));
            }

            context.ContentLibraries.Add(new() { Path = (string)path });
            context.SaveChanges();
            return new ContentLibrary(this.FileSystem.GetOrCreateSubFileSystem(path));
        }
    }
}
