using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Framework.Loader
{
    public class Module
    {
        public string Name { get; set; }
        public string Entry { get; set; }
        public DirectoryInfo ModuleDirectory { get; set; }
    }
}
