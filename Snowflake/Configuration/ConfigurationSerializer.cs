using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

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
            var converter = typeof(IConfigurationTypeMapper).GetMethod("ConvertValue");
            return (string)converter.MakeGenericMethod(valueType).Invoke(this.TypeMapper, new[] { value });
        }
        
        public abstract string Serialize(IConfigurationSection configurationSection);

    }
}
