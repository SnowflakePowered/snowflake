using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Game;

namespace Snowflake.Model.Database.Contexts
{
    internal class GameRecordModel : RecordModel
    {
        public PlatformId Platform { get; set; } 
    }

    internal class GameRecordContext : RecordContext
    {
        public GameRecordContext(DbContextOptions<GameRecordContext> options)
           : base(options)
        {

        }

        public DbSet<GameRecordModel> GameRecords { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<GameRecordModel>()
                .Property(r => r.Platform)
                .HasConversion(p => p.PlatformIdString, 
                    p => new PlatformId(p))
                .IsRequired();

            base.OnModelCreating(modelBuilder);

        }
    }
}
