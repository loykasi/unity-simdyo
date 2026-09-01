using System;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    private readonly Dictionary<Type, object> _services = new();

    public void Register(object service)
    {
        Type type = service.GetType();
        if (!_services.ContainsKey(type))
        {
            _services.Add(type, service);
            KDebug.Log($"[UIManager] Register {type}");
        }
    }

    public T Get<T>() where T : class
    {
        if (_services.TryGetValue(typeof(T), out object service))
        {
            return (T) service;
        }
        return null;
    }
}