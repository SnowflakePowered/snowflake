using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snowflake.Configuration;
using Snowflake.Configuration.Internal;
using Snowflake.Extensibility.Configuration;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Database.Models;

namespace Snowflake.Model.Database
{
    internal partial class PluginConfigurationStore : IPluginConfigurationStore
    {
        #region Asynchronous API
        public async Task SetAsync(Guid valueGuid, object? value)
        {
            await using var context = new DatabaseContext(Options.Options);
            var entity = await context.ConfigurationValues.FindAsync(valueGuid);
            if (entity == null) return;
            bool typeMatches = value switch
            {
                string => entity.ValueType == ConfigurationOptionType.String || entity.ValueType == ConfigurationOptionType.Path,
                bool => entity.ValueType == ConfigurationOptionType.Boolean,
                long => entity.ValueType == ConfigurationOptionType.Integer || entity.ValueType == ConfigurationOptionType.Selection,
                int => entity.ValueType == ConfigurationOptionType.Integer || entity.ValueType == ConfigurationOptionType.Selection,
                short => entity.ValueType == ConfigurationOptionType.Integer || entity.ValueType == ConfigurationOptionType.Selection,
                double => entity.ValueType == ConfigurationOptionType.Decimal,
                float => entity.ValueType == ConfigurationOptionType.Decimal,
                Enum => entity.ValueType == ConfigurationOptionType.Selection,
                _ => false,
            };
            if (!typeMatches) return;
            entity.Value = value.AsConfigurationStringValue();
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task SetAsync(IEnumerable<(Guid valueGuid, object? value)> values)
        {
            await using var context = new DatabaseContext(Options.Options);
            foreach ((Guid valueGuid, object? value) in values)
            {
                var entity = await context.ConfigurationValues.FindAsync(valueGuid);
                if (entity == null) continue;
                entity.Value = value.AsConfigurationStringValue();
                context.Entry(entity).State = EntityState.Modified;
            }

            await context.SaveChangesAsync();
        }

        [GenericTypeAcceptsConfigurationSection(0)]
        public async Task<IConfigurationSection<T>> GetAsync<T>()
            where T : class
        {
            await using var context = new DatabaseContext(Options.Options);
            var entity = await context.ConfigurationProfiles
                .Include(p => p.Values)
                .SingleOrDefaultAsync(p => p.ConfigurationSource == $"plugin:{typeof(T).Name}");

            if (entity != null) return entity.AsConfigurationSection<T>();

            var defaults = new ConfigurationSection<T>
                (new ConfigurationValueCollection(), typeof(T).Name);
            await context.ConfigurationProfiles.AddAsync(defaults.AsModel($"plugin:{typeof(T).Name}"));
            await context.SaveChangesAsync();
            return defaults;
        }

        public async Task SetAsync<T>(IConfigurationSection<T> configuration)
            where T : class
        {
            await using var context = new DatabaseContext(Options.Options);
            var entity = await context.ConfigurationProfiles
                .Include(p => p.Values)
                .SingleOrDefaultAsync(p => p.ConfigurationSource == $"plugin:{typeof(T).Name}");

            if (entity == null)
            {
                var defaults = new ConfigurationSection<T>
                    (new ConfigurationValueCollection(), typeof(T).Name);
                await context.ConfigurationProfiles.AddAsync(defaults.AsModel($"plugin:{typeof(T).Name}"));
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

            await context.SaveChangesAsync();
        }
        #endregion
    }
}
