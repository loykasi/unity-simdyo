using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToolManager : Singleton<ToolManager>
{
    public UnityAction OnToolChanged;

    public ToolType CurrentTool { get; private set; }
    public bool HasTool => _tool != null;

    [SerializeField] private Camera _camera;
    [SerializeField] private ToolType _defaultTool;

    [Header("Rotate Tool")]
    public float SnapRadius;

    [Header("Polygon Tool")]
    public float _baseWidth = 0.05f;
    
    // tools
    private ITool _tool;
    private ITool[] _tools = new ITool[]
    {
        new MoveTool(),
        new RotateTool(),
        new PanTool(),
        new ResizeTool(),
        new BoxTool(),
        new CircleTool(),
        new PolygonTool(),
        new TracerTool(),
    };
    private Dictionary<ToolType, ITool> _toolTable = new();

    protected override void Awake()
    {
        base.Awake();

        CreateToolTable();
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        SwitchTool(_defaultTool);
    }

    private void CreateToolTable()
    {
        foreach (ITool tool in _tools)
        {
            if (!_toolTable.ContainsKey(tool.Type))
            {
                _toolTable.Add(tool.Type, tool);
            }
        }
    }

    private void Update()
    {
        _tool?.OnUpdate();
    }

    public void SwitchTool(ToolType type)
    {
        _tool?.Disable();
        if (_toolTable.TryGetValue(type, out _tool))
        {
            CurrentTool = type;
            _tool.Enable();
        }
        OnToolChanged?.Invoke();
    }

    public ITool GetTool(ToolType type)
    {
        if (_toolTable.TryGetValue(type, out ITool tool))
        {
            return tool;
        }
        return default;
    }
}