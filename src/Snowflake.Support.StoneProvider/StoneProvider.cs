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
    /// <summary>
    /// Implements <see cref="IStoneProvider"/> with the built-in Stone definitions.
    /// </summary>
    public class StoneProvider : IStoneProvider
    {
        /// <inheritdoc/>
        public IDictionary<PlatformId, IPlatformInfo> Platforms { get; }

        /// <inheritdoc/>
        public IDictionary<ControllerId, IControllerLayout> Controllers { get; }

        private IDictionary<string, IList<IFileSignature>> FileSignatures { get; }
        /// <inheritdoc/>
        public Version StoneVersion { get; }

        /// <summary>
        /// Creates a new <see cref="StoneProvider"/> with the built-in Stone definitions.
        /// </summary>
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

            this.FileSignatures = new Dictionary<string, IList<IFileSignature>>()
            {
                {"application/vnd.stone-romfile.sony.psx-disctrack", new List<IFileSignature>() { new PlaystationCDRomFileSignature() } },
                {"application/vnd.stone-romfile.sony.ps2-discimage", new List<IFileSignature>() { new Playstation2Iso9660FileSignature(), new Playstation2CDRomFileSignature() } },
                {"application/vnd.stone-romfile.sony.psp-discimage", new List<IFileSignature>() { new PlaystationPortableIso9660FileSignature() } },
                {"application/vnd.stone-romfile.nintendo.wii-wbfs", new List<IFileSignature>() { new WiiWbfsFileSignature() } },
                {"application/vnd.stone-romfile.nintendo.wii-discimage", new List<IFileSignature>() { new WiiIso9660FileSignature() } },
                {"application/vnd.stone-romfile.nintendo.gcn-discimage", new List<IFileSignature>() { new GamecubeIso9660FileSignature() } },
                {"application/vnd.stone-romfile.nintendo.snes", new List<IFileSignature>() { new SuperNintendoHeaderlessFileSignature() } },
                {"application/vnd.stone-romfile.nintendo.snes-magiccard", new List<IFileSignature>() { new SuperNintendoSmcHeaderFileSignature() } },
                {"application/vnd.stone-romfile.nintendo.nes-unif", new List<IFileSignature>() { new NintendoEntertainmentSystemUnifFileSignature() } },
                {"application/vnd.stone-romfile.nintendo.nes-ines", new List<IFileSignature>() { new NintendoEntertainmentSystemiNesFileSignature() } },
                {"application/vnd.stone-romfile.nintendo.nds", new List<IFileSignature>() { new NintendoDSFileSignature() } },
                {"application/vnd.stone-romfile.nintendo.gbc", new List<IFileSignature>() { new GameboyColorFileSignature() } },
                {"application/vnd.stone-romfile.nintendo.gb", new List<IFileSignature>() { new GameboyFileSignature() } },
                {"application/vnd.stone-romfile.nintendo.gba", new List<IFileSignature>() { new GameboyAdvancedFileSignature() } },
                {"application/vnd.stone-romfile.nintendo.n64-littleendian", new List<IFileSignature>() { new Nintendo64LittleEndianFileSignature() } },
                {"application/vnd.stone-romfile.nintendo.n64-byteswapped", new List<IFileSignature>() { new Nintendo64ByteswappedFileSignature() } },
                {"application/vnd.stone-romfile.nintendo.n64-bigendian", new List<IFileSignature>() { new Nintendo64BigEndianFileSignature() } },
                {"application/vnd.stone-romfile.sega.32x", new List<IFileSignature>() { new Sega32XFileSignature() } },
                {"application/vnd.stone-romfile.sega.scd-disctrack", new List<IFileSignature>() { new SegaCdRawImageFileSignature() } },
                {"application/vnd.stone-romfile.sega.dc-discjuggler", new List<IFileSignature>() { new SegaDreamcastDiscJugglerFileSignature() } },
                {"application/vnd.stone-romfile.sega.dc-disctrack", new List<IFileSignature>() { new SegaDreamcastRawDiscFileSignature() } },
                {"application/vnd.stone-romfile.sega.sms",new List<IFileSignature>() { new SegaMasterSystemFileSignature() } },
                {"application/vnd.stone-romfile.sega.gen", new List<IFileSignature>() { new SegaGenesisFileSignature() } },
                {"application/vnd.stone-romfile.sega.gg", new List<IFileSignature>() { new SegaGameGearFileSignature() } },
                {"application/vnd.stone-romfile.sega.sat-disctrack", new List<IFileSignature>() { new SegaSaturnFileSignature() } },
                {"application/vnd.stone-romfile.sega.32xcd-disctrack", new List<IFileSignature>() { new Sega32XCdRawImageFileSignature() } },
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
                foreach (var sig in this.FileSignatures[mimetype])
                {
                    romStream.Seek(streamPos, SeekOrigin.Begin);
                    if (sig.HeaderSignatureMatches(romStream))
                    {
                        romStream.Seek(streamPos, SeekOrigin.Begin);
                        return mimetype;
                    }
                }
            }
            romStream.Seek(streamPos, SeekOrigin.Begin);
            return String.Empty;
        }

        /// <inheritdoc />
        public string GetStoneMimetype(Stream romStream)
        {
            long streamPos = romStream.Position;
            foreach (var (mimetype, sigs) in this.FileSignatures)
            {
                foreach (var sig in sigs)
                {
                    romStream.Seek(streamPos, SeekOrigin.Begin);
                    if (sig.HeaderSignatureMatches(romStream))
                    {
                        romStream.Seek(streamPos, SeekOrigin.Begin);
                        return mimetype;
                    }
                }
            }
            romStream.Seek(streamPos, SeekOrigin.Begin);
            return String.Empty;
        }

        /// <inheritdoc />
        public IEnumerable<IFileSignature> GetSignatures(string mimetype)
        {
            this.FileSignatures.TryGetValue(mimetype.ToLower(), out var s);
            return s;
        }

        /// <inheritdoc />
        public IFileSignature? GetFileSignature(string mimetype, Stream romStream)
        {
            if (!this.FileSignatures.TryGetValue(mimetype.ToLowerInvariant(), out var fileSignatures)) return null;
            long streamPos = romStream.Position;
            foreach (var sig in fileSignatures)
            {
                romStream.Seek(streamPos, SeekOrigin.Begin);
                if (sig.HeaderSignatureMatches(romStream))
                {
                    romStream.Seek(streamPos, SeekOrigin.Begin);
                    return sig;
                }
            }
            romStream.Seek(streamPos, SeekOrigin.Begin);
            return null;
        }
    }
}
