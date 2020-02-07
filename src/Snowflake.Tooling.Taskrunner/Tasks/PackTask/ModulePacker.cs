using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using Snowflake.Tooling.Taskrunner.Tasks.AssemblyModuleBuilderTasks;

namespace Snowflake.Tooling.Taskrunner.Tasks.PackTask
{
    internal class ModulePacker
    {
        private DirectoryInfo ModuleDirectory { get; }
        private ModuleDefinition ModuleDefinition { get; }

        public ModulePacker(DirectoryInfo moduleDirectory, ModuleDefinition moduleDefinition)
        {
            this.ModuleDirectory = moduleDirectory;
            this.ModuleDefinition = moduleDefinition;
        }

        private string GetPackageName() => this.ModuleDirectory.Name.StartsWith($"{this.ModuleDefinition.Loader}.") ?
            this.ModuleDirectory.Name : $"{this.ModuleDefinition.Loader}.{this.ModuleDirectory.Name}";

        private byte[] GetSHA512(FileStream archive)
        {
            archive.Position = 0;
            using var sha = SHA512.Create();
            byte[] hash = sha.ComputeHash(archive);
            archive.Position = 0;
            return hash;
        }

        public async Task<string> PackArchive(DirectoryInfo outputDirectory, bool noTreeShaking = false)
        {
            (FileStream packageContents, RSA rsa) = await this.CreateArchive(noTreeShaking);

            using var snowballArchiveStream =
                File.Create(Path.Combine(outputDirectory.FullName, $"{this.GetPackageName()}.snowpkg"));

            using var snowballArchive = new ZipOutputStream(snowballArchiveStream);

            byte[] sha512 = this.GetSHA512(packageContents);
            byte[] signedSha512 = rsa.SignHash(sha512, HashAlgorithmName.SHA512, RSASignaturePadding.Pss);

            snowballArchive.SetLevel(5);
            snowballArchive.UseZip64 = UseZip64.On;
            var contentEntry = new ZipEntry("contents");
            snowballArchive.PutNextEntry(contentEntry);
            packageContents.Position = 0;
            await packageContents.CopyToAsync(snowballArchive, 4096).ConfigureAwait(false);
            snowballArchive.CloseEntry();

            Console.WriteLine($"Signing package...");

            using var sigReader = new MemoryStream(signedSha512);

            snowballArchive.PutNextEntry(new ZipEntry("signature"));
            sigReader.Position = 0;
            await sigReader.CopyToAsync(snowballArchive, 1024).ConfigureAwait(false);
            snowballArchive.CloseEntry();


            using var defReader =
                new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this.ModuleDefinition)));

            snowballArchive.PutNextEntry(new ZipEntry("module"));
            defReader.Position = 0;
            await defReader.CopyToAsync(snowballArchive, 1024).ConfigureAwait(false);
            snowballArchive.CloseEntry();

            return Path.Combine(outputDirectory.FullName, $"{this.GetPackageName()}.snowpkg");
        }

        private async Task<(FileStream fileStream, RSA signature)> CreateArchive(bool noTreeShaking)
        {
            string fileName = Path.GetTempFileName();
            IList<string> treeShaker = noTreeShaking
                ? Enumerable.Empty<string>().ToList()
                : new AssemblyTreeShaker()
                    .GetFrameworkDependencies(this.ModuleDirectory, this.ModuleDefinition).ToList();

            // Never dispose archiveStream and rsa. These are passed along later!
            var archiveStream = File.Create(fileName, 4096, FileOptions.DeleteOnClose);
            var rsa = RSA.Create();
            
            rsa.KeySize = 2048;
            using var zipStream = new ZipOutputStream(archiveStream);

            zipStream.SetLevel(1);
            zipStream.UseZip64 = UseZip64.On;
            zipStream.IsStreamOwner = false;
            foreach (var file in this.ModuleDirectory.EnumerateFiles("*", SearchOption.AllDirectories)
                .Where(f => !treeShaker.Contains(Path.GetFileName(f.Name))))
            {
                Console.WriteLine($"Packing {file.Name}...");
                var entry = new ZipEntry(ZipEntry.CleanName(Path.Combine($"{this.GetPackageName()}",
                    file.GetRelativePathFrom(this.ModuleDirectory)))) {DateTime = file.LastWriteTime};
                zipStream.PutNextEntry(entry);
                using var streamReader = file.OpenRead();
                streamReader.Position = 0;
                await streamReader.CopyToAsync(zipStream, 4096).ConfigureAwait(false);
                zipStream.CloseEntry();
            }

            string rsaParameters = JsonConvert.SerializeObject(rsa.ExportParameters(false));

            zipStream.PutNextEntry(new ZipEntry(Path.Combine("manifest", "key")));
            using var keySig = new MemoryStream(Encoding.UTF8.GetBytes(rsaParameters)) {Position = 0};
            await keySig.CopyToAsync(zipStream, 4096).ConfigureAwait(false);
            zipStream.CloseEntry();

            zipStream.PutNextEntry(new ZipEntry(Path.Combine("manifest", "package")));
            using var packageName = new MemoryStream(Encoding.UTF8.GetBytes(this.GetPackageName())) {Position = 0};
            await packageName.CopyToAsync(zipStream, 4096).ConfigureAwait(false);
            zipStream.CloseEntry();

            return (archiveStream, rsa);
        }
    }
}