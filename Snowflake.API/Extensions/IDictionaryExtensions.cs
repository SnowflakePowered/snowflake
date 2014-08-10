using System;
using System.Collections.Generic;

namespace Snowflake.Extensions
{
    public static class IDictionaryExtensions
    {
        public static bool ContainsKeyWithValue<KeyType, KeyValue>(
            this IDictionary<KeyType, ValueType> Dictionary,
            KeyType Key, ValueType Value)
        {
            return (Dictionary.ContainsKey(Key) && Dictionary[Key].Equals(Value));
        }
    }
}