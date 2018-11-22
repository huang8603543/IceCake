using IceCake.Core.Json;
using IceCake.Core.Serializer;
using System;

namespace IceCake.Framework.AssetBundles
{
    public partial class ABVersionData : SerializerBinary
    {
        public string N;
        public int V;
        public string M;
        public long S;
        public string[] D;

        [SBIgnore][JsonIgnore]
        public string Name { get { return N; } set { N = value; } }

        [SBIgnore][JsonIgnore]
        public int Version { get { return V; } set { V = value; } }

        [SBIgnore][JsonIgnore]
        public string MD5 { get { return M; } set { M = value; } }

        [SBIgnore][JsonIgnore]
        public long Size { get { return S; } set { S = value; } }

        [SBIgnore][JsonIgnore]
        public string[] Dependencies { get { return D; } set { D = value; } }
    }
}
