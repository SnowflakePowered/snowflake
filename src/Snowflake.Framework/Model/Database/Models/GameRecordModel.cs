using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Game;
using Snowflake.Model.Records;
using Snowflake.Model.Records.Game;

#nullable disable
namespace Snowflake.Model.Database.Models
{
    internal class GameRecordModel : RecordModel, IGameRecordQuery
    {
        public PlatformId PlatformID { get; set; }

        public List<GameRecordConfigurationProfileModel> ConfigurationProfiles { get; set; }

        IEnumerable<IRecordMetadataQuery> IGameRecordQuery.Metadata => this.Metadata;

        internal static new void SetupModel(ModelBuilder modelBuilder)
        {    
            modelBuilder.Entity<GameRecordModel>()
                .Property(r => r.PlatformID)
                .HasConversion(p => p.ToString(), s => s)
                .IsRequired();
        }
    }
}
