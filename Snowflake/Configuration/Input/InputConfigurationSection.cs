using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using Snowflake.Configuration.Attributes;
using Snowflake.DynamicConfiguration.Interceptors;

namespace Snowflake.Configuration.Input
{
    public class InputConfigurationSection<T> : IConfigurationSection<T> where T : class, IInputTemplate<T>
    {
        public T Configuration { get; }
        public string Destination { get; }
        public string Description { get; }
        public string DisplayName { get; }
        public string SectionName { get; }
        public IList<IConfigurationOption> Options { get; }

        public IDictionary<string, IConfigurationValue> Values
            => ImmutableDictionary.CreateRange(this.configurationInterceptor.Values);

        public object this[string key]
        {
            get { return configurationInterceptor.Values[key]; }
            set { this.configurationInterceptor.Values[key].Value = value; }
        }

        private readonly ConfigurationInterceptor configurationInterceptor;

        internal InputConfigurationSection(InputTemplateCircularInterceptor<T> interceptor, InputTemplateInterceptor<T> inputTemplate, string destinationFile, string sectionName, string displayName)
        {
            this.Destination = destinationFile;
            this.SectionName = sectionName;
            this.DisplayName = displayName;
            ProxyGenerator generator = new ProxyGenerator();
            var options = from prop in typeof(T).GetProperties()
                          where prop.HasAttribute<ConfigurationOptionAttribute>()
                          let attr = prop.GetCustomAttribute<ConfigurationOptionAttribute>()
                          let name = prop.Name
                          let metadata = prop.GetCustomAttributes<CustomMetadataAttribute>()
                          select new ConfigurationOption(attr, metadata, name) as IConfigurationOption;

            this.Options = options.ToList();
            this.configurationInterceptor =
                new ConfigurationInterceptor(this.Options.ToDictionary(p => p, p => p.Default));
            this.Configuration =
                generator.CreateInterfaceProxyWithoutTarget<T>(interceptor,
                    configurationInterceptor, inputTemplate);
        }
    }
}
