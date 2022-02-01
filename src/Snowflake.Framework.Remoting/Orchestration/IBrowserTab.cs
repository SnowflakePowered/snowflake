using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Remoting.Orchestration
{
    public interface IBrowserTab : IDisposable
    {
        public void Navigate(Uri uri);
        public Uri? CurrentLocation { get; }
        public Task InitializeAsync() => this.InitializeAsync(new Uri("https://google.com"));
        public Task InitializeAsync(Uri uri);
        public NamedPipeClientStream GetCommandPipe();
    }
}
