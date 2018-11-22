using System;
using System.Collections.Generic;

namespace IceCake.Core.Serializer.Editor
{
    public class CodeGeneratorCommonSerializer : CodeGenerator
    {
        List<Type> generatedArray = new List<Type>();
        List<Type> generatedDynamicArray = new List<Type>();
        List<Type> generatedList = new List<Type>();
        List<Type> generatedDynamicList = new List<Type>();
        List<Type> generatedDictionary = new List<Type>();
        List<Type> generatedDynamicDictionary = new List<Type>();

        public CodeGeneratorCommonSerializer(string filePath) : base(filePath)
        { }

        public override void WriteHead()
        {
            StringBuilder?
                .Append("using System.IO;").Line()
                .Append("using System.Collections.Generic;").Line()
                .Append("using IceCake.Core;").Line()
                .Append("using IceCake.Core.Serializer;").Line()
                .Lines(1)
                .Append("/// <summary>").Line()
                .Append("/// Auto generate code, not modify.").Line()
                .Append("/// </summary>").Line()
                .Append("namespace IceCake.Framework.Serializer").Line()
                .Append("{").Line()
                .Tab(1).Append("public static class CommonSerializer").Line()
                .Tab(1).Append("{").Line();
        }

        public override void WriteEnd()
        {
            StringBuilder?
                .Tab(1).Append("}").Line()
                .Append("}");
        }

        public void AnalyzeGenerateCommon(Type type, bool dynamic)
        {
            if (SerializerAssists.IsBaseType(type))
                return;

            if (type.IsArray)
            {
                WriteArray(type, dynamic);
                AnalyzeGenerateCommon(type.GetElementType(), dynamic);
            }
            else if (type.GetInterface("System.Collections.IList") != null)
            {
                WriteList(type, dynamic);
                AnalyzeGenerateCommon(type.GetGenericArguments()[0], dynamic);
            }
            else if (type.GetInterface("System.Collections.IDictionary") != null || type.GetInterface("IceCake.Core.IDict") != null)
            {
                WriteDictionary(type, dynamic);
                AnalyzeGenerateCommon(type.GetGenericArguments()[0], dynamic);
                AnalyzeGenerateCommon(type.GetGenericArguments()[1], dynamic);
            }
        }

        public void WriteArray(Type type, bool dynamic)
        {
            if (ReceiveGeneratedArrayType(type, dynamic))
                return;

            var typeName = SerializerAssists.GetTypeName(type);
            var elementType = type.GetElementType();

            var tdText = dynamic ? "Dynamic" : string.Empty;
            var tdeText = dynamic && !SerializerAssists.IsBaseType(elementType, false) ? "Dynamic" : string.Empty;

            StringBuilder?
                .Tab(2).Format("public static void Serialize{0}(this BinaryWriter writer, {1} value)", tdText, typeName).Line()
                .Tab(2).Append("{").Line()
                    .Tab(3).Append("var bValid = (null != value);").Line()
                    .Tab(3).Append("writer.Serialize(bValid);").Line()
                    .Tab(3).Append("if (!bValid) return;").Line()
                    .Lines(1)
                    .Tab(3).Append("writer.Serialize(value.Length);").Line()
                    .Tab(3).Append("for (int nIndex = 0; nIndex < value.Length; nIndex++)").Line()
                        .Tab(4).Format("writer.Serialize{0}({1});", tdeText, (elementType.IsEnum ? "(int)value[nIndex]" : "value[nIndex]")).Line()
                .Tab(2).Append("}").Line()
                .Lines(1);

            StringBuilder?
                .Tab(2).Format("public static {0} Deserialize(this BinaryReader reader, {1} value)", typeName, typeName).Line()
                .Tab(2).Append("{").Line()
                    .Tab(3).Append("var bValid = reader.Deserialize(default(bool));").Line()
                    .Tab(3).Append("if (!bValid) return null;").Line()
                    .Lines(1)
                    .Tab(3).Append("var nCount  = reader.Deserialize(default(int));").Line()
                    .Tab(3).Format("var rResult = new {0};", typeName.Insert(typeName.IndexOf('[') + 1, "nCount")).Line()
                    .Tab(3).Append("for (int nIndex = 0; nIndex < nCount; nIndex++)").Line()
                        .Tab(4).Format("rResult[nIndex] = {0}reader.Deserialize{1}({2});",
                            (elementType.IsEnum ? string.Format("({0})", elementType.FullName) : string.Empty),
                            tdeText,
                            SerializerAssists.GetDeserializeUnwrap(elementType)).Line()
                    .Tab(3).Append("return rResult;").Line()
                .Tab(2).Append("}").Line()
                .Lines(1);
        }

