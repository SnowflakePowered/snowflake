using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Execution.SystemFiles;
using Snowflake.Model.Game;
using Snowflake.Services;

namespace Snowflake.Support.Execution
{
    public class SystemFileProvider : ISystemFileProvider
    {
        private DirectoryInfo SystemFileRoot { get; }

        public SystemFileProvider(IContentDirectoryProvider cdp)
        {
            this.SystemFileRoot = cdp.ApplicationData.CreateSubdirectory("bios");
        }

        public void AddSystemFile(IBiosFile biosFile, FileInfo systemFilePath)
        {
            systemFilePath.CopyTo(this.GetSystemFileInfo(biosFile).FullName, true);
        }

        public void AddSystemFile(IBiosFile biosFile, Stream systemFileStream)
        {
            using (FileStream stream = File.Create(this.GetSystemFileInfo(biosFile).FullName))
            {
                stream.Position = 0;
                systemFileStream.Position = 0;
                systemFileStream.CopyTo(stream);
                stream.Close();
            }
        }

        public Stream GetSystemFile(IBiosFile biosFile)
        {
            return File.OpenRead(this.GetSystemFileInfo(biosFile).FullName);
        }

        private FileInfo GetSystemFileInfo(IBiosFile biosFile)
        {
            var fileInfo = new FileInfo(Path.Combine(this.SystemFileRoot.FullName,
                $"{biosFile.Md5Hash}.{biosFile.FileName}"));
            return fileInfo;
        }

        public FileInfo GetSystemFilePath(IBiosFile biosFile)
        {
            var fileInfo = this.GetSystemFileInfo(biosFile);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException($"BIOS File with hash {biosFile.Md5Hash} does not exist.");
            }

            return fileInfo;
        }

        public async Task AddSystemFileAsync(IBiosFile biosFile, Stream systemFileStream)
        {
            using (FileStream stream = File.Create(this.GetSystemFileInfo(biosFile).FullName))
            {
                stream.Position = 0;
                systemFileStream.Position = 0;
                await systemFileStream.CopyToAsync(stream);
                stream.Close();
            }
        }

        public bool ContainsSystemFile(IBiosFile biosFile)
        {
            return this.GetSystemFileInfo(biosFile).Exists;
        }
    }
}
