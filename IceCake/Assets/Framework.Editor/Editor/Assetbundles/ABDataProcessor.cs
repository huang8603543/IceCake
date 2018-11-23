using IceCake.Core;
using System;
using UnityEditor;

namespace IceCake.Framework.AssetBundles.Editor
{
    /// <summary>
    /// ABData的预处理器///
    /// </summary>
    public class ABDataProcessor
    {
        public ABData Data;

        /// <summary>
        /// 预处理所有的资源///
        /// </summary>
        public virtual void PreprocessAssets()
        { }

        public void ProcessAssetBundleLabel()
        {
            if (Data == null)
                return;

            var assetbundleBuilds = Data.ToABBuild();
            for (int i = 0; i < assetbundleBuilds.Length; i++)
            {
                var abBuild = assetbundleBuilds[i];
                for (int j = 0; j < abBuild.assetNames.Length; j++)
                {
                    string assetPath = abBuild.assetNames[j];
                    AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
                    if (assetImporter == null)
                        return;

                    assetImporter.SetAssetBundleNameAndVariant(abBuild.assetBundleName, abBuild.assetBundleVariant);
                    AssetDatabase.WriteImportSettingsIfDirty(assetPath);
                }
            }
        }

        /// <summary>
        /// 将Data转成能用于打包的ABB
        /// </summary>
        /// <returns></returns>
        public AssetBundleBuild[] ToABBuild()
        {
            if (Data == null)
                return new AssetBundleBuild[0];
            return Data.ToABBuild();
        }

        /// <summary>
        /// 根据不同的类名构建不同的资源预处理器///
        /// </summary>
        /// <param name="abData"></param>
        /// <returns></returns>
        public static ABDataProcessor Create(ABData abData)
        {
            ABDataProcessor dataProcessor = null;

            Type type = Type.GetType(abData.ABClassName);
            if (type == null)
                dataProcessor = new ABDataProcessor();
            else
                dataProcessor = ReflectionAssist.Construct(type) as ABDataProcessor;

            dataProcessor.Data = abData;
            return dataProcessor;
        }
    }
}
