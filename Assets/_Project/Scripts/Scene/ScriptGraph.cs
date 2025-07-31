using UnityEngine;

public class ScriptGraph : Singleton<ScriptGraph>
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private VariableBoard _variableBoard;
    [SerializeField] private NodeBoard _nodeBoard;

    private bool _isOpen = false;

    public void TogglePanel()
    {
        _isOpen = !_isOpen;
        _panel.SetActive(_isOpen);

        SceneEntity selected = ObjectManager.Instance.SelectedObject;

        if (_isOpen)
        {
            _nodeBoard.SetVisualScripting(selected.VisualScripting);
            _variableBoard.Init();
        }
    }
}