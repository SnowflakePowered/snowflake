using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Game.LibraryExtensions
{
    public interface IGameExtensionProvider<out TExtension> : IGameExtensionProvider
        where TExtension : class, IGameExtension
    {
        TExtension MakeExtension(IGameRecord record);
    }

    public interface IGameExtensionProvider
    {
        IGameExtension MakeExtension(IGameRecord record);
    }
}
