using UnityEngine;
using UnityEditor;

namespace IceCake.Framework.AssetBundles.Editor
{
    public class ABBuilderEditor
    {
        /// <summary>
        /// 资源打包///
        /// </summary>
        [MenuItem("Tools/AssetBundle/Build AssetBundle...")]
        public static void Build()
        {
            ABBuilder.Instance.BuildAssetBundles(BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression);
        }

        /// <summary>
        /// 资源预处理///
        /// </summary>
        [MenuItem("Tools/AssetBundle/AssetBundle Proprocess...")]
        public static void Preprocess()
        {
            ABBuilder.Instance.AssetBundleDataBuilding();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            Debug.Log("AssetBundle Proprocess Sucess!");
        }
    }
}
