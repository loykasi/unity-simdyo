using System.Diagnostics;
using UnityEngine;

public static class KDebug
{
    [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
    public static void Log(object message, Object context = null)
    {
        UnityEngine.Debug.Log(message, context);
    }
}