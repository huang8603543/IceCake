using UnityEngine;
using System.Collections;

namespace IceCake.Core
{
    public class CoroutineHandler : MonoBehaviour
    {
        public Coroutine Coroutine;
        public bool IsCompleted;
        public bool IsRunning;

        public Coroutine StartHandler(IEnumerator enumerator)
        {
            IsCompleted = false;
            IsRunning = true;
            Coroutine = StartCoroutine(StartHandlerAsync(enumerator));
            return Coroutine;
        }

        IEnumerator StartHandlerAsync(IEnumerator enumerator)
        {
            yield return enumerator;
            IsRunning = false;
            IsCompleted = true;

            yield return 0;

            CoroutineManager.Instance.Stop(this);
        }
    }
}
