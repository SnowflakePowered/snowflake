using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Projections
{
    public static class DirectoryProjectionExtensions
    {
        public static DirectoryProjection P(this DirectoryProjection d, string name, IFile file)
            => d.Project(name, file);
        public static DirectoryProjection N(this DirectoryProjection d, string name)
            => d.Enter(name);
        public static DirectoryProjection X(this DirectoryProjection d)
           => d.Exit();
    }
}