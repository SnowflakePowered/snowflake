using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

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

        private static ProxyGenerator proxyGenerator = new ProxyGenerator();

        /// <summary>
        /// Gets a reused proxy generator that survives for the lifetime of the application.
        /// </summary>
        internal static IProxyGenerator GetProxyGenerator()
        {
            return ConfigurationDescriptorCache.proxyGenerator;
        }

        /// <summary>
        /// Gets a new or existing section descriptor
        /// </summary>
        /// <typeparam name="T">The type of the configuration section</typeparam>
        /// <returns>The section descriptor for <see cref="T"/></returns>
        internal static IConfigurationSectionDescriptor GetSectionDescriptor<T>()
            where T : class, IConfigurationSection<T>
        {
            if (sectionDescriptors.ContainsKey(typeof(T)))
            {
                return sectionDescriptors[typeof(T)];
            }

            sectionDescriptors = sectionDescriptors.Add(typeof(T), new ConfigurationSectionDescriptor<T>());
            return sectionDescriptors[typeof(T)];
        }

        /// <summary>
        /// Gets a new or existing collection descriptor
        /// </summary>
        /// <typeparam name="T">The type of the configuration collection</typeparam>
        /// <returns>The collection descriptor for <see cref="T"/></returns>
        internal static IConfigurationCollectionDescriptor GetCollectionDescriptor<T>()
            where T : class, IConfigurationCollection<T>
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
