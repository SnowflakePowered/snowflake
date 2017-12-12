using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Platform;
using Snowflake.Services;

namespace Snowflake.Emulator
{
    public class BiosManager : IBiosManager
    {
        private string BiosDirectory { get; }
        public BiosManager(string appdataDirectory)
        {
            this.BiosDirectory = Path.Combine(appdataDirectory, "bios");
        }

        /// <inheritdoc/>
        public string GetBiosDirectory(IPlatformInfo platformInfo)
        {
            string biosDirectory = Path.Combine(this.BiosDirectory, platformInfo.PlatformID);
            if (!Directory.Exists(biosDirectory))
            {
                Directory.CreateDirectory(biosDirectory);
            }

            return biosDirectory;
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetAvailableBios(IPlatformInfo platformInfo)
        {
            return Directory.GetFiles(Path.Combine(this.BiosDirectory, platformInfo.PlatformID))
                .Select(Path.GetFileName);
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetMissingBios(IPlatformInfo platformInfo)
        {
            string biosDirectory = Path.Combine(this.BiosDirectory, platformInfo.PlatformID);
            if (!Directory.Exists(biosDirectory))
            {
                return platformInfo.BiosFiles.Select(p => p.FileName);
            }

            return platformInfo.BiosFiles.Select(p => p.FileName)
                .Except(Directory.GetFiles(biosDirectory)
                           .Select(Path.GetFileName), StringComparer.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        public bool IsBiosAvailable(IPlatformInfo platformInfo, string biosName)
        {
            return File.Exists(Path.Combine(this.BiosDirectory, platformInfo.PlatformID, biosName));
        }
    }
}
