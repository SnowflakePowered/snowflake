using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Configuration;
using Snowflake.Configuration.Internal;
using Snowflake.Model.Database.Exceptions;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Database.Models;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Database
{
    internal partial class ConfigurationCollectionStore
    {
        private DbContextOptionsBuilder<DatabaseContext> Options { get; set; }

        public ConfigurationCollectionStore(DbContextOptionsBuilder<DatabaseContext> options)
        {
            this.Options = options;
            using var context = new DatabaseContext(this.Options.Options);
            context.Database.Migrate();
        }

        public IEnumerable<IGrouping<string, (string profileName, Guid collectionGuid)>>
            GetProfileNames(IGameRecord gameRecord)
        {
            using var context = new DatabaseContext(this.Options.Options);
            return context.GameRecords
                .Include(g => g.ConfigurationProfiles)
                .SingleOrDefault(g => g.RecordID == gameRecord.RecordID)?
                .ConfigurationProfiles
                .GroupBy(p => p.ConfigurationSource, p => (p.ProfileName, p.ProfileID)) ??
                Enumerable.Empty<IGrouping<string, (string, Guid)>>();
        }

        [GenericTypeAcceptsConfigurationCollection(0)]
        public IConfigurationCollection<T> CreateConfiguration<T>(string sourceName)
            where T : class, IConfigurationCollectionTemplate
        {
            var collection = new ConfigurationCollection<T>();

            using var context = new DatabaseContext(this.Options.Options);

            context.ConfigurationProfiles.Add(collection.AsModel(sourceName));
            context.SaveChanges();
            
            return collection;
        }

        [GenericTypeAcceptsConfigurationCollection(0)]
        public IConfigurationCollection<T> CreateConfigurationForGame<T>(IGameRecord gameRecord,
            string sourceName, string profileName)
            where T : class, IConfigurationCollectionTemplate
        {
            var collection = new ConfigurationCollection<T>();

            using var context = new DatabaseContext(this.Options.Options);

            var gameEntity = context.GameRecords
                .Include(p => p.ConfigurationProfiles)
                .SingleOrDefault(p => p.RecordID == gameRecord.RecordID);

            if (gameEntity == null) throw new DependentEntityNotExistsException(gameRecord.RecordID);

            var entity = context.ConfigurationProfiles
                .Add(collection.AsModel(sourceName));

            gameEntity.ConfigurationProfiles.Add(new GameRecordConfigurationProfileModel
            {
                ProfileName = profileName,
                ConfigurationSource = sourceName,
                Profile = entity.Entity
            });

            context.SaveChanges();
            
            return collection;
        }

        public IConfigurationValue GetValue(Guid valueGuid)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var entityValue = context.ConfigurationValues.Find(valueGuid);
            return entityValue.AsConfigurationValue();
        }

        public void DeleteConfiguration(Guid configurationValueCollectionGuid)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var profile = context.ConfigurationProfiles.Find(configurationValueCollectionGuid);
            if (profile == null) return;

            context.Entry(profile).State = EntityState.Deleted;
            context.SaveChanges();
        }

        public void DeleteConfigurationForGame(Guid gameGuid, string sourceName, Guid collectionGuid)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var profileJunction = context.GameRecordsConfigurationProfiles
                .Include(p => p.Profile)
                .SingleOrDefault(g => g.GameID == gameGuid
                                      && g.ConfigurationSource == sourceName
                                      && g.ProfileID == collectionGuid);
            if (profileJunction == null) return;

            context.Entry(profileJunction).State = EntityState.Deleted;
            context.Entry(profileJunction.Profile).State = EntityState.Deleted;

            context.SaveChanges();
        }

        public void UpdateConfiguration(IConfigurationCollection configurationCollection)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var guid = configurationCollection.ValueCollection.Guid;
            var config = context.ConfigurationProfiles?
                .Find(guid);
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

            var alreadyExists = context.ConfigurationValues.Where(g => valueGuids.Contains(g.Guid))
                .Select(g => g.Guid)
                .AsEnumerable()
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

            context.ConfigurationValues.AddRange(newValues);
            context.SaveChanges();
        }

        [GenericTypeAcceptsConfigurationCollection(0)]
        public IConfigurationCollection<T>? GetConfiguration<T>(Guid valueCollectionGuid)
            where T : class, IConfigurationCollectionTemplate
        {
            using var context = new DatabaseContext(this.Options.Options);
            var config = context.ConfigurationProfiles
                .Include(c => c.Values)
                .SingleOrDefault(c => c.ValueCollectionGuid == valueCollectionGuid);
            return config?.AsConfiguration<T>();
        }

        [GenericTypeAcceptsConfigurationCollection(0)]
        public IConfigurationCollection<T>? GetConfiguration<T>(Guid gameGuid,
            string sourceName, Guid valueCollectionGuid)
            where T : class, IConfigurationCollectionTemplate
        {
            using var context = new DatabaseContext(this.Options.Options);
            var profileJunction = context.GameRecordsConfigurationProfiles
                .Include(g => g.Profile)
                .ThenInclude(g => g.Values)
                .SingleOrDefault(g => g.GameID == gameGuid
                                      && g.ConfigurationSource == sourceName
                                      && g.ProfileID == valueCollectionGuid);
            if (profileJunction == null) return null;

            var profile = context.ConfigurationProfiles
                .SingleOrDefault(s => s.ValueCollectionGuid == profileJunction.ProfileID);

            return profile?.AsConfiguration<T>();
        }

        public void UpdateValue(IConfigurationValue value) => this.UpdateValue(value.Guid, value.Value);

        public void UpdateValue(Guid valueGuid, object? value)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var entityValue = context.ConfigurationValues.Find(valueGuid);
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
            context.SaveChanges();
        }

        public Guid GetOwningValueCollection(Guid valueGuid)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var entityValue = context.ConfigurationValues.Find(valueGuid);
            if (entityValue == null) return default;
            return entityValue.ValueCollectionGuid;
        }
    }
}