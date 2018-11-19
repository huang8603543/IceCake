using System.Collections;
using System.Collections.Generic;

namespace IceCake.Core
{
    public class Dict<TKey, TValue> : IEnumerable<CustomKeyValuePair<TKey, TValue>>, IDict
    {
        public class Enumerator : IEnumerator<CustomKeyValuePair<TKey, TValue>>
        {
            Dictionary<object, object>.Enumerator current;

            public Enumerator(Dictionary<object, object>.Enumerator enumerator)
            {
                current = enumerator;
            }

            public CustomKeyValuePair<TKey, TValue> Current
            {

                get
                {
                    return new CustomKeyValuePair<TKey, TValue>((TKey)current.Current.Key, (TValue)current.Current.Value);
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return current;
                }
            }

            public bool MoveNext()
            {
                return current.MoveNext();
            }

            public void Reset()
            {
                throw new System.NotImplementedException();
            }

            public void Dispose()
            {
                current.Dispose();
            }
        }

        public Dictionary<object, object> collection = new Dictionary<object, object>();

        public IEnumerator<CustomKeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(collection.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void AddObject(object key, object value)
        {
            collection.Add(key, value);
        }

        public void Add(TKey key, TValue value)
        {
            collection.Add((object)key, (object)value);
        }

        public int Count
        {
            get
            {
                return collection.Count;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (collection.ContainsKey((object)key))
                    return (TValue)collection[(object)key];
                return default(TValue);
            }
            set
            {
                collection[(object)key] = (object)value;
            }
        }

        public Dictionary<object, object> OriginCollection
        {
            get
            {
                return collection;
            }
        }
    }
}
