using IceCake.Core;
using System.IO;

namespace IceCake.Framework.AssetBundles.Editor
{
    public class ABHistory
    {
        public class Data
        {
            public string Time;
            public string Path;
            public ABVersion IncVer;
            public bool IsSelected;
        }

        public Dict<string, Data> Datas
        {
            get;
            private set;
        }

        public void Initialize(string outPath)
        {
            Datas = new Dict<string, Data>();

            string historyPath = UtilTool.PathCombine(outPath, "History");
            DirectoryInfo historyDirInfo = new DirectoryInfo(historyPath);
            if (!historyDirInfo.Exists)
                return;

            ABVersion curVersion = ABVersionEditor.Load(outPath);

            var subDirs = historyDirInfo.GetDirectories();
            for (int i = 0; i < subDirs.Length; i++)
            {
                Data data = new Data();
                data.Path = UtilTool.PathCombine(historyPath, subDirs[i].Name, ABVersion.ABVersionBinFile);
                data.Time = subDirs[i].Name;
                data.IncVer = new ABVersion();
                data.IncVer.Datas = new Dict<string, ABVersionData>();

                ABVersion historyVersion = ABVersionEditor.Load(subDirs[i].FullName);

                //比较两个版本//
                foreach (var pair in curVersion.Datas)
                {
                    var avData = pair.Value;
                    var oldData = historyVersion.GetData(avData.Name);
                    if (oldData == null) //说明是新增的//
                    {
                        data.IncVer.Datas.Add(avData.Name, avData);
                    }
                    else
                    {
                        if (oldData.Version != avData.Version) //版本不一致//
                        {
                            data.IncVer.Datas.Add(avData.Name, avData);
                        }
                    }
                }
                Datas.Add(data.Time, data);
            }
        }
    }
}
