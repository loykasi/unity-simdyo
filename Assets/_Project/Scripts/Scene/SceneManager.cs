using Loykas.Scripting;
using UnityEngine;
using UnityEngine.Events;
using System.Runtime.InteropServices;

public class SceneManager : Singleton<SceneManager>, ISaveable
{
    public event UnityAction OnSceneStart;
    public event UnityAction OnSceneStop;

    public int SaveLoadOrder { get; set; } = 0;

    [Header("Camera")]
    public Camera SceneCamera;
    [SerializeField] private Color _backgroundColor;
    public CameraSettings CameraSettings = new();

    [Header("References")]
    public ScriptFlow GlobalScript;

    public bool IsRuning => _isRunning;
    private bool _isRunning = false;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void OnGameRestarted();

    [DllImport("__Internal")]
    private static extern void OnGamePaused();

    [DllImport("__Internal")]
    private static extern void OnGameResumed();
#endif

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
        Time.timeScale = 1;
        SceneCamera.gameObject.SetActive(true);

        OnSceneStart?.Invoke();

        GlobalScript.StartVS();

        var entities = ObjectManager.Instance.SceneEntities;
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].OnStart();
        }

        _isRunning = true;
    }

    public void Stop()
    {
        Time.timeScale = 0;

        SceneCamera.gameObject.SetActive(false);

        OnSceneStop?.Invoke();

        _isRunning = false;
        Physics2D.SyncTransforms();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        OnSceneStop?.Invoke();

#if UNITY_WEBGL && !UNITY_EDITOR
        OnGameRestarted();
#endif
    }

    public void Pause()
    {
        Time.timeScale = 0;

#if UNITY_WEBGL && !UNITY_EDITOR
        OnGamePaused();
#endif
    }

    public void Resume()
    {
        Time.timeScale = 1;

#if UNITY_WEBGL && !UNITY_EDITOR
        OnGameResumed();
#endif
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