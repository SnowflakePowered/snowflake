using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Services
{
    internal class ContentDirectoryProvider : IContentDirectoryProvider
    {
        public ContentDirectoryProvider(string appDataDirectory)
        {
            this.ApplicationData = new DirectoryInfo(appDataDirectory);
        }
        public DirectoryInfo ApplicationData { get; }
        
    }
}
