using Snowflake.Model.Game.LibraryExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Orchestration.Saving
{
    public static class GameFileExtensionGameSaveManagerExtensions
    {
        /// <summary>
        /// Provides access to save game management
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IGameSaveManager WithSaves(this IGameFileExtension @this)
        {
            return new GameSaveManager(@this.SavesRoot);
        }
    }
}
