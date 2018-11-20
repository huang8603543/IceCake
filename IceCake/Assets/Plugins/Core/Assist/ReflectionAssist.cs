using System.Reflection;
using System;

namespace IceCake.Core
{
    /// <summary>
    /// 用于反射的帮助类
    /// </summary>
    public class ReflectionAssist
    {
        public static readonly BindingFlags CommonFlags = 
            BindingFlags.Instance |                                                                 
            BindingFlags.SetField | 
            BindingFlags.GetField |                                                            
            BindingFlags.GetProperty | 
            BindingFlags.SetProperty;

        public static readonly BindingFlags PublicFlags = 
            CommonFlags | 
            BindingFlags.Public;

        public static readonly BindingFlags NonPublicFlags = 
            CommonFlags | 
            BindingFlags.NonPublic;

        public static readonly BindingFlags AllFlags = 
            CommonFlags | 
            BindingFlags.Public | 
            BindingFlags.NonPublic;

        public static readonly BindingFlags MethodFlags = 
            BindingFlags.InvokeMethod | 
            BindingFlags.Public | 
            BindingFlags.NonPublic;

        public static readonly BindingFlags InstMethodFlags = 
            MethodFlags | 
            BindingFlags.Instance;

        public static readonly BindingFlags StaticMethodFlags = 
            MethodFlags | 
            BindingFlags.Static;

        public static readonly Type[] emptyTypes = new Type[0];

        public static ConstructorInfo GetConstructorInfo(BindingFlags bindFlags, Type type, Type[] types)
        {
            return type.GetConstructor(bindFlags, null, types, null);
        }

        public static object CreateInstance(Type type, BindingFlags bindFlags)
        {
            ConstructorInfo constructorInfo = GetConstructorInfo(bindFlags, type, emptyTypes);
            return constructorInfo.Invoke(null);
        }

        public static object Construct(Type type)
        {
            ConstructorInfo constructorInfo = GetConstructorInfo(AllFlags, type, emptyTypes);
            return constructorInfo.Invoke(null);
        }

        public static object Construct(Type type, Type[] types, params object[] _params)
        {
            ConstructorInfo constructorInfo = GetConstructorInfo(AllFlags, type, types);
            return constructorInfo.Invoke(null, _params);
        }

        public static object GetAttrMember(object _object, string memberName, BindingFlags bindFlags)
        {
            if (_object == null) return null;
            Type type = _object.GetType();
            return type.InvokeMember(memberName, bindFlags, null, _object, new object[] { });
        }

        public static void SetAttrMember(object _object, string memberName, BindingFlags bindFlags, params object[] _params)
        {
            if (_object == null) return;
            Type type = _object.GetType();
            type.InvokeMember(memberName, bindFlags, null, _object, _params);
        }

        public static object MethodMember(object _object, string memberName, BindingFlags bindFlags, params object[] _params)
        {
            if (_object == null) return null;
            Type type = _object.GetType();
            return type.InvokeMember(memberName, bindFlags, null, _object, _params);
        }

        public static object MethodMember(Type type, string memberName, BindingFlags bindFlags, params object[] _params)
        {
            return type.InvokeMember(memberName, bindFlags, null, null, _params);
        }

        public static object TypeConvert(Type type, string valueStr)
        {
            return Convert.ChangeType(valueStr, type);
        }
    }
}

