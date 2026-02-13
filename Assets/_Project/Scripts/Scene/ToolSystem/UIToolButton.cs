using UnityEngine;

public class UIToolButton : MonoBehaviour
{
    public ToolType Type => _type;
    
    [SerializeField] private ToolType _type;
    private UIToolSelection _toolSelection;

    public void Init(UIToolSelection toolSelection)
    {
        _toolSelection = toolSelection;
    }

    public void OnClick()
    {
        _toolSelection.SelectTool(_type);
    }
}