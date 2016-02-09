using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.Installation
{

    public class PackageKeyPair
    {
        public string PublicKey { get; }
        public string FullKey { get; }
        public string PackageId { get; }
        public PackageKeyPair(string publicKey, string FullKey, string packageId)
        {
            this.PublicKey = publicKey;
            this.FullKey = FullKey;
            this.PackageId = packageId;
        }
    }
}
