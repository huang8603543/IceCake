using System.IO;
using IceCake.Core;
using IceCake.Core.Serializer;
using IceCake.Framework.Serializer;

/// <summary>
/// Auto generate code, not modify.
/// </summary>
namespace IceCake.Framework.AssetBundles
{
	public partial class ABVersionData
	{
		public override void Serialize(BinaryWriter writer)
		{
			base.Serialize(writer);
			writer.Serialize(this.N);
			writer.Serialize(this.V);
			writer.Serialize(this.M);
			writer.Serialize(this.S);
			writer.Serialize(this.D);
		}
		public override void Deserialize(BinaryReader reader)
		{
			base.Deserialize(reader);
			this.N = reader.Deserialize(this.N);
			this.V = reader.Deserialize(this.V);
			this.M = reader.Deserialize(this.M);
			this.S = reader.Deserialize(this.S);
			this.D = reader.Deserialize(this.D);
		}
	}
}

