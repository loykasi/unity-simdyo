using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : Singleton<PoolManager>
{
    public Dictionary<Type, ObjectPool<GameObject>> _pools = new();

    public T Get<T>(T prefab)
    {
        throw new NotImplementedException();
    }
}