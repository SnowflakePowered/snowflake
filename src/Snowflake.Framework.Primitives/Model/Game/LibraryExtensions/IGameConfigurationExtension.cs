using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Configuration;

namespace Snowflake.Model.Game.LibraryExtensions
{
    /// <summary>
    /// Provides <see cref="IConfigurationCollection{T}"/> access to <see cref="IGame"/>s.
    /// </summary>
    public interface IGameConfigurationExtension : IGameExtension
    {
        /// <summary>
        /// Creates a new <see cref="IConfigurationCollection{T}"/> profile for this game.
        ///
        /// Configurations are unique on their <paramref name="sourceName"/> and <paramref name="profile"/>,
        /// NOT their prototype <typeparamref name="T"/>.
        /// In fact, it is possible to have one <see cref="IConfigurationCollection{T}"/>, but highly not recommended to
        /// be interpreted differently as different prototypes.
        /// </summary>
        /// <param name="sourceName">The name of the emulator plugin, or whatever is the source or creator of
        /// this profile. This is to avoid namespacing conflicts between emulator plugins that share
        /// a configuration prototype.</param>
        /// <param name="profile">The name of the configuration profile.</param>
        /// <typeparam name="T">The prototype of the configuration.</typeparam>
        /// <returns>A new configuration profile. If a configuration profile with the same
        /// source name and profile name exist, this will throw an exception.</returns>
        IConfigurationCollection<T> CreateNewProfile<T>(string sourceName, string profile)
            where T : class, IConfigurationCollection<T>;

        /// <summary>
        /// Gets all the names of profiles for this <see cref="IGame"/>, grouped by source.
        /// </summary>
        /// <returns>All the names of profiles for this <see cref="IGame"/>, grouped by source.</returns>
        IEnumerable<IGrouping<string, string>> GetProfileNames();

        /// <summary>
        /// Gets the specific, registered configuration profile with the given <paramref name="sourceName"/>,
        /// <paramref name="profile"/>, and interprets the profile with the prototype <typeparamref name="T"/>
        /// </summary>
        /// <param name="sourceName">The source or creator of this profile.</param>
        /// <param name="profile">The profile name.</param>
        /// <typeparam name="T">The configuration prototype.</typeparam>
        /// <returns>The previously added configuration profile. If this can not be found, throws an exception.</returns>
        IConfigurationCollection<T>? GetProfile<T>(string sourceName, string profile)
            where T : class, IConfigurationCollection<T>;

        /// <summary>
        /// Deletes the profile with the given source name and profile.
        /// </summary>
        /// <param name="sourceName">The source name or name of creator of the profile.</param>
        /// <param name="profile">The profile name,</param>
        void DeleteProfile(string sourceName, string profile);
    }

    /// <summary>
    /// Provides the <see cref="IGameConfigurationExtension"/> for a
    /// <see cref="IGame"/>, and exposes database-level methods that are not
    /// specific to a certain <see cref="IGame"/> instance.
    /// </summary>
    public interface IGameConfigurationExtensionProvider
        : IGameExtensionProvider<IGameConfigurationExtension>
    {
        /// <summary>
        /// Gets the <see cref="IConfigurationCollection{T}"/> configuration profile
        /// with the given <paramref name="valueCollectionGuid"/>
        /// </summary>
        /// <param name="valueCollectionGuid">The unique GUID of the configuration profile.</param>
        /// <typeparam name="T">The configuration collection prototype.</typeparam>
        /// <returns>The <see cref="IConfigurationCollection{T}"/> with the given GUID.</returns>
        IConfigurationCollection<T>? GetProfile<T>(Guid valueCollectionGuid)
            where T : class, IConfigurationCollection<T>;

        /// <summary>
        /// Deletes the profile with the given <paramref name="valueCollectionGuid"/>
        /// </summary>
        /// <param name="valueCollectionGuid">The unique GUID of the configuration profile.</param>
        void DeleteProfile(Guid valueCollectionGuid);

        /// <summary>
        /// Updates the given <see cref="IConfigurationValue"/>.
        /// </summary>
        /// <param name="newValue">
        /// The new <see cref="IConfigurationValue"/>.
        /// A value with the same <see cref="IConfigurationValue.Guid"/> must be present in the database,
        /// this is ensured if the value came from an existing <see cref="IConfigurationCollection{T}"/>.
        /// </param>
        void UpdateValue(IConfigurationValue newValue);
        
        /// <summary>
        /// Updates the <see cref="IConfigurationValue"/> in the database with the given
        /// <paramref name="valueGuid"/> and gives it the value <paramref name="newValue"/>.
        /// </summary>
        /// <param name="valueGuid">The unique GUID of the configuration value to update.</param>
        /// <param name="newValue">The new value of the configuration value.</param>

        void UpdateValue(Guid valueGuid, object newValue);

        /// <summary>
        /// Updates the entire <see cref="IConfigurationCollection"/>.
        /// Notice this method is type-agnostic and ignores the prototype of the <see cref="IConfigurationCollection"/>.
        /// </summary>
        /// <param name="profile">The configuration collection to update.</param>
        void UpdateProfile(IConfigurationCollection profile);
    }
    
    /// <summary>
    /// Fluent extensions to provide access to game configuration access.
    /// </summary>
    public static class GameConfigurationExtensionExtensions
    {
        /// <summary>
        /// Access configurations for this <see cref="IGame"/>.
        /// </summary>
        /// <param name="this">The <see cref="IGame"/> to access configurations for.</param>
        /// <returns>An accessor for configurations for this <see cref="IGame"/>.</returns>
        public static IGameConfigurationExtension WithConfigurations(this IGame @this)
        {
            return @this.GetExtension<IGameConfigurationExtension>()!;
        }

        /// <summary>
        /// Access configurations for the entire <see cref="IGameLibrary"/>.
        /// </summary>
        /// <param name="this">The <see cref="IGameLibrary"/> to access configurations for.</param>
        /// <returns>An accessor for configurations for this <see cref="IGameLibrary"/>.</returns>
        public static IGameConfigurationExtensionProvider
            WithConfigurationLibrary(this IGameLibrary @this)
        {
            return @this.GetExtension<IGameConfigurationExtensionProvider>();
        }
    }
}
