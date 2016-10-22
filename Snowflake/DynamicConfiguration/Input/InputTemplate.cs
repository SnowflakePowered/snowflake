using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using Snowflake.Configuration.Input;
using Snowflake.DynamicConfiguration.Attributes;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;

namespace Snowflake.DynamicConfiguration.Input
{
    public class InputTemplate<T> : IConfigurationSection<T> where T : class, IConfigurationSection<T>
    {
        private IConfigurationSection<T> Configuration { get; }
        public int PlayerIndex { get; }
        public T Template { get; }

        public IDictionary<string, ControllerElement> Values
            => ImmutableDictionary.CreateRange(this.inputTemplateInterceptor.Values);
        private IDictionary<string, InputOption> Options { get; }
        private readonly InputTemplateInterceptor<T> inputTemplateInterceptor;

        public ControllerElement this[ControllerElement virtualElement]
        {
            set
            {
                string optionKey = (from option in this.Options
                                    where option.Value.TargetElement == virtualElement
                                    where option.Value.InputOptionType.HasFlag(InputOptionType.Keyboard) == value.IsKeyboardKey()
                                    where option.Value.InputOptionType.HasFlag(InputOptionType.ControllerAxes) == value.IsAxis()
                                    select option.Key).FirstOrDefault();
                if (optionKey == null) throw new KeyNotFoundException("This template does not support the target element or element type.");
                this.inputTemplateInterceptor.Values[optionKey] = value;
            }
        }
        public InputTemplate(IMappedControllerElementCollection mappedElements)
        {
            ProxyGenerator generator = new ProxyGenerator();
            var attr = typeof(T).GetCustomAttribute<InputTemplateAttribute>();
           
            this.Options = (from prop in typeof(T).GetProperties()
                where prop.HasAttribute<InputOptionAttribute>()
                let name = prop.Name
                let inputOptionAttribute = prop.GetCustomAttribute<InputOptionAttribute>()
                select new KeyValuePair<string, InputOption>(name, new InputOption(inputOptionAttribute))).ToDictionary(o => o.Key,
                    o => o.Value);
            var overrides = (from element in mappedElements
                from key in this.Options.Keys
                let option = this.Options[key]
                let target = option.TargetElement
                where element.LayoutElement == target
                where option.InputOptionType.HasFlag(InputOptionType.Keyboard) == element.DeviceElement.IsKeyboardKey()
                where option.InputOptionType.HasFlag(InputOptionType.ControllerAxes) == element.DeviceElement.IsAxis()
                select new {key, element.DeviceElement}).ToDictionary(d => d.key, d => d.DeviceElement);
            var map = from key in this.Options.Keys
                let value = overrides.ContainsKey(key) ? overrides[key] : ControllerElement.NoElement
                select new KeyValuePair<string, ControllerElement>(key, value);
            this.inputTemplateInterceptor = new InputTemplateInterceptor<T>(map.ToDictionary(m => m.Key, m => m.Value));
            this.Configuration = new ConfigurationSection<T>(this.inputTemplateInterceptor, attr.Destination, attr.SectionName, attr.DisplayName);
            this.Template = generator.CreateInterfaceProxyWithoutTarget<T>(new ConfigurationCircularInterceptor<T>(this), this.inputTemplateInterceptor);

        }

        IList<IConfigurationOption> IConfigurationSection.Options => this.Configuration.Options;

        string IConfigurationSection.Destination => this.Configuration.Destination;

        string IConfigurationSection.Description => this.Configuration.Description;

        string IConfigurationSection.DisplayName => this.Configuration.DisplayName;

        string IConfigurationSection.SectionName => this.Configuration.SectionName;

        IDictionary<string, IConfigurationValue> IConfigurationSection.Values => this.Configuration.Values;

        object IConfigurationSection.this[string key]
        {
            get { return this.Configuration[key]; }
            set { this.Configuration[key] = value; }
        }

        T IConfigurationSection<T>.Configuration => this.Configuration.Configuration;
    }

    internal class InputTemplateInterceptor<T> : IInterceptor
    {
        internal InputTemplateInterceptor(IDictionary<string, ControllerElement> values)
        {
            this.Values = values;
        }

        internal IDictionary<string, ControllerElement> Values;
        public void Intercept(IInvocation invocation)
        {
            var propertyName = invocation.Method.Name.Substring(4); // remove get_ or set_
            if (!this.Values.ContainsKey(propertyName))
            {
                invocation.Proceed();
            }
            else
            {
                if (invocation.Method.Name.StartsWith("get_"))
                {
                    invocation.ReturnValue = Values[propertyName]; //type is IConfigurationSection<T>
                }
            }
        }
    }
}
