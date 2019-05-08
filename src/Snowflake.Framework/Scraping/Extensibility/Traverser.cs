using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Extensibility.Provisioning.Standalone;
using Snowflake.Installation;
using Snowflake.Model.Game;
using Snowflake.Model.Records;
using Snowflake.Model.Records.File;

namespace Snowflake.Scraping.Extensibility
{
    public abstract class Traverser<TProducts, TEffectTarget>
        : ProvisionedPlugin, ITraverser<TProducts, TEffectTarget>
    {
        protected Traverser(Type pluginType)
          : this(new StandalonePluginProvision(pluginType))
        {
        }

        protected Traverser(IPluginProvision provision) : base(provision)
        {
        }

        public abstract IAsyncEnumerable<TProducts> Traverse(TEffectTarget sideEffectContext, ISeed relativeRoot, ISeedRootContext context);

        public async Task<IEnumerable<TProducts>> TraverseAll(TEffectTarget sideEffectContext, ISeed relativeRoot, ISeedRootContext context)
        {
            IList<TProducts> list = new List<TProducts>();
            await foreach (var product in this.Traverse(sideEffectContext, relativeRoot, context).ConfigureAwait(false))
            {
                list.Add(product);
            }
            return list;
        }
    }

    public abstract class GameMetadataTraverserBase
        : Traverser<IRecordMetadata, IGame>,
          IGameMetadataTraverser
    {
        protected GameMetadataTraverserBase(Type pluginType) : base(pluginType)
        {
        }

        protected GameMetadataTraverserBase(IPluginProvision provision) : base(provision)
        {
        }
    }


    public abstract class FileInstallationTraverserBase 
        : Traverser<TaskResult<IFileRecord>, IGame>,
          IFileInstallationTraverser

    {
        protected FileInstallationTraverserBase(Type pluginType) : base(pluginType)
        {
        }

        protected FileInstallationTraverserBase(IPluginProvision provision) : base(provision)
        {
        }
    }
}
