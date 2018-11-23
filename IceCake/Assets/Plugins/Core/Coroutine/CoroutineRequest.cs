using UnityEngine;
using System.Collections;

namespace IceCake.Core
{
    public class CoroutineRequest<T> : CustomYieldInstruction where T : class
    {
        public CoroutineHandler Handler;

        public override bool keepWaiting
        {
            get
            {
                bool result = (Handler == null || (Handler != null && !Handler.IsRunning && Handler.IsCompleted));
                return !result;
            }
        }

        public CoroutineRequest<T> Start(IEnumerator enumerator)
        {
            Handler = CoroutineManager.Instance.StartHandler(enumerator);
            return this;
        }

        public void Stop()
        {
            CoroutineManager.Instance.Stop(Handler);
        }
    }
}
