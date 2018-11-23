using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace IceCake.Core.Editor
{
    /// <summary>
    /// Editor下的协程管理器///
    /// </summary>
    public static class EditorCoroutineManager
    {
        /// <summary>
        /// Editor下的一个协程///
        /// </summary>
        private class EditorCoroutine : IEnumerator
        {
            Stack<IEnumerator> executionStack;

            public EditorCoroutine(IEnumerator enumerator)
            {
                executionStack = new Stack<IEnumerator>();
                executionStack.Push(enumerator);
            }

            public object Current
            {
                get
                {
                    return executionStack.Peek().Current;
                }
            }

            public bool MoveNext()
            {
                IEnumerator enumerator = executionStack.Peek();
                if (enumerator.MoveNext())
                {
                    object result = enumerator.Current;
                    if (result != null && result is IEnumerator)
                    {
                        executionStack.Push((IEnumerator)result);
                    }
                    return true;
                }
                else
                {
                    if (executionStack.Count > 1)
                    {
                        executionStack.Pop();
                        return true;
                    }
                }
                return false;
            }

            public void Reset()
            {
                throw new System.NotImplementedException();
            }

            public bool Find(IEnumerator enumerator)
            {
                return this.executionStack.Contains(enumerator);
            }
        }

        static List<EditorCoroutine> editorCoroutines;
        static List<IEnumerator> buffers;

        /// <summary>
        /// 开始一个协程///
        /// </summary>
        /// <param name="enumerator"></param>
        /// <returns></returns>
        public static IEnumerator Start(IEnumerator enumerator)
        {
            if (editorCoroutines == null)
                editorCoroutines = new List<EditorCoroutine>();

            if (buffers == null)
                buffers = new List<IEnumerator>();

            if (editorCoroutines.Count == 0)
            {
                EditorApplication.update += Update;
            }

            buffers.Add(enumerator);

            return enumerator;
        }

        static bool Find(IEnumerator enumerator)
        {
            foreach (var editorCoroutine in editorCoroutines)
            {
                if (editorCoroutine.Find(enumerator))
                    return true;
            }
            return false;
        }

        static void Update()
        {
            editorCoroutines.RemoveAll((coroutine) =>
            {
                return coroutine.MoveNext() == false;
            });

            if (buffers.Count > 0)
            {
                foreach (var buffer in buffers)
                {
                    if (!Find(buffer))
                    {
                        editorCoroutines.Add(new EditorCoroutine(buffer));
                    }
                    buffers.Clear();
                }
            }

            if (editorCoroutines.Count == 0)
            {
                EditorApplication.update -= Update;
            }
        }

    }
}
