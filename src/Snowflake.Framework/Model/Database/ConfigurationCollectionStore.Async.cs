using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snowflake.Configuration;
using Snowflake.Model.Database.Exceptions;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Database.Models;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Database
{
    internal partial class ConfigurationCollectionStore
    {
        public async Task<IConfigurationCollection<T>> CreateConfigurationAsync<T>(string sourceName)
            where T : class, IConfigurationCollection<T>
        {
            var collection = new ConfigurationCollection<T>();

            await using var context = new DatabaseContext(this.Options.Options);

            await context.ConfigurationProfiles.AddAsync(collection.AsModel(sourceName));
            await context.SaveChangesAsync();
            
            return collection;
        }

        public async Task<IConfigurationCollection<T>> CreateConfigurationForGameAsync<T>(IGameRecord gameRecord,
            string sourceName, string profileName)
            where T : class, IConfigurationCollection<T>
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

        public async Task DeleteConfigurationForGameAsync(Guid gameGuid, string sourceName, string profileName)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var profileJunction = await context.GameRecordsConfigurationProfiles
                .Include(p => p.Profile)
                .SingleOrDefaultAsync(g => g.GameID == gameGuid
                                      && g.ConfigurationSource == sourceName
                                      && g.ProfileName == profileName);
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
                var realValue = configurationCollection[value.SectionKey][value.OptionKey]
                    ?.AsConfigurationStringValue();
                if (realValue == value.Value || realValue == null) continue;
                value.Value = realValue;
                context.Entry(value).State = EntityState.Modified;
            }

            foreach ((string section, string option, IConfigurationValue configurationValue) in configurationCollection.ValueCollection)
            {
                var value = await context.ConfigurationValues.FindAsync(configurationValue.Guid);
                if (value != null) continue;

               await context.ConfigurationValues.AddAsync(new ConfigurationValueModel
                {
                    SectionKey = section,
                    OptionKey = option,
                    Guid = configurationValue.Guid,
                    Value = configurationValue.Value.AsConfigurationStringValue(),
                    ValueCollectionGuid = configurationCollection.ValueCollection.Guid
                });
            }

            await context.SaveChangesAsync();
        }

        public async Task<IConfigurationCollection<T>?> GetConfigurationAsync<T>(Guid valueCollectionGuid)
            where T : class, IConfigurationCollection<T>
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var config = await context.ConfigurationProfiles
                .Include(c => c.Values)
                .SingleOrDefaultAsync(c => c.ValueCollectionGuid == valueCollectionGuid);
            return config?.AsConfiguration<T>();
        }

        public async Task<IConfigurationCollection<T>?> GetConfigurationAsync<T>(Guid gameGuid,
            string sourceName, string profileName)
            where T : class, IConfigurationCollection<T>
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var profileJunction = await context.GameRecordsConfigurationProfiles
                .Include(g => g.Profile)
                .ThenInclude(g => g.Values)
                .SingleOrDefaultAsync(g => g.GameID == gameGuid
                                      && g.ConfigurationSource == sourceName
                                      && g.ProfileName == profileName);
            if (profileJunction == null) return null;

            var profile = await context.ConfigurationProfiles
                .SingleOrDefaultAsync(s => s.ValueCollectionGuid == profileJunction.ProfileID);

            return profile?.AsConfiguration<T>();
        }

        public Task UpdateValueAsync(IConfigurationValue value) => this.UpdateValueAsync(value.Guid, value.Value);

        public async Task UpdateValueAsync(Guid valueGuid, object? value)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var entityValue = await context.ConfigurationValues.FindAsync(valueGuid);
            if (entityValue == null) return;
            entityValue.Value = value.AsConfigurationStringValue();
            context.Entry(entityValue).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}