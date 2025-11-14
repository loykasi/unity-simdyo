using Loykas.Scripting;
using UnityEngine;

public class SceneManager : Singleton<SceneManager>, ISaveable
{
    public int SaveLoadOrder { get; set; } = 0;

    [Header("Camera")]
    public Camera SceneCamera;
    [SerializeField] private Color _backgroundColor;
    public CameraSettings CameraSettings = new();

    [Header("References")]
    public ScriptFlow GlobalScript;

    private bool _isRunning = false;

    protected override void Awake()
    {
        base.Awake();

        CameraSettings.Color = new ColorHSV(_backgroundColor);
        CameraSettings.Size = SceneCamera.orthographicSize;
        CameraSettings.Position = SceneCamera.transform.position;
    }

    private void Update()
    {
        UpdateGame();
    }

    public void Play()
    {
        SceneCamera.gameObject.SetActive(true);

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
        SceneCamera.gameObject.SetActive(false);

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
        ObjectManager.Instance.ResetState();
    }

    public void SaveData(GameData data)
    {
        ScriptSaveHandler.Save(data.Scene.GlobalScript, GlobalScript);

        data.Scene.BackgroundColor = CameraSettings.Color;
        data.Scene.CameraPosition = CameraSettings.Position;
        data.Scene.CameraSize = CameraSettings.Size;
    }

    public void LoadData(GameData data)
    {
        ScriptSaveHandler.Load(data.Scene.GlobalScript, GlobalScript);

        UpdatePosition(data.Scene.CameraPosition);
        UpdateSize(data.Scene.CameraSize);
        UpdateColor(data.Scene.BackgroundColor);
    }

    #region Camera

    public void UpdatePosition(Vector3 position)
    {
        CameraSettings.Position = new Vector3(position.x, position.y, SceneCamera.transform.position.z);
        SceneCamera.transform.position = CameraSettings.Position;
    }

    public void UpdateSize(float value)
    {
        CameraSettings.Size = value;
        SceneCamera.orthographicSize = CameraSettings.Size;
    }

    public void UpdateColor(ColorHSV color)
    {
        CameraSettings.Color = color;
        // SceneCamera.backgroundColor = color.ToUnityColor();
        BackgroundColor.Instance.SetColor(color.ToUnityColor());
    }

    #endregion
}