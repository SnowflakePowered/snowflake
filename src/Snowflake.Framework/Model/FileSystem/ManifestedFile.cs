using System;
using System.Collections.Generic;
using System.Text;
using Zio;

namespace Snowflake.Model.FileSystem
{
    internal class ManifestedFile : File, IManifestedFile
    {
        internal ManifestedFile(ManifestedDirectory parentDirectory, 
            FileEntry file, Guid fileGuid, string mimeType)
            : base(parentDirectory, file)
        {
            this.FileGuid = fileGuid;
            this.MimeType = mimeType;
            this.ParentDirectory = parentDirectory;
        }

        private new ManifestedDirectory ParentDirectory { get; }
        public Guid FileGuid { get; }

        public string MimeType { get; }

        public override void Rename(string newName)
        {
            this.ParentDirectory.RenameGuidEntry(this.FileGuid, newName);
            base.Rename(newName);
        }
        public override void Delete()
        {
            this.ParentDirectory.DeleteGuidEntry(this.FileGuid);
            base.Delete();
        }


    }
}
