using System;
using System.Collections.Generic;

public class DelegateList<T>
{
    private readonly List<Action<T>> _actions = new();

    public void Add(Action<T> action)
    {
        _actions.Add(action);
    }

    public void Remove(Action<T> action)
    {
        _actions.Remove(action);
    }

    public void Invoke(T value)
    {
        int count = _actions.Count;
        for (int i = 0; i < count; i++)
        {
            _actions[i].Invoke(value);
        }
    }

    public static DelegateList<T> operator +(DelegateList<T> left, Action<T> right)
    {
        left.Add(right);
        return left;
    }

    public static DelegateList<T> operator -(DelegateList<T> left, Action<T> right)
    {
        left.Remove(right);
        return left;
    }
}

public class DelegateList
{
    private readonly List<Action> _actions = new();

    public void Add(Action action)
    {
        _actions.Add(action);
    }

    public void Remove(Action action)
    {
        _actions.Remove(action);
    }

    public void Invoke()
    {
        int count = _actions.Count;
        for (int i = 0; i < count; i++)
        {
            _actions[i].Invoke();
        }
    }

    public static DelegateList operator +(DelegateList left, Action right)
    {
        left.Add(right);
        return left;
    }

    public static DelegateList operator -(DelegateList left, Action right)
    {
        left.Remove(right);
        return left;
    }
}