using Snowflake.Orchestration.SystemFiles;
using Snowflake.Filesystem;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Snowflake.Services
{
    internal class SystemFileProvider : ISystemFileProvider
    {
        private IDirectory SystemFileRoot { get; }
        private IStoneProvider Stone { get; }

        public SystemFileProvider(IDirectory systemFileRoot, IStoneProvider stone)
        {
            this.SystemFileRoot = systemFileRoot;
            this.Stone = stone;
        }

        public IEnumerable<ISystemFile> GetMissingSystemFiles(PlatformId biosPlatform)
        {
            var biosFiles = this.Stone.Platforms[biosPlatform].BiosFiles;
            var directory = this.GetSystemFileDirectory(biosPlatform);
            
            foreach (var file in biosFiles)
            {
                if (!directory.ContainsFile(file.FileName) 
                    && directory.OpenFile(file.FileName).Created) yield return file;
            }
        }

        public IReadOnlyDirectory GetSystemFileDirectory(PlatformId biosPlatform)
        {
           return this.SystemFileRoot.OpenDirectory(biosPlatform).AsReadOnly();
        }

        public IReadOnlyFile? GetSystemFileByMd5Hash(PlatformId platformId, string md5Hash)
        {
            using var md5 = MD5.Create();
            return this.GetSystemFileDirectory(platformId).EnumerateFiles().AsParallel().FirstOrDefault(f =>
            {
                using var stream = f.OpenReadStream();
                var hash = md5.ComputeHash(stream);
                var hashString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                return hashString == md5Hash.ToLowerInvariant();
            });
        }

        public IReadOnlyFile? GetSystemFileByName(PlatformId platformId, string name)
        {
            var directory = this.GetSystemFileDirectory(platformId);
            if (directory.ContainsFile(name) && directory.OpenFile(name).Created) return directory.OpenFile(name);
            return null;
        }
    }
}
