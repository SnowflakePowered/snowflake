using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Snowflake.Presentation
{
    public class NodeWebkitInstance
    {
        public string NodeWebkitPath { get; private set; }
        public Process NodeProcess { get; private set; }
        public NodeWebkitInstance(string nwPath)
        {

        }
        public void Start()
        {
            this.NodeProcess = Process.Start(new ProcessStartInfo()
            {
                FileName = NodeWebkitPath
            });
        }

    }
}
