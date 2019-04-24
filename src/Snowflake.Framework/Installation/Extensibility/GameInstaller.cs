using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using System.Linq;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Extensibility.Provisioning.Standalone;
using Snowflake.Model.Game;
using Snowflake.Filesystem;

namespace Snowflake.Installation.Extensibility
{
    public abstract class GameInstaller
         : ProvisionedPlugin, IGameInstaller
    {
        public GameInstaller(Type pluginType)
           : this(new StandalonePluginProvision(pluginType))
        {
        }

        public GameInstaller(IPluginProvision provision)
            : base(provision)
        {
            this.SupportedPlatforms  = this.GetType()
                .GetCustomAttributes<SupportedPlatformAttribute>().Select(p => p.PlatformId).ToList();
        }

        public IEnumerable<PlatformId> SupportedPlatforms { get; }

        public abstract IAsyncEnumerable<TaskResult<IFile>> Install(IGame game, IEnumerable<FileSystemInfo> files);
    }
}
