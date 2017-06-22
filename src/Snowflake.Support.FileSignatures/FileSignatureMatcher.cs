using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Platform;
using Snowflake.Services;
using Snowflake.Romfile;

namespace Snowflake.Romfile.FileSignatures
{
    public class FileSignatureMatcher : IFileSignatureMatcher
    {
        private readonly IList<string> stoneMimetypes;
        private readonly IDictionary<string, IPlatformInfo> platforms;
        private readonly IDictionary<string, IList<IFileSignature>> fileSignatures;

        public FileSignatureMatcher(IStoneProvider provider)
        {
            this.stoneMimetypes = (from platform in provider.Platforms.Values
                                   from fileType in platform.FileTypes
                                   select fileType.Value).ToList();
            this.platforms = provider.Platforms;
            this.fileSignatures = new Dictionary<string, IList<IFileSignature>>();
        }
        public IEnumerable<string> GetPossibleMimetypes(string fileExtension)
            => (from platform in this.platforms.Values
                from mimetype in platform.FileTypes
                where mimetype.Key == fileExtension
                select mimetype.Value);

        public void RegisterFileSignature(string mimetype, IFileSignature fileSignature)
        {
            if (this.fileSignatures.ContainsKey(mimetype))
            {
                this.fileSignatures[mimetype].Add(fileSignature);
            }
            else
            {
                this.fileSignatures[mimetype] = new List<IFileSignature> {fileSignature};
            }

        }

        public IRomFileInfo GetInfo(string fileName, Stream fileContents)
        {
            string fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
            var mimetypes = this.GetPossibleMimetypes(fileExtension);
           
            var sig = (from mimetype in mimetypes
                                where this.fileSignatures.ContainsKey(mimetype)
                                let fsl = this.fileSignatures[mimetype]
                                from fs in fsl
                                where fs.HeaderSignatureMatchesSafe(fileContents)
                                select new { mimetype, fs }).FirstOrDefault();
            return sig == null ? null
                : new RomFileInfo(sig.mimetype, sig.fs.GetSerial(fileContents), sig.fs.GetInternalName(fileContents));
        }
    }

    static class FileSignatureExtensions
    {
        public static bool HeaderSignatureMatchesSafe(this IFileSignature @this, Stream fileContents)
        {
            try
            {
                return @this.HeaderSignatureMatches(fileContents);
            }
            catch
            {
                return false;
            }
        }

    }
}
