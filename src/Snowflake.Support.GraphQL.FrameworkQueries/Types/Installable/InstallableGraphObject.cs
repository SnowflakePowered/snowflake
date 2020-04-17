using Snowflake.Installation.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.Installable
{
    public class InstallableGraphObject
    {
        public InstallableGraphObject(IGameInstaller installer, IInstallable installable)
        {
            this.DisplayName = installable.DisplayName;
            this.Artifacts = installable.Artifacts.Select(a => a.FullName);
            this.InstallerName = installer.Name;
        }

        public string DisplayName { get; }
        public IEnumerable<string> Artifacts { get; }
        public string InstallerName { get; }
    }
}
