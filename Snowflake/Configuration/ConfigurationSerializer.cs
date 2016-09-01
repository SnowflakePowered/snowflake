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

        public IConfigurationTypeMapper TypeMapper { get; set; }

        public abstract string SerializeLine<T>(string key, T value);

        public string SerializeValue(object value)
        {
            if (value == null) return this.TypeMapper.ConvertValue((object) null);
            Type valueType = value.GetType();
            return this.TypeMapper[valueType, value];
        }

        public virtual string SerializeHeader(string headerString) => String.Empty;
        public virtual string SerializeFooter(string footerString) => String.Empty;

        public abstract string Serialize(IConfigurationSection configurationSection);

    }
}
