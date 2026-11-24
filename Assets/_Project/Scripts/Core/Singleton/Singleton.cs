using UnityEngine;

[DefaultExecutionOrder(-3000)]
public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance => _instance;
    private static T _instance;
    private static bool _isQuitting;

    protected virtual void Awake()
    {
        InitializeSingleton();
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    private void OnApplicationQuit()
    {
        _isQuitting = true;
    }

    private void InitializeSingleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this as T;
        }
    }
}