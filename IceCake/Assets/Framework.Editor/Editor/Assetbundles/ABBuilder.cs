using IceCake.Core;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using IceCake.Core.Editor;
using System.IO;

namespace IceCake.Framework.AssetBundles.Editor
{
    public class ABBuilder : TSingleton<ABBuilder>
    {
        public enum BuildPlatform
        {
            Windows = BuildTarget.StandaloneWindows, //Windows
            Windows64 = BuildTarget.StandaloneWindows64, //Windows64
            OSX = BuildTarget.StandaloneOSX, //OSX
            IOS = BuildTarget.iOS, //IOS
            Android = BuildTarget.Android, //Android
        };

        /// <summary>
        /// 输出的Assetbundle的目录///
        /// </summary>
        public static string AssetbundlePath = "Assets/../Assetbundles";

        /// <summary>
        /// 资源包配置文件路径///
        /// </summary>
        public static string ABDataConfigPath = "Assets/Framework.Editor/Editor/Assetbundles/AssetbundleSettings.asset";

        /// <summary>
        /// 当前工程的平台///
        /// </summary>
        public BuildPlatform CurBuildPlatform = BuildPlatform.Windows;

        public List<ABData> ABDatas;

        private ABBuilder()
        {
            CurBuildPlatform = GetCurrentBuildPlatform();
        }

        /// <summary>
        /// 打包资源///
        /// </summary>
        /// <param name="options"></param>
        public void BuildAssetBundles(BuildAssetBundleOptions options)
        {
            List<AssetBundleBuild> abbList = AssetBundleDataBuilding();

            string abPath = GetPathPrefixAssetBundle();
            DirectoryInfo dirInfo = new DirectoryInfo(abPath);
            if (!dirInfo.Exists)
                dirInfo.Create();

            //
            //

            //开始打包//
            var newABManifest = BuildPipeline.BuildAssetBundles(abPath, abbList.ToArray(), options, (BuildTarget)CurBuildPlatform);

            //生成新的版本文件//
            //
            //

            Debug.Log("资源打包完成！");
        }

        /// <summary>
        /// 构建需要打包的资源的路径、包名以及包的后缀///
        /// </summary>
        /// <returns></returns>
        public List<AssetBundleBuild> AssetBundleDataBuilding()
        {
            ABDatas = GenerateABDatas();
            if (ABDatas == null)
                ABDatas = new List<ABData>();

            //预处理图集配置//





            //资源预处理//
            List<ABDataProcessor> abDataProcessors = new List<ABDataProcessor>();
            foreach(var data in ABDatas)
            {
                ABDataProcessor processor = ABDataProcessor.Create(data);
                processor.PreprocessAssets();
                processor.ProcessAssetBundleLabel();
                abDataProcessors.Add(processor);
            }

            //打包//
            List<AssetBundleBuild> abbList = new List<AssetBundleBuild>();
            foreach (var processor in abDataProcessors)
            {
                abbList.AddRange(processor.ToABBuild());
            }
            return abbList;
        }

        /// <summary>
        /// 生成AB包的配置文件///
        /// </summary>
        /// <returns></returns>
        public List<ABData> GenerateABDatas()
        {
            var ABDataConfig = EditorAssists.ReceiveAsset<ABDataConfig>(ABDataConfigPath);
            if (ABDataConfig = null)
                return new List<ABData>();
            return ABDataConfig.ABDatas;
        }

        public static BuildPlatform GetCurrentBuildPlatform()
        {
            return (BuildPlatform)EditorUserBuildSettings.activeBuildTarget;
        }

        public static string GetCurrentBuildPlatformName()
        {
            string platformName = GetCurrentBuildPlatform().ToString();
            if (platformName.Equals("Windows64"))
                platformName = "Windows";
            return platformName;
        }

        /// <summary>
        /// 得到Assetbundle的路径前缀，根据不同的平台来选择///
        /// </summary>
        /// <returns></returns>
        public string GetPathPrefixAssetBundle()
        {
            return Path.Combine(AssetbundlePath, GetManifestName()).Replace("\\", "/");
        }

        /// <summary>
        /// 得到Manifest的名字
        /// </summary>
        /// <returns></returns>
        public string GetManifestName()
        {
            return GetCurrentBuildPlatformName() + "_Assetbundles";
        }
    }
}
