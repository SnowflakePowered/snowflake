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
            string realName = Path.GetFileName(directoryName);

            if (this.ProjectionState.ContainsKey(realName))
            {
                throw new ArgumentException($"Can not enter projected item {realName}.");
            }

            if (this.Children.TryGetValue(realName, out var projection))
                return projection;

            projection = new(this);
            this.Children.Add(realName, projection);
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
            string realName = Path.GetFileName(name);
            if (this.Children.ContainsKey(realName))
            {
                throw new ArgumentException("Can not project into an existing projection level.");
            }
            this.ProjectionState[realName] = file.UnsafeGetFilePath();
            return this;
        }
    
        public IReadOnlyDirectory Mount(IDisposableDirectory autoDisposingDirectory, string mountRoot)
        {
            IDirectory mountDir;
            if ((mountRoot == "/" || mountRoot == "") && 
                !autoDisposingDirectory.EnumerateFiles().Any() &&
                !autoDisposingDirectory.EnumerateDirectories().Any())
            {
                mountDir = autoDisposingDirectory;
            }
            // ContainsFile checks for directories as well.
            else if (!autoDisposingDirectory.ContainsFile(mountRoot)) 
            {
                mountDir = autoDisposingDirectory.OpenDirectory(mountRoot);
            }
            else
            {
                throw new IOException("Can not mount projection on an non-empty directory or already existing mount point.");
            }

            if (this.Parent != null)
            {
                throw new InvalidOperationException("Can not mount subtreee of projecction.");
            }

            var activeDir = mountDir;
            foreach (var (name, file) in this.ProjectionState)
            {
                // todo: project symlink from file to name here.
            }
            
            Stack<(string, DirectoryProjection)> projectionsToProcess = new(this.Children.Select((kvp) => (kvp.Key, kvp.Value)));
            while (projectionsToProcess.Count > 0)
            {
                var (projectionName, projectionLevel) = projectionsToProcess.Pop();
                activeDir = activeDir.OpenDirectory(projectionName);
                foreach (var (name, file) in projectionLevel.ProjectionState)
                {
                    // todo: project symlink from file to name here.
                }
                foreach (var (name, childProjection) in projectionLevel.Children)
                {
                    projectionsToProcess.Push((name, childProjection));
                }
            }
            return mountDir.AsReadOnly();
        }
    }
#pragma warning restore CS0618
}
