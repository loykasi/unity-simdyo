using Loykas.Scripting;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public enum SceneState
{
    Stopped,
    Running,
    Paused,
}

public class SceneManager : Singleton<SceneManager>, ISaveable
{
    public SceneState State { get; private set; } = SceneState.Stopped;
    public bool IsRunning => State == SceneState.Running;

    public event Action<SceneState> OnSceneStateChanged = delegate { };
    public event Action OnSceneStart = delegate { };
    public event Action OnSceneStop = delegate { };

    public int SaveLoadOrder { get; set; } = 0;

    public float TimeScale = 1f;

    [Header("References")]
    public ScriptFlow GlobalScript;

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
    }

    private void Update()
    {
        UpdateGame();
    }

    public void Play()
    {
        State = SceneState.Running;

        Time.timeScale = TimeScale;
        CameraManager.Instance.EnterPlay();
        OnSceneStart.Invoke();
        GlobalScript.StartVS();
        var entities = ObjectManager.Instance.SceneEntities;
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].OnStart();
        }
        OnSceneStateChanged.Invoke(State);
    }

    public void Stop()
    {
        OnSceneStop?.Invoke();
        Time.timeScale = 0;
        CameraManager.Instance.ExitPlay();
        Physics2D.SyncTransforms();
        
        State = SceneState.Stopped;
        OnSceneStateChanged.Invoke(State);
    }

    public void Restart()
    {
        OnSceneStop?.Invoke();
        
        CameraManager.Instance.RestartPlay();
        Time.timeScale = TimeScale;
        OnSceneStart.Invoke();
        GlobalScript.StartVS();

        var entities = ObjectManager.Instance.SceneEntities;
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].OnStart();
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        OnGameRestarted();
#endif
    }

    public void Pause()
    {
        State = SceneState.Paused;
        Time.timeScale = 0;

#if UNITY_WEBGL && !UNITY_EDITOR
        OnGamePaused();
#endif
    }

    public void Resume()
    {
        State = SceneState.Running;
        Time.timeScale = TimeScale;

#if UNITY_WEBGL && !UNITY_EDITOR
        OnGameResumed();
#endif
    }
    
    private void UpdateGame()
    {
        if (!IsRunning)
        {
            return;
        }

        var entities = ObjectManager.Instance.SceneEntities;
        GlobalScript.UpdateVS();

        // for (int i = 0; i < entities.Count; i++)
        for (int i = entities.Count - 1; i >= 0; i--)
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

        CameraSettings settings = CameraManager.Instance.CameraSettings;
        data.Scene.BackgroundColor = settings.Color;
        data.Scene.CameraPosition = settings.Position;
        data.Scene.CameraSize = settings.Size;
    }

    public void LoadData(GameData data)
    {
        ScriptSaveHandler.Load(data.Scene.GlobalScript, GlobalScript);

        CameraManager.Instance.UpdatePosition(data.Scene.CameraPosition);
        CameraManager.Instance.UpdateSize(data.Scene.CameraSize);
        CameraManager.Instance.UpdateColor(data.Scene.BackgroundColor);
    }
}