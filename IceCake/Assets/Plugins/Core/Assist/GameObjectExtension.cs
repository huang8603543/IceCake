using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IceCake.Core
{
    public static class GameObjectExtension
    {
        public static void ResetTransform(this GameObject go)
        {
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
        }

        public static T SafeGetComponent<T>(this GameObject go) where T : Component
        {
            if (go == null)
                return default(T);
            return go.GetComponent<T>();
        }

        public static T ReceiveComponent<T>(this GameObject go) where T : Component
        {
            if (go == null)
                return default(T);
            T component = go.GetComponent<T>();
            if (component == null)
                component = go.AddComponent<T>();

            return component;
        }
    }
}
