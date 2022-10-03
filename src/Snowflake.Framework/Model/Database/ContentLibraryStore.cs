using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using Snowflake.Filesystem;
using Snowflake.Filesystem.Library;
using Snowflake.Model.Database.Exceptions;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Database.Models;
using Snowflake.Model.Records;
using Snowflake.Model.Records.Game;
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
                .Select(model => new ContentLibrary(model.LibraryID,
                    new Filesystem.Directory(this.FileSystem.GetOrCreateSubFileSystem(model.Path))));
        }

        public IContentLibrary CreateLibrary(DirectoryInfo dirInfo)
        {
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            using var context = new DatabaseContext(this.Options.Options);
            if (context.ContentLibraries.Where(l => l.Path == dirInfo.FullName) is ContentLibraryModel library)
            {
                return new ContentLibrary(library.LibraryID, new Filesystem.Directory(this.FileSystem.GetOrCreateSubFileSystem(this.FileSystem.ConvertPathFromInternal(library.Path))));
            }

            var libraryId = Guid.NewGuid();

            context.ContentLibraries.Add(new() { Path = dirInfo.FullName, LibraryID = libraryId });
            context.SaveChanges();
            return new ContentLibrary(libraryId,
                new Filesystem.Directory(this.FileSystem.GetOrCreateSubFileSystem(this.FileSystem.ConvertPathFromInternal(dirInfo.FullName))));
        }


        public IContentLibrary? GetLibrary(Guid libraryId)
        {
            using var context = new DatabaseContext(this.Options.Options);
            if (context.ContentLibraries.Find(libraryId) is ContentLibraryModel model)
            {
                return new ContentLibrary(model.LibraryID, new Filesystem.Directory(this.FileSystem.GetOrCreateSubFileSystem(this.FileSystem.ConvertPathFromInternal(model.Path))));
            }
            return null;
        }

        public IContentLibrary? GetRecordLibrary(IRecord record)
        {
            using var context = new DatabaseContext(this.Options.Options);
            if (context.ContentLibraries.FirstOrDefault(g => g.Records.Select(g => g.RecordID).Contains(record.RecordID)) is ContentLibraryModel model)
            {
                return new ContentLibrary(model.LibraryID, new Filesystem.Directory(this.FileSystem.GetOrCreateSubFileSystem(this.FileSystem.ConvertPathFromInternal(model.Path))));
            }
            return null;
        }

        public void SetRecordLibrary(IContentLibrary library, IRecord record)
        {
            using var context = new DatabaseContext(this.Options.Options);
            RecordModel? recordModel = context.Records.Find(record.RecordID);
            if (recordModel == null)
            {
                throw new DependentEntityNotExistsException(record.RecordID);
            }
            recordModel.ContentLibrary = library.AsModel();
            context.SaveChanges();
        }
    }
}
