using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using IceCake.Core;
using IceCake.Core.Editor;

namespace IceCake.Framework.AssetBundles.Editor
{
    /// <summary>
    /// 同步资源到StreamingAssets的目录下///
    /// </summary>
    public static class StreamingAssetsSync
    {
        /// <summary>
        /// 文件的状态///
        /// </summary>
        public enum FileState
        {
            New,
            Modify,
            Delete,
            Same
        };

        public class FileVersion
        {
            public string SrcPath;
            public string DistPath;
            public FileState State;
        }

        [MenuItem("Tools/AssetBundle/Sync StreamingAssets Assets")]
        public static void SyncAssets()
        {
            EditorCoroutineManager.Start(SyncAssetsAsync());
        }

        /// <summary>
        /// 开始同步资源///
        /// </summary>
        public static IEnumerator SyncAssetsAsync()
        {
            AssetBundleManifest buildManifest = null;
            AssetBundleManifest streamingManifest = null;

            string manifestName = ABBuilder.Instance.GetManifestName();
            string buildABDir = Path.GetFullPath(ABBuilder.Instance.GetPathPrefixAssetBundle()).Replace('\\', '/');
            string streamingDir = Path.GetFullPath("Assets/StreamingAssets/Assetbundles/" + manifestName).Replace('\\', '/');

            string buildManifestURL = "file:///" + buildABDir + "/" + manifestName;
            string streamingManifestURL = "file:///" + streamingDir + "/" + manifestName;

            yield return EditorCoroutineManager.Start(EditorAssists.LoadManifest(buildManifestURL, (abManifest) =>
            {
                buildManifest = abManifest;
            }));

            yield return EditorCoroutineManager.Start(EditorAssists.LoadManifest(streamingManifestURL, (abManifest) =>
            {
                streamingManifest = abManifest;
            }));

            //得到文件复制信息//
            Dict<string, FileVersion> fileVersionDict = new Dict<string, FileVersion>();
            if (manifestName == null)
                yield break;

            List<string> srcFiles = new List<string>(buildManifest.GetAllAssetBundles());
            List<string> distFiles = new List<string>(streamingManifest != null ? streamingManifest.GetAllAssetBundles() : new string[] { });
            for (int i = 0; i < srcFiles.Count; i++)
            {
                FileVersion fileVersion = new FileVersion();
                fileVersion.SrcPath = buildABDir + "/" + srcFiles[i];
                fileVersion.DistPath = streamingDir + "/" + srcFiles[i];
                if (distFiles.Contains(srcFiles[i]))
                {
                    string srcMD5 = buildManifest.GetAssetBundleHash(srcFiles[i]).ToString();
                    string distMD5 = streamingManifest.GetAssetBundleHash(srcFiles[i]).ToString();

                    if (srcMD5.Equals(distMD5))
                        fileVersion.State = FileState.Same;
                    else
                        fileVersion.State = FileState.Modify;
                }
                else
                {
                    fileVersion.State = FileState.New;
                }
                fileVersionDict.Add(srcFiles[i], fileVersion);
            }
            for (int i = 0; i < distFiles.Count; i++)
            {
                FileVersion fileVersion = new FileVersion();
                fileVersion.SrcPath = buildABDir + "/" + distFiles[i];
                fileVersion.DistPath = streamingDir + "/" + distFiles[i];
                if (!srcFiles.Contains(distFiles[i]))
                {
                    fileVersion.State = FileState.Delete;
                    fileVersionDict.Add(distFiles[i], fileVersion);
                }
            }

            //开始复制文件，删除文件//
            foreach (var pair in fileVersionDict)
            {
                FileVersion fileVersion = pair.Value;

                if (fileVersion.State == FileState.New || fileVersion.State == FileState.Modify)
                {
                    FileInfo fileInfo = new FileInfo(fileVersion.DistPath);
                    if (!Directory.Exists(fileInfo.DirectoryName)) Directory.CreateDirectory(fileInfo.DirectoryName);

                    File.Copy(fileVersion.SrcPath, fileVersion.DistPath, true);
                }
                else if (fileVersion.State == FileState.Delete)
                {
                    if (File.Exists(fileVersion.DistPath))
                        File.Delete(fileVersion.DistPath);
                }
            }

            //复制Manifest//
            File.Copy(buildABDir + "/" + manifestName, streamingDir + "/" + manifestName, true);

            //复制版本文件和MD5文件//
            File.Copy(buildABDir + "/" + ABVersion.ABVersionBinFile, streamingDir + "/" + ABVersion.ABVersionBinFile, true);
            File.Copy(buildABDir + "/" + ABVersion.ABVersionMD5File, streamingDir + "/" + ABVersion.ABVersionMD5File, true);

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("提示", "复制完成!", "是");
        }
    }
}
