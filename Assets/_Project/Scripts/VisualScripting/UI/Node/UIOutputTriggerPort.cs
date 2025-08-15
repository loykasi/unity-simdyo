using UnityEngine;

public class UIOutputTriggerPort: UINodePort
{
    public override NodePortEdge Edge => NodePortEdge.Right;

    public override void Init()
    {
        base.Init();

        UpdateSize();
    }

    private void UpdateSize()
    {
        Vector2 size = _label.GetPreferredValues();
        _label.rectTransform.sizeDelta = new Vector2
        (
            size.x,
            _label.rectTransform.sizeDelta.y
        );

        Rect.sizeDelta = new Vector2
        (
            30f + size.x,
            30f
        );
    }
}