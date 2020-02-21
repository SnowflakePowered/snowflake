using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCDiff.Decoders;
using VCDiff.Includes;

namespace Snowflake.Orchestration.Saving.SaveProfiles
{
    internal sealed class DiffingSaveGame : SaveGame
    {
        public DiffingSaveGame(DateTimeOffset createdTime, Guid saveGuid, string saveType, IDirectory baseDir, IDirectory diffDir)
            : base(createdTime, saveGuid, saveType)
        {
            this.BaseDir = baseDir;
            this.DiffDir = diffDir;
        }

        public IDirectory BaseDir { get; }
        public IDirectory DiffDir { get; }
         
        private async Task CopyRecursiveNonDiffs(IDirectory outputDirectory)
        {
            var baseDir = this.DiffDir.OpenDirectory("copy");
            await foreach (var _ in outputDirectory.CopyFromDirectory(baseDir, true)) { };
        }

        private async Task CopyRecursiveDiffs(IDirectory outputDirectory)
        {
            var diffDir = this.DiffDir.OpenDirectory("diff");
            // Do the parent directory
            foreach (var f in this.BaseDir.EnumerateFiles())
            {
                if (!diffDir.ContainsFile(f.Name)) continue;
                using var baseStream = f.OpenReadStream();
                using var diffStream = diffDir.OpenFile(f.Name).OpenReadStream();
                using var outStream = outputDirectory.OpenFile(f.Name).OpenStream();
                using var decoder = new VcDecoder(baseStream, diffStream, outStream);
                (VCDiffResult result, var _) =  await decoder.DecodeAsync();
                if (result != VCDiffResult.SUCCESS) throw new IOException($"Failed to decode delta for {f.Name}");
            }

            var queuedDirs = (from baseDir in this.BaseDir.EnumerateDirectories()
                             where diffDir.ContainsDirectory(baseDir.Name)
                             select (outputDirectory, baseDir, diffDir.OpenDirectory(baseDir.Name))).ToList();         

            // BFS over all the children.
            Queue<(IDirectory parentDir, IDirectory baseDir, IDirectory diffDir)> dirsToProcess =
                new Queue<(IDirectory, IDirectory, IDirectory)>(queuedDirs);

            while (dirsToProcess.Count > 0)
            {
                var (parent, src, diff) = dirsToProcess.Dequeue();
                var dst = parent.OpenDirectory(src.Name);
                foreach (var f in src.EnumerateFiles())
                {
                    if (!diff.ContainsFile(f.Name)) continue;
                    using var baseStream = f.OpenReadStream();
                    using var diffStream = diff.OpenFile(f.Name).OpenReadStream();
                    using var outStream = dst.OpenFile(f.Name).OpenStream();
                    using var decoder = new VcDecoder(baseStream, diffStream, outStream);
                    (VCDiffResult result, var _) = await decoder.DecodeAsync();
                    if (result != VCDiffResult.SUCCESS) throw new IOException($"Failed to decode delta for {f.Name}");

                }

                var children = from baseDir in src.EnumerateDirectories()
                                where diff.ContainsDirectory(baseDir.Name)
                                select (dst, baseDir, diff.OpenDirectory(baseDir.Name));
                
                foreach (var childDirectory in children)
                {
                    dirsToProcess.Enqueue(childDirectory);
                }
            }
        }

        public override async Task ExtractSave(IDirectory outputDirectory)
        {
            await CopyRecursiveDiffs(outputDirectory);
            await CopyRecursiveNonDiffs(outputDirectory);
        }
    }
}
