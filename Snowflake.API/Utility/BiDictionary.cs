using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
// from https://github.com/Athari/Alba.Framework

namespace Snowflake.Utility
{
    [Serializable]
    public class BiDictionary<TFirst, TSecond> : IDictionary<TFirst, TSecond>, IReadOnlyDictionary<TFirst, TSecond>, IDictionary
    {
        private readonly IDictionary<TFirst, TSecond> _firstToSecond = new Dictionary<TFirst, TSecond>();
        [NonSerialized]
        private readonly IDictionary<TSecond, TFirst> _secondToFirst = new Dictionary<TSecond, TFirst>();
        [NonSerialized]
        private readonly ReverseDictionary _reverseDictionary;

        public BiDictionary()
        {
            this._reverseDictionary = new ReverseDictionary(this);
        }

        public IDictionary<TSecond, TFirst> Reverse
        {
            get { return this._reverseDictionary; }
        }

        public int Count
        {
            get { return this._firstToSecond.Count; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection) this._firstToSecond).SyncRoot; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection) this._firstToSecond).IsSynchronized; }
        }

        bool IDictionary.IsFixedSize
        {
            get { return ((IDictionary) this._firstToSecond).IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return this._firstToSecond.IsReadOnly || this._secondToFirst.IsReadOnly; }
        }

        public TSecond this[TFirst key]
        {
            get { return this._firstToSecond[key]; }
            set
            {
                this._firstToSecond[key] = value;
                this._secondToFirst[value] = key;
            }
        }

        object IDictionary.this[object key]
        {
            get { return ((IDictionary) this._firstToSecond)[key]; }
            set
            {
                ((IDictionary) this._firstToSecond)[key] = value;
                ((IDictionary) this._secondToFirst)[value] = key;
            }
        }

        public ICollection<TFirst> Keys
        {
            get { return this._firstToSecond.Keys; }
        }

        ICollection IDictionary.Keys
        {
            get { return ((IDictionary) this._firstToSecond).Keys; }
        }

        IEnumerable<TFirst> IReadOnlyDictionary<TFirst, TSecond>.Keys
        {
            get { return ((IReadOnlyDictionary<TFirst, TSecond>) this._firstToSecond).Keys; }
        }

        public ICollection<TSecond> Values
        {
            get { return this._firstToSecond.Values; }
        }

        ICollection IDictionary.Values
        {
            get { return ((IDictionary) this._firstToSecond).Values; }
        }

        IEnumerable<TSecond> IReadOnlyDictionary<TFirst, TSecond>.Values
        {
            get { return ((IReadOnlyDictionary<TFirst, TSecond>) this._firstToSecond).Values; }
        }

        public IEnumerator<KeyValuePair<TFirst, TSecond>> GetEnumerator()
        {
            return this._firstToSecond.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary) this._firstToSecond).GetEnumerator();
        }

        public void Add(TFirst key, TSecond value)
        {
            this._firstToSecond.Add(key, value);
            this._secondToFirst.Add(value, key);
        }

        void IDictionary.Add(object key, object value)
        {
            ((IDictionary) this._firstToSecond).Add(key, value);
            ((IDictionary) this._secondToFirst).Add(value, key);
        }

        void ICollection<KeyValuePair<TFirst, TSecond>>.Add(KeyValuePair<TFirst, TSecond> item)
        {
            this._firstToSecond.Add(item);
            this._secondToFirst.Add(item.Reverse());
        }

        public bool ContainsKey(TFirst key)
        {
            return this._firstToSecond.ContainsKey(key);
        }

        bool ICollection<KeyValuePair<TFirst, TSecond>>.Contains(KeyValuePair<TFirst, TSecond> item)
        {
            return this._firstToSecond.Contains(item);
        }

        public bool TryGetValue(TFirst key, out TSecond value)
        {
            return this._firstToSecond.TryGetValue(key, out value);
        }

        public bool Remove(TFirst key)
        {
            TSecond value;
            if (this._firstToSecond.TryGetValue(key, out value))
            {
                this._firstToSecond.Remove(key);
                this._secondToFirst.Remove(value);
                return true;
            }
            else
                return false;
        }

        void IDictionary.Remove(object key)
        {
            var firstToSecond = (IDictionary) this._firstToSecond;
            if (!firstToSecond.Contains(key))
                return;
            var value = firstToSecond[key];
            firstToSecond.Remove(key);
            ((IDictionary) this._secondToFirst).Remove(value);
        }

        bool ICollection<KeyValuePair<TFirst, TSecond>>.Remove(KeyValuePair<TFirst, TSecond> item)
        {
            return this._firstToSecond.Remove(item);
        }

        bool IDictionary.Contains(object key)
        {
            return ((IDictionary) this._firstToSecond).Contains(key);
        }

        public void Clear()
        {
            this._firstToSecond.Clear();
            this._secondToFirst.Clear();
        }

        void ICollection<KeyValuePair<TFirst, TSecond>>.CopyTo(KeyValuePair<TFirst, TSecond>[] array, int arrayIndex)
        {
            this._firstToSecond.CopyTo(array, arrayIndex);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((IDictionary) this._firstToSecond).CopyTo(array, index);
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            this._secondToFirst.Clear();
            foreach (var item in this._firstToSecond)
                this._secondToFirst.Add(item.Value, item.Key);
        }

        private class ReverseDictionary : IDictionary<TSecond, TFirst>, IReadOnlyDictionary<TSecond, TFirst>, IDictionary
        {
            private readonly BiDictionary<TFirst, TSecond> _owner;

            public ReverseDictionary(BiDictionary<TFirst, TSecond> owner)
            {
                this._owner = owner;
            }

            public int Count
            {
                get { return this._owner._secondToFirst.Count; }
            }

            object ICollection.SyncRoot
            {
                get { return ((ICollection) this._owner._secondToFirst).SyncRoot; }
            }

            bool ICollection.IsSynchronized
            {
                get { return ((ICollection) this._owner._secondToFirst).IsSynchronized; }
            }

            bool IDictionary.IsFixedSize
            {
                get { return ((IDictionary) this._owner._secondToFirst).IsFixedSize; }
            }

            public bool IsReadOnly
            {
                get { return this._owner._secondToFirst.IsReadOnly || this._owner._firstToSecond.IsReadOnly; }
            }

            public TFirst this[TSecond key]
            {
                get { return this._owner._secondToFirst[key]; }
                set
                {
                    this._owner._secondToFirst[key] = value;
                    this._owner._firstToSecond[value] = key;
                }
            }

            object IDictionary.this[object key]
            {
                get { return ((IDictionary) this._owner._secondToFirst)[key]; }
                set
                {
                    ((IDictionary) this._owner._secondToFirst)[key] = value;
                    ((IDictionary) this._owner._firstToSecond)[value] = key;
                }
            }

            public ICollection<TSecond> Keys
            {
                get { return this._owner._secondToFirst.Keys; }
            }

            ICollection IDictionary.Keys
            {
                get { return ((IDictionary) this._owner._secondToFirst).Keys; }
            }

            IEnumerable<TSecond> IReadOnlyDictionary<TSecond, TFirst>.Keys
            {
                get { return ((IReadOnlyDictionary<TSecond, TFirst>) this._owner._secondToFirst).Keys; }
            }

            public ICollection<TFirst> Values
            {
                get { return this._owner._secondToFirst.Values; }
            }

            ICollection IDictionary.Values
            {
                get { return ((IDictionary) this._owner._secondToFirst).Values; }
            }

            IEnumerable<TFirst> IReadOnlyDictionary<TSecond, TFirst>.Values
            {
                get { return ((IReadOnlyDictionary<TSecond, TFirst>) this._owner._secondToFirst).Values; }
            }

            public IEnumerator<KeyValuePair<TSecond, TFirst>> GetEnumerator()
            {
                return this._owner._secondToFirst.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            IDictionaryEnumerator IDictionary.GetEnumerator()
            {
                return ((IDictionary) this._owner._secondToFirst).GetEnumerator();
            }

            public void Add(TSecond key, TFirst value)
            {
                this._owner._secondToFirst.Add(key, value);
                this._owner._firstToSecond.Add(value, key);
            }

            void IDictionary.Add(object key, object value)
            {
                ((IDictionary) this._owner._secondToFirst).Add(key, value);
                ((IDictionary) this._owner._firstToSecond).Add(value, key);
            }

            void ICollection<KeyValuePair<TSecond, TFirst>>.Add(KeyValuePair<TSecond, TFirst> item)
            {
                this._owner._secondToFirst.Add(item);
                this._owner._firstToSecond.Add(item.Reverse());
            }

            public bool ContainsKey(TSecond key)
            {
                return this._owner._secondToFirst.ContainsKey(key);
            }

            bool ICollection<KeyValuePair<TSecond, TFirst>>.Contains(KeyValuePair<TSecond, TFirst> item)
            {
                return this._owner._secondToFirst.Contains(item);
            }

            public bool TryGetValue(TSecond key, out TFirst value)
            {
                return this._owner._secondToFirst.TryGetValue(key, out value);
            }

            public bool Remove(TSecond key)
            {
                TFirst value;
                if (this._owner._secondToFirst.TryGetValue(key, out value))
                {
                    this._owner._secondToFirst.Remove(key);
                    this._owner._firstToSecond.Remove(value);
                    return true;
                }
                else
                    return false;
            }

            void IDictionary.Remove(object key)
            {
                var firstToSecond = (IDictionary) this._owner._secondToFirst;
                if (!firstToSecond.Contains(key))
                    return;
                var value = firstToSecond[key];
                firstToSecond.Remove(key);
                ((IDictionary) this._owner._firstToSecond).Remove(value);
            }

            bool ICollection<KeyValuePair<TSecond, TFirst>>.Remove(KeyValuePair<TSecond, TFirst> item)
            {
                return this._owner._secondToFirst.Remove(item);
            }

            bool IDictionary.Contains(object key)
            {
                return ((IDictionary) this._owner._secondToFirst).Contains(key);
            }

            public void Clear()
            {
                this._owner._secondToFirst.Clear();
                this._owner._firstToSecond.Clear();
            }

            void ICollection<KeyValuePair<TSecond, TFirst>>.CopyTo(KeyValuePair<TSecond, TFirst>[] array, int arrayIndex)
            {
                this._owner._secondToFirst.CopyTo(array, arrayIndex);
            }

            void ICollection.CopyTo(Array array, int index)
            {
                ((IDictionary) this._owner._secondToFirst).CopyTo(array, index);
            }
        }
    }

    internal static class KeyValuePairExts
    {
        public static KeyValuePair<TValue, TKey> Reverse<TKey, TValue>(this KeyValuePair<TKey, TValue> @this)
        {
            return new KeyValuePair<TValue, TKey>(@this.Value, @this.Key);
        }
    }
}