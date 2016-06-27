using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Game;
using Snowflake.Romfile;
using System.IO.Compression;
using Snowflake.Platform;
using Snowflake.Records.File;
using Snowflake.Records.Metadata;
using Snowflake.Scraper.Providers;
using Snowflake.Scraper.Shiragame;
using Snowflake.Service;
using Snowflake.Utility;

namespace Snowflake.Scraper
{
    public interface IScrapeEngine
    {
        IFileRecord GetFileInformation(string romfile);

    }

    public class ScraperGenerator : IScrapeEngine
    {
        private readonly IStoneProvider stoneProvider;
        private readonly IShiragameProvider shiragameProvider;
        private readonly IEnumerable<IFileSignature> fileSignatures;
        private readonly IEnumerable<IScrapeProvider<IScrapedMetadataCollection>> providers;
        private readonly IDictionary<string, string> validMimetypes;
        private readonly IList<string> validFileExtensions;
    
        public ScraperGenerator (
            IStoneProvider stoneProvider,
            IShiragameProvider shiragameProvider,
            IEnumerable<IFileSignature> fileSignatures)
        {
            this.stoneProvider = stoneProvider;
            this.fileSignatures = fileSignatures;
            this.shiragameProvider = shiragameProvider;

            this.validMimetypes = (from platform in this.stoneProvider.Platforms.Values
                                  from fileType in platform.FileTypes
                                  select new KeyValuePair<string, string>(fileType.Value, fileType.Key)).
                                  ToDictionary(t => t.Key, t => t.Value);
            this.validFileExtensions = (from mimetype in this.validMimetypes select mimetype.Value).ToList();
        }

        public IGameRecord GetGameRecordFromFiles(IFileRecord fileRecord)
        {
            var gr = new GameRecord(this.stoneProvider.Platforms[fileRecord.Metadata[FileMetadataKeys.RomPlatform]], 
                fileRecord.Metadata[FileMetadataKeys.RomCanonicalTitle]);
            gr.Files.Add(fileRecord);
            //merge scrape resu;ts
            var matches = this.providers.Select(x => x.Query(fileRecord.Metadata));
            var _matches = matches.OrderByDescending(result => result.Accuracy).Select(x => x as IMetadataCollection);
            gr.Metadata[GameMetadataKeys.Description] = _matches.First()["scraped_description"];
            //.. and so forth
            // ssomehow merge all provider data? I don't know...
            //.. do run enabled mediaproviders
            return gr;
        }

        public IFileRecord GetFileInformation(string romfile)
        {
            string ext = Path.GetExtension(romfile)?.ToLowerInvariant();
            if (ext == ".zip")
            {
                return this.shiragameProvider.IsMameRom(romfile) ? this.GetMameInformation(romfile) 
                    : this.GetZipStreamInformation(romfile);
            }
            if (!this.validFileExtensions.Contains(ext)) return null; 
            //hopefully this doesn't come back to bite me in the ass.
            //this will break for any nonstandard file extensions
            using (Stream romStream = File.Open(romfile, FileMode.Open, FileAccess.Read))
            {
                return this.GetFileInformation(romfile, romStream);
            }

        }

        IFileRecord GetMameInformation(string mameFile)
        {
            var record = new FileRecord(mameFile, "application/x-romfile-arcade+zip");
            record.Metadata.Add("rom_platform", "ARCADE_MAME");
            record.Metadata.Add("rom_serial", Path.GetFileNameWithoutExtension(mameFile));
            return record;
        }

        IFileRecord GetZipStreamInformation(string zipFile)
        {
            using (ZipArchive archive = new ZipArchive(File.OpenRead(zipFile), ZipArchiveMode.Read))
            {
                var firstEntry = (from entry in archive.Entries
                                  let extension = Path.GetExtension(entry.FullName)?.ToLowerInvariant()
                                  where this.validFileExtensions.Contains(extension) 
                                  //hopefully this doesn't come back to bite me in the ass.
                                  where extension != ".zip"
                                  select entry).FirstOrDefault();
                if (firstEntry == null) return null;
                IFileRecord romRecord = this.GetFileInformation(firstEntry.FullName, firstEntry.Open());
                IFileRecord zipRecord = new FileRecord(zipFile, $"{romRecord.MimeType}+zip");
                zipRecord.Metadata.Add(romRecord.Metadata);
                zipRecord.Metadata.Add(FileMetadataKeys.RomZipRunnableFilename, firstEntry.FullName);
                return zipRecord;
            }

        }

        IFileRecord GetFileInformation(string romfile, Stream romStream)
        {
            string ext = Path.GetExtension(romfile)?.ToLowerInvariant();
            var potentialMimetypes = (from mimetype in this.validMimetypes
                                      where mimetype.Value == ext
                                      select mimetype.Key).ToList();

            if (!potentialMimetypes.Any()) return null;

            var romInfo = new FileSignatureMatcher().GetInfo(romfile, romStream); //todo use coreinstance

            return romInfo?.Serial != null ? 
                this.GetFromHash(romfile, romStream) 
                : this.GetFromSerial(romfile, romInfo);
        }

        IFileRecord GetFromHash(string romfile, Stream romStream)
        {
            var romInfo = this.shiragameProvider.GetFromMd5(FileHash.GetMD5(romStream));
            if (romInfo == null) return null;
            IFileRecord record = new FileRecord(romfile, romInfo.MimeType);
            record.Metadata.Add(FileMetadataKeys.RomRegion, romInfo.Region);
            record.Metadata.Add(FileMetadataKeys.FileHashMd5, romInfo.MD5);
            record.Metadata.Add(FileMetadataKeys.FileHashSha1, romInfo.SHA1);
            record.Metadata.Add(FileMetadataKeys.FileHashCrc32, romInfo.CRC32);
            record.Metadata.Add(FileMetadataKeys.RomCanonicalTitle, new StructuredFilename(romInfo.FileName).Title);
            record.Metadata.Add(FileMetadataKeys.RomPlatform, romInfo.PlatformId);
            record.Metadata.Add(FileMetadataKeys.RomTitleFromFilename, new StructuredFilename(romfile).Title);
            return record;
        }

        IFileRecord GetFromSerial(string romfile, IRomFileInfo romInfo)
        {
            var gamePlatform =
                this.stoneProvider.Platforms.First(p => p.Value.FileTypes.Values.Contains(romInfo.Mimetype)).Value;
            string romSerial = romInfo.Serial;
            string mimeType = romInfo.Mimetype;
            IFileRecord record = new FileRecord(romfile, $"{mimeType}");
            if(romInfo.InternalName != null)
                record.Metadata.Add(FileMetadataKeys.RomInternalName, romInfo.InternalName);
            record.Metadata.Add(FileMetadataKeys.RomSerial, romSerial);
            record.Metadata.Add(FileMetadataKeys.RomTitleFromFilename, new StructuredFilename(romfile).Title);
            var serialInfo = this.shiragameProvider.GetFromSerial(gamePlatform.PlatformID, romSerial);
            if (serialInfo == null) return record;
            record.Metadata.Add(FileMetadataKeys.RomCanonicalTitle, serialInfo.Title);
            record.Metadata.Add(FileMetadataKeys.RomRegion, serialInfo.Region);
            return record;
        }

    }
}
