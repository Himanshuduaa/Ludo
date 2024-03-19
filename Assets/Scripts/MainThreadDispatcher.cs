using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static MainThreadDispatcher instance;
    private static readonly Queue<Action> actions = new Queue<Action>();

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        var go = new GameObject("MainThreadDispatcher");
        instance = go.AddComponent<MainThreadDispatcher>();
        DontDestroyOnLoad(go);
    }

    private void Update()
    {
        lock (actions)
        {
            while (actions.Count > 0)
            {
                var action = actions.Dequeue();
                action.Invoke();
            }
        }
    }

    public static void RunOnMainThread(Action action)
    {
        lock (actions)
        {
            actions.Enqueue(action);
        }
    }
}
