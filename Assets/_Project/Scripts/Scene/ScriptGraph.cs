using UnityEngine;

public class ScriptGraph : Singleton<ScriptGraph>
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private VariableBoard _variableBoard;
    [SerializeField] private NodeBoard _nodeBoard;

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
            _nodeBoard.SetVisualScripting(selected.Script);
            _variableBoard.Init();
        }
    }

    public void Close()
    {
        _isOpen = false;
        _panel.SetActive(_isOpen);
    }
}