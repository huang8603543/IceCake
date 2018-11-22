using System;
using System.Collections.Generic;

namespace IceCake.Core
{
    public class TypeSearchFull<TSearchType, TIgnoreType> : TypeSearchBase where TIgnoreType : Attribute
    {
        static List<Type> types;
        static List<string> typeFullNames;
        static List<string> typeNames;

        static Type _type = typeof(TSearchType);
        static Type _ignoreAttributeType = typeof(TIgnoreType);

        public static List<Type> Types
        {
            get
            {
                if (types == null)
                    types = TypeSearchCore.Instance.GetSubClasses(_type);
                return types;
            }
        }

        public static List<string> TypeFullNames
        {
            get
            {
                if (typeFullNames == null)
                {
                    typeFullNames = new List<string>();
                    foreach (var type in types)
                        typeFullNames.Add(type.FullName);
                }
                return typeFullNames;
            }
        }

        public static List<string> TypeNames
        {
            get
            {
                if (typeNames == null)
                {
                    typeNames = new List<string>();
                    foreach (var type in types)
                        typeNames.Add(type.Name);
                }
                return typeNames;
            }
        }
    }
}
