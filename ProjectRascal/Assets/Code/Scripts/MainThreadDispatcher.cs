/*using System.Collections;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static MainThreadDispatcher instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }*//**//*
    }

    public static void RunOnMainThread(System.Action action)
    {
        if (instance != null)
        {
            instance.StartCoroutine(instance.RunOnMainThreadInternal(action));
        }
    }

    private IEnumerator RunOnMainThreadInternal(System.Action action)
    {
        yield return null; // Wait for one frame
        action.Invoke();
    }
}

*/