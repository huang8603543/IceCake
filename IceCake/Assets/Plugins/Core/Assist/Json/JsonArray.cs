using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IceCake.Core.Json
{
    public class JsonArray : JsonNode, IEnumerable
    {
        private List<JsonNode> list = new List<JsonNode>();

        public override JsonNode this[int index]
        {
            get
            {
                if (index >= 0 && index < Count) return list[index];
                Debug.LogError(string.Format("Index out of size limit, Index = {0}, Count = {1}", index, Count));
                return null;
            }
            set
            {
                if (index >= Count)
                    list.Add(value);
                else if (index >= 0 && index < Count)
                    list[index] = value;
            }
        }

        public override int Count { get { return list.Count; } }

        public override void Add(JsonNode item)
        {
            if (!list.Contains(item))
                list.Add(item);
        }

        public override void AddHead(JsonNode item)
        {
            if (!list.Contains(item))
                list.Insert(0, item);
        }

        public override JsonNode Remove(int index)
        {
            if (index < 0 || index >= Count)
                return null;
            JsonNode tmp = list[index];
            list.RemoveAt(index);
            return tmp;
        }

        public override JsonNode Remove(JsonNode node)
        {
            list.Remove(node);
            return node;
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var node in list)
            {
                yield return node;
            }
        }

        public override string ToString()
        {
            string jsonStr = "[";
            for (int i = 0; i < list.Count - 1; i++)
            {
                jsonStr += list[i].ToString();
                jsonStr += ",";
            }
            jsonStr += list.Count == 0 ? "" : list[list.Count - 1].ToString();
            jsonStr += "]";
            return jsonStr;
        }

        public override object ToObject(Type type)
        {
            type = ITypeRedirect.GetRedirectType(type);
            if (type.IsArray)
            {
                Array _object = Array.CreateInstance(type.GetElementType(), Count);
                Type arrayElemType = type.GetElementType();
                for (int i = 0; i < Count; i++)
                {
                    object value = list[i].ToObject(arrayElemType);
                    _object.SetValue(value, i);
                }
                return _object;
            }
            else if (type.IsGenericType && typeof(IList).IsAssignableFrom(type.GetGenericTypeDefinition()))  //是否为泛型
            {
                IList _object = (IList)Activator.CreateInstance(type);
                Type[] argsTypes = type.GetGenericArguments();
                for (int i = 0; i < Count; i++)
                {
                    var elemType = argsTypes[0];
                    object value = list[i].ToObject(elemType);
                    _object.Add(value);
                }
                return _object;
            }
            return null;
        }

        public override object ToList(Type listType, Type elemType)
        {
            var clrType = ITypeRedirect.GetRedirectType(listType);
            if (clrType.IsArray)
            {
                Array _object = Array.CreateInstance(clrType.GetElementType(), Count);
                for (int i = 0; i < Count; i++)
                {
                    object value = list[i].ToObject(elemType);
                    _object.SetValue(value, i);
                }
                return _object;
            }
            else if (clrType.IsGenericType && typeof(IList).IsAssignableFrom(clrType.GetGenericTypeDefinition()))  //是否为泛型
            {
                IList _object = (IList)Activator.CreateInstance(listType);
                for (int i = 0; i < Count; i++)
                {
                    object value = list[i].ToObject(elemType);
                    _object.Add(value);
                }
                return _object;
            }
            return null;
        }
    }
}
