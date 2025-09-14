using UnityEngine;

public class ScriptFlowGraph : MonoBehaviour
{
    public ScriptFlow Flow { get; set; }

    [SerializeField] private VariableBoard _variableBoard;
    [SerializeField] private NodeBoard _nodeBoard;

    private void Awake()
    {
        _variableBoard.FlowGraph = this;
        _nodeBoard.FlowGraph = this;
    }

    public void Open(ScriptFlow flow)
    {
        Flow = flow;
        _variableBoard.Init();
        _nodeBoard.Init();
    }

    public void Close()
    {
        _nodeBoard.Close();
    }
}