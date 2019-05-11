using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EnumsNET.NonGeneric;
using Snowflake.Configuration;
using Snowflake.Model.Database.Models;

namespace Snowflake.Model.Database.Extensions
{
    internal static class ConfigurationTypesExtensions
    {
        public static ConfigurationProfileModel AsModel<T>
            (this IConfigurationCollection<T> @this, string prototypeName)
            where T : class, IConfigurationCollection<T>
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
            where T : class, IConfigurationSection<T>
        {
            return new ConfigurationProfileModel
            {
                ValueCollectionGuid = @this.ValueCollection.Guid,
                ConfigurationSource = prototypeName,
                Values = @this.ValueCollection.AsModel()
            };
        }

        public static string? AsConfigurationStringValue(this object? @this)
        {
            if (@this == null) return null;

            return @this.GetType().GetTypeInfo().IsEnum
                ? NonGenericEnums.GetName(@this.GetType(), @this)
                : // optimized path for enums
                Convert.ToString(@this);
        }

        public static List<ConfigurationValueModel> AsModel(this IConfigurationValueCollection @this)
        {
            return (from t in @this
                select new ConfigurationValueModel
                {
                    SectionKey = t.section,
                    OptionKey = t.option,
                    Guid = t.value.Guid,
                    Value = t.value.Value.AsConfigurationStringValue()
                }).ToList();
        }

        public static IConfigurationCollection<T> AsConfiguration<T>(this ConfigurationProfileModel model)
            where T : class, IConfigurationCollection<T>
        {
            var values = model.Values.Select(v => (v.SectionKey, v.OptionKey, (v.Value, v.Guid)));
            var valueCollection = ConfigurationValueCollection.MakeExistingValueCollection<T>
                (values, model.ValueCollectionGuid);
            return new ConfigurationCollection<T>(valueCollection);
        }

        public static IConfigurationSection<T> AsConfigurationSection<T>(this ConfigurationProfileModel model)
            where T : class, IConfigurationSection<T>
        {
            var sectionKey = model.Values.First().SectionKey;
            var values = model.Values.Select(v => (v.OptionKey, (v.Value, v.Guid)));
            var valueCollection = ConfigurationValueCollection.MakeExistingValueCollection<T>
                (values, sectionKey, model.ValueCollectionGuid);
            return new ConfigurationSection<T>(valueCollection, sectionKey);
        }
    }
}
