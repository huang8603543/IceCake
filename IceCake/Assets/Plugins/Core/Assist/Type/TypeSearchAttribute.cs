using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace IceCake.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TSIgnoreAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class TSIgnoreInheritedAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public class TypeSearchAttribute :Attribute
    {
        public Type TypeSearchType
        {
            get;
            private set;
        }

        public TypeSearchAttribute(Type typeSearch)
        {
            TypeSearchType = typeSearch;
        }

        //public List<string> TypeFullNames
        //{
        //    get
        //    {

        //    }
        //}
    }
}
