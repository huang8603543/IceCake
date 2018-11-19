using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IceCake.Core
{
    public interface IDict : IEnumerable
    {
        void AddObject(object key, object value);
        int Count { get; }
        Dictionary<object, object> OriginCollection { get; }
    }
}
