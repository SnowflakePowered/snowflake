using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;

namespace Snowflake.JsonConverters
{
    internal class ConfigurationCollectionSerializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IConfigurationCollection collection = (IConfigurationCollection) value;
            JObject root = new JObject();
            
            foreach(var section in collection)
            {
                JObject sectionRoot = new JObject();
                sectionRoot.Add("Values", new JObject(section.Value.Values.Select(v => new JProperty(v.Key,JToken.FromObject(v.Value)))));
                sectionRoot.Add("Options", new JObject(section.Value.Descriptor.Options
                    .Where(o => !o.Private)
                    .Select(o => new JProperty(o.KeyName, ConfigurationCollectionSerializer.SerializeOption(o)))));
                sectionRoot.Add("Selections", new JObject(section.Value
                    .Where(o => o.Key.Type.IsEnum)
                    .Select(o => ConfigurationCollectionSerializer.SerializeEnumValues(o.Key.Type))));
                root.Add(section.Key, sectionRoot);
            }
           
            root.WriteTo(writer, new StringEnumConverter());
        }

        private static IEnumerable<JProperty> SerializeEnumValues(Type selectionEnum)
        {
            return from enumOption in selectionEnum.GetFields()
                where enumOption.HasAttribute<SelectionOptionAttribute>()
                let attribute = enumOption.GetCustomAttribute<SelectionOptionAttribute>()
                select new JProperty(enumOption.Name, new JObject()
                {
                    {nameof(attribute.DisplayName), attribute.DisplayName ?? enumOption.Name},
                    {nameof(attribute.Private), attribute.Private}
                });
        }
        private static JObject SerializeOption(IConfigurationOption o)
        {
            var optionRoot = new JObject
            {
                {nameof(o.Default), JToken.FromObject(o.Default)},
                {nameof(o.DisplayName), o.DisplayName != String.Empty ? o.DisplayName : o.KeyName},
                {nameof(o.Description), o.Description},
                {nameof(o.Simple), o.Simple},
                {nameof(o.CustomMetadata), JToken.FromObject(o.CustomMetadata)},
                {nameof(Type), ConfigurationCollectionSerializer.GetTypeString(o)}
            };

            if (o.Type == typeof(int))
            {
                optionRoot.Add(nameof(o.Min), (int)o.Min);
                optionRoot.Add(nameof(o.Max), (int)o.Max);
                optionRoot.Add(nameof(o.Increment), (int)o.Increment);
            }
            if (o.Type == typeof(double))
            {
                optionRoot.Add(nameof(o.Min), o.Min);
                optionRoot.Add(nameof(o.Max), o.Max);
                optionRoot.Add(nameof(o.Increment), o.Increment);
            }
            return optionRoot;
        }

        private static string GetTypeString(IConfigurationOption o)
        {
            if (o.Type == typeof(int)) return "integer";
            if (o.Type == typeof(bool)) return "boolean";
            if (o.Type == typeof(double)) return "decimal";
            if (o.Type.IsEnum) return "selection";
            if (o.IsPath) return "path";
            return "string";
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IConfigurationCollection).IsAssignableFrom(objectType);
        }
    }
}
