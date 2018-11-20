using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace IceCake.Core.Json
{
    public class JsonClass : JsonNode, IEnumerable
    {
        private Dict<string, JsonNode> dict = new Dict<string, JsonNode>();

        public override JsonNode this[string key]
        {
            get
            {
                JsonNode node = null;
                dict.TryGetValue(key, out node);
                return node;
            }
            set
            {
                if (dict.ContainsKey(key))
                    dict[key] = value;
                else
                    dict.Add(key, value);
            }
        }

        public override List<string> Keys
        {
            get
            {
                List<string> keys = new List<string>();
                foreach (var item in dict)
                {
                    keys.Add(item.Key);
                }
                return keys;
            }
        }

        public override string Key
        {
            get
            {
                if (dict.Count == 0) return string.Empty;
                return dict.FirstKey();
            }
            set
            { }
        }

        public override bool ContainsKey(string key)
        {
            return dict.ContainsKey(key);
        }

        public override int Count { get { return dict.Count; } }

        public override void Add(string key, JsonNode item)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (dict.ContainsKey(key))
                    dict[key] = item;
                else
                    dict.Add(key, item);
            }
            else
            {
                Debug.LogError("JsonClass dict cannot Add empty string key.");
            }
        }

        public override void AddHead(string key, JsonNode item)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (dict.ContainsKey(key))
                    dict[key] = item;
                else
                {
                    var tempdict = new Dict<string, JsonNode>();
                    tempdict.Add(key, item);
                    foreach (var pair in dict)
                    {
                        tempdict.Add(pair.Key, pair.Value);
                    }
                    dict = tempdict;
                }
            }
            else
            {
                Debug.LogError("JsonClass dict cannot Add empty string key.");
            }
        }

        public override JsonNode Remove(string key)
        {
            if (!dict.ContainsKey(key)) return null;
            JsonNode rNode = dict[key];
            dict.Remove(key);
            return rNode;
        }

        public override JsonNode Remove(JsonNode node)
        {
            return base.Remove(node);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var item in dict)
                yield return item;
        }

        public override string ToString()
        {
            string jsonStr = "{";
            int i = 0;
            foreach (var item in dict)
            {
                jsonStr += "\"" + item.Key + "\":" + item.Value.ToString();
                if (i < Count - 1) jsonStr += ",";
                i++;
            }
            jsonStr += "}";
            return jsonStr;
        }

        public override object ToObject(Type type)
        {
            type = ITypeRedirect.GetRedirectType(type);
            if (type.IsGenericType && typeof(IDictionary).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                // 特殊处理IDictionary<,>类型
                IDictionary _object = (IDictionary)ReflectionAssist.CreateInstance(type, ReflectionAssist.AllFlags);
                Type[] argsTypes = type.GetGenericArguments();
                foreach (var item in dict)
                {
                    object key = GetKeyByString(argsTypes[0], item.Key);
                    object value = item.Value.ToObject(argsTypes[1]);
                    _object.Add(key, value);
                }
                return _object;
            }
            else if (type.IsGenericType && typeof(IDict).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                // 特殊处理IDict<,>的类型
                IDict _object = (IDict)ReflectionAssist.CreateInstance(type, ReflectionAssist.AllFlags);
                Type[] argsTypes = type.GetGenericArguments();
                foreach (var item in dict)
                {
                    object key = GetKeyByString(argsTypes[0], item.Key);
                    object value = item.Value.ToObject(argsTypes[1]);
                    _object.AddObject(key, value);
                }
                return _object;
            }
            else if (type.IsClass)
            {
                BindingFlags bindFlags = ReflectionAssist.AllFlags;
                object _object = ReflectionAssist.CreateInstance(type, bindFlags);
                foreach (var item in dict)
                {
                    Type memberType = null;
                    FieldInfo fieldInfo = type.GetField(item.Key, bindFlags);
                    if (fieldInfo != null)
                    {
                        memberType = fieldInfo.FieldType;
                        object valueObj = item.Value.ToObject(memberType);
                        fieldInfo.SetValue(_object, valueObj);
                        continue;
                    }
                    PropertyInfo propInfo = type.GetProperty(item.Key, bindFlags);
                    if (propInfo != null)
                    {
                        memberType = propInfo.PropertyType;
                        object rValueObj = item.Value.ToObject(memberType);
                        propInfo.SetValue(_object, rValueObj, null);
                        continue;
                    }
                }
                return _object;
            }
            return null;
        }

        public override bool TryGetValue(string key, out JsonNode value)
        {
            return dict.TryGetValue(key, out value);
        }

        /// <summary>
        /// 转化Key
        /// </summary>
        private object GetKeyByString(Type keyType, string keyStr)
        {
            object key = keyStr;
            if (keyType == typeof(int))
            {
                int intKey = 0;
                int.TryParse(keyStr, out intKey);
                key = intKey;
            }
            else if (keyType == typeof(long))
            {
                long longKey = 0;
                long.TryParse(keyStr, out longKey);
                key = longKey;
            }
            return key;
        }

        public override object ToDict(Type dictType, Type keyType, Type valueType)
        {
            dictType = ITypeRedirect.GetRedirectType(dictType);
            if (dictType.IsGenericType && typeof(IDictionary).IsAssignableFrom(dictType.GetGenericTypeDefinition()))
            {
                // 特殊处理IDictionary<,>类型
                IDictionary _object = (IDictionary)ReflectionAssist.CreateInstance(dictType, BindingFlags.Default);
                foreach (var item in dict)
                {
                    object key = GetKeyByString(keyType, item.Key);
                    object value = item.Value.ToObject(valueType);
                    _object.Add(key, value);
                }
                return _object;
            }
            else if (dictType.IsGenericType && typeof(IDict).IsAssignableFrom(dictType.GetGenericTypeDefinition()))
            {
                // 特殊处理IDict<,>的类型
                IDict _object = (IDict)ReflectionAssist.CreateInstance(dictType, BindingFlags.Default);
                foreach (var item in dict)
                {
                    object key = GetKeyByString(keyType, item.Key);
                    object value = item.Value.ToObject(valueType);
                    _object.AddObject(key, value);
                }
                return _object;
            }
            return null;
        }
    }
}
