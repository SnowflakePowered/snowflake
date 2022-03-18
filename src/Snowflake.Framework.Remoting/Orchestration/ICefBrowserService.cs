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
        public Task InitializeAsync();
        public void Shutdown();
        public IBrowserTab GetTab(Guid tabId);
        public void FreeTab(Guid tabId);
    }
}
