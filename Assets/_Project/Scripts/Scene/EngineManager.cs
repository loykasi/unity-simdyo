using UnityEngine;

public class EngineManager : Singleton<EngineManager>
{
    public Camera EditorCamera => SceneManager.Instance.EditorCamera;
    public Camera SceneCamera => SceneManager.Instance.SceneCamera;

    [Header("Settings")]
    public Vector2 ZoomHeighLimit;

    [Header("References")]
    [SerializeField] private RectTransform _referenceCanvas;
    public float CanvasScale => _referenceCanvas.localScale.x;

    protected override void Awake()
    {
        base.Awake();
    }

    public void NewScene()
    {
        SceneManager.Instance.ResetState();
    }

    public void LoadScene()
    {
        SceneManager.Instance.ResetState();
        SaveLoadSystem.Instance.Load();
    }
    
    public void SaveScene()
    {
        SaveLoadSystem.Instance.Save();
    }
}