using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Snowflake.Model.Database.Models
{
    // Hackaround to allow a profile name for configurations.
    // big yikes thanks to EF :/
    // thankfully this is hidden behind the much nicer ConfigurationCollectionStore classes.
    internal class GameRecordConfigurationProfileModel
    {
        public Guid ProfileID { get; set; }
        public Guid GameID { get; set; }
        public GameRecordModel? Game { get; set; }
        public ConfigurationProfileModel Profile { get; set; }
        public string ProfileName { get; set; }
        public string ConfigurationSource { get; set; }

        public static void SetupModel(ModelBuilder modelBuilder)
        {
            // big yikes

            modelBuilder.Entity<GameRecordConfigurationProfileModel>()
                .HasOne(p => p.Game)
                .WithMany(p => p.ConfigurationProfiles)
                .HasForeignKey(p => p.GameID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GameRecordConfigurationProfileModel>()
                .HasOne(p => p.Profile)
                .WithOne()
                .HasForeignKey<GameRecordConfigurationProfileModel>(p => p.ProfileID);


            modelBuilder.Entity<GameRecordConfigurationProfileModel>()
                .Property(p => p.ProfileName)
                .IsRequired();


            modelBuilder.Entity<GameRecordConfigurationProfileModel>()
                .HasKey(k => new
                {
                    k.ProfileName, k.GameID,
                    k.ConfigurationSource
                });
        }
    }
}
