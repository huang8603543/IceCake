using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace IceCake.Core
{
    public class TypeSearchCore : TSingleton<TypeSearchCore>
    {
        protected Hashtable searchTypes = new Hashtable();

        private TypeSearchCore()
        {
            var typeSearchSubClasses = new List<KeyValuePair<Type, Type>>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(TypeSearchBase).IsAssignableFrom(type) &&
                       typeof(TypeSearchFull<,>) != type &&
                       typeof(TypeSearchDefault<>) != type &&
                       typeof(TypeSearchBase) != type)
                    {
                        var searchType = GetNonPublicField<Type>(type.SearchBaseTo(typeof(TypeSearchBase)), "_type");
                        var ignoreType = GetNonPublicField<Type>(type.SearchBaseTo(typeof(TypeSearchBase)), "_ignoreAttributeType");
                        if (null != searchType && ignoreType != null)
                            typeSearchSubClasses.Add(new KeyValuePair<Type, Type>(searchType as Type, ignoreType));
                    }
                }               
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var typeSearchSunClass in typeSearchSubClasses)
                    {
                        if (typeSearchSunClass.Key.IsAssignableFrom(type) && !type.IsApplyAttr(typeSearchSunClass.Value, true))
                        {
                            ReceiveTypeList(typeSearchSunClass.Key).Add(type);
                        }
                    }
                }
            }
        }

        public List<Type> GetSubClasses(Type type)
        {
            if (!searchTypes.ContainsKey(type))
                return new List<Type>();

            return (List<Type>)searchTypes[type];
        }

        protected T GetNonPublicField<T>(Type type, string name, T _default = default(T))
        {
            var fieldInfo = type.GetField(name, BindingFlags.Static | BindingFlags.NonPublic);
            if (fieldInfo == null)
                return _default;

            return (T)fieldInfo.GetValue(null);
        }

        protected List<Type> ReceiveTypeList(Type type)
        {
            if (searchTypes.ContainsKey(type))
                searchTypes.Add(type, new List<Type>());

            return (List<Type>)searchTypes[type];
        }
    }
}
