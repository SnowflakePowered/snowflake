using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace Snowflake.Tooling.Taskrunner.Tasks.PackTask
{
    public class CustomStaticDataSource : IStaticDataSource
    {
        private Stream _stream;
        // Implement method from IStaticDataSource
        public Stream GetSource()
        {
            return _stream;
        }

        // Call this to provide the memorystream
        public void SetStream(Stream inputStream)
        {
            _stream = inputStream;
            _stream.Position = 0;
        }
    }
}
