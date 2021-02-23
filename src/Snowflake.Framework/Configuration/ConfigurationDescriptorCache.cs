using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Provides a thread-safe cache for collection descriptors.
    /// Since descriptors use heavy attribute access, the descriptor cache caches
    /// the attributes for types that will not change throughout the
    /// lifetime of the application.
    /// </summary>
    internal static class ConfigurationDescriptorCache
    {
        private static IImmutableDictionary<Type, IConfigurationSectionDescriptor> sectionDescriptors =
            ImmutableDictionary<Type, IConfigurationSectionDescriptor>.Empty;

        private static IImmutableDictionary<Type, IConfigurationCollectionDescriptor> collectionDescriptors =
            ImmutableDictionary<Type, IConfigurationCollectionDescriptor>.Empty;

        /// <summary>
        /// Gets a new or existing section descriptor
        /// </summary>
        /// <typeparam name="T">The type of the configuration section</typeparam>
        /// <returns>The section descriptor for <see cref="T:self"/></returns>
        internal static IConfigurationSectionDescriptor GetSectionDescriptor<T>(string sectionKey)
            where T : class, IConfigurationSection<T>
        {
            if (sectionDescriptors.ContainsKey(typeof(T)))
            {
                return sectionDescriptors[typeof(T)];
            }

            sectionDescriptors = sectionDescriptors.Add(typeof(T), new ConfigurationSectionDescriptor<T>(sectionKey));
            return sectionDescriptors[typeof(T)];
        }

        /// <summary>
        /// Gets a new or existing collection descriptor
        /// </summary>
        /// <typeparam name="T">The type of the configuration collection</typeparam>
        /// <returns>The collection descriptor for <see cref="T:self"/></returns>
        internal static IConfigurationCollectionDescriptor GetCollectionDescriptor<T>()
            where T : class
        {
            if (collectionDescriptors.ContainsKey(typeof(T)))
            {
                return collectionDescriptors[typeof(T)];
            }

            collectionDescriptors = collectionDescriptors.Add(typeof(T), new ConfigurationCollectionDescriptor<T>());
            return collectionDescriptors[typeof(T)];
        }
    }
}
