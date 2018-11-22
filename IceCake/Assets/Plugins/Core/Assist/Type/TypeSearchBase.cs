using System.Collections.Generic;
using System;
using System.Reflection;

namespace IceCake.Core
{
    public class TypeSearchBase
    {
        public static List<Type> GetTypes(Type type)
        {
            var propertyInfo = GetStorePropertyInfo(type, "Types");
            return propertyInfo.GetValue(null, null) as List<Type>;
        }

        public static List<string> GetTypeFullNames(Type type)
        {
            var propertyInfo = GetStorePropertyInfo(type, "TypeFullNames");
            return propertyInfo.GetValue(null, null) as List<string>;
        }

        public static List<string> GetTypeNames(Type type)
        {
            var propertyInfo = GetStorePropertyInfo(type, "TypeNames");
            return propertyInfo.GetValue(null, null) as List<string>;
        }

        public static PropertyInfo GetStorePropertyInfo(Type type, string storePropertyName)
        {
            return type.SearchBaseTo<TypeSearchBase>().GetProperty(storePropertyName, BindingFlags.Static | BindingFlags.Public);
        }
    }
}
