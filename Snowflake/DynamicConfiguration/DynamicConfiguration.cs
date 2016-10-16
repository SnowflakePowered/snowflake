using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Snowflake.Configuration.Attributes;

namespace Snowflake.DynamicConfiguration
{
    public class DynamicConfiguration<T> : IConfigurationSection<T> where T : class, IConfigurationSection<T>
    {
        public T Configuration { get; }
        public string Destination { get; }
        public string Description { get; }
        public string DisplayName { get; }
        public string SectionName { get; }
        public IList<IConfigurationOption> Options { get; }
        public IDictionary<string, IConfigurationValue> Values => ImmutableDictionary.CreateRange(this.configurationInterceptor.Values);

        public object this[string key]
        {
            get { return configurationInterceptor.Values[key]; }
            set { this.configurationInterceptor.Values[key].Value = value; }
        }

        private readonly ConfigurationInterceptor configurationInterceptor;

        public DynamicConfiguration(string destinationFile, string sectionName, string displayName,
            string description = "")
            : this(destinationFile, sectionName, displayName, new Dictionary<string, IConfigurationValue>(), description
                )
        {
        }

        public DynamicConfiguration(string destinationFile, string sectionName, string displayName, IDictionary<string, IConfigurationValue> values, string description = "")
        {
            this.Destination = destinationFile;
            this.SectionName = sectionName;
            this.DisplayName = displayName;
            this.Description = description;
            ProxyGenerator generator = new ProxyGenerator();
            var options = typeof(T).GetProperties().Select(prop =>
                        new {attr = prop.GetCustomAttributes<ConfigurationOptionAttribute>().First(), name = prop.Name, metadata = prop.GetCustomAttributes<CustomMetadataAttribute>()})
                .Select(attr => new ConfigurationOption(attr.attr, attr.metadata, attr.name) as IConfigurationOption);
            this.Options = options.ToList();
            this.configurationInterceptor = new ConfigurationInterceptor(this.Options.ToDictionary(p => p, p => p.Default));
            foreach (var custom in values)
            {
                this.configurationInterceptor.Values[custom.Key] = custom.Value;
            }
            this.Configuration =
                generator.CreateInterfaceProxyWithoutTarget<T>(new ConfigurationCircularInterceptor<T>(this), configurationInterceptor);
        }
    }

    /// <summary>
    /// Interceptor to allow circular reference within <see cref="DynamicConfiguration{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class ConfigurationCircularInterceptor<T> : IInterceptor where T : class, IConfigurationSection<T>
    {
        private readonly IConfigurationSection<T> @this;
        public ConfigurationCircularInterceptor(IConfigurationSection<T> @this)
        {
            this.@this = @this;
        }
        public void Intercept(IInvocation invocation)
        {
            switch (invocation.Method.Name.Substring(4))
            {
                case nameof(@this.Configuration):
                    invocation.ReturnValue = @this.Configuration;
                    break;
                case nameof(@this.Destination):
                    invocation.ReturnValue = @this.Destination;
                    break;
                case nameof(@this.Options):
                    invocation.ReturnValue = @this.Options;
                    break;
                case nameof(@this.Values):
                    invocation.ReturnValue = @this.Values;
                    break;
                case nameof(@this.DisplayName):
                    invocation.ReturnValue = @this.DisplayName;
                    break;
                case nameof(@this.SectionName):
                    invocation.ReturnValue = @this.SectionName;
                    break;
                case nameof(@this.Description):
                    invocation.ReturnValue = @this.Description;
                    break;
                default:
                    invocation.Proceed();
                    break;
            }
        }
    }

    class ConfigurationInterceptor : IInterceptor
    {
        internal readonly IDictionary<string, IConfigurationValue> Values;

        public ConfigurationInterceptor(IDictionary<IConfigurationOption, object> options)
        {
            this.Values = options.ToDictionary(p => p.Key.KeyName, p => new ConfigurationValue(p.Key, p.Value) as IConfigurationValue);
        }

        public ConfigurationInterceptor(IDictionary<string, object> values, IList<IConfigurationOption> options)
        {
            this.Values = options.ToDictionary(p => p.KeyName, p 
                => new ConfigurationValue(p, values.ContainsKey(p.KeyName) ? values[p.KeyName] : p.Default) as IConfigurationValue);
        }
        public void Intercept(IInvocation invocation)
        {
            var propertyName = invocation.Method.Name.Substring(4); // remove get_ or set_
            if (!this.Values.ContainsKey(propertyName)) return;
            if (invocation.Method.Name.StartsWith("get_"))
            {
                invocation.ReturnValue = Values[propertyName].Value;
            }
            if (invocation.Method.Name.StartsWith("set_"))
            {
                Values[propertyName].Value = invocation.Arguments[0];
            }
        }
    }
}
