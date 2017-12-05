using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Snowflake.Extensions
{
    public static class IDictionaryExtensions
    {
        public static bool ContainsKeyWithValue<TKeyType, TKeyValue>(
            this IDictionary<TKeyType, ValueType> dictionary,
            TKeyType key, ValueType value)
        {
            return dictionary.ContainsKey(key) && dictionary[key].Equals(value);
        }

        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(
    this IDictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }
}