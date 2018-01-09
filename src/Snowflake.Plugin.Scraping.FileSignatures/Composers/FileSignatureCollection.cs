using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Snowflake.Romfile;

namespace Snowflake.Plugin.Scraping.FileSignatures.Comoposers
{
    public class FileSignatureCollection : IEnumerable<KeyValuePair<string, IFileSignature>>
    {
        public FileSignatureCollection()
        {
            this.FileSignatures = new Dictionary<string, IFileSignature>();
        }

        public IFileSignature this[string mimeType] => this.FileSignatures[mimeType];

        private IDictionary<string, IFileSignature> FileSignatures { get; }

        public void Add(string mimeType, IFileSignature fileSignature)
        {
            this.FileSignatures.Add(mimeType, fileSignature);
        }

        public bool Contains(string mimeType)
        {
            return this.FileSignatures.ContainsKey(mimeType);
        }

        public IEnumerator<KeyValuePair<string, IFileSignature>> GetEnumerator()
        {
            return this.FileSignatures.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
