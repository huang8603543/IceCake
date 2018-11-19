﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IceCake.Core
{
    public static class Extension
    {
        public static void ResetTransform(this GameObject go)
        {
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
        }
    }
}
