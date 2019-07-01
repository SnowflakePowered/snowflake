using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Filesystem;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Model.Records;
using Snowflake.Model.Records.File;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Game
{
    /// <summary>
    /// Represents a game within an <see cref="IGameLibrary"/>.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// The game record that stores metadata about the game.
        /// </summary>
        IGameRecord Record { get; }

        /// <summary>
        /// Retrieves an extension from the <see cref="IGameLibrary"/> this game is stored in.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension.</typeparam>
        /// <returns>The extension instance.</returns>
        TExtension GetExtension<TExtension>() where TExtension : class, IGameExtension;
    }
}
