using System.IO;
using System;

namespace IceCake.Core.Serializer
{
    [TSIgnore]
    public class SerializerBinary
    {
        public virtual void Serialize(BinaryWriter writer)
        { }

        public virtual void Deserialize(BinaryReader reader)
        { }
    }

    public class SerializerBinaryTypes : TypeSearchDefault<SerializerBinary>
    { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SBIgnoreAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SBEnableAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SBDynamicAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SBGroupAttribute : Attribute
    {
        public string GroupName;

        public SBGroupAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class SBGroupInheritedAttribute : SBGroupAttribute
    {
        public SBGroupInheritedAttribute(string groupName) : base(groupName)
        { }
    }
}
