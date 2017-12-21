using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Platform
{
    public class BiosFile : IBiosFile
    {
        public string FileName { get; }

        public string Md5Hash { get; }

        public BiosFile(string fileName, string md5Hash)
        {
            this.FileName = fileName;
            this.Md5Hash = md5Hash;
        }
    }
}
