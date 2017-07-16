﻿using System;
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

        public string GetBiosDirectory(IPlatformInfo platformInfo)
        {
            string biosDirectory = Path.Combine(this.BiosDirectory, platformInfo.PlatformID);
            if (!Directory.Exists(biosDirectory)) Directory.CreateDirectory(biosDirectory);
            return biosDirectory;
        }

        public IEnumerable<string> GetAvailableBios(IPlatformInfo platformInfo)
        {
            return Directory.GetFiles(Path.Combine(this.BiosDirectory, platformInfo.PlatformID))
                .Select(Path.GetFileName);
        }

        public IEnumerable<string> GetMissingBios(IPlatformInfo platformInfo)
        {
            string biosDirectory = Path.Combine(this.BiosDirectory, platformInfo.PlatformID);
            if (!Directory.Exists(biosDirectory)) return platformInfo.BiosFiles.Select(p => p.Key);
            return platformInfo.BiosFiles.Select(p=>p.Key).Except(Directory.GetFiles(biosDirectory)
                           .Select(Path.GetFileName), StringComparer.OrdinalIgnoreCase);
        }

        public bool IsBiosAvailable(IPlatformInfo platformInfo, string biosName)
        {
            return File.Exists(Path.Combine(this.BiosDirectory, platformInfo.PlatformID, biosName));
        }
    }
}
