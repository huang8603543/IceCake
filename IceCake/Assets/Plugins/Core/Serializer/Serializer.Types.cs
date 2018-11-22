using System.IO;
using System;

namespace IceCake.Core.Serializer
{
    /// <summary>
    /// 扩展BinaryWriter,封装Write方法///
    /// </summary>
    public static class ValueTypeSerialize
    {
        public static void Serialize(this BinaryWriter writer, char value) { writer.Write(value); }
        public static void Serialize(this BinaryWriter writer, byte value) { writer.Write(value); }
        public static void Serialize(this BinaryWriter writer, sbyte value) { writer.Write(value); }
        public static void Serialize(this BinaryWriter writer, bool value) { writer.Write(value); }
        public static void Serialize(this BinaryWriter writer, short value) { writer.Write(value); }
        public static void Serialize(this BinaryWriter writer, ushort value) { writer.Write(value); }
        public static void Serialize(this BinaryWriter writer, int value) { writer.Write(value); }
        public static void Serialize(this BinaryWriter writer, uint value) { writer.Write(value); }
        public static void Serialize(this BinaryWriter writer, long value) { writer.Write(value); }
        public static void Serialize(this BinaryWriter writer, ulong value) { writer.Write(value); }
        public static void Serialize(this BinaryWriter writer, float value) { writer.Write(value); }
        public static void Serialize(this BinaryWriter writer, double value) { writer.Write(value); }
        public static void Serialize(this BinaryWriter writer, decimal value) { writer.Write(value); }
        public static void Serialize(this BinaryWriter writer, string value)
        {
            bool valid = !string.IsNullOrEmpty(value);
            writer.Write(valid);
            if (valid)
                writer.Write(value);
        }
    }

    /// <summary>
    /// 扩展BinaryReader,封装Read方法///
    /// </summary>
    public static class ValueTypeDeserialize
    {
        public static char Deserialize(this BinaryReader reader, char value) { return reader.ReadChar(); }
        public static byte Deserialize(this BinaryReader reader, byte value) { return reader.ReadByte(); }
        public static sbyte Deserialize(this BinaryReader reader, sbyte value) { return reader.ReadSByte(); }
        public static bool Deserialize(this BinaryReader reader, bool value) { return reader.ReadBoolean(); }
        public static short Deserialize(this BinaryReader reader, short value) { return reader.ReadInt16(); }
        public static ushort Deserialize(this BinaryReader reader, ushort value) { return reader.ReadUInt16(); }
        public static int Deserialize(this BinaryReader reader, int value) { return reader.ReadInt32(); }
        public static uint Deserialize(this BinaryReader reader, uint value) { return reader.ReadUInt32(); }
        public static long Deserialize(this BinaryReader reader, long value) { return reader.ReadInt64(); }
        public static ulong Deserialize(this BinaryReader reader, ulong value) { return reader.ReadUInt64(); }
        public static float Deserialize(this BinaryReader reader, float value) { return reader.ReadSingle(); }
        public static double Deserialize(this BinaryReader reader, double value) { return reader.ReadDouble(); }
        public static decimal Deserialize(this BinaryReader reader, decimal value) { return reader.ReadDecimal(); }
        public static string Deserialize(this BinaryReader reader, string value)
        {
            bool valid = reader.ReadBoolean();
            if (!valid)
                return string.Empty;
            return reader.ReadString();
        }
    }

    /// <summary>
    /// 扩展BinaryWriter, 可序列化类序列化///
    /// </summary>
    public static class SerializerBinarySerialize
    {
        public static void Serialize<T>(this BinaryWriter writer, T value) where T : SerializerBinary
        {
            bool vaild = null != value;
            writer.Serialize(vaild);
            if (vaild)
                value.Serialize(writer);
        }

        public static void SerializeDynamic<T>(this BinaryWriter writer, T value) where T : SerializerBinary
        {
            bool vaild = null != value;
            writer.Serialize(vaild);
            if (vaild)
            {
                writer.Serialize(value.GetType().FullName);
                value.Serialize(writer);
            }
        }
    }

    /// <summary>
    /// 扩展BinaryReader, 可序列化类反序列化///
    /// </summary>
    public static class SerializerBinaryDeserialize
    {
        public static T Deserialize<T>(this BinaryReader reader, T value) where T : SerializerBinary
        {
            bool vaild = reader.Deserialize(false);
            if (!vaild)
                return null;
            var instance = ReflectExtension.Construct<T>();
            instance.Deserialize(reader);
            return instance;
        }

        public static T DeserializeDynamic<T>(this BinaryReader reader, T value) where T : SerializerBinary
        {
            bool vaild = reader.Deserialize(false);
            if (!vaild)
                return null;

            var fullName = reader.Deserialize(string.Empty);
            var instance = ReflectExtension.TConstruct<T>(Type.GetType(fullName));
            instance.Deserialize(reader);
            return instance;
        }
    }
}
