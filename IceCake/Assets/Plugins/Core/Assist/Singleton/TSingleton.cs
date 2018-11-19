using System;
using System.Reflection;

namespace IceCake.Core
{
    public class TSingleton<T> where T : class
    {
        static T _instance;
        static object _lock = new object();

        public static readonly Type[] emptyTypes = new Type[0];

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            ConstructorInfo ci = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, emptyTypes, null);

                            if (ci == null)
                            {
                                throw new InvalidOperationException("class must contain a private constructor");                           
                            }
                            _instance = (T)ci.Invoke(null);
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
