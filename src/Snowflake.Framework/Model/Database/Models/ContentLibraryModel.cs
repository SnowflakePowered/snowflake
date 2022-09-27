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

        public Guid LibraryId { get; set; }
        internal static void SetupModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContentLibraryModel>()
                .HasKey(p => p.LibraryId);

            modelBuilder.Entity<ContentLibraryModel>()
                .Property(p => p.Path)
                .IsRequired();
        }
    }
}
