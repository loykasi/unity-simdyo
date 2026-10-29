using System;
using UnityEngine;

public class EngineManager : Singleton<EngineManager>
{
    [SerializeField] private GameObject _playModeCanvas;

    [Header("Settings")]
    public float ZoomSpeed;
    public Vector2 ZoomHeighLimit;
    public float SmoothFactor;

    [Header("References")]
    [SerializeField] private GameObject _sceneCameraArea;
    [SerializeField] private GameObject _editorCanvas;
    [SerializeField] private RectTransform _referenceCanvas;
    public float CanvasScale => _referenceCanvas.localScale.x;

    private bool _isGridEnabled;

    protected override void Awake()
    {
        Time.timeScale = 0;
        base.Awake();
    }

    private void Start()
    {
        SceneManager.Instance.OnSceneStateChanged += OnSceneStateChanged;
    }

    private void OnDestroy()
    {
        if (SceneManager.Instance)
        {
            SceneManager.Instance.OnSceneStateChanged -= OnSceneStateChanged;   
        }
    }

    public void RequestPlay()
    {
        SceneManager.Instance.Play();
    }

    public void RequestStop()
    {
        SceneManager.Instance.Stop();
    }

    private void OnSceneStateChanged(SceneState state)
    {
        bool isRunning = state == SceneState.Running;

        _editorCanvas.SetActive(!isRunning);
        _sceneCameraArea.SetActive(!isRunning);
        _playModeCanvas.SetActive(isRunning);

        switch (state)
        {
            case SceneState.Running:
            {
                _isGridEnabled = GridController.Instance.GridEnabled;
                GridController.Instance.ToggleGrid(false);
                
                ObjectManager.Instance.Deselect();
                ScriptGraph.Instance.Close();
                break;
            }
            case SceneState.Stopped:
            {
                GridController.Instance.ToggleGrid(_isGridEnabled);
                break;   
            }
        }
    }

    public void NewScene()
    {
        ResetState();
    }

    public void LoadScene()
    {
        SaveLoadSystem.Instance.Load(ResetState);
    }

    public void ResetState()
    {
        CameraManager.Instance.ResetToDefault();
        SceneManager.Instance.ResetState();
    }

    public void SaveScene()
    {
        SaveLoadSystem.Instance.Save();
    }
}