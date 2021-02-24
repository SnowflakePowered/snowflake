using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snowflake.Configuration;
using Snowflake.Configuration.Generators;
using Snowflake.Model.Database.Exceptions;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Database.Models;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Database
{
    internal partial class ConfigurationCollectionStore
    {
        public async Task<IConfigurationCollection<T>> CreateConfigurationAsync<T>(string sourceName)
            where T : class, IConfigurationCollectionTemplate
        {
            var collection = new ConfigurationCollection<T>();

            await using var context = new DatabaseContext(this.Options.Options);

            await context.ConfigurationProfiles.AddAsync(collection.AsModel(sourceName));
            await context.SaveChangesAsync();
            
            return collection;
        }

        public async Task<IConfigurationCollection<T>> CreateConfigurationForGameAsync<T>(IGameRecord gameRecord,
            string sourceName, string profileName)
            where T : class, IConfigurationCollectionTemplate
        {
            var collection = new ConfigurationCollection<T>();

            await using var context = new DatabaseContext(this.Options.Options);

            var gameEntity = await context.GameRecords
                .Include(p => p.ConfigurationProfiles)
                .SingleOrDefaultAsync(p => p.RecordID == gameRecord.RecordID);

            if (gameEntity == null) throw new DependentEntityNotExistsException(gameRecord.RecordID);

            var entity = await context.ConfigurationProfiles
                .AddAsync(collection.AsModel(sourceName));

            gameEntity.ConfigurationProfiles.Add(new GameRecordConfigurationProfileModel
            {
                ProfileName = profileName,
                ConfigurationSource = sourceName,
                Profile = entity.Entity
            });

            await context.SaveChangesAsync();
            
            return collection;
        }

        public async Task DeleteConfigurationAsync(Guid configurationValueCollectionGuid)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var profile = await context.ConfigurationProfiles.FindAsync(configurationValueCollectionGuid);
            if (profile == null) return;

            context.Entry(profile).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }

        public async Task DeleteConfigurationForGameAsync(Guid gameGuid, string sourceName, Guid collectionGuid)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var profileJunction = await context.GameRecordsConfigurationProfiles
                .Include(p => p.Profile)
                .SingleOrDefaultAsync(g => g.GameID == gameGuid
                                      && g.ConfigurationSource == sourceName
                                      && g.ProfileID == collectionGuid);
            if (profileJunction == null) return;

            context.Entry(profileJunction).State = EntityState.Deleted;
            context.Entry(profileJunction.Profile).State = EntityState.Deleted;

            await context.SaveChangesAsync();
        }

        public async Task UpdateConfigurationAsync(IConfigurationCollection configurationCollection)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var guid = configurationCollection.ValueCollection.Guid;
            var config = await context.ConfigurationProfiles.FindAsync(guid);
            if (config == null) return;

            foreach
            (var value in
                context.ConfigurationValues.Where(v => v.ValueCollectionGuid == guid))
            {
                var realValue = configurationCollection.GetSection(value.SectionKey)?[value.OptionKey]
                    ?.AsConfigurationStringValue();
                if (realValue == value.Value || realValue == null) continue;
                value.Value = realValue;
                context.Entry(value).State = EntityState.Modified;
            }
            var valueGuids = configurationCollection.ValueCollection.Select(v => v.value.Guid).ToList();

            var alreadyExists = (await context.ConfigurationValues.Where(g => valueGuids.Contains(g.Guid))
                .Select(g => g.Guid)
                .ToListAsync())
                .ToHashSet();

            var newValues = valueGuids.Except(alreadyExists)
                .Select(g =>
                {
                    (string? section, string? option, IConfigurationValue? configurationValue) = configurationCollection.ValueCollection[g];
                    // Values must not be null because g is from the value collection's GUIDs.
                    return new ConfigurationValueModel
                    {
                        SectionKey = section!,
                        OptionKey = option!,
                        Guid = configurationValue!.Guid,
                        Value = configurationValue!.Value.AsConfigurationStringValue(),
                        ValueCollectionGuid = configurationCollection.ValueCollection.Guid
                    };
                });

            await context.AddRangeAsync(newValues);
            await context.SaveChangesAsync();
        }

        public async Task<IConfigurationCollection<T>?> GetConfigurationAsync<T>(Guid valueCollectionGuid)
            where T : class, IConfigurationCollectionTemplate
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var config = await context.ConfigurationProfiles
                .Include(c => c.Values)
                .SingleOrDefaultAsync(c => c.ValueCollectionGuid == valueCollectionGuid);
            return config?.AsConfiguration<T>();
        }

        public async Task<IConfigurationCollection<T>?> GetConfigurationAsync<T>(Guid gameGuid,
            string sourceName, Guid valueCollectionGuid)
            where T : class, IConfigurationCollectionTemplate
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var profileJunction = await context.GameRecordsConfigurationProfiles
                .Include(g => g.Profile)
                .ThenInclude(g => g.Values)
                .SingleOrDefaultAsync(g => g.GameID == gameGuid
                                      && g.ConfigurationSource == sourceName
                                      && g.ProfileID == valueCollectionGuid);
            if (profileJunction == null) return null;

            var profile = await context.ConfigurationProfiles
                .SingleOrDefaultAsync(s => s.ValueCollectionGuid == profileJunction.ProfileID);

            return profile?.AsConfiguration<T>();
        }

        public async Task<IConfigurationValue> GetValueAsync(Guid valueGuid)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var entityValue = await context.ConfigurationValues.FindAsync(valueGuid);
            return entityValue.AsConfigurationValue();
        }

        public Task UpdateValueAsync(IConfigurationValue value) => this.UpdateValueAsync(value.Guid, value.Value);

        public async Task UpdateValueAsync(Guid valueGuid, object? value)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var entityValue = await context.ConfigurationValues.FindAsync(valueGuid);
            if (entityValue == null) return;
            bool typeMatches = value switch
            {
                string str => entityValue.ValueType == ConfigurationOptionType.String 
                    || entityValue.ValueType == ConfigurationOptionType.Path
                    || Guid.TryParse(str, out var _) && entityValue.ValueType == ConfigurationOptionType.Resource,
                bool _ => entityValue.ValueType == ConfigurationOptionType.Boolean,
                long _ => entityValue.ValueType == ConfigurationOptionType.Integer || entityValue.ValueType == ConfigurationOptionType.Selection,
                int _ => entityValue.ValueType == ConfigurationOptionType.Integer || entityValue.ValueType == ConfigurationOptionType.Selection,
                short _ => entityValue.ValueType == ConfigurationOptionType.Integer || entityValue.ValueType == ConfigurationOptionType.Selection,
                double _ => entityValue.ValueType == ConfigurationOptionType.Decimal,
                float _ => entityValue.ValueType == ConfigurationOptionType.Decimal,
                Guid _ => entityValue.ValueType == ConfigurationOptionType.Resource,
                Enum _ => entityValue.ValueType == ConfigurationOptionType.Selection,
                _ => false,
            };
            if (!typeMatches) return;
            entityValue.Value = value.AsConfigurationStringValue();
            context.Entry(entityValue).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<Guid> GetOwningValueCollectionAsync(Guid valueGuid)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var entityValue = await context.ConfigurationValues.FindAsync(valueGuid);
            if (entityValue == null) return default;
            return entityValue.ValueCollectionGuid;
        }
    }
}