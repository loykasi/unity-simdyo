using UnityEngine;

public class EngineManager : Singleton<EngineManager>
{
    public Camera EditorCamera;
    public float EditorCameraHeight { get; set; } = 5f;
    public Camera SceneCamera;
    [SerializeField] private GameObject _playModeCanvas;

    [Header("Settings")]
    public Vector2 ZoomHeighLimit;

    public bool IsRunning => _isRunning;
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
        Time.timeScale = 0;
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
        Time.timeScale = 1;
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