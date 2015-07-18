using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squirrel;

namespace Snowflake.Shell.Windows
{
    internal class StagingUpdater
    {
        private string stagingDirectory;
        public StagingUpdater(string stagingDirectory)
        {
            this.stagingDirectory = stagingDirectory;
        }

        public async void ProcessStaging()
        {
            using (var mgr = new UpdateManager("https://path/to/my/update/folder"))
            {
                await mgr.UpdateApp();
            }
        }
    }
}
