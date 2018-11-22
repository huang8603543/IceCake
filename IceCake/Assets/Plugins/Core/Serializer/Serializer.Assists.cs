using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace IceCake.Core.Serializer
{
    public class SerializerAssists
    {
        public static List<MemberInfo> FindSerializeMembers(Type type)
        {
            var memberInfos = new List<MemberInfo>();
            foreach (var memberInfo in type.GetMembers())
            {
                if ((memberInfo.MemberType != MemberTypes.Field &&
                    memberInfo.MemberType != MemberTypes.Property) ||
                    memberInfo.DeclaringType != type)
                    continue;

                if (memberInfo.IsDefined(typeof(SBIgnoreAttribute), false))
                    continue;

                if (memberInfo.MemberType == MemberTypes.Property &&
                    (!(memberInfo as PropertyInfo).CanRead || !(memberInfo as PropertyInfo).CanWrite))
                {
                    Debug.LogFormat("{0}.{1} Skip Serialize!", type.FullName, memberInfo.Name);
                    continue;
                }

                memberInfos.Add(memberInfo);
            }

            foreach (var memberInfo in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if ((memberInfo.MemberType != MemberTypes.Field &&
                    memberInfo.MemberType != MemberTypes.Property) ||
                    memberInfo.DeclaringType != type)
                    continue;

                if (!memberInfo.IsDefined(typeof(SBEnableAttribute), false))
                    continue;

                if (memberInfo.MemberType == MemberTypes.Property &&
                    (!(memberInfo as PropertyInfo).CanRead || !(memberInfo as PropertyInfo).CanWrite))
                {
                    Debug.LogFormat("{0}.{1} Skip Serialize!", type.FullName, memberInfo.Name);
                    continue;
                }

                memberInfos.Add(memberInfo);
            }
            return memberInfos;
        }

        public static string GetClassMemberDummyText(MemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                return (memberInfo as FieldInfo).FieldType.IsEnum ?
                    string.Format("(int)this.{0}", memberInfo.Name) :
                    string.Format("this.{0}", memberInfo.Name);
            }
            else if (memberInfo.MemberType == MemberTypes.Property)
            {
                return (memberInfo as PropertyInfo).PropertyType.IsEnum ?
                    string.Format("(int)this.{0}", memberInfo.Name) :
                    string.Format("this.{0}", memberInfo.Name);
            }

            return string.Empty;
        }

        public static string GetTypeName(Type type)
        {
            if (type == typeof(char)) return "char";
            else if (type == typeof(byte)) return "byte";
            else if (type == typeof(sbyte)) return "sbyte";
            else if (type == typeof(short)) return "short";
            else if (type == typeof(ushort)) return "ushort";
            else if (type == typeof(int)) return "int";
            else if (type == typeof(uint)) return "uint";
            else if (type == typeof(long)) return "long";
            else if (type == typeof(ulong)) return "ulong";
            else if (type == typeof(float)) return "float";
            else if (type == typeof(double)) return "double";
            else if (type == typeof(decimal)) return "decimal";
            else if (type == typeof(string)) return "string";
            else if (type.IsArray)
            {
                return string.Format("{0}[]", GetTypeName(type.GetElementType()));
            }
            else if (type.GetInterface("System.Collections.IList") != null)
            {
                return string.Format("List<{0}>", GetTypeName(type.GetGenericArguments()[0]));
            }
            else if (type.GetInterface("System.Collections.IDictionary") != null)
            {
                return string.Format("Dictionary<{0}, {1}>",
                    GetTypeName(type.GetGenericArguments()[0]),
                    GetTypeName(type.GetGenericArguments()[1]));
            }
            else if (type.GetInterface("IceCake.Core.IDict") != null)
            {
                return string.Format("Dict<{0}, {1}>",
                    GetTypeName(type.GetGenericArguments()[0]),
                    GetTypeName(type.GetGenericArguments()[1]));
            }
            else
            {
                return type.FullName;
            }
        }

        public static object GetDeserializeUnwrap(Type type)
        {
            if (type == typeof(char)) return "default(char)";
            else if (type == typeof(byte)) return "default(byte)";
            else if (type == typeof(sbyte)) return "default(sbyte)";
            else if (type == typeof(short)) return "default(short)";
            else if (type == typeof(ushort)) return "default(ushort0";
            else if (type == typeof(int)) return "default(int)";
            else if (type == typeof(uint)) return "default(uint)";
            else if (type == typeof(long)) return "default(long)";
            else if (type == typeof(ulong)) return "default(ulong)";
            else if (type == typeof(float)) return "default(float)";
            else if (type == typeof(double)) return "default(double)";
            else if (type == typeof(decimal)) return "default(decimal)";
            else if (type == typeof(string)) return "string.Empty";
            else if (type.IsEnum)
            {
                return string.Format("int.MaxValue");
            }
            else
            {
                return string.Format("default({0})", GetTypeName(type));
            }
        }

        public static bool IsBaseType(Type type, bool includeSB = true)
        {
            return
                (type == typeof(char)) || (type == typeof(byte)) || (type == typeof(sbyte)) ||
                (type == typeof(short) || (type == typeof(ushort)) || (type == typeof(int)) ||
                (type == typeof(uint)) || (type == typeof(long)) || (type == typeof(ulong)) ||
                (type == typeof(float)) || (type == typeof(double)) || (type == typeof(decimal)) ||
                (type == typeof(string)) || (includeSB && typeof(SerializerBinary).IsAssignableFrom(type)));
        }

        public static Type GetMemberType(MemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
                return (memberInfo as FieldInfo).FieldType;
            else if (memberInfo.MemberType == MemberTypes.Property)
                return (memberInfo as PropertyInfo).PropertyType;
            return null;
        }

        public static string GetClassMemberTypeText(MemberInfo memberInfo)
        {
            var memberType = GetMemberType(memberInfo);
            if (memberType != null)
                return memberType.IsEnum ? string.Format("({0})", memberType.FullName) : string.Empty;

            return string.Empty;
        }
    }
}
