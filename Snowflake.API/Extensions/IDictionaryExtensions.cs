using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        public static ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(
    this IDictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }
}