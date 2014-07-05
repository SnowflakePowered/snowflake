using System.Collections.Generic;
// from https://github.com/Athari/Alba.Framework
namespace Snowflake.API.Collections
{
    public static class KeyValuePairExts
    {
        public static KeyValuePair<TValue, TKey> Reverse<TKey, TValue>(this KeyValuePair<TKey, TValue> @this)
        {
            return new KeyValuePair<TValue, TKey>(@this.Value, @this.Key);
        }
    }
}