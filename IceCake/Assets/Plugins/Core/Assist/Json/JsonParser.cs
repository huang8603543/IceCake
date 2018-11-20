using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace IceCake.Core.Json
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class JsonIgnoreAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class JsonEnableAttribute : Attribute { }

    public class JsonParser
    {
        public enum JsonSymbolType
        {
            Unknown = 0,    // 未知
            ObjStart,       // '{'
            ObjEnd,         // '}'
            ArrayStart,     // '['
            ArrayEnd,       // ']'
            ObjSplit,       // ','
            ElementSplit,   // ':'
            Key,            // 名字：字符串
            Value,          // 值类型：整数、实数、字符串、true、false、null
            Element,        // 元素
        }

        public class JsonSymbolItem
        {
            public string value;
            public JsonSymbolType type;
            public JsonNode node;

            public JsonSymbolItem() { }
            public JsonSymbolItem(JsonSymbolItem item)
            {
                value = item.value;
                type = item.type;
                node = item.node;
            }
        }

        public string originData;

        public bool isValid;

        public JsonParser(string originData)
        {
            this.originData = originData.Trim();
            isValid = true;
        }

        public static JsonNode Parse(string jsonStr)
        {
            JsonParser rJsonParser = new JsonParser(jsonStr);
            return rJsonParser.Parser();
        }

        /// <summary>
        /// 将json字符串归一化，将有用的信息保留，让其更加紧凑
        /// </summary>
        public string PretreatmentProc()
        {
            string temp = string.Empty;

            int i = 0;
            int end = 0;
            while (i < originData.Length)
            {
                //跳过那些无用的字符 ' ', '\t', '\r' '\n'
                if (!string.IsNullOrEmpty(LexicalAnalysis.isSpecialSymbol(originData, i, ref end)))
                {
                    i = end;
                    continue;
                }

                //跳过注释
                if (!string.IsNullOrEmpty(/*tempWord = */LexicalAnalysis.isComment(originData, i, ref end)))
                {
                    //Debug.Log(tempWord);
                    i = end;
                    continue;
                }
                temp += originData[i];
                i++;
            }

            return temp;
        }

        public JsonNode Parser()
        {
            isValid = true;
            int end = 0;
            int i = 0;

            JsonSymbolItem curSymbol = null;
            JsonSymbolItem lastSymbol = null;

            Stack<JsonSymbolItem> nodeStack = new Stack<JsonSymbolItem>();
            while (i < originData.Length)
            {
                //跳过那些无用的字符 ' ', '\t', '\r' '\n' 注释
                if (!string.IsNullOrEmpty(LexicalAnalysis.isSpecialSymbol(originData, i, ref end)) || !string.IsNullOrEmpty(LexicalAnalysis.isComment(originData, i, ref end)))
                {
                    i = end;
                    continue;
                }

                curSymbol = BuildSymbolItem(lastSymbol, i, ref end);
                if (curSymbol != null)
                {
                    switch (curSymbol.type)
                    {
                        case JsonSymbolType.Unknown:
                            Debug.LogError("Json format error.");
                            break;
                        case JsonSymbolType.ObjStart:
                            curSymbol.node = new JsonClass();
                            nodeStack.Push(curSymbol);
                            break;
                        case JsonSymbolType.ObjEnd:
                            JsonNode object0 = new JsonClass();
                            while (nodeStack.Count != 0 && nodeStack.Peek().type != JsonSymbolType.ObjStart)
                            {
                                var topSymbol = nodeStack.Pop();
                                if (topSymbol.type == JsonSymbolType.ObjSplit)
                                {
                                    continue;
                                }
                                else if (topSymbol.type == JsonSymbolType.Element)
                                {
                                    object0.AddHead(topSymbol.node.Key, topSymbol.node[topSymbol.node.Key]);
                                }
                            }
                            nodeStack.Pop();
                            var symbol0 = new JsonSymbolItem();
                            symbol0.type = JsonSymbolType.Value;
                            symbol0.node = object0;
                            symbol0.value = object0.ToString();
                            GenerateElementSymbol(ref nodeStack, symbol0);
                            break;
                        case JsonSymbolType.ArrayStart:
                            curSymbol.node = new JsonArray();
                            nodeStack.Push(curSymbol);
                            break;
                        case JsonSymbolType.ArrayEnd:
                            JsonNode array = new JsonArray();
                            while (nodeStack.Peek().type != JsonSymbolType.ArrayStart)
                            {
                                var topSymbol = nodeStack.Pop();
                                if (topSymbol.type == JsonSymbolType.ObjSplit)
                                {
                                    continue;
                                }
                                else if (topSymbol.type == JsonSymbolType.Element)
                                {
                                    array.AddHead(topSymbol.node);
                                }
                            }
                            nodeStack.Pop();
                            var symbol = new JsonSymbolItem();
                            symbol.type = JsonSymbolType.Value;
                            symbol.node = array;
                            symbol.value = array.ToString();
                            GenerateElementSymbol(ref nodeStack, symbol);
                            break;
                        case JsonSymbolType.ObjSplit:
                            nodeStack.Push(curSymbol);
                            break;
                        case JsonSymbolType.ElementSplit:
                            nodeStack.Push(curSymbol);
                            break;
                        case JsonSymbolType.Key:
                            nodeStack.Push(curSymbol);
                            break;
                        case JsonSymbolType.Value:
                            GenerateElementSymbol(ref nodeStack, curSymbol);
                            break;
                        default:
                            break;
                    }
                    i = end;
                    lastSymbol = curSymbol;
                    continue;
                }
                i++;
            }
            return nodeStack.Peek().node;
        }

        private void GenerateElementSymbol(ref Stack<JsonSymbolItem> nodeStack, JsonSymbolItem curSymbol)
        {
            if (nodeStack.Count == 0)
            {
                nodeStack.Push(curSymbol);
                return;
            }

            var symbol1 = nodeStack.Pop();
            var symbol4 = new JsonSymbolItem();
            if (symbol1.type == JsonSymbolType.ObjSplit || symbol1.type == JsonSymbolType.ArrayStart)
            {
                nodeStack.Push(symbol1);
                symbol4.type = JsonSymbolType.Element;
                symbol4.node = curSymbol.node;
                symbol4.value = symbol4.node.ToString();
                nodeStack.Push(symbol4);
            }
            else if (symbol1.type == JsonSymbolType.ElementSplit)
            {
                var symbol2 = nodeStack.Count == 0 ? null : nodeStack.Pop();
                if (symbol2 != null && symbol2.type == JsonSymbolType.Key)
                {
                    symbol4.type = JsonSymbolType.Element;
                    symbol4.node = new JsonClass();
                    symbol4.node.Add(symbol2.value, curSymbol.node);
                    symbol4.value = symbol4.node.ToString();
                    nodeStack.Push(symbol4);
                }
            }
            else
            {
                Debug.LogError("Json grammar error!");
            }
        }

        private JsonSymbolItem BuildSymbolItem(JsonSymbolItem lastSymbol, int begin, ref int end)
        {
            if (originData[begin] == '{')
            {
                end = begin + 1;
                return new JsonSymbolItem() { value = "{", type = JsonSymbolType.ObjStart };
            }
            else if (originData[begin] == '}')
            {
                end = begin + 1;
                return new JsonSymbolItem() { value = "}", type = JsonSymbolType.ObjEnd };
            }
            else if (originData[begin] == '[')
            {
                end = begin + 1;
                return new JsonSymbolItem() { value = "[", type = JsonSymbolType.ArrayStart };
            }
            else if (originData[begin] == ']')
            {
                end = begin + 1;
                return new JsonSymbolItem() { value = "]", type = JsonSymbolType.ArrayEnd };
            }
            else if (originData[begin] == ',')
            {
                end = begin + 1;
                return new JsonSymbolItem() { value = ",", type = JsonSymbolType.ObjSplit };
            }
            else if (originData[begin] == ':')
            {
                end = begin + 1;
                return new JsonSymbolItem() { value = ":", type = JsonSymbolType.ElementSplit };
            }

            string tempWord = "";
            //如果是关键字、数字或者字符串
            if (!string.IsNullOrEmpty(tempWord = LexicalAnalysis.isKeyword(originData, begin, ref end)))
            {
                JsonSymbolItem symbol = new JsonSymbolItem() { value = tempWord, type = JsonSymbolType.Value, node = new JsonData(tempWord) };
                LexicalAnalysis.isSpecialSymbol(originData, end, ref end);
                if (originData[end] == ':')
                {
                    symbol.type = JsonSymbolType.Key;
                    symbol.node = null;
                }
                return symbol;
            }
            if (!string.IsNullOrEmpty(tempWord = LexicalAnalysis.isDigit(originData, begin, ref end)))
            {
                JsonSymbolItem symbol = new JsonSymbolItem() { value = tempWord, type = JsonSymbolType.Value, node = new JsonData(tempWord) };
                LexicalAnalysis.isSpecialSymbol(originData, end, ref end);
                if (originData[end] == ':')
                {
                    symbol.type = JsonSymbolType.Key;
                    symbol.node = null;
                }
                return symbol;
            }
            if (!string.IsNullOrEmpty(tempWord = LexicalAnalysis.isString(originData, begin, ref end)))
            {
                tempWord = tempWord.Substring(1, tempWord.Length - 2);
                JsonSymbolItem symbol = new JsonSymbolItem() { value = tempWord, type = JsonSymbolType.Value, node = new JsonData(tempWord) };
                LexicalAnalysis.isSpecialSymbol(originData, end, ref end);
                if (originData[end] == ':')
                {
                    symbol.type = JsonSymbolType.Key;
                    symbol.node = null;
                }
                return symbol;
            }
            //Debug.Log(string.Format("Json parse symbol item error! LastSymbol = {0}",
            //               rLastSymbol != null ? rLastSymbol.value : "null"));
            isValid = false;
            return null;
        }

        public static JsonNode ToJsonNode(object _object)
        {
            Type type = _object.GetType();

            JsonNode rootNode = null;

            //如果是List
            if (type.IsGenericType && typeof(IList).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                rootNode = new JsonArray();
                IList listObj = (IList)_object;
                foreach (var item in listObj)
                {
                    JsonNode node = ToJsonNode(item);
                    rootNode.Add(node);
                }
            }
            else if (type.IsArray) //如果是Array
            {
                rootNode = new JsonArray();
                Array arrayObj = (Array)_object;
                foreach (var item in arrayObj)
                {
                    JsonNode node = ToJsonNode(item);
                    rootNode.Add(node);
                }
            }
            else if (type.IsGenericType && typeof(IDictionary).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                //如果是Dictionary
                rootNode = new JsonClass();
                IDictionary dictObj = (IDictionary)_object;
                foreach (var key in dictObj.Keys)
                {
                    JsonNode valueNode = ToJsonNode(dictObj[key]);
                    rootNode.Add(key.ToString(), valueNode);
                }
            }
            else if (type.IsGenericType && typeof(IDict).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                rootNode = new JsonClass();
                IDict dictObj = (IDict)_object;
                foreach (var item in dictObj.OriginCollection)
                {
                    JsonNode valueNode = ToJsonNode(item.Value);
                    rootNode.Add(item.Key.ToString(), valueNode);
                }
            }
            else if (type.IsClass) //如果是Class，获取Class所有的public的字段和属性
            {
                if (type == typeof(string))
                {
                    rootNode = new JsonData((string)_object);
                }
                else
                {
                    rootNode = new JsonClass();
                    // 所有公共的属性
                    PropertyInfo[] propInfos = type.GetProperties(ReflectionAssist.PublicFlags);
                    for (int i = 0; i < propInfos.Length; i++)
                    {
                        if (propInfos[i].IsDefined(typeof(JsonIgnoreAttribute), false))
                            continue;

                        object valueObj = propInfos[i].GetValue(_object, null);
                        JsonNode valueNode = ToJsonNode(valueObj);
                        rootNode.Add(propInfos[i].Name, valueNode);
                    }
                    // 所有公共的字段
                    FieldInfo[] fieldInfos = type.GetFields(ReflectionAssist.PublicFlags);
                    for (int i = 0; i < fieldInfos.Length; i++)
                    {
                        if (fieldInfos[i].IsDefined(typeof(JsonIgnoreAttribute), false))
                            continue;

                        object valueObj = fieldInfos[i].GetValue(_object);
                        JsonNode valueNode = ToJsonNode(valueObj);
                        rootNode.Add(fieldInfos[i].Name, valueNode);
                    }

                    // 所有预定义的序列化属性的private的字段
                    propInfos = type.GetProperties(ReflectionAssist.NonPublicFlags);
                    for (int i = 0; i < propInfos.Length; i++)
                    {
                        if (!propInfos[i].IsDefined(typeof(JsonEnableAttribute), false))
                            continue;

                        object valueObj = propInfos[i].GetValue(_object, null);
                        JsonNode valueNode = ToJsonNode(valueObj);
                        rootNode.Add(propInfos[i].Name, valueNode);
                    }
                    fieldInfos = type.GetFields(ReflectionAssist.NonPublicFlags);
                    for (int i = 0; i < fieldInfos.Length; i++)
                    {
                        if (!fieldInfos[i].IsDefined(typeof(JsonEnableAttribute), false))
                            continue;

                        object valueObj = fieldInfos[i].GetValue(_object);
                        JsonNode valueNode = ToJsonNode(valueObj);
                        rootNode.Add(fieldInfos[i].Name, valueNode);
                    }
                }
            }
            else if (type.IsPrimitive) //如果是实例
            {
                if (type == typeof(int))
                    rootNode = new JsonData((int)_object);
                else if (type == typeof(uint))
                    rootNode = new JsonData((uint)_object);
                else if (type == typeof(long))
                    rootNode = new JsonData((long)_object);
                else if (type == typeof(ulong))
                    rootNode = new JsonData((ulong)_object);
                else if (type == typeof(float))
                    rootNode = new JsonData((float)_object);
                else if (type == typeof(double))
                    rootNode = new JsonData((double)_object);
                else if (type == typeof(bool))
                    rootNode = new JsonData((bool)_object);
                else if (type == typeof(string))
                    rootNode = new JsonData((string)_object);
                else if (type == typeof(byte))
                    rootNode = new JsonData((byte)_object);
                else if (type == typeof(short))
                    rootNode = new JsonData((short)_object);
                else if (type == typeof(ushort))
                    rootNode = new JsonData((ushort)_object);
                else
                    Debug.LogError(string.Format("Type = {0}, 不支持序列化的变量类型!", _object.GetType()));
            }
            return rootNode;
        }

        public static object ToObject(JsonNode jsonNode, Type type)
        {
            return jsonNode.ToObject(type);
        }

        public static object ToList(JsonNode jsonNode, Type type, Type elemType)
        {
            return jsonNode.ToList(type, elemType);
        }

        public static object ToDict(JsonNode jsonNode, Type dictType, Type keyType, Type valueType)
        {
            return jsonNode.ToDict(dictType, keyType, valueType);
        }
    }
}