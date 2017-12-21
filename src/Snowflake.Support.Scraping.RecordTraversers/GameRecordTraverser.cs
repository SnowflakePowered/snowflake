using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.Scraping.RecordTraversers.Extensions;
using Snowflake.Utility;

namespace Snowflake.Support.Scraping.RecordTraversers
{
    public class GameRecordTraverser : ITraverser<IGameRecord>
    {
        public IStoneProvider StoneProvider { get; }
 
        public ITraverser<IFileRecord> FileTraverser { get; }

        public GameRecordTraverser(IStoneProvider provider, ITraverser<IFileRecord> fileTraverser)
        {
            this.StoneProvider = provider;
            this.FileTraverser = fileTraverser;
        }

        public IEnumerable<IGameRecord> Traverse(ISeed relativeRoot, ISeedRootContext context)
        {
            string platformId = context.GetAllOfType("platform").FirstOrDefault()?.Content.Value;
            if (!this.StoneProvider.Platforms.Keys.Contains(platformId))
            {
                yield break;
            }

            var platform = this.StoneProvider.Platforms[platformId];

            var fileRecords = this.FileTraverser.Traverse(relativeRoot, context);

            foreach (var resultSeed in context.GetAllOfType("result"))
            {
                var children = context.GetChildren(resultSeed);
                var metadataSeeds = context.GetDescendants(resultSeed)
                    .DistinctBy(p => p.Content.Type).Select(p => p.Content);
                var gameRecord = new GameRecord(platform, resultSeed.Content.Value);
                foreach (var content in metadataSeeds)
                {
                    gameRecord.Metadata[$"game_{content.Type}"] = content.Value;
                }

                foreach (var file in fileRecords.Concat(this.FileTraverser.Traverse(resultSeed, context)))
                {
                    gameRecord.Files.Add(file);
                }

                yield return gameRecord;
            }
        }
    }
}
