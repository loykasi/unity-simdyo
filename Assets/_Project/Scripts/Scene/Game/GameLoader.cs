using TMPro;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private string _projectUrl;
    [SerializeField] private bool _shouldLoadOnStart;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void OnGameLoaded();
#endif


    private void Start()
    {
        if (_shouldLoadOnStart)
        {
            Load(_projectUrl);
        }
    }

    public void Load(string url)
    {
        SaveLoadSystem.Instance.LoadFromUrl(url, OnLoadSucessful, OnLoadFailed);
    }

    private void OnLoadSucessful()
    {
        LoadingScreen.Instance.Toggle(false);

        #if UNITY_WEBGL && !UNITY_EDITOR
            OnGameLoaded();
        #endif

        SceneManager.Instance.Play();
    }

    private void OnLoadFailed()
    {
        LoadingScreen.Instance.ToggleFailureScreen(true);
    }
}