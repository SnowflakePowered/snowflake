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
        public IFile MoveFrom(IFile source) => this.MoveFrom(source, false);

        public IFile MoveFrom(IFile source, bool overwrite)
            => this.MoveFrom(source, source.Name, overwrite);

        public IFile MoveFrom(IFile source, string targetName) => this.MoveFrom(source, targetName, false);

        public IFile MoveFrom(IFile source, string targetName, bool overwrite)
        {
            this.CheckDeleted();
#pragma warning disable CS0618 // Type or member is obsolete
            string dest = Path.GetFileName(targetName);
            if (!Directory.IsValidFileName(dest)) throw new DirectoryNotFoundException($"Filename {dest} is invalid.");
            if (!source.Created) throw new FileNotFoundException($"{source.UnsafeGetPath().FullName} could not be found.");
            if (this.ContainsFile(dest) && !overwrite) throw new IOException($"{source.Name} already exists in the target directory");
            // Preserve GUID

            if (!this.FileGuidProvider.TryGetGuid(source.UnsafeGetPath(), out Guid existingGuid))
                existingGuid = Guid.NewGuid();
            var file = this.OpenFile(dest, existingGuid);

            // unsafe usage here as optimization.
            source.UnsafeGetPath()
                .MoveTo(file.UnsafeGetPath().ToString(), overwrite);
#pragma warning restore CS0618 // Type or member is obsolete
            source.Delete();
            return this.OpenFile(dest);
        }
    }
}
