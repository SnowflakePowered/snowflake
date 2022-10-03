using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Model.Database.Models
{
    internal class ContentLibraryModel
    {
        public string Path { get; set; }

        public Guid LibraryID { get; set; }

        public List<RecordModel> Records { get; set; }

        internal static void SetupModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContentLibraryModel>()
                .HasKey(p => p.LibraryID);

            modelBuilder.Entity<ContentLibraryModel>()
                .Property(p => p.Path)
                .IsRequired();

            modelBuilder.Entity<ContentLibraryModel>()
                .HasMany(g => g.Records)
                .WithOne(p => p.ContentLibrary);
        }
    }
}
