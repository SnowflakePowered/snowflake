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
                .Select(model => new ContentLibrary(model.LibraryId,
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
            if (context.ContentLibraries.Where(l => l.Path == path) is ContentLibraryModel library)
            {
                return new ContentLibrary(library.LibraryId, this.FileSystem.GetOrCreateSubFileSystem(library.Path));
            }

            var libraryId = Guid.NewGuid();

            context.ContentLibraries.Add(new() { Path = (string)path, LibraryId = libraryId });
            context.SaveChanges();
            return new ContentLibrary(libraryId, this.FileSystem.GetOrCreateSubFileSystem(path));
        }
    }
}
