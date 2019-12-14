using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input;
using Snowflake.Configuration.Interceptors;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;

namespace Snowflake.Configuration.Input
{
    public class InputTemplate<T> : IInputTemplate<T>
        where T : class, IInputTemplate<T>
    {
        /// <inheritdoc/>
        public int PlayerIndex { get; }

        /// <inheritdoc/>
        public T Template { get; }

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, DeviceCapability> Values
            => ImmutableDictionary.CreateRange(this.inputTemplateInterceptor.InputValues);

        /// <inheritdoc/>
        IEnumerable<IInputOption> IInputTemplate.Options =>
            ImmutableList.CreateRange(this._Options.Select(p => p.Value));

        private IConfigurationSection<T> Configuration { get; }

        private IDictionary<string, IInputOption> _Options { get; }

        private readonly InputTemplateInterceptor<T> inputTemplateInterceptor;

        /// <inheritdoc/>
        public DeviceCapability this[ControllerElement virtualElement]
        {
            set
            {
                string optionKey = (from option in this._Options
                                    where option.Value.TargetElement == virtualElement
                                    where option.Value.OptionType.HasFlag(InputOptionType.Keyboard) 
                                        == value.IsKeyboardKey()
                                    where option.Value.OptionType.HasFlag(InputOptionType.ControllerAxes)
                                        == value.IsAxis()
                                    select option.Key).FirstOrDefault();

                if (optionKey == null)
                {
                    throw new KeyNotFoundException(
                        "This template does not support the target element or element type.");
                }

                this.inputTemplateInterceptor.InputValues[optionKey] = value;
            }
        }

        public InputTemplate(IControllerElementMappings mappedElements, int playerIndex = 0)
        {
            this.PlayerIndex = playerIndex;
            ProxyGenerator generator = new ProxyGenerator();
            this._Options = (from prop in typeof(T).GetProperties()
                    let inputOptionAttribute = prop.GetCustomAttribute<InputOptionAttribute>()
                    where inputOptionAttribute != null
                    let name = prop.Name
                    select new KeyValuePair<string, IInputOption>(name, new InputOption(inputOptionAttribute, name)))
                .ToDictionary(o => o.Key,
                    o => o.Value);
            var overrides = (from element in mappedElements
                from key in this._Options.Keys
                let option = this._Options[key]
                let target = option.TargetElement
                where element.LayoutElement == target
                where option.OptionType.HasFlag(InputOptionType.Keyboard) == element.DeviceCapability.IsKeyboardKey()
                where option.OptionType.HasFlag(InputOptionType.ControllerAxes) == element.DeviceCapability.IsAxis()
                select new {key, element.DeviceCapability}).ToDictionary(d => d.key, d => d.DeviceCapability);
            var map = from key in this._Options.Keys
                let value = overrides.ContainsKey(key) ? overrides[key] : DeviceCapability.None
                select new KeyValuePair<string, DeviceCapability>(key, value);
           
            //this.configurationOptions = (from prop in typeof(T).GetProperties()
            //              let configAttribute = prop.GetCustomAttribute<ConfigurationOptionAttribute>()
            //              where configAttribute != null
            //              let name = prop.Name
            //              let metadata = prop.GetCustomAttributes<CustomMetadataAttribute>()
            //              select new ConfigurationOptionDescriptor(configAttribute, metadata, name) as IConfigurationOptionDescriptor).ToList();

            this.ValueCollection = new ConfigurationValueCollection();

            var configDescriptor = new ConfigurationSectionDescriptor<T>(typeof(T).Name);
            ((ConfigurationValueCollection) this.ValueCollection).EnsureSectionDefaults(configDescriptor);
            
            this.inputTemplateInterceptor = new InputTemplateInterceptor<T>(map.ToDictionary(m => m.Key, m => m.Value),
                this.ValueCollection,
                configDescriptor);
            var circular = new InputTemplateCircularInterceptor<T>(this);
            this.Configuration = new InputConfigurationSection<T>(circular, this.inputTemplateInterceptor);
            this.Template = generator.CreateInterfaceProxyWithoutTarget<T>(circular, this.inputTemplateInterceptor);
        }

        /// <inheritdoc/>
        IReadOnlyDictionary<string, IConfigurationValue> IConfigurationSection.Values => this.Configuration.Values;

        /// <inheritdoc/>
        IConfigurationSectionDescriptor IConfigurationSection.Descriptor => this.Configuration.Descriptor;

        /// <inheritdoc/>
        object? IConfigurationSection.this[string key]
        {
            get { return this.Configuration[key]; }
            set { this.Configuration[key] = value; }
        }

        /// <inheritdoc/>
        T IConfigurationSection<T>.Configuration => this.Configuration.Configuration;

        public IConfigurationValueCollection ValueCollection { get; }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<IConfigurationOptionDescriptor, IConfigurationValue>> GetEnumerator()
        {
            return this.Configuration.Descriptor.Options
                .Select(o =>
                    new KeyValuePair<IConfigurationOptionDescriptor, IConfigurationValue>(o, 
                    this.Configuration.Values[o.OptionKey]))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
