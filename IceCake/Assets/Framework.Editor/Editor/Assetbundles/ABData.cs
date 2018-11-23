using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.IO;
using Object = UnityEngine.Object;

namespace IceCake.Framework.AssetBundles.Editor
{
    /// <summary>
    /// 一个资源包项///
    /// </summary>
    [Serializable]
    public class ABData
    {
        /// <summary>
        /// 资源包的类型///
        /// </summary>
        public enum AssetSourceType
        {
            DirDir, //目录下的每一个目录是一个包//
            DirFile, //目录下的每一个文件是一个包//
            Dir, //一个目录一个包//
            File,  //一个文件一个包//
        }

        /// <summary>
        /// 资源包名///
        /// </summary>
        public string ABName;

        /// <summary>
        /// 资源包后缀名///
        /// </summary>
        public string ABVariant;

        /// <summary>
        /// 资源包原始路径///
        /// </summary>
        public string AssetResPath;

        /// <summary>
        /// 资源类型///
        /// </summary>
        public string AssetType;

        /// <summary>
        /// 需要过滤的资源///
        /// </summary>
        public List<string> FilterAssets;

        /// <summary>
        /// 原始资源类型///
        /// </summary>
        public AssetSourceType AssetSrcType;

        /// <summary>
        /// 资源的类名///
        /// </summary>
        public string ABClassName;

        /// <summary>
        /// 最原始路径///
        /// </summary>
        public string ABOriginalResPath;

        /// <summary>
        /// 资源全名加后缀///
        /// </summary>
        public string ABFullName
        {
            get
            {
                return ABName + "." + ABVariant;
            }
        }

        public AssetBundleBuild[] ToABBuild()
        {
            switch(AssetSrcType)
            {
                case AssetSourceType.DirDir:
                    return GetOneDirDirs();
                case AssetSourceType.DirFile:
                    return GetOneDirFiles();
                case AssetSourceType.Dir:
                    return GetOneDir();
                case AssetSourceType.File:
                    return GetOneFile();
            }
            return null;
        }

        /// <summary>
        /// 得到一个文件的ABB///
        /// </summary>
        /// <returns></returns>
        public AssetBundleBuild[] GetOneFile()
        {
            Object obj = AssetDatabase.LoadAssetAtPath(AssetResPath, typeof(Object));
            if (obj == null)
                return null;

            AssetBundleBuild abb = new AssetBundleBuild();
            abb.assetBundleName = ABName;
            abb.assetBundleVariant = ABVariant;
            abb.assetNames = new string[] { AssetResPath };

            return new AssetBundleBuild[] { abb };
        }

        /// <summary>
        ///  得到一个目录的ABB///
        /// </summary>
        /// <returns></returns>
        public AssetBundleBuild[] GetOneDir()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(AssetResPath);
            if (!dirInfo.Exists)
                return null;

            AssetBundleBuild abb = new AssetBundleBuild();
            abb.assetBundleName = ABName;
            abb.assetBundleVariant = ABVariant;
            abb.assetNames = new string[] { AssetResPath };

            return new AssetBundleBuild[] { abb };
        }

        /// <summary>
        /// 得到一个目录下的所有的文件对应的ABB///
        /// </summary>
        /// <returns></returns>
        public AssetBundleBuild[] GetOneDirFiles()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(AssetResPath);
            if (!dirInfo.Exists)
                return null;

            List<AssetBundleBuild> abbList = new List<AssetBundleBuild>();
            string[] guids = AssetDatabase.FindAssets(AssetType, new string[] { AssetResPath });
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

                AssetBundleBuild abb = new AssetBundleBuild();
                abb.assetBundleName = ABName + "/" + Path.GetFileNameWithoutExtension(assetPath);
                abb.assetBundleVariant = ABVariant;
                abb.assetNames = new string[] { assetPath };
                abbList.Add(abb);
            }

            return abbList.ToArray();
        }

        /// <summary>
        /// 得到一个目录下的所有的目录对应的ABB///
        /// </summary>
        /// <returns></returns>
        public AssetBundleBuild[] GetOneDirDirs()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(AssetResPath);
            if (!dirInfo.Exists)
                return null;

            List<AssetBundleBuild> abbList = new List<AssetBundleBuild>();
            DirectoryInfo[] subDirs = dirInfo.GetDirectories();
            for (int i = 0; i < subDirs.Length; i++)
            {
                string dirPath = subDirs[i].FullName;
                string rootPath = Environment.CurrentDirectory + "\\";
                dirPath = dirPath.Replace(rootPath, "").Replace("\\", "/");

                string fileName = Path.GetFileNameWithoutExtension(dirPath);
                if (FilterAssets != null && FilterAssets.FindIndex((item) => { return fileName.Contains(item); }) >= 0) continue;

                AssetBundleBuild abb = new AssetBundleBuild();
                abb.assetBundleName = ABName + "/" + fileName;
                abb.assetBundleVariant = ABVariant;
                abb.assetNames = new string[] { dirPath };
                abbList.Add(abb);
            }

            return abbList.ToArray();
        }
    }
}