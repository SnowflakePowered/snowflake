﻿using System;
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
        private static IImmutableDictionary<Type, IConfigurationSectionDescriptor> _sectionDescriptors = 
            ImmutableDictionary<Type, IConfigurationSectionDescriptor>.Empty;
        private static IImmutableDictionary<Type, IConfigurationCollectionDescriptor> _collectionDescriptors =
            ImmutableDictionary<Type, IConfigurationCollectionDescriptor>.Empty;

        /// <summary>
        /// Gets a new or existing section descriptor
        /// </summary>
        /// <typeparam name="T">The type of the configuration section</typeparam>
        /// <returns>The section descriptor for <see cref="T"/></returns>
        internal static IConfigurationSectionDescriptor GetSectionDescriptor<T>()
            where T : class, IConfigurationSection<T>
        {
            if (_sectionDescriptors.ContainsKey(typeof(T)))
                return _sectionDescriptors[typeof(T)];
            _sectionDescriptors = _sectionDescriptors.Add(typeof(T), new ConfigurationSectionDescriptor<T>());
            return _sectionDescriptors[typeof(T)];
        }
        
        /// <summary>
        /// Gets a new or existing collection descriptor
        /// </summary>
        /// <typeparam name="T">The type of the configuration collection</typeparam>
        /// <returns>The collection descriptor for <see cref="T"/></returns>
        internal static IConfigurationCollectionDescriptor GetCollectionDescriptor<T>() where T : class, IConfigurationCollection<T>
        {
            if (_collectionDescriptors.ContainsKey(typeof(T)))
                return _collectionDescriptors[typeof(T)];
            _collectionDescriptors = _collectionDescriptors.Add(typeof(T), new ConfigurationCollectionDescriptor<T>());
            return _collectionDescriptors[typeof(T)];
        }
    }
}
