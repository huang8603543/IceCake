using IceCake.Core.Serializer;
using IceCake.Core;
using System.Threading.Tasks;
using System.IO;

namespace IceCake.Framework.AssetBundles
{
    public partial class ABVersion : SerializerBinary
    {
        public class LoaderRequest
        {
            public ABVersion Version;
            public string Url;

            public LoaderRequest(string url)
            {
                Url = url;
            }
        }

        [SBIgnore]
        public static string ABVersionJsonFile = "ABVersion.Json";

        [SBIgnore]
        public static string ABVersionBinFile = "ABVersion.Bin";

        [SBIgnore]
        public static string ABVersionMD5File = "ABVersionMD5.Bin";

        public Dict<string, ABVersionData> Datas;

        public ABVersionData GetData(string abName)
        {
            if (Datas == null)
                return null;
            ABVersionData data = null;
            Datas.TryGetValue(abName, out data);
            return data;
        }

        //public static async Task<LoaderRequest> Load(string url)
        //{

        //}

        //public static async Task<LoaderRequest> Download(string url)
        //{

        //}

        public void Save(string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                using (var bw = new BinaryWriter(fs))
                {
                    Serialize(bw);
                }
            }
        }
    }
}
