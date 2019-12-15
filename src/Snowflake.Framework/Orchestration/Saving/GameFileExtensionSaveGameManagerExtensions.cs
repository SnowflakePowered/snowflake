using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Orchestration.Saving
{
    public static class GameFileExtensionSaveGameManagerExtensions
    {
        /// <summary>
        /// Provides SaveGameManagement
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static SaveGameManager WithSaves(this IGameFileExtension @this)
        {
            return new SaveGameManager(@this.SavesRoot);
        }
    }
}
