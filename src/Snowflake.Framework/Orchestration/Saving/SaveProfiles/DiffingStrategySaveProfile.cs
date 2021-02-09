using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCDiff.Encoders;
using VCDiff.Includes;

namespace Snowflake.Orchestration.Saving.SaveProfiles
{
    internal sealed class DiffingStrategySaveProfile : SaveProfile
    {
        private static readonly string DateFormat = "yyyy-MM-dd.HH-mm-ss";

        public DiffingStrategySaveProfile(Guid profileGuid, 
            string saveType, string profileName, IDirectory profileRoot) 
            : base(profileGuid, saveType, profileName, profileRoot)
        {
            var saveManifest = this.ProfileRoot.OpenFile("profile");
            if (!saveManifest.Created)
            {
                saveManifest.WriteAllText($"{profileName}{Environment.NewLine}{nameof(SaveManagementStrategy.Diff)}");
            }
        }

        public override SaveManagementStrategy ManagementStrategy => SaveManagementStrategy.Diff;

        private async Task CreateBaseSave(IReadOnlyDirectory saveContents)
        {
            var contentDirectory = this.ProfileRoot.OpenDirectory("base/content");
            await foreach (var _ in contentDirectory.CopyFromDirectory(saveContents)) { };
        }

        public async override Task<ISaveGame> CreateSave(IReadOnlyDirectory saveContents)
        {
            if (!this.ProfileRoot.ContainsDirectory("base")) await this.CreateBaseSave(saveContents);
            using var rollingHash = new RollingHash(32);

            // setup
            var newGuid = Guid.NewGuid();
            var saveName = $"{DateTimeOffset.UtcNow.ToString(DateFormat)}-{newGuid}";
            var saveDirectory = this.ProfileRoot.OpenDirectory(saveName);
            var contentDirectory = saveDirectory.OpenDirectory("content");

            // diff is for anything that exists in the base directory
            var diffDir = contentDirectory.OpenDirectory("diff");

            // copy is for anything that does not and can not be diffed.
            var copyDir = contentDirectory.OpenDirectory("copy"); 

            // Traverse base directory in tandem with saveContents
            var baseDir = this.ProfileRoot.OpenDirectory("base/content").AsReadOnly();

            foreach (var f in saveContents.EnumerateFiles())
            {
                if (!baseDir.ContainsFile(f.Name))
                {
                    await copyDir.CopyFromAsync(f);
                    continue;
                }

                using var targetStream = f.OpenReadStream();
                using var baseStream = baseDir.OpenFile(f.Name).OpenReadStream();

                using var outStream = diffDir.OpenFile(f.Name).OpenStream();
                using var decoder = new VcEncoder(baseStream, targetStream, outStream,
                    rollingHash: rollingHash, blockSize: 32);
                VCDiffResult result = await decoder.EncodeAsync();
                if (result != VCDiffResult.SUCCESS) throw new IOException($"Failed to encode delta for {f.Name}");
            }

            foreach (var d in saveContents.EnumerateDirectories().Where(d => !baseDir.ContainsDirectory(d.Name)))
            {
                // Copy all directories not in the base.
                await foreach (var _ in copyDir.OpenDirectory(d.Name).CopyFromDirectory(d)) { };
            }

            var queuedDirs = (from targetDir in saveContents.EnumerateDirectories()
                              where baseDir.ContainsDirectory(targetDir.Name)
                              select (diffDir, baseDir.OpenDirectory(targetDir.Name), targetDir)).ToList();

            Queue<(IDeletableDirectory parentDir, IReadOnlyDirectory baseDir, IReadOnlyDirectory targetDir)> dirsToProcess =
                new(queuedDirs);

            while (dirsToProcess.Count > 0)
            {
                var (parent, src, diff) = dirsToProcess.Dequeue();
                var dst = parent.OpenDirectory(src.Name);
                foreach (var f in src.EnumerateFiles())
                {
                    if (!diff.ContainsFile(f.Name)) continue;
                    using var baseStream = f.OpenReadStream();
                    using var targetStream = diff.OpenFile(f.Name).OpenReadStream();
                    using var outStream = dst.OpenFile(f.Name).OpenStream();
                    using var decoder = new VcEncoder(baseStream, targetStream, outStream,
                        rollingHash: rollingHash, blockSize: 32);
                    VCDiffResult result = await decoder.EncodeAsync();
                    if (result != VCDiffResult.SUCCESS) throw new IOException($"Failed to decode delta for {f.Name}");

                }

                var children = from targetDir in diff.EnumerateDirectories()
                               where src.ContainsDirectory(targetDir.Name)
                               select (dst, src.OpenDirectory(targetDir.Name), targetDir);

                foreach (var childDirectory in children)
                {
                    dirsToProcess.Enqueue(childDirectory);
                }
            }

            this.ProfileRoot.OpenFile("latest").WriteAllText(saveName, Encoding.UTF8);
            return this.GetSave(saveDirectory)!;
        }

        public override async Task<ISaveGame> CreateSave(ISaveGame saveGame)
        {
            // Get a random directory
            using var tempDirectory = this.ProfileRoot.OpenDirectory($"temp-{Guid.NewGuid()}").AsDisposable();

            // Extract directory contents
            await saveGame.ExtractSave(tempDirectory);

            var created = await this.CreateSave(tempDirectory.AsReadOnly());
            return created;
        }

        private ISaveGame? GetSave(IDirectory internalSaveDir)
        {
            string name = internalSaveDir.Name;
            if (!DateTimeOffset.TryParseExact(name[0..DateFormat.Length], DateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal, out var date)) return null;
            if (!Guid.TryParseExact(name[(DateFormat.Length + 1)..], "D", out var guid)) return null;

            var baseContent = this.ProfileRoot.OpenDirectory("base/content");
            return new DiffingSaveGame(date, guid, this.SaveType, baseContent, internalSaveDir.OpenDirectory("content"));
        }

        public override ISaveGame? GetHeadSave()
        {
            var latest = this.ProfileRoot.OpenFile("latest").ReadAllText();
            if (latest.Length < DateFormat.Length) return null;
            var saveDirectory = this.ProfileRoot.OpenDirectory(latest);
            return this.GetSave(saveDirectory);
        }

        public override IEnumerable<ISaveGame> GetHistory()
        {
            return this.ProfileRoot.EnumerateDirectories().Select(this.GetSave).Where(s => s != null);
        }

        public override void ClearHistory()
        {
            var latest = this.ProfileRoot.OpenFile("latest").ReadAllText();
            foreach (var toDelete in this.ProfileRoot.EnumerateDirectories().Where(d => d.Name != latest))
            {
                toDelete.Delete();
            }
        }
    }
}
