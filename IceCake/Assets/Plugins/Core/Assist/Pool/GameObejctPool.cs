using UnityEngine;

namespace IceCake.Core
{
    public class GameObjectPool
    {
        TObjectPool<GameObject> objectPool;

        GameObject prefabGo;

        public GameObject RootGo
        {
            get;
            private set;
        }

        public GameObjectPool(string poolName, GameObject prefabGo, int initCount = 0)
        {
            this.prefabGo = prefabGo;
            objectPool = new TObjectPool<GameObject>(OnAlloc, OnFree, OnDestory);

            RootGo = UtilTool.CreateGameObject(poolName);
            RootGo.SetActive(false);
            RootGo.transform.position = new Vector3(0, 1000, 0);

            for (int i = 0; i < initCount; i++)
            {
                objectPool.Alloc();
            }
        }

        public GameObjectPool(GameObject rootGo, GameObject prefabGo, int initCount = 0)
        {
            this.prefabGo = prefabGo;
            objectPool = new TObjectPool<GameObject>(OnAlloc, OnFree, OnDestory);

            RootGo = rootGo;
            RootGo.SetActive(false);

            for (int i = 0; i < initCount; i++)
            {
                objectPool.Alloc();
            }
        }

        public GameObject Alloc()
        {
            return objectPool.Alloc();
        }

        public void Free(GameObject go)
        {
            if (go == null)
                return;
            objectPool.Free(go);
        }

        public void Destory()
        {
            prefabGo = null;
            objectPool.Destory();
            UtilTool.SafeDestroy(RootGo);
        }

        GameObject OnAlloc()
        {
            return GameObject.Instantiate(prefabGo);
        }

        void OnFree(GameObject go)
        {
            go.transform.SetParent(RootGo.transform);
            go.ResetTransform();
        }

        void OnDestory(GameObject go)
        {
            UtilTool.SafeDestroy(go);
        }
    }
}
