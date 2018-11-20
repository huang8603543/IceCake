using System.Collections.Generic;
using UnityEngine;
using System;

namespace IceCake.Core.Json
{
    public class JsonData : JsonNode
    {
        private string value;

        public JsonData(string v)
        {
            value = v;
        }

        public JsonData(float v)
        {
            AsFloat = v;
        }

        public JsonData(double v)
        {
            AsDouble = v;
        }

        public JsonData(int v)
        {
            AsInt = v;
        }

        public JsonData(uint v)
        {
            AsUint = v;
        }

        public JsonData(long v)
        {
            AsLong = v;
        }

        public JsonData(ulong v)
        {
            AsUlong = v;
        }

        public JsonData(bool v)
        {
            AsBool = v;
        }

        public JsonData(byte v)
        {
            AsByte = v;
        }

        public JsonData(short v)
        {
            AsShort = v;
        }

        public JsonData(ushort v)
        {
            AsUShort = v;
        }

        public override string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public override string ToString()
        {
            return "\"" + value.ToString() + "\"";
        }

        public override List<string> Keys
        {
            get
            {
                var rKeys = new List<string>();
                rKeys.Add(Key);
                return rKeys;
            }
        }

        public override bool ContainsKey(string key)
        {
            return key.Equals(Key);
        }

        public override object ToObject(Type type)
        {
            type = ITypeRedirect.GetRedirectType(type);
            if (type.IsPrimitive)
            {
                if (type == typeof(int))
                {
                    return CastInt(value);
                }
                else if (type == typeof(uint))
                {
                    return CastUInt(value);
                }
                else if (type == typeof(long))
                {
                    return CastLong(value);
                }
                else if (type == typeof(ulong))
                {
                    return CastULong(value);
                }
                else if (type == typeof(float))
                {
                    return CastFloat(value);
                }
                else if (type == typeof(double))
                {
                    return CastDouble(value);
                }
                else if (type == typeof(bool))
                {
                    return CastBool(value);
                }
                else if (type == typeof(byte))
                {
                    return CastByte(value);
                }
                else if (type == typeof(short))
                {
                    return CastShort(value);
                }
                else if (type == typeof(ushort))
                {
                    return CastUShort(value);
                }
            }
            else if (type.IsEnum)
            {
                return CastEnum(type, value);
            }
            else if (type == typeof(string))
            {
                return value;
            }
            Debug.LogErrorFormat("{0}不是基础类型，不能解析成为JsonData !", this.value);
            return value.Trim('"');
        }
    }
}
