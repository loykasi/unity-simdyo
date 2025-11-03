using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToolManagement : Singleton<ToolManagement>
{
    public UnityAction OnToolChanged;

    public ToolType CurrentTool { get; private set; }
    public bool HasTool => _tool != null;

    public float PanZoomSpeed;

    [SerializeField] private Camera _camera;
    [SerializeField] private ToolType _defaultTool;
    
    private ITool _tool;
    private ITool[] _tools = new ITool[]
    {
        new MoveTool(),
        new RotateTool(),
        new BoxTool(),
        new CircleTool(),
        new PanTool(),
        new ResizeTool()
    };
    private Dictionary<ToolType, ITool> _toolTable = new();

    protected override void Awake()
    {
        base.Awake();

        CreateToolTable();
    }

    private void Start()
    {
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
        _tool.OnUpdate();
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
}