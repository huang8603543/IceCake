using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif

namespace IceCake.Core
{
    public abstract class DataService
    {
        SQLiteConnection connection;

        public DataService(string databaseName)
        {
#if UNITY_EDITOR
            var daPath = string.Format(@"Assets/StreamingAssets/DB/{0}", databaseName);
#else
#endif
            //connection = new SQLiteConnection(daPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        }

        public abstract void CreateDB<T>();


    }
}
