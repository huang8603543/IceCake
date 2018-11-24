using System.IO;
using IceCake.Core;
using IceCake.Core.Serializer;
using IceCake.Framework.Serializer;

/// <summary>
/// Auto generate code, not modify.
/// </summary>
namespace IceCake.Framework.AssetBundles
{
	public partial class ABVersion
	{
		public override void Serialize(BinaryWriter writer)
		{
			base.Serialize(writer);
			writer.Serialize(this.Datas);
		}
		public override void Deserialize(BinaryReader reader)
		{
			base.Deserialize(reader);
			this.Datas = reader.Deserialize(this.Datas);
		}
	}
}

