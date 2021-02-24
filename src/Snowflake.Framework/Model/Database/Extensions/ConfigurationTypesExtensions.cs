using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Configuration.Generators;
using Snowflake.Model.Database.Models;

namespace Snowflake.Model.Database.Extensions
{
    internal static class ConfigurationTypesExtensions
    {
        public static ConfigurationProfileModel AsModel<T>
            (this IConfigurationCollection<T> @this, string prototypeName)
            where T : class
        {
            return new ConfigurationProfileModel
            {
                ValueCollectionGuid = @this.ValueCollection.Guid,
                ConfigurationSource = prototypeName,
                Values = @this.ValueCollection.AsModel()
            };
        }

        public static ConfigurationProfileModel AsModel<T>
            (this IConfigurationSection<T> @this, string prototypeName)
            where T : class
        {
            return new ConfigurationProfileModel
            {
                ValueCollectionGuid = @this.ValueCollection.Guid,
                ConfigurationSource = prototypeName,
                Values = @this.ValueCollection.AsModel()
            };
        }

        internal static string? AsConfigurationStringValue(this object? @this)
        {
            if (@this == null) return null;

            return @this.GetType().GetTypeInfo().IsEnum
                ? Convert.ToString((int)@this)
                : // optimized path for enums
                Convert.ToString(@this);
        }

        internal static IConfigurationValue AsConfigurationValue(this ConfigurationValueModel @this)
        {
            Type type = @this.ValueType switch
            {
                ConfigurationOptionType.Boolean => typeof(bool),
                ConfigurationOptionType.String => typeof(string),
                ConfigurationOptionType.Path => typeof(string),
                ConfigurationOptionType.Integer => typeof(long),
                ConfigurationOptionType.Decimal => typeof(double),
                ConfigurationOptionType.Selection => typeof(int),
                ConfigurationOptionType.Resource => typeof(Guid),
                _ => throw new NotImplementedException(),
            };
            return new ConfigurationValue(ConfigurationValueCollection.FromString(@this.Value, type), @this.Guid, @this.ValueType);
        }

        public static List<ConfigurationValueModel> AsModel(this IConfigurationValueCollection @this)
        {
            return (from t in @this
                select new ConfigurationValueModel
                {
                    SectionKey = t.section,
                    OptionKey = t.option,
                    Guid = t.value.Guid,
                    ValueType = t.value.Type,
                    Value = t.value.Value.AsConfigurationStringValue()
                }).ToList();
        }

        public static IConfigurationCollection<T> AsConfiguration<T>(this ConfigurationProfileModel model)
            where T : class, IConfigurationCollectionTemplate
        {
            var values = model.Values.Select(v => (v.SectionKey, v.OptionKey, (v.Value, v.Guid, v.ValueType)));
            var valueCollection = ConfigurationValueCollection.MakeExistingValueCollection<T>
                (values, model.ValueCollectionGuid);
            return new ConfigurationCollection<T>(valueCollection);
        }

        public static IConfigurationSection<T> AsConfigurationSection<T>(this ConfigurationProfileModel model)
            where T : class
        {
            var sectionKey = model.Values.First().SectionKey;
            var values = model.Values.Select(v => (v.OptionKey, (v.Value, v.Guid, v.ValueType)));
            var valueCollection = ConfigurationValueCollection.MakeExistingValueCollection<T>
                (values, sectionKey, model.ValueCollectionGuid);
            return new ConfigurationSection<T>(valueCollection, sectionKey);
        }
    }
}
