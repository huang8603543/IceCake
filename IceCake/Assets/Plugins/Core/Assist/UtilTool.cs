using System;
using System.Collections.Generic;
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
    }
}
