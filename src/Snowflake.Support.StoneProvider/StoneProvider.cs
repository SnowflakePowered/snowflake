using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using Snowflake.Romfile;
using Snowflake.Stone.FileSignatures;
using Snowflake.Stone.FileSignatures.Nintendo;
using Snowflake.Stone.FileSignatures.Sega;
using Snowflake.Stone.FileSignatures.Sony;

namespace Snowflake.Services
{
    public class StoneProvider : IStoneProvider
    {
        /// <inheritdoc/>
        public IDictionary<PlatformId, IPlatformInfo> Platforms { get; }

        /// <inheritdoc/>
        public IDictionary<ControllerId, IControllerLayout> Controllers { get; }

        private IDictionary<string, IFileSignature> FileSignatures { get; }
        /// <inheritdoc/>
        public Version StoneVersion { get; }

        public StoneProvider()
        {
            string stoneData = this.GetStoneData();
            var stone = JsonConvert.DeserializeAnonymousType(stoneData,
                new
                {
                    Controllers = new Dictionary<ControllerId, ControllerLayout>(),
                    Platforms = new Dictionary<PlatformId, PlatformInfo>(),
                    version = string.Empty,
                });
            this.Platforms = stone.Platforms.ToDictionary(kvp => kvp.Key, kvp => kvp.Value as IPlatformInfo);
            this.Controllers = stone.Controllers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value as IControllerLayout);
            this.StoneVersion = Version.Parse(stone.version.Split('-')[0]); // todo: introduce semver

            this.FileSignatures = new Dictionary<string, IFileSignature>()
            {
                {"application/vnd.stone-romfile.sony.psx-discimage", new PlaystationRawDiscFileSignature()},
                {"application/vnd.stone-romfile.sony.ps2-discimage", new Playstation2Iso9660FileSignature()},
                {"application/vnd.stone-romfile.sony.psp-discimage", new PlaystationPortableIso9660FileSignature()},
                {"application/vnd.stone-romfile.nintendo.wii-wbfs", new WiiWbfsFileSignature()},
                {"application/vnd.stone-romfile.nintendo.wii-discimage", new WiiIso9660FileSignature()},
                {"application/vnd.stone-romfile.nintendo.snes", new SuperNintendoHeaderlessFileSignature()},
                {"application/vnd.stone-romfile.nintendo.snes-magiccard", new SuperNintendoSmcHeaderFileSignature()},
                {"application/vnd.stone-romfile.nintendo.nes-unif", new NintendoEntertainmentSystemUnifFileSignature()},
                {"application/vnd.stone-romfile.nintendo.nes-ines", new NintendoEntertainmentSystemiNesFileSignature()},
                {"application/vnd.stone-romfile.nintendo.nds", new NintendoDSFileSignature()},
                {"application/vnd.stone-romfile.nintendo.gbc", new GameboyColorFileSignature()},
                {"application/vnd.stone-romfile.nintendo.gb", new GameboyFileSignature()},
                {"application/vnd.stone-romfile.nintendo.gba", new GameboyAdvancedFileSignature()},
                {"application/vnd.stone-romfile.nintendo.n64-littleendian", new Nintendo64LittleEndianFileSignature()},
                {"application/vnd.stone-romfile.nintendo.n64-byteswapped", new Nintendo64ByteswappedFileSignature()},
                {"application/vnd.stone-romfile.nintendo.n64-bigendian", new Nintendo64BigEndianFileSignature()},
                {"application/vnd.stone-romfile.sega.32x", new Sega32XFileSignature()},
                {"application/vnd.stone-romfile.sega.scd-discimage", new SegaCdRawImageFileSignature()},
                {"application/vnd.stone-romfile.sega.gen", new SegaGenesisFileSignature()},
                {"application/vnd.stone-romfile.sega.gg", new SegaGameGearFileSignature()},
                {"application/vnd.stone-romfile.sega.sat-discimage", new SegaSaturnFileSignature()},
                {"application/vnd.stone-romfile.sega.32xcd-discimage", new Sega32XCdRawImageFileSignature()},
            };
        }

        private string GetStoneData()
        {
            var assembly = typeof(StoneProvider).GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream($"Snowflake.stone.dist.json"))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <inheritdoc />
        public string GetStoneMimetype(PlatformId knownPlatform, Stream romStream, string extensionFallback)
        {
            this.Platforms.TryGetValue(knownPlatform, out IPlatformInfo platform);
            if (platform == null) return String.Empty;
            long streamPos = romStream.Position;
            foreach (var mimetype in platform.FileTypes.Values)
            {
                if (!this.FileSignatures.ContainsKey(mimetype)) continue;
                romStream.Seek(streamPos, SeekOrigin.Begin);
                if (this.FileSignatures[mimetype].HeaderSignatureMatches(romStream))
                {
                    romStream.Seek(streamPos, SeekOrigin.Begin);
                    return mimetype;
                }
            }
            romStream.Seek(streamPos, SeekOrigin.Begin);
            return String.Empty;
        }

        /// <inheritdoc />
        public string GetStoneMimetype(Stream romStream)
        {
            long streamPos = romStream.Position;
            foreach (var (mimetype, sig) in this.FileSignatures)
            {
                romStream.Seek(streamPos, SeekOrigin.Begin);
                if (sig.HeaderSignatureMatches(romStream))
                {
                    romStream.Seek(streamPos, SeekOrigin.Begin);
                    return mimetype;
                }
            }
            romStream.Seek(streamPos, SeekOrigin.Begin);
            return String.Empty;
        }

        /// <inheritdoc />
        public IFileSignature? GetSignature(string mimetype)
        {
            this.FileSignatures.TryGetValue(mimetype.ToLower(), out IFileSignature s);
            return s;
        }
    }
}
