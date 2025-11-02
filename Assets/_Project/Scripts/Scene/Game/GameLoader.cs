using UnityEngine;

public class GameLoader : MonoBehaviour
{
    public string path;
    
    private void Start()
    {
        SceneManager.Instance.ResetState();
        SaveLoadSystem.Instance.Load(path);
    }
}