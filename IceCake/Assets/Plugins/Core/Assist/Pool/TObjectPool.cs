using System;
using System.Collections.Generic;
using UnityEngine;

namespace IceCake.Core
{
    public class TObjectPool<T> where T : new()
    {
        Stack<T> stack;

        Func<T> actionAlloc;
        Action<T> actionFree;
        Action<T> actionDestory;

        public TObjectPool(Func<T> actionAlloc, Action<T> actionFree, Action<T> actionDestory)
        {
            stack = new Stack<T>();

            this.actionAlloc = actionAlloc;
            this.actionFree = actionFree;
            this.actionDestory = actionDestory;
        }

        public T Alloc()
        {
            if (stack.Count == 0)
            {
                return UtilTool.SafeExcute(actionAlloc);
            }
            else
            {
                return stack.Pop();
            }
        }

        public void Free(T element)
        {
            if (stack.Count > 0 && object.ReferenceEquals(stack.Peek(), element))
            {
                Debug.Log("Internal error. Trying to destroy object that is already released to pool.");
            }
            UtilTool.SafeExcute(actionFree, element);
            stack.Push(element);
        }

        public void Destory()
        {
            if (stack == null)
                return;
            foreach (var element in stack)
            {
                UtilTool.SafeExcute(actionDestory, element);
            }
            stack.Clear();
        }
    }
}
