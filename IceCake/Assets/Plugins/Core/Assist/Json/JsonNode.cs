using System;
using System.Collections.Generic;
using UnityEngine;

namespace IceCake.Core.Json
{
    public class JsonNode
    {
        public virtual string Value { get; set; }
        public virtual string Key { get; set; }
        public virtual int Count { get; private set; }

        public virtual JsonNode this[int index] { get { return null; } set { } }
        public virtual JsonNode this[string key] { get { return null; } set { } }
        public virtual JsonNode Node { get; set; }

        public virtual void Add(string key, JsonNode item) { }
        public virtual void Add(JsonNode item) { }

        public virtual void AddHead(string key, JsonNode item) { }
        public virtual void AddHead(JsonNode item) { }

        public virtual JsonNode Remove(string key) { return null; }
        public virtual JsonNode Remove(int index) { return null; }
        public virtual JsonNode Remove(JsonNode node) { return node; }

        public virtual List<string> Keys { get { return new List<string>(); } }
        public virtual bool ContainsKey(string key) { return false; }

        public override string ToString() { return base.ToString(); }
        public virtual object ToObject(Type type) { return null; }
        public T ToObject<T>() { return (T)ToObject(typeof(T)); }
        public List<T> ToList<T>() { return (List<T>)ToObject(typeof(List<T>)); }
        public T[] ToArray<T>() { return (T[])ToObject(typeof(T[])); }

        public virtual object ToList(Type listType, Type elemType) { return null; }
        public virtual object ToDict(Type dictType, Type keyType, Type valueType) { return null; }

        public virtual bool TryGetValue(string key, out JsonNode value)
        {
            value = null;
            return false;
        }

        public Dict<TKey, TValue> ToDict<TKey, TValue>()
        {
            return (Dict<TKey, TValue>)ToObject(typeof(Dict<TKey, TValue>));
        }
        public Dictionary<TKey, TValue> ToDictionary<TKey, TValue>()
        {
            return (Dictionary<TKey, TValue>)ToObject(typeof(Dictionary<TKey, TValue>));
        }

        public virtual byte AsByte
        {
            get { return CastByte(Value); }
            set { Value = value.ToString(); }
        }

        public virtual short AsShort
        {
            get { return CastShort(Value); }
            set { Value = value.ToString(); }
        }

        public virtual ushort AsUShort
        {
            get { return CastUShort(Value); }
            set { Value = value.ToString(); }
        }

        public virtual int AsInt
        {
            get { return CastInt(Value); }
            set { Value = value.ToString(); }
        }

        public virtual uint AsUint
        {
            get { return CastUInt(Value); }
            set { Value = value.ToString(); }
        }

        public virtual long AsLong
        {
            get { return CastLong(Value); }
            set { Value = value.ToString(); }
        }

        public virtual ulong AsUlong
        {
            get { return CastULong(Value); }
            set { Value = value.ToString(); }
        }

        public virtual float AsFloat
        {
            get { return CastFloat(Value); }
            set { Value = value.ToString(); }
        }

        public virtual double AsDouble
        {
            get { return CastDouble(Value); }
            set { Value = value.ToString(); }
        }

        public virtual bool AsBool
        {
            get { return CastBool(Value); }
            set { Value = value.ToString(); }
        }

        public virtual string AsString
        {
            get { return Value; }
            set { Value = value; }
        }

        public byte CastByte(string value)
        {
            byte re = 0;
            if (byte.TryParse(value, out re)) return re;
            Debug.LogError(string.Format("Value: {0} is not byte type.", value));
            return re;
        }

        public short CastShort(string value)
        {
            short re = 0;
            if (short.TryParse(value, out re)) return re;
            Debug.LogError(string.Format("Value: {0} is not short type.", value));
            return re;
        }

        public ushort CastUShort(string value)
        {
            ushort re = 0;
            if (ushort.TryParse(value, out re)) return re;
            Debug.LogError(string.Format("Value: {0} is not ushort type.", value));
            return re;
        }

        public int CastInt(string value)
        {
            int re = 0;
            if (int.TryParse(value, out re)) return re;
            Debug.LogError(string.Format("Value: {0} is not int type.", value));
            return re;
        }

        public uint CastUInt(string value)
        {
            uint re = 0;
            if (uint.TryParse(value, out re)) return re;
            Debug.LogError(string.Format("Value: {0} is not int type.", value));
            return re;
        }

        public long CastLong(string value)
        {
            long re = 0;
            if (long.TryParse(value, out re)) return re;
            Debug.LogError(string.Format("Value: {0} is not int type.", value));
            return re;
        }

        public ulong CastULong(string value)
        {
            ulong re = 0;
            if (ulong.TryParse(value, out re)) return re;
            Debug.LogError(string.Format("Value: {0} is not int type.", value));
            return re;
        }

        public float CastFloat(string value)
        {
            float re = 0;
            if (float.TryParse(value, out re)) return re;
            Debug.LogError(string.Format("Value: {0} is not int type.", value));
            return re;
        }

        public double CastDouble(string value)
        {
            double re = 0;
            if (double.TryParse(value, out re)) return re;
            Debug.LogError(string.Format("Value: {0} is not int type.", value));
            return re;
        }

        public bool CastBool(string value)
        {
            if (value.ToLower() == "false" || value.ToLower() == "true")
                return value.ToLower() == "false" ? false : true;
            else
                return CastInt(value) == 0 ? false : true;
        }

        public object CastEnum(Type type, string value)
        {
            //如果enum是数字，那么返回数字
            int re = 0;
            if (int.TryParse(value, out re)) return re;

            //如果不是数字，而是字符串，直接转换为enum
            return Enum.Parse(type, value, true);
        }
    }
}
