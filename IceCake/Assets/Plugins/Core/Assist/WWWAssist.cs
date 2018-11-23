using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using UnityFx.Async;

namespace IceCake.Core
{
    public class WWWAssist
    {
        public static void Dispose(ref WWW www, bool isUnloadAllLoadedObjects = false)
        {
            if (www == null)
                return;

            if (string.IsNullOrEmpty(www.error) && www.assetBundle != null)
                www.assetBundle.Unload(isUnloadAllLoadedObjects);

            www.Dispose();
            www = null;
        }
    }
}
