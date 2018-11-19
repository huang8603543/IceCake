namespace IceCake.Core
{
    public class CustomKeyValuePair<TKey, TValue>
    {
        public TKey Key
        {
            get;
            set;
        }

        public TValue Value
        {
            get;
            set;
        }

        public CustomKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return $"CustomKeyValuePair<{typeof(TKey)},{typeof(TValue)}>({Key}, {Value})";
        }
    }
}
