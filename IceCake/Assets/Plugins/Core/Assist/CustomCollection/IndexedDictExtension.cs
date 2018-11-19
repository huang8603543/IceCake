namespace IceCake.Core
{
    public static class IndexedDictExtension
    {
        public static bool TryGetValue<TKey, TValue>(this IndexedDict<TKey, TValue> indexedDict, TKey key, TValue value)
        {
            return indexedDict.Dict.TryGetValue(key, out value);
        }

        public static TKey LastKey<TKey, TValue>(this IndexedDict<TKey, TValue> indexedDict)
        {
            return indexedDict.Dict.LastKey();
        }

        public static TValue LastValue<TKey, TValue>(this IndexedDict<TKey, TValue> indexedDict)
        {
            return indexedDict.Dict.LastValue();
        }

        public static TKey FirstKey<TKey, TValue>(this IndexedDict<TKey, TValue> indexedDict)
        {
            return indexedDict.Dict.FirstKey();
        }

        public static TValue FirstValue<TKey, TValue>(this IndexedDict<TKey, TValue> indexedDict)
        {
            return indexedDict.Dict.FirstValue();
        }

        public static CustomKeyValuePair<TKey, TValue> First<TKey, TValue>(this IndexedDict<TKey, TValue> indexedDict)
        {
            if (indexedDict.Count == 0)
                return null;
            return new CustomKeyValuePair<TKey, TValue>(indexedDict.FirstKey(), indexedDict.FirstValue());
        }

        public static CustomKeyValuePair<TKey, TValue> Last<TKey, TValue>(this IndexedDict<TKey, TValue> indexedDict)
        {
            if (indexedDict.Count == 0)
                return null;
            return new CustomKeyValuePair<TKey, TValue>(indexedDict.LastKey(), indexedDict.LastValue());
        }

        public static void Clear<TKey, TValue>(this IndexedDict<TKey, TValue> indexedDict)
        {
            indexedDict.Dict.Clear();
            indexedDict.Keys.Clear();
        }

        public static bool Remove<TKey, TValue>(this IndexedDict<TKey, TValue> indexedDict, TKey key)
        {
            indexedDict.Keys.Remove(key);
            return indexedDict.Dict.Remove(key);
        }
    }
}
