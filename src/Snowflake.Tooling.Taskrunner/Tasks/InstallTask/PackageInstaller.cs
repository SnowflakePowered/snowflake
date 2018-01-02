using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;

namespace Snowflake.Tooling.Taskrunner.Tasks.InstallTask
{
    public class PackageInstaller
    {
        private Stream PackageStream { get; }
        public PackageInstaller(Stream stream)
        {
            this.PackageStream = stream;
        }

        private byte[] GetSHA512(Stream archive)
        {
            using (var sha = SHA512.Create())
            {
                byte[] hash = sha.ComputeHash(archive);
                return hash;
            }
        }

        public async Task<string> InstallPackage(DirectoryInfo moduleDirectory)
        {
            this.PackageStream.Position = 0;
            using (ZipFile file = new ZipFile(this.PackageStream))
            {
                file.IsStreamOwner = false;
                using (Stream contents = file.GetInputStream(file.GetEntry("contents")))
                {
                    using (FileStream tempContents = File.Create(Path.GetTempFileName(), 4096, FileOptions.DeleteOnClose))
                    {
                        await contents.CopyToAsync(tempContents);
                        using (ZipFile contentArchive = new ZipFile(tempContents))
                        {
                            var packageEntry = contentArchive.GetEntry("manifest/package");
                            var packageEntryStream = contentArchive.GetInputStream(packageEntry);
                            MemoryStream packageStream = new MemoryStream();
                            await packageEntryStream.CopyToAsync(packageStream);
                            byte[] packageNameBytes = packageStream.ToArray();
                            packageStream.Close();
                            string packageName = Encoding.UTF8.GetString(packageNameBytes);

                            try
                            {
                                Console.WriteLine("Cleaning module directory...");
                                moduleDirectory.CreateSubdirectory(packageName).Delete(true);
                            }
                            catch
                            {
                                throw new IOException("Unable to clean directory module directory, is it in use?");
                            }

                            foreach(ZipEntry entry in contentArchive)
                            {
                                if (!entry.IsFile)
                                {
                                    continue;
                                }
                                String entryFileName = entry.Name;

                                if (!entryFileName.StartsWith(packageName))
                                {
                                    continue;
                                }

                                byte[] buffer = new byte[4096];

                                Stream zipStream = contentArchive.GetInputStream(entry);

                                String fullZipToPath = Path.Combine(moduleDirectory.FullName, entryFileName);
                                string directoryName = Path.GetDirectoryName(fullZipToPath);
                                if (directoryName.Length > 0)
                                {
                                    Directory.CreateDirectory(directoryName);
                                }
                                using (FileStream streamWriter = File.Create(fullZipToPath))
                                {
                                    Console.WriteLine($"Unpacking {entryFileName}");
                                    await zipStream.CopyToAsync(streamWriter, 4096);
                                }
                            }
                            return Path.Combine(moduleDirectory.FullName, packageName);
                        }
                    }
                }
            }
        }

        public async Task<bool> VerifyPackage()
        {
            this.PackageStream.Position = 0;
            using (ZipFile file = new ZipFile(this.PackageStream))
            {
                file.IsStreamOwner = false;
                var signatureEntry = file.GetEntry("signature");
                var signatureEntryStream = file.GetInputStream(signatureEntry);
                MemoryStream signatureStream = new MemoryStream();
                await signatureEntryStream.CopyToAsync(signatureStream);
                byte[] signature = signatureStream.ToArray();
                signatureStream.Close();
                signatureEntryStream.Close();
                using (Stream contents = file.GetInputStream(file.GetEntry("contents")))
                {
                    using (FileStream tempContents = File.Create(Path.GetTempFileName(), 4096, FileOptions.DeleteOnClose))
                    {
                        await contents.CopyToAsync(tempContents);
                        using (ZipFile contentArchive = new ZipFile(tempContents))
                        {
                            byte[] hash = this.GetSHA512(file.GetInputStream(file.GetEntry("contents")));
                            var keyEntry = contentArchive.GetEntry("manifest/key");
                            var keyEntryStream = contentArchive.GetInputStream(keyEntry);
                            MemoryStream keyStream = new MemoryStream();
                            await keyEntryStream.CopyToAsync(keyStream);
                            byte[] rsaParamBytes = keyStream.ToArray();
                            keyStream.Close();
                            string keyJson = Encoding.UTF8.GetString(rsaParamBytes);
                            RSAParameters parameters = JsonConvert.DeserializeObject<RSAParameters>(keyJson);
                            using (RSA rsa = RSA.Create(parameters))
                            {
                                return rsa.VerifyHash(hash, signature, HashAlgorithmName.SHA512, RSASignaturePadding.Pss);
                            }
                        }
                    }

                }
            }
        }
    }
}
