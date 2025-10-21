using Loykas.Scripting;
using UnityEngine;

public class SceneManager : Singleton<SceneManager>, ISaveable
{
    public int SaveLoadOrder { get; set; } = 0;

    [Header("Camera")]
    public float EditorCameraHeight { get; set; } = 5f;
    public Camera EditorCamera;
    public Camera SceneCamera;
    [SerializeField] private GameObject _playModeCanvas;

    [Header("References")]
    public ScriptFlow GlobalScript;
    public SceneMenuController SceneController;

    private bool _isRunning = false;

    private void Update()
    {
        UpdateGame();
    }

    public void Play()
    {
        Time.timeScale = 1;
        EditorCamera.gameObject.SetActive(false);
        SceneCamera.gameObject.SetActive(true);
        _playModeCanvas.SetActive(true);

        GlobalScript.OnSceneStart();
        
        var entities = ObjectManager.Instance.SceneEntities;
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

    public void ResetState()
    {
        GlobalScript.ResetState();
        SceneController.ResetState();
        ObjectManager.Instance.ResetState();

        EditorCamera.transform.position = new Vector3(0f, 0f, EditorCamera.transform.position.z);
        EditorCamera.orthographicSize = 5f;
        EditorCameraHeight = 5f;
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