using TMPro;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private string _projectUrl;
    [SerializeField] private bool _shouldLoadOnStart;

    private void Start()
    {
        if (_shouldLoadOnStart)
        {
            SaveLoadSystem.Instance.LoadFromUrl(_projectUrl, OnLoadSucessful, OnLoadFailed);
        }
    }

    public void Load(string url)
    {
        SaveLoadSystem.Instance.LoadFromUrl(url, OnLoadSucessful, OnLoadFailed);
    }

    private void OnLoadSucessful()
    {
        LoadingScreen.Instance.Toggle(false);
    }

    private void OnLoadFailed()
    {
        LoadingScreen.Instance.ToggleFailureScreen(true);
    }
}