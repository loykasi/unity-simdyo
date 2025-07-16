using UnityEngine;

public static class OperatorHandle
{
    public static object Add(object a, object b)
    {
        Debug.Log($"{a.GetType()} + {b.GetType()}");
        return null;
    }
}