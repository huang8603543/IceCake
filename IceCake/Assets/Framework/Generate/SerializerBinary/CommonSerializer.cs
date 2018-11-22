using System.IO;
using System.Collections.Generic;
using IceCake.Core;
using IceCake.Core.Serializer;

/// <summary>
/// Auto generate code, not modify.
/// </summary>
namespace IceCake.Framework.Serializer
{
	public static class CommonSerializer
	{
		public static void Serialize(this BinaryWriter writer, string[] value)
		{
			var bValid = (null != value);
			writer.Serialize(bValid);
			if (!bValid) return;

			writer.Serialize(value.Length);
			for (int nIndex = 0; nIndex < value.Length; nIndex++)
				writer.Serialize(value[nIndex]);
		}

		public static string[] Deserialize(this BinaryReader reader, string[] value)
		{
			var bValid = reader.Deserialize(default(bool));
			if (!bValid) return null;

			var nCount  = reader.Deserialize(default(int));
			var rResult = new string[nCount];
			for (int nIndex = 0; nIndex < nCount; nIndex++)
				rResult[nIndex] = reader.Deserialize(string.Empty);
			return rResult;
		}

	}
}