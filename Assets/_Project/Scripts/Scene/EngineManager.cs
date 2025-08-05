using UnityEngine;

public class EngineManager : Singleton<EngineManager>
{
    public Camera EditorCamera;
    public Camera SceneCamera;
    [SerializeField] private GameObject _playModeCanvas;

    private bool _isRunning = false;

    private void Update()
    {
        UpdateGame();   
    }

    public void Play()
    {
        StartGame();
    }

    public void Stop()
    {
        EditorCamera.gameObject.SetActive(true);
        SceneCamera.gameObject.SetActive(false);
        _playModeCanvas.SetActive(false);

        var entities = ObjectManager.Instance.SceneEntities;
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].OnSceneStop();
        }

        _isRunning = false;
    }

    private void StartGame()
    {
        EditorCamera.gameObject.SetActive(false);
        SceneCamera.gameObject.SetActive(true);
        _playModeCanvas.SetActive(true);

        var entities = ObjectManager.Instance.SceneEntities;
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].OnSceneStart();
            entities[i].Script.StartVS();
        }

        _isRunning = true;
    }

    private void UpdateGame()
    {
        if (!_isRunning)
        {
            return;
        }

        var entities = ObjectManager.Instance.SceneEntities;
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].Script.UpdateVS();
        }
    }
}