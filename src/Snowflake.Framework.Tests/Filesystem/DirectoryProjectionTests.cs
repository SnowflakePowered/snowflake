using Snowflake.Orchestration.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Filesystem
{
    public class DirectoryProjectionTests
    {
        public void Test()
        {
            var p = new DirectoryProjection();
            p.Enter("")
                .Project("project", null)
                .Project("project", null)
            .Exit();
        }
    }
}
