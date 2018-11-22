using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace IceCake.Core
{
    public static class UtilTool
    {
        #region GameObject

        #region CreateGameObject

        public static GameObject CreateGameObject(string name, params Type[] comps)
        {
            GameObject go = new GameObject(name, comps);
            go.ResetTransform();

            return go;
        }

        public static GameObject CreateGameObject(GameObject parentGo, string name, params Type[] comps)
        {
            GameObject go = new GameObject(name, comps);
            go.transform.parent = parentGo.transform;
            go.ResetTransform();
            return go;
        }

        public static GameObject CreateGameObject(GameObject templateGo)
        {
            GameObject go = GameObject.Instantiate(templateGo);
            go.name = templateGo.name;
            go.ResetTransform();
            return go;
        }

        public static GameObject CreateGameObject(GameObject parentGo, GameObject templateGo)
        {
            GameObject go = GameObject.Instantiate(templateGo);
            go.name = templateGo.name;
            go.transform.parent = parentGo.transform;
            go.ResetTransform();
            return go;
        }

        #endregion

        public static void SafeDestroy(UnityEngine.Object obj)
        {
            if (obj != null)
                GameObject.DestroyImmediate(obj, true);
            obj = null;
        }

        #region SetLayer

        public static void SetLayer(GameObject obj, string layerName, bool isIncludeChildren = false)
        {
            int layer = LayerMask.NameToLayer(layerName);
            SetLayer(obj, layer, isIncludeChildren);
        }

        public static void SetLayer(GameObject obj, int layer, bool isIncludeChildren = false)
        {
            obj.layer = layer;
            if (isIncludeChildren)
            {
                for (int i = 0, childCount = obj.transform.childCount; i < childCount; i++)
                {
                    var childObj = obj.transform.GetChild(i).gameObject;
                    SetLayer(childObj, layer, isIncludeChildren);
                }
            }
        }

        #endregion

        #endregion

        #region SafeEcecute

        public static void SafeExcute(Action action)
        {
            action?.Invoke();
        }

        public static void SafeExcute<T>(Action<T> action, T obj)
        {
            action?.Invoke(obj);
        }

        public static void SafeExcute<T1, T2>(Action<T1, T2> action, T1 obj1, T2 obj2)
        {
            action?.Invoke(obj1, obj2);
        }

        public static void SafeExcute<T1, T2, T3>(Action<T1, T2, T3> action, T1 obj1, T2 obj2, T3 obj3)
        {
            action?.Invoke(obj1, obj2, obj3);
        }

        public static void SafeExcute<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 obj1, T2 obj2, T3 obj3, T4 obj4)
        {
            action?.Invoke(obj1, obj2, obj3, obj4);
        }

        public static TResult SafeExcute<TResult>(Func<TResult> func)
        {
            if (func == null)
                return default(TResult);
            return func();
        }

        public static TResult SafeExcute<T, TResult>(Func<T, TResult> func, T obj)
        {
            if (func == null)
                return default(TResult);
            return func(obj);
        }

        public static TResult SafeExcute<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 obj1, T2 obj2)
        {
            if (func == null)
                return default(TResult);
            return func(obj1, obj2);
        }

        public static TResult SafeExcute<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 obj1, T2 obj2, T3 obj3)
        {
            if (func == null)
                return default(TResult);
            return func(obj1, obj2, obj3);
        }

        public static TResult SafeExcute<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, T1 obj1, T2 obj2, T3 obj3, T4 obj4)
        {
            if (func == null)
                return default(TResult);
            return func(obj1, obj2, obj3, obj4);
        }

        #endregion

        #region MD5

        public static string GetMD5String(string text)
        {
            return GetMD5String(text, Encoding.Default);
        }

        public static string GetMD5String(string text, Encoding encoding)
        {
            return HashAlgorithmByString(text, "MD5", encoding);
        }

        public static string HashAlgorithmByString(string text, string hashName, Encoding rEncoding)
        {
            var hashAlgorithm = HashAlgorithm.Create(hashName);
            var textBytes = rEncoding.GetBytes(text);
            hashAlgorithm.TransformFinalBlock(textBytes, 0, textBytes.Length);
            return hashAlgorithm.Hash.ToHEXString();
        }

        public static string ToHEXString(this byte[] self)
        {
            var stringBuilder = new StringBuilder();
            for (int index = 0; index < self.Length; ++index)
                stringBuilder.AppendFormat("{0:X2}", self[index]);
            return stringBuilder.ToString();
        }

        public static byte[] GetMD5(string contentFile)
        {
            return GetMD5(new List<string>() { contentFile });
        }

        public static byte[] GetMD5(List<string> contentFile)
        {
            contentFile.Sort((a1, a2) => { return a1.CompareTo(a2); });

            HashAlgorithm hasAlgo = HashAlgorithm.Create("MD5");
            byte[] hashValue = new byte[20];

            byte[] tempBuffer = new byte[4096];
            int tempCount = 0;
            for (int i = 0; i < contentFile.Count; i++)
            {
                if (File.Exists(contentFile[i]))
                {
                    FileStream fs = File.OpenRead(contentFile[i]);
                    while (fs.Position != fs.Length)
                    {
                        tempCount += fs.Read(tempBuffer, 0, 4096 - tempCount);
                        if (tempCount == 4096)
                        {
                            if (hasAlgo.TransformBlock(tempBuffer, 0, tempCount, null, 0) != 4096)
                                Debug.LogError("TransformBlock error.");
                            tempCount = 0;
                        }
                    }
                    fs.Close();
                }
            }
            hasAlgo.TransformFinalBlock(tempBuffer, 0, tempCount);
            hashValue = hasAlgo.Hash;
            return hashValue;
        }

        #endregion

        #region Write

        public static void WriteAllText(string path, string content)
        {
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(path, content);
        }

        public static void WriteAllText(string path, string content, Encoding encoding)
        {
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(path, content, encoding);
        }

        public static void WriteAllBytes(string path, byte[] bytes)
        {
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllBytes(path, bytes);
        }

        #endregion

        #region Path

        public static string PathCombine(params string[] paths)
        {
            return PathCombine('/', paths);
        }

        public static string PathCombine(char directoryChar, params string[] paths)
        {
            var replaceChar = directoryChar == '\\' ? '/' : '\\';
            if (paths.Length == 0)
                return string.Empty;

            var firstPath = paths[0].Replace(replaceChar, directoryChar);
            if (firstPath.Length > 0 && firstPath[firstPath.Length - 1] == directoryChar)
                firstPath = firstPath.Substring(0, firstPath.Length - 1);

            var builder = new StringBuilder(firstPath);
            for (int index = 1; index < paths.Length; index++)
            {
                if (string.IsNullOrEmpty(paths[index]))
                    continue;

                var path = paths[index].Replace(replaceChar, directoryChar);
                if (path[0] != directoryChar)
                    path = directoryChar + path;
                if (path[path.Length - 1] == directoryChar)
                    path = path.Substring(0, path.Length - 1);
                builder.Append(path);
            }

            return builder.ToString();
        }

        #endregion
    }
}
