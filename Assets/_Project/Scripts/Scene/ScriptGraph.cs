using UnityEngine;

public class ScriptGraph : Singleton<ScriptGraph>
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private ScriptFlowGraph _flowGraph;

    private bool _isOpen = false;

    public void TogglePanel()
    {
        SceneEntity selected = ObjectManager.Instance.SelectedObject;

        if (selected == null)
        {
            return;
        }

        _isOpen = !_isOpen;
        _panel.SetActive(_isOpen);

        if (_isOpen)
        {
            _flowGraph.Open(selected.Script);
        }
    }

    public void Close()
    {
        _isOpen = false;
        _panel.SetActive(_isOpen);
        _flowGraph.Close();
    }
}