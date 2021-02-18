using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Extensions;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Default implementation for <see cref="IConfigurationCollectionDescriptor"/>
    /// </summary>
    /// <typeparam name="T">The type of the configuration collection</typeparam>
    public class ConfigurationCollectionDescriptor<T> : IConfigurationCollectionDescriptor
        where T : class
    {
        /// <inheritdoc/>
        public IEnumerable<string> SectionKeys { get; }

        internal ConfigurationCollectionDescriptor()
        {
            var sections =
                (from props in typeof(T).GetPublicProperties()
                    where props.GetIndexParameters().Length == 0 
                        && props.PropertyType.GetInterfaces().Contains(typeof(IConfigurationSection))
                    select props).ToImmutableList();
            this.SectionKeys = ImmutableList.CreateRange(from props in sections select props.Name);
        }
    }
}
