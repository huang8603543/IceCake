namespace IceCake.Core
{
    public class Singleton<T> where T : new()
    {
        static T _instance;
        static object _lock = new object();

        Singleton()
        {

        }

        public static T GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}
