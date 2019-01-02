using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;

namespace Snowflake.Configuration
{
    public abstract class ConfigurationSerializer : IConfigurationSerializer
    {
        protected const string IteratorKey = "{N}";

        protected ConfigurationSerializer(IConfigurationTypeMapper typeMapper)
        {
            this.TypeMapper = typeMapper;
        }

        protected ConfigurationSerializer(IBooleanMapping booleanMapping, string nullSerializer)
            : this(new DefaultConfigurationTypeMapper(booleanMapping, nullSerializer))
        {
        }

        /// <inheritdoc/>
        public IConfigurationTypeMapper TypeMapper { get; set; }

        /// <inheritdoc/>
        public abstract string SerializeLine<T>(string key, T value);

        /// <inheritdoc/>
        public string SerializeValue(object? value)
        {
            if (value == null)
            {
                return this.TypeMapper.ConvertValue((object?) null);
            }

            Type valueType = value.GetType();
            return this.TypeMapper[valueType, value];
        }

        /// <inheritdoc/>
        public virtual string SerializeHeader(string headerString) => string.Empty;

        /// <inheritdoc/>
        public virtual string SerializeFooter(string footerString) => string.Empty;

        /// <inheritdoc/>
        public abstract string Serialize(IConfigurationSection configurationSection);
    }
}
