using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.Game;

namespace Snowflake.Model.Game
{
    internal class BiosFile : ISystemFile
    {
        public string FileName { get; }

        public string Md5Hash { get; }

        internal BiosFile(string fileName, string md5Hash)
        {
            this.FileName = fileName;
            this.Md5Hash = md5Hash;
        }
    }
}
