using UnityEditor;
using IceCake.Core;
using System.IO;
using IceCake.Core.Json;
using System;
using UnityEngine;

namespace IceCake.Framework.AssetBundles.Editor
{
    public static class ABVersionEditor
    {
        public static ABVersion Load(string outPath)
        {
            string versionPath = Path.Combine(outPath, ABVersion.ABVersionBinFile);
            if (!File.Exists(versionPath))
                return null;

            ABVersion abVersion = new ABVersion();
            using (FileStream fs = new FileStream(versionPath, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    abVersion.Deserialize(br);
                }
            }
            return abVersion;
        }

        public static void SaveInEditor(this ABVersion version, string outPath)
        {
            if (version == null)
                return;

            string versionBinPath = Path.Combine(outPath, ABVersion.ABVersionBinFile);
            string versionJsonPath = Path.Combine(outPath, ABVersion.ABVersionJsonFile);
            string versionMD5Path = Path.Combine(outPath, ABVersion.ABVersionMD5File);

            using (FileStream fs = new FileStream(versionBinPath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    version.Serialize(bw);
                }
            }
            JsonNode jsonNode = JsonParser.ToJsonNode(version);
            File.WriteAllText(versionJsonPath, jsonNode.ToString());

            string versionMD5 = UtilTool.GetMD5(versionBinPath).ToHEXString();
            File.WriteAllText(versionMD5Path, versionMD5);

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        public static void SaveHistory(this ABVersion version, string outPath)
        {
            if (version == null)
                return;

            string historyDateStr = string.Format("History/{0}", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            string versionBinPath = UtilTool.PathCombine(outPath, historyDateStr, ABVersion.ABVersionBinFile);

            string dirPath = Path.GetDirectoryName(versionBinPath);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

            using (FileStream fs = new FileStream(versionBinPath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    version.Serialize(bw);
                }
            }
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        public static void SaveIncrement(this ABVersion version, string abPath, string targetPath)
        {
            if (version == null)
                return;

            //保存增量版本文件//
            string targetVersionBinPath = UtilTool.PathCombine(targetPath, ABVersion.ABVersionBinFile);
            using (FileStream fs = new FileStream(targetVersionBinPath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    version.Serialize(bw);
                }
            }

            //复制增量AB包//
            foreach (var versionPair in version.Datas)
            {
                var avData = versionPair.Value;

                string srcABPath = UtilTool.PathCombine(abPath, avData.Name);
                string distABPath = UtilTool.PathCombine(targetPath, avData.Name);

                string distDir = Path.GetDirectoryName(distABPath);
                if (!Directory.Exists(distDir))
                    Directory.CreateDirectory(distDir);

                File.Copy(srcABPath, distABPath, true);
            }

            //复制MD5文件//
            string srcMD5Path = UtilTool.PathCombine(abPath, ABVersion.ABVersionMD5File);
            string distMD5Path = UtilTool.PathCombine(targetPath, ABVersion.ABVersionMD5File);

            string distMD5Dir = Path.GetDirectoryName(distMD5Path);
            if (!Directory.Exists(distMD5Dir))
                Directory.CreateDirectory(distMD5Dir);

            File.Copy(srcMD5Path, distMD5Path, true);
        }

        public static ABVersion CreateVersion(string outPath, ABVersion oldVersion, AssetBundleManifest newABManifest)
        {
            ABVersion version = new ABVersion();
            version.Datas = new Dict<string, ABVersionData>();

            string[] allAssetbundles = newABManifest.GetAllAssetBundles();
            for (int i = 0; i < allAssetbundles.Length; i++)
            {
                ABVersionData avData = new ABVersionData();
                avData.Name = allAssetbundles[i];

                var oldData = oldVersion != null ? oldVersion.GetData(allAssetbundles[i]) : null;

                string oldMD5 = oldData != null ? oldData.MD5 : string.Empty;
                string newMD5 = GetMD5InManifest(newABManifest, allAssetbundles[i]);

                avData.MD5 = newMD5;

                if (!string.IsNullOrEmpty(oldMD5) && !oldMD5.Equals(newMD5))
                    avData.Version = GetVersionInABVersion(oldVersion, avData.Name) + 1;
                else
                    avData.Version = GetVersionInABVersion(oldVersion, avData.Name);

                avData.Size = GetABSizeInManifest(outPath, allAssetbundles[i]);
                avData.Dependencies = newABManifest.GetDirectDependencies(allAssetbundles[i]);
                version.Datas.Add(allAssetbundles[i], avData);
            }
            return version;
        }

        public static string GetMD5InManifest(AssetBundleManifest manifest, string abName)
        {
            if (manifest == null)
                return string.Empty;
            return manifest.GetAssetBundleHash(abName).ToString();
        }

        public static string GetMD5ForABVersion(string outPath)
        {
            string versionMD5Path = Path.Combine(outPath, ABVersion.ABVersionBinFile);
            if (!File.Exists(versionMD5Path))
                return string.Empty;

            return UtilTool.GetMD5(versionMD5Path).ToHEXString();
        }

        public static int GetVersionInABVersion(ABVersion version, string abName)
        {
            if (version == null)
                return 1;

            ABVersionData avData = version.GetData(abName);
            if (avData == null)
                return 1;
            return avData.Version;
        }

        public static long GetABSizeInManifest(string outPath, string abName)
        {
            string abFilePath = outPath + "/" + abName;
            var abFileInfo = new FileInfo(abFilePath);
            if (abFilePath != null)
            {
                return abFileInfo.Length;
            }
            return 0;
        }
    }
}
