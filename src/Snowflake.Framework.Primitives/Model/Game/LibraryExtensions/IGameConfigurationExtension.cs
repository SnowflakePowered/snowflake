using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Configuration;

namespace Snowflake.Model.Game.LibraryExtensions
{
    public interface IGameConfigurationExtension : IGameExtension
    {
        IConfigurationCollection<T> CreateNewProfile<T>(string sourceName, string profile)
            where T : class, IConfigurationCollection<T>;

        IEnumerable<IGrouping<string, string>> GetProfileNames();

        IConfigurationCollection<T> GetProfile<T>(string sourceName, string profile)
            where T : class, IConfigurationCollection<T>;

        void DeleteProfile(string sourceName, string profile);
    }

    public interface IGameConfigurationExtensionProvider
        : IGameExtensionProvider<IGameConfigurationExtension>
    {
        IConfigurationCollection<T> GetProfile<T>(Guid valueCollectionGuid)
            where T : class, IConfigurationCollection<T>;

        void DeleteProfile(Guid valueCollectionGuid);

        void UpdateValue(IConfigurationValue newValue);

        void UpdateValue(Guid valueGuid, object newValue);

        void UpdateProfile(IConfigurationCollection profile);
    }

    public static class GameConfigurationExtensionExtensions
    {
        public static IGameConfigurationExtension WithConfigurations(this IGame @this)
        {
            return @this.GetExtension<IGameConfigurationExtension>()!;
        }

        public static IGameConfigurationExtensionProvider
            WithConfigurationLibrary(this IGameLibrary @this)
        {
            return @this.GetExtension<IGameConfigurationExtensionProvider>();
        }
    }
}
