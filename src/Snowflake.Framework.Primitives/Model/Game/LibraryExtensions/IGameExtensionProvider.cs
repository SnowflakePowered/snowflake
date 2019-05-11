using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Game.LibraryExtensions
{
    /// <summary>
    /// Factory for a mixin that extends <see cref="IGameLibrary"/>.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public interface IGameExtensionProvider<out TExtension> : IGameExtensionProvider
        where TExtension : class, IGameExtension
    {
        /// <summary>
        /// Creates a new instance of the extension <typeparamref name="TExtension"/>.
        /// </summary>
        /// <param name="record">The <see cref="IGameRecord"/> that is provided to the extension.</param>
        /// <returns>A new instance of the library extension.</returns>
        new TExtension MakeExtension(IGameRecord record);
    }

    /// <summary>
    /// Factory for a mixin that extends <see cref="IGameLibrary"/>.
    /// </summary>
    public interface IGameExtensionProvider
    {
        /// <summary>
        /// Creates a new instance of the extension.
        ///
        /// This method is only used for type-erasure purposes,
        /// see <see cref="IGameExtensionProvider{TExtension}.MakeExtension"/>.
        /// </summary>
        /// <param name="record">The <see cref="IGameRecord"/> that is provided to the extension.</param>
        /// <returns>A new instance of the library extension.</returns>
        IGameExtension MakeExtension(IGameRecord record);
    }
}
