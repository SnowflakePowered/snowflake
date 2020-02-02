using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snowflake.Configuration;
using Snowflake.Extensibility.Configuration;
using Snowflake.Model.Database.Exceptions;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Database.Models;

namespace Snowflake.Model.Database
{
    internal class PluginConfigurationStore : IPluginConfigurationStore
    {
        private DbContextOptionsBuilder<DatabaseContext> Options { get; set; }

        public PluginConfigurationStore(DbContextOptionsBuilder<DatabaseContext> options)
        {
            this.Options = options;
            using var context = new DatabaseContext(Options.Options);
            context.Database.Migrate();
        }

        #region Synchronous API
        public void Set(IConfigurationValue value)
        {
            using var context = new DatabaseContext(Options.Options);
            var entity = context.ConfigurationValues
                .SingleOrDefault(v => v.Guid == value.Guid);
            if (entity == null) return;
            entity.Value = value.Value.AsConfigurationStringValue();
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Set(IEnumerable<IConfigurationValue> values)
        {
            using var context = new DatabaseContext(Options.Options);
            foreach (var value in values)
            {
                var entity = context.ConfigurationValues.Find(value.Guid);
                if (entity == null) continue;
                entity.Value = value.Value.AsConfigurationStringValue();
                context.Entry(entity).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public IConfigurationSection<T> Get<T>()
            where T : class, IConfigurationSection<T>
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

        public void Set<T>(IConfigurationSection<T> configuration)
            where T : class, IConfigurationSection<T>
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

        #region Asynchronous API
        public async Task SetAsync(IConfigurationValue value)
        {
            await using var context = new DatabaseContext(Options.Options);
            var entity = await context.ConfigurationValues
                .SingleOrDefaultAsync(v => v.Guid == value.Guid);
            if (entity == null) return;
            entity.Value = value.Value.AsConfigurationStringValue();
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task SetAsync(IEnumerable<IConfigurationValue> values)
        {
            await using var context = new DatabaseContext(Options.Options);
            foreach (var value in values)
            {
                var entity = await context.ConfigurationValues.FindAsync(value.Guid);
                if (entity == null) continue;
                entity.Value = value.Value.AsConfigurationStringValue();
                context.Entry(entity).State = EntityState.Modified;
            }

            await context.SaveChangesAsync();
        }

        public async Task<IConfigurationSection<T>> GetAsync<T>()
            where T : class, IConfigurationSection<T>
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
            where T : class, IConfigurationSection<T>
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
