using UnityEngine;

public class TooltipSystem : Singleton<TooltipSystem>
{
    [SerializeField] private Tooltip _tooltip;

    public void Show(string content)
    {
        _tooltip.SetContent(content);
        _tooltip.Toggle(true);
    }

    public void Hide()
    {
        _tooltip.Toggle(false);
    }
}