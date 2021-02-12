using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Projections
{
#pragma warning disable CS0618
    public class DirectoryProjection
    {
        private DirectoryProjection? Parent { get; }

        private Dictionary<string, FileSystemInfo> ProjectionState { get; }

        private Dictionary<string, DirectoryProjection> Children { get; }

        public DirectoryProjection()
        {
            this.Parent = null;
            this.ProjectionState = new();
            this.Children = new();
        }

        private DirectoryProjection(DirectoryProjection parent)
        {
            this.ProjectionState = new();
            this.Children = new();
            this.Parent = parent;
        }

        public DirectoryProjection Enter(string directoryName)
        {
            if (this.ProjectionState.ContainsKey(directoryName))
            {
                throw new ArgumentException($"Can not enter projected item {directoryName}.");
            }

            if (this.Children.TryGetValue(directoryName, out var projection))
                return projection;

            projection = new();
            this.Children.Add(directoryName, projection);
            return projection;
        }

        public DirectoryProjection Exit()
        {
            if (this.Parent == null)
            {
                throw new InvalidOperationException("Can not exist out of the root directory of a projection.");
            }
            return this.Parent;
        }

        public DirectoryProjection Project(string name, IFile file)
        {
            if (this.Children.ContainsKey(name))
            {
                throw new ArgumentException("Can not project into an existing projection level.");
            }
            this.ProjectionState[name] = file.UnsafeGetFilePath();
            return this;
        }
    }
#pragma warning restore CS0618
}
