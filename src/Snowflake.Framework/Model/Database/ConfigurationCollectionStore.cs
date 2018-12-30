using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Configuration;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Database.Models;

namespace Snowflake.Model.Database
{
    internal class ConfigurationCollectionStore
    {
        private DbContextOptionsBuilder<DatabaseContext> Options { get; set; }

        public ConfigurationCollectionStore(DbContextOptionsBuilder<DatabaseContext> options)
        {
            this.Options = options;
            using (var context = new DatabaseContext(this.Options.Options))
            {
                context.Database.EnsureCreated();
            }
        }

        public IConfigurationCollection<T> CreateConfiguration<T>()
            where T: class, IConfigurationCollection<T>
        {
            var collection = new ConfigurationCollection<T>();
            
            using (var context = new DatabaseContext(this.Options.Options))
            {
                context.ConfigurationProfiles.Add(collection.AsModel());
                context.SaveChanges();
            }

            return collection;
        }

        public void UpdateConfiguration(IConfigurationCollection configurationCollection)
        {

            using (var context = new DatabaseContext(this.Options.Options))
            {
                var guid = configurationCollection.ValueCollection.Guid;
                var config = context.ConfigurationProfiles?
                    .Find(guid);
                if (config == null) return;

                foreach
                   (var value in
                   context.ConfigurationValues.Where(v => v.ValueCollectionGuid == guid))
                {
                    var realValue = configurationCollection[value.SectionKey][value.OptionKey]
                        ?.AsConfigurationStringValue();
                    if (realValue != value.Value && realValue != null)
                    {
                        value.Value = realValue;
                        context.Entry(value).State = EntityState.Modified;
                    }

                }

                foreach (var t in configurationCollection.ValueCollection)
                {
                    var value = context.ConfigurationValues.Find(t.value.Guid);
                    if (value != null) continue;
                  
                    context.ConfigurationValues.Add(new ConfigurationValueModel
                    {
                        SectionKey = t.section,
                        OptionKey = t.option,
                        Guid = t.value.Guid,
                        Value = t.value.Value.AsConfigurationStringValue(),
                        ValueCollectionGuid = configurationCollection.ValueCollection.Guid
                    });
                }
                context.SaveChanges();
            }
        }

        public IConfigurationCollection<T>? GetConfiguration<T>(Guid valueCollectionGuid)
            where T : class, IConfigurationCollection<T>
        {
            using (var context = new DatabaseContext(this.Options.Options))
            {
                var config = context.ConfigurationProfiles
                    .Include(c => c.Values)
                    .SingleOrDefault(c => c.ValueCollectionGuid == valueCollectionGuid);
                if (config == null) return null;
                return config.AsConfiguration<T>();
            }
        }
    }
}
