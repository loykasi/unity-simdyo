using UnityEngine;

public class EngineManager : Singleton<EngineManager>
{
    public float EditorCameraHeight { get; set; } = 5f;
    public Vector3 EditorCameraPostiion { get; set; } = Vector3.zero;
    public Camera EditorCamera;
    public Camera SceneCamera => SceneManager.Instance.SceneCamera;
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

    public void Play()
    {        
        _editorCanvas.SetActive(false);
        _sceneCameraArea.SetActive(false);
        
        if (GridController.Instance != null)
        {
            _isGridEnabled = GridController.Instance.GridEnabled;
            GridController.Instance.ToggleGrid(false);
        }
        
        ObjectManager.Instance.Deselect();
        ScriptGraph.Instance.Close();

        _playModeCanvas.SetActive(true);
        EditorCamera.gameObject.SetActive(false);
        SceneManager.Instance.Play();
    }

    public void Stop()
    {
        _editorCanvas.SetActive(true);
        _sceneCameraArea.SetActive(true);

        _playModeCanvas.SetActive(false);
        EditorCamera.gameObject.SetActive(true);
        SceneManager.Instance.Stop();

        GridController.Instance.ToggleGrid(_isGridEnabled);
    }

    public void NewScene()
    {
        ResetState();
    }

    public void LoadScene()
    {
        SaveLoadSystem.Instance.Load(ResetState);
    }

    public void SaveScene()
    {
        SaveLoadSystem.Instance.Save();
    }

    public void ResetState()
    {
        EditorCamera.transform.position = new Vector3(0f, 0f, EditorCamera.transform.position.z);
        EditorCamera.orthographicSize = 5f;
        EditorCameraHeight = 5f;
        SceneManager.Instance.ResetState();
    }
}