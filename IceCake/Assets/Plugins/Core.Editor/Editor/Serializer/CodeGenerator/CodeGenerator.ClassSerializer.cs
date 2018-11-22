using System;

namespace IceCake.Core.Serializer.Editor
{
    public class CodeGeneratorClassSerializer : CodeGenerator
    {
        public string GroupName = string.Empty;

        public CodeGeneratorClassSerializer(string filePath) : base(filePath)
        { }

        public override void WriteHead()
        {
            StringBuilder?
                .Append("using System.IO;").Line()
                .Append("using IceCake.Core;").Line()
                .Append("using IceCake.Core.Serializer;").Line()
                .Append("using IceCake.Framework.Serializer;").Line()
                .Lines(1)
                .Append("/// <summary>").Line()
                .Append("/// Auto generate code, not modify.").Line()
                .Append("/// </summary>").Line();
        }

        public override void WriteEnd()
        {
            Write();
        }

        public void WriteClass(Type type)
        {
            StringBuilder?
                .Format("namespace {0}", type.Namespace).Line()
                .Append("{").Line()
                .Tab(1).Format("public partial class {0}", type.Name).Line()
                .Tab(1).Append("{").Line()
                    .Tab(2).Append("public override void Serialize(BinaryWriter writer)").Line()
                    .Tab(2).Append("{").Line()
                        .Tab(3).Append("base.Serialize(writer);").Line();

            var allSerializeMembers = SerializerAssists.FindSerializeMembers(type);
            for (int i = 0; i < allSerializeMembers.Count; i++)
            {
                var memberInfo = allSerializeMembers[i];
                var paramText = SerializerAssists.GetClassMemberDummyText(memberInfo);

                if (memberInfo.IsDefined(typeof(SBDynamicAttribute), true) &&
                    !SerializerAssists.IsBaseType(SerializerAssists.GetMemberType(memberInfo), false))
                    StringBuilder?
                        .Tab(3).Format("writer.SerializeDynamic({0});", paramText).Line();
                else
                    StringBuilder?
                        .Tab(3).Format("writer.Serialize({0});", paramText).Line();
            }
            StringBuilder
                .Tab(2).Append("}").Line();


            StringBuilder.Tab(2).Append("public override void Deserialize(BinaryReader reader)").Line()
                .Tab(2).Append("{").Line()
                    .Tab(3).Append("base.Deserialize(reader);").Line();

            for (int i = 0; i < allSerializeMembers.Count; i++)
            {
                var memberInfo = allSerializeMembers[i];
                var memberText = SerializerAssists.GetClassMemberTypeText(memberInfo);
                var memberDummyText = SerializerAssists.GetClassMemberDummyText(memberInfo);

                if (memberInfo.IsDefined(typeof(SBDynamicAttribute), false) &&
                    !SerializerAssists.IsBaseType(SerializerAssists.GetMemberType(memberInfo), false))
                    this.StringBuilder
                        .Tab(3).Format("this.{0} = {1}reader.DeserializeDynamic({2});", memberInfo.Name, memberText, memberDummyText).Line();
                else
                    this.StringBuilder
                        .Tab(3).Format("this.{0} = {1}reader.Deserialize({2});", memberInfo.Name, memberText, memberDummyText).Line();
            }
            StringBuilder
                    .Tab(2).Append("}").Line()
                .Tab(1).Append("}").Line()
            .Append("}").Line();

        }
    }
}
