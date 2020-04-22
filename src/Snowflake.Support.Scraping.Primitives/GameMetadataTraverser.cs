using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Model.Game;
using Snowflake.Model.Records;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;

namespace Snowflake.Support.Scraping.Primitives
{
    [Plugin("Traverser-GameMetadataDefault")]
    public sealed class GameMetadataTraverser : GameMetadataTraverserBase
    {
        public GameMetadataTraverser() 
            : base(typeof(GameMetadataTraverser))
        {
        }

        public override async IAsyncEnumerable<IRecordMetadata> Traverse(IGame game, ISeed relativeRoot, ISeedRootContext context, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var result in context.GetAllOfType("result").SelectMany(s => context.GetChildren(s).Distinct()))
            {
                if (game.Record.Metadata.ContainsKey($"game_{result.Content.Type}")) continue;
                game.Record.Metadata.Add($"game_{result.Content.Type}", result.Content.Value);
                yield return (game.Record.Metadata as IDictionary<string, IRecordMetadata>)[$"game_{result.Content.Type}"];
            }
        }
    }
}
