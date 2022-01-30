using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Remoting.Orchestration
{
    // todo move this to its own thing?
    public interface ICefBrowserService
    {
        public Task Initialize();
        public void Shutdown();
        public void Browse(Uri uri);
        public Uri? CurrentLocation { get; }
        public NamedPipeClientStream GetCommandPipe();
    }
}
