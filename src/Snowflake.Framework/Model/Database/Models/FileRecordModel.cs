using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Game;

namespace Snowflake.Model.Database.Models
{
    internal class FileRecordModel : RecordModel
    {
        public string MimeType { get; set; } = "application/octet-stream";

        internal static void SetupModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileRecordModel>()
              .Property(r => r.MimeType)
              .IsRequired();
        }
    }
}
