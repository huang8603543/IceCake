using System.Collections.Generic;

namespace IceCake.Core
{
    public class IndexedDict<TKey, TValue>
    {
        public Dict<TKey, TValue> Dict
        {
            get;
        }

        public List<TKey> Keys
        {
            get;
        }

        public IndexedDict()
        {
            Dict = new Dict<TKey, TValue>();
            Keys = new List<TKey>();
        }

        public void Add(TKey key, TValue value)
        {
            Dict.Add(key, value);
            Keys.Add(key);
        }

        public int Count
        {
            get
            {
                return Dict.Count;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return Dict[key];
            }
            set
            {
                Dict[key] = value;
            }
        }
    }
}