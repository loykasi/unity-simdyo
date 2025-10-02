using UnityEngine;
using Loykas.Scripting;

public class ScriptGraph : Singleton<ScriptGraph>
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private ScriptFlowGraph _flowGraph;

    private bool _isOpen = false;

    public void ToggleGlobalScriptPanel()
    {
        ScriptFlow flow = EngineManager.Instance.GlobalScript;
        TogglePanel(flow);
    }

    public void TogglePanel(ScriptFlow scriptFlow = null)
    {
        if (scriptFlow == null)
        {
            return;
        }

        _isOpen = !_isOpen;
        _panel.SetActive(_isOpen);

        if (_isOpen)
        {
            _flowGraph.Open(scriptFlow);
        }
    }

    public void Close()
    {
        _isOpen = false;
        _panel.SetActive(_isOpen);
        _flowGraph.Close();
    }
}