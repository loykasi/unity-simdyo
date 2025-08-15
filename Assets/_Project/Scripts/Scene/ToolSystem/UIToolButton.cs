using UnityEngine;

public class UIToolButton : MonoBehaviour
{
    public ToolType Type => _type;
    
    [SerializeField] private UIToolSelection _toolSelection;
    [SerializeField] private ToolType _type;

    public void OnClick()
    {
        _toolSelection.SelectTool(_type);
    }
}