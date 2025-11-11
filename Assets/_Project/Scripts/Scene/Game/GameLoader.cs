using TMPro;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    public string Url;

    private void Start()
    {
        SceneManager.Instance.ResetState();
    }

    public void Load(string url)
    {
        SaveLoadSystem.Instance.LoadFromUrl(Url);
    }
}