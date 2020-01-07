using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.Database;
using Snowflake.Filesystem;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Model.Records;
using Snowflake.Model.Records.File;
using Snowflake.Model.Records.Game;
using Zio;

namespace Snowflake.Model.Game
{
    public class Game : IGame
    {
        public IGameRecord Record { get; }
        public IDictionary<Type, IGameExtension> Extensions { get; }

        internal Game(IGameRecord record, IDictionary<Type, IGameExtension> extensions)
        {
            this.Record = record;
            this.Extensions = extensions;
        }

        public TExtension GetExtension<TExtension>() where TExtension : class, IGameExtension
        {
            if (this.Extensions.TryGetValue(typeof(TExtension), out IGameExtension? t))
            {
                return (TExtension)t;
            }
            throw new KeyNotFoundException($"Unable to find extension of type {typeof(TExtension).Name}");
        }
    }
}
