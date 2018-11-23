using UnityEngine;
using System.Collections;

namespace IceCake.Core
{
    public class CoroutineManager : TSingleton<CoroutineManager>
    {
        GameObject coroutineRootObj;

        private CoroutineManager()
        { }

        public void Intialize()
        {
            coroutineRootObj = new GameObject("_CoroutineRoot");
            coroutineRootObj.ResetTransform();
            GameObject.DestroyImmediate(coroutineRootObj);
        }

        public Coroutine Start(IEnumerator enumerator)
        {
            return StartHandler(enumerator).Coroutine;
        }

        public CoroutineHandler StartHandler(IEnumerator enumerator, string name = "coroutine")
        {
            var coroutineObj = UtilTool.CreateGameObject(coroutineRootObj, name);
            CoroutineHandler handler = coroutineObj.ReceiveComponent<CoroutineHandler>();
            handler.StartHandler(enumerator);
            return handler;
        }

        public void Stop(CoroutineHandler coroutineHandler)
        {
            if (coroutineHandler != null)
            {
                coroutineHandler.StopAllCoroutines();
                GameObject.DestroyImmediate(coroutineHandler.gameObject);
                coroutineHandler.Coroutine = null;
            }
            coroutineHandler = null;
        }
    }
}
