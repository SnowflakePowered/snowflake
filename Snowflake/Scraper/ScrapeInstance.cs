using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Game;
using Snowflake.Romfile;
using Snowflake.Service;
using System.IO.Compression;
using Snowflake.Platform;
using Snowflake.Records.File;
using Snowflake.Records.Metadata;

namespace Snowflake.Scraper
{
    public class ScrapeInstance
    {
        private readonly IList<string> fileNames;
        private readonly IStoneProvider stoneProvider;
        private readonly IEnumerable<IFileSignature> fileSignatures;
        public ScrapeInstance(IEnumerable<string> fileNames, 
            IScraper gameScraper, 
            IStoneProvider stoneProvider,
            IEnumerable<IFileSignature> fileSignatures)
        {
            this.fileNames = fileNames.ToList();
            this.stoneProvider = stoneProvider;
            this.fileSignatures = fileSignatures;
        }

        IEnumerable<IFileRecord> GetZipFileInformation(IEnumerable<string> zipfiles, string suggestedPlatform = null)
        {
            foreach (string romfile in zipfiles)
            {

                using (Stream stream = File.Open(romfile, FileMode.Open, FileAccess.Read))
                using (ZipArchive archive = new ZipArchive(stream))
                {
                    var romContents = archive.Entries.FirstOrDefault();
                    if (romContents == null) continue;
                    var potentialMimetypes = (from platform in this.stoneProvider.Platforms.Values
                                              from fileType in platform.FileTypes
                                              let extension = Path.GetExtension(romContents.FullName)
                                              where fileType.Key == extension
                                              select new KeyValuePair<string, string>(fileType.Value, fileType.Key))
                                              .ToDictionary(t => t.Key, t => t.Value);
                    if (!potentialMimetypes.Any()) continue;
                    using (Stream romStream = romContents.Open())
                    {
                        var fileSignature = (from fs in this.fileSignatures
                            where fs.FileTypes.Intersect(potentialMimetypes.Keys).Any()
                            where fs.HeaderSignatureMatches(romStream)
                            select fs).FirstOrDefault();
                        //todo compare with crc32 database

                        IFileRecord record;
                        if (fileSignature != null)
                        {
                            var platform = this.stoneProvider.Platforms[fileSignature.SupportedPlatform];
                            string mimeType = platform.FileTypes[Path.GetExtension(romContents.FullName)];
                            record = new FileRecord(romfile, $"{mimeType}+zip");
                            record.Metadata.Add("rom_internalname", fileSignature.GetInternalName(romStream));
                            record.Metadata.Add("rom_gameid", fileSignature.GetGameId(romStream));
                        }
                        else
                        {
                            var platform = this.stoneProvider.Platforms[suggestedPlatform] ??
                                           (from p in this.stoneProvider.Platforms.Values
                                               from fileType in p.FileTypes
                                               let extension = Path.GetExtension(romContents.FullName)
                                               where fileType.Key == extension
                                               select p).FirstOrDefault();
                            if (platform == null) continue;
                            string mimeType = platform.FileTypes[Path.GetExtension(romContents.FullName)];
                            record = new FileRecord(romfile, $"{mimeType}+zip");
                            record.Metadata.Add("scrape_unsure_platform", platform.PlatformID);

                        }
                        record.Metadata.Add("file_crc32", Utility.FileHash.GetCRC32(romStream));
                        yield return record;
                    }
                }
            }
        }
        IEnumerable<IFileRecord> GetFileInformation(IEnumerable<string> filenames, string suggestedPlatform = null)
        {

            foreach (string romfile in filenames)
            {
                var potentialMimetypes = (from platform in this.stoneProvider.Platforms.Values
                                          from fileType in platform.FileTypes
                                          let extension = Path.GetExtension(romfile)
                                          where fileType.Key == extension
                                          select new KeyValuePair<string, string>(fileType.Value, fileType.Key))
                                               .ToDictionary(t => t.Key, t => t.Value);
                if (!potentialMimetypes.Any()) continue;
                using (Stream romStream = File.Open(romfile, FileMode.Open, FileAccess.Read))
                {
                    var fileSignature = (from fs in this.fileSignatures
                                         where fs.FileTypes.Intersect(potentialMimetypes.Keys).Any()
                                         where fs.HeaderSignatureMatches(romStream)
                                         select fs).FirstOrDefault();
                    //todo compare with crc32 database
                    IFileRecord record;
                    if (fileSignature != null)
                    {
                        var platform = this.stoneProvider.Platforms[fileSignature.SupportedPlatform];
                        string mimeType = platform.FileTypes[Path.GetExtension(romfile)];
                        record = new FileRecord(romfile, $"{mimeType}+zip");
                        record.Metadata.Add("rom_internalname", fileSignature.GetInternalName(romStream));
                        record.Metadata.Add("rom_gameid", fileSignature.GetGameId(romStream));
                    }
                    else
                    {
                        var platform = this.stoneProvider.Platforms[suggestedPlatform] ??
                                       (from p in this.stoneProvider.Platforms.Values
                                        from fileType in p.FileTypes
                                        let extension = Path.GetExtension(romfile)
                                        where fileType.Key == extension
                                        select p).FirstOrDefault();
                        if (platform == null) continue;
                        string mimeType = platform.FileTypes[Path.GetExtension(romfile)];
                        record = new FileRecord(romfile, $"{mimeType}+zip");
                        record.Metadata.Add("scrape_unsure_platform", platform.PlatformID);

                    }
                    record.Metadata.Add("file_crc32", Utility.FileHash.GetCRC32(romStream));
                    yield return record;
                }
            }
        }
    }
}
