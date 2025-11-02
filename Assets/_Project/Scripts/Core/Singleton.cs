using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance
    {
        get
        {
            if (_isQuitting)
            {
                return null;
            }
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();
                if (_instance == null)
                {
                    GameObject gameObject = new GameObject(nameof(T));
                    _instance = gameObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    // [Header("Singleton")]
    public bool isAutoUnparentOnAwake;
    public bool isNotDestroyOnLoad;

    private static T _instance;
    private static bool _isQuitting;

    protected virtual void Awake()
    {
        InitializeSingleton();
    }

    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    // static void OnBeforeSceneLoad()
    // {
    //     _isQuitting = false;
    // }

    private void OnApplicationQuit()
    {
        _isQuitting = true;
    }

    private void InitializeSingleton()
    {
        if (_instance != null &&_instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this as T;
            if (isAutoUnparentOnAwake)
            {
                transform.SetParent(null);
            }
            if (isNotDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }
        }
    }
}