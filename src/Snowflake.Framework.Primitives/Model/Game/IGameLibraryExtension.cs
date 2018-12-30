using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Game
{
    public interface IGameLibraryExtension<TExtension> : IGameLibraryExtension
        where TExtension : class, IGameExtension
    {
        TExtension MakeExtension(IGameRecord record);
    }

    public interface IGameLibraryExtension
    {
        IGameExtension MakeExtension(IGameRecord record);
    }
}
