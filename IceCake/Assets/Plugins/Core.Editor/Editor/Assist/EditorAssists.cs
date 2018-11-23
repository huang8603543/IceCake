using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

namespace IceCake.Core.Editor
{
    public class EditorAssists
    {
        public static T ReceiveAsset<T>(string assetPath) where T : ScriptableObject
        {
            var obj = AssetDatabase.LoadAssetAtPath<T>(assetPath) as T;
            if (obj == null)
            {
                obj = ScriptableObject.CreateInstance(typeof(T)) as T;
                AssetDatabase.CreateAsset(obj, assetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                obj = AssetDatabase.LoadAssetAtPath<T>(assetPath) as T;
            }
            return obj;
        }

        public static IEnumerator LoadManifest(string manifestURL, Action<AssetBundleManifest> loadCompleted)
        {
            WWW www = new WWW(manifestURL);
            yield return www;

            if (www == null || !string.IsNullOrEmpty(www.error))
            {
                Debug.Log("加载Manifest出错: " + www.error);
                UtilTool.SafeExcute(loadCompleted, null);
                yield break;
            }
            var abManifest = www.assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest") as AssetBundleManifest;
            WWWAssist.Dispose(ref www);
            UtilTool.SafeExcute(loadCompleted, abManifest);
        }

        public static AssetBundleManifest LoadManifest(string manifestURL)
        {
            var assetbundle = AssetBundle.LoadFromFile(manifestURL);
            if (assetbundle == null)
                return null;

            var manifest = assetbundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest") as AssetBundleManifest;
            return manifest;
        }
    }
}
