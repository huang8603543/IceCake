using System;
using System.Collections.Generic;
using System.Linq;

namespace IceCake.Core
{
    public static class DictExtension
    {
        public static bool TryGetValue<TKey, TValue>(this Dict<TKey, TValue> dict, TKey key, out TValue value)
        {
            object result = null;
            if (dict.collection.TryGetValue((object)key, out result))
            {
                value = (TValue)result;
                return true;
            }
            value = default(TValue);
            return false;
        }

        public static bool ContainsKey<TKey, TValue>(this Dict<TKey, TValue> dict, TKey key)
        {
            return dict.collection.ContainsKey((object)key);
        }

        public static bool ContainValue<TKey, TValue>(this Dict<TKey, TValue> dict, TValue value)
        {
            return dict.collection.ContainsValue((object)value);
        }

        public static TKey LastKey<TKey, TValue>(this Dict<TKey, TValue> dict)
        {
            return (TKey)dict.collection.Keys.Last();
        }

        public static TValue LastValue<TKey, TValue>(this Dict<TKey, TValue> dict)
        {
            return (TValue)dict.collection.Values.Last();
        }

        public static TKey FirstKey<TKey, TValue>(this Dict<TKey, TValue> dict)
        {
            return (TKey)dict.collection.Keys.First();
        }

        public static TValue FirstValue<TKey, TValue>(this Dict<TKey, TValue> dict)
        {
            return (TValue)dict.collection.Values.First();
        }

        public static CustomKeyValuePair<TKey, TValue> First<TKey, TValue>(this Dict<TKey, TValue> dict)
        {
            if (dict.Count == 0) return null;
            return new CustomKeyValuePair<TKey, TValue>(dict.FirstKey(), dict.FirstValue());
        }

        public static CustomKeyValuePair<TKey, TValue> Last<TKey, TValue>(this Dict<TKey, TValue> dict)
        {
            if (dict.Count == 0) return null;

            return new CustomKeyValuePair<TKey, TValue>(dict.LastKey(), dict.LastValue());
        }

        public static void Clear<TKey, TValue>(this Dict<TKey, TValue> dict)
        {
            dict.collection.Clear();
        }

        public static bool Remove<TKey, TValue>(this Dict<TKey, TValue> dict, TKey key)
        {
            return dict.collection.Remove((object)key);
        }

        public static void RemoveLast<TKey, TValue>(this Dict<TKey, TValue> dict)
        {
            dict.collection.Remove(dict.LastKey());
        }

        public static void RemoveFirst<TKey, TValue>(this Dict<TKey, TValue> dict)
        {
            dict.collection.Remove(dict.FirstKey());
        }

        public static Dict<TKey, TValue> Clone<TKey, TValue>(this Dict<TKey, TValue> dict)
        {
            var newDict = new Dict<TKey, TValue>();
            foreach (var item in dict)
            {
                newDict.Add((TKey)item.Key, (TValue)item.Value);
            }
            return newDict;
        }

        public static Dict<TKey, TValue> Sort<TKey, TValue>(this Dict<TKey, TValue> dict, Comparison<CustomKeyValuePair<TKey, TValue>> cmpAlgo)
        {
            var list = new List<CustomKeyValuePair<TKey, TValue>>();
            foreach (var item in dict)
            {
                list.Add(item);
            }

            list.Sort(cmpAlgo);

            dict.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                dict.Add(list[i].Key, list[i].Value);
            }

            return dict;
        }
    }
}
