using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration
{
    public abstract class ConfigurationTypeMapper : IConfigurationTypeMapper
    {
        private readonly IDictionary<Type, Func<object, string>> typeMappers;
        private string NullSerializer { get; }
        protected ConfigurationTypeMapper(string nullSerializer)
        {
            this.typeMappers = new Dictionary<Type, Func<object, string>>();
            this.NullSerializer = nullSerializer;
        }

        public void Add<T>(Func<T, string> converter)
        {
            this.typeMappers.Add(typeof (T), value => converter.Invoke((T)value));
        }

        public string ConvertValue<T>(T value)
        {
            if (value == null) return this.NullSerializer;
            if (value is Enum && !this.typeMappers.ContainsKey(value.GetType())) //Use the default Enum converter if a custom one isn't present
                return this.typeMappers[typeof (Enum)].Invoke(value);
            return this.typeMappers.ContainsKey(value.GetType()) ? this.typeMappers[typeof (T)].Invoke(value) 
                : Convert.ToString(value);
        }
    }
}
