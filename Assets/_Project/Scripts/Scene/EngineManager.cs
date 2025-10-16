using Loykas.Scripting;
using UnityEngine;

public class EngineManager : Singleton<EngineManager>, ISaveable
{
    [Header("Camera")]
    public float EditorCameraHeight { get; set; } = 5f;
    public Camera EditorCamera;
    public Camera SceneCamera;
    [SerializeField] private GameObject _playModeCanvas;

    [Header("Settings")]
    public Vector2 ZoomHeighLimit;

    public bool IsRunning => _isRunning;

    public int SaveLoadOrder { get; set; } = 0;

    private bool _isRunning = false;

    [Header("Global")]
    public ScriptFlow GlobalScript;

    protected override void Awake()
    {
        base.Awake();
    }

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

        GlobalScript.OnSceneStop();

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
        // first loop to init all scene object
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].OnSceneStart();
        }

        GlobalScript.StartVS();
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].OnStart();
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
        GlobalScript.UpdateVS();
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].OnUpdate();
        }
    }

    public void SaveData(GameData data)
    {
        ScriptSaveHandler.Save(data.Scene.GlobalScript, GlobalScript);
    }

    public void LoadData(GameData data)
    {
        ScriptSaveHandler.Load(data.Scene.GlobalScript, GlobalScript);
    }
}