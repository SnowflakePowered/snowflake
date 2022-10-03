﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Snowflake.Model.Database.Models;

#nullable disable

namespace Snowflake.Model.Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20221003040212_AddContentLibrary")]
    partial class AddContentLibrary
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("Snowflake.Model.Database.Models.ConfigurationProfileModel", b =>
                {
                    b.Property<Guid>("ValueCollectionGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ConfigurationSource")
                        .HasColumnType("TEXT");

                    b.HasKey("ValueCollectionGuid");

                    b.ToTable("ConfigurationProfiles");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.ConfigurationValueModel", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("OptionKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SectionKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ValueCollectionGuid")
                        .HasColumnType("TEXT");

                    b.Property<int>("ValueType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Guid");

                    b.HasIndex("ValueCollectionGuid");

                    b.ToTable("ConfigurationValues");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.ContentLibraryModel", b =>
                {
                    b.Property<Guid>("LibraryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LibraryID");

                    b.ToTable("ContentLibraries");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.ControllerElementMappingCollectionModel", b =>
                {
                    b.Property<Guid>("ProfileID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ControllerID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("DriverType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProfileName")
                        .HasColumnType("TEXT");

                    b.Property<int>("VendorID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProfileID");

                    b.ToTable("ControllerElementMappings");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.ControllerElementMappingModel", b =>
                {
                    b.Property<Guid>("ProfileID")
                        .HasColumnType("TEXT");

                    b.Property<string>("LayoutElement")
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceCapability")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ProfileID", "LayoutElement");

                    b.ToTable("MappedControllerElements");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.GameRecordConfigurationProfileModel", b =>
                {
                    b.Property<Guid>("ProfileID")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GameID")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConfigurationSource")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProfileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ProfileID", "GameID", "ConfigurationSource");

                    b.HasIndex("GameID");

                    b.HasIndex("ProfileID")
                        .IsUnique();

                    b.ToTable("GameRecordsConfigurationProfiles");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.PortDeviceEntryModel", b =>
                {
                    b.Property<string>("OrchestratorName")
                        .HasColumnType("TEXT");

                    b.Property<string>("PlatformID")
                        .HasColumnType("TEXT");

                    b.Property<int>("PortIndex")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ControllerID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Driver")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("InstanceGuid")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProfileGuid")
                        .HasColumnType("TEXT");

                    b.HasKey("OrchestratorName", "PlatformID", "PortIndex");

                    b.ToTable("PortDeviceEntries");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.RecordMetadataModel", b =>
                {
                    b.Property<Guid>("RecordMetadataID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("MetadataKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MetadataValue")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RecordID")
                        .HasColumnType("TEXT");

                    b.HasKey("RecordMetadataID");

                    b.HasIndex("RecordID");

                    b.ToTable("Metadata");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.RecordModel", b =>
                {
                    b.Property<Guid>("RecordID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ContentLibraryLibraryID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RecordType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("RecordID");

                    b.HasIndex("ContentLibraryLibraryID");

                    b.ToTable("Records");

                    b.HasDiscriminator<string>("Discriminator").HasValue("RecordModel");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.FileRecordModel", b =>
                {
                    b.HasBaseType("Snowflake.Model.Database.Models.RecordModel");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("FileRecordModel");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.GameRecordModel", b =>
                {
                    b.HasBaseType("Snowflake.Model.Database.Models.RecordModel");

                    b.Property<string>("PlatformID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("GameRecordModel");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.ConfigurationValueModel", b =>
                {
                    b.HasOne("Snowflake.Model.Database.Models.ConfigurationProfileModel", "Profile")
                        .WithMany("Values")
                        .HasForeignKey("ValueCollectionGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.ControllerElementMappingModel", b =>
                {
                    b.HasOne("Snowflake.Model.Database.Models.ControllerElementMappingCollectionModel", "Collection")
                        .WithMany("MappedElements")
                        .HasForeignKey("ProfileID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Collection");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.GameRecordConfigurationProfileModel", b =>
                {
                    b.HasOne("Snowflake.Model.Database.Models.GameRecordModel", "Game")
                        .WithMany("ConfigurationProfiles")
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Snowflake.Model.Database.Models.ConfigurationProfileModel", "Profile")
                        .WithOne()
                        .HasForeignKey("Snowflake.Model.Database.Models.GameRecordConfigurationProfileModel", "ProfileID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.RecordMetadataModel", b =>
                {
                    b.HasOne("Snowflake.Model.Database.Models.RecordModel", "Record")
                        .WithMany("Metadata")
                        .HasForeignKey("RecordID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Record");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.RecordModel", b =>
                {
                    b.HasOne("Snowflake.Model.Database.Models.ContentLibraryModel", "ContentLibrary")
                        .WithMany("Records")
                        .HasForeignKey("ContentLibraryLibraryID");

                    b.Navigation("ContentLibrary");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.ConfigurationProfileModel", b =>
                {
                    b.Navigation("Values");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.ContentLibraryModel", b =>
                {
                    b.Navigation("Records");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.ControllerElementMappingCollectionModel", b =>
                {
                    b.Navigation("MappedElements");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.RecordModel", b =>
                {
                    b.Navigation("Metadata");
                });

            modelBuilder.Entity("Snowflake.Model.Database.Models.GameRecordModel", b =>
                {
                    b.Navigation("ConfigurationProfiles");
                });
#pragma warning restore 612, 618
        }
    }
}
