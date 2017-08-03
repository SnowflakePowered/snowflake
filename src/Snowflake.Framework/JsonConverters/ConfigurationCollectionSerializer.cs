using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EnumsNET.NonGeneric;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;

namespace Snowflake.JsonConverters
{
    internal class ConfigurationCollectionSerializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IConfigurationCollection collection = (IConfigurationCollection) value;
            JObject collectionRoot = new JObject();
            foreach(var section in collection.Select(s => s.Value))
            {
                JObject sectionOptionsRoot = new JObject();
                foreach(var option in section)
                {
                   JObject optionRoot = new JObject()
                   {
                       {"Value", JObject.FromObject(option.Value)},
                       {"Descriptor", ConfigurationCollectionSerializer.SerializeOption(option.Key)}
                   };
                   if (option.Key.Type.GetTypeInfo().IsEnum)
                   {
                        optionRoot.Add("Selection", new JObject(ConfigurationCollectionSerializer.SerializeEnumValues(option.Key.Type)));
                   }
                    sectionOptionsRoot.Add(option.Key.KeyName, optionRoot);
                }

                JObject sectionRoot = new JObject
                {
                    { "Configuration", sectionOptionsRoot },
                    { "Descriptor", ConfigurationCollectionSerializer.SerializeSectionDescriptor(section.Descriptor) }
                };

                collectionRoot.Add(section.Descriptor.SectionName, sectionRoot);
            }
           
            collectionRoot.WriteTo(writer, new StringEnumConverter());
        }

        private static JObject SerializeSectionDescriptor(IConfigurationSectionDescriptor d)
        {
            var descriptorRoot = new JObject
            {
                {nameof(d.Description), d.Description},
                {nameof(d.DisplayName), d.DisplayName},
                {nameof(d.SectionName), d.SectionName }
            };
            return descriptorRoot;
        }
        private static IEnumerable<JProperty> SerializeEnumValues(Type selectionEnum)
        {


            return from enumOption in NonGenericEnums.GetMembers(selectionEnum)
                where enumOption.Attributes.Has<SelectionOptionAttribute>()
                let attribute = enumOption.Attributes.Get<SelectionOptionAttribute>()
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
            if (o.Type.GetTypeInfo().IsEnum) return "selection";
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
