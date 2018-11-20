using System.Linq;
using System.Collections.Generic;

namespace IceCake.Core
{
    public class MultiMap<T, K>
    {
        readonly SortedDictionary<T, List<K>> dictionary = new SortedDictionary<T, List<K>>();

        readonly Queue<List<K>> queue = new Queue<List<K>>();

        T firstKey;

        public SortedDictionary<T, List<K>> GetDictionary()
        {
            return dictionary;
        }

        public void Add(T t, K k)
        {
            List<K> list;
            dictionary.TryGetValue(t, out list);
            if (list == null)
                list = FetchList();
            list.Add(k);
            dictionary[t] = list;
        }

        public KeyValuePair<T, List<K>> First()
        {
            return dictionary.First();
        }

        public T FirstKey()
        {
            return dictionary.Keys.First();
        }

        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

        List<K> FetchList()
        {
            if (queue.Count > 0)
            {
                List<K> list = queue.Dequeue();
                list.Clear();
                return list;
            }
            return new List<K>();
        }

        void RecycleList(List<K> list)
        {
            if (queue.Count > 100)
                return;
            list.Clear();
            queue.Enqueue(list);
        }

        public bool Remove(T t, K k)
        {
            List<K> list;
            dictionary.TryGetValue(t, out list);
            if (list == null)
                return false;
            if (!list.Remove(k))
                return false;
            if (list.Count == 0)
            {
                RecycleList(list);
                dictionary.Remove(t);
            }
            return true;
        }

        public bool Remove(T t)
        {
            List<K> list = null;
            dictionary.TryGetValue(t, out list);
            if (list != null)
                RecycleList(list);
            return dictionary.Remove(t);
        }

        public K[] GetAll(T t)
        {
            List<K> list;
            dictionary.TryGetValue(t, out list);
            if (list == null)
                return new K[0];
            return list.ToArray();
        }

        public List<K> this[T t]
        {
            get
            {
                List<K> list;
                dictionary.TryGetValue(t, out list);
                return list;
            }
        }

        public K GetOne(T t)
        {
            List<K> list;
            dictionary.TryGetValue(t, out list);
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            return default(K);
        }

        public bool Contains(T t, K k)
        {
            List<K> list;
            dictionary.TryGetValue(t, out list);
            if (list == null)
                return false;
            return list.Contains(k);
        }

        public bool ContainsKey(T t)
        {
            return dictionary.ContainsKey(t);
        }
    }
}
