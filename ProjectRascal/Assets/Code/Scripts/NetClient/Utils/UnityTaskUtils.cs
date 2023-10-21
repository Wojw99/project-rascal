using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.NetClient
{
    public static class UnityTaskUtils
    {
        public static IEnumerator RunTaskWithResultAsync<T>(Func<Task<T>> taskFunc, Action<T> resultCallback)
        {
            Task<T> task = taskFunc.Invoke();
            yield return WaitForTask(task);

            if (task.IsCompletedSuccessfully)
            {
                resultCallback.Invoke(task.Result);
            }
        }

        public static IEnumerator RunTaskAsync(Func<Task> taskFunc)
        {
            Task task = taskFunc.Invoke();
            yield return WaitForTask(task);
        }

        public static IEnumerator WaitForTask(Task task)
        {
            while (!task.IsCompleted)
                yield return null;
        }
    }
}
