using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Snowflake.MediaStore
{
    public interface IMediaStoreSection
    {
        string SectionName { get; set; }
        [Obsolete("Never access MediaStoreItems directly, instead use IMediaStoreSection indexer")]
        IReadOnlyDictionary<string, string> MediaStoreItems { get; }
        void Add(string key, string value);
        void Remove(string key);
        string this[string key] { get; set; }
    }
}
