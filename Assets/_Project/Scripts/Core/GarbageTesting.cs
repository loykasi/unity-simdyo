using System;
using UnityEngine;
using UnityEngine.Pool;

public class GarbageTesting : MonoBehaviour
{
    TestPort value;
    ValueWrapper result = new();
    ObjectPool<ValueWrapper> valuePool;

    private void Awake()
    {
        valuePool = new ObjectPool<ValueWrapper>(
            createFunc: CreateItem
        );
    }

    private ValueWrapper CreateItem()
    {
        return new ValueWrapper();
    }

    private void Start()
    {
        // value = new TestPort(Get);
    }

    private void Update()
    {
        // if (value.Value != null)
        // {
        //     valuePool.Release((ValueWrapper<int>)(value.Value));
            
        // }
        // intValue.Value = 5;
        // value.Value = intValue;
        // // IValueWrapper a = value.GetValue();
        // Test(value.Value);

        ValueWrapper a = Get();
        int b = a.Value + 1;
        Test(b);
    }

    private void Test(int b)
    {
        Debug.Log(b);
    }

    private ValueWrapper Get()
    {
        using ValueWrapper intValue = valuePool.Get();
        intValue.Value = 5;
        intValue.valuePool = valuePool;

        return intValue;
    }


    public class TestPort
    {
        Func<ValueWrapper> action;
        public TestPort(Func<ValueWrapper> func)
        {
            action = func;
        }

        public ValueWrapper GetValue()
        {
            return action();
        }
    }

    public class ValueWrapper : IDisposable
    {
        public ObjectPool<ValueWrapper> valuePool;
        public int Value;

        public void Dispose()
        {
            Value = 0;
            valuePool.Release(this);
        }
    }
}