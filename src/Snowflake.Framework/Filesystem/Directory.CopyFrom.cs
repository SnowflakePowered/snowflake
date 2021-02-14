using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsuku.Extensions;

namespace Snowflake.Filesystem
{
    internal sealed partial class Directory
    {
        public IFile CopyFrom(IReadOnlyFile source, bool overwrite) => this.CopyFrom(source, source.Name, overwrite);

        public IFile CopyFrom(IReadOnlyFile source) => this.CopyFrom(source, false);

        public IFile CopyFrom(IReadOnlyFile source, string targetName) => this.CopyFrom(source, targetName, false);

        public IFile CopyFrom(IReadOnlyFile source, string targetName, bool overwrite)
        {
            if (this.ContainsFile(Path.GetFileName(targetName)) && !overwrite)
                throw new IOException($"{Path.GetFileName(targetName)} already exists in the target directory.");
#pragma warning disable CS0618 // Type or member is obsolete
            return this.CopyFrom(source.UnsafeGetFilePath(), overwrite);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public IFile CopyFrom(FileInfo source) => this.CopyFrom(source, false);

        public IFile CopyFrom(FileInfo source, bool overwrite) => this.CopyFrom(source, Path.GetFileName(source.Name), overwrite);

        public IFile CopyFrom(FileInfo source, string targetName) => this.CopyFrom(source, Path.GetFileName(source.Name), false);

        public IFile CopyFrom(FileInfo source, string targetName, bool overwrite)
        {
            this.CheckDeleted();
            if (!source.Exists()) throw new FileNotFoundException($"{source.FullName} could not be found.");
            string? fileName = Path.GetFileName(targetName);

            if (!Directory.IsValidFileName(fileName)) throw new DirectoryNotFoundException($"Filename {targetName} is invalid.");

            source.CopyTo(this.RootFileSystem.ConvertPathToInternal(this.ThisDirectory.Path / fileName), overwrite);

            // Preserve GUID, on Linux xattrs are not preserved from copy.
            if (!source.TryGetGuidAttribute(File.SnowflakeFile, out Guid existingGuid))
                existingGuid = Guid.NewGuid();

            return this.OpenFile(fileName, existingGuid);
        }

    }
}
