using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Constants.Plugin;
using Snowflake.Extensibility.Configuration;
using Snowflake.Service;
using Snowflake.Extensibility;
using Snowflake.Platform;

namespace Snowflake.Romfile
{
    public abstract class FileSignature : Extensibility.Plugin, IFileSignature
    {

        public IList<string> FileTypes { get; }
        public string SupportedPlatform { get; }
        private IPlatformInfo supportedPlatform;
        protected FileSignature(ICoreService coreInstance) : base(coreInstance)
        {
            this.FileTypes = this.PluginProperties.GetEnumerable("file_filetype").ToList();
            this.SupportedPlatform = this.SupportedPlatforms.First();
            this.supportedPlatform = this.CoreInstance.Get<IStoneProvider>().Platforms[this.SupportedPlatform];
        }

        public abstract byte[] HeaderSignature { get; }

        public virtual string GetSerial(Stream fileContents)
        {
            return "";
        }
        public virtual string GetInternalName(Stream fileContents)
        {
            return "";
        }

        public abstract bool HeaderSignatureMatches(Stream fileContents);

        /// <summary>
        /// Implement this to give a mime type based solely on file contents.
        /// Return null if nothing is found.
        /// </summary>
        /// <param name="fileContents">The contents of the ROM</param>
        /// <returns>The mime type based solely on file contents</returns>
        protected abstract string GetMimeType(Stream fileContents);

        public string GetMimeType(string fileName, Stream fileContents)
        {
            string mimeType = this.GetMimeType(fileContents);
            return String.IsNullOrWhiteSpace(mimeType)
                ? this.supportedPlatform.FileTypes[Path.GetExtension(fileName)?.ToLowerInvariant()]
                : mimeType;
        }


    }
}
