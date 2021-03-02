using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snowflake.Configuration;
using Snowflake.Configuration.Generators;
using Snowflake.Configuration.Internal;
using Snowflake.Extensibility.Configuration;
using Snowflake.Model.Database.Exceptions;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Database.Models;

namespace Snowflake.Model.Database
{
    internal partial class PluginConfigurationStore : IPluginConfigurationStore
    {
        private DbContextOptionsBuilder<DatabaseContext> Options { get; set; }

        public PluginConfigurationStore(DbContextOptionsBuilder<DatabaseContext> options)
        {
            this.Options = options;
            using var context = new DatabaseContext(Options.Options);
            context.Database.Migrate();
        }

        #region Synchronous API
        public void Set(Guid valueGuid, object? value)
        {
            using var context = new DatabaseContext(Options.Options);
            var entity = context.ConfigurationValues.Find(valueGuid);
            if (entity == null) return;
            bool typeMatches = value switch
            {
                string _ => entity.ValueType == ConfigurationOptionType.String || entity.ValueType == ConfigurationOptionType.Path,
                bool _ => entity.ValueType == ConfigurationOptionType.Boolean,
                long _ => entity.ValueType == ConfigurationOptionType.Integer || entity.ValueType == ConfigurationOptionType.Selection,
                int _ => entity.ValueType == ConfigurationOptionType.Integer || entity.ValueType == ConfigurationOptionType.Selection,
                short _ => entity.ValueType == ConfigurationOptionType.Integer || entity.ValueType == ConfigurationOptionType.Selection,
                double _ => entity.ValueType == ConfigurationOptionType.Decimal,
                float _ => entity.ValueType == ConfigurationOptionType.Decimal,
                Enum _ => entity.ValueType == ConfigurationOptionType.Selection,
                _ => false,
            };
            if (!typeMatches) return;
            entity.Value = value.AsConfigurationStringValue();
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Set(IEnumerable<(Guid valueGuid, object? value)> values)
        {
            using var context = new DatabaseContext(Options.Options);
            foreach ((Guid valueGuid, object? value) in values)
            {
                var entity = context.ConfigurationValues.Find(valueGuid);
                if (entity == null) continue;
                bool typeMatches = value switch
                {
                    string _ => entity.ValueType == ConfigurationOptionType.String || entity.ValueType == ConfigurationOptionType.Path,
                    bool _ => entity.ValueType == ConfigurationOptionType.Boolean,
                    long _ => entity.ValueType == ConfigurationOptionType.Integer || entity.ValueType == ConfigurationOptionType.Selection,
                    int _ => entity.ValueType == ConfigurationOptionType.Integer || entity.ValueType == ConfigurationOptionType.Selection,
                    short _ => entity.ValueType == ConfigurationOptionType.Integer || entity.ValueType == ConfigurationOptionType.Selection,
                    double _ => entity.ValueType == ConfigurationOptionType.Decimal,
                    float _ => entity.ValueType == ConfigurationOptionType.Decimal,
                    Enum _ => entity.ValueType == ConfigurationOptionType.Selection,
                    _ => false,
                };
                if (!typeMatches) return;
                entity.Value = value.AsConfigurationStringValue();
                context.Entry(entity).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        [GenericTypeAcceptsConfigurationSection(0)]
        public IConfigurationSection<T> Get<T>()
            where T : class
        {
            using var context = new DatabaseContext(Options.Options);
            var entity = context.ConfigurationProfiles
                .Include(p => p.Values)
                .SingleOrDefault(p => p.ConfigurationSource == $"plugin:{typeof(T).Name}");

            if (entity != null) return entity.AsConfigurationSection<T>();

            var defaults = new ConfigurationSection<T>
                (new ConfigurationValueCollection(), typeof(T).Name);
            context.ConfigurationProfiles.Add(defaults.AsModel($"plugin:{typeof(T).Name}"));
            context.SaveChanges();
            return defaults;
        }

        [GenericTypeAcceptsConfigurationSection(0)]
        public void Set<T>(IConfigurationSection<T> configuration)
            where T : class
        {
            using var context = new DatabaseContext(Options.Options);
            var entity = context.ConfigurationProfiles
                .Include(p => p.Values)
                .SingleOrDefault(p => p.ConfigurationSource == $"plugin:{typeof(T).Name}");

            if (entity == null)
            {
                var defaults = new ConfigurationSection<T>
                    (new ConfigurationValueCollection(), typeof(T).Name);
                context.ConfigurationProfiles.Add(defaults.AsModel($"plugin:{typeof(T).Name}"));
            }
            else
            {
                foreach (var value in entity.Values)
                {
                    string? newValue = configuration
                        .Values[value.OptionKey].Value.AsConfigurationStringValue();
                    if (newValue != value.Value)
                    {
                        value.Value = newValue;
                        context.Entry(value).State = EntityState.Modified;
                    }
                }
            }

            context.SaveChanges();
        }
        #endregion
    }
}
