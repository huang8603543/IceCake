using System;

namespace IceCake.Core
{
    public static class TypeExtension
    {
        public static Type SearchBaseTo(this Type type, Type baseType)
        {
            var searchType = type;
            while (searchType.BaseType != baseType && searchType.BaseType != null)
                searchType = searchType.BaseType;

            return searchType.BaseType == baseType ? searchType : null;
        }

        public static Type SearchBaseTo<T>(this Type type)
        {
            return SearchBaseTo(type, typeof(T));
        }
    }
}