        public void WriteList(Type type, bool dynamic)
        {
            if (ReceiveGeneratedListType(type, dynamic))
                return;

            var typeName = SerializerAssists.GetTypeName(type);
            var elementType = type.GetGenericArguments()[0];

            var tdText = dynamic ? "Dynamic" : string.Empty;
            var tdeText = dynamic && !SerializerAssists.IsBaseType(elementType, false) ? "Dynamic" : string.Empty;

            StringBuilder?
                .Tab(2).Format("public static void Serialize{0}(this BinaryWriter writer, {1} value)", tdText, typeName).Line()
                .Tab(2).Append("{").Line()
                    .Tab(3).Append("var bValid = (null != value);").Line()
                    .Tab(3).Append("writer.Serialize(bValid);").Line()
                    .Tab(3).Append("if (!bValid) return;").Line()
                    .Lines(1)
                    .Tab(3).Append("writer.Serialize(value.Count);").Line()
                    .Tab(3).Append("for (int nIndex = 0; nIndex < value.Count; ++ nIndex)").Line()
                        .Tab(4).Format("writer.Serialize{0}({1});", tdeText, (elementType.IsEnum ? "(int)value[nIndex]" : "value[nIndex]")).Line()
                .Tab(2).Append("}").Line()
                .Lines(1);

            StringBuilder?
                .Tab(2).Format("public static {0} Deserialize{1}(this BinaryReader reader, {2} value)", typeName, tdText, typeName).Line()
                .Tab(2).Append("{").Line()
                    .Tab(3).Append("var bValid = reader.Deserialize(default(bool));").Line()
                    .Tab(3).Append("if (!bValid) return null;").Line()
                    .Lines(1)
                    .Tab(3).Append("var nCount  = reader.Deserialize(default(int));").Line()
                    .Tab(3).Format("var rResult = new {0}(nCount);", typeName).Line()
                    .Tab(3).Append("for (int nIndex = 0; nIndex < nCount; nIndex++)").Line()
                        .Tab(4).Format("rResult.Add({0}reader.Deserialize{1}({2}));",
                            (elementType.IsEnum ? string.Format("({0})", elementType.FullName) : string.Empty),
                            tdeText,
                            SerializerAssists.GetDeserializeUnwrap(type.GetGenericArguments()[0])).Line()
                    .Tab(3).Append("return rResult;").Line()
                .Tab(2).Append("}").Line()
                .Lines(1);
        }

        public void WriteDictionary(Type type, bool dynamic)
        {
            if (ReceiveGeneratedDictionaryType(type, dynamic))
                return;
        
            var typeName = SerializerAssists.GetTypeName(type);
            var keyType = type.GetGenericArguments()[0];
            var valueType = type.GetGenericArguments()[1];

            var tdText = dynamic ? "Dynamic" : string.Empty;
            var tdkText = dynamic && !SerializerAssists.IsBaseType(keyType, false) ? "Dynamic" : string.Empty;
            var tdvText = dynamic && !SerializerAssists.IsBaseType(valueType, false) ? "Dynamic" : string.Empty;

            StringBuilder?
                .Tab(2).Format("public static void Serialize{0}(this BinaryWriter writer, {1} value)", tdText, typeName).Line()
                .Tab(2).Append("{").Line()
                    .Tab(3).Append("var bValid = (null != value);").Line()
                    .Tab(3).Append("writer.Serialize(bValid);").Line()
                    .Tab(3).Append("if (!bValid) return;").Line()
                    .Lines(1)
                    .Tab(3).Append("writer.Serialize(value.Count);").Line()
                    .Tab(3).Append("foreach(var rPair in value)").Line()
                    .Tab(3).Append("{").Line()
                        .Tab(4).Format("writer.Serialize{0}({1});", tdkText, (keyType.IsEnum ? "(int)rPair.Key" : "rPair.Key")).Line()
                        .Tab(4).Format("writer.Serialize{0}({1});", tdvText, (valueType.IsEnum ? "(int)rPair.Value" : "rPair.Value")).Line()
                    .Tab(3).Append("}").Line()
                .Tab(2).Append("}").Line()
                .Lines(1);

            StringBuilder?
                .Tab(2).Format("public static {0} Deserialize{1}(this BinaryReader reader, {2} value)", typeName, tdText, typeName).Line()
                .Tab(2).Append("{").Line()
                    .Tab(3).Append("var bValid = reader.Deserialize(default(bool));").Line()
                    .Tab(3).Append("if (!bValid) return null;").Line()
                    .Lines(1)
                    .Tab(3).Append("var nCount  = reader.Deserialize(default(int));").Line()
                    .Tab(3).Format("var rResult = new {0}();", typeName).Line()
                    .Tab(3).Append("for (int nIndex = 0; nIndex < nCount; ++ nIndex)").Line()
                    .Tab(3).Append("{").Line()
                        .Tab(4).Format("var rKey   = {0}reader.Deserialize{1}({2});",
                            (keyType.IsEnum ? string.Format("({0})", keyType.FullName) : string.Empty),
                            tdkText,
                            SerializerAssists.GetDeserializeUnwrap(keyType)).Line()
                        .Tab(4).Format("var rValue = {0}reader.Deserialize{1}({2});",
                            (valueType.IsEnum ? string.Format("({0})", valueType.FullName) : string.Empty),
                            tdvText,
                            SerializerAssists.GetDeserializeUnwrap(valueType)).Line()
                        .Tab(4).Append("rResult.Add(rKey, rValue);").Line()
                    .Tab(3).Append("}").Line()
                    .Tab(3).Append("return rResult;").Line()
                .Tab(2).Append("}").Line()
                .Lines(1);
        }

        bool ReceiveGeneratedArrayType(Type type, bool dynamic)
        {
            return ReceiveType(generatedArray, generatedDynamicArray, type, dynamic);
        }

        bool ReceiveGeneratedListType(Type type, bool dynamic)
        {
            return ReceiveType(generatedList, generatedDynamicList, type, dynamic);
        }

        bool ReceiveGeneratedDictionaryType(Type type, bool dynamic)
        {
            return ReceiveType(generatedDictionary, generatedDynamicDictionary, type, dynamic);
        }

        bool ReceiveType(List<Type> generated, List<Type> generatedDynamic, Type type, bool dynamic)
        {
            if (dynamic)
            {
                if (generatedDynamic.Contains(type))
                    return true;
                generatedDynamic.Add(type);
            }
            else
            {
                if (generated.Contains(type))
                    return true;
                generated.Add(type);
            }
            return false;
        }
    }
}
